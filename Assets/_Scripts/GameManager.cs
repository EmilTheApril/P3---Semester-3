using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using System;

public partial class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public List<GameObject> players = new List<GameObject>();
    public List<int> playerPoints = new List<int>();
    public int playersFinished;
    public int playersDNF;
    public bool isDrawing { get; private set; }
    public bool startGame { get; private set; }

    [SerializeField] private GameObject uiPanel;
    [SerializeField] private GameObject platforms;
    [SerializeField] private TextMeshProUGUI playerScoresText;
    [SerializeField] private TextMeshProUGUI CountDownText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    private void Start()
    {
        CountDownText.text = "Press \"Space\" to start game (Minimum 2 players)";
    }

    private void Update()
    {
        StopDrawingStartPython();
        StartGame();
    }

    public void StartGame()
    {
        if (!startGame && players.Count >= 2)
        {
            isDrawing = true;
            startGame = true;
            DrawingFinished();
        }
    }

    public async void StopDrawingStartPython()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isDrawing)
        {
            uiPanel.SetActive(false);
            platforms.SetActive(false);

            Time.timeScale = 0;

            await StartPython();

            MapManager.instance.ClearMap();

            Application.OpenURL(Application.dataPath + "/CameraObjectDetection.exe");

            await FetchJSONFile();

            MapManager.instance.ReadJson();

            DrawingFinished();
        }
    }

    public async Task FetchJSONFile()
    {
        bool fileFetched = false;

        while (!fileFetched)
        {
            //fetchFile
            string filepath = Application.dataPath + "/data.json";
            if (File.Exists(filepath))
            {
                fileFetched = true;
            } else await Task.Delay(1000);
        }
    }

    public async Task StartPython() 
    {
        string filePath = Application.dataPath + "/start.txt";
        StreamWriter outStream = File.CreateText(filePath);
        outStream.WriteLine("");
        outStream.Close();
        await Task.Delay(1);
    }

    public async void DrawingFinished()
    {
        uiPanel.SetActive(true);
        platforms.SetActive(true);
        Time.timeScale = 1;
        CountDownText.text = "Starting in 3...";
        await Task.Delay(1000);
        CountDownText.text = "Starting in 2...";
        await Task.Delay(1000);
        CountDownText.text = "Starting in 1...";
        await Task.Delay(1000);
        CountDownText.text = "GOOOO!";
        isDrawing = false;
        await Task.Delay(1000);
        CountDownText.text = "";
    }

    public void AddPlayer(GameObject player)
    {
        players.Add(player);
        playerPoints.Add(0);
        UpdatePlayerScoreText();
    }

    public void PlayerFinished(GameObject player)
    {
        playersFinished++;
        int index = 0;
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i] == player)
            {
                index = i;
                break;
            }
        }
        playerPoints[index] += players.Count - playersFinished;
        CheckIfRoundDone();
    }

    public void PlayerDied(GameObject player)
    {
        playersDNF++;
        CheckIfRoundDone();
    }

    public void CheckIfRoundDone()
    {
        if (playersDNF + playersFinished == players.Count)
        {
            playersDNF = 0;
            playersFinished = 0;
            isDrawing = true;
            uiPanel.SetActive(true);
            foreach (GameObject player in players)
            {
                player.GetComponent<PlayerMovement>().NewRound();
            }

            UpdatePlayerScoreText();
            CountDownText.text = "Time to draw!\nPress \"Space\" to stop drawing";
        }
    }

    public void UpdatePlayerScoreText()
    {
        playerScoresText.text = "";
        for (int i = 0; i < playerPoints.Count; i++)
        {
            if (i < playerPoints.Count - 1)
            {
                playerScoresText.text += $"Player {i + 1}: {playerPoints[i]}    ";
            }
            else playerScoresText.text += $"Player {i + 1}: {playerPoints[i]}";
        }
    }
}
