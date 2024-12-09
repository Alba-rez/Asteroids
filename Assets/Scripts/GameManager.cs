using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class GameManager : MonoBehaviour
{
    const int LIVES = 3;
    const int SCORE_ENEMY = 50;
    const int SCORE_ASTEROID_BIG = 10;
    const int SCORE_ASTEROID_SMALL = 25;
    const int EXTRA_LIVE = 1500;
    const string DATA_FILE = "data.jason";

    static GameManager instance;

    [SerializeField] Text txtScore;
    [SerializeField] Text txtHScore;
    [SerializeField] Text txtMessage;
    [SerializeField] Image[] imgLives;
    [SerializeField] AudioClip sfxExtra;
    [SerializeField] AudioClip sfxGameOver;
    int score;
    int lives=LIVES;

    
    bool extra=false;
    bool gameover;
    bool paused = false;

    GameData gameData;

   
    public static GameManager GetInstance()
    {
        return instance;
    }
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        
    }

    private void Start()
    {
        gameData = LoadData();
    }

    GameData LoadData()
    {
        if (File.Exists(DATA_FILE))
        {
            string fileText = File.ReadAllText(DATA_FILE);
            return JsonUtility.FromJson<GameData>(fileText);
        }
        else
        {
            return new GameData();
        }

    }

    public bool isGameOver()
    {
        return gameover;
    }

    public bool isGamePaused()
    {
        return paused;
    }

    public void AddScore(string tag)
    {
        int pts = 0;

        switch (tag)
        {
            case "Enemy":
                pts = SCORE_ENEMY;
                break;
            case "AsteroidBig":
                pts = SCORE_ASTEROID_BIG;
                break;
            case "AsteroidSmall":
	            pts = SCORE_ASTEROID_SMALL;
                break;

        }
        score += pts;
        if(!extra && score >= EXTRA_LIVE)
        {
            ExtraLife();
        }
        if (score > gameData.hscore)
        {
            gameData.hscore = score;
        }


    }

    void ExtraLife()
    {
        extra = true;
        lives++;
        AudioSource.PlayClipAtPoint(sfxExtra, Vector3.zero, 1);
    }

   public  void LoseLive()
    {
        lives--;
        if (lives == 0)
        {
            GameOver();
        }
    }
     void GameOver()
    {
        gameover = true;
        Time.timeScale = 1;
        AudioSource.PlayClipAtPoint(sfxGameOver, new Vector3(0, 0, -10), 1);
        txtMessage.text = "GAME OVER \n PRESS <RET> TO START";
        SaveData();
       
    }

    void SaveData()
    {
        // creamos la representación json de gameData 
        string json = JsonUtility.ToJson(gameData);
        // escribimos en el archivo esos datos 
       File.WriteAllText(DATA_FILE, json);
}

    private void OnGUI()
    {
        for(int i = 0; i < imgLives.Length; i++)
        {
            imgLives[i].enabled = i < lives - 1;
            
        }
        txtScore.text = string.Format("{0,4:D4}", score);
        txtHScore.text = string.Format("{0,4:D4}", gameData.hscore);

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        else if (!gameover)  
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                if (paused)
                {
                    ResumeGame();
                }
                else
                {
                    PauseGame();
                }
            }
            else if (Input.GetKeyDown(KeyCode.F1))
            {
                Time.timeScale /= 1.25f;
            }
            else if (Input.GetKeyDown(KeyCode.F2))
            {
                Time.timeScale *= 1.25f;
            }
            else if (Input.GetKeyDown(KeyCode.F3))
            {
                Time.timeScale = 1;
            }


        }

        if (gameover && Input.GetKeyUp(KeyCode.Return)) 
            SceneManager.LoadScene(0);
    }

    void PauseGame()
    {
        
        paused = true;
        Camera.main.GetComponent<AudioSource>().Stop();
        txtMessage.text = "PAUSED \nPRESS <P> TO RESUME";
        Time.timeScale = 0;
    }

    void ResumeGame()
    {
        paused = false;
        Camera.main.GetComponent<AudioSource>().Play();
        txtMessage.text = "";
        Time.timeScale = 1;

    }
}
