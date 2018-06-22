using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.VR.WSA.Input;
using UnityEngine.VR.WSA.Persistence;
using UnityEngine.VR.WSA;
using HoloToolkit.Unity;

public class HoloGrid : MonoBehaviour {

    // grid
    [SerializeField]
    public int rows;
    public int colns;
    public int myrows;
    public List<GameObject> prefabList;
    public GameObject go;
    public List<TargetClass> TargetList;
    public float Xmovement = 0.08f;
    public Quaternion roat;
    int indexer = 1;
    public float Ymovement = 0.06f;
    [SerializeField]
    public Sprite mysprite;
    private bool flag;
    [SerializeField]
    CompassText ct;
    private Vector2 gridsize = new Vector2(5.8f, 3.2f);

    [SerializeField]
    private Vector2 gridoffset;
    //cells

    public GameObject cellObject;
    public GameObject[] c0;

    private float resize = 3.2f;

    [SerializeField]
    private Sprite cellsprite;
    GameObject g1;
    private Vector2 cellsize;
    private Vector2 cellscale;
    GestureRecognizer recognizer;
    [SerializeField]
    public HoloToolkit.Unity.InputModule.GazeManager gm;
    [SerializeField]
    UDPResponse udpr;
    [SerializeField]
    float timeLeft = 30f;

    [SerializeField]
    public UDPCommunication udpc;
    public float result = 12;
    public bool incomingFlag = false;
    public bool incomingFlag2 = false;
    public bool incomingFlag3 = false;
    Vector3 closest;
    double MinDistance;
    Camera mainCamera;
    Vector3[] places;
    [SerializeField]
    GameObject left;
    [SerializeField]
    GameObject right;
    [SerializeField]
    GameObject down;
    [SerializeField]
    GameObject up;
    TextMesh g2=null;
    [SerializeField]
    TextMesh gt;
    public List<String> warninglist;
    [SerializeField]
    public Sprite flesh;
    [SerializeField]
    GameObject yell;
    int ans;
    // Use this for initialization
    void Start()
    {
        Debug.Log("start");
        ans = 0;
        prefabList = new List<GameObject>();
        roat = new Quaternion();
        this.transform.localScale = gridsize;
        myrows = rows;
        InitCells(rows); //initial all cells
        recognizer = new GestureRecognizer();
        recognizer.SetRecognizableGestures(GestureSettings.Tap);
        recognizer.TappedEvent += Recognizer_TappedEvent;
        recognizer.StartCapturingGestures();
        MinDistance = 9999;
        Camera mainCamera = Camera.main;
        //TargetList.Add(new TargetClass("1", Instantiate(go, new Vector3(1335.34f, 3423.54f, 12330.323f), Quaternion.identity)));
    }

    public void ShotLizer()// when a soldier make gesture to mark targets and ask for lizer   
    {
        string message;
        int number = (colns * myrows) / 2;
        if (c0[number].GetComponent<SpriteRenderer>().sprite == mysprite)
        {
             message = "delete target!";
        }
        else // send azimout and elv to maek target
        {
            message = "mark elv " + System.Math.Round(ct.bear, 2) + " azimuth "+ System.Math.Round(ct.degree, 2);
            byte[] bytes = Encoding.ASCII.GetBytes(message);
            udpc.SendUDPMessage(udpc.externalIP, udpc.externalPort, bytes);
        }

        
    }

    public void CheckIncomeTargets()// when Soldier get incoming target add the new target to the list 
    {
        Debug.Log("CheckIncomeTargets()");
        Quaternion playerRotation = Camera.main.transform.rotation;
        Vector3 spawnPos = new Vector3(udpr.x, udpr.y, udpr.z);
        string id = udpr.id;
        bool exsist = false;
        for (int i = 0; i < TargetList.Count; i++)
        {
            if (TargetList[i].id == id)
                exsist = true;
        }
        if (exsist == false)
        {
            g1 = Instantiate(go, spawnPos, playerRotation);
            prefabList.Add(g1);
            TargetList.Add(new TargetClass(id, g1));
        }
        incomingFlag = false;
    }

    public void DeleteById() // delete target after receving target id
    {
        string id = udpr.id;
        for (int i = 0; i < TargetList.Count; i++)
        {
            if (TargetList[i].id == id)
            {
                Debug.Log("try to destory");
                Destroy(TargetList[i].targetobject);
                TargetList.RemoveAt(i);
                incomingFlag3 = false;
                break;
            }
        }
    }

    public void DeleteTarget()// delete atarget by position **not in use**
    {
        double mindist = 9999999999999999999;
        Vector3 closer = new Vector3(); 
        Vector3 vec3 = new Vector3(udpr.x, udpr.y, udpr.z);
        for (int q = 0; q < TargetList.Count; q++)
        {
            Vector3 temp = TargetList[q].transform.position;
            double dist = Vector3.Distance(vec3, temp);
            if (dist < mindist)
            {
                mindist = dist;
                closer = temp;
            }
        }
        for (int q = 0; q < TargetList.Count; q++)
        {
            if(closer == TargetList[q].transform.position)
            {
                TargetList.RemoveAt(q);
                incomingFlag2 = false;
                break;
            }
        }


    }

    public void WriterWarning()
    {
        String str="";
        if (warninglist.Count > 0)
        {
            str = warninglist[0];
            warninglist.RemoveAt(0);
            g2 = (TextMesh)Instantiate(gt, gt.transform.position, gt.transform.localRotation) as TextMesh;
            g2.transform.SetParent(gt.transform, false);
            g2.transform.parent = gt.transform.parent;
            g2.transform.localPosition = new Vector3(gt.transform.localPosition.x, gt.transform.localPosition.y, 0f);
            string[] parts1 = str.Split(' ');
            string str1 = "";
            int width = 0;
            for (int i = 0; i < parts1.Length; i++)
            {
                str1 = str1 + " " + parts1[i];
                width += parts1[i].Length + 1;
                if (width > 15)
                {
                    str1 = str1 + '\n';
                    width = 0;
                }
            }
            g2.text = str1;
            Destroy(g2, 30);
        }
    }

    public int OnAirTapped()// after Soldier make gesture
    {
        yell = new GameObject();
        yell.AddComponent<SpriteRenderer>().sprite = flesh;
        int number1 = (colns * myrows) / 2;
        Debug.Log(yell.GetInstanceID());
        yell.transform.localPosition= c0[number1].transform.localPosition;
        yell.transform.position = c0[number1].transform.position;
        yell.transform.parent = c0[number1].transform.parent;
        yell.transform.rotation = c0[number1].transform.rotation;
        yell.transform.localScale = c0[number1].transform.localScale;
        Destroy(yell, 0.5f);
        GameObject tempgo = null; 
        Debug.Log("OnAirTapped");
        flag = !flag;
        Vector3 vec3 = gm.HitPosition;
        for (int i = 0; i < c0.Length; i++)// return the place in the real world where the marking was used
        {
            Vector3 temp = c0[i].transform.position;
            double dist = Vector3.Distance(vec3, temp);
            if (dist < MinDistance)
            {
                MinDistance = dist;
                closest = temp;
            }
        }
        for (int i = 0; i < c0.Length; i++)
        {
            if (c0[i].transform.position == closest)
            {
                MinDistance = 9999999;
                int number = (colns * myrows) / 2;
                if (c0[number].GetComponent<SpriteRenderer>().sprite == mysprite)// check if the center square is red (target marked) or if its in origin color (target not marked)
                {//make gesture when the center is marked and the Soldier want to delete target 
                    Vector3 closer= new Vector3();
                    double distance;
                    double mindist = 9999999;
                    TargetClass tc = new TargetClass();
                    foreach (TargetClass red in TargetList)
                    {
                        if (red.targetobject.GetComponent<MeshRenderer>().isVisible)// if the trarget is visible to camera 
                        {
                            distance = Vector3.Distance(red.targetobject.transform.position, c0[i].transform.position);
                            if (distance < mindist)
                            {
                                closer = red.targetobject.transform.position;
                                mindist = distance;
                                tempgo = red.targetobject;
                                tc = red;
                            }
                        }
                    }
                    //send to Pi request to delete target
                    string message = "remove id " + tc.id;
                    byte[] bytes = Encoding.ASCII.GetBytes(message);
                    udpc.SendUDPMessage(udpc.externalIP, udpc.externalPort, bytes);
                    TargetList.Remove(tc);
                    Destroy(tempgo);
                    c0[i].GetComponent<SpriteRenderer>().sprite = cellsprite;
                }
                else
                {

                    Vector3 playerPos = Camera.main.transform.position;
                    Vector3 playerDirection = Camera.main.transform.forward;
                    Quaternion playerRotation = Camera.main.transform.rotation;
                    float spawnDistance;
                    result =udpr.answer;
                    if (result != -1)// if we have just lizer with out a PI **mybe will be in use in the future**
                    {
                        Debug.Log("result hg:" + result);
                        spawnDistance = result;
                        result = -1;
                        Vector3 spawnPos = playerPos + playerDirection * spawnDistance;
                        g1 = Instantiate(go, spawnPos, playerRotation);
                        TargetList.Add(new TargetClass(indexer.ToString(),g1));
                        indexer++;
                        int somenum = 0;
                        for (int z = 0; z < TargetList.Count; z++)
                        {
                            if (TargetList[z].targetobject == g1)
                            {
                                somenum = z;
                                Debug.Log("found gameobject:" + z);
                            }
                        }
                    }
                    string message = "target:" + "(" + g1.transform.position.x + "," + g1.transform.position.y + "," + g1.transform.position.y + ")";
                    byte[] bytes = Encoding.ASCII.GetBytes(message);
                    udpc.SendUDPMessage(udpc.externalIP, udpc.externalPort, bytes);
                    g1.transform.parent = null;
                    c0[i].GetComponent<SpriteRenderer>().sprite = mysprite;
                }
                return i;
            }
        }
        return -1;
    } 

    private void Recognizer_TappedEvent(InteractionSourceKind source, int tapCount, Ray headRay)// recognize the gesture
    {
        ShotLizer();
        int index = OnAirTapped();
        //c0[index].GetComponent<SpriteRenderer>().sprite = getSprite(c0[index]);
    }

    void InitCells(int rows1)// create the grid
    {
        Debug.Log("InitCells");
        if (rows1 < 1)
        {
            myrows = 1;
            return;
        }
        colns = 3 * rows1;
        for (int i = 0; i < c0.Length; i++)
        {
            if (c0[i] != null)
            {
                Destroy(c0[i]);
            }
        }
        c0 = new GameObject[colns * rows1];
        places = new Vector3[colns * rows1];
        cellObject = new GameObject();
        cellObject.AddComponent<SpriteRenderer>().sprite = cellsprite;
        cellObject.AddComponent<CellControl>();
        //cellObject.GetComponent<SpriteRenderer>().sortingOrder = 32766;

        cellsize = cellsprite.bounds.size;
        Vector2 newcellsize = new Vector2(gridsize.x / (float)colns, gridsize.y / (float)rows1);
        cellscale.x = newcellsize.x / cellsize.x;
        cellscale.y = newcellsize.y / cellsize.y;
        cellsize = newcellsize;
        cellObject.AddComponent<BoxCollider>();
        cellObject.transform.localScale = new Vector2(cellsize.x * resize, cellsize.y * resize);
        gridoffset.x = -(gridsize.x / 2) + cellsize.x / 2;
        gridoffset.y = -(gridsize.y / 2) + cellsize.y / 2;
        float Xoffset = 0;
        float Yoffset = 0;
        for (int row = 0; row < rows1; row++)
        {
            Xoffset = 100;
            Xoffset = (Xoffset / rows1)*row;
            for (int col = 0; col < colns; col++)
            {
                Yoffset = 100;
                Yoffset = (Yoffset / colns) * col;
                Vector2 pos = new Vector2(col * cellsize.x + gridoffset.x + transform.position.x, row * cellsize.y + gridoffset.y + transform.position.y);
                c0[col + row * colns] = Instantiate(cellObject, pos, Camera.main.transform.rotation) as GameObject;
                c0[col + row * colns].transform.parent = transform;
                float x = -100;
                x = x - (rows1 - 1) * 150;
                x /= colns;
                float y = -50;
                y = y * (rows1 - 1);
                y /= rows1;
                c0[col + row * colns].transform.localPosition = new Vector3(x + Yoffset, y + Xoffset, c0[col + row * colns].transform.localPosition.z);
                places[col + row * colns] = Camera.main.WorldToScreenPoint(c0[col + row * colns].transform.position);
                Debug.Log(places[col + row * colns]);
            }
        }
        Destroy(cellObject);
    }

    void OnDrawGizmo()
    {
        Gizmos.DrawWireCube(transform.position, gridsize);
    }

    /*change from red squre to prefab list to see the if it works go should be an anchor */
    // Update is called once per frame
    void Update()
    {         
        Camera mainCamera = Camera.main;
        int visiblecount = 0;
        Vector3 closest3;
        for (int j = 0; j < TargetList.Count; j++)
        {
            float Zaxis = TargetList[j].targetobject.transform.position.z;
            Vector3 vec3 = TargetList[j].targetobject.transform.position;
            if (!TargetList[j].targetobject.GetComponent<MeshRenderer>().isVisible)//if target is not visible to the camera than put arrow in the right direction
            {
                visiblecount++;
                if (visiblecount == TargetList.Count)
                {
                    for (int q = 0; q < c0.Length; q++)
                    {
                        c0[q].GetComponent<SpriteRenderer>().sprite = cellsprite;
                    }

                    double mindist = 9999999999999999999;
                    closest3 = new Vector3();
                    for (int p = 0; p < c0.Length; p++)
                    {
                        Vector3 temp = c0[p].transform.position;
                        double dist = Vector3.Distance(vec3,temp);
                        if (dist < mindist)
                        {
                            mindist = dist;
                            closest3 = temp;
                        }
                    }

                    for (int i = 0; i < c0.Length; i++)//the arrow mechanism
                    {
                        if (closest3 == c0[i].transform.position)
                        {
                            if (i % colns == 0)
                            {
                                left.GetComponent<SpriteRenderer>().enabled = true;
                                right.GetComponent<SpriteRenderer>().enabled = false;
                            }
                            else if (i % colns == colns - 1)
                            {
                                left.GetComponent<SpriteRenderer>().enabled = false;
                                right.GetComponent<SpriteRenderer>().enabled = true;
                            }
                            else
                            {
                                left.GetComponent<SpriteRenderer>().enabled = false;
                                right.GetComponent<SpriteRenderer>().enabled = false;
                            }
                            if (i < colns)
                            {
                                up.GetComponent<SpriteRenderer>().enabled = false;
                                down.GetComponent<SpriteRenderer>().enabled = true;
                            }
                            else if(i >c0.Length-colns)
                            {
                                up.GetComponent<SpriteRenderer>().enabled = true;
                                down.GetComponent<SpriteRenderer>().enabled = false;
                            }
                            else
                            {
                                up.GetComponent<SpriteRenderer>().enabled = false;
                                down.GetComponent<SpriteRenderer>().enabled = false;
                            }
                        }

                    }
                    //continue;
                }
            }
            else// if there target in the camera filed of view and we are tarcking the target
            {
                left.GetComponent<SpriteRenderer>().enabled = false;
                right.GetComponent<SpriteRenderer>().enabled = false;
                up.GetComponent<SpriteRenderer>().enabled = false;
                down.GetComponent<SpriteRenderer>().enabled = false;
                double mindist = 9999999999999999999;
                closest = new Vector3();
                Vector2 vec31 = mainCamera.WorldToScreenPoint(vec3);
                for (int q = 0; q < c0.Length; q++)
                {
                    Vector3 temp = c0[q].transform.position;
                    double dist = Vector3.Distance(vec31,places[q]);
                    Debug.Log(dist);
                    if (dist < mindist)
                    {
                        mindist = dist;
                        closest = temp;
                    }
                }

                for (int i = 0; i < c0.Length; i++)
                {
                    if (closest == c0[i].transform.position)
                    {
                        c0[i].GetComponent<SpriteRenderer>().sprite = mysprite;
                        for (int q = 0; q < c0.Length; q++)
                        {
                            if (q != i)
                                c0[q].GetComponent<SpriteRenderer>().sprite = cellsprite;
                        }
                    }
                }
            }
            
        }

        if (TargetList.Count==0)
        {
            for(int i=0;i<c0.Length;i++)
                c0[i].GetComponent<SpriteRenderer>().sprite = cellsprite;
            left.GetComponent<SpriteRenderer>().enabled = false;
            right.GetComponent<SpriteRenderer>().enabled = false;
            up.GetComponent<SpriteRenderer>().enabled = false;
            down.GetComponent<SpriteRenderer>().enabled = false;

        }
        if (incomingFlag == true)
            CheckIncomeTargets();
        if (incomingFlag2 == true)
            DeleteTarget();
        if (incomingFlag3 == true)
            DeleteById();
        if (warninglist.Count > 0)
            if (g2 == null)
                WriterWarning();
    }

    public Sprite getSprite(bool _flag)
    {
        Debug.Log("getSprite");
        if (_flag == false)
            return mysprite;
        else
            return cellsprite;
    }

    public Sprite getSprite(GameObject c)
    {
        Debug.Log("getSprite");
        if (c.GetComponent<SpriteRenderer>().sprite == cellsprite)
            return mysprite;
        else
            return cellsprite;
    }


    public Sprite getSprite2()
    {
        Debug.Log("getSprite2");
        return cellsprite;
    }

    public void IncreaseGrid()//Increase the size Grid
    {
        Debug.Log("IncreaseGrid");
        myrows = myrows + 1;
        InitCells(myrows);
    }

    public void ReductionGrid()//Reduction the size of the Grid
    {
        Debug.Log("ReductionGrid");
        myrows = myrows - 1;
        InitCells(myrows);
    }

    public void GridOff()// turn off the grid
    {
     for(int i = 0; i < c0.Length; i++)
        {
            c0[i].GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    public void GridOn()// turn on the grid
    {
        for (int i = 0; i < c0.Length; i++)
        {
            c0[i].GetComponent<SpriteRenderer>().enabled = true;
            c0[i].GetComponent<SpriteRenderer>().sprite = cellsprite;
        }
    }
}