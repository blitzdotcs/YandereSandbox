using System;
using UnityEngine;

[Serializable]
public class ParticleDeathScript : MonoBehaviour
{
	public ParticleSystem Particles;

	public virtual void LateUpdate()
	{
		if (Particles.particleCount == 0)
		{
			UnityEngine.Object.Destroy(gameObject);
		}
	}

	public virtual void Main()
	{
	}
}
