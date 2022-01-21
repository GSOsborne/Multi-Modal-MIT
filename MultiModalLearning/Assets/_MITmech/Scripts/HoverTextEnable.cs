using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverTextEnable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject hoverText;
    
    // Start is called before the first frame update
    void Start()
    {
        hoverText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hoverText.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hoverText.SetActive(false);
    }

}
