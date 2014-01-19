using UnityEngine;
using System.Collections;

public class GameController : BaseGameEntity 
{
    public UIEventController EventController;
    public UIViewController ViewController;
	
	public GameSceneController GameSceneController;
	
	public FSClient Client {get;set;}
	
	private StateMachine<GameController> m_PStateMachine;
	
	public FactionType MyFactionType {get;set;}

    private void Awake()
    {
        this.LoadGlobalConfig();
		
		CustomTypes.Register();
		this.Client = new FSClient();
        this.Client.MasterServerAddress = "app-eu.exitgamescloud.com:5055";
        this.Client.AppId = "78162ccb-b912-423b-bf93-7f6ac0210adf";
        this.Client.AppVersion = "1.0";
        this.Client.PlayerName = SystemInfo.deviceName == null ? "Player" : SystemInfo.deviceName;
		this.Client.gameController = this;
        this.Client.Connect();
    }

    private void Start ()
    {
		Application.runInBackground = true;
		
		this.m_PStateMachine = new StateMachine<GameController>(this);
        this.m_PStateMachine.SetCurrentState(GameState_HomePage.Instance());
        this.m_PStateMachine.SetGlobalState(GameState_GlobalState.Instance());
    }
	
	public StateMachine<GameController> GetFSM()
    {
        return this.m_PStateMachine;
    }

    public override bool HandleMessage(Telegram telegram)
    {
        return this.m_PStateMachine.HandleMessage(telegram);
    }

    private void LoadGlobalConfig()
    {
        GameObject loadObj = new GameObject("LoadConfigOfLua");
        loadObj.AddComponent<LoadConfigOfLua>();
    }
	
	private void Update()
	{
		if (this.m_PStateMachine != null)
        {
            this.m_PStateMachine.SMUpdate();
        }
	}
	
	public void StartGame()
	{
		Debug.Log("StartGame");
		this.ViewController.DestroyHomePage(true);
		this.ViewController.DestroyShadowCover(true);
		GameObject gameSceneCtrl = (GameObject)Instantiate(Resources.Load("GameScene/GameSceneController"));
		gameSceneCtrl.transform.localPosition = new Vector3(0, 0, 0);
		gameSceneCtrl.name = "GameSceneController";
		this.GameSceneController = gameSceneCtrl.GetComponent<GameSceneController>();
		this.GameSceneController.MyFactionType = this.MyFactionType;
	}
}
