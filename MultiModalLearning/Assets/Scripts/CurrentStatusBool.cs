using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrentStatusBool : MonoBehaviour
{
    public TextMeshProUGUI defaultStateText;
    public TextMeshProUGUI interactedStateText;
    bool interactedState;

    private void Start()
    {
        interactedState = false;
        defaultStateText.enabled = true;
        interactedStateText.enabled = false;
    }

    public void CycleInteracted()
    {
        interactedState = !interactedState;
        defaultStateText.enabled = !interactedState;
        interactedStateText.enabled = interactedState;
    }
}
