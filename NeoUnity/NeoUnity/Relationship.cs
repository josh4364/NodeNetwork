using UnityEngine;
namespace NeoUnity
{
    public class Relationship : MonoBehaviour
    {
        public LineRenderer LR;
        public string RelationshipType;
        public Node Node1;
        public Node Node2;
        public Neo4j.RelationshipProperties Properties;

        void Start()
        {
            LR = this.GetComponent<LineRenderer>();
        }

        void Update()
        {
            LR.SetPositions(new Vector3[] {Node1.transform.position, Node2.transform.position });
        }
    }
}
