using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
                Time.timeScale = 0f;
                isCursorOpen = IsCursorOpen();
                escMenu.SetActive(true);
                CursorOpenClose(true);

                isMenuActive = true;
            }
            else
            {
                Time.timeScale = 1f;
                escMenu.SetActive(false);
                CursorOpenClose(isCursorOpen);

                isMenuActive = false;
            }
        }
    }

    public void CloseMenu()
    {
        Time.timeScale = 1f;
        escMenu.SetActive(false);
        CursorOpenClose(isCursorOpen);

        isMenuActive = false;
    }

    bool IsCursorOpen()
    {
        if(Cursor.visible) return true;
        else return false;
    }

    public void MainMenu()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        Application.Quit();
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
