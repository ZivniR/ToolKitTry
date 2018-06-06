using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targets : MonoBehaviour {


    Vector3 position { get; set; }
    Vector3 GridPos { get; set; }

    Targets()
    {
        position= new Vector2(0,0) ;

    }

    public Targets(Vector3 vec3, Vector3 gridpos)
    {
        position = vec3;
        GridPos = gridpos;
    }


}
