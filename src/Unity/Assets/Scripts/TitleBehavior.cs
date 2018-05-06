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
		// use transform.Find for objects that are not active
		enterNameDialog = GameObject.Find("Canvas").transform.Find("EnterNameDialog").gameObject;

		if(string.IsNullOrEmpty(Globals.SessionTicket))
		{
			PlayFabAuth.Login(
				result =>
				{
					Debug.Log($"User '{Globals.DisplayName}' logged in with ticket {Globals.SessionTicket}");

					// user has no DisplayName, allow them to create one
					if(string.IsNullOrEmpty(Globals.DisplayName))
					{
						Animation a = enterNameDialog.GetComponent<Animation>();
						a.Play();
					}

					UpdateUI(true);
				},
				error =>
				{
					Debug.Log("Failed logging in: " + error.GenerateErrorReport());
					DialogBox.Show(error.GenerateErrorReport());
				});
		}
		else
		{
			UpdateUI(true);
		}
	}

	private void UpdateUI(bool loggedIn)
	{
		GameObject.Find("StartButton").GetComponent<Button>().interactable = loggedIn;
		GameObject.Find("LeaderboardButton").GetComponent<Button>().interactable = loggedIn;

		if(loggedIn)
			GameObject.Find("DisplayName").gameObject.GetComponent<Text>().text = Globals.DisplayName;
	}

	public void EnterNameOkOnClick()
	{
		string text = GameObject.Find("NameText").GetComponent<Text>().text;

		if(!string.IsNullOrEmpty(text))
		{
			UpdateUserTitleDisplayNameRequest userReq = new UpdateUserTitleDisplayNameRequest
			{
				DisplayName = text
			};

			// save user-entered name to their PlayFab profile
			PlayFabClientAPI.UpdateUserTitleDisplayName(userReq,
				result => 
				{
					Debug.Log($"User '{result.DisplayName}' saved");
					Globals.DisplayName = result.DisplayName;
					UpdateUI(true);

					Animation a = enterNameDialog.GetComponent<Animation>();
					a.clip = a.GetClip("DialogExit");
					a.Play();
				},
				error => 
				{
					Debug.Log("Failed updating user: " + error.GenerateErrorReport());
					DialogBox.Show(error.GenerateErrorReport());
				});
		}
	}

	public void StartGame()
	{
		SceneManager.LoadScene("ItemList");
	}

	public void Leaderboard()
	{
		SceneManager.LoadScene("Leaderboard");
	}
}
