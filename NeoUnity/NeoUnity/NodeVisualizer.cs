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
        public static NodeVisualizer Singleton;
        public GameObject NodePrefab;
        public string Query;

        public string Username;
        public string Password;
        public string IP;
        public bool DebugLog = false;
        public RootObject result;


        public void SpawnWorldNodes(List<GraphData> data)
        {
            var nodes = new Dictionary<int, Node>();

            float x = 0;
            foreach (var graphdata in data)
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

                    go.tag = "Node";

                    go.name = n.ID + ":" + n.Name;

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

        void Awake()
        {
            Singleton = this;
        }

        void Start()
        {
            Server.DebugLog = DebugLog;
            Server.Username = Username;
            Server.Password = Password;
            Server.IP = IP;
        }


    }
}