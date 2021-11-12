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

    public float scrollZoomMultiplier;
    public float maxZoomScale, minZoomScale;
    [SerializeField] float scaleFactor;
    bool returningToZero;

    private void Start()
    {
        isRotating = false;
        scaleFactor = 1;
        returningToZero = false;
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
        returningToZero = true;
        Quaternion startRotation = sceneObjects.rotation;
        Vector3 startScale = transform.localScale;
        for(int i = 0; i <= framesToZero; i++)
        {
            float lerp = (float)i / (float)framesToZero;
            //Debug.Log(lerp);
            sceneObjects.rotation = Quaternion.Lerp(startRotation, Quaternion.identity, lerp);
            transform.localScale = Vector3.Lerp(startScale, new Vector3(1f, 1f, 1f), lerp);
            yield return null;
        }
        returningToZero = false;
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
        if(Input.mouseScrollDelta.magnitude > 0.1 && !returningToZero)
        {
            scaleFactor -= Input.mouseScrollDelta.y * Time.deltaTime * scrollZoomMultiplier;
            scaleFactor = Mathf.Clamp(scaleFactor, minZoomScale, maxZoomScale);
            transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
        }
    }



}
