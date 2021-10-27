using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MouseRotate : MonoBehaviour, IDragHandler
{
    Camera mainCamera;
    Vector3 previousRay;
    public float storedRotation;
    public float angleSensitivity;
    float startX, startY;
    public float angleBounds;
    public Transform buttonTransform;
    public float modelRotationMultiplier = 1;

    //public float storedRotationMultiplier;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        previousRay = new Vector3(0f, 0f, 0f);
        storedRotation = 0f;
        startY = buttonTransform.localRotation.eulerAngles.y;
        startX = buttonTransform.localRotation.eulerAngles.x;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        Vector3 newRay = Input.mousePosition - mainCamera.WorldToScreenPoint(transform.position);
        //Debug.Log("I'm being dragged, new Ray is " + newRay);

        //find angle between the last frame's mouse position and new mouse position
        float angle = Vector3.Angle(previousRay, newRay) * angleSensitivity;
        //Debug.DrawRay(transform.position, previousRay, Color.green);
        //Debug.DrawRay(transform.position, newRay, Color.green);
        //Debug.Log("Change in angle is " + angle);

        //we will use the cross product to determine if we're going clockwise or counterclockwise
        Vector3 cross = Vector3.Cross(previousRay, newRay);
        //Debug.Log("Cross product is: " + cross);
        //Debug.DrawRay(transform.position, cross, Color.blue);
        //Debug.Log("Angle between forwards and cross is: " + Vector3.Angle(transform.forward, cross));
        if(Vector3.Angle(transform.forward, cross) > 90)
        {
            storedRotation += angle;
            //Debug.Log("Rotating clockwise! Stored rotation is: " + storedRotation);
        }
        else
        {
            storedRotation -= angle;
            //Debug.Log("Rotating counterclockwise! Stored rotation is: " + storedRotation);
        }
        //make sure stored rotation value is clamped
        //storedRotation = Mathf.Clamp(storedRotation, -angleBounds, angleBounds);
        if(storedRotation > angleBounds)
        {
            //Debug.Log(storedRotation + " is greater than angle bounds of: " + angleBounds);
            storedRotation = angleBounds;
            //Debug.Log("rotation was greater than angle bounds. Now: " + storedRotation);
        }
        else if(storedRotation < -angleBounds)
        {
            //Debug.Log(storedRotation + " is less than angle bounds of: " + -angleBounds);
            storedRotation = -angleBounds;
            //Debug.Log("rotation was less than angle bounds. Now: " + storedRotation);
        }

        //now we can rotate using the stored rotation
        buttonTransform.localRotation = Quaternion.Euler(startX, startY, storedRotation * modelRotationMultiplier);

        //reset previous ray for the next frame of dragging
        previousRay = newRay;

    }

    public void Jog(float jogSpeed)
    {
        storedRotation += jogSpeed;
        if(storedRotation > angleBounds || storedRotation < -angleBounds)
        {
            //Debug.Log("Do we need an error message here?");
        }
        //make sure stored rotation value is clamped
        if (storedRotation > angleBounds)
        {
            storedRotation = angleBounds;
        }
        else if (storedRotation < -angleBounds)
        {
            storedRotation = -angleBounds;
        }

        //now we can rotate using the stored rotation
        buttonTransform.localRotation = Quaternion.Euler(startX, startY, storedRotation * modelRotationMultiplier);
    }
}
