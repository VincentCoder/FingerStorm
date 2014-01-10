using UnityEditor;

using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour 
{
    public UIEventController EventController;
    public UIViewController ViewController;

    private void Awake()
    {
        this.LoadGlobalConfig();
    }

    private void Start ()
    {
        if (this.ViewController != null)
            this.ViewController.ShowHomePage();
    }

    private void LoadGlobalConfig()
    {
        GameObject loadObj = new GameObject("LoadConfigOfLua");
        loadObj.AddComponent<LoadConfigOfLua>();
    }
}
