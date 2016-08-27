using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public enum GameStateEnum
{
    Start, Flying, Gameover, Win
}

public class GameState : MonoBehaviour
{
    public GameStateEnum currentState = GameStateEnum.Start;
    public HudController hudController;
    public string currentLevel = "Level1";

    private bool buttonPressed = false;

    public static GameState instance;

    GameState getInstance()
    {
        return instance;
    }

    // Use this for initialization
    void Awake()
    {
        // First we check if there are any other instances conflicting
        if (instance != null && instance != this)
        {
            // If that is the case, we destroy other instances
            Destroy(gameObject);
        }
        // Here we save our singleton instance
        instance = this;

        // Furthermore we make sure that we don't destroy between scenes (this is optional)
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Restart()
    {
        GameObject.Find("PaperPlane").GetComponent<Rigidbody>().useGravity = false;
        currentState = GameStateEnum.Start;

        GameObject player = GameObject.Find("PaperPlane");

        if (player)
        {
            player.GetComponent<PlayerController>().Reset();
        }
        if (hudController)
        {
            hudController.Hide();
            hudController.ShowTitle();
        }
        Debug.Log("Back to title!");
    }

    public void StartGame()
    {
        // Start flying!
        SceneManager.LoadScene(currentLevel);
        // loading
        currentState = GameStateEnum.Flying;

        if (hudController)
        {
            hudController.Hide();
        }
        Debug.Log("Started!");
    }

    public void GameOver()
    {
        GameObject.Find("PaperPlane").GetComponent<Rigidbody>().useGravity = true;
        currentState = GameStateEnum.Gameover;
        if (hudController)
        {
            hudController.ShowGameOver();
        }
        Debug.Log("Game over!");
    }

    public void GameWin()
    {
        GameObject.Find("PaperPlane").GetComponent<Rigidbody>().useGravity = true;
        currentState = GameStateEnum.Win;
        if (hudController)
        {
            hudController.ShowGameWin();
        }
        Debug.Log("Bravo!");
    }


    void OnGUI()
    {
        if (Input.GetButton("Fire1"))
        {
            if(!buttonPressed)
            {
                if (currentState == GameStateEnum.Start)
                {
                    StartGame();
                }
                else if (currentState == GameStateEnum.Gameover)
                {
                    Restart();
                }
                else if (currentState == GameStateEnum.Win)
                {
                    currentState = GameStateEnum.Flying;
                    currentLevel = "Level2";
                    SceneManager.LoadScene("Level2");
                    if (hudController)
                    {
                        hudController.Hide();
                    }
                }
            }
            buttonPressed = true;
        }
        else
        {
            buttonPressed = false;
        }
    }
}
