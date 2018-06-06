using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetCount : MonoBehaviour {

    public Text Mover;
    int targets;
    [SerializeField]
    public HoloGrid hg;
    public float nextActionTime = 0.0f;
    public float period = 30.0f;
    // Use this for initialization
    void Start()
    {

        targets = hg.TargetList.Count;

        Mover.text ="" + targets;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        targets = hg.TargetList.Count;
        Mover.text = "" + targets;
    }
}
