using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connection : MonoBehaviour {
    [SerializeField]
    public TcpClientHandler tcpClientHandler;
    [SerializeField]
    string ip = "127.0.0.1";
    [SerializeField]
    string socket = "12345";
    // Use this for initialization
    void Start () {
        tcpClientHandler.Connect(ip, socket);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
