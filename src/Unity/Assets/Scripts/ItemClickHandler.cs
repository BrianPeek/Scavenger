using System.Collections;
using System.Collections.Generic;
using ScavengerServer;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemClickHandler : MonoBehaviour, IPointerClickHandler
 {
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	 public void OnPointerClick(PointerEventData eventData)
	 {
		ListItem li = GetComponent<ListItem>();
		Debug.Log($"Clicked: {li.item.Id}");
	 }
 }
