﻿using UnityEngine;
using System.Collections;

public class ScoreKeeper : MonoBehaviour {
    public bool inGame = false;

	public float timeAlive = 0;
	public int score = 0;
    public int highScore;

    string HIGHSCORE_POS = "HSCORE";

    GameObject playerPrefab;
	EnemySpawner enemSpawner;
    void Start ()
    {
		enemSpawner = GetComponent<EnemySpawner> ();
        highScore = PlayerPrefs.GetInt(HIGHSCORE_POS, 0);
        playerPrefab = Resources.Load("Player") as GameObject;
    }
	
	// Update is called once per frame
	void Update () {
        if(inGame)timeAlive += Time.deltaTime;
    }
    
    public void StartGame()
    {
        foreach (GameObject gameObj in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Destroy(gameObj);
        }
        Time.timeScale = 1;
        SpawnPlayer();
        inGame = true;

    }

    void SpawnPlayer()
    {
        GameObject.Instantiate(playerPrefab, new Vector3(0, 1),Quaternion.identity);
    }

    public void OnGameEnd()
    {
		score = GetScore();
		if(score > highScore)highScore = score;
        timeAlive = 0;
        inGame = false;

        Time.timeScale = 0;
        Destroy(GameObject.FindGameObjectWithTag("Player"));
    }

    void OnDestroy()
    {
        PlayerPrefs.SetInt(HIGHSCORE_POS, highScore);
    }

    void OnGUI()
    {
		if (inGame) GUI.Box(new Rect(10, 10, Screen.width / 8, Screen.height / 8), "Score:" + GetScore() + "\n" + "HighScore: " + highScore);
        else {
            int buttonWidth = Screen.width / 8;
            int buttonHeight = Screen.height / 8;
			GUI.Box (new Rect (Screen.width / 2 - buttonWidth / 2, Screen.height / 2.5f - buttonHeight / 2, buttonWidth, buttonHeight), "Score: " + score + "\n" + "Highscore: " + highScore);
            if (GUI.Button(new Rect(Screen.width / 2 - buttonWidth/2, Screen.height / 2 - buttonHeight / 2, buttonWidth, buttonHeight), "Play Again")) StartGame();
            GUI.BeginGroup(new Rect(Screen.width / 2 - buttonWidth/1.5f, Screen.height / 1.5f - buttonHeight / 2f, buttonWidth * 1.5f, buttonHeight));
            GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
			EnemySpawner enemSpawner = GetComponent<EnemySpawner> ();
			if(GUILayout.Button("-"))enemSpawner.changeSpawnTime(-0.1f);
			if(GUILayout.Button("+"))enemSpawner.changeSpawnTime(0.1f);
			GUILayout.Label ("Enemy spawn time:" + enemSpawner.timeBetweenSpawn);
            GUILayout.EndHorizontal();
            GUI.EndGroup();
        }
    }

	int GetScore(){
		return Mathf.RoundToInt(timeAlive / enemSpawner.timeBetweenSpawn);
	}
}
