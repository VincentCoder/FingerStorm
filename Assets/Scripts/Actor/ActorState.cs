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

    private float stunDuration;

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
            this.stunDuration -= Time.deltaTime;
            if (this.stunDuration <= 0)
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
                this.stunDuration = Mathf.Max((float)telegram.Parameters["StunDuration"], this.stunDuration);
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
                        this.SendDamage(entityType, actorSpell.DirectDamage);
                    }
                    break;
                }
            case ActorSpellName.ArcaneExplosion:
                {
                    List<GameObject> enemies = entityType.SeekAndGetEnemiesInDistance(actorSpell.AttackRange);
                    if (enemies != null && enemies.Count != 0)
                    {
                        this.SendDamage(entityType, actorSpell.DirectDamage);
                    }
                    break;
                }
        }
    }

    private void SendDamage ( ActorController entityType, int damage )
    {
        Hashtable parameters = new Hashtable();
        parameters.Add(
            "Damage",
            damage);
        MessageDispatcher.Instance()
            .DispatchMessage(
                0f,
                entityType,
                entityType.TargetEnemy.GetComponent<ActorController>(),
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
		Debug.Log("Enter Walk");
        StringBuilder animName = new StringBuilder("Terran_");
        animName.Append(entityType.MyActor.ActorType);
        animName.Append("_Walk_");
        animName.Append(entityType.MyActor.FactionType);
        entityType.SelfAnimator.Play(animName.ToString());
        //base.Enter(entityType);
    }

    public override void Execute(ActorController entityType)
    {
        if(entityType.IsStun)
            return;
        Vector3 moveDistance = entityType.moveSpeed * Time.deltaTime
                               * (entityType.TargetBuilding.transform.position - entityType.myTransform.position)
                                     .normalized;
        entityType.myTransform.Translate(moveDistance, Space.World);

        this.seekEnemyCounter += Time.deltaTime;
        if (this.seekEnemyCounter >= this.seekEnemyInterval)
        {
            if (entityType.SeekEnemies())
            {
                entityType.GetFSM().ChangeState(Actor_StateBeforeFight.Instance());
            }
            this.seekEnemyCounter = 0.0f;
        }
        //base.Execute(entityType);
    }

    public override void Exit(ActorController entityType)
    {
		Debug.Log("Exit Walk");
        base.Exit(entityType);
    }

    public override bool OnMessage(ActorController entityType, Telegram telegram)
    {
        return base.OnMessage(entityType, telegram);
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
		Debug.Log("Enter BeforeFight");

        List<GameObject> enemies = entityType.SeekAndGetEnemies();
        if (enemies == null || enemies.Count == 0)
        {
            entityType.GetFSM().ChangeState(Actor_StateWalk.Instance());
            return;
        }
        entityType.TargetEnemy = enemies[0];
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
		Debug.Log("Exit BeforeFight");
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

    #endregion

    #region Public Methods and Operators

    public static Actor_StateFight Instance()
    {
        //return instance ?? (instance = new Actor_StateFight());
		return new Actor_StateFight();
    }

    public override void Enter(ActorController entityType)
    {
		Debug.Log("Enter Fight");
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
        entityType.SelfAnimator.AnimationEventTriggered = (animator, clip, arg3) =>
            {
                if (arg3 == 5)
                {
                    this.SendDamage(entityType);
                }
            };
    }

    public override void Execute(ActorController entityType)
    {
        if(entityType.IsStun)
            return;
        
        if (entityType.TargetEnemy == null)
        {
            entityType.GetFSM().ChangeState(Actor_StateBeforeFight.Instance());
        }
    }

    public override void Exit(ActorController entityType)
    {
		Debug.Log("Exit Fight");
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
        if (passiveSpellDictionary != null)
        {
            foreach (KeyValuePair<ActorSpellName, ActorSpell> kv in passiveSpellDictionary)
            {
                float probability = kv.Value.DamageBonusPercentProbability;
                int randomIndex;
                if (probability > 0)
                {
                    randomIndex = Random.Range(1, 101);
                }
            }
        }
        Hashtable parameters = new Hashtable();
        parameters.Add(
            "Damage",
            Time.deltaTime * entityType.MyActor.ActorAttack.Dps * entityType.SelfAnimator.ClipTimeSeconds);
        MessageDispatcher.Instance()
            .DispatchMessage(
                0f,
                entityType,
                entityType.TargetEnemy.GetComponent<ActorController>(),
                FSMessageType.FSMessageAttack,
                parameters);
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