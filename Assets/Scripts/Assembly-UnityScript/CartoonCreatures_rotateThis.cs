using System;
using UnityEngine;

[Serializable]
public class CartoonCreatures_rotateThis : MonoBehaviour
{
	public float rotationSpeedX;

	public float rotationSpeedY;

	public float rotationSpeedZ;

	public CartoonCreatures_rotateThis()
	{
		rotationSpeedX = 90f;
	}

	public virtual void Start()
	{
	}

	public virtual void Update()
	{
		transform.Rotate(new Vector3(rotationSpeedX, rotationSpeedY, rotationSpeedZ) * Time.deltaTime);
	}

	public virtual void Main()
	{
	}
}
