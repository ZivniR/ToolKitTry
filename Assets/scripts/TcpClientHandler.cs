using System;
using System.IO;
using System.Text;
using System.Threading;
using UnityEngine;

#if !UNITY_EDITOR
using System.Threading.Tasks;  
#endif

public class TcpClientHandler : MonoBehaviour
{


#if !UNITY_EDITOR
    private bool _useUWP = true;
    private Windows.Networking.Sockets.StreamSocket socket;
#endif

#if UNITY_EDITOR
    private bool _useUWP = false;
    System.Net.Sockets.TcpClient client;
    System.Net.Sockets.NetworkStream stream;
#endif

    private Byte[] bytes = new Byte[256];
    private StreamWriter writer;
    private StreamReader reader;

    private bool isConnected = false;



    /// <summary>
    ///     Needs to connect first
    /// </summary>
    /// <param name="host">Host ip</param>
    /// <param name="port">Port Number</param>
    public void Connect(string host, string port)
    {
        if (_useUWP)
        {
            ConnectUWP(host, port);
        }
        else
        {
            ConnectUnity(host, port);
        }
    }



#if UNITY_EDITOR
    private void ConnectUWP(string host, string port)
#else
    private void ConnectUWP(string host, string port)
#endif
    {
#if UNITY_EDITOR
        errorStatus = "UWP TCP client used in Unity!";

#else
        try
        {
            socket = new Windows.Networking.Sockets.StreamSocket();
            Windows.Networking.HostName serverHost = new Windows.Networking.HostName(host);
            socket.ConnectAsync(serverHost, port);

            Stream streamOut = socket.OutputStream.AsStreamForWrite();
            writer = new StreamWriter(streamOut) { AutoFlush = true };

            Stream streamIn = socket.InputStream.AsStreamForRead();
            reader = new StreamReader(streamIn);
            Debug.Log("success connected to" + host);
        }
        catch (Exception e)
        {
            errorStatus = e.ToString();
            Debug.Log("Error could not connect to" + host);
            isConnected = false;
            return ;
        }
#endif
        isConnected = true;
    }

    private bool ConnectUnity(string host, string port)
    {
#if !UNITY_EDITOR
        errorStatus = "Unity TCP client used in UWP!";
#else
        try
        {
            client = new System.Net.Sockets.TcpClient(host, Int32.Parse(port));
            stream = client.GetStream();
            reader = new StreamReader(stream);
            writer = new StreamWriter(stream) { AutoFlush = true };
            Debug.Log("success connected to" + host);
        }
        catch (Exception e)
        {
            errorStatus = e.ToString();
            Debug.Log("Error could not connect to" + host + ":" + port);
            return false;
        }
#endif
        return true;
    }

    private string errorStatus = null;

    /// <summary>
    ///     write messages to server.
    ///     Blocking.
    /// </summary>
    /// <param name="massage"></param>
    public bool Write(String massage)
    {
        try
        {
            writer.Write(massage);
            Debug.Log("Sent data!");
            return true;
        }
        catch (Exception e)
        {
            Debug.Log(e);
            return false;
        }
    }

    /// <summary>
    ///     Read messages from server.
    ///     Blocking
    /// </summary>
    /// <returns>
    ///     String of the msg
    ///     If there is no data then return null!!
    /// </returns>
    public String Read()
    {
        String received = "";
        try
        {
            received = reader.ReadLine();
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
#if !UNITY_EDITOR
        socket.Dispose();
#else
            client.Close();
#endif
            throw e;
        }
        Debug.Log("TcpClientHandler Read : " + received);
        return received;

    }

    void OnDisable()
    {
        Write("stop");
#if !UNITY_EDITOR
        if(socket != null){
            socket.Dispose();
        }
#else
        if (client != null)
        {
            client.Close();
        }
#endif
    }
}