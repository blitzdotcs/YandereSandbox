using System;
using UnityEngine;

[Serializable]
public class ShoulderCameraScript : MonoBehaviour
{
	public PauseScreenScript PauseScreen;

	public YandereScript Yandere;

	public RPG_Camera RPGCamera;

	public Transform ShoulderFocus;

	public Transform ShoulderPOV;

	public Vector3 LastPosition;

	public bool OverShoulder;

	public float Height;

	public float Timer;

	public virtual void Update()
	{
		if (PauseScreen.Show)
		{
			return;
		}
		if (OverShoulder)
		{
			if (RPGCamera.enabled)
			{
				ShoulderFocus.position = RPGCamera.cameraPivot.position;
				LastPosition = transform.position;
				RPGCamera.enabled = false;
			}
			transform.position = Vector3.Lerp(transform.position, ShoulderPOV.position, Time.deltaTime * 10f);
			ShoulderFocus.position = Vector3.Lerp(ShoulderFocus.position, Yandere.TargetStudent.transform.position + Vector3.up * Height, Time.deltaTime * 10f);
			transform.LookAt(ShoulderFocus);
		}
		else if (!RPGCamera.enabled)
		{
			Timer += Time.deltaTime;
			transform.position = Vector3.Lerp(transform.position, LastPosition, Time.deltaTime * 10f);
			ShoulderFocus.position = Vector3.Lerp(ShoulderFocus.position, RPGCamera.cameraPivot.position, Time.deltaTime * 10f);
			transform.LookAt(ShoulderFocus);
			if (!(Timer <= 0.5f))
			{
				RPGCamera.enabled = true;
				Yandere.Talking = false;
				Yandere.CanMove = true;
				Timer = 0f;
			}
		}
	}

	public virtual void Main()
	{
	}
}
