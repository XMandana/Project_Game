using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

[Serializable]
public class ColorItem
{
    public string Label;
    public Color ColorValue;
}

public class Game_Master : MonoBehaviour
{
    public List<ColorItem> ColorsList;
    public TextMeshProUGUI DisplayColorName;

    public Image[] Buttons;

    private int targetIndex;
    private int score;
    private float timeLeft;
    private bool gameActive;

    private int selected_Color_Index;

    [Header("UI Elements")]
    public TextMeshProUGUI ScoreDisplay;
    public TextMeshProUGUI TimerDisplay;
    public GameObject WinPanel;
    public GameObject LosePanel;


    [Header("Audio")]
    public AudioSource SuccessSound;
    public AudioSource FailSound;

    [Header("Settings")]
    public float initialTime = 5f;
    public float startDelay = 3f;


    public float Total_Time;


    void Start()
    {
        InitGame();
    }

    private void InitGame()
    {
        score = 0;
      
        StartCoroutine(StartGameWithDelay(startDelay));
    }

    private IEnumerator StartGameWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        AssignColors();
        gameActive = true;
        timeLeft = initialTime;
    }

     public List<int> usedIndices = new List<int>();
    private void AssignColors()
    {
        

        for (int i = 0; i < Buttons.Length; i++)
        {
            int randomIndex;
            do
            {
                randomIndex = UnityEngine.Random.Range(0, ColorsList.Count);

            } while (usedIndices.Contains(randomIndex));

            usedIndices.Add(randomIndex);

            Buttons[i].color = ColorsList[randomIndex].ColorValue;
            Buttons[i].GetComponent<Block_Controler>().Color_Name = ColorsList[randomIndex].Label;
           


        }
       
        targetIndex = UnityEngine.Random.Range(0, usedIndices.Count);


        DisplayColorName.text = ColorsList[usedIndices[targetIndex]].Label;

        selected_Color_Index = usedIndices[targetIndex];

        usedIndices.Clear();
    }


    void Update()
    {
        if (gameActive)
        {
            Total_Time+= Time.deltaTime;

            timeLeft -= Time.deltaTime;
            TimerDisplay.text = $"Timer: {timeLeft:F2}";

            if (timeLeft <= 0)
            {
                Lost_Time();
            }


        }
    }

    public void Check_The_Color(string selectedColor)
    {
        if (selectedColor == ColorsList[selected_Color_Index].Label)
        {
            HandleCorrectSelection();
        }
        else
        {
            HandleIncorrectSelection();
        }
    }

    private void HandleCorrectSelection()
    {
        SuccessSound.Play();
        UpdateScore(5);
        ResetGame();
    }

    private void HandleIncorrectSelection()
    {
        FailSound.Play();
        UpdateScore(-5);
        ResetGame();
    }


    public void Lost_Time()
    {
        LosePanel.SetActive(true);
        LosePanel.GetComponent<Panel_Controller>().Time.text = Total_Time.ToString("N2");
        LosePanel.GetComponent<Panel_Controller>().Score.text = score.ToString();

        gameActive = false;
    }


    private void UpdateScore(int change)
    {
        score += change;
        ScoreDisplay.text = $"Score: {score}";

        if (score >= 100)
        {
            WinPanel.SetActive(true);
            gameActive = false;
        }
        else if (score <= 0)
        {
            score = 0;
            LosePanel.SetActive(true);
           

            gameActive = false;
        }
    }

    private void ResetGame()
    {
        if (gameActive)
        {
            timeLeft = initialTime;
            AssignColors();
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

