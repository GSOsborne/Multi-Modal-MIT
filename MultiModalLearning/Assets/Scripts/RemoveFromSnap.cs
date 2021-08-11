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
    public TextMeshProUGUI cantRemoveText;
    public SnapZone snapZone;
    Vector3 offset;
    bool offsetCalculated;
    Vector3 originalPosition;
    SnapObjectType currentlySnapped;

    SnapRestrictions restrictions;

    // Start is called before the first frame update
    void Start()
    {
        snapZone.currentlySnapped = SnapObjectType.Empty;
        hoverText.enabled = false;
        cantRemoveText.enabled = false;
        SnapZoneManager.Instance.SnappedObjectReplaced += ReevaluateCurrentSnapObject;
    }

    void OnDisable()
    {
        SnapZoneManager.Instance.SnappedObjectReplaced -= ReevaluateCurrentSnapObject;
    }

    void OnEnable()
    {
        SnapZoneManager.Instance.SnappedObjectReplaced += ReevaluateCurrentSnapObject;
    }

    IEnumerator ReevaluateCoroutine()
    {
        yield return null;
        Debug.Log("Ooooo, a new snap object!");
        //we have a new snapped object, so all our logic for removal has to take into account the various restrictions and position of the new object

        if (snapZone.gameObject.activeSelf == true)
        {
            originalPosition = snapZone.inventoryObjectModelDictionary[snapZone.currentlySnapped].transform.position;
            currentlySnapped = snapZone.currentlySnapped;
            Debug.Log("Now the currently snapped object is: " + currentlySnapped);
            restrictions = snapZone.inventoryObjectModelDictionary[snapZone.currentlySnapped].GetComponent<SnapRestrictions>();
            Debug.Log("Referencing the restrictions component on " + snapZone.inventoryObjectModelDictionary[snapZone.currentlySnapped]);
        }
    }

    void ReevaluateCurrentSnapObject(SnapObjectType junk)
    {
        if (gameObject.activeSelf)
        {
            Debug.Log("Dammit I'm on, im gonna do the coroutine, see: " + gameObject.name + " : " + gameObject.activeSelf);
            StartCoroutine(ReevaluateCoroutine());
        }


    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        if (restrictions.removable)
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
            //Debug.Log("Desired Postion is: " + desiredPos);
            snapZone.inventoryObjectModelDictionary[snapZone.currentlySnapped].transform.position = desiredPos;
            SnapZoneManager.Instance.FireHoveringEvent(true);
            //Debug.Log("I'm being dragged!");
        }
        else
        {
            Debug.Log("Uh, you can't remove that yet. For reasons.");
        }

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("Hey wait you let go ahhhhhhhhhhhh");
        ReturnToOriginalPosition();
        offsetCalculated = false;
        SnapZoneManager.Instance.FireHoveringEvent(false);

        if (restrictions.removable)
        {

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
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentlySnapped != SnapObjectType.Empty)
        {
            if (restrictions.removable)
            {
                hoverText.enabled = true;
            }
            else
            {
                cantRemoveText.enabled = true;

                //Debug.Log("Should add a can't remove text here");
            }

        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hoverText.enabled = false;
        cantRemoveText.enabled = false;
    }

    void ReturnToOriginalPosition()
    {
        Debug.Log("Returning " + snapZone.inventoryObjectModelDictionary[currentlySnapped] + " to original position.");
        snapZone.inventoryObjectModelDictionary[currentlySnapped].transform.position = originalPosition;
    }
}
