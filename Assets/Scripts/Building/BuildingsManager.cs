﻿#region

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
	
	public void CreateNewBuilding(BuildingType buildingType, FactionType factionType, Vector3 pos)
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
        }
        this.allBuildingsDictionary.Add(buildingCtrl.Building.BuildingId, buildingObj);
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
	
	private int GenerateNewBuildingId()
    {
        int result;
        lock (_lock)
        {
            result = (++ this.buildingIdSeq);
        }
        return result;
    }

    #endregion
}