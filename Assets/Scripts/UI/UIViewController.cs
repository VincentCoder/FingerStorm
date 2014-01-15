using UnityEngine;
using System.Collections;

public class UIViewController : MonoBehaviour 
{
    private GameObject _rootPanel;
    private GameObject _loadingPage;
    private GameController _gameController;

    private void Awake ()
    {
        this._rootPanel = GameObject.Find("PanelRoot");
    }

    public void ShowHomePage ()
    {
        GameObject homePage = (GameObject)Instantiate(Resources.Load("UI/HomePage"));
        homePage.name = "HomePage";
        homePage.tag = "HomePage";
        homePage.transform.parent = this._rootPanel.transform;
        homePage.transform.localScale = new Vector3(1, 1, 1);
        homePage.transform.localPosition = new Vector3(0, 0, 0);
        this.GameController.EventController.RegisterInHierarchy(homePage);
    }

    public void DestroyHomePage (bool now)
    {
        GameObject homePage = GameObject.FindWithTag("HomePage");
        this.GameController.EventController.UnRegisterInHierarchy(homePage);
        if (now)
            DestroyImmediate(homePage);
        else
            Destroy(homePage);
    }
	
	public void ShowBuildingsSelectorPanel()
	{
		GameObject selectorPanel = (GameObject)Instantiate(Resources.Load("UI/BuildingsSelectorPanel"));
		selectorPanel.name = "BuildingsSelectorPanel";
		selectorPanel.tag = "GameSceneBuildingsSelectorPanel";
		selectorPanel.transform.parent = this._rootPanel.transform;
		selectorPanel.transform.localScale = new Vector3(1,1,1);
	}
	
	public void DestroyBuildingsSelectorPanel(bool now)
	{
		GameObject selectorPanel = GameObject.FindWithTag("GameSceneBuildingsSelectorPanel");
		if(now)
			DestroyImmediate(selectorPanel);
		else
			Destroy(selectorPanel);
	}
	
	public void ShowBuildingDetailPanel()
	{
		GameObject detailPanel = (GameObject)Instantiate(Resources.Load("UI/BuildingDetailPanel"));
		detailPanel.name = "BuildingDetailPanel";
		detailPanel.tag = "GameSceneBuildingDetailPanel";
		detailPanel.transform.parent = this._rootPanel.transform;
		detailPanel.transform.localScale = new Vector3(1,1,1);
	}
	
	public void DestroyBuildingDetailPanel(bool now)
	{
		GameObject detailPanel = GameObject.FindWithTag("GameSceneBuildingDetailPanel");
		if(now)
			DestroyImmediate(detailPanel);
		else
			Destroy(detailPanel);
	}
	
	public void AddBuildingCard()
	{
		
	}
	
    public GameController GameController
    {
        get
        {
            if (this._gameController == null)
                this._gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
            return this._gameController;
        }
    }
}
