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
		enterNameDialog = GameObject.Find("Canvas").transform.Find("EnterNameDialog").gameObject;

		if(string.IsNullOrEmpty(Globals.SessionTicket))
		{
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
						Globals.SessionTicket = result?.SessionTicket;
						Globals.DisplayName = result?.InfoResultPayload?.AccountInfo?.TitleInfo?.DisplayName;

						UpdateUI(true);

						Debug.Log($"User '{Globals.DisplayName}' logged in with session {Globals.SessionTicket}");

						if(string.IsNullOrEmpty(Globals.DisplayName))
						{
							Animation a = enterNameDialog.GetComponent<Animation>();
							a.Play();
						}
					},

				error => Debug.Log("Not logged in: " + error.GenerateErrorReport())
			);
		}
		else
		{
			UpdateUI(true);
		}
#endif
	}

	private void UpdateUI(bool loggedIn)
	{
		Button startButton = GameObject.Find("StartButton").GetComponent<Button>();
		Button leaderboardButton = GameObject.Find("LeaderboardButton").GetComponent<Button>();
		Text displayNameText = GameObject.Find("DisplayName").gameObject.GetComponent<Text>();

		startButton.interactable = loggedIn;
		leaderboardButton.interactable = loggedIn;

		if(loggedIn)
			displayNameText.text = Globals.DisplayName;
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
		SceneManager.LoadScene("ItemList");
	}

	public void Leaderboard()
	{
		SceneManager.LoadScene("Leaderboard");
	}
}
