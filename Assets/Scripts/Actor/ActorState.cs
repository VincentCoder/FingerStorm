#region

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
        return base.OnMessage(entityType, telegram);
    }

    #endregion
}

public class Actor_StateWalk : State<ActorController>
{
    #region Static Fields

    private static Actor_StateWalk instance;

    #endregion

    #region Fields

    private float seekEnemyInterval = 0.5f;

    private float seekEnemyCounter = 0.0f;

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
                entityType.GetFSM().ChangeState(Actor_StateFight.Instance());
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

    private bool SeekEnemies(ActorController entityType)
    {
        return ActorsManager.GetInstance().HasEnemyActorsInDistance(entityType, entityType.MyActor.ActorAttack.AttackRange);
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
        return base.OnMessage(entityType, telegram);
    }

    #endregion
}

public class Actor_StateFight : State<ActorController>
{
    #region Static Fields

    private static Actor_StateFight instance;

    private List<GameObject> enemiesList; 

    #endregion

    #region Public Methods and Operators

    public static Actor_StateFight Instance()
    {
        return instance ?? (instance = new Actor_StateFight());
    }

    public override void Enter(ActorController entityType)
    {
        this.enemiesList = this.SeekEnemies(entityType);
        if (this.enemiesList == null || this.enemiesList.Count == 0)
        {
            entityType.GetFSM().ChangeState(Actor_StateWalk.Instance());
            return;  
        }

        StringBuilder animName = new StringBuilder("Terran_");
        animName.Append(entityType.MyActor.ActorType);
        animName.Append("_Attack_");
        animName.Append(entityType.MyActor.FactionType);
        entityType.SelfAnimator.Play(animName.ToString());

        //base.Enter(entityType);
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
        //base.Enter(entityType);
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
        return base.OnMessage(entityType, telegram);
    }

    #endregion
}