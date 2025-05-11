using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    public GameObject player;
    public GameObject cpuL;
    public GameObject cpuR;

    public GameObject successTxt;
    public GameObject failTxt;
    public TextMeshProUGUI timeTXT;

    private float timeTaken;

    private void Start()
    {
        Time.timeScale = 1f;
    }

    private void Update()
    {
        timeTaken += Time.deltaTime;

        if (!player)
        {
            ShowScreen();
            successTxt.SetActive(false);
            return;
        }

        if (!cpuL && !cpuR)
        {
            ShowScreen();
            failTxt.SetActive(false);
            return;
        }
    }

    private void ShowScreen()
    {
        if (timeTXT)
        {
            timeTaken = Mathf.Round(timeTaken * 100f) / 100f;
            timeTXT.text = $"Time: {timeTaken}";
        }

        Time.timeScale = 0f;
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Redo()
    {
        SceneManager.LoadScene(1);
    }
}
