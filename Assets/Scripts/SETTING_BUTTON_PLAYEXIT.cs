using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SETTING_BUTTON_PLAYEXIT : MonoBehaviour
{
    public void PlayGame()
    {
         SceneManager.UnloadSceneAsync("SETTING PLAY");
    }
}