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
						List<GameObject> enemyActors = ActorsManager.GetInstance().GetAllEnemyActors(this.gameSceneController.MyFactionType);
						enemyActors.ForEach(enemyActor => 
						{
							ActorController actorCtrl = enemyActor.GetComponent<ActorController>();			
							actorCtrl.TakeDamage(300, -1, -1, true);
						});
						List<GameObject> enemyBuildings = BuildingsManager.GetInstance().GetAllEnemyBuildings(this.gameSceneController.MyFactionType);
						enemyBuildings.ForEach(enemyBuilding => 
						{
							BuildingController buildingCtrl = enemyBuilding.GetComponent<BuildingController>();
							buildingCtrl.TakeDamage(300);
						});
					};
				}
				break;
			case "RoadblocksSurgery":
				GameObject roadblocksSurgery = (GameObject)Instantiate(Resources.Load("GameScene/PlayerSkillRoadblocksSurgery"));
				roadblocksSurgery.transform.position = new Vector3(Screen.width/2, Screen.height/2, 0);
				break;
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
				GameObject bloodlust = (GameObject)Instantiate(Resources.Load("GameScene/PlayerSkillBloodlust"));
			bloodlust.transform.position = new Vector3(Screen.width/2, Screen.height/2, 0);
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
					List<GameObject> enemies = ActorsManager.GetInstance().GetEnemyActorsInDistance(this.gameSceneController.MyFactionType, (Vector3)gesture.Position, 50);
					enemies.ForEach(enemy => 
					{
						ActorController actorCtrl = enemy.GetComponent<ActorController>();
						actorCtrl.TakeDamage(1000, -1, -1, true);
					});
					Destroy(fireBall);
				};
			}
		}
    }
}
