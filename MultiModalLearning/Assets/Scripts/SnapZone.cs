using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static SnapZoneManager;



public class SnapZone : MonoBehaviour
{
    public SnapObjectType[] snapZoneInventory;
    public GameObject[] inventoryObjectModel;
    public Dictionary<SnapObjectType, GameObject> inventoryObjectModelDictionary = new Dictionary<SnapObjectType, GameObject>();

    public SnapObjectType currentlySnapped;

    MeshRenderer rend;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<MeshRenderer>();
        rend.enabled = false;
        SnapZoneManager.Instance.Hovering += OnHover;
        int i = 0;
        foreach (SnapObjectType snapObject in snapZoneInventory)
        {
            //associating the arrays in the editor. make sure they're referenced in the same order.
            inventoryObjectModelDictionary.Add(snapObject, inventoryObjectModel[i]);
            Debug.Log(snapObject + " is connected to " + inventoryObjectModelDictionary[snapObject]);

            //and turn them all off at the beginning.
            inventoryObjectModelDictionary[snapObject].SetActive(false);
            i++;
        }
        currentlySnapped = SnapObjectType.Empty;
        SnapIn(SnapObjectType.Empty);
    }

    public bool CheckIfInInventory(SnapObjectType potentialSnapObject)
    {
        bool snapAvailable = false;
        foreach (SnapObjectType objectType in snapZoneInventory)
        {
            if (potentialSnapObject == objectType)
            {
                snapAvailable = true;
                SnapIn(potentialSnapObject);
                SnapZoneManager.Instance.FireSnapReplacement(potentialSnapObject);
            }
        }
        return snapAvailable;
    }

    void SnapIn(SnapObjectType recievedSnapObject)
    {
        inventoryObjectModelDictionary[currentlySnapped].SetActive(false);
        SnapZoneManager.Instance.FireSnapReplacement(currentlySnapped);
        inventoryObjectModelDictionary[recievedSnapObject].SetActive(true);
        Debug.Log("Removed " + inventoryObjectModelDictionary[currentlySnapped] + 
            " and replaced it with " + inventoryObjectModelDictionary[recievedSnapObject]);

        currentlySnapped = recievedSnapObject;
    }


    void OnHover(bool hovering)
    {
        rend.enabled = hovering;
    }


}
