using UnityEngine;

public class UIManager : MonoBehaviour
{

	public GameObject SelectedObject;


    void Update()
    {
		if (Input.GetMouseButton (0)) {
			Ray r = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit rh;
			if (Physics.Raycast (r, out rh, float.PositiveInfinity)) {
				if (rh.collider.tag == "Node") {
					SelectedObject = rh.collider.gameObject;
				}
			}
		}

		if (SelectedObject) {
			SelectedObject.transform.position = Camera.main.ScreenToWorldPoint (Input.mousePosition+ new Vector3(0,0,10));
		}

		if (Input.GetMouseButtonUp (0)) {
			SelectedObject = null;
		}
		if (Input.GetMouseButton (1)) {
			float x = -Input.GetAxis ("Mouse X");
			float y = -Input.GetAxis ("Mouse Y");
			Debug.Log ($"x: {x} y: {y}");
			this.transform.position += new Vector3 (x, y);
		}


    }

	void OnDrawGizmos(){
		Vector3 p = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0,0,10));
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere(p,1);
	}
}
