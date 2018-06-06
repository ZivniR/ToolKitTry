using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR.WSA.Input;

public class CellControl : MonoBehaviour {
    public MyGrid gr;
    public bool flag;
    public Sprite sprite1;
    public Sprite sprite2;
    GestureRecognizer recognizer;

    void start()
    {
        recognizer = new GestureRecognizer();
        recognizer.SetRecognizableGestures(GestureSettings.Tap);
        recognizer.TappedEvent += Recognizer_TappedEvent;
        recognizer.StartCapturingGestures();
        flag = false;
        sprite1 = Resources.Load("target", typeof(Sprite)) as Sprite;
        sprite2 = Resources.Load("white", typeof(Sprite)) as Sprite;
    }

    private void Recognizer_TappedEvent(InteractionSourceKind source, int tapCount, Ray headRay)
    {
        GameObject go = GameObject.Find("GameObject");
        HoloGrid other = (HoloGrid)go.GetComponent(typeof(HoloGrid));
        //List<Vector2> redSqures = other.Getlist();
        if (flag == false)
        {
            Sprite mysprite = other.getSprite(flag);
            this.GetComponent<SpriteRenderer>().sprite = mysprite;
            this.GetComponent<SpriteRenderer>().sprite = sprite1;
            //redSqures.Add(this.transform.position);
            flag = true;
        }
        else
        {
            Sprite mysprite = other.getSprite2();
            this.GetComponent<SpriteRenderer>().sprite = mysprite;
            this.GetComponent<SpriteRenderer>().sprite = sprite2;
            //redSqures.Remove(this.transform.position);
            flag = false;
        }
    }

  


    /*void OnMouseOver()
    {
        //If your mouse hovers over the GameObject with the script attached, output this message
        Debug.Log("Mouse is over GameObject.");
        GameObject go = GameObject.Find("GameObject");
        HoloGrid other = (HoloGrid)go.GetComponent(typeof(HoloGrid));
        List<Vector2> redSqures = other.Getlist();
        
        if (Input.GetMouseButtonDown(0))
        {
            if (flag == false)
            {
                Sprite mysprite = other.getSprite();
                this.GetComponent<SpriteRenderer>().sprite = mysprite;
                redSqures.Add(this.transform.position);
                flag = true;
            }
            else
            {
                Sprite mysprite = other.getSprite2();
                this.GetComponent<SpriteRenderer>().sprite = mysprite;
                redSqures.Remove(this.transform.position);
                flag = false;
            }
        }

    }

    void OnMouseExit()
    {
        //The mouse is no longer hovering over the GameObject so output this message each frame
        Debug.Log("Mouse is no longer on GameObject.");
    }*/
}
