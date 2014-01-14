using System.Diagnostics;
using System.Runtime.InteropServices;

public class Building
{
    #region Constructors and Destructors

    public Building(
        int id,
        bool isBase,
        BuildingType buildingType,
        FactionType factionType)
    {
        this.BuildingId = id;
        this.IsBase = isBase;
        this.BuildingType = buildingType;
        this.FactionType = factionType;
        this.Race = this.GetRaceTypeFromBuildingType();
        this.ActorType = this.GetActorTypeFromBuildingType();
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

    /// <summary>
    ///     Gets or sets the building type.
    /// </summary>
    public BuildingType BuildingType { get; set; }

    /// <summary>
    ///     Gets or sets the faction type.
    /// </summary>
    public FactionType FactionType { get; set; }

    /// <summary>
    ///     Gets or sets the is base.
    /// </summary>
    public bool IsBase { get; set; }

    /// <summary>
    /// Gets or sets the produced time.
    /// </summary>
    public int ProducedTime { get; set; }

    /// <summary>
    ///     Gets or sets the race.
    /// </summary>
    public RaceType Race { get; set; }

    /// <summary>
    /// Gets or sets the hp.
    /// </summary>
    public int Hp { get; set; }

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
                break;
            case BuildingType.Terran_Fortress:
                this.ProducedTime = GlobalConfig.TerranBuildingFortressProduceTime;
                this.Hp = GlobalConfig.TerranBuildingFortressHp;
                break;
            case BuildingType.Terran_SniperHouse:
                this.ProducedTime = GlobalConfig.TerranBuildingSniperHouseProduceTime;
                this.Hp = GlobalConfig.TerranBuildingSniperHouseHp;
                break;
            case BuildingType.Terran_MarksmanCamp:
                this.ProducedTime = GlobalConfig.TerranBuildingMarksmanCampProduceTime;
                this.Hp = GlobalConfig.TerranBuildingMarksmanCampHp;
                break;
            case BuildingType.Terran_ArtilleryHall:
                this.ProducedTime = GlobalConfig.TerranBuildingArtilleryHallProduceTime;
                this.Hp = GlobalConfig.TerranBuildingArtilleryHallHp;
                break;
            case BuildingType.Terran_ArtilleryLab:
                this.ProducedTime = GlobalConfig.TerranBuildingArtilleryLabProduceTime;
                this.Hp = GlobalConfig.TerranBuildingArtilleryLabHp;
                break;
            case BuildingType.Terran_MysterySchool:
                this.ProducedTime = GlobalConfig.TerranBuildingMysterySchoolProduceTime;
                this.Hp = GlobalConfig.TerranBuildingMysterySchoolHp;
                break;
            case BuildingType.Terran_Aviary:
                this.ProducedTime = GlobalConfig.TerranBuildingAviaryProduceTime;
                this.Hp = GlobalConfig.TerranBuildingAviaryHp;
                break;
            case BuildingType.Terran_AdvancedAviary:
                this.ProducedTime = GlobalConfig.TerranBuildingAdvancedAviaryProduceTime;
                this.Hp = GlobalConfig.TerranBuildingAdvancedAviaryHp;
                break;
            case BuildingType.Terran_Church:
                this.ProducedTime = GlobalConfig.TerranBuildingChurchProduceTime;
                this.Hp = GlobalConfig.TerranBuildingChurchHp;
                break;
            case BuildingType.Terran_Temple:
                this.ProducedTime = GlobalConfig.TerranBuildingTempleProduceTime;
                this.Hp = GlobalConfig.TerranBuildingTempleHp;
                break;
            default:
                this.ProducedTime = GlobalConfig.TerranBuildingBarrackProduceTime;
                this.Hp = GlobalConfig.TerranBuildingBarrackHp;
                break;
        }
    }
}