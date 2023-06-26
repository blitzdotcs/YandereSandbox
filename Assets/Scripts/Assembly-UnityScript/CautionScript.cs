using System;
using UnityEngine;

[Serializable]
public class CautionScript : MonoBehaviour
{
	public YandereScript Yandere;

	public UISprite Sprite;

	public virtual void Start()
	{
		int num = 0;
		Color color = Sprite.color;
		float num2 = (color.a = num);
		Color color3 = (Sprite.color = color);
	}

	public virtual void Update()
	{
		if (Yandere.Armed || Yandere.Bloodiness > 0f || Yandere.Sanity < 33.33333f || Yandere.NearBodies > 0)
		{
			float a = Sprite.color.a + Time.deltaTime;
			Color color = Sprite.color;
			float num = (color.a = a);
			Color color3 = (Sprite.color = color);
			if (!(Sprite.color.a <= 1f))
			{
				int num2 = 1;
				Color color4 = Sprite.color;
				float num3 = (color4.a = num2);
				Color color6 = (Sprite.color = color4);
			}
		}
		else
		{
			float a2 = Sprite.color.a - Time.deltaTime;
			Color color7 = Sprite.color;
			float num4 = (color7.a = a2);
			Color color9 = (Sprite.color = color7);
			if (!(Sprite.color.a >= 0f))
			{
				int num5 = 0;
				Color color10 = Sprite.color;
				float num6 = (color10.a = num5);
				Color color12 = (Sprite.color = color10);
			}
		}
	}

	public virtual void Main()
	{
	}
}
