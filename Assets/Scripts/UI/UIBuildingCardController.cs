using UnityEngine;
using System.Collections;

public class UIBuildingCardController : MonoBehaviour 
{
	private GameObject building;
	
	void Start ()
	{
	    UIEventListener.Get(this.gameObject).onPress = HandleEvent_Press;
		UIEventListener.Get(this.gameObject).onDrag = HandleEvent_Drag;
		UIEventListener.Get(this.gameObject).onDrop = HandleEvent_Drop;
	}

    private void HandleEvent_Press(GameObject go, bool state)
    {
        building = (GameObject)Instantiate(Resources.Load("GameScene/Building"));
        building.transform.position = UICamera.lastHit.point;
    }
	
	private void HandleEvent_Drag( GameObject eventObj, Vector2 delta )
	{
		if(building != null)
		{
            building.transform.position += ((Vector3)delta * GameObject.Find("UIRoot").GetComponent<UIRoot>().pixelSizeAdjustment);
		}
	}
	
	private void HandleEvent_Drop(GameObject go, GameObject draggedObject)
	{
		Debug.Log("Drop");
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
