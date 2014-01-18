#region

using UnityEngine;

#endregion

/// <summary>
///     The game scene controller.
/// </summary>
public class GameSceneController : MonoBehaviour
{
    #region Fields

    /// <summary>
    ///     The battle field map.
    /// </summary>
    private GameObject battleFieldMap;

    #endregion

    #region Methods

    /// <summary>
    ///     The awake.
    /// </summary>
    private void Awake()
    {
        this.CreateBattleFieldMap();
        BuildingsManager.GetInstance()
            .CreateNewBuilding(
                BuildingType.Terran_TheMainCity, FactionType.Blue,
                new Vector3(100, 400, 0));
		
		BuildingsManager.GetInstance()
            .CreateNewBuilding(
                BuildingType.Terran_Barrack, FactionType.Blue,
                new Vector3(100, 530, 0));
		
		BuildingsManager.GetInstance()
            .CreateNewBuilding(
                BuildingType.Terran_Fortress, FactionType.Blue,
                new Vector3(100, 270, 0));
		
		BuildingsManager.GetInstance()
            .CreateNewBuilding(
                BuildingType.Terran_MarksmanCamp, FactionType.Blue,
                new Vector3(250, 400, 0));
		
        BuildingsManager.GetInstance()
            .CreateNewBuilding(
                BuildingType.Terran_TheMainCity, FactionType.Red,
                new Vector3(860, 400, 0));
		
		BuildingsManager.GetInstance()
            .CreateNewBuilding(
                BuildingType.Terran_ArtilleryHall, FactionType.Red,
                new Vector3(860, 270, 0));
		
		BuildingsManager.GetInstance()
            .CreateNewBuilding(
                BuildingType.Terran_Church, FactionType.Red,
                new Vector3(710, 400, 0));
		
		BuildingsManager.GetInstance()
            .CreateNewBuilding(
                BuildingType.Terran_SniperHouse, FactionType.Red,
                new Vector3(710, 530, 0));
		
		GameController gameController = GameObject.Find("GameController").GetComponent<GameController>();
		gameController.ViewController.ShowBuildingsSelectorPanel();
		gameController.ViewController.ShowBuildingDetailPanel();
    }

    /// <summary>
    ///     The create battle field map.
    /// </summary>
    private void CreateBattleFieldMap()
    {
        this.battleFieldMap = (GameObject)Instantiate(Resources.Load("Map/BattleFieldMap"));
        this.battleFieldMap.name = "BattleFieldMap";
        this.battleFieldMap.transform.parent = GameObject.Find("tkAnchor").transform;
        this.battleFieldMap.transform.localScale = new Vector3(1, 1, 1);
        this.battleFieldMap.transform.localPosition = new Vector3(0, 0, 0);
		
		GameObject obstacle = (GameObject)Instantiate(Resources.Load("Map/Obstacle"));
        obstacle.name = "Obstacle";
        obstacle.transform.parent = GameObject.Find("tkAnchor").transform;
        obstacle.transform.localScale = new Vector3(1, 1, 1);
        obstacle.transform.localPosition = new Vector3(0, 80, -1);
    }

    #endregion
}