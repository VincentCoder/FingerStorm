using UnityEngine;
using System.Collections;

public class UIBuildingCardController : MonoBehaviour 
{
	private GameObject buildingObj;
	private bool IsDragging;
	public string Description {get;set;}
	public Building Building{get;set;}
	
	private UILabel buildingDescriptionLabel;
	private GameSceneController gameSceneCtrl;
	
	void Start ()
	{
	    UIEventListener.Get(this.gameObject).onPress = HandleEvent_Press;
		UIEventListener.Get(this.gameObject).onDrag = HandleEvent_Drag;
		//UIEventListener.Get(this.gameObject).onDrop = HandleEvent_Drop;
		this.IsDragging = false;
		GameObject detailPanel = GameObject.FindWithTag("GameSceneBuildingDetailPanel");
		if(detailPanel != null)
			this.buildingDescriptionLabel = detailPanel.GetComponentInChildren<UILabel>();
		GameObject gameScene = GameObject.Find("GameSceneController");
		if(gameScene != null)
			this.gameSceneCtrl = gameScene.GetComponent<GameSceneController>();
	}
    
    private void HandleEvent_Press(GameObject go, bool state)
    {
		if(state)
		{
			if(this.buildingDescriptionLabel != null)
				this.buildingDescriptionLabel.text = this.Description;
		}
		else if(this.IsDragging)
		{
			if(this.CheckBuildingPositionValid() && BuildingsManager.GetInstance().PayForTheBuilding(this.Building.CoinCost))
			{
				GameObject.Find("GameController").GetComponent<GameController>().Client.SendCreateBuilding(this.buildingObj.transform.position, this.Building.BuildingType);
				this.buildingObj.GetComponent<BuildingController>().GetFSM().ChangeState(Building_StateBuilding.Instance());
			}
			else
			{
				Destroy(this.buildingObj);
			}
			this.IsDragging = false;
		}
    }
	
	private void HandleEvent_Drag( GameObject eventObj, Vector2 delta )
	{
		if(!this.IsDragging)
		{
			Vector3 pos = this.GetWorldPos(UICamera.lastTouchPosition);
			pos = new Vector3(pos.x, pos.y, 0);
			this.buildingObj = BuildingsManager.GetInstance().CreateNewBuilding(this.Building.BuildingType, this.gameSceneCtrl.MyFactionType, pos);
			this.IsDragging = true;
		}
		else if(this.buildingObj != null)
		{
            this.buildingObj.transform.position += ((Vector3)delta * NGUITools.FindInParents<UIRoot>(this.transform.parent).pixelSizeAdjustment);
		}
	}
	
	public Vector3 GetWorldPos( Vector2 screenPos )
    {
        Ray ray = Camera.main.ScreenPointToRay( screenPos );

        // we solve for intersection with z = 0 plane
        float t = -ray.origin.z / ray.direction.z;

        return ray.GetPoint( t );
    }
	
	private bool CheckBuildingPositionValid()
	{
		if(this.buildingObj != null)
		{
			Rect validRect;
			if(this.Building.FactionType == FactionType.Blue)
				validRect = new Rect(32, 192, 231, 416);
			else
				validRect = new Rect(697, 192, 231, 416);
			return validRect.Contains(this.buildingObj.transform.position);
		}
		return false;
	}
}
