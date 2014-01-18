using System.Text;

using UnityEngine;

public class Building_GlobalState : State<BuildingController>
{
    //private static Actor_GlobalState instance;

    #region Public Methods and Operators

    public static Building_GlobalState Instance()
    {
        //return instance ?? (instance = new Actor_GlobalState());
        return new Building_GlobalState();
    }

    public override void Enter(BuildingController entityType)
    {
        base.Enter(entityType);
    }

    public override void Execute(BuildingController entityType)
    {
        base.Execute(entityType);
    }

    public override void Exit(BuildingController entityType)
    {
        base.Exit(entityType);
    }

    public override bool OnMessage(BuildingController entityType, Telegram telegram)
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

public class Building_StateBeforeBuilt : State<BuildingController>
{
    #region Public Methods and Operators

    public static Building_StateBeforeBuilt Instance()
    {
        return new Building_StateBeforeBuilt();
    }

    public override void Enter(BuildingController entityType)
    {
        StringBuilder spriteName = new StringBuilder(string.Empty);
        spriteName.Append(entityType.Building.BuildingType);
        spriteName.Append("_");
        spriteName.Append(entityType.Building.FactionType);
        entityType.SelfSprite.SetSprite(spriteName.ToString());

        entityType.GetFSM().ChangeState(Building_StateBuilding.Instance());
    }

    public override void Execute(BuildingController entityType)
    {
        base.Execute(entityType);
    }

    public override void Exit(BuildingController entityType)
    {
        base.Exit(entityType);
    }

    public override bool OnMessage(BuildingController entityType, Telegram telegram)
    {
        return base.OnMessage(entityType, telegram);
    }

    #endregion
}

public class Building_StateBuilding : State<BuildingController>
{
    #region Public Methods and Operators

    public static Building_StateBuilding Instance()
    {
        return new Building_StateBuilding();
    }

    public override void Enter(BuildingController entityType)
    {
        entityType.GetFSM().ChangeState(Building_StateDispatching.Instance());
    }

    public override void Execute(BuildingController entityType)
    {
        base.Execute(entityType);
    }

    public override void Exit(BuildingController entityType)
    {
        base.Exit(entityType);
    }

    public override bool OnMessage(BuildingController entityType, Telegram telegram)
    {
        return base.OnMessage(entityType, telegram);
    }

    #endregion
}

public class Building_StateDispatching : State<BuildingController>
{
    private int dispatchInterval;

    private float dispatchIntervalCounter;

    #region Public Methods and Operators

    public static Building_StateDispatching Instance()
    {
        return new Building_StateDispatching();
    }

    public override void Enter(BuildingController entityType)
    {
        this.dispatchInterval = entityType.DispatchInterval;
        this.dispatchIntervalCounter = 0f;
    }

    public override void Execute(BuildingController entityType)
    {
		if(entityType.Building.IsMainCity)
			return;
        if (this.dispatchInterval != entityType.DispatchInterval)
        {
            this.dispatchInterval = entityType.DispatchInterval;
            this.dispatchIntervalCounter = 0f;
        }
        else
        {
            this.dispatchIntervalCounter += Time.deltaTime;
            if (this.dispatchIntervalCounter >= this.dispatchInterval)
            {
                this.DispatchActor(entityType);
                this.dispatchIntervalCounter = 0f;
            }
        }
    }

    public override void Exit(BuildingController entityType)
    {
        base.Exit(entityType);
    }

    public override bool OnMessage(BuildingController entityType, Telegram telegram)
    {
        return base.OnMessage(entityType, telegram);
    }

    private void DispatchActor ( BuildingController entityType )
    {
        ActorsManager.GetInstance().CreateNewActor(entityType.Building.FactionType, entityType.Building.ActorType, entityType.MyTransform.position);
    }

    #endregion
}

public class Building_StateBeforeDestroy : State<BuildingController>
{
    #region Public Methods and Operators

    public static Building_StateBeforeDestroy Instance()
    {
        return new Building_StateBeforeDestroy();
    }

    public override void Enter(BuildingController entityType)
    {
     	Debug.Log("Win !!!!!!!!!!!");   
    }

    public override void Execute(BuildingController entityType)
    {
        base.Execute(entityType);
    }

    public override void Exit(BuildingController entityType)
    {
        base.Exit(entityType);
    }

    public override bool OnMessage(BuildingController entityType, Telegram telegram)
    {
        return base.OnMessage(entityType, telegram);
    }

    #endregion
}

public class Building_StateDestroy : State<BuildingController>
{
    #region Public Methods and Operators

    public static Building_StateDestroy Instance()
    {
        return new Building_StateDestroy();
    }

    public override void Enter(BuildingController entityType)
    {
        base.Enter(entityType);
    }

    public override void Execute(BuildingController entityType)
    {
        base.Execute(entityType);
    }

    public override void Exit(BuildingController entityType)
    {
        base.Exit(entityType);
    }

    public override bool OnMessage(BuildingController entityType, Telegram telegram)
    {
        return base.OnMessage(entityType, telegram);
    }

    #endregion
}