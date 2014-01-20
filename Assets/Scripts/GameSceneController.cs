#region

using UnityEngine;

#endregion

/// <summary>
///     The game scene controller.
/// </summary>
public class GameSceneController : MonoBehaviour
{
    #region Fields

    public float CoinIncreaseSpeed = 20;

    public float MpIncreaseSpeed = 1;

    private float CoinIncreaseCounter;

    private float MpIncreaseCounter;

    /// <summary>
    ///     The battle field map.
    /// </summary>
    private GameObject battleFieldMap;

    private int coinCount;

    private GameObject menuBar;

    private int mp;

    #endregion

    #region Public Properties

    public int CoinCount
    {
        get
        {
            return this.coinCount;
        }
        set
        {
            this.coinCount = value;
            this.RefreshCoinCount();
        }
    }

    public int Mp
    {
        get
        {
            return this.mp;
        }
        set
        {
            this.mp = value;
            this.RefreshMp();
        }
    }

    public FactionType MyFactionType { get; set; }

    public RaceType MyRaceType { get; set; }

    #endregion

    #region Methods

    /// <summary>
    ///     The create battle field map.
    /// </summary>
    private void CreateBattleFieldMap()
    {
        this.battleFieldMap = (GameObject)Instantiate(Resources.Load("Map/BattleFieldMap"));
        this.battleFieldMap.name = "BattleFieldMap";
        this.battleFieldMap.transform.parent = GameObject.Find("tkAnchor").transform;
        this.battleFieldMap.transform.localScale = new Vector3(1, 1, 1);
        this.battleFieldMap.transform.localPosition = new Vector3(0, 0, 0);

        GameObject obstacle = (GameObject)Instantiate(Resources.Load("Map/Obstacle"));
        obstacle.name = "Obstacle";
        obstacle.transform.parent = GameObject.Find("tkAnchor").transform;
        obstacle.transform.localScale = new Vector3(1, 1, 1);
        obstacle.transform.localPosition = new Vector3(0, 80, -1);
    }

    private void RefreshCoinCount()
    {
        if (this.menuBar != null)
        {
            Transform coinLabel = this.menuBar.transform.FindChild("CoinLabel");
            if (coinLabel != null)
            {
                coinLabel.gameObject.GetComponent<UILabel>().text = this.CoinCount.ToString();
            }
        }
    }

    private void RefreshMp()
    {
        if (this.menuBar != null)
        {
            Transform mpLabel = this.menuBar.transform.FindChild("MpLabel");
            if (mpLabel != null)
            {
                mpLabel.gameObject.GetComponent<UILabel>().text = this.Mp.ToString();
            }
        }
    }

    /// <summary>
    ///     The start.
    /// </summary>
    private void Start()
    {
        this.CreateBattleFieldMap();
        BuildingsManager.GetInstance()
            .CreateNewBuilding(BuildingType.Terran_TheMainCity, FactionType.Blue, new Vector3(100, 400, 0));
        BuildingsManager.GetInstance()
            .CreateNewBuilding(BuildingType.Terran_TheMainCity, FactionType.Red, new Vector3(860, 400, 0));

        GameController gameCtrl = GameObject.Find("GameController").GetComponent<GameController>();
        if (gameCtrl != null)
        {
            if (gameCtrl.GameType == GameType.PVE)
            {
                BuildingsManager.GetInstance()
                    .CreateNewBuilding(BuildingType.Terran_Barrack, FactionType.Blue, new Vector3(100, 530, 0));

                BuildingsManager.GetInstance()
                    .CreateNewBuilding(BuildingType.Terran_Fortress, FactionType.Blue, new Vector3(100, 270, 0));

                BuildingsManager.GetInstance()
                    .CreateNewBuilding(BuildingType.Terran_MarksmanCamp, FactionType.Blue, new Vector3(250, 400, 0));

                BuildingsManager.GetInstance()
                    .CreateNewBuilding(BuildingType.Terran_ArtilleryHall, FactionType.Red, new Vector3(860, 270, 0));

                BuildingsManager.GetInstance()
                    .CreateNewBuilding(BuildingType.Terran_Church, FactionType.Red, new Vector3(710, 400, 0));

                BuildingsManager.GetInstance()
                    .CreateNewBuilding(BuildingType.Terran_SniperHouse, FactionType.Red, new Vector3(710, 530, 0));
            }

            gameCtrl.ViewController.ShowBuildingsSelectorPanel();
            gameCtrl.ViewController.ShowBuildingDetailPanel();
			gameCtrl.ViewController.ShowPlayerSkillPanel();
            this.menuBar = gameCtrl.ViewController.ShowMenuBar();
        }

        this.CoinCount = 300;
        this.Mp = 0;
    }

    private void Update()
    {
        this.CoinIncreaseCounter += Time.deltaTime;
        if (this.CoinIncreaseCounter >= this.CoinIncreaseSpeed)
        {
            this.CoinCount += 300;
            this.CoinIncreaseCounter = 0f;
        }
        this.MpIncreaseCounter += Time.deltaTime;
        if (this.MpIncreaseCounter >= this.MpIncreaseSpeed)
        {
            this.Mp ++;
            this.MpIncreaseCounter = 0f;
        }
    }

    #endregion
}