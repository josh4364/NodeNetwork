using NeoUnity;
using NeoUnity.Neo4j;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject LeftMenuPanel;
    public GameObject SelectedObject;

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


    public void ForgetEverything()
    {
        Server.Query("MATCH (n:Unity) DETACH DELETE (n)");
    }

    public void Forget()
    {
        Debug.Log(Server.Query($"MATCH (n:Unity {{Name:\\\"{ForgetText.text}\\\"}}) DETACH DELETE (n)"));
    }

    public void Remember()
    {
        if (RememberRelatedText.text != "")
        {
            RootObject s = Server.QueryObject($"MATCH (n:Unity {{Name:\\\"{RememberRelatedText.text}\\\"}}) RETURN (n)");
            if (s.results[0].data.Count > 0)
            {
                Server.Query($"MERGE(n:Unity {{Name:\\\"{RememberText.text}\\\", data:\\\"{RememberDataText.text}\\\"}}) WITH n MATCH(p:Unity {{Name:\\\"{RememberRelatedText.text}\\\"}}) WITH n,p MERGE (n) -[r:Rel]-> (p) return n,r,p");
            }
            else
            {
                Server.Query("MERGE (a:Unity{Name:\"" + RememberText.text + "\", data:\"" + RememberDataText.text + "\"}) MERGE (b:Unity{Name:\"" + RememberRelatedText.text + "\"}) MERGE (a) -[r:Rel]-> (b) return a, r, b");
            }
        }
        else
        {
            string s = $"MERGE (n:Unity {{Name:\\\"{RememberText.text}\\\", data:\\\"{RememberDataText.text}\\\"}}) RETURN n";
            Debug.Log(s);
            Server.Query(s);
        }
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
            RaycastHit rh;
            if (Physics.Raycast(r, out rh, float.PositiveInfinity))
            {
                if (rh.collider.tag == "Node")
                {
                    SelectedObject = rh.collider.gameObject;
                }
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
            Debug.Log($"x: {x} y: {y}");
            this.transform.position += new Vector3(x, y);
        }


    }

    void OnDrawGizmos()
    {
        Vector3 p = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, 10));
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(p, 1);
    }
    public void Recall()
    {
        RootObject o =  Server.QueryObject(QueryText.text);
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
