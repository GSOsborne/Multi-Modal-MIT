using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static Spinning;

public class CycleTextObject : MonoBehaviour
{
    public TextMeshProUGUI looseText, tightText;
    public bool isTight;

    private void Start()
    {
        isTight = tightText.gameObject.activeSelf;
        Spinning.SpinningEvent += CheckIfTight;
    }

    public void CycleText()
    {
        isTight = !isTight;
        looseText.gameObject.SetActive(!looseText.gameObject.activeSelf);
        tightText.gameObject.SetActive(!tightText.gameObject.activeSelf);
    }

    void CheckIfTight(bool isSpinning)
    {
        if (isSpinning)
        {
            if (!isTight)
            {
                FailureState.Instance.SystemFailure("You started spinning before the chuck was tightened.");
            }
        }
    }
}
