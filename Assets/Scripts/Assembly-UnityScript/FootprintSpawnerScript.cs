using System;
using UnityEngine;

[Serializable]
public class FootprintSpawnerScript : MonoBehaviour
{
	public GameObject BloodyFootprint;

	public Transform BloodParent;

	public Transform Yandere;

	public bool LeftFoot;

	public bool FootUp;

	public int Bloodiness;

	public virtual void Update()
	{
		if (!FootUp)
		{
			if (!(transform.position.y <= 0.1f))
			{
				FootUp = true;
			}
		}
		else
		{
			if (transform.position.y >= 0.01f)
			{
				return;
			}
			FootUp = false;
			if (Bloodiness > 0)
			{
				GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(BloodyFootprint, new Vector3(transform.position.x, Yandere.position.y + 0.012f, transform.position.z), Quaternion.identity);
				float y = transform.eulerAngles.y;
				Vector3 eulerAngles = gameObject.transform.eulerAngles;
				float num = (eulerAngles.y = y);
				Vector3 vector2 = (gameObject.transform.eulerAngles = eulerAngles);
				gameObject.transform.parent = BloodParent;
				if (LeftFoot)
				{
					int num2 = -1;
					Vector3 localScale = gameObject.transform.localScale;
					float num3 = (localScale.x = num2);
					Vector3 vector4 = (gameObject.transform.localScale = localScale);
				}
				Bloodiness--;
			}
		}
	}

	public virtual void Main()
	{
	}
}
