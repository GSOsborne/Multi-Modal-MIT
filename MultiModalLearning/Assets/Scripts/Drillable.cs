using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoleInfo
{
    public int holeNumber;
    public Vector3 holeStartPos;
    public float holeDepth;
    public float holeThickness;
}

public class Drillable : MonoBehaviour
{
    public Transform referenceZero;

    public Transform rotatingWorldZero;

    public GameObject holeSprite;

    public List<HoleInfo> drilledHoles;

    public Text displayText;

    int holeCount;

    public float lowestHoleDepth;

    public float currentDrillHeat;
    public float maximumDrillHeatValue;

    public float xDimensions, yDimensions;

    public float minSpinSpeed, maxSpinSpeed;

    // Start is called before the first frame update
    void Start()
    {
        drilledHoles = new List<HoleInfo>();
        holeCount = -1;
        drilledHoles.Clear();
    }

    public void NewHole(float sentThickness, Vector3 contactLocation)
    {
        Debug.Log("sent contact location was " + contactLocation);

        Vector3 zeroedReferencePos = contactLocation - rotatingWorldZero.InverseTransformPoint(referenceZero.position);
        zeroedReferencePos = new Vector3(zeroedReferencePos.x, 0, zeroedReferencePos.z);
        Debug.Log("zeroed out contact location is: " + zeroedReferencePos);

        bool tooCloseToTheEdge = (zeroedReferencePos.x + sentThickness / 2f) > xDimensions || 
            (zeroedReferencePos.x - sentThickness / 2f) < 0f || 
            (zeroedReferencePos.z + sentThickness / 2f) > yDimensions || 
            (zeroedReferencePos.z - sentThickness / 2f) < 0;
        if (tooCloseToTheEdge)
        {
            FailureState.Instance.SystemFailure("You drilled too close to the edge.");
        }

        //check if we've drilled this position before.
        bool newHole = true;
        foreach (HoleInfo holeInfo in drilledHoles)
        {
            float distanceFromHole = (zeroedReferencePos - holeInfo.holeStartPos).magnitude;
            bool sameHole = distanceFromHole < holeInfo.holeThickness/5;
            bool tooCloseToOtherHole = distanceFromHole > holeInfo.holeThickness / 5 && distanceFromHole < (holeInfo.holeThickness / 2 + sentThickness / 2);
            if (sameHole)
            {
                Debug.Log("Drilling into the same hole!");
                holeCount = holeInfo.holeNumber;
                newHole = false;
            }
            else if (tooCloseToOtherHole)
            {
                Debug.Log("You're too close to an existing hole and that's dangerous.");
                FailureState.Instance.SystemFailure("You tried to drill too close to an already existing hole.");
            }
        }

        if (newHole)
        {
            holeCount = drilledHoles.Count;
            Debug.Log("Starting new hole #" + holeCount);
            HoleInfo holeData = new HoleInfo();
            holeData.holeThickness = sentThickness;
            holeData.holeStartPos = zeroedReferencePos;
            holeData.holeNumber = holeCount;
            Debug.Log("Hole #" + holeData.holeNumber + " has a thickness of " + holeData.holeThickness + " and a start location of " + holeData.holeStartPos);
            GameObject holeSpriteObject = Instantiate(holeSprite);
            holeSpriteObject.transform.parent = transform;
            holeSpriteObject.transform.position = rotatingWorldZero.InverseTransformPoint(referenceZero.position) + holeData.holeStartPos + Vector3.up * 0.1f;
            holeSpriteObject.transform.localScale *= holeData.holeThickness;
            
            drilledHoles.Add(holeData);
            //Debug.Log(drilledHoles[holeCount]);
        }
    }

    public void UpdateHoleDepth(Vector3 depthLocation)
    {
        Vector3 zeroedDepth = depthLocation - rotatingWorldZero.InverseTransformPoint(referenceZero.position);
        //Debug.Log("Zeroed Depth location is " + zeroedDepth);

        Vector3 sentHolePosition = new Vector3(zeroedDepth.x, 0f, zeroedDepth.z);
        //Debug.Log("Sent Hole Position zeroed out is " + sentHolePosition + " while hole #" + holeCount + " has starting position of " + drilledHoles[holeCount].holeStartPos);
        bool movedLaterally = (sentHolePosition - drilledHoles[holeCount].holeStartPos).magnitude > drilledHoles[holeCount].holeThickness/5;
        if(movedLaterally)
        {
            Debug.Log("moved laterally with magnitude of " + (sentHolePosition - drilledHoles[holeCount].holeStartPos).magnitude);
            FailureState.Instance.SystemFailure("You moved the drill laterally while still inside the block and now your drill is broken.");
        }
        else if (drilledHoles[holeCount].holeDepth > zeroedDepth.y)
        {
            drilledHoles[holeCount].holeDepth = Mathf.Max(zeroedDepth.y, lowestHoleDepth);
            Debug.Log("Hole #" + holeCount + " depth is now " + drilledHoles[holeCount].holeDepth);

            //This should also add heat to the drill
            currentDrillHeat += Time.deltaTime * 5;
        }
    }

    public void DisplayHoleData()
    {
        displayText.text = "";
        foreach (HoleInfo holeInfo in drilledHoles)
        {
            displayText.text += "Hole #" + holeInfo.holeNumber + " at coordinates (" +
                holeInfo.holeStartPos.x + ", " + holeInfo.holeStartPos.z + ") with thickness " + 
                holeInfo.holeThickness + " with a depth of " + holeInfo.holeDepth + "\n";
            Debug.Log("Hole #" + holeInfo.holeNumber + " at coordinates (" +
                holeInfo.holeStartPos.x + ", " + holeInfo.holeStartPos.z + ") with thickness " +
                holeInfo.holeThickness + " with a depth of " + holeInfo.holeDepth);
        }
    }



    // Update is called once per frame
    void Update()
    {
        if(currentDrillHeat > maximumDrillHeatValue)
        {
            FailureState.Instance.SystemFailure("Your drill overheated. Try pecking next time.");
        }
        currentDrillHeat = Mathf.Max(currentDrillHeat - Time.deltaTime, 0);
    }
}
