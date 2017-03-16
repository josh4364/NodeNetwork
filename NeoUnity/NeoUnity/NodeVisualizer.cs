using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;

namespace NeoUnity
{
    public class NodeVisualizer : MonoBehaviour
    {
        public GameObject NodePrefab;
        public string Query;

        public string Username;
        public string Password;
        public string Server;
        public bool DebugLog = false;

        void Start()
        {
            Neo4j.DebugLog = DebugLog;
            Neo4j.Username = Username;
            Neo4j.Password = Password;
            Neo4j.Server = Server;

            Neo4j.Query(Query);

            var nodes = new Dictionary<int, Node>();
            nodes.Add(0, new Node
            {
                Name = "Computers"
            });
            nodes.Add(1, new Node
            {
                Name = "Programing"
            });
            nodes.Add(2, new Node
            {
                Name = "Programing Language"
            });
            nodes.Add(3, new Node
            {
                Name = "C#"
            });
            nodes.Add(4, new Node
            {
                Name = "C++"
            });
            nodes.Add(5, new Node
            {
                Name = "Java"
            });
            nodes.Add(6, new Node
            {
                Name = "Lua"
            });
            nodes.Add(7, new Node
            {
                Name = "python"
            });
        }


    }
}