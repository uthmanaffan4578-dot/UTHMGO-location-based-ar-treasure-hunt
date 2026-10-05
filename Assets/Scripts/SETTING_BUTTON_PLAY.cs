using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SETTING_BUTTON_PLAY : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(5, LoadSceneMode.Additive);
    }
}
