using UnityEngine;
using System.Collections;

public class HudController : MonoBehaviour {

    public GameObject titleScreen;
    public GameObject gameOverScreen;
    public GameObject gameWinScreen;

    public static HudController instance;

    HudController getInstance()
    {
        return instance;
    }


    // Use this for initialization
    void Start()
    {


        Hide();
        ShowTitle();
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

    public void ShowTitle()
    {
        titleScreen.SetActive(true);
    }

    public void ShowGameOver()
    {
        gameOverScreen.SetActive(true);
    }
    public void ShowGameWin()
    {
        gameWinScreen.SetActive(true);
    }

    public void Hide()
    {
        titleScreen.SetActive(false);
        gameOverScreen.SetActive(false);
        gameWinScreen.SetActive(false);
    }

}
