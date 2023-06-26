using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class BlitHelper : MonoBehaviour
{
	private void OnRenderImage(RenderTexture src, RenderTexture dst)
	{
		Graphics.Blit(src, dst);
	}
}
