#region

using System.Text;

using UnityEngine;

#endregion

public class ActorController : BaseGameEntity
{
    #region Fields

    private Actor _myActor;

    private tk2dSpriteAnimator _selfAnimator;

    public float moveSpeed;
    
    public Transform myTransform;

    private StateMachine<ActorController> m_PStateMachine; 

    #endregion

    #region Public Properties

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

    public GameObject TargetBuilding { get; set; }

    public GameObject TargetEnemy { get; set; }

    #endregion

    #region Properties

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

    #endregion

    #region Methods

    private void Awake()
    {
        this.myTransform = this.gameObject.transform;
        this.moveSpeed = 30;
    }

    private void InitActor()
    {
        if(this._myActor == null)
            Debug.LogError("Actor cannot be null !");
        this.m_PStateMachine = new StateMachine<ActorController>(this);
        this.m_PStateMachine.SetCurrentState(Actor_StateWalk.Instance());
        this.m_PStateMachine.SetGlobalState(Actor_GlobalState.Instance());
    }

    private void Update()
    {
        if(this.m_PStateMachine != null)
            this.m_PStateMachine.SMUpdate();
    }

    public StateMachine<ActorController> GetFSM()
    {
        return this.m_PStateMachine;
    }

    public override bool HandleMessage(Telegram telegram)
    {
        return this.m_PStateMachine.HandleMessage(telegram);
    }

    public void TakeDamage(float damage)
    {
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
        if(this.MyActor.Hp <= 0)
            this.m_PStateMachine.ChangeState(Actor_StateBeforeDie.Instance());
    }

    public void DestroySelf()
    {
        Destroy(this.gameObject);
    }

    #endregion
}