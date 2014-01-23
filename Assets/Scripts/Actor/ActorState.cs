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
				entityType.StunDuration = 0;
            }
            return;
        }
		
		if(entityType.IsBleed)
		{
			entityType.BleedDuration -= Time.deltaTime;
			Damage bleedDamage = new Damage();
			bleedDamage.DamageValue = entityType.BleedDps * Time.deltaTime;
			entityType.TakeDamage(bleedDamage);
			if(entityType.BleedDuration <= 0)
			{
				entityType.IsBleed = false;
				entityType.BleedDps = 0;
				entityType.BleedDuration = 0;
			}
		}
		
        if (this.releaseCounterDictionary != null && this.activeSpellDictionary != null)
        {
            //foreach (KeyValuePair<ActorSpellName, float> kv in this.releaseCounterDictionary)
			ActorSpellName[] actorSpellArray = new ActorSpellName[this.releaseCounterDictionary.Keys.Count];
			float[] counterArray = new float[this.releaseCounterDictionary.Keys.Count];
			this.releaseCounterDictionary.Keys.CopyTo(actorSpellArray, 0);
			this.releaseCounterDictionary.Values.CopyTo(counterArray, 0);
			for(int i = 0; i < counterArray.Length; i ++)	
            {
                float temp = counterArray[i] + Time.deltaTime;
                ActorSpell spell = this.activeSpellDictionary[actorSpellArray[i]];
                if (temp >= spell.ReleaseInterval)
                {
                    this.ReleaseActiveSpell(entityType, spell);
                    temp = 0f;
                }
                this.releaseCounterDictionary[actorSpellArray[i]] = temp;
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
			if (telegram.Parameters.ContainsKey("Damage"))
            {
                entityType.TakeDamage((Damage)telegram.Parameters["Damage"]);
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
                    List<GameObject> enemies = entityType.SeekAndGetEnemiesInDistance(40);
                    if (enemies != null && enemies.Count != 0)
                    {
						Damage motarAttackDamage = new Damage();
						motarAttackDamage.DamageValue = 21;
						motarAttackDamage.ShowCrit = true;
						enemies.ForEach(enemy => 
						{
							ActorController actorCtrl = enemy.GetComponent<ActorController>();
							entityType.SendDamage(actorCtrl, motarAttackDamage);
						});
                    }
                    break;
                }
            case ActorSpellName.ArcaneExplosion:
                {
                    List<GameObject> enemies = entityType.SeekAndGetEnemiesInDistance(50);
                    if (enemies != null && enemies.Count != 0)
                    {
                        Damage motarAttackDamage = new Damage();
						motarAttackDamage.DamageValue = 27;
						motarAttackDamage.ShowCrit = true;
						enemies.ForEach(enemy => 
						{
							ActorController actorCtrl = enemy.GetComponent<ActorController>();
							entityType.SendDamage(actorCtrl, motarAttackDamage);
						});
                    }
                    break;
                }
			case ActorSpellName.Zap:
			{
				List<GameObject> myActors = ActorsManager.GetInstance().GetActorsOfFaction(entityType.MyActor.FactionType);
				if(myActors != null && myActors.Count != 0)
				{
					GameObject targetActor = myActors[Random.Range(0, myActors.Count)];
					Damage zapDamage = new Damage();
					zapDamage.DamageValue = -300;
					entityType.SendDamage(targetActor.GetComponent<ActorController>(), zapDamage);
				}
				break;
			}
        }
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
            <= Mathf.Max(Mathf.Pow(entityType.MyActor.ActorAttack.AttackRange, 2), Mathf.Pow(30, 2)))
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

    private GameObject bloodBustVampire;

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
				entityType.SendDamage(entityType.TargetEnemy, entityType.CalculateCommonAttackDamage(entityType.TargetEnemy));
            };
        this.PlayVampireAnimation(entityType);
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
					entityType.SendDamage(entityType.TargetEnemy, entityType.CalculateCommonAttackDamage(entityType.TargetEnemy));
                };
            this.PlayVampireAnimation(entityType);
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

    private void PlayVampireAnimation(ActorController entityType)
    {
        if (entityType.BloodSuckingRatio > 0)
        {
            if (this.bloodBustVampire == null)
                bloodBustVampire = (GameObject)Object.Instantiate(Resources.Load("GameScene/PlayerSkillBloodlust"));
            bloodBustVampire.SetActive(true);
            bloodBustVampire.transform.parent = entityType.myTransform;
            tk2dSpriteAnimator vampireAnimator = bloodBustVampire.GetComponent<tk2dSpriteAnimator>();
            vampireAnimator.Play("Vampire");
            vampireAnimator.AnimationCompleted = delegate
            {
                bloodBustVampire.SetActive(false);
            };
        }
        else
        {
            Object.Destroy(this.bloodBustVampire);
        }
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