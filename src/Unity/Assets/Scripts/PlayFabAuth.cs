using System;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class PlayFabAuth
{
	public static void Login(Action<LoginResult> successCallback, Action<PlayFabError> errorCallback)
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
				SuccessHandler(result);
				successCallback(result);
			},

			error =>
			{
				ErrorHandler(error);
				errorCallback(error);
			}
		);

#elif UNITY_IOS
		LoginWithIOSDeviceIDRequest req = new LoginWithIOSDeviceIDRequest
		{
				DeviceModel = SystemInfo.deviceModel, 
				OS = SystemInfo.operatingSystem,
				DeviceId = SystemInfo.deviceUniqueIdentifier,
				CreateAccount = true,
				InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
				{
					GetUserAccountInfo = true
				}
		};

		PlayFabClientAPI.LoginWithIOSDeviceID(req,
			result => 
			{
				SuccessHandler(result);
				successCallback(result);
			},

			error =>
			{
				ErrorHandler(error);
				errorCallback(error);
			}
		);
#else
		LoginWithCustomIDRequest req = new LoginWithCustomIDRequest
		{
			TitleId = PlayFabSettings.TitleId,
			CustomId = SystemInfo.deviceUniqueIdentifier,
			CreateAccount = true,
			InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
			{
				GetUserAccountInfo = true
			}
		};

		PlayFabClientAPI.LoginWithCustomID(req,
			result => 
			{
				SuccessHandler(result);
				successCallback(result);
			},

			error =>
			{
				ErrorHandler(error);
				errorCallback(error);
			}
		);
#endif
	}

	private static void SuccessHandler(LoginResult result)
	{
		Globals.SessionTicket = result?.SessionTicket;
		Globals.DisplayName = result?.InfoResultPayload?.AccountInfo?.TitleInfo?.DisplayName;
	}

	private static void ErrorHandler(PlayFabError error)
	{
		Debug.Log("Not logged in: " + error.GenerateErrorReport());
	}
}
