using NeoUnity;
using NeoUnity.Neo4j;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject LeftMenuPanel;
    public GameObject SelectedObject;
    public GameObject NodeCardPrefab;

    [Header("Query")]
    public Text QueryText;

    [Header("Remember")]
    public Text RememberText;
    public Text RememberDataText;
    public Text RememberRelatedText;

    [Header("Forget")]
    public Text ForgetText;

    private const float _tSpeed = 100;
    private bool _leftMenu;
    private bool _leftMenuMoving;


    private GameObject _nodeCard;


    void Start()
    {

        RecallEverything();
    }



    public void ForgetEverything()
    {
        Server.Query("MATCH (n:Unity) DETACH DELETE (n)");

        RecallEverything();
    }

    public void Forget()
    {
        Server.Query("MATCH (n:Unity {Name:'" + ForgetText.text + "'}) DETACH DELETE (n)");
        RecallEverything();
    }

    public void Remember()
    {
        if (RememberRelatedText.text != "")
        {
            Server.Query("merge (a:Unity{Name:'" + RememberText.text + "'}) set a.data = '" + RememberDataText.text + "' merge (b:Unity{Name:'" + RememberRelatedText.text + "'}) merge (a) -[r:Rel]-> (b) return a, r, b");
        }
        else
        {
            string s = "MERGE (n:Unity{Name:'" + RememberText.text + "'}) SET n.data = '" + RememberDataText.text + "' RETURN n";
            Server.Query(s);
        }
        RecallEverything();
    }

    void Update()
    {
        if (_leftMenuMoving)
        {
            RectTransform rt = LeftMenuPanel.GetComponent<RectTransform>();
            if (_leftMenu)
            {
                if (rt.position.x < -220)
                {
                    _leftMenuMoving = false;
                    _leftMenu = false;
                    rt.position = new Vector3(-220, Screen.height, 0);
                }
                else
                {
                    rt.position -= new Vector3(_tSpeed, 0, 0) * Time.deltaTime;
                }
            }
            else
            {
                if (rt.position.x >= 0)
                {
                    _leftMenuMoving = false;
                    _leftMenu = true;
                    rt.position = new Vector3(0, Screen.height, 0);
                }
                else
                {
                    rt.position += new Vector3(_tSpeed, 0, 0) * Time.deltaTime;
                }
            }
        }

        if (Input.GetMouseButton(0))
        {
            Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D rh = Physics2D.GetRayIntersection(r, Mathf.Infinity);
            
            if (rh)
            {
                if (rh.collider.tag == "Node")
                {
                    SelectedObject = rh.collider.gameObject;
                    if (!_nodeCard)
                    {
                        _nodeCard = Instantiate(NodeCardPrefab);
                        _nodeCard.transform.parent = SelectedObject.transform;
                        _nodeCard.transform.localPosition = new Vector3(1, 1);

                        NeoUnity.Node n = SelectedObject.GetComponent<NeoUnity.Node>();


                        _nodeCard.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>().text = "Name: "+n.Name + "\nData: " + n.Data;
                    }

                    if (_nodeCard.transform.parent != SelectedObject.transform)
                    {
                        DestroyImmediate(_nodeCard);
                    }
                }
            }
            else
            {
                DestroyImmediate(_nodeCard);
            }
        }

        if (SelectedObject)
        {

            SelectedObject.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, 10));
        }

        if (Input.GetMouseButtonUp(0))
        {
            SelectedObject = null;
        }
        if (Input.GetMouseButton(1))
        {
            float x = -Input.GetAxis("Mouse X");
            float y = -Input.GetAxis("Mouse Y");
            this.transform.position += new Vector3(x, y) * (Zoom / MaxZoom) * PanMult;
        }

        Zoom = Mathf.Clamp(Zoom - (Input.GetAxis("Mouse ScrollWheel") * (Zoom / MaxZoom) * ZoomSpeed), MinZoom, MaxZoom);
        Camera.main.orthographicSize = Zoom;

    }
    public float Zoom = 8;
    public float MinZoom = 1;
    public float MaxZoom = 100;
    public float ZoomSpeed = 20f;
    public float PanMult = 2f;

    void OnDrawGizmos()
    {
        Vector3 p = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, 10));
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(p, 1);
    }
    public void Recall()
    {
        if (QueryText.text == "*")
        {

            RootObject o = Server.QueryObject("MATCH(all:Unity) MATCH (n:Unity) -[r:Rel]- (b:Unity) return all,n,r");
            if (o?.results.Count > 0)
                NodeVisualizer.Singleton.SpawnWorldNodes(o?.results[0]?.data);
        }
        else
        {
            RootObject o = Server.QueryObject("MATCH(all:Unity) MATCH (n:Unity{Name:'" + QueryText.text + "'}) -[r:Rel]- (b:Unity) return all, n,r,b");
            if (o?.results.Count > 0)
                NodeVisualizer.Singleton.SpawnWorldNodes(o?.results[0]?.data);

        }
    }

    public void RecallEverything()
    {
        RootObject o = Server.QueryObject("MATCH(all:Unity) MATCH (n:Unity) -[r:Rel]- (b:Unity) return all,n,r");
        if (o?.results.Count > 0)
            NodeVisualizer.Singleton.SpawnWorldNodes(o?.results[0]?.data);
    }
    public void ToggleMenu()
    {
        if (!_leftMenuMoving)
        {
            _leftMenuMoving = true;
        }
    }

}
