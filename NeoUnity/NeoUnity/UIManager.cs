using NeoUnity;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject LeftMenuPanel;
    public GameObject SelectedObject;

    public Text QueryText;

    private const float _tSpeed = 100;
    private bool _leftMenu;
    private bool _leftMenuMoving;
    
    


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
    public void RunQuery()
    {
        NeoUnity.Neo4j.Server.Query(QueryText.text);
    }
    public void ToggleMenu()
    {
        if (!_leftMenuMoving)
        {
            _leftMenuMoving = true;
        }
    }

}
