////
// Adapted from https://www.salusgames.com/2017/01/08/circle-loading-animation-in-unity3d/
////

using UnityEngine;

public class LoadingCircle : MonoBehaviour
{
	private RectTransform rectComponent;
	private float rotateSpeed = 200f;
	private static GameObject circle;

	private void Start()
	{
		rectComponent = GetComponent<RectTransform>();
	}

	private void Update()
	{
		rectComponent.Rotate(0f, 0f, rotateSpeed * Time.deltaTime);
	}

	public static GameObject Show()
	{
		GameObject prefab = Resources.Load<GameObject>("Prefabs/Loading");
		circle = Instantiate(prefab);

		Canvas canvas = FindObjectOfType<Canvas>();
		circle.transform.SetParent(canvas.transform, false);

		return circle;
	}

	public static void Dismiss()
	{
		Destroy(circle);
	}
}