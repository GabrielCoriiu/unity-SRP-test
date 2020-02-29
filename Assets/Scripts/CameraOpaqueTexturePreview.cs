using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.LWRP;

[DisallowMultipleComponent]
[RequireComponent(typeof(RawImage))]
public class CameraOpaqueTexturePreview : MonoBehaviour
{
	static readonly int CameraOpaqueTextureID = Shader.PropertyToID("_CameraOpaqueTexture");

	[SerializeField] private Text textLabel = null;
	private Texture opaqueTexture = null;

	protected void LateUpdate()
    {
		if (opaqueTexture != null)
			return;

		opaqueTexture = Shader.GetGlobalTexture(CameraOpaqueTextureID);

		GetComponent<RawImage>().texture = opaqueTexture;
		RectTransform rectTransform = (transform as RectTransform);
		rectTransform.sizeDelta = new Vector2(
			rectTransform.sizeDelta.y * Screen.width / Screen.height,
			rectTransform.sizeDelta.y
		);

		if (textLabel != null && opaqueTexture != null)
		{
			textLabel.text = $"" +
				$"{opaqueTexture.width} x {opaqueTexture.height} \n" +
				$"supposed to be: {opaqueTexture.width / 4} x {opaqueTexture.height / 4} \n" +
				$"downsample: {(GraphicsSettings.renderPipelineAsset as LightweightRenderPipelineAsset).opaqueDownsampling}";
		}
	}
}
