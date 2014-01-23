#region

using System.Collections.Generic;

using UnityEngine;

#endregion

public class UIPlayerSkillController : MonoBehaviour
{
    #region Fields

    private GameSceneController gameSceneController;

    private Transform myTranform;

    private bool toReleaseFireBall;

    #endregion

    #region Methods

    private void HandleEvent(GameObject eventObj)
    {
        switch (eventObj.name)
        {
            case "FireBall":
                if (this.gameSceneController.Mp >= 60)
                {
                    this.gameSceneController.Mp -= 60;
                    this.toReleaseFireBall = true;
                }
                break;
            case "LightningBolt":
                {
                    if (this.gameSceneController.Mp >= 90)
                    {
                        this.gameSceneController.Mp -= 90;

                        Damage lightningBoltDamage = new Damage();
                        lightningBoltDamage.DamageValue = 300;
                        lightningBoltDamage.ShowCrit = true;
                        List<GameObject> enemyActors =
                            ActorsManager.GetInstance().GetAllEnemyActors(this.gameSceneController.MyFactionType);
                        enemyActors.ForEach(
                            enemyActor =>
                                {
                                    GameObject lightningBolt =
                                        (GameObject)Instantiate(Resources.Load("GameScene/PlayerSkillLightningBolt"));
                                    lightningBolt.transform.position = enemyActor.transform.position;
                                    tk2dSpriteAnimator animator = lightningBolt.GetComponent<tk2dSpriteAnimator>();
                                    animator.AnimationCompleted = delegate
                                        {
											if(enemyActor != null)
											{
												ActorController actorCtrl = enemyActor.GetComponent<ActorController>();
                                            	actorCtrl.TakeDamage(lightningBoltDamage);
                                            	Destroy(lightningBolt);
											}
                                        };
                                });
                        //List<GameObject> enemyBuildings = BuildingsManager.GetInstance().GetAllEnemyBuildings(this.gameSceneController.MyFactionType);
                        //enemyBuildings.ForEach(enemyBuilding => 
                        //{
                        //	BuildingController buildingCtrl = enemyBuilding.GetComponent<BuildingController>();
                        //	buildingCtrl.TakeDamage(lightningBoltDamage);
                        //});
                    }
                    break;
                }
            case "RoadblocksSurgery":
                {
                    if (this.gameSceneController.Mp >= 60000)
                    {
                        this.gameSceneController.Mp -= 60;
                        GameObject roadblocksSurgery =
                            (GameObject)Instantiate(Resources.Load("GameScene/PlayerSkillRoadblocksSurgery"));
                        roadblocksSurgery.transform.position = new Vector3(Screen.width / 2, Screen.height / 2, 0);
                        tk2dSpriteAnimator animator = roadblocksSurgery.GetComponent<tk2dSpriteAnimator>();
                        animator.AnimationCompleted = delegate
                            {
                                Vector3[] positionArr = new Vector3[4]
                                                            {
                                                                new Vector3(295, 485, 0), new Vector3(665, 485, 0),
                                                                new Vector3(295, 290, 0), new Vector3(665, 290, 0)
                                                            };
                                for (int i = 0; i < 4; i ++)
                                {
                                    GameObject roadBlock = GameObject.Find("RoadBlock" + i);
                                    if (roadBlock == null)
                                    {
                                        roadBlock = (GameObject)Instantiate(Resources.Load("GameScene/RoadBlock"));
                                        roadBlock.name = "RoadBlock" + i;
                                        roadBlock.transform.position = positionArr[i];
                                    }
                                    else
                                    {
                                        RoadBlockController roadBlockCtrl =
                                            roadBlock.GetComponent<RoadBlockController>();
                                        roadBlockCtrl.ResetHp();
                                    }
                                }
                            };
                    }
                    break;
                }
            case "Heal":
                {
                    if (this.gameSceneController.Mp >= 100)
                    {
                        this.gameSceneController.Mp -= 100;

                        Damage healDamage = new Damage();
                        healDamage.DamageValue = -300;
                        healDamage.ShowCrit = true;
                        List<GameObject> myActors =
                            ActorsManager.GetInstance().GetActorsOfFaction(this.gameSceneController.MyFactionType);
                        myActors.ForEach(
                            actor =>
                                {
                                    GameObject heal =
                                        (GameObject)Instantiate(Resources.Load("GameScene/PlayerSkillHeal"));
                                    heal.transform.position = actor.transform.position;
                                    tk2dSpriteAnimator animator = heal.GetComponent<tk2dSpriteAnimator>();
                                    animator.AnimationCompleted = delegate
                                        {
                                            ActorController actorCtrl = actor.GetComponent<ActorController>();
                                            actorCtrl.TakeDamage(healDamage);
                                            Destroy(heal);
                                        };
                                });
                    }
                    break;
                }
            case "Bloodlust":
                {
                    if (this.gameSceneController.Mp >= 120)
                    {
                        this.gameSceneController.Mp -= 120;

                        List<GameObject> myActors =
                            ActorsManager.GetInstance().GetActorsOfFaction(this.gameSceneController.MyFactionType);
                        myActors.ForEach(
                            actor =>
                                {
                                    GameObject bloodlust =
                                        (GameObject)Instantiate(Resources.Load("GameScene/PlayerSkillBloodlust"));
                                    bloodlust.transform.position = actor.transform.position;
                                    tk2dSpriteAnimator animator = bloodlust.GetComponent<tk2dSpriteAnimator>();
                                    animator.Play("Release");
                                    animator.AnimationCompleted = delegate
                                        {
                                            Destroy(bloodlust);
                                            ActorController actorCtrl = actor.GetComponent<ActorController>();
                                            actorCtrl.BloodSuckingRatio = 30;
                                            actorCtrl.AttackPlusRatio = 1.5f;
                                            GameObject bloodBustHalo =
                                                (GameObject)
                                                Instantiate(Resources.Load("GameScene/PlayerSkillBloodlust"));
                                            bloodBustHalo.transform.parent = actor.transform;
                                            bloodBustHalo.GetComponent<tk2dSpriteAnimator>().Play("Halo");
                                        };
                                });
                    }
                    break;
                }
        }
    }

    private void OnTap(TapGesture gesture)
    {
        if (this.toReleaseFireBall)
        {
            Rect validRect = new Rect(0, 160, Screen.width, Screen.height - 160 - 50);
            if (validRect.Contains(gesture.Position))
            {
                this.toReleaseFireBall = false;
                GameObject fireBall = (GameObject)Instantiate(Resources.Load("GameScene/PlayerSkillFireBall"));
                fireBall.transform.position = gesture.Position;
                tk2dSpriteAnimator animator = fireBall.GetComponent<tk2dSpriteAnimator>();
                animator.AnimationCompleted = delegate
                    {
                        Damage fireBallDamage = new Damage();
                        fireBallDamage.DamageValue = 1000;
                        fireBallDamage.ShowCrit = true;
                        List<GameObject> enemies =
                            ActorsManager.GetInstance()
                                .GetEnemyActorsInDistance(this.gameSceneController.MyFactionType, gesture.Position, 50);
                        enemies.ForEach(
                            enemy =>
                                {
                                    ActorController actorCtrl = enemy.GetComponent<ActorController>();
                                    actorCtrl.TakeDamage(fireBallDamage);
                                });
                        Destroy(fireBall);
                    };
            }
        }
    }

    private void Start()
    {
        this.myTranform = this.transform;
        this.gameSceneController = GameObject.Find("GameSceneController").GetComponent<GameSceneController>();
        this.toReleaseFireBall = false;

        UIEventListener.Get(this.myTranform.FindChild("FireBall").gameObject).onClick = this.HandleEvent;
        UIEventListener.Get(this.myTranform.FindChild("LightningBolt").gameObject).onClick = this.HandleEvent;
        UIEventListener.Get(this.myTranform.FindChild("RoadblocksSurgery").gameObject).onClick = this.HandleEvent;
        UIEventListener.Get(this.myTranform.FindChild("Heal").gameObject).onClick = this.HandleEvent;
        UIEventListener.Get(this.myTranform.FindChild("Bloodlust").gameObject).onClick = this.HandleEvent;
    }

    #endregion
}