using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Microsoft.ProjectOxford.Vision.Contract;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraBehavior : MonoBehaviour
{
	public DeviceCameraController controller;

	public void StartSnapPhoto()
	{
		StartCoroutine(controller.SnapPhoto(async tex =>
		{
			byte[] pngBuff = tex.EncodeToPNG();
			MemoryStream ms = new MemoryStream(pngBuff);
			AnalysisResult result = await Cognitive.CheckImage(ms);
			Debug.Log("**********");
			foreach(string t in result.Description.Tags)
				Debug.Log($"{t}");
		}));
	}

	public void BackButtonOnClick()
	{
		SceneManager.LoadScene("Title");
	}
}
