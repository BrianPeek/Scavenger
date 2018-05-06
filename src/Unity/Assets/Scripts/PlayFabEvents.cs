using System;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public enum PlayFabEventType
{
	ItemFound,
	ItemNotFound,
}

public class PlayFabEvents
{
	public static void WriteEvent(PlayFabEventType type)
	{
		WriteClientPlayerEventRequest req = new WriteClientPlayerEventRequest
		{
			EventName = EventTypeToString(type),
			Body = new Dictionary<string, object>
			{
				{ "item", Globals.CurrentItem }
			}
		};

		PlayFabClientAPI.WritePlayerEvent(req, 
			result => {},
			error => Debug.Log("Failed writing event: " + error.ErrorMessage)
		);
	}

	private static string EventTypeToString(PlayFabEventType type)
	{
		switch(type)
		{
			case PlayFabEventType.ItemFound:
				return "player_found_item";
			case PlayFabEventType.ItemNotFound:
				return "player_notfound_item";
			default:
				throw new ArgumentOutOfRangeException(nameof(type), type, null);
		}
	}
}

