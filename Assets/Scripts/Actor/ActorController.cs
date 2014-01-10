#region

using System.Text;

using UnityEngine;

#endregion

public class ActorController : MonoBehaviour
{
    #region Fields

    private Actor _myActor;

    private tk2dSpriteAnimator _selfAnimator;

    private float moveSpeed;

    private Transform myTransform;

    #endregion

    #region Public Properties

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

    public GameObject TargetBuilding { get; set; }

    #endregion

    #region Properties

    private tk2dSpriteAnimator SelfAnimator
    {
        get
        {
            if (this._selfAnimator == null)
            {
                this._selfAnimator = this.gameObject.GetComponent<tk2dSpriteAnimator>();
            }
            return this._selfAnimator;
        }
        set
        {
            this._selfAnimator = value;
        }
    }

    #endregion

    #region Methods

    private void Awake()
    {
        this.myTransform = this.gameObject.transform;
    }

    private void InitActor()
    {
        if (this._myActor != null)
        {
            StringBuilder animName = new StringBuilder("Terran_");
            switch (this._myActor.ActorType)
            {
                case ActorType.CRUSADE:
                    animName.Append("Crusader");
                    break;
                case ActorType.GRYPHONRIDER:
                    animName.Append("GryphonRider");
                    break;
                case ActorType.HEAVYGUNNER:
                    animName.Append("HeavyGunner");
                    break;
                case ActorType.INFANTRY:
                    animName.Append("Infantry");
                    break;
                case ActorType.MARKSMAN:
                    animName.Append("Marksman");
                    break;
                case ActorType.MOTARTEAM:
                    animName.Append("MotarTeam");
                    break;
                case ActorType.SENIORGRYPHONRIDER:
                    animName.Append("SeniorGryphonRider");
                    break;
                case ActorType.SNIPER:
                    animName.Append("Sniper");
                    break;
                case ActorType.SUPPORTER:
                    animName.Append("Supporter");
                    break;
                case ActorType.TEMPLARWARRIOR:
                    animName.Append("TemplarWarrior");
                    break;
                case ActorType.WARLOCK:
                    animName.Append("Warlock");
                    break;
                default:
                    animName.Append("Infantry");
                    break;
            }
            animName.Append("_Walk_");
            switch (this._myActor.FactionType)
            {
                case FactionType.Blue:
                    animName.Append("Blue");
                    break;
                case FactionType.Red:
                    animName.Append("Red");
                    break;
                default:
                    animName.Append("Blue");
                    break;
            }
            this.SelfAnimator.Play(animName.ToString());
        }
    }

    private void Update()
    {
        Vector3 moveDistance = this.moveSpeed * Time.deltaTime
                               * (this.TargetBuilding.transform.position - this.myTransform.position).normalized;
        this.myTransform.Translate(moveDistance, Space.World);
    }

    #endregion
}