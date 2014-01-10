#region

using UnityEngine;

#endregion

public class BuildingController : MonoBehaviour
{
    #region Fields

    private Building _building;

    private int _dispatchInterval;

    private float _dispatchIntervalCounter;

    private ActorsManager actorsManager;

    #endregion

    #region Public Properties

    public Building Building
    {
        get
        {
            return this._building;
        }
        set
        {
            this._building = value;
            this.InitBuilding();
        }
    }

    public int DispatchInterval
    {
        get
        {
            return this._dispatchInterval;
        }
        set
        {
            this._dispatchInterval = value;
            this._dispatchIntervalCounter = 0.0f;
        }
    }

    #endregion

    #region Public Methods and Operators

    public void DestroySelf()
    {
        Destroy(this.gameObject);
    }

    #endregion

    #region Methods

    private void DispatchActor()
    {
        ActorType actorType;
        switch (this.Building.BuildingType)
        {
            case BuildingType.Terran_Barrack:
                actorType = ActorType.INFANTRY;
                break;
            case BuildingType.Terran_Fortress:
                actorType = ActorType.SUPPORTER;
                break;
            case BuildingType.Terran_SniperHouse:
                actorType = ActorType.SNIPER;
                break;
            case BuildingType.Terran_MarksmanCamp:
                actorType = ActorType.MARKSMAN;
                break;
            case BuildingType.Terran_ArtilleryHall:
                actorType = ActorType.HEAVYGUNNER;
                break;
            case BuildingType.Terran_ArtilleryLab:
                actorType = ActorType.MOTARTEAM;
                break;
            case BuildingType.Terran_MysterySchool:
                actorType = ActorType.WARLOCK;
                break;
            case BuildingType.Terran_Aviary:
                actorType = ActorType.GRYPHONRIDER;
                break;
            case BuildingType.Terran_AdvancedAviary:
                actorType = ActorType.SENIORGRYPHONRIDER;
                break;
            case BuildingType.Terran_Church:
                actorType = ActorType.CRUSADE;
                break;
            case BuildingType.Terran_Temple:
                actorType = ActorType.TEMPLARWARRIOR;
                break;
            default:
                actorType = ActorType.INFANTRY;
                break;
        }
        this.actorsManager.CreateNewActor(this.Building.FactionType, actorType);
    }

    private void InitBuilding()
    {
        if (this.Building != null)
        {
            switch (this.Building.BuildingType)
            {
                case BuildingType.Terran_Barrack:
                    this.Building.ActorType = ActorType.INFANTRY;
                    break;
                case BuildingType.Terran_Fortress:
                    this.Building.ActorType = ActorType.SUPPORTER;
                    break;
                case BuildingType.Terran_SniperHouse:
                    this.Building.ActorType = ActorType.SNIPER;
                    break;
                case BuildingType.Terran_MarksmanCamp:
                    this.Building.ActorType = ActorType.MARKSMAN;
                    break;
                case BuildingType.Terran_ArtilleryHall:
                    this.Building.ActorType = ActorType.HEAVYGUNNER;
                    break;
                case BuildingType.Terran_ArtilleryLab:
                    this.Building.ActorType = ActorType.MOTARTEAM;
                    break;
                case BuildingType.Terran_MysterySchool:
                    this.Building.ActorType = ActorType.WARLOCK;
                    break;
                case BuildingType.Terran_Aviary:
                    this.Building.ActorType = ActorType.GRYPHONRIDER;
                    break;
                case BuildingType.Terran_AdvancedAviary:
                    this.Building.ActorType = ActorType.SENIORGRYPHONRIDER;
                    break;
                case BuildingType.Terran_Church:
                    this.Building.ActorType = ActorType.CRUSADE;
                    break;
                case BuildingType.Terran_Temple:
                    this.Building.ActorType = ActorType.TEMPLARWARRIOR;
                    break;
                default:
                    this.Building.ActorType = ActorType.INFANTRY;
                    break;
            }
        }
    }

    private void Start()
    {
        this.actorsManager = ActorsManager.GetInstance();
    }

    private void Update()
    {
        this._dispatchIntervalCounter += Time.deltaTime;
        if (this._dispatchIntervalCounter >= this._dispatchInterval)
        {
            this.DispatchActor();
        }
    }

    #endregion
}