using System;
using UnityEngine;

[Serializable]
public class ClothingScript : MonoBehaviour
{
	public PromptScript Prompt;

	public virtual void Update()
	{
		if (!(Prompt.Circle[0].fillAmount > 0f))
		{
			Prompt.Yandere.Bloodiness = 0f;
			Prompt.Yandere.UpdateBlood();
			UnityEngine.Object.Destroy(gameObject);
		}
	}

	public virtual void Main()
	{
	}
}
