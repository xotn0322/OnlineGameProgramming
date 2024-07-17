using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UI_End : MonoBehaviour
{
    public static UI_End instance;

    public AudioListener listener;
    public GameObject End;
    public GameObject Clear_Scene;
    public TMP_Text keys;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private void Start()
    {
        End.SetActive(false);
        Clear_Scene.SetActive(false);
        Time.timeScale = 1;
        listener.enabled = true;
    }

    private void Update()
    {
        keys.text = "Key : " + Door_lock.instance.getKey + "/4";
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Retry()
    {
        Cursor.visible = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Dead()
    {
        listener.enabled = false;
        End.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
    }

    public void Clear()
    {
        listener.enabled = false;
        Clear_Scene.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
    }
}
