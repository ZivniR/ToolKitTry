using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedSqure : MonoBehaviour {
    public Vector3 position { get; set; }
    public Quaternion routation { get;  set; }
    public int place { get; set; }
    public bool flag { get; set; }
    public Vector3 source { get; set; }
    public RedSqure(Vector3 pos,Quaternion rout, int pl)
    {
        position = pos;
        routation = rout;
        place = pl;
        flag = false;
    }

    public RedSqure() {
        position = new Vector3(9999,9999,9999);
        routation = new Quaternion(9999, 9999, 9999, 9999);
        place = -1;
        flag = false;


    }

}
