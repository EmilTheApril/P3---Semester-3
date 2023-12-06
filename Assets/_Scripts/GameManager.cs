using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public partial class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public List<GameObject> players = new List<GameObject>();
    public List<int> playerPoints = new List<int>();
    public int playersFinished;
    public int playersDNF;
    public bool isDrawing { get; private set; }

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
        
    }

    private void Update()
    {
        StopDrawingStartPython();
    }

    public async void StopDrawingStartPython()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isDrawing)
        {
            await StartPython();

            Application.OpenURL(Application.dataPath + "/CameraObjectDetection.exe");

            await FetchJSONFile();

            MapManager.instance.ClearMap();
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

    public void DrawingFinished()
    {
        isDrawing = false;
    }

    public void AddPlayer(GameObject player)
    {
        players.Add(player);
        playerPoints.Add(0);
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
            foreach (GameObject player in players)
            {
                player.GetComponent<PlayerMovement>().NewRound();
            }
        }
    }
}
