using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CameraBehavior : MonoBehaviour
{
	public DeviceCameraController controller;
	private TextMeshProUGUI textmesh;
	private Button cameraButton;
	private AudioSource audioSource;

	private AudioClip clipCamera, clipFound, clipNotFound;

	public void Start()
	{
		textmesh = GameObject.Find("CaptionText").GetComponent<TextMeshProUGUI>();
		textmesh.text = Globals.CurrentItem?.Name;

		cameraButton = GameObject.Find("CameraButton").GetComponent<Button>();
		audioSource = GameObject.Find("Sounds").GetComponent<AudioSource>();

		clipCamera = Resources.Load<AudioClip>("Sounds/camera");
		clipFound = Resources.Load<AudioClip>("Sounds/found");
		clipNotFound = Resources.Load<AudioClip>("Sounds/notfound");
	}

	public void StartSnapPhoto()
	{
		textmesh.text = "Verifying...";
		cameraButton.interactable = false;

		LoadingCircle.Show();

		StartCoroutine(controller.SnapPhoto(async tex =>
		{
			try
			{
				audioSource.PlayOneShot(clipCamera);

				// encode the image from the camera as a PNG to send to the Computer Vision API
				byte[] pngBuff = tex.EncodeToPNG();
				MemoryStream ms = new MemoryStream(pngBuff);

				// call the vision service and get the image analysis
				ComputerVisionClient client = new ComputerVisionClient(new ApiKeyServiceClientCredentials(Globals.VisionKey), new DelegatingHandler[] { });
				client.Endpoint = Globals.VisionEndpoint;
				ImageDescription result = await client.DescribeImageInStreamAsync(ms);

				// send the tag list to the debug log
				string tags = result.Tags.Aggregate((x, y) => $"{x}, {y}");
				Debug.Log(tags);

				foreach(string itemTag in Globals.CurrentItem.Tags)
				{
					if(result.Tags.Contains(itemTag.ToLower()))
					{
						audioSource.PlayOneShot(clipFound);
						textmesh.text = "You found it!";

						PlayFabEvents.WriteEvent(PlayFabEventType.ItemFound);

						// if the image matches, call the ItemFound function to record it
						string s = JsonConvert.SerializeObject(Globals.CurrentItem);
						await Globals.HttpClient.PostAsync("ItemFound", new StringContent(s, Encoding.UTF8, "application/json"));
						LoadingCircle.Dismiss();
						SceneManager.LoadScene("ItemList");
						return;
					}
				}

				audioSource.PlayOneShot(clipNotFound);
				textmesh.text = "Not a match, please try again.";

				PlayFabEvents.WriteEvent(PlayFabEventType.ItemNotFound);

				controller.StartStream();
				cameraButton.interactable = true;
				LoadingCircle.Dismiss();
			}
			catch(Exception e)
			{
				LoadingCircle.Dismiss();
				Debug.Log(e);
				DialogBox.Show(e.Message);
			}
		}));
	}

	public void BackButtonOnClick()
	{
		SceneManager.LoadScene("ItemList");
	}
}
