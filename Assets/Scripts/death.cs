using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class death : MonoBehaviour
{
    public GameObject DeathUI;
    public GameObject Counter;
    public TMP_Text ScoreNum;
    public TMP_Text CounterNum;

    void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object is on a specific layer
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            ShowDeathScreen();
        }
    }

    void ShowDeathScreen()
    {
        Counter.SetActive(false);
        ScoreNum.text = "You delivered " + CounterNum.text;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
        DeathUI.SetActive(true);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f; // Reset time scale before loading
        SceneManager.LoadScene("MainMenu");
    }

    
}
