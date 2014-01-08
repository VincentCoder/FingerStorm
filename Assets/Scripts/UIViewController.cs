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
