using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* handle the compass text*/

public class CompassText : MonoBehaviour {
    [SerializeField]
    Text compass;
    [SerializeField]
    Text bearing;
    Compass compassinfo;
    public float bear;
    public float degree;
    // Use this for initialization
    void Start () {
    }

    // Update is called once per frame
    void Update() {
        degree = Camera.main.transform.rotation.eulerAngles.y;
        if (30 < degree && degree < 60)
            compass.text = "NE " + System.Math.Round(degree, 2);
        else if (degree > 60 && degree < 120)
            compass.text = "E " + System.Math.Round(degree, 2);
        else if (degree > 120 && degree < 150)
            compass.text = "SE " + System.Math.Round(degree, 2);
        else if (degree > 150 && degree < 210)
            compass.text = "S " + System.Math.Round(degree, 2);
        else if (degree > 210 && degree < 240)
            compass.text = "SW " + System.Math.Round(degree, 2);
        else if (degree > 240 && degree < 300)
            compass.text = "W " + System.Math.Round(degree, 2);
        else if (degree > 300 && degree < 330)
            compass.text = "NW " + System.Math.Round(degree, 2);
        else
            compass.text = "N " + System.Math.Round(degree, 2);
        bear = Camera.main.transform.rotation.eulerAngles.x;
        if (0 < bear && bear < 180)
        {
            bear = -bear;
            bearing.text = "" + bear;
        }
        else
        {
            bear = 360 - bear;
            bearing.text = "" + bear;
        }
    }
}
