using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Spinning;
using static ClampMovement;
using static FailureState;

public class DrillBit : MonoBehaviour
{
    public Transform entryPoint;
    public float drillThickness;
    Drillable drill;

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
                    drill.NewHole(drillThickness, entryPoint.position);
                }
                else
                {
                    Debug.Log("YOUR DRILL IS NOW BROKEN.");
                    FailureState.Instance.SystemFailure("Your drill wasn't spinning when it touched the drillable piece.");
                }
            }
            else
            {
                FailureState.Instance.SystemFailure("Your block wasn't clamped when you tried to drill it.");
            }

        }
        else if(other.gameObject.CompareTag("Untagged"))
        {
            Debug.Log("You should not be drilling that! Stop it now!");
            FailureState.Instance.SystemFailure("Your drill touched something it shouldn't. Did you forget parallels?");
        }
    }


    

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Drillable"))
        {
            if (Spinning.Instance.isSpinning)
            {
                drill.UpdateHoleDepth(entryPoint.position);
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
            other.gameObject.GetComponent<Drillable>().DisplayHoleData();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
