using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESCMenu : MonoBehaviour
{
    bool isCursorOpen;
    bool isMenuActive = false;
    public GameObject escMenu;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isMenuActive)
            {
                isCursorOpen = IsCursorOpen();
                escMenu.SetActive(true);
                CursorOpenClose(true);

                isMenuActive = true;
            }
            else
            {
                escMenu.SetActive(false);
                CursorOpenClose(isCursorOpen);

                isMenuActive = false;
            }
        }
    }

    public void CloseMenu()
    {
        escMenu.SetActive(false);
        CursorOpenClose(isCursorOpen);

        isMenuActive = false;
    }

    bool IsCursorOpen()
    {
        if(Cursor.visible) return true;
        else return false;
    }

    void CursorOpenClose(bool open)
    {
        if (open)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
