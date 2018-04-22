using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
			result => 
			{
				GameObject prefab = Resources.Load("Prefabs/LeaderboardRow") as GameObject;
				var container = GameObject.Find("ItemList");

				foreach(var li in result.Leaderboard)
				{
					GameObject item = Instantiate(prefab);

					var displayName = item.transform.Find("DisplayName").gameObject.GetComponent<Text>();
					var score = item.transform.Find("Score").gameObject.GetComponent<Text>();

					displayName.text = li.DisplayName;
					score.text = li.StatValue.ToString();

					item.transform.SetParent(container.transform, false);
					item.SetActive(true);
				}
			},
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
