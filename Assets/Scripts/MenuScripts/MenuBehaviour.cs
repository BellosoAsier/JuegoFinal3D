using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuBehaviour : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadSceneAsync("GameScene");
    }
}
