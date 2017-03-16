using System;

namespace NeoUnity.Neo4j
{
    [Serializable]
    public class Relationship
    {
        public string id { get; set; }
        public string type { get; set; }
        public string startNode { get; set; }
        public string endNode { get; set; }
        public RelationshipProperties properties { get; set; }
    }
}
