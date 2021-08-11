using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using static SnapZoneManager;

public class InventoryObject : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    Camera cam;
    Vector3 originalPosition;
    public SnapObjectType thisObjectType;
    Image image;
    public TextMeshProUGUI hoverText;
    bool isDragging;
    public LayerMask snapzoneLayerMask = 1 << 6;
    public LayerMask interactableLayerMask = 1 << 7;

    void Start()
    {
        image = GetComponent<Image>();
        originalPosition = GetComponent<RectTransform>().position;
        cam = Camera.main;
        SnapZoneManager.Instance.SnappedObjectReplaced += ReenableImage;
        isDragging = false;
        hoverText.enabled = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        SnapZoneManager.Instance.FireHoveringEvent(false);
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        //first, check if we hit any snapzones by doing a raycast that only checks the snapzone layer.
        if (Physics.Raycast(ray, out hit, 20f, snapzoneLayerMask))
        {
            Transform objectHit = hit.transform;
            Debug.Log("Watch it, you nearly dropped me onto: " + objectHit.name);
            if (objectHit.CompareTag("SnapZone"))
            {
                bool isSnappable = objectHit.gameObject.GetComponent<SnapZone>().CheckIfInInventory(thisObjectType);
                if (isSnappable)
                {
                    Debug.Log("Snapping in your chosen object.");
                    image.enabled = false;
                    
                }
            }
            else
            {
                
                Debug.Log("Uh, that wasn't valid. There should probably be an error message here for the user.");
            }
        }

        //next, we check if we hit anything interactable by doing a raycast that only checks the interactable layer
        if (Physics.Raycast(ray, out hit, 20f, interactableLayerMask))
        {
            Debug.Log("Woah, we should check if we can interact with this object!");
            Transform objectHit = hit.transform;
            if (objectHit.CompareTag("InteractableZone"))
            {
                Debug.Log("Gonna try to interact with: " + objectHit.name);
                objectHit.GetComponent<InteractableZone>().CheckIfInInventory(thisObjectType);
            }


        }


        isDragging = false;
        ReturnToOriginalPosition();
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
        SnapZoneManager.Instance.FireHoveringEvent(true);
        isDragging = true;
        //Debug.Log("I'm being dragged!");
    }

    public void ReturnToOriginalPosition()
    {
        transform.position = originalPosition;
    }

    void ReenableImage(SnapObjectType replacedObject)
    {
        if(replacedObject == thisObjectType)
        {
            image.enabled = true;
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
}
