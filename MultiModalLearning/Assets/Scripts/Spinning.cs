using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinning : MonoBehaviour
{
    public static Spinning Instance;

    public static event System.Action<bool> SpinningEvent;
    public Transform spinningTrans;
    public bool isSpinning;
    public float spinSpeed;
   
    
    Vector3 startRot;

    public GameObject dial;

    // Start is called before the first frame update
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        SpinningEventCall(false);
        startRot = dial.transform.rotation.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        if (isSpinning)
        {
            spinningTrans.Rotate(0f, 0f, -spinSpeed * Time.deltaTime);
        }
    }

    public void SpinningEventCall(bool isSpinningBool)
    {
        isSpinning = isSpinningBool;
        SpinningEvent?.Invoke(isSpinning);
    }

    public void CycleSpin()
    {
        //Debug.Log("Cycling spin.");
        SpinningEventCall(!isSpinning);
        if (isSpinning)
        {
            dial.transform.rotation = Quaternion.Euler(startRot + new Vector3(0f, 0f, 50f));
        }
        else
        {
            dial.transform.rotation = Quaternion.Euler(startRot);
        }
    }
}
