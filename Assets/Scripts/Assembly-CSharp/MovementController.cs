using UnityEngine;

public class MovementController : MonoBehaviour
{
	public bool moveX;

	public bool moveY;

	public bool moveZ;

	public float speed = 1.2f;

	public Vector3 amplitude = Vector3.one;

	private Transform tr;

	private float counter;

	private Vector3 initialOffsets;

	private void Awake()
	{
		tr = GetComponent<Transform>();
		initialOffsets = tr.position;
		counter = 0f;
	}

	private void Update()
	{
		counter += Time.deltaTime * speed;
		Vector3 position = new Vector3((!moveX) ? initialOffsets.x : (initialOffsets.x + amplitude.x * Mathf.Sin(counter)), (!moveY) ? initialOffsets.y : (initialOffsets.y + amplitude.y * Mathf.Sin(counter)), (!moveZ) ? initialOffsets.z : (initialOffsets.z + amplitude.z * Mathf.Sin(counter)));
		tr.position = position;
	}
}
