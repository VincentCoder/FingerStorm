using UnityEngine;
using System.Collections;
using System.Text;

public class UIViewController : MonoBehaviour 
{
    private GameObject _rootPanel;
    private GameObject _loadingPage;
    private GameController _gameController;

    private GameObject RootPanel
    {
		get
		{
			if(this._rootPanel == null)
				this._rootPanel = GameObject.Find("PanelRoot");
			return this._rootPanel;
		}
    }

    public void ShowHomePage ()
    {
        GameObject homePage = (GameObject)Instantiate(Resources.Load("UI/HomePage"));
        homePage.name = "HomePage";
        homePage.tag = "HomePage";
        homePage.transform.parent = this.RootPanel.transform;
        homePage.transform.localScale = new Vector3(1, 1, 1);
        homePage.transform.localPosition = new Vector3(0, 0, 0);
        this.GameController.EventController.RegisterInHierarchy(homePage);
    }
	
	public void SetHomePageButtonStatus(bool status)
	{
		GameObject homePage = GameObject.FindWithTag("HomePage");
		UIImageButton[] buttons = homePage.GetComponentsInChildren<UIImageButton>();
		for(int i = 0; i < buttons.Length; i ++)
		{
			UIImageButton button = buttons[i];
			button.enabled = status;
		}
	}

    public void DestroyHomePage (bool now)
    {
        GameObject homePage = GameObject.FindWithTag("HomePage");
        this.GameController.EventController.UnRegisterInHierarchy(homePage);
        if (now)
            DestroyImmediate(homePage);
        else
            Destroy(homePage);
    }
	
	public void ShowBuildingsSelectorPanel()
	{
		GameObject selectorPanel = (GameObject)Instantiate(Resources.Load("UI/BuildingsSelectorPanel"));
		selectorPanel.name = "BuildingsSelectorPanel";
		selectorPanel.tag = "GameSceneBuildingsSelectorPanel";
		selectorPanel.transform.parent = this.RootPanel.transform;
		selectorPanel.transform.localScale = new Vector3(1,1,1);
		selectorPanel.transform.localPosition = new Vector3(-125,-240, 0);
		Debug.Log(this.GameController.MyFactionType);
		this.AddBuildingCard(new Building(0, BuildingType.Terran_Barrack,this.GameController.MyFactionType), 0);
		this.AddBuildingCard(new Building(0, BuildingType.Terran_Fortress,this.GameController.MyFactionType), 1);
		this.AddBuildingCard(new Building(0, BuildingType.Terran_SniperHouse,this.GameController.MyFactionType), 2);
		this.AddBuildingCard(new Building(0, BuildingType.Terran_MarksmanCamp,this.GameController.MyFactionType), 3);
		this.AddBuildingCard(new Building(0, BuildingType.Terran_ArtilleryHall,this.GameController.MyFactionType), 4);
		this.AddBuildingCard(new Building(0, BuildingType.Terran_ArtilleryLab,this.GameController.MyFactionType), 5);
		this.AddBuildingCard(new Building(0, BuildingType.Terran_MysterySchool,this.GameController.MyFactionType), 6);
		this.AddBuildingCard(new Building(0, BuildingType.Terran_Aviary,this.GameController.MyFactionType), 7);
		this.AddBuildingCard(new Building(0, BuildingType.Terran_AdvancedAviary,this.GameController.MyFactionType), 8);
		this.AddBuildingCard(new Building(0, BuildingType.Terran_Church,this.GameController.MyFactionType), 9);
		this.AddBuildingCard(new Building(0, BuildingType.Terran_Temple,this.GameController.MyFactionType), 10);
	}
	
	public void DestroyBuildingsSelectorPanel(bool now)
	{
		GameObject selectorPanel = GameObject.FindWithTag("GameSceneBuildingsSelectorPanel");
		if(now)
			DestroyImmediate(selectorPanel);
		else
			Destroy(selectorPanel);
	}
	
	public void ShowBuildingDetailPanel()
	{
		GameObject detailPanel = (GameObject)Instantiate(Resources.Load("UI/BuildingDetailPanel"));
		detailPanel.name = "BuildingDetailPanel";
		detailPanel.tag = "GameSceneBuildingDetailPanel";
		detailPanel.transform.parent = this.RootPanel.transform;
		detailPanel.transform.localScale = new Vector3(1,1,1);
		detailPanel.transform.localPosition = new Vector3(355, -240, 0);
	}
	
	public void DestroyBuildingDetailPanel(bool now)
	{
		GameObject detailPanel = GameObject.FindWithTag("GameSceneBuildingDetailPanel");
		if(now)
			DestroyImmediate(detailPanel);
		else
			Destroy(detailPanel);
	}
	
	public void AddBuildingCard(Building building, int index)
	{
		GameObject buildingCard = (GameObject)Instantiate(Resources.Load("UI/BuildingCard"));
		buildingCard.name = building.BuildingType + "";
		buildingCard.tag = "GameSceneBuildingCard";
		buildingCard.transform.parent = GameObject.FindWithTag("GameSceneBuildingsSelectorPanel").transform.FindChild("UIGrid").transform;
		buildingCard.transform.localScale = new Vector3(1,1,1);
		buildingCard.transform.localPosition = new Vector3(index*100,0,0);
		
		StringBuilder spriteName = new StringBuilder(string.Empty);
        spriteName.Append(building.BuildingType);
        spriteName.Append("_");
        spriteName.Append(building.FactionType);
		buildingCard.transform.FindChild("Building").gameObject.GetComponent<UISprite>().spriteName = spriteName.ToString();
		buildingCard.transform.FindChild("GoldCost").gameObject.GetComponent<UILabel>().text = building.CoinCost.ToString();
		buildingCard.transform.FindChild("BuildingName").gameObject.GetComponent<UILabel>().text = building.BuildingName;
		
		UIBuildingCardController buildingCardCtrl = buildingCard.GetComponent<UIBuildingCardController>();
		buildingCardCtrl.Building = building;
		switch(building.BuildingType)
		{
			case BuildingType.Terran_Barrack:
				buildingCardCtrl.Description = "兵营：\n    生产兵种：步兵\n    生产时间：20秒/个\n    血量：1200\n步兵：\n    攻击类型：普通近战\n    攻击力：18\n    防御力：重甲4\n    技能：无";
				break;
			case BuildingType.Terran_Fortress:
				buildingCardCtrl.Description = "要塞：\n    生产兵种：拥护者\n    生产时间：20秒/个\n    血量：1300\n拥护者：\n    攻击类型：普通近战\n    攻击力：38\n    防御力：重甲7\n    技能：闪避";
				break;
			case BuildingType.Terran_SniperHouse:
				buildingCardCtrl.Description = "狙击兵小屋：\n    生产兵种：狙击兵\n    生产时间：22秒/个\n    血量：1200\n狙击兵：\n    攻击类型：穿刺\n    攻击力：22\n    防御力：轻甲0\n    技能：致命一击";
				break;
			case BuildingType.Terran_MarksmanCamp:
				buildingCardCtrl.Description = "神射手营地：\n    生产兵种：神射手\n    生产时间：32秒/个\n    血量：1300\n神射手：\n    攻击类型：穿刺\n    攻击力：61\n    防御力：轻甲1\n    技能：爆头";
				break;
			case BuildingType.Terran_MysterySchool:
				buildingCardCtrl.Description = "神秘学院：\n    生产兵种：术士\n    生产时间：30秒/个\n    血量：1300\n术士：\n    攻击类型：混乱\n    攻击力：17\n    防御力：轻甲1\n    技能：奥术爆炸";
				break;
			case BuildingType.Terran_ArtilleryHall:
				buildingCardCtrl.Description = "炮兵大厅：\n    生产兵种：重装炮兵\n    生产时间：31秒/个\n    血量：1300\n重装炮兵：\n    攻击类型：穿刺\n    攻击力：53\n    防御力：轻甲3\n    技能：溅射伤害";
				break;
			case BuildingType.Terran_ArtilleryLab:
				buildingCardCtrl.Description = "火炮实验室：\n    生产兵种：迫击炮小队\n    生产时间：25秒/个\n    血量：1200\n迫击炮小队：\n    攻击类型：攻城\n    攻击力：21\n    防御力：轻甲0\n    技能：迫击轰炸";
				break;
			case BuildingType.Terran_Aviary:
				buildingCardCtrl.Description = "狮鹫笼：\n    生产兵种：狮鹫骑士\n    生产时间：28秒/个\n    血量：1200\n狮鹫骑士：\n    攻击类型：魔法\n    攻击力：23\n    防御力：轻甲2\n    技能：重击/闪电链";
				break;
			case BuildingType.Terran_AdvancedAviary:
				buildingCardCtrl.Description = "高级狮鹫笼：\n    生产兵种：高级狮鹫骑士\n    生产时间：32秒/个\n    血量：1400\n高级狮鹫骑士：\n    攻击类型：魔法\n    攻击力：30\n    防御力：轻甲5\n    技能：重击/闪电链";
				break;
			case BuildingType.Terran_Church:
				buildingCardCtrl.Description = "教堂：\n    生产兵种：十字军\n    生产时间：29秒/个\n    血量：1200\n十字军：\n    攻击类型：普通近战\n    攻击力：42\n    防御力：重甲6\n    技能：重击/天赐祝福";
				break;
			case BuildingType.Terran_Temple:
				buildingCardCtrl.Description = "圣殿：\n    生产兵种：圣堂勇士\n    生产时间：36秒/个\n    血量：1500\n圣堂勇士：\n    攻击类型：英雄攻击\n    攻击力：67\n    防御力：重甲9\n    技能：重击/天赐祝福/圣光术";
				break;
		}
		
	}
	
	public GameObject ShowMenuBar()
	{
		GameObject menuBar = (GameObject)Instantiate(Resources.Load("UI/MenuBar"));
		menuBar.name = "MenuBar";
		menuBar.tag = "GameSceneMenuBar";
		menuBar.transform.parent = this.RootPanel.transform;
		menuBar.transform.localScale = new Vector3(1, 1, 1);
		menuBar.transform.localPosition = new Vector3(414, 285, 0);
		return menuBar;
	}
	
	public void DestroyMenuBar(bool now)
	{
		GameObject menuBar = GameObject.FindWithTag("GameSceneMenuBar");
		if(now)
			DestroyImmediate(menuBar);
		else
			Destroy(menuBar);
	} 
	
	public void ShowShadowCover()
	{
		GameObject shadowCover = (GameObject)Instantiate(Resources.Load("UI/ShadowCover"));
		shadowCover.name = "ShadowCover";
		shadowCover.tag = "ShadowCover";
		shadowCover.transform.parent = this.RootPanel.transform;
		shadowCover.transform.localScale = new Vector3(1, 1, 1);
		shadowCover.transform.localPosition = new Vector3(0, 0, 0);
	}
	
	public void DestroyShadowCover(bool now)
	{
		GameObject shadowCover = GameObject.FindWithTag("ShadowCover");
		if(now)
			DestroyImmediate(shadowCover);
		else
			Destroy(shadowCover);
	} 
	
	public void ShowGameResultView(bool win)
	{
		GameObject gameResult = (GameObject)Instantiate(Resources.Load("UI/GameResult"));
		gameResult.name = "GameResultView";
		gameResult.tag = "GameResultView";
		gameResult.transform.parent = this.RootPanel.transform;
		gameResult.transform.localScale = new Vector3(1, 1, 1);
		gameResult.transform.FindChild("ResultLabel").gameObject.GetComponent<UILabel>().text = win ? "胜利" : "失败";
		
		this.GameController.EventController.RegisterInHierarchy(gameResult);
	}
	
	public void DestroyGameResultView(bool now)
	{
		GameObject gameResult = GameObject.FindWithTag("GameResultView");
		this.GameController.EventController.UnRegisterInHierarchy(gameResult);
		if(now)
			DestroyImmediate(gameResult);
		else
			Destroy(gameResult);
	}
	
    public GameController GameController
    {
        get
        {
            if (this._gameController == null)
                this._gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
            return this._gameController;
        }
    }
}
