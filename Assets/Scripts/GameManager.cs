using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum GameState{
    menu,
    inGame,
    gameOver
}

public class GameManager : MonoBehaviour
{
    public GameState currentGameState = GameState.menu;
    public static GameManager sharedInstance;

    PlayerController controller;
    AudioSource backgroudMusic;

    public int collectedObject = 0;

    private void Awake() 
    {
        if(sharedInstance == null) {
            sharedInstance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("Player").GetComponent<PlayerController>();
        backgroudMusic = GameObject.Find("BackgroundMusic").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Submit") && currentGameState != GameState.inGame){
            StartGame();
        }
    }
    public void StartGame(){
        setGameState(GameState.inGame);
        backgroudMusic.Play();
        
    }

    public void GameOver(){
        setGameState(GameState.gameOver);
    }

    public void BackToMenu() {
        setGameState(GameState.menu);
    }

    void setGameState(GameState newGameState){
        if(newGameState == GameState.menu){
            MenuManager.sharedInstance.ShowMainMenu();
            MenuManager.sharedInstance.HideGameMenu();
            MenuManager.sharedInstance.HideGameOverMenu();
        }
        else if(newGameState == GameState.inGame){
            //TODO: Hay que preparar la escena para jugar
            LevelManager.sharedInstance.RemoveAlllevelBlocks();
            LevelManager.sharedInstance.GenerateInitialBlocks();
            controller.StartGame();
            MenuManager.sharedInstance.HideMainMenu();
            MenuManager.sharedInstance.HideGameOverMenu();
            MenuManager.sharedInstance.ShowGameMenu();
            
        }
        else if(newGameState == GameState.gameOver){
            MenuManager.sharedInstance.HideGameMenu();
            MenuManager.sharedInstance.HideMainMenu();
            MenuManager.sharedInstance.ShowGameOverMenu();
        }
        else {

        }

        this.currentGameState = newGameState;
    }

    public void CollectObject(Collectable collectable)
    {
        collectedObject += collectable.value;
    }
}
