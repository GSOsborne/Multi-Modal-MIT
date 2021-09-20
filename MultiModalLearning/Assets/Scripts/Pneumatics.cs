using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FailureState;
using static Spinning;

public class Pneumatics : MonoBehaviour
{
    public SnapRestrictions restrictions;
    public GameObject interactableZoneObject;
    public bool isTight;
    bool areWeSpinning;
    //public GameObject snapZoneObject;

    private void Start()
    {
        isTight = interactableZoneObject.activeSelf;
        Spinning.SpinningEvent += StartedSpinning;
    }

    void StartedSpinning(bool isSpinning)
    {
        areWeSpinning = isSpinning;
        if (isSpinning)
        {
            if (!isTight)
            {
                FailureState.Instance.SystemFailure("You started spinning before the pneumatics had been used to hold the tool in place.");
            }
        }
    }

    public void CycleRestrictions()
    {
        if (restrictions.gameObject.activeSelf)
        {
            restrictions.removable = !restrictions.removable;
            interactableZoneObject.SetActive(!interactableZoneObject.activeSelf);
            isTight = !isTight;
            if(!isTight && areWeSpinning)
            {
                FailureState.Instance.SystemFailure("You loosened the tool while the machine was still spinning.");
            }
        }
        else
        {
            FailureState.Instance.SystemFailure("You turned on the pneumatics without any tool to grab.");
        }
    }

}
