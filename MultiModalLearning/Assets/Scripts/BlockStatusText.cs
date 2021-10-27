using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static ClampMovement;

public class BlockStatusText : MonoBehaviour
{
    SnapRestrictions snapRestrictions;

    public TextMeshProUGUI clampedSecurely, looseText, needsATestWackText, jiggleRequredText;

    // Start is called before the first frame update
    void Start()
    {
        snapRestrictions = GetComponent<SnapRestrictions>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ClampMovement.Instance.currentClampStatus == ClampStatus.Loose)
        {
            //Debug.Log("Loose.");
            looseText.enabled = true;
            clampedSecurely.enabled = false;
            needsATestWackText.enabled = false;
            jiggleRequredText.enabled = false;
        }
        else if (ClampMovement.Instance.currentClampStatus == ClampStatus.NeedsAWack)
        {
            //Debug.Log("Needs wack.");
            needsATestWackText.enabled = true;
            jiggleRequredText.enabled = false;
            clampedSecurely.enabled = false;
            looseText.enabled = false;
        }
        else if (ClampMovement.Instance.currentClampStatus == ClampStatus.NeedsATestJiggle)
        {
            //Debug.Log("Needs jiggle.");
            jiggleRequredText.enabled = true;
            needsATestWackText.enabled = false;
            clampedSecurely.enabled = false;
            looseText.enabled = false;
        }
        else if(ClampMovement.Instance.currentClampStatus == ClampStatus.FullySecure)
        {
            //Debug.Log("Secure.");
            clampedSecurely.enabled = true;
            jiggleRequredText.enabled = false;
            needsATestWackText.enabled = false;
            looseText.enabled = false;
        }
    }
}
