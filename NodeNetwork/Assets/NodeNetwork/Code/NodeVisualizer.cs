using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;


public class NodeVisualizer : MonoBehaviour
{
    public GameObject NodePrefab;
    public Root responceRoot;

    void Start()
    {
        //simple query to get a node I created earlyer

        try
        {
            //build request
            var wreq = WebRequest.Create("http://localhost:7474/db/data/transaction/commit");
            wreq.Method = "POST";
            wreq.Credentials = new NetworkCredential("neo4j", "asdf");

            //grab request stream so we can send some json
            var requestStream = new StreamWriter(wreq.GetRequestStream());

            var requestJson = "{\"statements\" : [ {\"statement\" : \"MATCH (n) RETURN (n)\"} ]}";
            requestStream.Write(requestJson);

            //close up the io
            requestStream.Flush();
            requestStream.Close();

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
            Debug.Log("Request Headers:\n" + wreq.Headers);
            Debug.Log("Responce Headers:\n" + wres.Headers);

            responceRoot = JsonUtility.FromJson<Root>(responseJson);
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

//responce classes
[Serializable]
public class Person
{
    public string name;
}

[Serializable]
public class Meta
{
    public int id;
    public string type;
    public bool deleted;
}

[Serializable]
public class Data
{
    public List<Person> row;
    public List<Meta> meta;
}

[Serializable]
public class Result
{
    public List<string> columns;
    public List<Data> data;
}

[Serializable]
public class Root
{
    public List<Result> results;
    public List<object> errors;
}



