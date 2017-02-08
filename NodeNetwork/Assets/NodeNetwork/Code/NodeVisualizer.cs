using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;


public class NodeVisualizer : MonoBehaviour
{
    public GameObject NodePrefab;
    public string requestJson;
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

            //var requestJson = "{\"statements\" : [ {\"statement\" : \"MATCH (n) RETURN (n)\"} ]}";
            requestStream.Write("{\"statements\" : [ { \"statement\" : \"" + requestJson + "\", \"resultDataContents\" : [ \"graph\" ] } ]}");

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

            return;
        }


        //create a game node to represent
        var go = Instantiate(NodePrefab);
        go.transform.position = Vector3.zero;

        var data = responceRoot.results[0].data;

        var sb = new StringBuilder();

        for (int i = 0; i < data.Count; i++)
        {
            sb.AppendLine("Data " + i + ":");

            for (int r = 0; r < data[i].row.Count; r++)
            {
                sb.AppendLine("    Row:" + r + ", " + data[i].row[r].name);
            }
        }

        Debug.Log(sb);
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
public class NodeProperties
{
    public string name;
}

[Serializable]
public class Node
{
    public string id;
    public List<string> labels;
    public NodeProperties properties;
}

[Serializable]
public class Graph
{
    public List<Node> nodes;
    public List<Relationship> relationships;
}

[Serializable]
public class RelationshipProperties
{
    public string Type;
}

[Serializable]
public class Relationship
{
    public string id;
    public string type;
    public string startNode;
    public string endNode;
    public RelationshipProperties properties;
}

[Serializable]
public class Data
{
    public List<Person> row;
    public Graph graph;
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
}
