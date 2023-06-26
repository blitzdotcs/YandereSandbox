using System;
using UnityEngine;

[Serializable]
public class SplashSpawnerScript : MonoBehaviour
{
	public GameObject BloodSplash;

	public Transform YandereChan;

	public bool Planted;

	public bool Bloody;

	public virtual void Update()
	{
		if (!(transform.position.y >= 0.1f))
		{
			if (!Planted)
			{
				Planted = true;
				if (Bloody)
				{
					GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(BloodSplash, new Vector3(transform.position.x, YandereChan.position.y, transform.position.z), Quaternion.identity);
					int num = -90;
					Vector3 eulerAngles = gameObject.transform.eulerAngles;
					float num2 = (eulerAngles.x = num);
					Vector3 vector2 = (gameObject.transform.eulerAngles = eulerAngles);
				}
			}
		}
		else
		{
			Planted = false;
		}
	}

	public virtual void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.name == "BloodPool(Clone)")
		{
			Bloody = true;
		}
	}

	public virtual void OnTriggerExit(Collider other)
	{
		if (other.gameObject.name == "BloodPool(Clone)")
		{
			Bloody = false;
		}
	}

	public virtual void Main()
	{
	}
}
