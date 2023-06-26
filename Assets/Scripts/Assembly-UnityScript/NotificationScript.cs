using System;
using UnityEngine;

[Serializable]
public class NotificationScript : MonoBehaviour
{
	public NotificationManagerScript NotificationManager;

	public UIPanel Panel;

	public UILabel Label;

	public bool Display;

	public float Timer;

	public int ID;

	public virtual void Update()
	{
		if (!Display)
		{
			if (NotificationManager.NotificationsSpawned > ID + 2)
			{
				Panel.alpha -= Time.deltaTime * 3f;
			}
			else
			{
				Panel.alpha -= Time.deltaTime;
			}
			if (!(Panel.alpha > 0f))
			{
				UnityEngine.Object.Destroy(gameObject);
			}
			return;
		}
		Timer += Time.deltaTime;
		if (!(Timer <= 4f))
		{
			Display = false;
		}
		if (NotificationManager.NotificationsSpawned > ID + 2)
		{
			Display = false;
		}
	}

	public virtual void Main()
	{
	}
}
