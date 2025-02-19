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
        
    }

    // Update is called once per frame
    void Update()
    {
        
       
    }
}
