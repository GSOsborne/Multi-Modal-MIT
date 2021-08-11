using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CycleTextObject : MonoBehaviour
{
    public TextMeshProUGUI looseText, tightText;
    public bool isTight;

    private void Start()
    {
        isTight = tightText.gameObject.activeSelf;
    }

    public void CycleText()
    {
        isTight = !isTight;
        looseText.gameObject.SetActive(!looseText.gameObject.activeSelf);
        tightText.gameObject.SetActive(!tightText.gameObject.activeSelf);
    }
}
