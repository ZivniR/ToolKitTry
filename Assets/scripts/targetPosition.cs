using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*handle the targets position*/

public class targetPosition : MonoBehaviour {

    public Text Mover;
    private Vector3 vec3;
    [SerializeField]
    public HoloGrid hg;
    public Text Distance;
    public int index=0;
    public float nextActionTime = 0.0f;
    public float period = 60.0f;
    Vector3 target;
    // Use this for initialization
    void Start()
    { 
        vec3 = new Vector3();
        if (hg.TargetList.Count > 0)
            if(hg.targetongreed!=null)
                vec3 = target;
        if (hg.TargetList.Count > 0)
        {
            Mover.text = "Target " + "[" + index % hg.TargetList.Count + "]:" + "(" + System.Math.Round(vec3.x, 2) + "," + System.Math.Round(vec3.y, 2) + "," + System.Math.Round(vec3.z, 2) + ")";
            Distance.text = "Distance: " + System.Math.Round(Vector3.Distance(vec3, Camera.main.transform.position),2);
        }
    }

    // Update is called once per frame
    void Update()
    {
        target = hg.targetongreed;
            if (hg.TargetList.Count < 1)
            {
                Mover.text = "NO TARGETS";
                Distance.text = "None";
            }

            else
            {
                if (target != null)
                {
                    vec3 = hg.targetongreed;
                    Mover.text = "Target: " + "(" + System.Math.Round(vec3.x, 0) + "," + System.Math.Round(vec3.y, 0) + "," + System.Math.Round(vec3.z, 0) + ")";
                    Distance.text = "Distance: " + System.Math.Round(Vector3.Distance(vec3, Camera.main.transform.position), 2);
                }
            }
    }
}

