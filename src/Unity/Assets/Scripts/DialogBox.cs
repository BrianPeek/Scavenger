using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogBox : MonoBehaviour, IPointerClickHandler
{
	private static GameObject dialog;

	public static GameObject Show(string msg)
	{
		GameObject prefab = Resources.Load<GameObject>("Prefabs/DialogBox");
		dialog = Instantiate(prefab);

		Canvas canvas = FindObjectOfType<Canvas>();
		dialog.transform.SetParent(canvas.transform, false);

		dialog.GetComponentInChildren<Text>().text = msg;

		Animation a = dialog.GetComponent<Animation>();
		a.Play();

		return dialog;
	}

	public void Dismiss()
	{
		Animation a = GetComponent<Animation>();
		a.clip = a.GetClip("DialogExit");
		a.Play();
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		Dismiss();
	}
}
