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
	
	public ActorPath ActorPath {get; set;}

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
					this.ShowTip("Stun");
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

    public BaseGameEntity TargetEnemy { get; set; }

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

    public List<GameObject> SeekAndGetEnemies(bool ignoreObstacles = true)
    {
        return ActorsManager.GetInstance()
            .GetEnemyActorsInDistanceAndSortByDistance(this, this.MyActor.ActorAttack.ViewDistance, ignoreObstacles);
    }

    public List<GameObject> SeekAndGetEnemiesInDistance(int distance, bool ignoreObstacles = true)
    {
        return ActorsManager.GetInstance().GetEnemyActorsInDistanceAndSortByDistance(this, distance, ignoreObstacles);
    }

    public bool SeekEnemies()
    {
        return ActorsManager.GetInstance().HasEnemyActorsInDistance(this, this.MyActor.ActorAttack.ViewDistance);
    }

    public void TakeDamage(float damage, int attackType = -1, int spellName = -1)
    {
		if(spellName >= 0)
		{
			bool showCrit = false;
			ActorSpellName actorSpellName = (ActorSpellName)spellName;
			switch(actorSpellName)
			{
				case ActorSpellName.CirticalStrike:
					showCrit = true;
					break;
				case ActorSpellName.HeadShot:
					showCrit = true;
					break;
				case ActorSpellName.ArcaneExplosion:
					showCrit = true;
					break;
				case ActorSpellName.MortarAttack:
					showCrit = true;
					break;
				default:
					showCrit = false;
					break;
			}

			if(showCrit)
				this.ShowTip(damage.ToString());
		}
		
        ActorSpell dodgeSpell = this.MyActor.GetSpell(ActorSpellName.Dodge);
        if (dodgeSpell != null)
        {
            int randomIndex = Random.Range(1, 101);
            if (randomIndex <= dodgeSpell.EvasiveProbability)
            {
				this.ShowTip("Dodge");
                return;
            }
		}
		
		if(attackType >= 0)
		{
			ActorAttackType actorAttackType = (ActorAttackType)attackType;
			switch(actorAttackType)
			{
				case ActorAttackType.Normal:
				{
					if(this.MyActor.ActorArmor.ActorArmorType == ActorArmorType.LightArmor)
						damage *= 0.9f;
					else if(this.MyActor.ActorArmor.ActorArmorType == ActorArmorType.HeavyArmor)
						damage *= 0.8f;
					else if(this.MyActor.ActorArmor.ActorArmorType == ActorArmorType.HeroArmor)
						damage *= 1;
					break;
				}
			case ActorAttackType.Pierce:
				{
					if(this.MyActor.ActorArmor.ActorArmorType == ActorArmorType.LightArmor)
						damage *= 2f;
					else if(this.MyActor.ActorArmor.ActorArmorType == ActorArmorType.HeavyArmor)
						damage *= 0.35f;
					else if(this.MyActor.ActorArmor.ActorArmorType == ActorArmorType.HeroArmor)
						damage *= 0.5f;
					break;
				}
				case ActorAttackType.Siege:
				{
					if(this.MyActor.ActorArmor.ActorArmorType == ActorArmorType.LightArmor)
						damage *= 1f;
					else if(this.MyActor.ActorArmor.ActorArmorType == ActorArmorType.HeavyArmor)
						damage *= 1.5f;
					else if(this.MyActor.ActorArmor.ActorArmorType == ActorArmorType.HeroArmor)
						damage *= 0.5f;
					break;
				}
				case ActorAttackType.Magic:
				{
					if(this.MyActor.ActorArmor.ActorArmorType == ActorArmorType.LightArmor)
						damage *= 1.25f;
					else if(this.MyActor.ActorArmor.ActorArmorType == ActorArmorType.HeavyArmor)
						damage *= 2f;
					else if(this.MyActor.ActorArmor.ActorArmorType == ActorArmorType.HeroArmor)
						damage *= 0.35f;
					break;
				}
				case ActorAttackType.Confuse:
				{
					if(this.MyActor.ActorArmor.ActorArmorType == ActorArmorType.LightArmor)
						damage *= 1f;
					else if(this.MyActor.ActorArmor.ActorArmorType == ActorArmorType.HeavyArmor)
						damage *= 1f;
					else if(this.MyActor.ActorArmor.ActorArmorType == ActorArmorType.HeroArmor)
						damage *= 1f;
					break;
				}
				case ActorAttackType.HeroAttack:
				{
					if(this.MyActor.ActorArmor.ActorArmorType == ActorArmorType.LightArmor)
						damage *= 1.2f;
					else if(this.MyActor.ActorArmor.ActorArmorType == ActorArmorType.HeavyArmor)
						damage *= 1.2f;
					else if(this.MyActor.ActorArmor.ActorArmorType == ActorArmorType.HeroArmor)
						damage *= 1f;
					break;
				}
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
        this.MyActor.CurrentHp -= damage;
        if (this.MyActor.CurrentHp <= 0)
        {
            this.m_PStateMachine.ChangeState(Actor_StateBeforeDie.Instance());
        }
        this.RefreshHpBar();
    }

    #endregion

    #region Methods

    private void InitActor()
    {
        if (this._myActor == null)
        {
            Debug.LogError("Actor cannot be null !");
        }
		
		this.myTransform = this.gameObject.transform;
        this.moveSpeed = 30;
        this.RefreshHpBar();
		
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
	
	private void ShowTip(string tip)
	{
		GameObject tipObj = (GameObject)Instantiate(Resources.Load("Tips/TipsTextMesh"));
		tipObj.transform.localPosition = this.myTransform.position + new Vector3(6, 20, -1);
		tipObj.GetComponent<tk2dTextMesh>().text = tip;
		tipObj.GetComponent<ActorTip>().Show();
	}

    private void RefreshHpBar()
    {
        Transform hpBarTran = this.transform.FindChild("HpBar");
        if (hpBarTran != null)
        {
            tk2dSprite barSprite = hpBarTran.gameObject.GetComponent<tk2dSprite>();
            barSprite.scale = new Vector3(this.MyActor.CurrentHp / this.MyActor.TotalHp, 1f, 1f);
        }
    }

    #endregion
}