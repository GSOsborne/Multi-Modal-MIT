using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Spinning;
using static ClampMovement;
using static FailureState;

public class DrillBit : MonoBehaviour
{
    public Transform rotatingWorldZero;
    public Transform entryPoint;
    public float drillThickness;
    Drillable drill;

    public MouseRotate zRotator;
    float storedAngleBounds;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Drillable"))
        {
            if (ClampMovement.Instance.isClamped)
            {
                if (Spinning.Instance.isSpinning)
                {
                    drill = other.gameObject.GetComponent<Drillable>();
                    if(Spinning.Instance.spinSpeed < drill.minSpinSpeed)
                    {
                        FailureState.Instance.SystemFailure("Your drill was spinning too slowly!");
                    }
                    else if(Spinning.Instance.spinSpeed > drill.maxSpinSpeed)
                    {
                        FailureState.Instance.SystemFailure("Your drill was spinning to quickly!");
                    }
                    else
                    {
                        drill.NewHole(drillThickness, rotatingWorldZero.InverseTransformPoint(entryPoint.position));
                    }

                }
                else
                {
                    Debug.Log("Locking z rotation");
                    storedAngleBounds = zRotator.angleBounds;
                    zRotator.angleBounds = Mathf.Abs(zRotator.storedRotation);
                    //Debug.Log("YOUR DRILL IS NOW BROKEN.");
                    //FailureState.Instance.SystemFailure("Your drill wasn't spinning when it touched the drillable piece.");
                }
            }
            else
            {
                FailureState.Instance.SystemFailure("Your block wasn't clamped when you touched it with your dril.");
            }

        }
        else if(other.gameObject.CompareTag("Untagged"))
        {
            if (Spinning.Instance.isSpinning)
            {
                Debug.Log("You should not be drilling that! Stop it now!");
                FailureState.Instance.SystemFailure("Your spinning drill touched something it shouldn't. Did you forget parallels?");
            }
            else
            {
                Debug.Log("Locking z rotation");
                storedAngleBounds = zRotator.angleBounds;
                zRotator.angleBounds = Mathf.Abs(zRotator.storedRotation);
                FailureState.Instance.DisplayWarning("Your drill bit hit something it shouldn't have, be careful.");
            }

        }
    }


    

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Drillable"))
        {
            if (Spinning.Instance.isSpinning)
            {
                drill.UpdateHoleDepth(rotatingWorldZero.InverseTransformPoint(entryPoint.position));
            }
            else
            {
                Debug.Log("STILL BROKEN!");
            }

        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Drillable"))
        {
            if (Spinning.Instance.isSpinning)
            {
                other.gameObject.GetComponent<Drillable>().DisplayHoleData();
            }
            else
            {
                Debug.Log("Resetting angle bounds.");
                zRotator.angleBounds = storedAngleBounds;
            }

        }
        else if (other.gameObject.CompareTag("Untagged"))
        {
            Debug.Log("Resetting angle bounds.");
            zRotator.angleBounds = storedAngleBounds;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
