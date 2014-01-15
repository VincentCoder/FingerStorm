using UnityEngine;
using System.Collections;

public class UIBuildingCardController : MonoBehaviour 
{
	void Start () 
	{
		UIEventListener.Get(this.gameObject).onDrag = HandleEvent;
	}
	
	private void HandleEvent( GameObject eventObj, Vector2 delta )
	{
		Debug.Log("HandleEvent");
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
