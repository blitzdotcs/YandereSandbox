using System;
using HighlightingSystem;
using UnityEngine;

[Serializable]
public class BooHighlighterController : MonoBehaviour
{
	protected Highlighter h;

	public void Awake()
	{
		h = gameObject.GetComponent<Highlighter>() as Highlighter;
		if (h == null)
		{
			h = gameObject.AddComponent<Highlighter>() as Highlighter;
		}
	}

	public void Start()
	{
		h.FlashingOn(new Color(0f, 1f, 0f, 0f), new Color(0f, 1f, 0f, 1f), 1f);
	}
}
