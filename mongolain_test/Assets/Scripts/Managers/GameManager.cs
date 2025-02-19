using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public DialogueUIController DUI; //thats my dui

    //for the future
    //public GameSettingsSO gameSettings; // Reference to game settings
    //public PlayerDataSO playerData; // Reference to player data
    // Start is called once before the first execution of Update after the MonoBehaviour is created
     private void Awake()
    {
        // Implement Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        //apply the game settings
        
    }

    // Update is called once per frame
    void Update()
    {
        //pause the game
    }
    //load a new map area// or reference a map class
    
    private void ApplyGameSettings()
    {
        // Apply game settings
        //AudioManager.Instance.SetVolume(gameSettings.masterVolume);
        //QualitySettings.SetQualityLevel(gameSettings.graphicsQuality);
    }


}
