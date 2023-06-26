using System;
using UnityEngine;

[Serializable]
public class BloodPoolScript : MonoBehaviour
{
	public bool Grow;

	public virtual void Start()
	{
		transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
	}

	public virtual void Update()
	{
		if (Grow)
		{
			transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1f, 1f, 1f), Time.deltaTime * 1f);
		}
	}

	public virtual void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.name == "BloodSpawner")
		{
			Grow = true;
		}
	}

	public virtual void OnTriggerExit(Collider other)
	{
		if (!(other.gameObject.name == "BloodSpawner"))
		{
		}
	}

	public virtual void Main()
	{
	}
}
