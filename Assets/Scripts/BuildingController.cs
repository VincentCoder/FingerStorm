using UnityEngine;
using System.Collections;

public class BuildingController : MonoBehaviour 
{
    public Building Building { get; set; }
    private int _dispatchInterval;
    private float _dispatchIntervalCounter;

    private void Start ()
    {
        this.InitBuilding();
    }

    private void InitBuilding ()
    {
        if (this.Building != null)
        {
            switch (this.Building.BuildingType)
            { 
                case BuildingType.Terran_ArtilleryHall:
                    this.Building.ActorType = ActorType.MOTARTEAM;
                    break;
                case BuildingType.Terran_Barrack:
                    this.Building.ActorType = ActorType.INFANTRY;
                    break;
                case BuildingType.Terran_Church:
                    this.Building.ActorType = ActorType.CRUSADES;
                    break;
                case BuildingType.Terran_Fortress:
                    this.Building.ActorType = ActorType.SUPPORTER;
                    break;
                case BuildingType.Terran_MarksmanCamp:
                    this.Building.ActorType = ActorType.MARKSMAN;
                    break;
                case BuildingType.Terran_SniperHouse:
                    this.Building.ActorType = ActorType.SNIPER;
                    break;
                case BuildingType.Terran_Temple:
                    this.Building.ActorType = ActorType.TEMPLARWARRIORS;
                    break;
            }
        }
    }

    private void Update ()
    {
        this._dispatchIntervalCounter += Time.deltaTime;
        if (this._dispatchIntervalCounter >= this._dispatchInterval)
        {
            this.DispatchActor();
        }
    }

    private void DispatchActor ()
    { 
        
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
}
