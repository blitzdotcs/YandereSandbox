using System;
using UnityEngine;

[Serializable]
public class EasterEggScript : MonoBehaviour
{
	public Renderer YandereRenderer;

	public Texture DarkSeifuku;

	public virtual void OnTriggerEnter()
	{
		YandereRenderer.materials[0].mainTexture = DarkSeifuku;
	}

	public virtual void Main()
	{
	}
}
