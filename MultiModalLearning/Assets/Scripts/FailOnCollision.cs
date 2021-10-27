using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FailureState;

public class FailOnCollision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collided with something.");


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Untagged"))
        {
            FailureState.Instance.SystemFailure("Something touched something it shouldn't have: " + other.gameObject.name);
        }
    }
}
