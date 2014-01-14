#region

using System.Collections.Generic;
using System.Linq;

using UnityEngine;

#endregion

public class ActorsManager
{
    #region Static Fields

    private static readonly object _lock = new object();

    private static ActorsManager instance;

    #endregion

    #region Fields

    private readonly Dictionary<int, GameObject> actorsDictionary = new Dictionary<int, GameObject>();

    private int actorIdSeq;

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

    public void CreateNewActor(FactionType factionType, ActorType actorType, Vector3 pos)
    {
        var actorObj = (GameObject)Object.Instantiate(Resources.Load("GameScene/Actor"));
        int actorId = this.GenerateNewActorId();
        actorObj.transform.name = "Actor_" + actorId.ToString("D3");
        actorObj.transform.localScale = new Vector3(1, 1, 1);
        actorObj.transform.position = pos;
        var actorCtrl = actorObj.GetComponent<ActorController>();
        var actor = new Actor(actorId, actorType, factionType);
        actorCtrl.MyActor = actor;
        actorCtrl.TargetBuilding =
            BuildingsManager.GetInstance()
                .GetBaseBuildingOfFaction(actor.FactionType == FactionType.Blue ? FactionType.Red : FactionType.Blue);
        this.actorsDictionary.Add(actorId, actorObj);
    }

    public void CreateNewActor(Actor actor, Vector3 pos)
    {
        var actorObj = (GameObject)Object.Instantiate(Resources.Load("GameScene/Actor"));
        int actorId = this.GenerateNewActorId();
        actorObj.transform.name = "Actor_" + actorId.ToString("D3");
        actorObj.transform.localScale = new Vector3(1, 1, 1);
        actorObj.transform.position = pos;
        var actorCtrl = actorObj.GetComponent<ActorController>();
        actor.ActorId = actorId;
        actorCtrl.MyActor = actor;
        actorCtrl.TargetBuilding =
            BuildingsManager.GetInstance()
                .GetBaseBuildingOfFaction(actor.FactionType == FactionType.Blue ? FactionType.Red : FactionType.Blue);
        this.actorsDictionary.Add(actorId, actorObj);
    }

    public GameObject GetActorById(int actorId)
    {
        return this.actorsDictionary.ContainsKey(actorId) ? this.actorsDictionary[actorId] : null;
    }

    public List<GameObject> GetActorsOfFaction(FactionType factionType)
    {
        return (from kv in this.actorsDictionary
                let actorCtrl = kv.Value.GetComponent<ActorController>()
                where actorCtrl.MyActor.FactionType == factionType
                select kv.Value).ToList();
    }

    public List<GameObject> GetAllActors()
    {
        return new List<GameObject>(this.actorsDictionary.Values.ToArray());
    }

    public List<GameObject> GetEnemyActorsInDistance(ActorController actorController, int distance)
    {
        return (from kv in this.actorsDictionary
                let actorCtrl = kv.Value.GetComponent<ActorController>()
                where actorCtrl.MyActor.FactionType != actorController.MyActor.FactionType
                where
                    (actorController.gameObject.transform.position - kv.Value.transform.position).sqrMagnitude
                    <= Mathf.Pow(distance, 2)
                select kv.Value).ToList();
    }

    public List<GameObject> GetEnemyActorsInDistanceAndSortByDistance(ActorController actorController, int distance)
    {
        List<GameObject> result = (from kv in this.actorsDictionary
                                   let actorCtrl = kv.Value.GetComponent<ActorController>()
                                   where actorCtrl.MyActor.FactionType != actorController.MyActor.FactionType
                                   where
                                       (actorController.gameObject.transform.position - kv.Value.transform.position)
                                           .sqrMagnitude <= Mathf.Pow(distance, 2)
                                   select kv.Value).ToList();
        result.Sort(
            (actor1, actor2) =>
            (actor1.transform.position - actorController.gameObject.transform.position).sqrMagnitude.CompareTo(
                (actor2.transform.position - actorController.gameObject.transform.position).sqrMagnitude));
        return result;
    }

    public bool HasEnemyActorsInDistance(ActorController actorController, int distance)
    {
        return (from kv in this.actorsDictionary
                let actorCtrl = kv.Value.GetComponent<ActorController>()
                where actorCtrl.MyActor.FactionType != actorController.MyActor.FactionType
                select kv).Any(
                    kv =>
                    (actorController.gameObject.transform.position - kv.Value.transform.position).sqrMagnitude
                    <= Mathf.Pow(distance, 2));
    }

    #endregion

    #region Methods

    private int GenerateNewActorId()
    {
        int result;
        lock (_lock)
        {
            result = (++ this.actorIdSeq);
        }
        return result;
    }

    #endregion
}