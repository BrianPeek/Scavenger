using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraBehavior : MonoBehaviour
{

	WebCamTexture webCamTexture;
	public RawImage rawImage;
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
		
		if(last != webCamTexture.videoRotationAngle)
		{
			rawImage.transform.rotation = Quaternion.AngleAxis(-webCamTexture.videoRotationAngle, Vector3.forward);
			Debug.Log(webCamTexture.videoRotationAngle);
		}

		last = webCamTexture.videoRotationAngle;
	}

	public void StartSnapPhoto()
	{
		StartCoroutine(SnapPhoto());
	}

	private IEnumerator SnapPhoto()
	{
		yield return new WaitForEndOfFrame();

		Texture2D photo = new Texture2D(webCamTexture.width, webCamTexture.height);
		photo.SetPixels(webCamTexture.GetPixels());
		photo.Apply();

		byte[] bytes = photo.EncodeToPNG();
	}
}
