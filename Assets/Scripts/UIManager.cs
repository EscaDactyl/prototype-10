using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

[DefaultExecutionOrder(1000)] // Make this spawn last
public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public TMP_InputField nameInput;
    public TextMeshProUGUI highScoreNameTMP;
    public TextMeshProUGUI highScoreScoreTMP;

    void Awake()
    {
        instance = this;
        FillData();
    }

    public void ChangeName()
    {
        GameData.instance.SetPlayerName(nameInput.text);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        GameData.instance.SaveGameData();

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    public void FillData()
    {
        nameInput.SetTextWithoutNotify(GameData.instance.currentPlayerName);

        // Blank out high score list
        highScoreNameTMP.text = "";
        highScoreScoreTMP.text = "";

        // load the high score list
        foreach (GameData.ScoreEntry thisScore in GameData.instance.currentScoreList)
        {
            // Check that the playername isn't blank AND the score isn't 0
            if (thisScore.playerName != "" && thisScore.playerScore > 0)
            {
                highScoreNameTMP.text += thisScore.playerName + "\n";
                highScoreScoreTMP.text += thisScore.playerScore.ToString("N0") + "\n";
            }
        }
    }
}
