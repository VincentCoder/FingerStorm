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

    public GameController GameController;

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

    #region Public Methods and Operators

    public void ClearGameScene()
    {
        BuildingsManager.GetInstance().DestroyAllBuildings();
        ActorsManager.GetInstance().DestroyAllActors();
        Destroy(this.battleFieldMap);
        Destroy(GameObject.FindGameObjectWithTag("Obstacle"));
        GameObject[] torchs = GameObject.FindGameObjectsWithTag("GameSceneTorch");
        for (int i = 0; i < torchs.Length; i ++)
        {
            Destroy(torchs[i]);
        }
    }

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

        Vector3[] pos =
            {
                new Vector3(215, 34, -2), new Vector3(215, 140, -2), new Vector3(-209, 140, -2),
                new Vector3(-209, 34, -2)
            };
        for (int i = 0; i < 4; i ++)
        {
            GameObject torch = (GameObject)Instantiate(Resources.Load("Map/Torch"));
            torch.name = "Torch" + i;
            torch.transform.parent = GameObject.Find("tkAnchor").transform;
            torch.transform.localScale = new Vector3(1, 1, 1);
            torch.transform.localPosition = pos[i];
        }
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

        if (this.GameController != null)
        {
            Vector3 mainCityPos;
            if (this.MyFactionType == FactionType.Blue)
            {
                mainCityPos = new Vector3(50, 400, 0);
            }
            else
            {
                mainCityPos = new Vector3(910, 400, 0);
            }
            if (this.MyRaceType == RaceType.Terran)
            {
                BuildingsManager.GetInstance()
                    .CreateNewBuilding(BuildingType.Terran_TheMainCity, this.MyFactionType, mainCityPos);
                //BuildingsManager.GetInstance()
                //    .CreateNewBuilding(BuildingType.Terran_TheMainCity, FactionType.Red, new Vector3(910, 400, 0));

                if (this.GameController.GameType == GameType.PVE)
                {
                    BuildingsManager.GetInstance()
                        .CreateNewBuilding(BuildingType.Terran_TheMainCity, FactionType.Red, new Vector3(910, 400, 0));

                    BuildingsManager.GetInstance()
                        .CreateNewBuilding(BuildingType.Terran_Barrack, FactionType.Red, new Vector3(910, 250, 0));
                    BuildingsManager.GetInstance()
                        .CreateNewBuilding(BuildingType.Terran_ArtilleryLab, FactionType.Red, new Vector3(910, 530, 0));
                    BuildingsManager.GetInstance()
                        .CreateNewBuilding(BuildingType.Terran_Aviary, FactionType.Red, new Vector3(830, 400, 0));
                    BuildingsManager.GetInstance()
                        .CreateNewBuilding(BuildingType.Terran_Church, FactionType.Red, new Vector3(830, 250, 0));
                    BuildingsManager.GetInstance()
                        .CreateNewBuilding(BuildingType.Terran_Fortress, FactionType.Red, new Vector3(830, 530, 0));
                    BuildingsManager.GetInstance()
                        .CreateNewBuilding(BuildingType.Terran_MysterySchool, FactionType.Red, new Vector3(750, 400, 0));
                    BuildingsManager.GetInstance()
                        .CreateNewBuilding(BuildingType.Terran_SniperHouse, FactionType.Red, new Vector3(750, 250, 0));
                    BuildingsManager.GetInstance()
                        .CreateNewBuilding(BuildingType.Terran_Temple, FactionType.Red, new Vector3(750, 530, 0));
                }
                else if (this.GameController.GameType == GameType.PVP)
                {
                    this.GameController.Client.SendCreateBuilding(mainCityPos, BuildingType.Terran_TheMainCity);
                }
            }
            else if (this.MyRaceType == RaceType.Orc)
            {
                BuildingsManager.GetInstance()
                    .CreateNewBuilding(BuildingType.Orc_TheMainCity, this.MyFactionType, mainCityPos);

                if (this.GameController.GameType == GameType.PVE)
                {
                    BuildingsManager.GetInstance()
                        .CreateNewBuilding(BuildingType.Orc_TheMainCity, FactionType.Red, new Vector3(910, 400, 0));

                    BuildingsManager.GetInstance()
                        .CreateNewBuilding(BuildingType.Orc_AnimalFarm, FactionType.Red, new Vector3(910, 250, 0));
                    BuildingsManager.GetInstance()
                        .CreateNewBuilding(BuildingType.Orc_OrcFactory, FactionType.Red, new Vector3(910, 530, 0));
                    BuildingsManager.GetInstance()
                        .CreateNewBuilding(BuildingType.Orc_ShamanTent, FactionType.Red, new Vector3(830, 400, 0));
                    BuildingsManager.GetInstance()
                        .CreateNewBuilding(BuildingType.Orc_ThePound, FactionType.Red, new Vector3(830, 250, 0));
                    BuildingsManager.GetInstance()
                        .CreateNewBuilding(BuildingType.Orc_TheTaurenVine, FactionType.Red, new Vector3(830, 530, 0));
                    BuildingsManager.GetInstance()
                        .CreateNewBuilding(BuildingType.Orc_TrollHouse, FactionType.Red, new Vector3(750, 400, 0));
                    BuildingsManager.GetInstance()
                        .CreateNewBuilding(BuildingType.Orc_WarriorHall, FactionType.Red, new Vector3(750, 250, 0));
                    BuildingsManager.GetInstance()
                        .CreateNewBuilding(BuildingType.Orc_WyvernCamp, FactionType.Red, new Vector3(750, 530, 0));
                }
                else if (this.GameController.GameType == GameType.PVP)
                {
                    this.GameController.Client.SendCreateBuilding(mainCityPos, BuildingType.Orc_TheMainCity);
                }
            }
            this.GameController.ViewController.ShowBuildingsSelectorPanel();
            this.GameController.ViewController.ShowBuildingDetailPanel();
            this.GameController.ViewController.ShowPlayerSkillPanel();
            this.menuBar = this.GameController.ViewController.ShowMenuBar();
        }

        this.CoinCount = 1000;
        this.Mp = 200;
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