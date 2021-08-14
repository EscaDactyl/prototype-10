using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData instance;

    const int highScoreLimit = 20;

    [System.Serializable]
    public class ScoreEntry
    {
        public string playerName;
        public int playerScore;
    }

    public class SaveData
    {
        public string initialPlayerName;
        public ScoreEntry[] highScoreList;
    }

    public ScoreEntry[] currentScoreList = new ScoreEntry[highScoreLimit];
    public string currentPlayerName;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadGameData();
        }

    }

    public void SaveGameData()
    {
        SaveData thisData = new SaveData();
        thisData.highScoreList = currentScoreList;
        thisData.initialPlayerName = currentPlayerName;

        string jsonFile = JsonUtility.ToJson(thisData);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", jsonFile);
    }

    public void LoadGameData()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        Debug.Log(path); // Very useful when the file becomes corrupt/wrong

        if (File.Exists(path))
        {
            string jsonFile = File.ReadAllText(path);
            SaveData thisData = JsonUtility.FromJson<SaveData>(jsonFile);

            SetPlayerName(thisData.initialPlayerName);
            currentScoreList = thisData.highScoreList;
        }
    }

    public void SortScore(int scoreToCheck)
    {
        ScoreEntry tempEntry = new ScoreEntry();
        bool isPushingScoresDown = false;

        for(int n = 0; n < currentScoreList.Length; n++)
        {
            if(isPushingScoresDown)
            {
                ScoreEntry thisEntry = currentScoreList[n];
                currentScoreList[n] = tempEntry;
                tempEntry = thisEntry;
            }
            else if (scoreToCheck > currentScoreList[n].playerScore)
            {
                tempEntry.playerScore = scoreToCheck;
                tempEntry.playerName = currentPlayerName;

                ScoreEntry thisEntry = currentScoreList[n];
                currentScoreList[n] = tempEntry;
                tempEntry = thisEntry;
                isPushingScoresDown = true;
            }
        }
    }

    public void SetPlayerName(string playerName)
    {
        currentPlayerName = playerName;
        SaveGameData();
    }

}
