using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallelsReveal : MonoBehaviour
{
    public MeshRenderer parallel1, parallel2;
    public GameObject parallelSnapZone;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        parallel1.enabled = true;
        parallel2.enabled = true;
        parallelSnapZone.SetActive(true);
    }

    private void OnDisable()
    {
        parallel1.enabled = false;
        parallel2.enabled = false;
        parallelSnapZone.SetActive(false);
    }
}
