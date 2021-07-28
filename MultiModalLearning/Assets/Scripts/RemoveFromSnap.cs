using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using static SnapZoneManager;

public class RemoveFromSnap : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI hoverText;
    public SnapZone snapZone;
    Vector3 offset;
    bool offsetCalculated;
    Vector3 originalPosition;
    SnapObjectType currentlySnapped;

    // Start is called before the first frame update
    void Start()
    {
        hoverText.enabled = false;

        SnapZoneManager.Instance.SnappedObjectReplaced += ReevaluateCurrentSnapObject;

    }

    void ReevaluateCurrentSnapObject(SnapObjectType junk)
    {
        Debug.Log("Ooooo, a new snap object!");

        originalPosition = snapZone.inventoryObjectModelDictionary[snapZone.currentlySnapped].transform.position;
        currentlySnapped = snapZone.currentlySnapped;
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        if (!offsetCalculated)
        {
            offset = Camera.main.transform.position - snapZone.inventoryObjectModelDictionary[snapZone.currentlySnapped].transform.position;
            Debug.Log("Offset is: " + offset);
            offsetCalculated = true;
        }

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = (offset - mousePos).z;
        
        Vector3 desiredPos = Camera.main.ScreenToWorldPoint(mousePos);
        Debug.Log("Desired Postion is: " + desiredPos);
        snapZone.inventoryObjectModelDictionary[snapZone.currentlySnapped].transform.position = desiredPos;
        SnapZoneManager.Instance.FireHoveringEvent(true);
        //Debug.Log("I'm being dragged!");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        ReturnToOriginalPosition();
        offsetCalculated = false;
        SnapZoneManager.Instance.FireHoveringEvent(false);
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            Transform objectHit = hit.transform;
            Debug.Log("Watch it, you nearly dropped me onto: " + objectHit.name);
            if (objectHit.CompareTag("SnapZone"))
            {
                bool isSnappable = objectHit.gameObject.GetComponent<SnapZone>().CheckIfInInventory(snapZone.currentlySnapped);
                if (isSnappable)
                {
                    Debug.Log("Snapping in your chosen object.");
                }
            }
            else
            {
                Debug.Log("I think you're trying to remove this object, let me help you with that.");
                snapZone.CheckIfInInventory(SnapObjectType.Empty);
            }

        }


    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
        hoverText.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hoverText.enabled = false;
    }

    void ReturnToOriginalPosition()
    {
        snapZone.inventoryObjectModelDictionary[currentlySnapped].transform.position = originalPosition;
    }
}
