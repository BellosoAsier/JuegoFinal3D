using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinalScreen : MonoBehaviour
{
    [Header("Colors")]
    [SerializeField] private Color winScreen;
    [SerializeField] private Color loseScreen;
    // Start is called before the first frame update
    void Start()
    {
        int value = PlayerPrefs.GetInt("winOrLose");
        if (value == 0)
        {
            transform.GetChild(0).GetComponent<Image>().color = winScreen;
            transform.GetChild(1).GetComponent<TMP_Text>().text = "You Win";
            GetComponent<AudioSource>().Play();
        }
        else
        {
            transform.GetChild(0).GetComponent<Image>().color = loseScreen;
            transform.GetChild(1).GetComponent<TMP_Text>().text = "Game Over";
        }
    }

    public void ReturnMenu()
    {
        SceneManager.LoadSceneAsync("MenuScene");
    }
}
