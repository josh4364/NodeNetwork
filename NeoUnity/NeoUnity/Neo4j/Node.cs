using System.Collections.Generic;

namespace NeoUnity.Neo4j
{
    [Serializable]
    public class Node
    {
        public string id { get; set; }
        public List<string> labels { get; set; }
        public NodeProperties properties { get; set; }
    }
}
