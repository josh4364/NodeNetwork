using System;
using System.IO;
using System.Net;
using UnityEngine;


public class NodeVisualizer : MonoBehaviour
{
    public GameObject NodePrefab;

    void Start()
    {
        //simple query to get a node I created earlyer

        try
        {
            //build request
            var wreq = WebRequest.Create("http://localhost:7474/db/data/");
            wreq.Method = "GET";
            wreq.Credentials = new NetworkCredential("neo4j", "asdf");
            wreq.Headers[""] = "";

            //get response
            var wres = wreq.GetResponse();
            //convert the raw stream into a string readable one
            var stream = wres.GetResponseStream();
            var streamReader = new StreamReader(stream);
            var responseJson = streamReader.ReadToEnd();

            //close both io
            streamReader.Close();
            stream.Close();

            Debug.Log(responseJson);
        }
        catch (WebException webex)
        {
            Debug.Log(webex.Message);
        }
        //create a game node to represent
        var go = Instantiate(NodePrefab);
        go.transform.position = Vector3.zero;

    }


}