using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ItemClickHandler : MonoBehaviour, IPointerClickHandler
 {
	 public void OnPointerClick(PointerEventData eventData)
	 {
		ListItem li = GetComponent<ListItem>();
		Debug.Log($"Clicked: {li.item.Name}");

		// don't allow a user to find an item more than once
		if(li.item.Found)
			return;

		Globals.CurrentItem = li.item;

		SceneManager.LoadScene("Camera");
	 }
 }
