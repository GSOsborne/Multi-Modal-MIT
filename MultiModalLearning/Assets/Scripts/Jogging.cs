using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Jogging : MonoBehaviour
{
    public bool isPositive;
    public float jogXSpeed, jogYSpeed, jogZSpeed;
    public MouseRotate xRot, yRot, millHeadRot;
    public Text posNegText;
    public CoordinateDisplay coordDisp;
    // Start is called before the first frame update
    void Start()
    {
        isPositive = true;
        posNegText.text = "+";
    }

    public void CyclePosNeg()
    {
        if (coordDisp.isJogging)
        {
            isPositive = !isPositive;
            if (isPositive)
            {
                posNegText.text = "+";
            }
            else
            {
                posNegText.text = "-";
            }
        }
    }

    public void JogX()
    {
        if (isPositive)
        {
            xRot.Jog(jogXSpeed * Time.deltaTime);
        }
        else
        {
            xRot.Jog(-jogXSpeed * Time.deltaTime);
        }
    }

    public void JogY()
    {
        if (isPositive)
        {
            yRot.Jog(jogYSpeed * Time.deltaTime);
        }
        else
        {
            yRot.Jog(-jogYSpeed * Time.deltaTime);
        }
    }

    public void JogZ()
    {
        if (isPositive)
        {
            millHeadRot.Jog(jogZSpeed * Time.deltaTime);
        }
        else
        {
            millHeadRot.Jog(-jogZSpeed * Time.deltaTime);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
