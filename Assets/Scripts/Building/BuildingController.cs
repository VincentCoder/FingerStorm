#region

using UnityEngine;

#endregion

public class BuildingController : BaseGameEntity
{
    #region Fields

    private Building _building;

    private Transform _myTransform;

    private tk2dSprite _selfSprite;

    private StateMachine<BuildingController> m_PStateMachine;

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

    public int DispatchIntervalLevel1 { get; set; }
    public int DispatchIntervalLevel2 { get; set; }

    public Transform MyTransform
    {
        get
        {
            if(this._myTransform == null)
				this._myTransform = this.gameObject.transform;
			return this._myTransform;
        }
        set
        {
            this._myTransform = value;
        }
    }

    public tk2dSprite SelfSprite
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

    public void DestroySelf()
    {
        Destroy(this.gameObject);
    }

    public StateMachine<BuildingController> GetFSM()
    {
        return this.m_PStateMachine;
    }

    public override bool HandleMessage(Telegram telegram)
    {
        return this.m_PStateMachine.HandleMessage(telegram);
    }
	
	public void TakeDamage(float damage)
	{
		this.Building.CurrentHp -= damage;
        if (this.Building.CurrentHp <= 0)
		{
			this.m_PStateMachine.ChangeState(Building_StateBeforeDestroy.Instance());
		}
	}

    #endregion

    #region Methods

    private void InitBuilding()
    {
        if (this.Building != null)
        {
            this.DispatchIntervalLevel1 = this.Building.ProducedTimeLevel1;
            this.DispatchIntervalLevel2 = this.Building.ProducedTimeLevel2;

            this.m_PStateMachine = new StateMachine<BuildingController>(this);
            this.m_PStateMachine.SetCurrentState(Building_StateBeforeBuilt.Instance());
            this.m_PStateMachine.SetGlobalState(Building_GlobalState.Instance());
        }
    }

    private void Update()
    {
        if (this.m_PStateMachine != null)
        {
            this.m_PStateMachine.SMUpdate();
        }
    }

    #endregion
}