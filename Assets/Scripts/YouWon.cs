﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class YouWon : MonoBehaviour {

    public string sceneToLoad;
    public void ClickAction()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
