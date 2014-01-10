using System.Collections.Generic;

using UnityEngine;

public class BuildingsManager 
{
	private static BuildingsManager instance;
    private static object _lock = new object();

    private BuildingsManager ()
    {
    }

    public static BuildingsManager GetInstance ()
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

    private Dictionary<FactionType, GameObject> baseBuildingDictionary = new Dictionary<FactionType, GameObject>();
    private Dictionary<int, GameObject> allBuildingsDictionary = new Dictionary<int, GameObject>(); 

    public void CreateNewBuilding(Building building)
    {
        GameObject buildingObj = (GameObject)MonoBehaviour.Instantiate(Resources.Load("GameScene/Building"));
        buildingObj.name = "Building";
        buildingObj.transform.localScale = new Vector3(1, 1, 1);
        BuildingController buildingCtrl = buildingObj.GetComponent<BuildingController>();
        buildingCtrl.Building = building;
        if (building.IsBase)
        {
            this.baseBuildingDictionary.Add(building.FactionType, buildingObj);
        }
        this.allBuildingsDictionary.Add(building.BuildingId, buildingObj);
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

}
