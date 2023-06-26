using System;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class HeartbrokenScript : MonoBehaviour
{
	public UILabel[] Letters;

	public UILabel[] Options;

	public Vector3[] Origins;

	public float Timer;

	public int ShakeID;

	public int ID;

	public virtual void Start()
	{
		while (ID < Extensions.get_length((System.Array)Letters))
		{
			Letters[ID].transform.localScale = new Vector3(10f, 10f, 1f);
			int num = 0;
			Color color = Letters[ID].color;
			float num2 = (color.a = num);
			Color color3 = (Letters[ID].color = color);
			Origins[ID] = Letters[ID].transform.localPosition;
			ID++;
		}
		for (ID = 0; ID < Extensions.get_length((System.Array)Options); ID++)
		{
			int num3 = 0;
			Color color4 = Options[ID].color;
			float num4 = (color4.a = num3);
			Color color6 = (Options[ID].color = color4);
		}
		ID = 0;
	}

	public virtual void Update()
	{
		Timer += Time.deltaTime;
		if (!(Timer <= 1f))
		{
			if (ID < Extensions.get_length((System.Array)Letters))
			{
				Letters[ID].transform.localScale = Vector3.MoveTowards(Letters[ID].transform.localScale, new Vector3(1f, 1f, 1f), Time.deltaTime * 100f);
				float a = Letters[ID].color.a + Time.deltaTime * 10f;
				Color color = Letters[ID].color;
				float num = (color.a = a);
				Color color3 = (Letters[ID].color = color);
				if (Letters[ID].transform.localScale == new Vector3(1f, 1f, 1f))
				{
					ID++;
				}
			}
			else if (ID - 11 < Extensions.get_length((System.Array)Options))
			{
				float a2 = Options[ID - 11].color.a + Time.deltaTime;
				Color color4 = Options[ID - 11].color;
				float num2 = (color4.a = a2);
				Color color6 = (Options[ID - 11].color = color4);
				if (!(Options[ID - 11].color.a < 1f))
				{
					ID++;
				}
			}
		}
		for (ShakeID = 0; ShakeID < Extensions.get_length((System.Array)Letters); ShakeID++)
		{
			float x = Origins[ShakeID].x + UnityEngine.Random.Range(-5f, 5f);
			Vector3 localPosition = Letters[ShakeID].transform.localPosition;
			float num3 = (localPosition.x = x);
			Vector3 vector2 = (Letters[ShakeID].transform.localPosition = localPosition);
			float y = Origins[ShakeID].y + UnityEngine.Random.Range(-5f, 5f);
			Vector3 localPosition2 = Letters[ShakeID].transform.localPosition;
			float num4 = (localPosition2.y = y);
			Vector3 vector4 = (Letters[ShakeID].transform.localPosition = localPosition2);
		}
	}

	public virtual void Main()
	{
	}
}
