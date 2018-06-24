using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web;
using UnityEngine;

public class IpRequest : MonoBehaviour {
    string url = "https://reconsevice.herokuapp.com/reconunit/1";
    [SerializeField]
    string host;
    [SerializeField]
    UDPCommunication udpc;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
       /* IPHostEntry hostEntry;

        hostEntry = Dns.GetHostEntry(host);

        //you might get more than one ip for a hostname since 
        //DNS supports more than one record
        int num = hostEntry.AddressList.Length;
        for (int i=0;i< hostEntry.AddressList.Length;i++)
        {
            IPAddress ipa = hostEntry.AddressList[i];
            if ((!hostEntry.AddressList[i].IsIPv6LinkLocal) && (!hostEntry.AddressList[i].IsIPv6Multicast) && (!hostEntry.AddressList[i].IsIPv6SiteLocal))
            {
                var ip = hostEntry.AddressList[i];

                //Debug.Log("number[" + i+"]:"+ ip);
                //Debug.Log(hostEntry.AddressList.Length);
            }
        }
        Debug.Log(hostEntry.AddressList[num - 1].ToString());
        udpc.externalIP = hostEntry.AddressList[num - 1].ToString();
        Debug.Log(udpc.externalIP);*/ 
    }

}


