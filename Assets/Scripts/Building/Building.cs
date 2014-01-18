using System.Diagnostics;
using System.Runtime.InteropServices;

public class Building
{
    #region Constructors and Destructors

    public Building(
        int id,
        BuildingType buildingType,
        FactionType factionType)
    {
        this.BuildingId = id;
        this.BuildingType = buildingType;
        this.FactionType = factionType;
        this.Race = this.GetRaceTypeFromBuildingType();
        this.ActorType = this.GetActorTypeFromBuildingType();
		this.IsMainCity = false;
        this.LoadFromConfig();
    }

    #endregion

    #region Public Properties

    /// <summary>
    ///     Gets or sets the actor type.
    /// </summary>
    public ActorType ActorType { get; set; }

    /// <summary>
    ///     Gets or sets the building id.
    /// </summary>
    public int BuildingId { get; set; }
	
	public string BuildingName{get;set;}

    /// <summary>
    ///     Gets or sets the building type.
    /// </summary>
    public BuildingType BuildingType { get; set; }

    /// <summary>
    ///     Gets or sets the faction type.
    /// </summary>
    public FactionType FactionType { get; set; }

    /// <summary>
    /// Gets or sets the produced time.
    /// </summary>
    public int ProducedTime { get; set; }

    /// <summary>
    ///     Gets or sets the race.
    /// </summary>
    public RaceType Race { get; set; }
	
	public int CoinCost{get;set;}

    /// <summary>
    /// Gets or sets the hp.
    /// </summary>
    public float Hp { get; set; }
	
	public bool IsMainCity {get;set;}

    #endregion

    private RaceType GetRaceTypeFromBuildingType()
    {
        RaceType raceType;
        string[] strArr = this.BuildingType.ToString().Split('_');
        string raceTypeStr = strArr[0];
        switch (raceTypeStr)
        {
            case "Terran":
                raceType = RaceType.Terran;
                break;
            default:
                raceType = RaceType.Terran;
                break;
        }
        return raceType;
    }

    private ActorType GetActorTypeFromBuildingType()
    {
        ActorType actorType;
        switch (this.BuildingType)
        {
            case BuildingType.Terran_Barrack:
                actorType = ActorType.Infantry;
                break;
            case BuildingType.Terran_Fortress:
                actorType = ActorType.Supporter;
                break;
            case BuildingType.Terran_SniperHouse:
                actorType = ActorType.Sniper;
                break;
            case BuildingType.Terran_MarksmanCamp:
                actorType = ActorType.Marksman;
                break;
            case BuildingType.Terran_ArtilleryHall:
                actorType = ActorType.HeavyGunner;
                break;
            case BuildingType.Terran_ArtilleryLab:
                actorType = ActorType.MortarTeam;
                break;
            case BuildingType.Terran_MysterySchool:
                actorType = ActorType.Warlock;
                break;
            case BuildingType.Terran_Aviary:
                actorType = ActorType.GryphonRider;
                break;
            case BuildingType.Terran_AdvancedAviary:
                actorType = ActorType.SeniorGryphonRider;
                break;
            case BuildingType.Terran_Church:
                actorType = ActorType.Crusader;
                break;
            case BuildingType.Terran_Temple:
                actorType = ActorType.TemplarWarrior;
                break;
            default:
                actorType = ActorType.Infantry;
                break;
        }
        return actorType;
    }

    private void LoadFromConfig()
    {
        switch (this.BuildingType)
        {
            case BuildingType.Terran_Barrack:
                this.ProducedTime = GlobalConfig.TerranBuildingBarrackProduceTime;
                this.Hp = GlobalConfig.TerranBuildingBarrackHp;
				this.CoinCost = GlobalConfig.TerranBuildingBarrackCoinCost;
				this.BuildingName = "兵营";
                break;
            case BuildingType.Terran_Fortress:
                this.ProducedTime = GlobalConfig.TerranBuildingFortressProduceTime;
                this.Hp = GlobalConfig.TerranBuildingFortressHp;
				this.CoinCost = GlobalConfig.TerranBuildingFortressCoinCost;
			this.BuildingName = "要塞";
                break;
            case BuildingType.Terran_SniperHouse:
                this.ProducedTime = GlobalConfig.TerranBuildingSniperHouseProduceTime;
                this.Hp = GlobalConfig.TerranBuildingSniperHouseHp;
			this.CoinCost = GlobalConfig.TerranBuildingSniperHouseCoinCost;
			this.BuildingName = "狙击兵小屋";
                break;
            case BuildingType.Terran_MarksmanCamp:
                this.ProducedTime = GlobalConfig.TerranBuildingMarksmanCampProduceTime;
                this.Hp = GlobalConfig.TerranBuildingMarksmanCampHp;
			this.CoinCost = GlobalConfig.TerranBuildingMarksmanCampCoinCost;
			this.BuildingName = "神射手营地";
                break;
            case BuildingType.Terran_ArtilleryHall:
                this.ProducedTime = GlobalConfig.TerranBuildingArtilleryHallProduceTime;
                this.Hp = GlobalConfig.TerranBuildingArtilleryHallHp;
			this.CoinCost = GlobalConfig.TerranBuildingArtilleryHallCoinCost;
			this.BuildingName = "炮兵大厅";
                break;
            case BuildingType.Terran_ArtilleryLab:
                this.ProducedTime = GlobalConfig.TerranBuildingArtilleryLabProduceTime;
                this.Hp = GlobalConfig.TerranBuildingArtilleryLabHp;
			this.CoinCost = GlobalConfig.TerranBuildingArtilleryLabCoinCost;
			this.BuildingName = "火炮实验室";
                break;
            case BuildingType.Terran_MysterySchool:
                this.ProducedTime = GlobalConfig.TerranBuildingMysterySchoolProduceTime;
                this.Hp = GlobalConfig.TerranBuildingMysterySchoolHp;
			this.CoinCost = GlobalConfig.TerranBuildingMysterySchoolCoinCost;
			this.BuildingName = "神秘学院";
                break;
            case BuildingType.Terran_Aviary:
                this.ProducedTime = GlobalConfig.TerranBuildingAviaryProduceTime;
                this.Hp = GlobalConfig.TerranBuildingAviaryHp;
			this.CoinCost = GlobalConfig.TerranBuildingAviaryCoinCost;
			this.BuildingName = "狮鹫笼";
                break;
            case BuildingType.Terran_AdvancedAviary:
                this.ProducedTime = GlobalConfig.TerranBuildingAdvancedAviaryProduceTime;
                this.Hp = GlobalConfig.TerranBuildingAdvancedAviaryHp;
			this.CoinCost = GlobalConfig.TerranBuildingAdvancedAviaryCoinCost;
			this.BuildingName = "高级狮鹫笼";
                break;
            case BuildingType.Terran_Church:
                this.ProducedTime = GlobalConfig.TerranBuildingChurchProduceTime;
                this.Hp = GlobalConfig.TerranBuildingChurchHp;
			this.CoinCost = GlobalConfig.TerranBuildingChurchCoinCost;
			this.BuildingName = "教堂";
                break;
            case BuildingType.Terran_Temple:
                this.ProducedTime = GlobalConfig.TerranBuildingTempleProduceTime;
                this.Hp = GlobalConfig.TerranBuildingTempleHp;
			this.CoinCost = GlobalConfig.TerranBuildingTempleCoinCost;
			this.BuildingName = "圣殿";
                break;
            default:
                this.ProducedTime = GlobalConfig.TerranBuildingBarrackProduceTime;
                this.Hp = GlobalConfig.TerranBuildingBarrackHp;
				this.CoinCost = GlobalConfig.TerranBuildingBarrackCoinCost;
				this.BuildingName = "兵营";
                break;
        }
    }
}