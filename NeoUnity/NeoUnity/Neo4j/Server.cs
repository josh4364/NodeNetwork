using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using UnityEngine;

namespace NeoUnity.Neo4j
{
    public static class Server
    {
        public static string Username = "neo4j";
        public static string Password = "neo4j";
        public static string IP = "localhost:7474";
        public static bool DebugLog = false;

        public static string Query(string Query)
        {
            try
            {
                //build request
                var wreq = WebRequest.Create("http://" + IP + "/db/data/transaction/commit");
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
}
