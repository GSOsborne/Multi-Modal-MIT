using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pneumatics : MonoBehaviour
{
    public SnapRestrictions restrictions;
    public GameObject interactableZoneObject;
    public bool isTight;
    //public GameObject snapZoneObject;

    private void Start()
    {
        isTight = interactableZoneObject.activeSelf;
    }

    public void CycleRestrictions()
    {
        if (restrictions.gameObject.activeSelf)
        {
            restrictions.removable = !restrictions.removable;
            interactableZoneObject.SetActive(!interactableZoneObject.activeSelf);
            isTight = !isTight;
            //snapZoneObject.SetActive(!snapZoneObject.activeSelf);
        }
        else
        {
            Debug.Log("Uh, you're trying to tighten a chuck that isn't even there, you sure you're ok?");
        }
    }

}
