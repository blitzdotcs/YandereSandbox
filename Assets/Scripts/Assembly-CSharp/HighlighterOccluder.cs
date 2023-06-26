using HighlightingSystem;
using UnityEngine;

public class HighlighterOccluder : MonoBehaviour
{
	private Highlighter h;

	private void Awake()
	{
		h = GetComponent<Highlighter>();
		if (h == null)
		{
			h = base.gameObject.AddComponent<Highlighter>();
		}
	}

	private void OnEnable()
	{
		h.OccluderOn();
	}
}
