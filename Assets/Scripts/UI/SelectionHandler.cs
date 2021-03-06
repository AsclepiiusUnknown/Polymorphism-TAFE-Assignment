﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectionHandler : MonoBehaviour
{
    public void GoToScene(int buildIndex)
    {
        SceneManager.LoadScene(buildIndex);
    }

    public void SceneProgress(int direction)
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentIndex + direction);
    }
}