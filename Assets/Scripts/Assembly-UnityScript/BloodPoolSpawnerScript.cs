using System;
using UnityEngine;

[Serializable]
public class BloodPoolSpawnerScript : MonoBehaviour
{
	public Transform BloodParent;

	public GameObject BloodPool;

	public int NearbyBlood;

	public virtual void Start()
	{
		BloodParent = GameObject.Find("Blood").transform;
	}

	public virtual void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.name == "BloodPool(Clone)")
		{
			NearbyBlood++;
		}
	}

	public virtual void OnTriggerExit(Collider other)
	{
		if (other.gameObject.name == "BloodPool(Clone)")
		{
			NearbyBlood--;
		}
	}

	public virtual void Update()
	{
		transform.localPosition = new Vector3(0f, 0f, 0f);
		if (!(transform.position.y >= 0.33333f) && NearbyBlood < 1)
		{
			GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(BloodPool, new Vector3(transform.position.x, 0.012f, transform.position.z), Quaternion.identity);
			gameObject.transform.localEulerAngles = new Vector3(90f, UnityEngine.Random.Range(0f, 360f), 0f);
			gameObject.transform.parent = BloodParent;
		}
	}

	public virtual void Main()
	{
	}
}
