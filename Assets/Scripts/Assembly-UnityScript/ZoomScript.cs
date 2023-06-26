using System;
using UnityEngine;

[Serializable]
public class ZoomScript : MonoBehaviour
{
	public RPG_Camera CameraScript;

	public YandereScript Yandere;

	public float TargetZoom;

	public float Zoom;

	public float ShakeStrength;

	public float Timer;

	public Vector3 Target;

	public virtual void Update()
	{
		float y = 1f + Zoom;
		Vector3 localPosition = transform.localPosition;
		float num = (localPosition.y = y);
		Vector3 vector2 = (transform.localPosition = localPosition);
		TargetZoom += Input.GetAxis("Mouse ScrollWheel");
		if (!(TargetZoom >= 0f))
		{
			TargetZoom = 0f;
		}
		else if (!(TargetZoom <= 0.4f))
		{
			TargetZoom = 0.4f;
		}
		Zoom = Mathf.Lerp(Zoom, TargetZoom, Time.deltaTime);
		CameraScript.distance = 2f - Zoom * 3.33333f;
		CameraScript.distanceMax = 2f - Zoom * 3.33333f;
		CameraScript.distanceMin = 2f - Zoom * 3.33333f;
		if (!Yandere.TimeSkipping)
		{
			Timer += Time.deltaTime;
			ShakeStrength = Mathf.Lerp(ShakeStrength, 1f - Yandere.Sanity * 0.01f, Time.deltaTime);
			if (!(Timer <= 0.1f + Yandere.Sanity * 0.01f))
			{
				Target.x = UnityEngine.Random.Range(-1f * ShakeStrength, 1f * ShakeStrength);
				Target.y = transform.localPosition.y;
				Target.z = UnityEngine.Random.Range(-1f * ShakeStrength, 1f * ShakeStrength);
				Timer = 0f;
			}
		}
		else
		{
			Target = new Vector3(0f, transform.localPosition.y, 0f);
		}
		transform.localPosition = Vector3.MoveTowards(transform.localPosition, Target, Time.deltaTime * ShakeStrength * 0.1f);
	}

	public virtual void Main()
	{
	}
}
