using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarnOnEnable : MonoBehaviour
{
    public string warningText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        FailureState.Instance.DisplayWarning(warningText);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
