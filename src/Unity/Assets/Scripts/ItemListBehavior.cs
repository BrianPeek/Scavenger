using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemListBehavior : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
		string[] x = new string[] { "text1", "text2", "text3", "text1", "text2", "text3" };

		GameObject prefab = Resources.Load("Prefabs/ItemRow") as GameObject;
		var container = GameObject.Find("ItemList");

		//foreach(var li in result.Leaderboard)
		for(int i = 0; i < x.Length; i++)
		{
			GameObject item = Instantiate(prefab);

			var itemName = item.transform.Find("ItemName").gameObject.GetComponent<Text>();

			itemName.text = x[i];

			item.transform.SetParent(container.transform, false);
			item.SetActive(true);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
