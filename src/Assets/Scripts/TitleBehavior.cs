using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleBehavior : MonoBehaviour
{
	// Use this for initialization
	void Start () 
	{
		//PlayFabSettings.TitleId = "A246";

#if UNITY_ANDROID
		LoginWithAndroidDeviceIDRequest req = new LoginWithAndroidDeviceIDRequest
		{
			AndroidDeviceId = SystemInfo.deviceUniqueIdentifier,
			CreateAccount = true
		};
		PlayFabClientAPI.LoginWithAndroidDeviceID(req, OnLoginSuccess, OnLoginFailure);
#endif
	}

	private void OnLoginSuccess(LoginResult result)
	{
		Debug.Log("Logged in");
	}

	private void OnLoginFailure(PlayFabError error)
	{
		Debug.Log("Not logged in - " + error.GenerateErrorReport());
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
