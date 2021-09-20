using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FakeCyclePosNeg : MonoBehaviour
{
    public CoordinateDisplay coordDisp;
    public Jogging jogging;
    public Text posNegText;
    public void CyclePosNeg()
    {
        if (coordDisp.isJogging)
        {
            if (jogging.isPositive)
            {
                posNegText.text = "+";
            }
            else
            {
                posNegText.text = "-";
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
