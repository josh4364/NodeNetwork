using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using UnityEngine;

namespace NeoUnity
{
    public static class Neo4j
    {
        public static string Username = "neo4j";
        public static string Password = "neo4j";
        public static string Server = "localhost:7474";
        public static bool DebugLog = false;

        public static string Query(string Query)
        {
            try
            {
                //build request
                var wreq = WebRequest.Create("http://" + Server + "/db/data/transaction/commit");
                wreq.Method = "POST";
                wreq.Credentials = new NetworkCredential(Username, Password);
                
                //grab request stream so we can send some json
                var requestStream = new StreamWriter(wreq.GetRequestStream());
                requestStream.Write("{\"statements\" : [ { \"statement\" : \"" + Query + "\", \"resultDataContents\" : [ \"graph\" ] } ]}");

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

                if (DebugLog)
                {
                    Debug.Log("Request Headers:\n" + wreq.Headers);
                    Debug.Log("Responce Headers:\n" + wres.Headers);
                    Debug.Log("Responce Json:" + responseJson);
                }

                //return JsonUtility.FromJson<Root>(responseJson);
                return responseJson;
            }
            catch (WebException webex)
            {
                Debug.LogError("Was not able to run query against neo4j.\nReason:" + webex.Message);

                return "{}";
            }
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

}
