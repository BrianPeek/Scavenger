using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundBehavior : MonoBehaviour
{
	public Sprite BackgroundImage;
	public string Text;
	
	// Use this for initialization
	void Awake()
	{
		var t = GetComponentInChildren<Text>();
		t.text = Text;

		var i = GetComponentInChildren<Image>();
		i.sprite = BackgroundImage;
	}
}
