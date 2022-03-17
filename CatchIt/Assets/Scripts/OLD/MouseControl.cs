using UnityEngine;

public class MouseControl : MonoBehaviour 
{
	Ray pointer;
	RaycastHit pointerInfo;

	void Update () 
	{
		pointer = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(pointer, out pointerInfo))
		{
			Debug.Log(pointerInfo.collider.gameObject.name);

			if (Input.GetMouseButton(0))
			{ }
		}
	}
}
