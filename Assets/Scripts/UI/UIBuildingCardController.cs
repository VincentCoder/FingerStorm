using UnityEngine;
using System.Collections;

public class UIBuildingCardController : MonoBehaviour 
{
	private GameObject building;
	
	void Start () 
	{
		UIEventListener.Get(this.gameObject).onDrag = HandleEvent_Drag;
		UIEventListener.Get(this.gameObject).onDrop = HandleEvent_Drop;
	}
	
	private void HandleEvent_Drag( GameObject eventObj, Vector2 delta )
	{
		if(building == null)
		{
			building = (GameObject)Instantiate(Resources.Load("GameScene/Building"));
		}
		building.transform.position = new Vector3(UICamera.currentTouch.pos.x, UICamera.currentTouch.pos.y, 0);
	}
	
	private void HandleEvent_Drop(GameObject go, GameObject draggedObject)
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
