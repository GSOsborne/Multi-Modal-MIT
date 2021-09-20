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
    float startY, startZ;
    public float angleBounds;
    public Transform buttonTransform;
    public float modelRotationMultiplier;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        previousRay = new Vector3(0f, 0f, 0f);
        storedRotation = 0f;
        startY = buttonTransform.rotation.eulerAngles.y;
        startZ = buttonTransform.rotation.eulerAngles.z;
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
        Debug.DrawRay(transform.position, cross, Color.blue);
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
        storedRotation = Mathf.Clamp(storedRotation, -angleBounds, angleBounds);

        //now we can rotate using the stored rotation
        buttonTransform.rotation = Quaternion.Euler(storedRotation * modelRotationMultiplier, startY, startZ);

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
        storedRotation = Mathf.Clamp(storedRotation, -angleBounds, angleBounds);

        //now we can rotate using the stored rotation
        buttonTransform.rotation = Quaternion.Euler(storedRotation * modelRotationMultiplier, startY, startZ);
    }
}
