using UnityEngine;

public class CameraFlyController : MonoBehaviour
{
	private float speed = 4f;

	private Transform tr;

	private Vector3 mpStart;

	private Vector3 originalRotation;

	private float t;

	private void Awake()
	{
		tr = GetComponent<Transform>();
		t = Time.realtimeSinceStartup;
	}

	private void Update()
	{
		float num = 0f;
		if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
		{
			num += 1f;
		}
		if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
		{
			num -= 1f;
		}
		float num2 = 0f;
		if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
		{
			num2 += 1f;
		}
		if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
		{
			num2 -= 1f;
		}
		float num3 = 0f;
		if (Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.Space))
		{
			num3 += 1f;
		}
		if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.C))
		{
			num3 -= 1f;
		}
		float num4 = Time.realtimeSinceStartup - t;
		t = Time.realtimeSinceStartup;
		tr.position += tr.TransformDirection(new Vector3(num2, num3, num) * speed * ((!Input.GetKey(KeyCode.LeftShift)) ? 1f : 2f) * num4);
		Vector3 mousePosition = Input.mousePosition;
		if (Input.GetMouseButtonDown(1))
		{
			originalRotation = tr.localEulerAngles;
			mpStart = mousePosition;
		}
		if (Input.GetMouseButton(1))
		{
			Vector2 vector = new Vector2((mousePosition.x - mpStart.x) / (float)Screen.width, (mpStart.y - mousePosition.y) / (float)Screen.height);
			tr.localEulerAngles = originalRotation + new Vector3(vector.y * 360f, vector.x * 360f, 0f);
		}
	}
}
