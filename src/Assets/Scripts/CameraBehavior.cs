using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Microsoft.ProjectOxford.Vision.Contract;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CameraBehavior : MonoBehaviour
{
	public RawImage rawImage;

	private WebCamTexture webCamTexture;
	private int last;
	
	void Start() 
	{
		webCamTexture = new WebCamTexture();
		rawImage.texture = webCamTexture;
		rawImage.material.mainTexture = webCamTexture;
		webCamTexture.Play();
	}

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape))
			SceneManager.LoadScene("Title");
		
		if(last != webCamTexture.videoRotationAngle)
		{
			rawImage.transform.rotation = Quaternion.AngleAxis(-webCamTexture.videoRotationAngle, Vector3.forward);
			Debug.Log(webCamTexture.videoRotationAngle);
		}

		last = webCamTexture.videoRotationAngle;
	}

	public void StartSnapPhoto()
	{
		StartCoroutine(SnapPhoto(async tex =>
		{
			webCamTexture.Stop();
			rawImage.texture = tex;
			rawImage.material.mainTexture = tex;

			byte[] pngBuff = tex.EncodeToPNG();
			MemoryStream ms = new MemoryStream(pngBuff);
			AnalysisResult result = await Cognitive.CheckImage(ms);
			foreach(var t in result.Tags)
				Debug.Log($"{t.Confidence}: {t.Name}");
		}));
	}

	private IEnumerator SnapPhoto(Action<Texture2D> callback)
	{
		yield return new WaitForEndOfFrame();

		Texture2D photo = new Texture2D(webCamTexture.width, webCamTexture.height);
		photo.SetPixels(webCamTexture.GetPixels());
		photo.Apply();

		callback(photo);
	}
}
