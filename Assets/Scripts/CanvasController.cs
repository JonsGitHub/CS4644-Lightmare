using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    private enum MouseState
    {
        Locked, Unlocked
    }
    
    private MouseState CurrentState;
    private GameObject Fade;
    private GameObject PauseMenu;

    void Awake()
    {
        Fade = transform.Find("Fade").gameObject;
        PauseMenu = transform.Find("Pause_Menu").gameObject;
        
        Unpause();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (CurrentState == MouseState.Unlocked)
            {
                Unpause();
            }
            else
            {
                Pause();
            }
        }
    }

    public void OnContinuePressed()
    {
        Unpause();
    }

    private void Unpause()
    {
        PauseMenu.SetActive(false);
        Fade.SetActive(false);

        CurrentState = MouseState.Locked;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        Time.timeScale = 1;
    }

    private void Pause()
    {
        PauseMenu.SetActive(true);
        Fade.SetActive(true);

        CurrentState = MouseState.Unlocked;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        Time.timeScale = 0;
    }

    
}
