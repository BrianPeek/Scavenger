﻿using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleBehavior : MonoBehaviour
{
	private GameObject enterNameDialog;

	// Use this for initialization
	void Start () 
	{
		GameObject canvas = GameObject.Find("Canvas");
		enterNameDialog = canvas.transform.Find("EnterNameDialog").gameObject;

#if UNITY_ANDROID
		LoginWithAndroidDeviceIDRequest req = new LoginWithAndroidDeviceIDRequest
		{
			AndroidDeviceId = SystemInfo.deviceUniqueIdentifier,
			CreateAccount = true,
			InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
			{
				GetUserAccountInfo = true
			}
		};

		PlayFabClientAPI.LoginWithAndroidDeviceID(req, 
			result => 
				{
					string displayName = result?.InfoResultPayload?.AccountInfo?.TitleInfo?.DisplayName;
					var displayNameText = GameObject.Find("DisplayName").gameObject.GetComponent<Text>();
					displayNameText.text = displayName;

					Debug.Log($"User '{displayName}' logged in");

		//UpdatePlayerStatisticsRequest req2 = new UpdatePlayerStatisticsRequest();
		//req2.Statistics = new List<StatisticUpdate>
		//{
		//	new StatisticUpdate
		//	{
		//		StatisticName = "Score", 
		//		Value = 1
		//	}
		//};
		//PlayFabClientAPI.UpdatePlayerStatistics(req2, x => {}, y => {});

					if(string.IsNullOrEmpty(displayName))
					{
						Animation a = enterNameDialog.GetComponent<Animation>();
						a.Play();
					}
				},

			error => Debug.Log("Not logged in: " + error.GenerateErrorReport())
		);
#endif
	}

	public void EnterNameOkOnClick()
	{
		GameObject nameText = GameObject.Find("NameText");
		Text textComponent = nameText.GetComponent<Text>();
		string text = textComponent.text;

		if(!string.IsNullOrEmpty(text))
		{
			UpdateUserTitleDisplayNameRequest userReq = new UpdateUserTitleDisplayNameRequest
			{
				DisplayName = text
			};

			PlayFabClientAPI.UpdateUserTitleDisplayName(userReq,
				result => 
				{
					Debug.Log($"User '{result.DisplayName}' saved");
					Animation a = enterNameDialog.GetComponent<Animation>();
					a.clip = a.GetClip("DialogExit");
					a.Play();
				},
				error => Debug.Log("Failed updating user: " + error.GenerateErrorReport())
			);

		}
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
