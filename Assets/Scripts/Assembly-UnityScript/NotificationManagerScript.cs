using System;
using UnityEngine;

[Serializable]
public class NotificationManagerScript : MonoBehaviour
{
	public Transform NotificationSpawnPoint;

	public Transform NotificationParent;

	public GameObject Notification;

	public int NotificationsSpawned;

	public virtual void Update()
	{
		float y = Mathf.Lerp(NotificationParent.localPosition.y, -0.049f * (float)NotificationsSpawned, Time.deltaTime * 10f);
		Vector3 localPosition = NotificationParent.localPosition;
		float num = (localPosition.y = y);
		Vector3 vector2 = (NotificationParent.localPosition = localPosition);
	}

	public virtual void DisplayNotification(string Type)
	{
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(Notification);
		NotificationScript notificationScript = (NotificationScript)gameObject.GetComponent(typeof(NotificationScript));
		gameObject.transform.parent = NotificationParent;
		gameObject.transform.localPosition = new Vector3(0f, 0.60275f + 0.049f * (float)NotificationsSpawned, 0f);
		gameObject.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
		notificationScript.NotificationManager = this;
		switch (Type)
		{
		case "Bloody":
			notificationScript.Label.text = "Visibly Bloody";
			break;
		case "Body":
			notificationScript.Label.text = "Near Body";
			break;
		case "Insane":
			notificationScript.Label.text = "Visibly Insane";
			break;
		case "Armed":
			notificationScript.Label.text = "Visibly Armed";
			break;
		}
		NotificationsSpawned++;
		notificationScript.ID = NotificationsSpawned;
	}

	public virtual void Main()
	{
	}
}
