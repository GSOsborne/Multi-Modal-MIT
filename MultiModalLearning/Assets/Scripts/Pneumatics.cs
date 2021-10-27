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
                Debug.Log("isTight has a value of: " + isTight);
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

    public void Loosen()
    {

        restrictions.removable = true;
        interactableZoneObject.SetActive(false);
        isTight = false;
        Debug.Log("Loose, with an isTight value of: " + isTight);
        if (!isTight && areWeSpinning)
        {
            FailureState.Instance.SystemFailure("You loosened the tool while the machine was still spinning.");
        }
    }

    public void Tighten()
    {
        restrictions.removable = false;
        interactableZoneObject.SetActive(true);
        isTight = true;
        Debug.Log("Tight, with an isTight value of: " + isTight);
        if (!restrictions.gameObject.activeSelf)
        {
            FailureState.Instance.SystemFailure("You turned on the pneumatics without any tool to grab.");
        }
    }

}
