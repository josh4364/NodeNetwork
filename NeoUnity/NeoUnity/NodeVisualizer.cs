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
        public string Server;
        public bool DebugLog = false;

        void Start()
        {
            Neo4j.Server.DebugLog = DebugLog;
            Neo4j.Server.Username = Username;
            Neo4j.Server.Password = Password;
            Neo4j.Server.IP = Server;

            Neo4j.Server.Query(Query);

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