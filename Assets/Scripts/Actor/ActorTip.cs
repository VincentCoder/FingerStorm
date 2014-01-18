using UnityEngine;
using System.Collections;

public class ActorTip : MonoBehaviour 
{
	public void Show()
	{
		iTween.MoveTo(this.gameObject, iTween.Hash("position", this.gameObject.transform.localPosition + new Vector3(0, 10, 0), "time", 1f, "oncomplete", "OnMoveCompleted", "oncompletetarget", this.gameObject));
	}
	
	private void OnMoveCompleted()
	{
		Destroy(this.gameObject);
	}
}
