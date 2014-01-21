#region

using System.Collections.Generic;

using UnityEngine;

#endregion

public class BuildingsManager
{
    #region Static Fields

    private static object _lock = new object();

    private static BuildingsManager instance;

    #endregion

    #region Fields

    private Dictionary<int, GameObject> allBuildingsDictionary = new Dictionary<int, GameObject>();

    private Dictionary<FactionType, GameObject> baseBuildingDictionary = new Dictionary<FactionType, GameObject>();
	
	private int buildingIdSeq;

    #endregion

    #region Constructors and Destructors

    private BuildingsManager()
    {
    }

    #endregion

    #region Public Methods and Operators

    public static BuildingsManager GetInstance()
    {
        if (instance == null)
        {
            lock (_lock)
            {
                if (instance == null)
                {
                    instance = new BuildingsManager();
                }
            }
        }
        return instance;
    }
	
	public GameObject CreateNewBuilding(BuildingType buildingType, FactionType factionType, Vector3 pos)
    {
        GameObject buildingObj = (GameObject)Object.Instantiate(Resources.Load("GameScene/Building"));
        buildingObj.name = "Building";
        buildingObj.transform.localScale = new Vector3(1, 1, 1);
        buildingObj.transform.position = pos;
        BuildingController buildingCtrl = buildingObj.GetComponent<BuildingController>();
        buildingCtrl.Building = new Building(this.GenerateNewBuildingId(), buildingType, factionType);
		string buildingTypeStr = buildingType + "";
        if (buildingTypeStr.Contains("TheMainCity"))
        {
            this.baseBuildingDictionary.Add(factionType, buildingObj);
			buildingCtrl.Building.IsMainCity = true;
        }
        this.allBuildingsDictionary.Add(buildingCtrl.Building.BuildingId, buildingObj);
		return buildingObj;
    }

    public void DestroyAllBuildings()
    {
        GameObject tempObj = null;
        BuildingController tempCtrl = null;
        foreach (KeyValuePair<int, GameObject> kv in this.allBuildingsDictionary)
        {
            tempObj = kv.Value;
            tempCtrl = tempObj.GetComponent<BuildingController>();
            tempCtrl.DestroySelf();
        }
        this.allBuildingsDictionary.Clear();
        this.baseBuildingDictionary.Clear();
    }
	
	public void DestroyBuilding(GameObject building)
	{
		BuildingController buildingCtrl = building.GetComponent<BuildingController>();
		if(buildingCtrl != null)
		{
			this.allBuildingsDictionary.Remove(buildingCtrl.Building.BuildingId);
			if(this.baseBuildingDictionary.ContainsValue(building))
				this.baseBuildingDictionary.Remove(buildingCtrl.Building.FactionType);
			buildingCtrl.DestroySelf();
		}
	}

    public GameObject GetBaseBuildingOfFaction(FactionType faction)
    {
        GameObject baseBuilding = null;
        if (this.baseBuildingDictionary != null)
        {
            if (this.baseBuildingDictionary.ContainsKey(faction))
            {
                baseBuilding = this.baseBuildingDictionary[faction];
            }
        }
        return baseBuilding;
    }
	
	public List<GameObject> GetAllEnemyBuildings(FactionType factionType)
	{
		List<GameObject> result = new List<GameObject>(this.allBuildingsDictionary.Values);
		for(int i = 0; i < result.Count; i ++)
		{
			BuildingController buildingCtrl = result[i].GetComponent<BuildingController>();
			if(buildingCtrl.Building.FactionType == factionType)
				result.RemoveAt(i);
		}
		return result;
	}
	
	public List<GameObject> GetBuildingsOfFaction(FactionType factionType)
	{
		List<GameObject> result = new List<GameObject>(this.allBuildingsDictionary.Values);
		for(int i = 0; i < result.Count; i ++)
		{
			BuildingController buildingCtrl = result[i].GetComponent<BuildingController>();
			if(buildingCtrl.Building.FactionType != factionType)
				result.RemoveAt(i);
		}
		return result;
	}
	
	private int GenerateNewBuildingId()
    {
        int result;
        lock (_lock)
        {
            result = (++ this.buildingIdSeq);
        }
        return result;
    }
	
	public bool PayForTheBuilding(int buildingCost)
	{
		GameObject gameScene = GameObject.Find("GameSceneController");
		if(gameScene != null)
		{
			GameSceneController gameSceneCtrl = gameScene.GetComponent<GameSceneController>();
			if(gameSceneCtrl.CoinCount >= buildingCost)
			{
				gameSceneCtrl.CoinCount -= buildingCost;
				return true;
			}
		}
		return false;
	}

    #endregion
}