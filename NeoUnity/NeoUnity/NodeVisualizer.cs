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

        }


    }
}