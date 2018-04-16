using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleBehavior : MonoBehaviour
{
	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update ()
	{
	}

	public void StartGame()
	{
		SceneManager.LoadScene("Camera");
	}

	public void Leaderboard()
	{
		SceneManager.LoadScene("Leaderboard");
	}
}
