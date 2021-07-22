using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoordinateDisplay : MonoBehaviour
{
    public Slider xSlider, ySlider, zSlider;
    public Text xText, yText, zText;

    public Vector3 zeroPosition;
    public Vector3 currentCoordinates;

    // Start is called before the first frame update
    void Start()
    {
        ResetZero();
    }

    // Update is called once per frame
    void Update()
    {
        float xCoord = xSlider.value - zeroPosition.x;
        float yCoord = ySlider.value - zeroPosition.y;
        float zCoord = zSlider.value - zeroPosition.z;
        xText.text = "X: " + xCoord;
        yText.text = "Y: " + yCoord;
        zText.text = "Z: " + zCoord;
        currentCoordinates = new Vector3(xCoord, yCoord, zCoord);
    }

    public void ResetZero()
    {
        zeroPosition = new Vector3(xSlider.value, ySlider.value, zSlider.value);
    }
}
