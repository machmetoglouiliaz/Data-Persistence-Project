using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public string playerName;
    public string playerWithHighScore;
    public int highScore;

    private TextMeshProUGUI highscoreText;
    private TMP_InputField nameInputField;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance != null) Destroy(gameObject);

        playerWithHighScore = "None";
        highScore = 0;
        instance = this;
        DontDestroyOnLoad(gameObject);
        LoadData();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetPlayerName()
    {
        nameInputField = GameObject.Find("NameInputField").GetComponent<TMP_InputField>();
        if (nameInputField == null) Debug.Log("No TMP_inputfield");
        playerName = nameInputField.text;
    }

    public void StartGame()
    {
        SetPlayerName();
        SceneManager.LoadScene(1);
    }

    public void SaveData()
    {
        DataToSave data = new DataToSave();
        data.playerName = playerWithHighScore;
        data.highScore = highScore;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/SaveFile.json", json);
    }

    public void LoadData()
    {
        DataToSave data;
        string path = Application.persistentDataPath + "/SaveFile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);

            data = JsonUtility.FromJson<DataToSave>(json);
            playerWithHighScore = data.playerName;
            highScore = data.highScore;
        }
        UpdateHighscore();
    }

    public void UpdateHighscore()
    {
        if (highscoreText == null) highscoreText = GameObject.Find("HighScoreText").GetComponent<TextMeshProUGUI>();

        highscoreText.text = "Highscore: " + playerWithHighScore + " : " + highScore;
    }

    public void CheckHighScore(string name, int score)
    {
        if (score < highScore) return;

        highScore = score;
        playerWithHighScore = name;
        SaveData();
        UpdateHighscore();
    }

    [System.Serializable]
    private class DataToSave
    {
        public string playerName;
        public int highScore;
    }


}
