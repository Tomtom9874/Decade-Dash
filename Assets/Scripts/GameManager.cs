using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public delegate void ValueChanged(int currentValue);

    public static GameManager Instance;
    private const int LevelCount = 17;
    [SerializeField] private int currentLevel;
    private bool[] _levelsCompleted;
    private int _lives = 3;
    private string _saveIDPath;
    private int _saveID;
    
    
    private int _score;
    

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(gameObject);
        
        _saveIDPath = Application.persistentDataPath + "/SaveID.dat";
        if (File.Exists(_saveIDPath))
        {
            _saveID = GetCurrentSaveID();
        }
        else
        {
            _saveID = 0;
        }

        
        _levelsCompleted = new bool[LevelCount];
        var saveFileExists = CheckForSave();
        if (saveFileExists) LoadGame();
        Debug.Log(_levelsCompleted.Length);
    }

    private void Start()
    {
        ScoreChanged?.Invoke(_score);
        LivesChanged?.Invoke(_lives);
    }

    private void OnEnable()
    {
        Collectable.CollectableCollides += AddScore;
    }

    private void OnDisable()
    {
        Collectable.CollectableCollides -= AddScore;
    }

    public static event ValueChanged ScoreChanged;
    public static event ValueChanged LivesChanged;

    private void AddScore(int amount)
    {
        _score += amount;
        ScoreChanged?.Invoke(_score);
    }

    private void AddLive(int amount)
    {
        _lives += amount;
        if (_lives < 0) GameOver();
        LivesChanged?.Invoke(_lives);
    }

    public void FinishLevel()
    {
        Debug.Log(_levelsCompleted.Length);
        Debug.Log(currentLevel - 1);
        _levelsCompleted[currentLevel - 1] = true;
        SaveGame();
        SceneManager.LoadScene("Level Navigator");
    }

    private bool CheckForSave()
    {
        var path = Application.persistentDataPath + "/playerInfo" + _saveID + ".dat";
        return File.Exists(path);
    }

    private int GetCurrentSaveID()
    {
        var bf = new BinaryFormatter();
        var file = File.Open(_saveIDPath, FileMode.Open);
        var id = (int) bf.Deserialize(file);
        file.Close();
        return id;
    }

    public void SetCurrentSaveID(int id)
    {
        var bf = new BinaryFormatter();
        var file = File.Create(_saveIDPath);
        bf.Serialize(file, id);
        file.Close();
    }

    public void GoToSceneManager()
    {
        SceneManager.LoadScene("Scenes/Level Navigator");
    }

    public void GoToNewGame()
    {
        SceneManager.LoadScene("Scenes/New Game");
    }

    public void GoToLoadGame()
    {
        SceneManager.LoadScene("Scenes/Load Screen");
    }

    public void DeleteSave(int id)
    {
        var path = Application.persistentDataPath + "/playerInfo" + id + ".dat";
        File.Delete(path);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void SaveGame()
    {
        var save = new SaveData();
        var bf = new BinaryFormatter();
        var path = Application.persistentDataPath + "/playerInfo" + _saveID + ".dat";
        var file = File.Create(path);
        bf.Serialize(file, save);
        file.Close();

        Debug.Log("Game Saved");
    }

    private void LoadGame()
    {
        var bf = new BinaryFormatter();
        var path = Application.persistentDataPath + "/playerInfo" + _saveID + ".dat";
        var file = File.Open(path, FileMode.Open);
        var saveData = (SaveData) bf.Deserialize(file);
        file.Close();
        _score = saveData.Score;
        _lives = saveData.Lives;
        _levelsCompleted = saveData.LevelsCompleted;
        Debug.Log("Game Loaded");
    }

    public int GetHighestLevelCompleted()
    {
        var highest = 0;
        foreach (var levelCompleted in _levelsCompleted)
        {
            if (!levelCompleted) return highest;
            highest++;
        }
        return highest;
    }    
    
    private void GameOver()
    {
        Debug.Log("Game OVer");
        _lives = 3;
        SaveGame();
        SceneManager.LoadScene("Game Over");
        Debug.Log("Scene loaded");
    }

    public void KillPlayer()
    {
        RestartScene();
        AddLive(-1);
        SaveGame();
    }

    private static void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public bool GetLevelIsCompleted(int levelID)
    {
        return _levelsCompleted[levelID];
    }
    

    [Serializable]
    private class SaveData
    {
        public SaveData()
        {
            LevelsCompleted = Instance._levelsCompleted;
            Score = Instance._score;
            Lives = Instance._lives;
        }

        public int Score { get; }
        public int Lives { get; }

        public bool[] LevelsCompleted { get; }
    }
}