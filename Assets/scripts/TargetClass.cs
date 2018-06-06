using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetClass : MonoBehaviour {

    public string id {get; set;}
    public GameObject targetobject { get; set; }

    public TargetClass()
    {
        id = "";
        targetobject = null;
    }
        
    public TargetClass(string _id,GameObject g1)
    {
        id = _id;
        targetobject = g1;
    }
}
