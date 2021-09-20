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

    public GameObject holeSprite;

    public List<HoleInfo> drilledHoles;

    public Text displayText;

    int holeCount;

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

        Vector3 zeroedReferencePos = contactLocation - referenceZero.position;
        zeroedReferencePos = new Vector3(zeroedReferencePos.x, 0, zeroedReferencePos.z);
        Debug.Log("zeroed out contact location is: " + zeroedReferencePos);

        //check if we've drilled this position before.
        bool newHole = true;
        foreach (HoleInfo holeInfo in drilledHoles)
        {
            bool sameHole = (zeroedReferencePos - holeInfo.holeStartPos).magnitude < .03f;
            if (sameHole)
            {
                Debug.Log("Drilling into the same hole!");
                holeCount = holeInfo.holeNumber;
                newHole = false;
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
            holeSpriteObject.transform.position = referenceZero.position + holeData.holeStartPos + Vector3.up * 0.1f;
            holeSpriteObject.transform.localScale *= holeData.holeThickness;
            
            drilledHoles.Add(holeData);
            //Debug.Log(drilledHoles[holeCount]);
        }
    }

    public void UpdateHoleDepth(Vector3 depthLocation)
    {
        Vector3 zeroedDepth = depthLocation - referenceZero.position;
        //Debug.Log("Zeroed Depth location is " + zeroedDepth);

        Vector3 sentHolePosition = new Vector3(zeroedDepth.x, 0f, zeroedDepth.z);
        //Debug.Log("Sent Hole Position zeroed out is " + sentHolePosition + " while hole #" + holeCount + " has starting position of " + drilledHoles[holeCount].holeStartPos);
        bool movedLaterally = (sentHolePosition - drilledHoles[holeCount].holeStartPos).magnitude > .03f;
        if(movedLaterally)
        {
            Debug.Log("moved laterally with magnitude of " + (sentHolePosition - drilledHoles[holeCount].holeStartPos).magnitude);
            FailureState.Instance.SystemFailure("You moved the drill laterally while still inside the block and now your drill is broken.");
        }
        else if (drilledHoles[holeCount].holeDepth > zeroedDepth.y)
        {
            drilledHoles[holeCount].holeDepth = zeroedDepth.y;
            Debug.Log("Hole #" + holeCount + " depth is now " + drilledHoles[holeCount].holeDepth);
        }
    }

    public void DisplayHoleData()
    {
        displayText.text = "";
        foreach (HoleInfo holeInfo in drilledHoles)
        {
            displayText.text += "Hole #" + holeInfo.holeNumber + " at coordinates " + holeInfo.holeStartPos + " with thickness " + holeInfo.holeThickness + " with a depth of " + holeInfo.holeDepth + "\n";
            Debug.Log("Hole #" + holeInfo.holeNumber + " at coordinates " + holeInfo.holeStartPos + " with thickness " + holeInfo.holeThickness + " with a depth of " + holeInfo.holeDepth);
        }
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
