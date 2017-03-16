using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;
using NeoUnity.Neo4j;

namespace NeoUnity
{
    public class NodeVisualizer : MonoBehaviour
    {
        public GameObject NodePrefab;
        public string Query;

        public string Username;
        public string Password;
        public string IP;
        public bool DebugLog = false;
        public RootObject result;

        void Start()
        {
            Server.DebugLog = DebugLog;
            Server.Username = Username;
            Server.Password = Password;
            Server.IP = IP;

            result = Server.QueryObject(Query);

            var nodes = new Dictionary<int, Node>();

            //does this query even return data
            if (result.results.Count > 0)
            {
                float x = 0;
                foreach (var graphdata in result.results[0].data)
                {
                    //loop though all neo4j nodes and make our nodes from it
                    foreach (var node in graphdata.graph.nodes)
                    {
                        var go = Instantiate(NodePrefab);
                        go.transform.position = new Vector3(x, 0, 0);
                        var n = go.GetComponent<Node>();
                        n.ID = int.Parse(node.id);
                        n.Name = node.properties.Name;
                        n.Data = node.properties.data;

                        go.name = n.ID + ":" + n.Name;

                        if (n.Name == "hurts to live")
                        {
                            Debug.Log("Ayy");

                            //System.Convert.ToBase64String()
                            var pngBytes = Convert.FromBase64String(n.Data);

                            var t = new Texture2D(1, 1);
                            t.LoadImage(pngBytes);
                            t.Apply();

                            go.GetComponent<Renderer>().material.mainTexture = t;
                        }

                        if (nodes.ContainsKey(n.ID))
                        {
                            Debug.LogError("Duplicate Node " + n.ID + ", Droping Node");
                        }
                        else
                        {
                            nodes.Add(n.ID, n);
                        }

                        x += 2.0f;
                    }
                }
            }
        }


    }
}