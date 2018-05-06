///////////
// Adapted from post at https://answers.unity.com/questions/773464/webcamtexture-correct-resolution-and-ratio.html#answer-1148424
///////////

using System;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;

public class DeviceCameraController : MonoBehaviour
{
	public RawImage image;
	public RectTransform imageParent;
	public AspectRatioFitter imageFitter;

	// Device cameras
	private WebCamDevice frontCameraDevice;
	private WebCamDevice backCameraDevice;
	private WebCamDevice activeCameraDevice;
	 
	private WebCamTexture frontCameraTexture;
	private WebCamTexture backCameraTexture;
	private WebCamTexture activeCameraTexture;
	 
	// Image rotation
	private Vector3 rotationVector = new Vector3(0f, 0f, 0f);
	 
	// Image uvRect
	private Rect defaultRect = new Rect(0f, 0f, 1f, 1f);
	private Rect fixedRect = new Rect(0f, 1f, 1f, -1f);
	 
	// Image Parent's scale
	private Vector3 defaultScale = new Vector3(1f, 1f, 1f);
	private Vector3 fixedScale = new Vector3(-1f, 1f, 1f);
	 
	void Start()
	{
		// Check for device cameras
		if (WebCamTexture.devices.Length == 0)
		{
			Debug.Log("No devices cameras found");
			return;
		}
	 
		// Get the device's cameras and create WebCamTextures with them
		frontCameraDevice = WebCamTexture.devices.Last();
		backCameraDevice = WebCamTexture.devices.First();
	 
		frontCameraTexture = new WebCamTexture(frontCameraDevice.name);
		backCameraTexture = new WebCamTexture(backCameraDevice.name);
	 
		// Set camera filter modes for a smoother looking image
		frontCameraTexture.filterMode = FilterMode.Trilinear;
		backCameraTexture.filterMode = FilterMode.Trilinear;
	 
		// Set the camera to use by default
		SetActiveCamera(backCameraTexture);
	}

	private void OnDisable()
	{
		activeCameraTexture.Stop();
	}

	// Set the device camera to use and start it
	public void SetActiveCamera(WebCamTexture cameraToUse)
	{
		if (activeCameraTexture != null)
			activeCameraTexture.Stop();
				 
		activeCameraTexture = cameraToUse;
		activeCameraDevice = WebCamTexture.devices.FirstOrDefault(device => device.name == cameraToUse.deviceName);
	 
		image.texture = activeCameraTexture;
		image.material.mainTexture = activeCameraTexture;
	 
		activeCameraTexture.Play();
	}
	 
	// Switch between the device's front and back camera
	public void SwitchCamera()
	{
		SetActiveCamera(activeCameraTexture.Equals(frontCameraTexture) ? backCameraTexture : frontCameraTexture);
	}
			 
	// Make adjustments to image every frame to be safe, since Unity isn't 
	// guaranteed to report correct data as soon as device camera is started
	void Update()
	{
		// Skip making adjustment for incorrect camera data
		if (activeCameraTexture.width < 100)
		{
			Debug.Log("Still waiting another frame for correct info...");
			return;
		}
	 
		// Rotate image to show correct orientation 
		rotationVector.z = -activeCameraTexture.videoRotationAngle;
		image.rectTransform.localEulerAngles = rotationVector;
	 
		// Set AspectRatioFitter's ratio
		float videoRatio = (float)activeCameraTexture.width / (float)activeCameraTexture.height;
		imageFitter.aspectRatio = videoRatio;
	 
		// Unflip if vertically flipped
		image.uvRect = activeCameraTexture.videoVerticallyMirrored ? fixedRect : defaultRect;
	 
		// Mirror front-facing camera's image horizontally to look more natural
		imageParent.localScale = activeCameraDevice.isFrontFacing ? fixedScale : defaultScale;
	}

	public IEnumerator SnapPhoto(Action<Texture2D> callback)
	{
		yield return new WaitForEndOfFrame();

		Texture2D photo = new Texture2D(activeCameraTexture.width, activeCameraTexture.height);
		photo.SetPixels(activeCameraTexture.GetPixels());
		photo.Apply();

		activeCameraTexture.Stop();

		image.texture = photo;
		image.material.mainTexture = photo;

		callback(photo);
	}

	public void StartStream()
	{
		image.texture = activeCameraTexture;
		image.material.mainTexture = activeCameraTexture;

		activeCameraTexture.Play();
	}
}
