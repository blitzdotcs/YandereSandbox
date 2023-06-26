using System.Collections;
using UnityEngine;

public class FlashingController : HighlighterController
{
	public Color flashingStartColor = Color.blue;

	public Color flashingEndColor = Color.cyan;

	public float flashingDelay = 2.5f;

	public float flashingFrequency = 2f;

	private new void Start()
	{
		base.Start();
		StartCoroutine(DelayFlashing());
	}

	protected IEnumerator DelayFlashing()
	{
		yield return new WaitForSeconds(flashingDelay);
		h.FlashingOn(flashingStartColor, flashingEndColor, flashingFrequency);
	}
}
