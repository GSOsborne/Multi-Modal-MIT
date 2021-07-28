using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapZoneManager : MonoBehaviour
{

    public enum SnapObjectType { Empty, Drill1, Drill2, Drill3, Chuck, ChuckKey }
    public static SnapZoneManager Instance;

    public event System.Action<bool> Hovering;
    public event System.Action<SnapObjectType> SnappedObjectReplaced;

    private void Awake()
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

    public void FireSnapReplacement(SnapObjectType replacedObjectType)
    {
        SnappedObjectReplaced?.Invoke(replacedObjectType);
    }

    public void FireHoveringEvent(bool hovering)
    {
        Hovering?.Invoke(hovering);
        //Debug.Log("Where we dropping? Tomato town?");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
