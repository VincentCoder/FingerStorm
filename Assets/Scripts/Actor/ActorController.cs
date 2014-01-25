#region

using System.Collections;
using System.Collections.Generic;

using UnityEngine;

#endregion

public class ActorController : BaseGameEntity
{
    #region Fields

    public float AttackInterval;

    public float moveSpeed;

    public Transform myTransform;

    private bool _isStun;

    private Actor _myActor;

    private tk2dSpriteAnimator _selfAnimator;

    private tk2dSpriteAnimator stunEffectAnimator;

    private tk2dSpriteAnimator bleedEffectAnimator;

    private tk2dSlicedSprite hpBarSprite;

    private float hpbarLength;

    private bool isBleed;

    private bool isRaging;

    private StateMachine<ActorController> m_PStateMachine;

    #endregion

    #region Public Properties

    public ActorPath ActorPath { get; set; }

    public float AttackPlusRatio { get; set; }

    public float BleedDps { get; set; }

    public float BleedDuration { get; set; }

    public int BloodSuckingRatio { get; set; }

    public bool IsBleed
    {
        get
        {
            return this.isBleed;
        }
        set
        {
            this.isBleed = value;
            if (this.isBleed)
            {
                if (this.bleedEffectAnimator == null)
                {
                    Debug.Log(this.MyActor.ActorType + " Play Bleed Animation");
                    GameObject bleedEffect = (GameObject)Instantiate(Resources.Load("GameScene/ActorSkillEffect"));
                    bleedEffect.name = "BleedEffect";
                    bleedEffect.transform.parent = this.myTransform;
                    bleedEffect.transform.localPosition = new Vector3(1.5f,20f,0f);
                    this.bleedEffectAnimator = bleedEffect.GetComponent<tk2dSpriteAnimator>();
                    this.bleedEffectAnimator.Play("Bleed");
                }
            }
            else
            {
                if (this.bleedEffectAnimator  != null)
                {
                    Destroy(this.bleedEffectAnimator.gameObject);
                }
            }
        }
    }

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
                if (this.stunEffectAnimator == null)
                {
                    GameObject stunEffect = (GameObject)Instantiate(Resources.Load("GameScene/ActorSkillEffect"));
                    stunEffect.name = "StunEffect";
                    stunEffect.transform.parent = this.myTransform;
                    stunEffect.transform.localPosition = new Vector3(5.4f, 20f, 0f);
                    this.stunEffectAnimator = stunEffect.GetComponent<tk2dSpriteAnimator>();
                    this.stunEffectAnimator.Play("Dizzy");
                }
            }
            else
            {
                if (this.SelfAnimator.Paused)
                {
                    this.SelfAnimator.Resume();
                }
                if (this.stunEffectAnimator  != null)
                {
                    Destroy(this.stunEffectAnimator.gameObject);
                }
            }
        }
    }

    public bool IsRaging
    {
        get
        {
            return this.isRaging;
        }
        set
        {
            this.isRaging = value;
            if (this.isRaging)
            {
                this.myTransform.localScale = new Vector3(1.2f, 1.2f, 1f);
                this.RagingDuration = 6;
                this.AttackInterval = 0.5f;
                this.moveSpeed *= 1.25f;
            }
            else
            {
                this.myTransform.localScale = new Vector3(1,1,1);
                this.AttackInterval = 1f;
                this.moveSpeed *= 1.25f;
            }
        }
    }

    public float RagingDuration { get; set; }

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
        damage.DamageValue = this.MyActor.ActorAttack.Dps / this.AttackInterval;
        Debug.Log("Damage " + damage.DamageValue);
        damage.DamageValue *= this.AttackPlusRatio;
        Debug.Log("After plus Ratio " + damage.DamageValue);
        if (!(targetEntity is ActorController))
        {
            return damage;
        }

        ActorController targetActor = (ActorController)targetEntity;
        ActorSpell dodgeSpell = targetActor.MyActor.GetSpell(ActorSpellName.Dodge);
        if (dodgeSpell != null)
        {
            int randomIndex = Random.Range(1, 101);
            if (targetActor.MyActor.ActorType == ActorType.Supporter && randomIndex <= 15)
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
                                Debug.Log("致命一击");
                                damage.DamageValue *= 1.5f;
                                damage.ShowCrit = true;
								damage.ActorSpellName = ActorSpellName.CirticalStrike;
                                Debug.Log("After Cirtical Strike " + damage.DamageValue);
                            }
                            break;
                        }
                    case ActorSpellName.HeadShot:
                        {
                            if (Random.Range(1, 101) <= 40)
                            {
                                damage.DamageValue *= 3f;
                                damage.ShowCrit = true;
								damage.ActorSpellName = ActorSpellName.HeadShot;
                                Debug.Log("After HeadShot" + damage.DamageValue);
                            }
                            break;
                        }
                    case ActorSpellName.SplashDamage:
                        {
                            List<GameObject> enemies = targetActor.SeekAndGetFriendlyActorsInDistance(100);
                            if (enemies != null && enemies.Count != 0)
                            {
                                Damage splashDamage = new Damage();
                                splashDamage.DamageValue = 0.5f * damage.DamageValue;
                                Debug.Log("splash Damage " + splashDamage.DamageValue);
                                enemies.ForEach(
                                    enemy =>
                                        {
                                            if (enemy != null && enemy != targetActor.gameObject)
                                            {
                                                ActorController actorCtrl = enemy.GetComponent<ActorController>();
                                                if (actorCtrl != null
                                                    && actorCtrl.gameObject != this.TargetEnemy.gameObject)
                                                {
                                                    this.SendDamage(actorCtrl, splashDamage);
                                                }
                                            }
                                        });
                            }
                            break;
                        }
                    case ActorSpellName.Bleed:
                        {
                            if (this.MyActor.ActorType == ActorType.GryphonRider)
                            {
                                if (Random.Range(1, 101) <= 25)
                                {
                                    Debug.Log("Bleed");
                                    damage.Bleed = true;
                                    damage.BleedDuration = 3;
                                    damage.BleedDps = 15;
                                }
                            }
                            else if (this.MyActor.ActorType == ActorType.SeniorGryphonRider)
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
                            if (this.MyActor.ActorType == ActorType.Crusader)
                            {
                                if (Random.Range(1, 101) <= 20)
                                {
                                    Debug.Log("重击");
                                    damage.Stun = true;
                                    damage.StunDuration = 2;
                                    damage.DamageValue += 25;
                                    damage.ShowCrit = true;
                                }
                            }
                            else if (this.MyActor.ActorType == ActorType.TemplarWarrior)
                            {
                                if (Random.Range(1, 101) <= 25)
                                {
                                    Debug.Log("重击");
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
                            if (this.MyActor.ActorType == ActorType.Sage)
                            {
                                if (Random.Range(1, 101) <= 25 && this.TargetEnemy != null)
                                {
                                    /*Debug.Log("闪电链");
                                    GameObject chainLightningAEffect = (GameObject)Instantiate(Resources.Load("GameScene/ActorSkillEffect"));
                                    chainLightningAEffect.name = "ChainLightningAEffect";
                                    chainLightningAEffect.transform.position = this.myTransform.position;
                                    this.SetRotation(
                                        chainLightningAEffect,
                                        this.TargetEnemy.transform.position);
                                    float scale = Vector3.Distance(
                                        this.myTransform.position,
                                        this.TargetEnemy.transform.position) / 32f;
                                    chainLightningAEffect.transform.localScale = new Vector3(scale, 1f, 1f);
                                    tk2dSpriteAnimator animator = chainLightningAEffect.GetComponent<tk2dSpriteAnimator>();
                                    animator.Play("ChainLightningA");
                                    animator.AnimationCompleted = delegate
                                        {
                                            Destroy(chainLightningAEffect);
                                            Debug.Log("1111");
                                            if (this.TargetEnemy != null)
                                            {
                                                Debug.Log("Test");
                                                GameObject chainLightningBEffect =
                                                (GameObject)Instantiate(Resources.Load("GameScene/ActorSkillEffect"));
                                                chainLightningBEffect.name = "ChainLightningBEffect";
                                                chainLightningBEffect.transform.parent = this.TargetEnemy.transform;
                                                chainLightningBEffect.transform.localPosition = new Vector3(0,0,0);
                                                Damage chainLightningDamage = new Damage();
                                                chainLightningDamage.DamageValue = 200;
                                                chainLightningDamage.ShowCrit = true;
                                                //this.SendDamage(this.TargetEnemy, chainLightningDamage);
                                                tk2dSpriteAnimator animator1 =
                                                    chainLightningBEffect.GetComponent<tk2dSpriteAnimator>();
                                                animator1.Play("ChainLightningB");
                                                animator1.AnimationCompleted = delegate
                                                    {
                                                        Destroy(chainLightningBEffect);
                                                        Debug.Log("2222");
                                                        if (this.TargetEnemy != null)
                                                        {
                                                            List<GameObject> friendlyActors =
                                                                targetActor.SeekAndGetFriendlyActorsInDistance(150);
                                                            int attackCount = Mathf.Min(friendlyActors.Count, 3);
                                                            Debug.Log(attackCount);
                                                            GameObject preGameObject = targetActor.gameObject;
                                                            for (int i = 0; i < attackCount; i ++)
                                                            {
                                                                GameObject target = friendlyActors[i];
                                                                Debug.Log("3333");
                                                                if (target != targetActor.gameObject && preGameObject != null)
                                                                {
                                                                    Debug.Log("xxx");
                                                                    GameObject chainLightningAEffect1 = (GameObject)Instantiate(Resources.Load("GameScene/ActorSkillEffect"));
                                                                    chainLightningAEffect1.name = "ChainLightningAEffect";
                                                                    chainLightningAEffect1.transform.position =
                                                                        preGameObject.transform.position;
                                                                    this.SetRotation(
                                                                        chainLightningAEffect1,
                                                                        this.TargetEnemy.transform.position);
                                                                    float scale1 = Vector3.Distance(
                                                                        target.transform.position,
                                                                        preGameObject.transform.position) / 32f;
                                                                    chainLightningAEffect1.transform.localScale = new Vector3(scale1, 1f, 1f);
                                                                    tk2dSpriteAnimator animator2 = chainLightningAEffect1.GetComponent<tk2dSpriteAnimator>();
                                                                    animator2.Play("ChainLightningA");
                                                                    animator2.AnimationCompleted = delegate
                                                                        {
                                                                            Destroy(chainLightningAEffect1);
                                                                            GameObject chainLightningBEffect1 =
                                                                                (GameObject)
                                                                                Instantiate(
                                                                                    Resources.Load(
                                                                                        "GameScene/ActorSkillEffect"));
                                                                            chainLightningBEffect1.name = "ChainLightningBEffect";
                                                                            chainLightningBEffect1.transform.parent =
                                                                                target.transform;
                                                                            chainLightningBEffect1.transform.localPosition = new Vector3(0, 0, 0);
                                                                            this.SendDamage(target.GetComponent<ActorController>(), chainLightningDamage);
                                                                            tk2dSpriteAnimator animator3 =
                                                                                chainLightningBEffect1
                                                                                    .GetComponent<tk2dSpriteAnimator>();
                                                                            animator3.Play("ChainLightningB");
                                                                            animator3.AnimationCompleted = delegate
                                                                                {
                                                                                    Destroy(chainLightningBEffect1);
                                                                                };
                                                                        };
                                                                    preGameObject = target;
                                                                }
                                                            }
                                                        }
                                                    };
                                            }
                                        };*/
                                }
                            }
                            break;
                        }
                    case ActorSpellName.FireBomb:
                        {
                            Debug.Log("燃烧弹 " + targetActor);
                            if (targetActor != null)
                            {
                                if (this.MyActor.ActorType == ActorType.BatRider)
                                {
                                    targetActor.AttackInterval *= 1.6f;
                                    targetActor.MyActor.ActorArmor.ArmorAmount =
                                    Mathf.Max(targetActor.MyActor.ActorArmor.ArmorAmount - 3, 0);
                                }
                                else
                                {
                                    targetActor.AttackInterval *= 1.6f;
                                    targetActor.MyActor.ActorArmor.ArmorAmount =
                                    Mathf.Max(targetActor.MyActor.ActorArmor.ArmorAmount - 6, 0);
                                }
                            }
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
        Debug.Log("After armor " + damage.DamageValue);
        return damage;
    }

    private void SetRotation ( GameObject chainLightning, Vector3 targetPos )
    {
        Vector3 targetDirection = (targetPos - chainLightning.transform.position).normalized;
        Vector3 xPositiveDirection = new Vector3(1, 0, 0);
         Quaternion newQuaternion = Quaternion.Euler(0, 0, 0);
         newQuaternion.eulerAngles = new Vector3(0, 0, Mathf.Acos(Vector3.Dot(xPositiveDirection.normalized, targetDirection.normalized))*Mathf.Rad2Deg);
         chainLightning.transform.rotation = newQuaternion;
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

    public RoadBlockController HasRoadBlockInFront()
    {
        if (this.ActorPath.pathType == ActorPathType.FirstPath)
        {
        }
        return null;
    }

    public List<GameObject> SeekAndGetEnemies(bool ignoreObstacles = true, bool ignoreAir = true)
    {
        List<GameObject> result = ActorsManager.GetInstance()
            .GetEnemyActorsInDistanceAndSortByDistance(this, this.MyActor.ActorAttack.ViewDistance, ignoreObstacles);
        if (!ignoreAir && !this.MyActor.AttackAirForce)
        {
            for (int i = 0; i < result.Count; i ++)
            {
                if (result[i] != null)
                {
                    ActorController actorCtrl = result[i].GetComponent<ActorController>();
                    if (actorCtrl.MyActor.IsAirForce)
                    {
                        result.RemoveAt(i);
                    }
                }
            }
        }
        return result;
    }

    public List<GameObject> SeekAndGetEnemiesInDistance(
        int distance,
        bool ignoreObstacles = true,
        bool ignoreAir = true)
    {
        List<GameObject> result = ActorsManager.GetInstance()
            .GetEnemyActorsInDistanceAndSortByDistance(this, distance, ignoreObstacles);
        if (!ignoreAir && !this.MyActor.AttackAirForce)
        {
            for (int i = 0; i < result.Count; i++)
            {
                if (result[i] != null)
                {
                    ActorController actorCtrl = result[i].GetComponent<ActorController>();
                    if (actorCtrl.MyActor.IsAirForce)
                    {
                        result.RemoveAt(i);
                    }
                }
            }
        }
        return result;
    }

    public List<GameObject> SeekAndGetFriendlyActorsInDistance (
        int distance,
        bool ignoreObstacles = true,
        bool ignoreAir = true )
    {
        List<GameObject> result = ActorsManager.GetInstance()
            .GetFriendlyActorsInDistance(this.MyActor.FactionType, this.myTransform.position, distance, ignoreObstacles);
        if (!ignoreAir && !this.MyActor.AttackAirForce)
        {
            for (int i = 0; i < result.Count; i++)
            {
                if (result[i] != null)
                {
                    ActorController actorCtrl = result[i].GetComponent<ActorController>();
                    if (actorCtrl.MyActor.IsAirForce)
                    {
                        result.RemoveAt(i);
                    }
                }
            }
        }
        return result;
    }

    public List<GameObject> SeekAndGetEnemyBuildings()
    {
        return BuildingsManager.GetInstance()
            .GetEnemyBuildingsInDistanceAndSortByDistance(this, this.MyActor.ActorAttack.ViewDistance);
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

    public void TakeDamage(Damage damage, BaseGameEntity fromGameEntity = null)
    {
        //Debug.Log(this.MyActor.FactionType + " DamageValue " + damage.DamageValue);
        if (damage.ShowCrit)
        {
            int temp = Mathf.RoundToInt(-damage.DamageValue);
            if (temp < 0)
            {
                this.ShowTip(temp.ToString());
            }
            else
            {
                this.ShowTip("+" + temp.ToString(), 1);
            }
        }

        GameObject damageEffect = (GameObject)Instantiate(Resources.Load("GameScene/ActorSkillEffect"));
        damageEffect.name = "ShortRangeWeaponDamageEffect";
        damageEffect.transform.parent = this.myTransform;
        tk2dSpriteAnimator animator = damageEffect.GetComponent<tk2dSpriteAnimator>();

		if(damage.ActorSpellName == ActorSpellName.CirticalStrike || damage.ActorSpellName == ActorSpellName.HeadShot)
		{
            animator.Play("ShortRangeWeaponDamage"); 
		}
		else
		{
            animator.Play("CommonDamageEffect"); 
		}
        animator.AnimationCompleted = delegate
        {
            Destroy(damageEffect);
        };

        if (damage.Stun)
        {
            this.IsStun = true;
            this.StunDuration = Mathf.Max(damage.StunDuration, this.StunDuration);
        }
        if (damage.Bleed)
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
            if (fromGameEntity != null && fromGameEntity is ActorController)
            {
                ((ActorController)fromGameEntity).KilledSomeEntity(this);
            }
            this.MyActor.CurrentHp = 0;
            this.m_PStateMachine.ChangeState(Actor_StateBeforeDie.Instance());
        }
        else
        {
            this.MyActor.CurrentHp -= damage.DamageValue;
        }
        this.MyActor.CurrentHp = Mathf.Min(this.MyActor.CurrentHp, this.MyActor.TotalHp);
        this.RefreshHpBar();
        //Debug.Log(this.MyActor.FactionType + " " + this.MyActor.CurrentHp);
    }

    public void SendBullet(BaseGameEntity gameEntity, BulletType bulletType)
    {
        if (gameEntity != null)
        {
            GameObject bullet = (GameObject)Instantiate(Resources.Load("GameScene/Bullet"));
            bullet.name = string.Empty + bulletType;
            bullet.transform.position = this.transform.position;
            BulletController bulletCtrl = bullet.GetComponent<BulletController>();
            bulletCtrl.Target = gameEntity;
            bulletCtrl.BulletType = bulletType;
        }
    }

    public void KilledSomeEntity(BaseGameEntity killedEntity)
    {
        if (this.MyActor.HasSpell(ActorSpellName.Rage))
        {
            this.IsRaging = true;
        }
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
        this.AttackInterval = 1;
        this.hpbarLength = 200;
        this.RefreshHpBar();
        this.SwitchAnimation();

        this.m_PStateMachine = new StateMachine<ActorController>(this);
        this.m_PStateMachine.SetCurrentState(Actor_StateWalk.Instance());
        this.m_PStateMachine.SetGlobalState(Actor_GlobalState.Instance());
    }

    private void SwitchAnimation()
    {
        this.SelfAnimator.Library =
            Resources.Load("Animation/" + this.MyActor.RaceType + "ActorAnimation", typeof(tk2dSpriteAnimation)) as
            tk2dSpriteAnimation;
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
            this.hpBarSprite.dimensions = new Vector2(
                this.MyActor.CurrentHp / this.MyActor.TotalHp * this.hpbarLength,
                this.hpBarSprite.dimensions.y);
        }
    }

    private void ShowTip(string tip, int gradient = 7)
    {
        GameObject tipObj = (GameObject)Instantiate(Resources.Load("Tips/TipsTextMesh"));
        tipObj.transform.parent = this.myTransform;
        tipObj.transform.localPosition = new Vector3(6, 20, -1);
        tk2dTextMesh textMesh = tipObj.GetComponent<tk2dTextMesh>();
        textMesh.text = tip;
        textMesh.textureGradient = gradient;
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