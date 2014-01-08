using UnityEngine;
using System.Collections;

public class UIEventController : MonoBehaviour 
{
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
                    break;
                }
            case "HomePageOptionsButton":
                {
                    break;
                }
        }
    }

}
