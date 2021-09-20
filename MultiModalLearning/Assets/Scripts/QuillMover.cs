using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuillMover : MonoBehaviour
{
    //public Slider zAxisSlider;
    public MouseRotate zRotator;
    Vector3 startPos;
    Vector3 startZRot;

    public GameObject zQuillLever;
    public float rotationMultiplier;


    // Start is called before the first frame update
    void Start()
    {
        startZRot = zQuillLever.transform.rotation.eulerAngles;
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateZPosition();
    }

    public void UpdateZPosition()
    {
        transform.position = startPos + Vector3.up * zRotator.storedRotation;
        Vector3 newZRot = startZRot + new Vector3(1f, 0f, 0f) * rotationMultiplier * zRotator.storedRotation;
        zQuillLever.transform.rotation = Quaternion.Euler(newZRot);
    }
}
