#region

using System.Text;

using UnityEngine;

#endregion

public class BuildingController : BaseGameEntity
{
    #region Fields

    private Building _building;

    private int _dispatchInterval;

    private float _dispatchIntervalCounter;

    private tk2dSprite _selfSprite;

    private ActorsManager actorsManager;

    private Transform myTransform;

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

    #region Properties

    private tk2dSprite SelfSprite
    {
        get
        {
            return this._selfSprite ?? (this._selfSprite = this.gameObject.GetComponent<tk2dSprite>());
        }
        set
        {
            this._selfSprite = value;
        }
    }

    #endregion

    #region Public Methods and Operators

    private void Awake()
    {
        this.myTransform = this.transform;
    }

    public void DestroySelf()
    {
        Destroy(this.gameObject);
    }

    public override bool HandleMessage(Telegram telegram)
    {
        return base.HandleMessage(telegram);
    }

    #endregion

    #region Methods

    private void DispatchActor()
    {
        this.actorsManager.CreateNewActor(this.Building.FactionType, this.Building.ActorType, this.myTransform.position);
    }

    private void InitBuilding()
    {
        if (this.Building != null)
        {
            this.DispatchInterval = this.Building.ProducedTime;

            var spriteName = new StringBuilder("");
            spriteName.Append(this.Building.BuildingType);
            spriteName.Append("_");
            spriteName.Append(this.Building.FactionType);
            this.SelfSprite.SetSprite(spriteName.ToString());
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
            this._dispatchIntervalCounter = 0.0f;
        }
    }

    #endregion
}