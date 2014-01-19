﻿using ExitGames.Client.Photon;
using ExitGames.Client.Photon.LoadBalancing;

using UnityEngine;
using HashTable = ExitGames.Client.Photon.Hashtable;

public class FSClient : LoadBalancingClient
{
	public string ErrorMessageToShow { get; set; }

    public Vector3 lastMoveEv;

    public int evCount = 0;
	
	public GameController gameController;
	
	public override void OnOperationResponse(OperationResponse operationResponse)
    {
        base.OnOperationResponse(operationResponse);
        this.DebugReturn(DebugLevel.ERROR, operationResponse.ToStringFull());

        switch (operationResponse.OperationCode)
        {
            case (byte)OperationCode.Authenticate:
                if (operationResponse.ReturnCode == ErrorCode.InvalidAuthentication)
                {
                    this.ErrorMessageToShow = string.Format("Authentication failed. Your AppId: {0}.\nMake sure to set the AppId in DemoGUI.cs by replacing \"<insert your app id here>\".\nResponse: {1}", this.AppId, operationResponse.ToStringFull());
                    this.DebugReturn(DebugLevel.ERROR, this.ErrorMessageToShow);
                }
                if (operationResponse.ReturnCode == ErrorCode.InvalidOperationCode || operationResponse.ReturnCode == ErrorCode.InternalServerError)
                {
                    this.ErrorMessageToShow = string.Format("Authentication failed. You successfully connected but the server ({0}) but it doesn't know the 'authenticate'. Check if it runs the Loadblancing server-logic.\nResponse: {1}", this.MasterServerAddress, operationResponse.ToStringFull());
                    this.DebugReturn(DebugLevel.ERROR, this.ErrorMessageToShow);
                }
                break;

            case (byte)OperationCode.CreateGame:
                if (!string.IsNullOrEmpty(this.GameServerAddress) && this.GameServerAddress.StartsWith("127.0.0.1"))
                {
                    this.ErrorMessageToShow = string.Format("The master forwarded you to a gameserver with address: {0}.\nThat address points to 'this computer' anywhere. This might be a configuration error in the game server.", this.GameServerAddress);
                    this.DebugReturn(DebugLevel.ERROR, this.ErrorMessageToShow);
                }
                break;

            case (byte)OperationCode.JoinRandomGame:
                if (!string.IsNullOrEmpty(this.GameServerAddress) && this.GameServerAddress.StartsWith("127.0.0.1"))
                {
                    this.ErrorMessageToShow = string.Format("The master forwarded you to a gameserver with address: {0}.\nThat address points to 'this computer' anywhere. This might be a configuration error in the game server.", this.GameServerAddress);
                    this.DebugReturn(DebugLevel.ERROR, this.ErrorMessageToShow);
                }
				Debug.Log(operationResponse.OperationCode);
                if (operationResponse.ReturnCode != 0)
                {
                    this.OpCreateRoom(null, true, true, 2, null, null);
                }
				else
				{
					this.gameController.MyFactionType = FactionType.Red;
					this.gameController.GetFSM().ChangeState(GameState_BeforeStartGame.Instance());
				}
                break;
        }
    }

    public override void OnEvent(EventData photonEvent)
    {
        base.OnEvent(photonEvent);

        switch (photonEvent.Code)
        {
            case (byte)1:
				Hashtable content = photonEvent.Parameters[ParameterCode.CustomEventContent] as Hashtable;
                this.lastMoveEv = (Vector3)content[(byte)1];
                this.evCount++;
                break;
			
			case EventCode.PropertiesChanged:
				var data = photonEvent.Parameters[ParameterCode.Properties] as Hashtable;
				DebugReturn(DebugLevel.ALL, "got something: " + (data["data"] as string));
				break;
			case EventCode.Join:
				this.gameController.GameSceneController.MyFactionType = FactionType.Blue;
				this.gameController.GetFSM().ChangeState(GameState_BeforeStartGame.Instance());
				break;
        }
    }

    public override void DebugReturn(DebugLevel level, string message)
    {
        base.DebugReturn(level, message);
        Debug.Log(message);
    }

    public override void OnStatusChanged(StatusCode statusCode)
    {
        base.OnStatusChanged(statusCode);

        switch (statusCode)
        {
            case StatusCode.Exception:
            case StatusCode.ExceptionOnConnect:
                Debug.LogWarning("Exception on connection level. Is the server running? Is the address (" + this.MasterServerAddress+ ") reachable?");
                break;
        }
    }
}