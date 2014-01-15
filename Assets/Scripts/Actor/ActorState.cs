#region

using System.Collections;
using System.Collections.Generic;
using System.Text;

using UnityEngine;

#endregion

public class Actor_GlobalState : State<ActorController>
{
    #region Static Fields

    private static Actor_GlobalState instance;

    #endregion

    #region Public Methods and Operators

    public static Actor_GlobalState Instance()
    {
        return instance ?? (instance = new Actor_GlobalState());
    }

    public override void Enter(ActorController entityType)
    {
        base.Enter(entityType);
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
        if (telegram.Msg == FSMessageType.FSMessageAttack)
        {
            if (telegram.Parameters.ContainsKey("Damage"))
            {
                entityType.TakeDamage((float)telegram.Parameters["Damage"]);
                return true;
            }
        }
        return false;
    }

    #endregion
}

public class Actor_StateWalk : State<ActorController>
{
    #region Static Fields

    private static Actor_StateWalk instance;

    #endregion

    #region Fields

    private float seekEnemyCounter;

    private float seekEnemyInterval = 0.5f;

    #endregion

    #region Public Methods and Operators

    public static Actor_StateWalk Instance()
    {
        return instance ?? (instance = new Actor_StateWalk());
    }

    public override void Enter(ActorController entityType)
    {
        StringBuilder animName = new StringBuilder("Terran_");
        animName.Append(entityType.MyActor.ActorType);
        animName.Append("_Walk_");
        animName.Append(entityType.MyActor.FactionType);
        entityType.SelfAnimator.Play(animName.ToString());
        //base.Enter(entityType);
    }

    public override void Execute(ActorController entityType)
    {
        Vector3 moveDistance = entityType.moveSpeed * Time.deltaTime
                               * (entityType.TargetBuilding.transform.position - entityType.myTransform.position)
                                     .normalized;
        entityType.myTransform.Translate(moveDistance, Space.World);

        this.seekEnemyCounter += Time.deltaTime;
        if (this.seekEnemyCounter >= this.seekEnemyInterval)
        {
            if (this.SeekEnemies(entityType))
            {
                entityType.GetFSM().ChangeState(Actor_StateBeforeFight.Instance());
            }
            this.seekEnemyCounter = 0.0f;
        }
        //base.Execute(entityType);
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

    #region Methods

    private bool SeekEnemies(ActorController entityType)
    {
        return ActorsManager.GetInstance()
            .HasEnemyActorsInDistance(entityType, entityType.MyActor.ActorAttack.ViewDistance);
    }

    #endregion
}

public class Actor_StateBeforeFight : State<ActorController>
{
    #region Static Fields

    private static Actor_StateBeforeFight instance;

    #endregion

    #region Public Methods and Operators

    public static Actor_StateBeforeFight Instance()
    {
        return instance ?? (instance = new Actor_StateBeforeFight());
    }

    public override void Enter(ActorController entityType)
    {
        List<GameObject> enemies = this.SeekEnemies(entityType);
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
            <= Mathf.Max(Mathf.Pow(entityType.MyActor.ActorAttack.AttackRange, 2), Mathf.Pow(20,2)))
        {
            entityType.GetFSM().ChangeState(Actor_StateFight.Instance());
        }

        //base.Execute(entityType);
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

    #region Methods

    private List<GameObject> SeekEnemies(ActorController entityType)
    {
        return ActorsManager.GetInstance()
            .GetEnemyActorsInDistanceAndSortByDistance(entityType, entityType.MyActor.ActorAttack.ViewDistance);
    }

    #endregion
}

public class Actor_StateFight : State<ActorController>
{
    #region Static Fields

    private static Actor_StateFight instance;

    #endregion

    #region Public Methods and Operators

    public static Actor_StateFight Instance()
    {
        return instance ?? (instance = new Actor_StateFight());
    }

    public override void Enter(ActorController entityType)
    {
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

    }

    public override void Execute(ActorController entityType)
    {
        if (entityType.TargetEnemy == null)
        {
            entityType.GetFSM().ChangeState(Actor_StateBeforeFight.Instance());
            return;
        }
        Hashtable parameters = new Hashtable();
        parameters.Add("Damage", Time.deltaTime * entityType.MyActor.ActorAttack.Dps);
        MessageDispatcher.Instance()
            .DispatchMessage(
                0f,
                entityType,
                entityType.TargetEnemy.GetComponent<ActorController>(),
                FSMessageType.FSMessageAttack,
                parameters);
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

    #region Methods

    private List<GameObject> SeekEnemies(ActorController entityType)
    {
        return ActorsManager.GetInstance()
            .GetEnemyActorsInDistanceAndSortByDistance(entityType, entityType.MyActor.ActorAttack.AttackRange);
    }

    #endregion
}

public class Actor_StateBeforeDie : State<ActorController>
{
    #region Static Fields

    private static Actor_StateBeforeDie instance;

    #endregion

    #region Public Methods and Operators

    public static Actor_StateBeforeDie Instance()
    {
        return instance ?? (instance = new Actor_StateBeforeDie());
    }

    public override void Enter(ActorController entityType)
    {
        StringBuilder animName = new StringBuilder("Terran_");
        animName.Append(entityType.MyActor.ActorType);
        animName.Append("_Die_");
        animName.Append(entityType.MyActor.FactionType);
        entityType.SelfAnimator.Play(animName.ToString());
        entityType.SelfAnimator.AnimationCompleted = delegate
            {
                entityType.GetFSM().ChangeState(Actor_StateDie.Instance());
            };
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

    private static Actor_StateDie instance;

    #endregion

    #region Public Methods and Operators

    public static Actor_StateDie Instance()
    {
        return instance ?? (instance = new Actor_StateDie());
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