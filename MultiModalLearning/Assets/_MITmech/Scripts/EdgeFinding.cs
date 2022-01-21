using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeFinding : MonoBehaviour
{
    public Transform parentTrans;
    public Transform notSpinning;
    public Transform spinningSource;
    public float maximumOffset;
    public bool isPopped;
    public float sensitivityMultiplier;
    public float popOutDistance;
    Vector3 centerPos;

    public RemoveFromSnap removeFromSnap;

    public GameObject resetButton;


    // Start is called before the first frame update
    void Start()
    {
        isPopped = false;
        centerPos = transform.localPosition;
        Debug.Log("Center position is: " + centerPos);
        ResetPop();
    }

    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("Woah we're inside " + other.gameObject.name);
        if (other.CompareTag("Drillable"))
        {
            Debug.Log("transform x is: " + transform.localPosition.x + " while we're subrtracting: " + Time.deltaTime * sensitivityMultiplier);
            float newX = transform.localPosition.x - Time.deltaTime * sensitivityMultiplier;

            if(newX < 0f)
            {
                Debug.Log("We're gonna pop out.");
                newX = 0f;
                if (!isPopped)
                {
                    PopOut();
                }

            }
            else
            {
                //Debug.Log("New X is: " + newX);
                transform.localPosition = new Vector3(newX, transform.localPosition.y, transform.localPosition.z);
                Debug.Log("Currently interacting with drillable object! New offset is: " + newX);
            }

        }
    }

    public void PopOut()
    {

        Debug.Log("Yeah, that's close enough, we're popping out.");
        /*
        // briefly resetting the spinning so that we're in more control of what direction the pop out is going
        Vector3 eulerRotation = spinningSource.rotation.eulerAngles;
        spinningSource.rotation = Quaternion.Euler(eulerRotation.x, eulerRotation.y, 0);

        //set the local position so the edgefinder pops out to the side a little bit
        transform.localPosition = new Vector3(centerPos.x + maximumOffset / 2, centerPos.y, centerPos.z);
        */
        //set the parent to an object that isn't spinning.
        transform.localPosition = centerPos + new Vector3(popOutDistance, 0f, 0f);
        isPopped = true;
        transform.SetParent(notSpinning, true);
        //transform.localPosition += new Vector3(popOutDistance, 0f, 0f);
        resetButton.SetActive(true);

    }

    public void ResetPop()
    {
        resetButton.SetActive(false);
        transform.SetParent(parentTrans, true);
        transform.localPosition = new Vector3(centerPos.x + maximumOffset, centerPos.y, centerPos.z);
        isPopped = false;
    }

    private void Update()
    {
        if (removeFromSnap.isRemoving)
        {
            ResetPop();
        }
    }

}
