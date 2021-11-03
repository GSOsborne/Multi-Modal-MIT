using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineRotator : MonoBehaviour
{
    public float maxDegrees;
    public float rotateOutSpeed;
    //public float returnToCenterSpeed;
    bool isRotating;
    public int framesToZero;

    public RectTransform[] rotationCanvases;
    public Transform sceneObjects;

    private void Start()
    {
        isRotating = false;
    }

    public void RotateLeft()
    {
        sceneObjects.Rotate(new Vector3(0f, -rotateOutSpeed * Time.deltaTime, 0f));
        float currentYRotation = transform.rotation.eulerAngles.y;
        if(currentYRotation > 180)
        {
            currentYRotation -= 360;
        }
        if (currentYRotation > maxDegrees)
        {
            //Debug.Log("A little too far left.");
            transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        }
        else
        {
            foreach (RectTransform rect in rotationCanvases)
            {
                rect.Rotate(new Vector3(0f, rotateOutSpeed * Time.deltaTime, 0f));
            }
        }

    }

    public void RotateRight()
    {
        sceneObjects.Rotate(new Vector3(0f, rotateOutSpeed * Time.deltaTime, 0f));
        float currentYRotation = transform.rotation.eulerAngles.y;
        if (currentYRotation > 180)
        {
            currentYRotation -= 360;
        }
        if (currentYRotation < -maxDegrees)
        {
            //Debug.Log("A little too far right.");
            transform.rotation = Quaternion.Euler(0f, -90f, 0f);
        }
        else
        {
            foreach (RectTransform rect in rotationCanvases)
            {
                rect.Rotate(new Vector3(0f, -rotateOutSpeed * Time.deltaTime, 0f));
            }
        }
    }

    public void IsRotating(bool areWeRotating)
    {
        isRotating = areWeRotating;
    }

    public void ReturnToZero()
    {
        StopAllCoroutines();
        StartCoroutine(ReturnToZeroCoroutine());
    }

    IEnumerator ReturnToZeroCoroutine()
    {
        Quaternion startRotation = transform.rotation;
        for(int i = 0; i <= framesToZero; i++)
        {
            float lerp = (float)i / (float)framesToZero;
            //Debug.Log(lerp);
            transform.rotation = Quaternion.Lerp(startRotation, Quaternion.identity, lerp);
            yield return null;
        }
    }

    private void Update()
    {
        /*
        if (!isRotating)
        {
            Vector3 rotationVector = transform.rotation.eulerAngles;
            if(rotationVector.y > 180)
            {
                rotationVector.y -= 360;
            }
            transform.rotation = Quaternion.Euler(Vector3.RotateTowards(rotationVector, Vector3.zero, returnToCenterSpeed, 1));
        }
        */
    }



}
