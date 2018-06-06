using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class positioner : MonoBehaviour {

    public Text Mover;
    private Vector3 vec3;
    //Quaternion vec3;
    int col;
    int row;
    int number;
    [SerializeField]
    public HoloGrid hg;
    // Use this for initialization
    void Start()
    {
        vec3 = new Vector3();
        //vec3 = new Quaternion();
        col = hg.colns;
        row = hg.rows;
        number = (col * row) / 2;
        //vec3 = hg.c0[number].transform.position;
        vec3 = Camera.main.transform.position;
        //vec3 = transform.rotation;
        Mover.text = "position:" + "(" + System.Math.Round(vec3.x, 0) + "," + System.Math.Round(vec3.y, 0) + "," + System.Math.Round(vec3.z, 0) + ")";
    }

    // Update is called once per frame
    void Update()
    {
        col = hg.colns;
        row = hg.myrows;
        number = (col * row) / 2;
        //vec3 = hg.c0[number].transform.position;
        //vec3 = transform.rotation;
        vec3 = Camera.main.transform.position;
        Mover.text = "position:" + "(" + System.Math.Round(vec3.x, 0) + "," + System.Math.Round(vec3.y, 0) + "," + System.Math.Round(vec3.z, 0) + ")";
    }

}
