using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LeaderboardBehavior : MonoBehaviour
{
	// Use this for initialization
	void Start ()
	{
		GetLeaderboardRequest req = new GetLeaderboardRequest
		{
			MaxResultsCount = 10,
			StatisticName = "Score"
		};

		PlayFabClientAPI.GetLeaderboard(req, 
			result => Debug.Log(result.Leaderboard[0].DisplayName),
			error => Debug.Log("Error getting leaderboard: " + error.GenerateErrorReport())
		);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(Input.GetKeyDown(KeyCode.Escape))
			SceneManager.LoadScene("Title");
	}
}
