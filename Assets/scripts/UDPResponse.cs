using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class UDPResponse : MonoBehaviour {

    [SerializeField]
    public TextMesh gt;
    public TextMesh tm = null;
    public HoloGrid hg = new HoloGrid();
    string some;
    public float answer = -1;
    public float x = 0;
    public float y = 0;
    public float z = 0;
    TextMesh g1;
    private bool isConnected;
    public string id;
    Queue<string> queue;
   /* public void Start()
    {
        string host = "127.0.0.1";
        string port = "12323";
        Debug.Log("trying to connect");
        isConnected = tcpClientHandler.Connect(host, port);
        //tcpClientHandler.Write("Hello from hololens");
        //string str = tcpClientHandler.Read();
        //tm.text = str;


    }*/

    public void write() { 
        some = "hello evrybody my name is ziv nir";
        string str = some;
        Debug.Log(str);
        g1 = Instantiate(gt, gt.transform.localPosition, gt.transform.localRotation) as TextMesh;
        g1.transform.SetParent(gt.transform, false);
        g1.transform.parent = gt.transform.parent;
        g1.transform.localPosition = new Vector3(gt.transform.localPosition.x, gt.transform.localPosition.y, 0f);
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
        g1.text = str1;

    }
#if UNITY_EDITOR  
    void Update()
    {
        
        string some = "shut up";
        //some = conn.tcpClientHandler.Read();
        //conn.tcpClientHandler.Write("welcome back");
        switch (some)
        {
            case "gfg": { break; }
            default: { break; }
                
        }
        int index = 0;
        char[] delimiterChars = { ' ', ',', '.', ':', '\t', '(', ')' };
        string[] parts = some.Split(delimiterChars);//distance: 3
        foreach (string str1 in parts)
        {
            //Debug.Log("parts["+index+"]:" + str1);
            index++;
        }
        if(parts[0] == "target")
        {
            try
            {
                x = float.Parse(parts[3]);
                y = float.Parse(parts[5]);
                z = float.Parse(parts[7]);
                hg.incomingFlag = true;
            }
            catch
            {

            }
            finally
            {

            }
        }        
        else if (parts[0] == "warning")
        {
            string str = some.Substring(9);
            Debug.Log(str);
            g1 = Instantiate(gt, gt.transform.position, gt.transform.localRotation) as TextMesh;
            g1.transform.SetParent(gt.transform, false);
            g1.transform.parent = gt.transform.parent;
            g1.transform.localPosition = new Vector3(gt.transform.localPosition.x, gt.transform.localPosition.y, 0f);
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
            g1.text = str1;
            Destroy(g1, 30);

        }
        tm.text = some;
    }
#endif    
    public void ResponseToUDPPacket(string incomingIP, string incomingPort, byte[] data)
    {

        if (tm != null)
            tm.text = System.Text.Encoding.UTF8.GetString(data);
        Debug.Log("responsetoudp");

#if !UNITY_EDITOR

        //ECHO 
        int index = 0;
        UDPCommunication comm = UDPCommunication.Instance;
        some = System.Text.Encoding.UTF8.GetString(data);
        Debug.Log(some);
        char[] delimiterChars = { ' ', ',', ':', '\t','(',')' };
        string[] parts = some.Split(delimiterChars);//distance: 3
        foreach (string str in parts)
        {
            Debug.Log("parts[" + index + "]:" + str);
            index++;
        }
        switch (parts[0])
        {

            case "distance":
            {
                try{
                    answer = float.Parse(parts[2]);
                    Debug.Log("the distance is:" + answer);
                }
                catch{
                      string message = "ERROR";
                      data = Encoding.ASCII.GetBytes(message);
                     }
                
                break;
                
            }
            case "target":
            {
                    try
                    {
                        x = float.Parse(parts[3]);
                        y = float.Parse(parts[4]);
                        z = float.Parse(parts[5]);
                        Debug.Log("(" + x + "," + y + "," + z + ")");
                        hg.incomingFlag = true;
                    }
                    catch
                    {
                        string message = "ERROR";
                        data = Encoding.ASCII.GetBytes(message);
                    }
                    break;
            }
            case "add":
            {        //add: Id 7385 azimuth 73 distance 24 alpha 32
                    //add: Id 7385 azimuth 270 distance 24 alpha 0
                    try
                    {
                        id = parts[3];
                        float angle1 = float.Parse(parts[5]);
                        float angle2 = angle1 * (Mathf.PI / 180);
                        float resultsin = Mathf.Sin(angle2);
                        float resultcos = Mathf.Cos(angle2);
                        float distance = float.Parse(parts[7]);
                        float bearing = float.Parse(parts[9]);
                        bearing = bearing * (Mathf.PI / 180);
                        float resultbea = Mathf.Sin(bearing);
                        float dis_new = distance * resultbea;
                        float resultBeaCos = Mathf.Cos(bearing);
                        float dis_xz = distance * resultBeaCos;
                        Vector3 playerDirection = Camera.main.transform.forward;
                        x =  (resultsin * dis_xz) + Camera.main.transform.position.x;
                        z =  (resultcos * dis_xz) + Camera.main.transform.position.z;
                        y = dis_new + Camera.main.transform.position.y;
                        Debug.Log("(" + x + "," + y + "," + z + ")");
                        hg.incomingFlag = true;
                    }
                    catch
                    {
                        string message = "ERROR";
                        data = Encoding.ASCII.GetBytes(message);
                    }
                    break;
            }
            case "warning":
            {
                    try
                    {
                        string str = some.Substring(9);
                        queue.Enqueue(str);
                        str = queue.Dequeue();
                        Debug.Log(str);
                        g1 = Instantiate(gt, gt.transform.position, gt.transform.localRotation) as TextMesh;
                        g1.transform.SetParent(gt.transform, false);
                        g1.transform.parent = gt.transform.parent;
                        g1.transform.localPosition = new Vector3(gt.transform.localPosition.x, gt.transform.localPosition.y, 0f);
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
                        g1.text = str1;
                        Destroy(g1, 30);
                    }
                    catch
                    {
                        string message = "ERROR";
                        data = Encoding.ASCII.GetBytes(message);
                    }
                    break;
            }
            case "delete":
            {
                    try
                    {
                        x = float.Parse(parts[3]);
                        y = float.Parse(parts[4]);
                        z = float.Parse(parts[5]);
                        Debug.Log("(" + x + "," + y + "," + z + ")");
                        hg.incomingFlag2 = true;
                    }
                    catch
                    {
                        string message = "ERROR";
                        data = Encoding.ASCII.GetBytes(message);
                    }
                    break;
            }
            case "remove":
            {
                    try
                    {
                        id = parts[3];
                        hg.incomingFlag3 = true;
                    }
                    catch
                    {
                        string message = "ERROR";
                        data = Encoding.ASCII.GetBytes(message);

                    }
                    break;
            }
            default: 
            { 
            string message = "ERROR";
            data = Encoding.ASCII.GetBytes(message);
            break; 
            }
        }        
        comm.SendUDPMessage(incomingIP, comm.externalPort, data);

#endif
    }
}