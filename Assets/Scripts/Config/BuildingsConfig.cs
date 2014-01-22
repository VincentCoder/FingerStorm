using System.Collections.Generic;

public static class BuildingsConfig
{
    #region Public Methods and Operators

    public static string GetBuildingName(RaceType raceType, BuildingType buildingType)
    {
        if (raceType == RaceType.Terran)
        {
            switch (buildingType)
            {
                case BuildingType.Terran_Barrack:
                    return TerranBuildingBarrackConfig.BuildingName;
                case BuildingType.Terran_SniperHouse:
                    return TerranBuildingSniperHouseConfig.BuildingName;
                case BuildingType.Terran_ArtilleryLab:
                    return TerranBuildingArtilleryLabConfig.BuildingName;
                case BuildingType.Terran_MysterySchool:
                    return TerranBuildingMysterySchoolConfig.BuildingName;
                case BuildingType.Terran_Aviary:
                    return TerranBuildingAviaryConfig.BuildingName;
                case BuildingType.Terran_Fortress:
                    return TerranBuildingFortressConfig.BuildingName;
                case BuildingType.Terran_Church:
                    return TerranBuildingChurchConfig.BuildingName;
                case BuildingType.Terran_Temple:
                    return TerranBuildingTempleConfig.BuildingName;
            }
        }
        return string.Empty;
    }

    public static BuildingLevel GetMaxLevel ( RaceType raceType, BuildingType buildingType )
    {
        if (raceType == RaceType.Terran)
        {
            switch (buildingType)
            {
                case BuildingType.Terran_Barrack:
                    return TerranBuildingBarrackConfig.MaxLevel;
                case BuildingType.Terran_SniperHouse:
                    return TerranBuildingSniperHouseConfig.MaxLevel;
                case BuildingType.Terran_ArtilleryLab:
                    return TerranBuildingArtilleryLabConfig.MaxLevel;
                case BuildingType.Terran_MysterySchool:
                    return TerranBuildingMysterySchoolConfig.MaxLevel;
                case BuildingType.Terran_Aviary:
                    return TerranBuildingAviaryConfig.MaxLevel;
                case BuildingType.Terran_Fortress:
                    return TerranBuildingFortressConfig.MaxLevel;
                case BuildingType.Terran_Church:
                    return TerranBuildingChurchConfig.MaxLevel;
                case BuildingType.Terran_Temple:
                    return TerranBuildingTempleConfig.MaxLevel;
            }
        }
        return 0;
    }

    public static int GetCoinCost ( RaceType raceType, BuildingType buildingType, BuildingLevel buildingLevel )
    {
        if (raceType == RaceType.Terran)
        {
            switch (buildingType)
            {
                case BuildingType.Terran_Barrack:
                    return buildingLevel == BuildingLevel.BuildingLevel1 ? TerranBuildingBarrackConfig.CoinCostLevel1 : TerranBuildingBarrackConfig.CoinCostLevel2;
                case BuildingType.Terran_SniperHouse:
                    return buildingLevel == BuildingLevel.BuildingLevel1 ? TerranBuildingSniperHouseConfig.CoinCostLevel1 : TerranBuildingSniperHouseConfig.CoinCostLevel2;
                case BuildingType.Terran_ArtilleryLab:
                    return buildingLevel == BuildingLevel.BuildingLevel1 ? TerranBuildingArtilleryLabConfig.CoinCostLevel1 : TerranBuildingArtilleryLabConfig.CoinCostLevel2;
                case BuildingType.Terran_MysterySchool:
                    return TerranBuildingMysterySchoolConfig.CoinCostLevel1;
                case BuildingType.Terran_Aviary:
                    return buildingLevel == BuildingLevel.BuildingLevel1 ? TerranBuildingAviaryConfig.CoinCostLevel1 : TerranBuildingAviaryConfig.CoinCostLevel2;
                case BuildingType.Terran_Fortress:
                    return buildingLevel == BuildingLevel.BuildingLevel1 ? TerranBuildingFortressConfig.CoinCostLevel1 : TerranBuildingFortressConfig.CoinCostLevel2;
                case BuildingType.Terran_Church:
                    return buildingLevel == BuildingLevel.BuildingLevel1 ? TerranBuildingChurchConfig.CoinCostLevel1 : TerranBuildingChurchConfig.CoinCostLevel2;
                case BuildingType.Terran_Temple:
                    return buildingLevel == BuildingLevel.BuildingLevel1 ? TerranBuildingTempleConfig.CoinCostLevel1 : TerranBuildingTempleConfig.CoinCostLevel2;
            }
        }
        return 0;
    }

    public static int GetHp ( RaceType raceType, BuildingType buildingType, BuildingLevel buildingLevel )
    {
        if (raceType == RaceType.Terran)
        {
            switch (buildingType)
            {
                case BuildingType.Terran_Barrack:
                    return buildingLevel == BuildingLevel.BuildingLevel1 ? TerranBuildingBarrackConfig.HpLevel1 : TerranBuildingBarrackConfig.HpLevel2;
                case BuildingType.Terran_SniperHouse:
                    return buildingLevel == BuildingLevel.BuildingLevel1 ? TerranBuildingSniperHouseConfig.HpLevel1 : TerranBuildingSniperHouseConfig.HpLevel2;
                case BuildingType.Terran_ArtilleryLab:
                    return buildingLevel == BuildingLevel.BuildingLevel1 ? TerranBuildingArtilleryLabConfig.HpLevel1 : TerranBuildingArtilleryLabConfig.HpLevel2;
                case BuildingType.Terran_MysterySchool:
                    return TerranBuildingMysterySchoolConfig.HpLevel1;
                case BuildingType.Terran_Aviary:
                    return buildingLevel == BuildingLevel.BuildingLevel1 ? TerranBuildingAviaryConfig.HpLevel1 : TerranBuildingAviaryConfig.HpLevel2;
                case BuildingType.Terran_Fortress:
                    return buildingLevel == BuildingLevel.BuildingLevel1 ? TerranBuildingFortressConfig.HpLevel1 : TerranBuildingFortressConfig.HpLevel2;
                case BuildingType.Terran_Church:
                    return buildingLevel == BuildingLevel.BuildingLevel1 ? TerranBuildingChurchConfig.HpLevel1 : TerranBuildingChurchConfig.HpLevel2;
                case BuildingType.Terran_Temple:
                    return buildingLevel == BuildingLevel.BuildingLevel1 ? TerranBuildingTempleConfig.HpLevel1 : TerranBuildingTempleConfig.HpLevel2;
            }
        }
        return 0;
    }

    public static ActorType GetProducedActorType ( RaceType raceType, BuildingType buildingType, BuildingLevel buildingLevel )
    {
        if (raceType == RaceType.Terran)
        {
            switch (buildingType)
            {
                case BuildingType.Terran_Barrack:
                    return buildingLevel == BuildingLevel.BuildingLevel1 ? TerranBuildingBarrackConfig.ProducedActorTypeLevel1 : TerranBuildingBarrackConfig.ProducedActorTypeLevel2;
                case BuildingType.Terran_SniperHouse:
                    return buildingLevel == BuildingLevel.BuildingLevel1 ? TerranBuildingSniperHouseConfig.ProducedActorTypeLevel1 : TerranBuildingSniperHouseConfig.ProducedActorTypeLevel2;
                case BuildingType.Terran_ArtilleryLab:
                    return buildingLevel == BuildingLevel.BuildingLevel1 ? TerranBuildingArtilleryLabConfig.ProducedActorTypeLevel1 : TerranBuildingArtilleryLabConfig.ProducedActorTypeLevel2;
                case BuildingType.Terran_MysterySchool:
                    return TerranBuildingMysterySchoolConfig.ProducedActorTypeLevel1;
                case BuildingType.Terran_Aviary:
                    return buildingLevel == BuildingLevel.BuildingLevel1 ? TerranBuildingAviaryConfig.ProducedActorTypeLevel1 : TerranBuildingAviaryConfig.ProducedActorTypeLevel2;
                case BuildingType.Terran_Fortress:
                    return buildingLevel == BuildingLevel.BuildingLevel1 ? TerranBuildingFortressConfig.ProducedActorTypeLevel1 : TerranBuildingFortressConfig.ProducedActorTypeLevel2;
                case BuildingType.Terran_Church:
                    return buildingLevel == BuildingLevel.BuildingLevel1 ? TerranBuildingChurchConfig.ProducedActorTypeLevel1 : TerranBuildingChurchConfig.ProducedActorTypeLevel2;
                case BuildingType.Terran_Temple:
                    return buildingLevel == BuildingLevel.BuildingLevel1 ? TerranBuildingTempleConfig.ProducedActorTypeLevel1 : TerranBuildingTempleConfig.ProducedActorTypeLevel2;
            }
        }
        return 0;
    }

    public static int GetProducedTime ( RaceType raceType, BuildingType buildingType, BuildingLevel buildingLevel )
    {
        if (raceType == RaceType.Terran)
        {
            switch (buildingType)
            {
                case BuildingType.Terran_Barrack:
                    return buildingLevel == BuildingLevel.BuildingLevel1 ? TerranBuildingBarrackConfig.ProducedTimeLevel1 : TerranBuildingBarrackConfig.ProducedTimeLevel2;
                case BuildingType.Terran_SniperHouse:
                    return buildingLevel == BuildingLevel.BuildingLevel1 ? TerranBuildingSniperHouseConfig.ProducedTimeLevel1 : TerranBuildingSniperHouseConfig.ProducedTimeLevel2;
                case BuildingType.Terran_ArtilleryLab:
                    return buildingLevel == BuildingLevel.BuildingLevel1 ? TerranBuildingArtilleryLabConfig.ProducedTimeLevel1 : TerranBuildingArtilleryLabConfig.ProducedTimeLevel2;
                case BuildingType.Terran_MysterySchool:
                    return TerranBuildingMysterySchoolConfig.ProducedTimeLevel1;
                case BuildingType.Terran_Aviary:
                    return buildingLevel == BuildingLevel.BuildingLevel1 ? TerranBuildingAviaryConfig.ProducedTimeLevel1 : TerranBuildingAviaryConfig.ProducedTimeLevel2;
                case BuildingType.Terran_Fortress:
                    return buildingLevel == BuildingLevel.BuildingLevel1 ? TerranBuildingFortressConfig.ProducedTimeLevel1 : TerranBuildingFortressConfig.ProducedTimeLevel2;
                case BuildingType.Terran_Church:
                    return buildingLevel == BuildingLevel.BuildingLevel1 ? TerranBuildingChurchConfig.ProducedTimeLevel1 : TerranBuildingChurchConfig.ProducedTimeLevel2;
                case BuildingType.Terran_Temple:
                    return buildingLevel == BuildingLevel.BuildingLevel1 ? TerranBuildingTempleConfig.ProducedTimeLevel1 : TerranBuildingTempleConfig.ProducedTimeLevel2;
            }
        }
        return 0;
    }

    public static RaceType GetBuildingRaceType(BuildingType buildingType)
    {
        string temp = buildingType + "";
        if (temp.Contains("Terran")) return RaceType.Terran;
        return 0;
    }

    public static List<BuildingType> GetAllBuildingTypesOfRaceType(RaceType raceType)
    {
        List<BuildingType> result = new List<BuildingType>();
        switch (raceType)
        {
                case RaceType.Terran:
                {
                    result.Add(BuildingType.Terran_Barrack);
                    result.Add(BuildingType.Terran_SniperHouse);
                    result.Add(BuildingType.Terran_ArtilleryLab);
                    result.Add(BuildingType.Terran_MysterySchool);
                    result.Add(BuildingType.Terran_Aviary);
                    result.Add(BuildingType.Terran_Fortress);
                    result.Add(BuildingType.Terran_Church);
                    result.Add(BuildingType.Terran_Temple);
                    break;
                }
        }
        return result;
    }

    #endregion
}

public static class TerranBuildingBarrackConfig
{
    #region Static Fields

    public static string BuildingName = "兵营";

    public static BuildingType BuildingType = BuildingType.Terran_Barrack;

    public static int CoinCostLevel1 = 100;

    public static int CoinCostLevel2 = 175;

    public static int HpLevel1 = 1200;

    public static int HpLevel2 = 1300;

    public static BuildingLevel MaxLevel = BuildingLevel.BuildingLevel2;

    public static ActorType ProducedActorTypeLevel1 = ActorType.Infantry;

    public static ActorType ProducedActorTypeLevel2 = ActorType.Supporter;

    public static int ProducedTimeLevel1 = 20;

    public static int ProducedTimeLevel2 = 20;

    public static RaceType RaceType = RaceType.Terran;

    #endregion
}

public static class TerranBuildingSniperHouseConfig
{
    #region Static Fields

    public static string BuildingName = "狙击兵小屋";

    public static BuildingType BuildingType = BuildingType.Terran_SniperHouse;

    public static int CoinCostLevel1 = 140;

    public static int CoinCostLevel2 = 160;

    public static int HpLevel1 = 1200;

    public static int HpLevel2 = 1300;

    public static BuildingLevel MaxLevel = BuildingLevel.BuildingLevel2;

    public static ActorType ProducedActorTypeLevel1 = ActorType.Sniper;

    public static ActorType ProducedActorTypeLevel2 = ActorType.Marksman;

    public static int ProducedTimeLevel1 = 22;

    public static int ProducedTimeLevel2 = 32;

    public static RaceType RaceType = RaceType.Terran;

    #endregion
}

public static class TerranBuildingArtilleryLabConfig
{
    #region Static Fields

    public static string BuildingName = "火炮实验室";

    public static BuildingType BuildingType = BuildingType.Terran_ArtilleryLab;

    public static int CoinCostLevel1 = 180;

    public static int CoinCostLevel2 = 210;

    public static int HpLevel1 = 1300;

    public static int HpLevel2 = 1200;

    public static BuildingLevel MaxLevel = BuildingLevel.BuildingLevel2;

    public static ActorType ProducedActorTypeLevel1 = ActorType.HeavyGunner;

    public static ActorType ProducedActorTypeLevel2 = ActorType.MortarTeam;

    public static int ProducedTimeLevel1 = 31;

    public static int ProducedTimeLevel2 = 25;

    public static RaceType RaceType = RaceType.Terran;

    #endregion
}

public static class TerranBuildingMysterySchoolConfig
{
    #region Static Fields

    public static string BuildingName = "神秘学院";

    public static BuildingType BuildingType = BuildingType.Terran_MysterySchool;

    public static int CoinCostLevel1 = 300;

    public static int HpLevel1 = 1300;

    public static BuildingLevel MaxLevel = BuildingLevel.BuildingLevel1;

    public static ActorType ProducedActorTypeLevel1 = ActorType.Warlock;

    public static int ProducedTimeLevel1 = 30;

    public static RaceType RaceType = RaceType.Terran;

    #endregion
}

public static class TerranBuildingAviaryConfig
{
    #region Static Fields

    public static string BuildingName = "狮鹫笼";

    public static BuildingType BuildingType = BuildingType.Terran_Aviary;

    public static int CoinCostLevel1 = 250;

    public static int CoinCostLevel2 = 200;

    public static int HpLevel1 = 1200;

    public static int HpLevel2 = 1400;

    public static BuildingLevel MaxLevel = BuildingLevel.BuildingLevel2;

    public static ActorType ProducedActorTypeLevel1 = ActorType.GryphonRider;

    public static ActorType ProducedActorTypeLevel2 = ActorType.SeniorGryphonRider;

    public static int ProducedTimeLevel1 = 28;

    public static int ProducedTimeLevel2 = 32;

    public static RaceType RaceType = RaceType.Terran;

    #endregion
}

public static class TerranBuildingFortressConfig
{
    #region Static Fields

    public static string BuildingName = "要塞";

    public static BuildingType BuildingType = BuildingType.Terran_Fortress;

    public static int CoinCostLevel1 = 280;

    public static int CoinCostLevel2 = 230;

    public static int HpLevel1 = 1200;

    public static int HpLevel2 = 1500;

    public static BuildingLevel MaxLevel = BuildingLevel.BuildingLevel2;

    public static ActorType ProducedActorTypeLevel1 = ActorType.Crusader;

    public static ActorType ProducedActorTypeLevel2 = ActorType.TemplarWarrior;

    public static int ProducedTimeLevel1 = 29;

    public static int ProducedTimeLevel2 = 36;

    public static RaceType RaceType = RaceType.Terran;

    #endregion
}

public static class TerranBuildingChurchConfig
{
    #region Static Fields

    public static string BuildingName = "教堂";

    public static BuildingType BuildingType = BuildingType.Terran_Church;

    public static int CoinCostLevel1 = 350;

    public static int CoinCostLevel2 = 300;

    public static int HpLevel1 = 1200;

    public static int HpLevel2 = 1500;

    public static BuildingLevel MaxLevel = BuildingLevel.BuildingLevel2;

    public static ActorType ProducedActorTypeLevel1 = ActorType.Pastor;

    public static ActorType ProducedActorTypeLevel2 = ActorType.Sage;

    public static int ProducedTimeLevel1 = 30;

    public static int ProducedTimeLevel2 = 32;

    public static RaceType RaceType = RaceType.Terran;

    #endregion
}

public static class TerranBuildingTempleConfig
{
    #region Static Fields

    public static string BuildingName = "宫殿";

    public static BuildingType BuildingType = BuildingType.Terran_Temple;

    public static int CoinCostLevel1 = 400;

    public static int CoinCostLevel2 = 350;

    public static int HpLevel1 = 1200;

    public static int HpLevel2 = 1600;

    public static BuildingLevel MaxLevel = BuildingLevel.BuildingLevel2;

    public static ActorType ProducedActorTypeLevel1 = ActorType.Knight;

    public static ActorType ProducedActorTypeLevel2 = ActorType.Paladin;

    public static int ProducedTimeLevel1 = 40;

    public static int ProducedTimeLevel2 = 40;

    public static RaceType RaceType = RaceType.Terran;

    #endregion
}