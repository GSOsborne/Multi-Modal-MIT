using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoordinateDisplay : MonoBehaviour
{
    //public Slider xSlider, ySlider, zSlider;
    public MouseRotate xRotator, yRotator, zRotator;
    MouseRotate currentlySelectedRotator;
    public Text xText, yText, zText;
    public Text spinSpeedText;
    public GameObject spinSpeedButton;

    public Vector3 zeroPosition;
    public Vector3 currentCoordinates;

    public bool isJogging;
    public Jogging jogging;

    public bool enteringSpinSpeed;

    bool valueUnselected;
    bool valueEntered;
    string enteredValue;

    bool enterValueRoutineStarted;

    // Start is called before the first frame update
    void Start()
    {
        isJogging = false;
        enteringSpinSpeed = false;
        valueUnselected = true;
        valueEntered = false;
        enterValueRoutineStarted = false;
        ResetZero();
    }

    // Update is called once per frame
    void Update()
    {
        float xCoord = xRotator.storedRotation - zeroPosition.x;
        float yCoord = yRotator.storedRotation - zeroPosition.y;
        float zCoord = zRotator.storedRotation - zeroPosition.z;
        if (valueUnselected)
        {
            xText.text = "X: " + xCoord;
            yText.text = "Y: " + yCoord;
            zText.text = "Z: " + zCoord;
        }

        currentCoordinates = new Vector3(xCoord, yCoord, zCoord);
    }

    public void ResetZero()
    {
        zeroPosition = new Vector3(xRotator.storedRotation, yRotator.storedRotation, zRotator.storedRotation);
    }

    public void CycleJogStatus()
    {
        isJogging = !isJogging;
    }

    public void UnselectAxis()
    {
        valueUnselected = true;
        enteringSpinSpeed = false; //if they actually clicked the spin speed button, this should fire first before it gets set back to true
    }

    public void SubmitValue()
    {
        float parsedNumber;
        if (float.TryParse(enteredValue, out parsedNumber))
        {
            valueEntered = true;
            UnselectAxis();
        }
        else
        {
            FailureState.Instance.SystemFailure("You did not input a valid value.");
            UnselectAxis();
        }



    }

    public void DeleteDigit()
    {
        if (enteredValue.Length > 0)
        {
            enteredValue = enteredValue.Substring(0, enteredValue.Length - 1);
        }

    }

    public void StartInputtingSpinSpeed()
    {
        if (gameObject.activeSelf)
        {
            UnselectAxis();
            enteringSpinSpeed = true;
            StartCoroutine(EnterSpinSpeedValue());
        }

    }

    public void XButtonHeld()
    {
        if (isJogging)
        {
            jogging.JogX();
        }
        else
        {
            if (currentlySelectedRotator != xRotator)
            {
                UnselectAxis();
                StartCoroutine(EnterValue(xRotator));
            }
            else if (!enterValueRoutineStarted)
            {
                StartCoroutine(EnterValue(xRotator));
            }
        }
    }

    public void YButtonHeld()
    {
        if (isJogging)
        {
            jogging.JogY();
        }
        else
        {
            if (currentlySelectedRotator != yRotator)
            {
                UnselectAxis();
                StartCoroutine(EnterValue(yRotator));
            }
            else if (!enterValueRoutineStarted)
            {
                StartCoroutine(EnterValue(yRotator));
            }
        }
    }

    public void ZButtonHeld()
    {
        if (isJogging)
        {
            jogging.JogZ();
        }
        else
        {
            if (currentlySelectedRotator != zRotator)
            {
                UnselectAxis();
                StartCoroutine(EnterValue(zRotator));
            }
            else if (!enterValueRoutineStarted)
            {
                StartCoroutine(EnterValue(zRotator));
            }
        }
    }

    IEnumerator EnterValue(MouseRotate whichAxis)
    {
        enterValueRoutineStarted = true;
        enteredValue = null;
        Debug.Log("Ready to enter value!");
        Text whichText;
        if(whichAxis == xRotator)
        {
            whichText = xText;
        }
        else if (whichAxis == yRotator)
        {
            whichText = yText;
        }
        else if (whichAxis == zRotator)
        {
            whichText = zText;
        }
        else
        {         
            whichText = xText;
            Debug.Log("You should not have been able to get here!");
        }
        valueEntered = false;
        whichText.fontStyle = FontStyle.Bold;
        string previousValue = whichText.text;


        yield return null;


        valueUnselected = false;
        whichText.text = whichText.text.Substring(0, whichText.text.Length - 1);
        float timer = 0;
        bool underscoreAdded = false;
        while (!valueUnselected)
        {
            timer += Time.deltaTime;

            //underscore logic
            if (timer > 1f && !underscoreAdded)
            {
                //whichText.text = whichText.text.Substring(0,2) + enteredValue + "_";
                //Debug.Log("Adding underscore at time: " + timer);
                underscoreAdded = true;
            }
            if(timer > 2f && underscoreAdded)
            {
                timer -= 2f;
                //whichText.text = whichText.text.Substring(0, whichText.text.Length - 1);
                //Debug.Log("Removing underscore at time: " + timer);
                underscoreAdded = false;
            }

            if (underscoreAdded)
            {
                whichText.text = whichText.text.Substring(0, 2) + " " + enteredValue + "_";
            }
            else
            {
                whichText.text = whichText.text.Substring(0, 2) + " " + enteredValue;
            }



            yield return null;
        }

        Debug.Log("We've been unselected!");

        /*
        if(timer > 1)
        {
            whichText.text.Remove(whichText.text.Length - 1);
        }
        */

        if (valueEntered)
        {
            whichText.text = enteredValue;
            if(whichText == xText)
            {

                zeroPosition.x = xRotator.storedRotation - float.Parse(enteredValue);
            }
            else if(whichText == yText)
            {
                zeroPosition.y = yRotator.storedRotation - float.Parse(enteredValue);
            }
            else if(whichText == zText)
            {
                zeroPosition.z = zRotator.storedRotation - float.Parse(enteredValue);
            }
        }
        else
        {
            whichText.text = previousValue;
        }
        whichText.fontStyle = FontStyle.Normal;

        enterValueRoutineStarted = false;

    }

    IEnumerator EnterSpinSpeedValue()
    {
        yield return null;
        enteredValue = null;
        enterValueRoutineStarted = true;
        valueEntered = false;
        spinSpeedText.fontStyle = FontStyle.Bold;
        string previousValue = spinSpeedText.text;
        valueUnselected = false;
        //spinSpeedText.text = spinSpeedText.text.Substring(0, spinSpeedText.text.Length - 1);
        float timer = 0;
        bool underscoreAdded = false;
        while (!valueUnselected)
        {
            timer += Time.deltaTime;

            //underscore logic
            if (timer > 1f && !underscoreAdded)
            {
                underscoreAdded = true;
            }
            if (timer > 2f && underscoreAdded)
            {
                timer -= 2f;
                underscoreAdded = false;
            }

            if (underscoreAdded)
            {
                spinSpeedText.text = enteredValue + "_";
            }
            else
            {
                spinSpeedText.text = enteredValue;
            }
            yield return null;
        }

        if (valueEntered)
        {
            spinSpeedText.text = enteredValue;
        }
        else
        {
            spinSpeedText.text = previousValue;
        }
        spinSpeedText.fontStyle = FontStyle.Normal;
        enteringSpinSpeed = false;
        enterValueRoutineStarted = false;
        spinSpeedButton.GetComponent<CycleButtonSelected>().CycleSelectionHighlight();

    }

    public void AddDigit(string digit)
    {
        enteredValue += digit;
    }
}
