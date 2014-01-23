using UnityEngine;
using System.Collections.Generic;

public class UIPlayerSkillController : MonoBehaviour 
{
	private Transform myTranform;
	private GameSceneController gameSceneController;
	
	private bool toReleaseFireBall;
	
	private void Start () 
	{
		this.myTranform = this.transform;
		this.gameSceneController = GameObject.Find("GameSceneController").GetComponent<GameSceneController>();
		this.toReleaseFireBall = false;
		
		UIEventListener.Get(this.myTranform.FindChild("FireBall").gameObject).onClick = HandleEvent;
		UIEventListener.Get(this.myTranform.FindChild("LightningBolt").gameObject).onClick = HandleEvent;
		UIEventListener.Get(this.myTranform.FindChild("RoadblocksSurgery").gameObject).onClick = HandleEvent;
		UIEventListener.Get(this.myTranform.FindChild("Heal").gameObject).onClick = HandleEvent;
		UIEventListener.Get(this.myTranform.FindChild("Bloodlust").gameObject).onClick = HandleEvent;
	}
	
	private void HandleEvent( GameObject eventObj )
	{
		switch(eventObj.name)
		{
			case "FireBall":
				if(this.gameSceneController.Mp >= 60)
				{
					this.gameSceneController.Mp -= 60;
					this.toReleaseFireBall = true;
				}
				break;
			case "LightningBolt":
				if(this.gameSceneController.Mp >= 90)
				{
					this.gameSceneController.Mp -= 90;
					GameObject lightningBolt = (GameObject)Instantiate(Resources.Load("GameScene/PlayerSkillLightningBolt"));
					lightningBolt.transform.position = new Vector3(Screen.width/2, Screen.height/2, 0);
					tk2dSpriteAnimator animator = lightningBolt.GetComponent<tk2dSpriteAnimator>();
					animator.AnimationCompleted = delegate 
					{
						Damage lightningBoltDamage = new Damage();
							lightningBoltDamage.DamageValue = 300;
							lightningBoltDamage.ShowCrit = true;
						List<GameObject> enemyActors = ActorsManager.GetInstance().GetAllEnemyActors(this.gameSceneController.MyFactionType);
						enemyActors.ForEach(enemyActor => 
						{
							ActorController actorCtrl = enemyActor.GetComponent<ActorController>();		
							actorCtrl.TakeDamage(lightningBoltDamage);
						});
						List<GameObject> enemyBuildings = BuildingsManager.GetInstance().GetAllEnemyBuildings(this.gameSceneController.MyFactionType);
						enemyBuildings.ForEach(enemyBuilding => 
						{
							BuildingController buildingCtrl = enemyBuilding.GetComponent<BuildingController>();
							buildingCtrl.TakeDamage(lightningBoltDamage);
						});
					};
				}
				break;
			case "RoadblocksSurgery":
		        {
		            if (this.gameSceneController.Mp >= 60)
		            {
		                this.gameSceneController.Mp -= 60;
                        GameObject roadblocksSurgery =
		                (GameObject)Instantiate(Resources.Load("GameScene/PlayerSkillRoadblocksSurgery"));
                        roadblocksSurgery.transform.position = new Vector3(Screen.width / 2, Screen.height / 2, 0);
		                tk2dSpriteAnimator animator = roadblocksSurgery.GetComponent<tk2dSpriteAnimator>();
		                animator.AnimationCompleted = delegate
		                    {
                                Vector3[] positionArr = new Vector3[4]{new Vector3(295, 485, 0), new Vector3(665, 485, 0), new Vector3(295, 290, 0), new Vector3(665, 290, 0)};
		                        for (int i = 0; i < 4; i ++)
		                        {
									GameObject roadBlock = GameObject.Find("RoadBlock"+i);
									if(roadBlock == null)
									{
										roadBlock = (GameObject)Instantiate(Resources.Load("GameScene/RoadBlock"));
										roadBlock.name = "RoadBlock" + i;
		                            	roadBlock.transform.position = positionArr[i];
									}
									else
									{
										RoadBlockController roadBlockCtrl = roadBlock.GetComponent<RoadBlockController>();
										roadBlockCtrl.ResetHp();
									}
		                        }
		                    };
		            }
		            break;
		        }
		    case "Heal":
				if(this.gameSceneController.Mp >= 100)
				{
					this.gameSceneController.Mp -= 100;
					GameObject heal = (GameObject)Instantiate(Resources.Load("GameScene/PlayerSkillHeal"));
					heal.transform.position = new Vector3(Screen.width/2, Screen.height/2, 0);
					tk2dSpriteAnimator animator = heal.GetComponent<tk2dSpriteAnimator>();
					animator.AnimationCompleted = delegate 
					{
						List<GameObject> myActors = ActorsManager.GetInstance().GetActorsOfFaction(this.gameSceneController.MyFactionType);
						myActors.ForEach(actor => 
						{
							ActorController actorCtrl = actor.GetComponent<ActorController>();
							actorCtrl.MyActor.CurrentHp = Mathf.Min(actorCtrl.MyActor.CurrentHp + 300, actorCtrl.MyActor.TotalHp);
						});
						List<GameObject> myBuildings = BuildingsManager.GetInstance().GetBuildingsOfFaction(this.gameSceneController.MyFactionType);
						myBuildings.ForEach(building => 
						{
							BuildingController buildingCtrl = building.GetComponent<BuildingController>();
							buildingCtrl.Building.CurrentHp = Mathf.Min(buildingCtrl.Building.CurrentHp + 300, buildingCtrl.Building.TotalHp);
						});
						Destroy(heal);
					};
				}
				
				break;
			case "Bloodlust":
		        {
		            if (this.gameSceneController.Mp >= 120)
		            {
		                this.gameSceneController.Mp -= 120;
                        GameObject bloodlust = (GameObject)Instantiate(Resources.Load("GameScene/PlayerSkillBloodlust"));
                        bloodlust.transform.position = new Vector3(Screen.width/2, Screen.height/2, 0);
		                tk2dSpriteAnimator animator = bloodlust.GetComponent<tk2dSpriteAnimator>();
                        animator.Play("Release");
		                animator.AnimationCompleted = delegate
		                    {
		                        List<GameObject> myActors =
		                            ActorsManager.GetInstance().GetActorsOfFaction(this.gameSceneController.MyFactionType);
                                myActors.ForEach(
                                    actor =>
                                        {
                                            ActorController actorCtrl = actor.GetComponent<ActorController>();
                                            actorCtrl.BloodSuckingRatio = 30;
                                            actorCtrl.AttackPlusRatio = 1.5f;
                                            GameObject bloodBustHalo = (GameObject)Instantiate(Resources.Load("GameScene/PlayerSkillBloodlust"));
                                            bloodBustHalo.transform.parent = actor.transform;
                                            bloodBustHalo.GetComponent<tk2dSpriteAnimator>().Play("Halo");
                                        });
		                    };
		            }
		        }
		        
				break;
		}
	}
	
	private void OnTap( TapGesture gesture )
    {
        if(this.toReleaseFireBall)
		{
			Rect validRect = new Rect(0, 160, Screen.width, Screen.height - 160 - 50);
			if(validRect.Contains(gesture.Position))
			{
				this.toReleaseFireBall = false;
				GameObject fireBall = (GameObject)Instantiate(Resources.Load("GameScene/PlayerSkillFireBall"));
				fireBall.transform.position = (Vector3)gesture.Position;
				tk2dSpriteAnimator animator = fireBall.GetComponent<tk2dSpriteAnimator>();
				animator.AnimationCompleted = delegate 
				{
					Damage fireBallDamage = new Damage();
					fireBallDamage.DamageValue = 1000;
					fireBallDamage.ShowCrit = true;
					List<GameObject> enemies = ActorsManager.GetInstance().GetEnemyActorsInDistance(this.gameSceneController.MyFactionType, (Vector3)gesture.Position, 50);
					enemies.ForEach(enemy => 
					{
						ActorController actorCtrl = enemy.GetComponent<ActorController>();
						actorCtrl.TakeDamage(fireBallDamage);
					});
					Destroy(fireBall);
				};
			}
		}
    }
}
