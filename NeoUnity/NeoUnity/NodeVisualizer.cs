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
        public GameObject RelationshipPrefab;
        public GameObject NodePrefab;
        public string Query;

        public string Username;
        public string Password;
        public string IP;
        public bool DebugLog = false;
        public RootObject result;
        public Dictionary<int, Node> nodes = new Dictionary<int, Node>();
        public Dictionary<int, Relationship> rels = new Dictionary<int, Relationship>();

        public void SpawnWorldNodes(List<GraphData> data)
        {
            //Clear nodes
            if (nodes == null)
                nodes = new Dictionary<int, Node>();

            foreach (var item in nodes)
            {
                DestroyImmediate(item.Value.gameObject);
            }
            nodes.Clear();

            //Clear relationships
            if (rels == null)
                rels = new Dictionary<int, Relationship>();

            foreach (var item in rels)
            {
                DestroyImmediate(item.Value.gameObject);
            }
            rels.Clear();




            float x = 0;
            foreach (var graphdata in data)
            {
                //loop though all neo4j nodes and make our nodes from it
                foreach (var node in graphdata.graph.nodes)
                {
                    if (nodes.ContainsKey(int.Parse(node.id)))
                    {
                        //Debug.LogError("Duplicate Node " + int.Parse(node.id) + ", Droping Node");
                    }
                    else
                    {
                        var go = Instantiate(NodePrefab);
                        go.transform.position = new Vector3(x, 0, 0);
                        var n = go.GetComponent<Node>();
                        n.ID = int.Parse(node.id);
                        n.Name = node.properties.Name;
                        n.Data = node.properties.data;

                        go.tag = "Node";

                        go.name = n.ID + ":" + n.Name;


                        nodes.Add(n.ID, n);
                        //x += 2.0f;
                    }

                }
                foreach (var rel in graphdata.graph.relationships)
                {
                    if (!rels.ContainsKey(int.Parse(rel.id)))
                    {
                        var gorel = Instantiate(RelationshipPrefab);
                        gorel.transform.position = new Vector3(0, 0, 0);
                        var r = gorel.GetComponent<Relationship>();
                        r.Node1 = nodes[int.Parse(rel.startNode)];
                        r.Node2 = nodes[int.Parse(rel.endNode)];
                        r.RelationshipType = rel.type;
                        r.Properties = rel.properties;

                        rels.Add(int.Parse(rel.id), r);


                    }

                }

            }
        }

        void Awake()
        {
            Singleton = this;
            Server.DebugLog = DebugLog;
            Server.Username = Username;
            Server.Password = Password;
            Server.IP = IP;
        }

    }
}