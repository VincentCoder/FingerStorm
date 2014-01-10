#region

using UnityEngine;

#endregion

public class ActorsManager
{
    #region Static Fields

    private static object _lock = new object();

    private static ActorsManager instance;

    #endregion

    #region Constructors and Destructors

    private ActorsManager()
    {
    }

    #endregion

    #region Public Methods and Operators

    public static ActorsManager GetInstance()
    {
        if (instance == null)
        {
            lock (_lock)
            {
                if (instance == null)
                {
                    instance = new ActorsManager();
                }
            }
        }
        return instance;
    }

    public void CreateNewActor(FactionType factionType, ActorType actorType)
    {
        GameObject actorObj = (GameObject)Object.Instantiate(Resources.Load("GameScene/Actor"));
        actorObj.transform.name = "Actor";
        actorObj.transform.localScale = new Vector3(1, 1, 1);
        ActorController actorCtrl = actorObj.GetComponent<ActorController>();
        Actor actor = new Actor();
        actor.FactionType = factionType;
        actor.ActorType = actorType;
        actorCtrl.MyActor = actor;
    }

    public void CreateNewActor(Actor actor)
    {
        GameObject actorObj = (GameObject)Object.Instantiate(Resources.Load("GameScene/Actor"));
        actorObj.transform.name = "Actor";
        actorObj.transform.localScale = new Vector3(1, 1, 1);
        ActorController actorCtrl = actorObj.GetComponent<ActorController>();
        actorCtrl.MyActor = actor;
        actorCtrl.TargetBuilding =
            BuildingsManager.GetInstance()
                .GetBaseBuildingOfFaction(actor.FactionType == FactionType.Blue ? FactionType.Red : FactionType.Blue);
    }

    #endregion
}