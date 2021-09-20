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
        SpinningEventCall(!isSpinning);
    }
}
