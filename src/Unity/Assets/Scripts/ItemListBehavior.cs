using System;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

public class ItemListBehavior : MonoBehaviour
{
	// Use this for initialization
	async void Start ()
	{
		LoadingCircle.Show();

		GameObject prefab = Resources.Load("Prefabs/ItemRow") as GameObject;
		GameObject container = GameObject.Find("ItemList");

		try
		{
			string result = await Globals.HttpClient.GetStringAsync("GetDailyItems");

			DailyItems items = JsonConvert.DeserializeObject<DailyItems>(result);

			GameObject.Find("DateText").GetComponent<Text>().text = DateTime.Parse(items.Date).ToShortDateString();

			foreach(Item i in items.Items)
			{
				GameObject item = Instantiate(prefab);
				ListItem objItem = item.GetComponent<ListItem>();
				objItem.item = i;

				item.transform.Find("ItemName").gameObject.GetComponent<Text>().text = i.Name;
				item.transform.Find("Checkmark").gameObject.SetActive(i.Found);

				item.transform.SetParent(container.transform, false);
				item.SetActive(true);
			}
		}
		catch(Exception e)
		{
			Debug.Log(e);
			DialogBox.Show(e.Message);
		}
		finally
		{
			LoadingCircle.Dismiss();
		}
	}
}
