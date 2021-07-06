using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStarter : MonoBehaviour
{
    void Awake()
    {
        SceneManager.LoadSceneAsync(2, LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync(3, LoadSceneMode.Additive);
    }
}
