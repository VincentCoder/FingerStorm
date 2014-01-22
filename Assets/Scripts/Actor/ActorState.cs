#region

using System.Collections;
using System.Collections.Generic;
using System.Text;

using UnityEngine;

#endregion

public class Actor_GlobalState : State<ActorController>
{
    #region Fields

    private Dictionary<ActorSpellName, ActorSpell> activeSpellDictionary;

    private Dictionary<ActorSpellName, float> releaseCounterDictionary;

    #endregion

    #region Public Methods and Operators

    public static Actor_GlobalState Instance()
    {
        return new Actor_GlobalState();
    }

    public override void Enter(ActorController entityType)
    {
        this.activeSpellDictionary = entityType.MyActor.GetSpellsByType(ActorSpellType.ActiveSpell);
        if (this.activeSpellDictionary != null)
        {
            this.releaseCounterDictionary = new Dictionary<ActorSpellName, float>();
            foreach (KeyValuePair<ActorSpellName, ActorSpell> kv in this.activeSpellDictionary)
            {
                this.releaseCounterDictionary.Add(kv.Key, 0f);
            }
        }
    }

    public override void Execute(ActorController entityType)
    {
        if (entityType.IsStun)
        {
            entityType.StunDuration -= Time.deltaTime;
            if (entityType.StunDuration <= 0)
            {
                entityType.IsStun = false;
            }
            return;
        }
        if (this.releaseCounterDictionary != null && this.activeSpellDictionary != null)
        {
            foreach (KeyValuePair<ActorSpellName, float> kv in this.releaseCounterDictionary)
            {
                float temp = kv.Value + Time.deltaTime;
                ActorSpell spell = this.activeSpellDictionary[kv.Key];
                if (temp >= spell.ReleaseInterval)
                {
                    this.ReleaseActiveSpell(entityType, spell);
                    temp = 0f;
                }
                this.releaseCounterDictionary[kv.Key] = temp;
            }
        }
    }

    public override void Exit(ActorController entityType)
    {
    }

    public override bool OnMessage(ActorController entityType, Telegram telegram)
    {
        if (telegram.Msg == FSMessageType.FSMessageAttack)
        {
			if (telegram.Parameters.ContainsKey("Damage") && telegram.Parameters.ContainsKey("AttackType"))
            {
                entityType.TakeDamage((float)telegram.Parameters["Damage"], (int)telegram.Parameters["AttackType"]);
                return true;
            }
            if (telegram.Parameters.ContainsKey("Damage"))
            {
                entityType.TakeDamage((float)telegram.Parameters["Damage"]);
                return true;
            }
        }
        else if (telegram.Msg == FSMessageType.FSMessageStun)
        {
            if (telegram.Parameters.ContainsKey("StunDuration"))
            {
                entityType.StunDuration = Mathf.Max((float)telegram.Parameters["StunDuration"], entityType.StunDuration);
                entityType.IsStun = true;
                return true;
            }
        }
        return false;
    }

    public void ReleaseActiveSpell(ActorController entityType, ActorSpell actorSpell)
    {
        switch (actorSpell.ActorSpellName)
        {
            case ActorSpellName.MortarAttack:
                {
                    List<GameObject> enemies = entityType.SeekAndGetEnemiesInDistance(actorSpell.AttackRange);
                    if (enemies != null && enemies.Count != 0)
                    {
                        this.SendDamage(entityType, actorSpell.DirectDamage * entityType.AttackPlusRatio);
                    }
                    break;
                }
            case ActorSpellName.ArcaneExplosion:
                {
                    List<GameObject> enemies = entityType.SeekAndGetEnemiesInDistance(actorSpell.AttackRange);
                    if (enemies != null && enemies.Count != 0)
                    {
                        this.SendDamage(entityType, actorSpell.DirectDamage * entityType.AttackPlusRatio);
                    }
                    break;
                }
        }
    }

    private void SendDamage ( ActorController entityType, float damage )
    {
        Hashtable parameters = new Hashtable();
        parameters.Add(
            "Damage",
            damage);
        MessageDispatcher.Instance()
            .DispatchMessage(
                0f,
                entityType,
                entityType.TargetEnemy,
                FSMessageType.FSMessageAttack,
                parameters);
    }

    #endregion
}

public class Actor_StateWalk : State<ActorController>
{
    #region Static Fields

    //private static Actor_StateWalk instance;

    #endregion

    #region Fields

    private float seekEnemyCounter;

    private float seekEnemyInterval = 0.5f;

    #endregion

    #region Public Methods and Operators

    public static Actor_StateWalk Instance()
    {
        return new Actor_StateWalk();
    }

    public override void Enter(ActorController entityType)
    {
        StringBuilder animName = new StringBuilder("Terran_");
        animName.Append(entityType.MyActor.ActorType);
        animName.Append("_Walk_");
        animName.Append(entityType.MyActor.FactionType);
        entityType.SelfAnimator.Play(animName.ToString());

        this.ResetRotation(entityType);
    }

    public override void Execute(ActorController entityType)
    {
        if(entityType.IsStun)
            return;
        Vector3 moveDistance = entityType.moveSpeed * Time.deltaTime
                               * (entityType.ActorPath.CurrentNode() - entityType.myTransform.position)
                                     .normalized;
		entityType.myTransform.Translate(moveDistance, Space.World);
		if(entityType.ActorPath.CurrentIndex == entityType.ActorPath.NodesCount() - 1)
		{
			if(Vector3.Distance(entityType.ActorPath.CurrentNode(), entityType.myTransform.position) <= entityType.MyActor.ActorAttack.AttackRange)
			{
				entityType.TargetEnemy = entityType.TargetBuilding.GetComponent<BuildingController>();
				entityType.GetFSM().ChangeState(Actor_StateFight.Instance());
				return;
			}
		}
		if((entityType.ActorPath.CurrentNode() - entityType.myTransform.position).sqrMagnitude <= 0.5)
		{
		    if (entityType.ActorPath.HasNext())
		    {
                entityType.ActorPath.NextNode();
                this.ResetRotation(entityType);
		    }
		}

        this.seekEnemyCounter += Time.deltaTime;
        if (this.seekEnemyCounter >= this.seekEnemyInterval)
        {
            if (entityType.SeekAndGetEnemies(false).Count != 0)
            {
                entityType.GetFSM().ChangeState(Actor_StateBeforeFight.Instance());
            }
			else 
			{	
				List<GameObject> enemies = entityType.SeekAndGetEnemiesInDistance(entityType.MyActor.ActorAttack.AttackRange, true);
				if(enemies.Count != 0)
				{
					entityType.TargetEnemy = enemies[0].GetComponent<ActorController>();
					entityType.GetFSM().ChangeState(Actor_StateFight.Instance());
				}
			}
            this.seekEnemyCounter = 0.0f;
        }
        //base.Execute(entityType);
    }

    public override void Exit(ActorController entityType)
    {
		//Debug.Log("Exit Walk");
        base.Exit(entityType);
    }

    public override bool OnMessage(ActorController entityType, Telegram telegram)
    {
        return base.OnMessage(entityType, telegram);
    }

    private void ResetRotation ( ActorController entityType )
    {
        Quaternion newQuaternion = Quaternion.Euler(0, 0, 0);
        if (entityType.ActorPath.CurrentNode().x < entityType.myTransform.position.x)
        {
            newQuaternion.eulerAngles = new Vector3(0, 180, 0);
            entityType.gameObject.transform.rotation = newQuaternion;
        }
        else
        {
            newQuaternion.eulerAngles = new Vector3(0, 0, 0);
            entityType.gameObject.transform.rotation = newQuaternion;
        }
    }

    #endregion
}

public class Actor_StateBeforeFight : State<ActorController>
{
    #region Static Fields

    //private static Actor_StateBeforeFight instance;

    #endregion

    #region Public Methods and Operators

    public static Actor_StateBeforeFight Instance()
    {
        //return instance ?? (instance = new Actor_StateBeforeFight());
		return new Actor_StateBeforeFight();
    }

    public override void Enter(ActorController entityType)
    {
		//Debug.Log("Enter BeforeFight");
		
		StringBuilder animName = new StringBuilder("Terran_");
        animName.Append(entityType.MyActor.ActorType);
        animName.Append("_Walk_");
        animName.Append(entityType.MyActor.FactionType);
        entityType.SelfAnimator.Play(animName.ToString());

        List<GameObject> enemies = entityType.SeekAndGetEnemies();
        if (enemies == null || enemies.Count == 0)
        {
            entityType.GetFSM().ChangeState(Actor_StateWalk.Instance());
            return;
        }
        entityType.TargetEnemy = enemies[0].GetComponent<ActorController>();
        //base.Enter(entityType);
    }

    public override void Execute(ActorController entityType)
    {
        if(entityType.IsStun)
            return;

        if (entityType.TargetEnemy == null)
        {
            entityType.GetFSM().ChangeState(Actor_StateWalk.Instance());
            return;
        }

        Vector3 moveDistance = entityType.moveSpeed * Time.deltaTime
                               * (entityType.TargetEnemy.transform.position - entityType.myTransform.position)
                                     .normalized;
        entityType.myTransform.Translate(moveDistance, Space.World);

        if ((entityType.TargetEnemy.transform.position - entityType.myTransform.position).sqrMagnitude
            <= Mathf.Max(Mathf.Pow(entityType.MyActor.ActorAttack.AttackRange, 2), Mathf.Pow(20, 2)))
        {
            entityType.GetFSM().ChangeState(Actor_StateFight.Instance());
        }

        //base.Execute(entityType);
    }

    public override void Exit(ActorController entityType)
    {
		//Debug.Log("Exit BeforeFight");
        base.Exit(entityType);
    }

    public override bool OnMessage(ActorController entityType, Telegram telegram)
    {
        return base.OnMessage(entityType, telegram);
    }

    #endregion
}

public class Actor_StateFight : State<ActorController>
{
    #region Static Fields

    //private static Actor_StateFight instance;
    private float attackSpeedCounter;

    #endregion

    #region Public Methods and Operators

    public static Actor_StateFight Instance()
    {
        //return instance ?? (instance = new Actor_StateFight());
		return new Actor_StateFight();
    }

    public override void Enter(ActorController entityType)
    {
        this.attackSpeedCounter = 0.0f;
        if (entityType.TargetEnemy == null)
        {
            entityType.GetFSM().ChangeState(Actor_StateBeforeFight.Instance());
            return;
        }
        StringBuilder animName = new StringBuilder("Terran_");
        animName.Append(entityType.MyActor.ActorType);
        animName.Append("_Attack_");
        animName.Append(entityType.MyActor.FactionType);
        entityType.SelfAnimator.Play(animName.ToString());
        entityType.SelfAnimator.AnimationCompleted = delegate 
            {
                entityType.SelfAnimator.SetFrame(0);
				this.SendDamage(entityType);
            };
    }

    public override void Execute(ActorController entityType)
    {
        if(entityType.IsStun)
            return;
        if (entityType.TargetEnemy == null)
        {
            entityType.GetFSM().ChangeState(Actor_StateBeforeFight.Instance());
            return;
        }

        this.attackSpeedCounter += Time.deltaTime;
        if (this.attackSpeedCounter >= entityType.AttackSpeed)
        {
            entityType.SelfAnimator.Play();
            entityType.SelfAnimator.AnimationCompleted = delegate
                {
                    entityType.SelfAnimator.SetFrame(0);
                    this.SendDamage(entityType);
                };
            this.attackSpeedCounter = 0f;
        }
    }

    public override void Exit(ActorController entityType)
    {
		//Debug.Log("Exit Fight");
        base.Exit(entityType);
    }

    public override bool OnMessage(ActorController entityType, Telegram telegram)
    {
        return base.OnMessage(entityType, telegram);
    }

    #endregion

    #region Methods

    private void SendDamage(ActorController entityType)
    {
        Dictionary<ActorSpellName, ActorSpell> passiveSpellDictionary =
            entityType.MyActor.GetSpellsByType(ActorSpellType.PassiveSpell);
		bool isBaseAttackSent = false;
        if (passiveSpellDictionary != null)
        {
            foreach (KeyValuePair<ActorSpellName, ActorSpell> kv in passiveSpellDictionary)
            {
                switch (kv.Key)
                {
                    case ActorSpellName.None:
                        {
                            this.SendAttack(
                                entityType,
                                entityType.CalculateCommonAttackDamageToActor((ActorController)entityType.TargetEnemy));
                            break;
                        }
                    case ActorSpellName.CirticalStrike:
                        {
                            if (Random.Range(1, 101) <= 10)
                            {
                            }
                            break;
                        }

                }






                float probability = kv.Value.DamageBonusPercentProbability;
                int randomIndex;
                if (probability > 0)
                {
                    randomIndex = Random.Range(1, 101);
					if(randomIndex <= probability)
					{
						isBaseAttackSent = true;
					}
                }
				int percent = kv.Value.SplashPercent;
				if(percent > 0)
				{
					List<GameObject> enemies = entityType.SeekAndGetEnemiesInDistance(kv.Value.AttackRange);
					if(enemies != null && enemies.Count != 0)
					{
						enemies.ForEach(enemy => 
						{
							ActorController actorCtrl = enemy.GetComponent<ActorController>();
							if(actorCtrl != null && actorCtrl.gameObject != entityType.TargetEnemy.gameObject)
							{
								this.SendAttack(entityType, actorCtrl, 0.5f * entityType.MyActor.ActorAttack.Dps);
							}
						});
					}
				}
				probability = kv.Value.StunProbability;
				if(probability > 0)
				{
					randomIndex = Random.Range(1, 101);
					if(randomIndex <= probability)
					{
						this.SendStun(entityType, kv.Value.StunDuration);
					}
				}
				probability = kv.Value.DirectDamageProbability;
				if(probability > 0)
				{
					randomIndex = Random.Range(1, 101);
					if(randomIndex <= probability)
					{
						isBaseAttackSent = true;
					}
				}
            }
        }
    }
	
	private void SendAttack(ActorController entityType, Damage damage)
	{
		Hashtable parameters = new Hashtable();
        parameters.Add(
            "Damage",
            damage);
        MessageDispatcher.Instance()
            .DispatchMessage(
                0f,
                entityType,
                entityType.TargetEnemy,
                FSMessageType.FSMessageAttack,
                parameters);
	}
						
	private void SendAttack(ActorController entityType, ActorController targetEntity, float damage)
	{
		Hashtable parameters = new Hashtable();
        parameters.Add(
            "Damage",
            damage);
        MessageDispatcher.Instance()
            .DispatchMessage(
                0f,
                entityType,
                targetEntity,
                FSMessageType.FSMessageAttack,
                parameters);
	}	
	
	private void SendStun(ActorController entityType, float duration)
	{
		Hashtable parameters = new Hashtable();
		parameters.Add("StunDuration", duration);
		MessageDispatcher.Instance().DispatchMessage(0f, entityType, entityType.TargetEnemy,
			FSMessageType.FSMessageStun, parameters);
	}

    #endregion
}

public class Actor_StateBeforeDie : State<ActorController>
{
    #region Static Fields

    //private static Actor_StateBeforeDie instance;

    #endregion

    #region Public Methods and Operators

    public static Actor_StateBeforeDie Instance()
    {
        //return instance ?? (instance = new Actor_StateBeforeDie());
		return new Actor_StateBeforeDie();
    }

    public override void Enter(ActorController entityType)
    {
        StringBuilder animName = new StringBuilder("Terran_");
        animName.Append(entityType.MyActor.ActorType);
        animName.Append("_Die_");
        animName.Append(entityType.MyActor.FactionType);
        entityType.SelfAnimator.Play(animName.ToString());
        entityType.SelfAnimator.AnimationCompleted =
            delegate { entityType.GetFSM().ChangeState(Actor_StateDie.Instance()); };
    }

    public override void Execute(ActorController entityType)
    {
    }

    public override void Exit(ActorController entityType)
    {
    }

    public override bool OnMessage(ActorController entityType, Telegram telegram)
    {
        return base.OnMessage(entityType, telegram);
    }

    #endregion
}

public class Actor_StateDie : State<ActorController>
{
    #region Static Fields

    //private static Actor_StateDie instance;

    #endregion

    #region Public Methods and Operators

    public static Actor_StateDie Instance()
    {
        //return instance ?? (instance = new Actor_StateDie());
		return new Actor_StateDie();
    }

    public override void Enter(ActorController entityType)
    {
        GameController gameCtrl = GameObject.Find("GameController").GetComponent<GameController>();
        if (gameCtrl != null)
        {
            if (gameCtrl.GameType == GameType.PVE)
            {
                if (entityType.MyActor.FactionType != gameCtrl.MyFactionType)
                {
                    GameSceneController gameSceneCtrl =
                        GameObject.Find("GameSceneController").GetComponent<GameSceneController>();
                    if (gameSceneCtrl != null)
                    {
                        switch (entityType.MyActor.ActorLevel)
                        {
                            case ActorLevel.Normal:
                                gameSceneCtrl.CoinCount += 5;
                                break;
                            case ActorLevel.Senior:
                                gameSceneCtrl.CoinCount += 10;
                                break;
                            case ActorLevel.Hero:
                                gameSceneCtrl.CoinCount += 50;
                                break;
                        }
                    }
                }
            }
            else
            {
                if (entityType.MyActor.FactionType != gameCtrl.MyFactionType)
                {
                    GameSceneController gameSceneCtrl =
                        GameObject.Find("GameSceneController").GetComponent<GameSceneController>();
                    if (gameSceneCtrl != null)
                    {
                        switch (entityType.MyActor.ActorLevel)
                        {
                            case ActorLevel.Normal:
                                gameSceneCtrl.CoinCount += 5;
                                break;
                            case ActorLevel.Senior:
                                gameSceneCtrl.CoinCount += 10;
                                break;
                            case ActorLevel.Hero:
                                gameSceneCtrl.CoinCount += 50;
                                break;
                        }
                    }
                }
            }
        }
        entityType.DestroySelf();
    }

    public override void Execute(ActorController entityType)
    {
        base.Execute(entityType);
    }

    public override void Exit(ActorController entityType)
    {
        base.Exit(entityType);
    }

    public override bool OnMessage(ActorController entityType, Telegram telegram)
    {
        return base.OnMessage(entityType, telegram);
    }

    #endregion
}