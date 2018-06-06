using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class MyGrid : MonoBehaviour {
    // grid
    [SerializeField]
    private int rows; 
    private int colns;
    private int myrows;
    public List<Vector2> redSqures;

    [SerializeField]
    public Sprite mysprite;

    private Vector2 gridsize= new Vector2(9.9f,2.3f);

    [SerializeField]
    private Vector2 gridoffset;
    //cells

    public GameObject cellObject;
    public GameObject[] c0;

    private float resize=3.2f;

    [SerializeField]
    private Sprite cellsprite;

    private Vector2 cellsize;
    private Vector2 cellscale;
    CellControl cc;
    
    // Use this for initialization
    void Start () {
        myrows = rows;
        InitCells(rows); //initial all cells

	}
	
    void InitCells(int rows1)
    {
        if (rows1 < 1)
            return;
        colns = 5 * rows1;
        for (int i = 0; i < c0.Length; i++)
        {
            if (c0[i] != null)
            {
                Destroy(c0[i]);
            }
        }
        c0 = new GameObject[colns*rows1];
        cellObject = new GameObject();
        cellObject.AddComponent<SpriteRenderer>().sprite=cellsprite;
        cellObject.AddComponent<CellControl>();
        cellObject.GetComponent<SpriteRenderer>().sortingOrder=1;

        cellsize = cellsprite.bounds.size;
        Vector2 newcellsize = new Vector2(gridsize.x / (float)colns, gridsize.y / (float)rows1);
        cellscale.x = newcellsize.x / cellsize.x;
        cellscale.y = newcellsize.y / cellsize.y;
        cellsize = newcellsize;
        cellObject.AddComponent<BoxCollider>();
        cellObject.transform.localScale = new Vector2(cellsize.x * resize, cellsize.y * resize);
        gridoffset.x = -(gridsize.x / 2) + cellsize.x / 2;
        gridoffset.y = -(gridsize.y / 2) + cellsize.y / 2;
        for (int row = 0; row < rows1; row++)
        {
            for (int col = 0; col < colns; col++)
            {
                Vector2 pos = new Vector2(col * cellsize.x + gridoffset.x + transform.position.x, row * cellsize.y + gridoffset.y + transform.position.y);
                c0[col + row*colns] = Instantiate(cellObject, pos, Quaternion.identity) as GameObject;
                c0[col + row * colns].transform.parent = transform;
            }
        }
        Destroy(cellObject);
    }

    void OnDrawGizmo()
    {
        Gizmos.DrawWireCube(transform.position, gridsize);
    }

	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Fire3"))
        {
            List<Vector2> temp = new List<Vector2>();
            Vector3 vec3= c0[0].transform.localScale;
            float x = vec3.x / 2 - 0.1f * vec3.y;
            float y = vec3.y / 2 - 0.1f * vec3.y;
            myrows = 1+myrows ;
            InitCells(myrows);
            for(int j=0;j<c0.Length;j++)
            {
                for(int i=0; i<redSqures.Count;i++)
                {
                    if (redSqures[i].x + x > c0[j].transform.position.x)
                        if(redSqures[i].x - (x) < c0[j].transform.position.x)
                            if(redSqures[i].y + y > c0[j].transform.position.y)
                                if(redSqures[i].y - (y) < c0[j].transform.position.y)
                                {
                                    c0[j].GetComponent<SpriteRenderer>().sprite = mysprite;
                                    c0[j].GetComponent<CellControl>().flag = true;
                                    temp.Add(c0[j].transform.position);
                                }
                }
            }
            redSqures.Clear();
            redSqures = temp;
        }

        if (Input.GetButtonDown("Fire2"))
        {
            List<Vector2> temp = new List<Vector2>();
            myrows = myrows - 1 ;
            InitCells(myrows);
            Vector3 vec3 = c0[0].transform.localScale;
            float x = vec3.x / 2 - 0.1f * vec3.y;
            float y = vec3.y / 2 - 0.1f * vec3.y;
            for (int j = 0; j < c0.Length; j++)
            {
                for (int i = 0; i < redSqures.Count; i++)
                {
                    if (redSqures[i].x < c0[j].transform.position.x + x)
                        if (redSqures[i].x > c0[j].transform.position.x - x)
                            if (redSqures[i].y < c0[j].transform.position.y + y)
                                if (redSqures[i].y > c0[j].transform.position.y - y)
                                {
                                    c0[j].GetComponent<SpriteRenderer>().sprite = mysprite;
                                    c0[j].GetComponent<CellControl>().flag = true;
                                    temp.Add(c0[j].transform.position);
                                }
                }
            }
            redSqures.Clear();
            redSqures = temp;
        }

    }

    public Sprite getSprite()
    {
        return mysprite;
    }

    public Sprite getSprite2()
    {
        return cellsprite;
    }

    public List<Vector2> Getlist()
    {
        return redSqures;
    }

}
