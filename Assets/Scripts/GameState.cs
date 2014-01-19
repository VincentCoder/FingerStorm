using UnityEngine;
using System.Collections;

public class GameState_GlobalState : State<GameController> 
{
	private static GameState_GlobalState instance;
	
	public static GameState_GlobalState Instance()
	{
		return instance ?? (instance = new GameState_GlobalState());
	}
	
	public override void Enter (GameController entityType)
	{
		base.Enter (entityType);
	}
	
	public override void Execute (GameController entityType)
	{
		entityType.Client.Service();
	}
	
	public override void Exit (GameController entityType)
	{
		base.Exit (entityType);
	}
	
	public override bool OnMessage (GameController entityType, Telegram telegram)
	{
		return base.OnMessage (entityType, telegram);
	}
}

public class GameState_HomePage : State<GameController>
{
	private static GameState_HomePage instance;
	
	public static GameState_HomePage Instance()
	{
		return instance ?? (instance = new GameState_HomePage());
	}
	
	public override void Enter (GameController entityType)
	{
		if (entityType.ViewController != null)
            entityType.ViewController.ShowHomePage();
	}
	
	public override void Execute (GameController entityType)
	{
		base.Execute (entityType);
	}
	
	public override void Exit (GameController entityType)
	{
		base.Exit (entityType);
	}
	
	public override bool OnMessage (GameController entityType, Telegram telegram)
	{
		return base.OnMessage (entityType, telegram);
	}
}

public class GameState_Matching : State<GameController>
{
	private static GameState_Matching instance;
	
	public static GameState_Matching Instance()
	{
		return instance ?? (instance = new GameState_Matching());
	}
	
	public override void Enter (GameController entityType)
	{
		entityType.ViewController.SetHomePageButtonStatus(false);
		entityType.ViewController.ShowShadowCover();
	}
	
	public override void Execute (GameController entityType)
	{
		base.Execute (entityType);
	}
	
	public override void Exit (GameController entityType)
	{
		base.Exit (entityType);
	}
	
	public override bool OnMessage (GameController entityType, Telegram telegram)
	{
		return base.OnMessage (entityType, telegram);
	}
}

public class GameState_BeforeStartGame : State<GameController>
{
	private static GameState_BeforeStartGame instance;
	
	public static GameState_BeforeStartGame Instance()
	{
		return instance ?? (instance = new GameState_BeforeStartGame());
	}
	
	public override void Enter (GameController entityType)
	{
		entityType.GetFSM().ChangeState(GameState_StartGame.Instance());
	}
	
	public override void Execute (GameController entityType)
	{
		base.Execute (entityType);
	}
	
	public override void Exit (GameController entityType)
	{
		base.Exit (entityType);
	}
	
	public override bool OnMessage (GameController entityType, Telegram telegram)
	{
		return base.OnMessage (entityType, telegram);
	}
}

public class GameState_StartGame : State<GameController>
{
	private static GameState_StartGame instance;
	
	public static GameState_StartGame Instance()
	{
		return instance ?? (instance = new GameState_StartGame());
	}
	
	public override void Enter (GameController entityType)
	{
		entityType.StartGame();
	}
	
	public override void Execute (GameController entityType)
	{
		base.Execute (entityType);
	}
	
	public override void Exit (GameController entityType)
	{
		base.Exit (entityType);
	}
	
	public override bool OnMessage (GameController entityType, Telegram telegram)
	{
		return base.OnMessage (entityType, telegram);
	}
}

public class GameState_GameOver : State<GameController>
{
	private static GameState_GameOver instance;
	
	public static GameState_GameOver Instance()
	{
		return instance ?? (instance = new GameState_GameOver());
	}
	
	public override void Enter (GameController entityType)
	{
		base.Enter (entityType);
	}
	
	public override void Execute (GameController entityType)
	{
		base.Execute (entityType);
	}
	
	public override void Exit (GameController entityType)
	{
		base.Exit (entityType);
	}
	
	public override bool OnMessage (GameController entityType, Telegram telegram)
	{
		return base.OnMessage (entityType, telegram);
	}
}
