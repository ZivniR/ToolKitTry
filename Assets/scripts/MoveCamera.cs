using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveCamera : MonoBehaviour {
    public Text Mover;
    private Quaternion vec3;
    // Use this for initialization
    void Start () {
        vec3 = new Quaternion();
        vec3 = Camera.main.transform.rotation;
        Mover.text = "rotation:" + "(" + System.Math.Round(vec3.x, 2) + "," + System.Math.Round(vec3.y, 2) + ")";
	}
	
	// Update is called once per frame
	void Update () {
        vec3 = Camera.main.transform.rotation;
        Mover.text = "rotation:" + "(" + System.Math.Round(vec3.x,2) + "," + System.Math.Round(vec3.y, 2) + ")";
    }
}
