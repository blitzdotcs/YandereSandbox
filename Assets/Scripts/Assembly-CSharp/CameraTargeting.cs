using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraTargeting : MonoBehaviour
{
	public LayerMask targetingLayerMask = -1;

	private float targetingRayLength = float.PositiveInfinity;

	private Camera cam;

	private string info = "Left Click - switch flashing for object under mouse cursor\nRight Click - switch see-through mode for object under mouse cursor\n'1' - fade in/out constant highlighting\n'2' - turn on/off constant highlighting immediately\n'3' - turn off all types of highlighting immediately\n";

	private void Awake()
	{
		cam = GetComponent<Camera>();
	}

	private void Update()
	{
		TargetingRaycast();
	}

	public void TargetingRaycast()
	{
		Transform transform = null;
		if (cam != null)
		{
			Ray ray = cam.ScreenPointToRay(Input.mousePosition);
			RaycastHit hitInfo;
			if (Physics.Raycast(ray, out hitInfo, targetingRayLength, targetingLayerMask.value))
			{
				transform = hitInfo.collider.transform;
			}
		}
		if (!(transform != null))
		{
			return;
		}
		HighlighterController componentInParent = transform.GetComponentInParent<HighlighterController>();
		if (componentInParent != null)
		{
			if (Input.GetButtonDown("Fire1"))
			{
				componentInParent.Fire1();
			}
			if (Input.GetButtonUp("Fire2"))
			{
				componentInParent.Fire2();
			}
			componentInParent.MouseOver();
		}
	}

	private void OnGUI()
	{
		GUI.Label(new Rect(10f, Screen.height - 100, 500f, 100f), info);
	}
}
