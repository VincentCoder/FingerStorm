#region

using System.Collections;
using System.Collections.Generic;

using UnityEngine;

#endregion

public class ActorController : BaseGameEntity
{
    #region Fields

    public float AttackSpeed;

    public float moveSpeed;

    public Transform myTransform;

    private bool _isStun;

    private Actor _myActor;

    private tk2dSpriteAnimator _selfAnimator;

    private tk2dSlicedSprite hpBarSprite;
	
	private float hpbarLength;
	
    private StateMachine<ActorController> m_PStateMachine;
	
	private bool isBleed;

    #endregion

    #region Public Properties

    public ActorPath ActorPath { get; set; }

    public float AttackPlusRatio { get; set; }

    public int BloodSuckingRatio { get; set; }
	
	public bool IsBleed {get;set;}
	
	public float BleedDps {get;set;}
	
	public float BleedDuration {get;set;}

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

    public float StunDuration { get; set; }

    public GameObject TargetBuilding { get; set; }

    public BaseGameEntity TargetEnemy { get; set; }

    #endregion

    #region Public Methods and Operators

    public Damage CalculateCommonAttackDamage(BaseGameEntity targetEntity)
    {
        Damage damage = new Damage();
        damage.DamageValue = this.MyActor.ActorAttack.Dps / this.AttackSpeed;
        damage.DamageValue *= this.AttackPlusRatio;
		if(!targetEntity.GetType().IsInstanceOfType(typeof(ActorController)))
			return damage;
		
		ActorController targetActor = (ActorController)targetEntity;
        ActorSpell dodgeSpell = targetActor.MyActor.GetSpell(ActorSpellName.Dodge);
        if (dodgeSpell != null)
        {
            int randomIndex = Random.Range(1, 101);
            if (randomIndex <= dodgeSpell.EvasiveProbability)
            {
                this.ShowTip("Miss");
                damage.DamageValue = 0;
                return damage;
            }
        }

        Dictionary<ActorSpellName, ActorSpell> passiveSpellDictionary =
            this.MyActor.GetSpellsByType(ActorSpellType.PassiveSpell);
        if (passiveSpellDictionary != null)
        {
            foreach (KeyValuePair<ActorSpellName, ActorSpell> kv in passiveSpellDictionary)
            {
                switch (kv.Key)
                {
                    case ActorSpellName.None:
                        break;
                    case ActorSpellName.CirticalStrike:
                        {
                            if (Random.Range(1, 101) <= 10)
                            {
                                damage.DamageValue *= 1.5f;
                                damage.ShowCrit = true;
                            }
                            break;
                        }
                    case ActorSpellName.HeadShot:
                        {
                            if (Random.Range(1, 101) <= 40)
                            {
                                damage.DamageValue *= 3f;
                                damage.ShowCrit = true;
                            }
                            break;
                        }
                    case ActorSpellName.SplashDamage:
                        {
                            List<GameObject> enemies = this.SeekAndGetEnemiesInDistance(18);
                            if (enemies != null && enemies.Count != 0)
                            {
                                Damage splashDamage = new Damage();
                                splashDamage.DamageValue = 0.5f * damage.DamageValue;
                                enemies.ForEach(
                                    enemy =>
                                        {
                                            ActorController actorCtrl = enemy.GetComponent<ActorController>();
                                            if (actorCtrl != null && actorCtrl.gameObject != this.TargetEnemy.gameObject)
                                            {
                                                this.SendDamage(actorCtrl, splashDamage);
                                            }
                                        });
                            }
                            break;
                        }
                    case ActorSpellName.Bleed:
                        {
                            if (targetActor.MyActor.ActorType == ActorType.GryphonRider)
                            {
                                if (Random.Range(1, 101) <= 25)
                                {
                                    damage.Bleed = true;
                                    damage.BleedDuration = 3;
                                    damage.BleedDps = 15;
                                }
                            }
                            else if (targetActor.MyActor.ActorType == ActorType.SeniorGryphonRider)
                            {
                                if (Random.Range(1, 101) <= 30)
                                {
                                    damage.Bleed = true;
                                    damage.BleedDuration = 5;
                                    damage.BleedDps = 20;
                                }
                            }
                            break;
                        }
                    case ActorSpellName.Bash:
                        {
                            if (targetActor.MyActor.ActorType == ActorType.Crusader)
                            {
                                if (Random.Range(1, 101) <= 20)
                                {
                                    damage.Stun = true;
                                    damage.StunDuration = 2;
                                    damage.DamageValue += 25;
                                    damage.ShowCrit = true;
                                }
                            }
                            else if (targetActor.MyActor.ActorType == ActorType.TemplarWarrior)
                            {
                                if (Random.Range(1, 101) <= 25)
                                {
                                    damage.Stun = true;
                                    damage.StunDuration = 2;
                                    damage.DamageValue += 30;
                                    damage.ShowCrit = true;
                                }
                            }
                            break;
                        }
                    case ActorSpellName.ChainLightning:
                        {
                            break;
                        }
                }
            }
        }

        ActorAttackType actorAttackType = this.MyActor.ActorAttack.ActorAttackType;
        switch (actorAttackType)
        {
            case ActorAttackType.Normal:
                {
                    if (targetActor.MyActor.ActorArmor.ActorArmorType == ActorArmorType.LightArmor)
                    {
                        damage.DamageValue *= 0.9f;
                    }
                    else if (targetActor.MyActor.ActorArmor.ActorArmorType == ActorArmorType.HeavyArmor)
                    {
                        damage.DamageValue *= 0.8f;
                    }
                    else if (targetActor.MyActor.ActorArmor.ActorArmorType == ActorArmorType.HeroArmor)
                    {
                        damage.DamageValue *= 1;
                    }
                    break;
                }
            case ActorAttackType.Pierce:
                {
                    ActorSpell parrySpell = targetActor.MyActor.GetSpell(ActorSpellName.Parry);
                    if (parrySpell != null)
                    {
                        damage.DamageValue *= 0.7f;
                    }

                    if (targetActor.MyActor.ActorArmor.ActorArmorType == ActorArmorType.LightArmor)
                    {
                        damage.DamageValue *= 2f;
                    }
                    else if (targetActor.MyActor.ActorArmor.ActorArmorType == ActorArmorType.HeavyArmor)
                    {
                        damage.DamageValue *= 0.35f;
                    }
                    else if (targetActor.MyActor.ActorArmor.ActorArmorType == ActorArmorType.HeroArmor)
                    {
                        damage.DamageValue *= 0.5f;
                    }
                    break;
                }
            case ActorAttackType.Siege:
                {
                    if (targetActor.MyActor.ActorArmor.ActorArmorType == ActorArmorType.LightArmor)
                    {
                        damage.DamageValue *= 1f;
                    }
                    else if (targetActor.MyActor.ActorArmor.ActorArmorType == ActorArmorType.HeavyArmor)
                    {
                        damage.DamageValue *= 1.5f;
                    }
                    else if (targetActor.MyActor.ActorArmor.ActorArmorType == ActorArmorType.HeroArmor)
                    {
                        damage.DamageValue *= 0.5f;
                    }
                    break;
                }
            case ActorAttackType.Magic:
                {
                    if (targetActor.MyActor.ActorArmor.ActorArmorType == ActorArmorType.LightArmor)
                    {
                        damage.DamageValue *= 1.25f;
                    }
                    else if (targetActor.MyActor.ActorArmor.ActorArmorType == ActorArmorType.HeavyArmor)
                    {
                        damage.DamageValue *= 2f;
                    }
                    else if (targetActor.MyActor.ActorArmor.ActorArmorType == ActorArmorType.HeroArmor)
                    {
                        damage.DamageValue *= 0.35f;
                    }
                    break;
                }
            case ActorAttackType.Confuse:
                {
                    if (targetActor.MyActor.ActorArmor.ActorArmorType == ActorArmorType.LightArmor)
                    {
                        damage.DamageValue *= 1f;
                    }
                    else if (targetActor.MyActor.ActorArmor.ActorArmorType == ActorArmorType.HeavyArmor)
                    {
                        damage.DamageValue *= 1f;
                    }
                    else if (targetActor.MyActor.ActorArmor.ActorArmorType == ActorArmorType.HeroArmor)
                    {
                        damage.DamageValue *= 1f;
                    }
                    break;
                }
            case ActorAttackType.HeroAttack:
                {
                    if (targetActor.MyActor.ActorArmor.ActorArmorType == ActorArmorType.LightArmor)
                    {
                        damage.DamageValue *= 1.2f;
                    }
                    else if (targetActor.MyActor.ActorArmor.ActorArmorType == ActorArmorType.HeavyArmor)
                    {
                        damage.DamageValue *= 1.2f;
                    }
                    else if (targetActor.MyActor.ActorArmor.ActorArmorType == ActorArmorType.HeroArmor)
                    {
                        damage.DamageValue *= 1f;
                    }
                    break;
                }
        }
        return damage;
    }

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

    public void SendDamage(BaseGameEntity targeEntity, Damage damage)
    {
        Hashtable parameters = new Hashtable();
        parameters.Add("Damage", damage);
        MessageDispatcher.Instance().DispatchMessage(0f, this, targeEntity, FSMessageType.FSMessageAttack, parameters);
        if (damage.DamageValue > 0)
        {
            float bloodSuck = damage.DamageValue * this.BloodSuckingRatio * 0.01f;
            this.MyActor.CurrentHp = Mathf.Min(this.MyActor.CurrentHp + bloodSuck, this.MyActor.TotalHp);
        }
    }

    public void TakeDamage(Damage damage)
    {
        if (damage.ShowCrit)
        {
            this.ShowTip((-damage.DamageValue).ToString());
        }
		
		if(damage.Stun)
		{
			this.IsStun = true;
			this.StunDuration = Mathf.Max(damage.StunDuration, this.StunDuration);
		}
		if(damage.Bleed)
		{
			this.IsBleed = true;
			this.BleedDps = Mathf.Max(damage.BleedDps, this.BleedDps);
			this.BleedDuration = Mathf.Max(damage.BleedDuration, this.BleedDuration);
		}

        if (this.MyActor.ActorArmor.ArmorAmount > 0f)
        {
            float dValue = this.MyActor.ActorArmor.ArmorAmount - damage.DamageValue;
            if (dValue >= 0)
            {
                this.MyActor.ActorArmor.ArmorAmount = dValue;
            }
            else
            {
                this.MyActor.ActorArmor.ArmorAmount = 0f;
                damage.DamageValue = Mathf.Abs(dValue);
            }
        }
        
        if (this.MyActor.CurrentHp <= damage.DamageValue)
        {
			this.MyActor.CurrentHp = 0;
            this.m_PStateMachine.ChangeState(Actor_StateBeforeDie.Instance());
        }
		else
		{
			this.MyActor.CurrentHp -= damage.DamageValue;
		}
		this.MyActor.CurrentHp = Mathf.Min(this.MyActor.CurrentHp, this.MyActor.TotalHp);
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
        this.moveSpeed = 22.5f;
        this.AttackPlusRatio = 1;
        this.BloodSuckingRatio = 0;
        this.AttackSpeed = 1;
		this.hpbarLength = 200;
        this.RefreshHpBar();

        this.m_PStateMachine = new StateMachine<ActorController>(this);
        this.m_PStateMachine.SetCurrentState(Actor_StateWalk.Instance());
        this.m_PStateMachine.SetGlobalState(Actor_GlobalState.Instance());
    }

    private void RefreshHpBar()
    {
        if (this.hpBarSprite == null)
        {
            Transform hpBarTran = this.transform.FindChild("HpBar");
            this.hpBarSprite = hpBarTran.gameObject.GetComponent<tk2dSlicedSprite>();
        }
		else
		{
			this.hpBarSprite.dimensions = new Vector2(this.MyActor.CurrentHp/this.MyActor.TotalHp*this.hpbarLength, this.hpBarSprite.dimensions.y);
		}
    }

    private void ShowTip(string tip)
    {
        GameObject tipObj = (GameObject)Instantiate(Resources.Load("Tips/TipsTextMesh"));
        tipObj.transform.localPosition = this.myTransform.position + new Vector3(6, 20, -1);
        tipObj.GetComponent<tk2dTextMesh>().text = tip;
        tipObj.GetComponent<ActorTip>().Show();
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