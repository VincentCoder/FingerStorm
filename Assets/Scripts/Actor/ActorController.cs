#region

using System.Collections.Generic;

using UnityEngine;

#endregion

public class ActorController : BaseGameEntity
{
    #region Fields

    public float moveSpeed;

    public Transform myTransform;

    private Actor _myActor;

    private tk2dSpriteAnimator _selfAnimator;

    private bool _isStun;

    private StateMachine<ActorController> m_PStateMachine;

    #endregion

    #region Public Properties

    public bool IsStun 
    {
        get
        {
            return this._isStun;
        }
        set
        {
            this._isStun = value;
            if (this._isStun)
            {
                if (!this.SelfAnimator.Paused)
                {
                    this.SelfAnimator.Pause();
                }
            }
            else
            {
                if (this.SelfAnimator.Paused)
                {
                    this.SelfAnimator.Resume();
                }
            }
        } 
    }

    public Actor MyActor
    {
        get
        {
            return this._myActor;
        }
        set
        {
            this._myActor = value;
            this.InitActor();
        }
    }

    public tk2dSpriteAnimator SelfAnimator
    {
        get
        {
            if (this._selfAnimator == null)
            {
                this._selfAnimator = this.gameObject.GetComponent<tk2dSpriteAnimator>();
            }
            return this._selfAnimator;
        }
    }

    public GameObject TargetBuilding { get; set; }

    public GameObject TargetEnemy { get; set; }

    #endregion

    #region Public Methods and Operators

    public void DestroySelf()
    {
    	ActorsManager.GetInstance().RemoveActorById(this._myActor.ActorId);
        Destroy(this.gameObject);
    }

    public StateMachine<ActorController> GetFSM()
    {
        return this.m_PStateMachine;
    }

    public override bool HandleMessage(Telegram telegram)
    {
        return this.m_PStateMachine.HandleMessage(telegram);
    }

    public List<GameObject> SeekAndGetEnemies()
    {
        return ActorsManager.GetInstance()
            .GetEnemyActorsInDistanceAndSortByDistance(this, this.MyActor.ActorAttack.ViewDistance);
    }

    public List<GameObject> SeekAndGetEnemiesInDistance(int distance)
    {
        return ActorsManager.GetInstance().GetEnemyActorsInDistanceAndSortByDistance(this, distance);
    }

    public bool SeekEnemies()
    {
        return ActorsManager.GetInstance().HasEnemyActorsInDistance(this, this.MyActor.ActorAttack.ViewDistance);
    }

    public void TakeDamage(float damage)
    {
		Debug.Log("TakeDamage");
        ActorSpell dodgeSpell = this.MyActor.GetSpell(ActorSpellName.Dodge);
        if (dodgeSpell != null)
        {
            int randomIndex = Random.Range(1, 101);
            if (randomIndex <= dodgeSpell.EvasiveProbability)
            {
                return;
            }
        }

        if (this.MyActor.ActorArmor.ArmorAmount > 0f)
        {
            float dValue = this.MyActor.ActorArmor.ArmorAmount - damage;
            if (dValue >= 0)
            {
                this.MyActor.ActorArmor.ArmorAmount = dValue;
            }
            else
            {
                this.MyActor.ActorArmor.ArmorAmount = 0f;
                damage = Mathf.Abs(dValue);
            }
        }
        this.MyActor.Hp -= damage;
        if (this.MyActor.Hp <= 0)
        {
            this.m_PStateMachine.ChangeState(Actor_StateBeforeDie.Instance());
        }
		Debug.Log(this._myActor.Hp);
    }

    #endregion

    #region Methods

    private void Awake()
    {
        this.myTransform = this.gameObject.transform;
        this.moveSpeed = 30;
    }

    private void InitActor()
    {
        if (this._myActor == null)
        {
            Debug.LogError("Actor cannot be null !");
        }
        this.m_PStateMachine = new StateMachine<ActorController>(this);
        this.m_PStateMachine.SetCurrentState(Actor_StateWalk.Instance());
        this.m_PStateMachine.SetGlobalState(Actor_GlobalState.Instance());
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