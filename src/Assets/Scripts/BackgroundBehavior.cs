using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BackgroundBehavior : MonoBehaviour
{
	public Sprite BackgroundImage;
	public string Text;
	public string PreviousScene;

	// Use this for initialization
	void Awake()
	{
		var t = GetComponentInChildren<Text>();
		t.text = Text;

		var i = GetComponentInChildren<Image>();
		i.sprite = BackgroundImage;
	}

	public void BackButtonOnClick()
	{
		SceneManager.LoadScene(PreviousScene);
	}
}
