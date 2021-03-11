﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public void OnTestSceneClicked()
    {
        LevelManager.Load("TestScene1");
    }

    public void OnQuitClicked()
    {
        LevelManager.Quit();
    }
}
