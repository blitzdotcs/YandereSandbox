using System;
using UnityEngine;

[Serializable]
public class DetectionMarkerScript : MonoBehaviour
{
	public Transform Target;

	public UITexture Tex;

	public virtual void Start()
	{
		transform.LookAt(new Vector3(Target.position.x, transform.position.y, Target.position.z));
		Tex.transform.localScale = new Vector3(1f, 0f, 1f);
		transform.localScale = new Vector3(1f, 1f, 1f);
		int num = 0;
		Color color = Tex.color;
		float num2 = (color.a = num);
		Color color3 = (Tex.color = color);
	}

	public virtual void Update()
	{
		transform.LookAt(new Vector3(Target.position.x, transform.position.y, Target.position.z));
	}

	public virtual void Main()
	{
	}
}
