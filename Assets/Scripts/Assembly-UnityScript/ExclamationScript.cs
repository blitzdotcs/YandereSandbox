using System;
using UnityEngine;

[Serializable]
public class ExclamationScript : MonoBehaviour
{
	public Renderer Graphic;

	public float Alpha;

	public float Timer;

	public virtual void Start()
	{
		transform.localScale = new Vector3(0f, 0f, 0f);
		Graphic.material.SetColor("_TintColor", new Color(0.5f, 0.5f, 0.5f, 0f));
	}

	public virtual void Update()
	{
		Timer -= Time.deltaTime;
		if (!(Timer <= 0f))
		{
			transform.LookAt(Camera.main.transform);
			if (!(Timer <= 1.5f))
			{
				transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1f, 1f, 1f), Time.deltaTime * 10f);
				Alpha = Mathf.Lerp(Alpha, 0.5f, Time.deltaTime * 10f);
				Graphic.material.SetColor("_TintColor", new Color(0.5f, 0.5f, 0.5f, Alpha));
			}
			else
			{
				transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(0f, 0f, 0f), Time.deltaTime * 10f);
				Alpha = Mathf.Lerp(Alpha, 0f, Time.deltaTime * 10f);
				Graphic.material.SetColor("_TintColor", new Color(0.5f, 0.5f, 0.5f, Alpha));
			}
		}
	}

	public virtual void Main()
	{
	}
}
