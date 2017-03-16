using System;
using System.Collections.Generic;

namespace NeoUnity.Neo4j
{
    [Serializable]
    public class Result
    {
        public List<string> columns { get; set; }
        public List<GraphData> data { get; set; }
    }
}
