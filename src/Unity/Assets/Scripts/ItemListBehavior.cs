using System;
using System.Net.Http;
using Newtonsoft.Json;
using ScavengerServer;
using UnityEngine;
using UnityEngine.UI;

public class ItemListBehavior : MonoBehaviour {

	// Use this for initialization
	async void Start ()
	{
		DailyItems items;

		using(var http = new HttpClient())
		{
			//http.BaseAddress = new Uri("https://scavengersrv.azurewebsites.net/api/");
			http.BaseAddress = new Uri("http://localhost:7071/api/");
			string result = await http.GetStringAsync($"GetDailyItems/{Globals.SessionTicket}");
			DailyItems[] di = JsonConvert.DeserializeObject<DailyItems[]>(result);
			items = di[0];
		}

		Text dateText = GameObject.Find("DateText").GetComponent<Text>();
		dateText.text = DateTime.Parse(items.Date).ToShortDateString();

		GameObject prefab = Resources.Load("Prefabs/ItemRow") as GameObject;
		var container = GameObject.Find("ItemList");

		foreach(Item i in items.Items)
		{
			GameObject item = Instantiate(prefab);

			var itemName = item.transform.Find("ItemName").gameObject.GetComponent<Text>();

			itemName.text = i.Name;

			item.transform.SetParent(container.transform, false);
			item.SetActive(true);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
