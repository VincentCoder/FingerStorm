using UnityEngine;
using System.Collections;

public class UIEventController : MonoBehaviour 
{
    private GameController _gameController;
    public GameController GameController
    {
        get
        {
            if (this._gameController == null)
                this._gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
            return this._gameController;
        }
    }

    public void Register ( GameObject registerObj )
    { 
        if (registerObj != null)
        {
            UIImageButton imageButton = registerObj.GetComponent<UIImageButton>();
            if (imageButton != null)
            {
                UIEventListener.Get(registerObj).onClick = HandleEvent;
            }
            else
            {
                UIButton button = registerObj.GetComponent<UIButton>();
                if (button != null)
                {
                    UIEventListener.Get(registerObj).onClick = HandleEvent;
                }
            }
        }
    }

    public void UnRegister ( GameObject registerObj )
    {
        if (registerObj != null)
            UIEventListener.Get(registerObj).onClick = null;
    }

    public void RegisterInHierarchy ( GameObject registerObj )
    {
        if(registerObj != null)
        {
            UIImageButton[] imageButtons = registerObj.GetComponentsInChildren<UIImageButton>();
            foreach (UIImageButton imageButton in imageButtons)
            {
                UIEventListener.Get(imageButton.gameObject).onClick = HandleEvent;
            }
            UIButton[] buttons = registerObj.GetComponentsInChildren<UIButton>();
            foreach (UIButton button in buttons)
            {
                UIEventListener.Get(button.gameObject).onClick = HandleEvent;
            }
        }
    }

    public void UnRegisterInHierarchy ( GameObject registerObj )
    {
        if (registerObj != null)
        {
            UIImageButton[] imageButtons = registerObj.GetComponentsInChildren<UIImageButton>();
            foreach (UIImageButton imageButton in imageButtons)
            {
                UIEventListener.Get(imageButton.gameObject).onClick = null;
            }
            UIButton[] buttons = registerObj.GetComponentsInChildren<UIButton>();
            foreach (UIButton button in buttons)
            {
                UIEventListener.Get(button.gameObject).onClick = null;
            }
        }
    }

    private void HandleEvent ( GameObject eventObj )
    {
        string eventObjTag = eventObj.tag;

        switch (eventObjTag)
        {
            case "HomePageStartGameButton":
                {
					if(this.GameController.Client.State == ExitGames.Client.Photon.LoadBalancing.ClientState.JoinedLobby)
					{
						this.GameController.GetFSM().ChangeState(GameState_Matching.Instance());
						this.GameController.Client.OpJoinRandomRoom(null, 0);
					}
                    break;
                }
            case "HomePageOptionsButton":
                {
                    break;
                }
			case "HomePageCreateGameButton":
                {
					
                    break;
                }
			case "HomePageJoinGameButton":
                {
                    break;
                }
			case "GameResultBackToMenuButton":
			{
				Time.timeScale = 1;
				this.GameController.GetFSM().ChangeState(GameState_HomePage.Instance());
				break;
			}
        }
    }
}
