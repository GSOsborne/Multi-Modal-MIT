using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClampMovement : MonoBehaviour
{
    public static ClampMovement Instance;

    public enum ClampStatus { Loose, NeedsAWack, NeedsATestJiggle, FullySecure};
    public ClampStatus currentClampStatus;

    public MouseRotate viseRotator;
    public float rotationMultiplier;
    Vector3 zeroPosition;

    public Transform farPoint, nearPoint;
    public float yThickness;
    public bool isClamped;
    bool justExitedClamp;

    public bool needsAWack;
    public bool needsATestJiggle;
    public bool fullySecure;

    SnapRestrictions blockSnapRestrictions;

    float originalMouseRotateAngleBounds;

    public GameObject hammerWackInteractableZone;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        zeroPosition = transform.localPosition;
        isClamped = false;
        originalMouseRotateAngleBounds = viseRotator.angleBounds;
    }

    // Update is called once per frame
    void Update()
    {
        if((farPoint.position - nearPoint.position).magnitude < yThickness)
        {
            //Debug.Log("We are clamped now.");
            isClamped = true;
            hammerWackInteractableZone.SetActive(true);
            viseRotator.angleBounds = viseRotator.storedRotation;

            

            if (fullySecure)
            {
                if (blockSnapRestrictions != null)
                {
                    blockSnapRestrictions.removable = false;
                }
            }
            else if (!needsATestJiggle)
            {
                currentClampStatus = ClampStatus.NeedsAWack;
                needsAWack = true;
            }

        }
        else
        {
            if(isClamped == true)
            {
                StopAllCoroutines();
                StartCoroutine(JustExitedClamp());
            }
            isClamped = false;
            needsATestJiggle = false;
            fullySecure = false;
            needsAWack = false;
            currentClampStatus = ClampStatus.Loose;
            hammerWackInteractableZone.SetActive(false);
            viseRotator.angleBounds = originalMouseRotateAngleBounds;
            if (blockSnapRestrictions != null)
            {
                blockSnapRestrictions.removable = true;
            }

        }

        Vector3 newPos = new Vector3(viseRotator.storedRotation * rotationMultiplier, 0f, 0f);
        //Debug.Log(newPos);
        if((zeroPosition+newPos).x > transform.localPosition.x && isClamped)
        {
            //Debug.Log("Tried to move forwards while clamped. Didn't.");
        }
        else
        {
            transform.localPosition = zeroPosition + newPos;
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Drillable"))
        {
            blockSnapRestrictions = other.transform.parent.gameObject.GetComponent<SnapRestrictions>();
            //Debug.Log("Trying to push the drillable object.");
            yThickness = other.GetComponent<Drillable>().yDimensions;
            if (!isClamped && !justExitedClamp)
            {
                //Debug.Log("We aren't clamped, so we're gonna try to push.");
                other.transform.parent.position += Vector3.forward * Time.deltaTime;
            }
        }
    }

    IEnumerator JustExitedClamp()
    {
        justExitedClamp = true;
        yield return new WaitForSeconds(.1f);
        justExitedClamp = false;
    }

    public void DoTestJiggle()
    {
        float randomFloat = Random.Range(0f, 2f);
        //Debug.Log("Random float was: " + randomFloat);
        bool isSecure = randomFloat > 1f;
        if (isSecure)
        {
            fullySecure = true;
            needsATestJiggle = false;
            needsAWack = false;
            currentClampStatus = ClampStatus.FullySecure;
        }
        else
        {
            fullySecure = false;
            needsATestJiggle = false;
            needsAWack = true;
            currentClampStatus = ClampStatus.NeedsAWack;

        }
    }

    public void DoHammerWack()
    {
        currentClampStatus = ClampStatus.NeedsATestJiggle;
        needsAWack = false;
        needsATestJiggle = true;
    }

}
