using System;
using UnityEngine;

namespace Assets.Scripts.Cam.Effects {
	[ExecuteInEditMode]
	[RequireComponent(typeof(UnityEngine.Camera))]
	[AddComponentMenu("Image Effects/Custom/Retro Size")]
	public class RetroSize : MonoBehaviour {
		[Header("Resolution")]
		public int horizontalResolution = 160;
		public int verticalResolution = 144;

		public void OnRenderImage(RenderTexture src, RenderTexture dest) {
			horizontalResolution = Mathf.Clamp(horizontalResolution, 1, 2048);
			verticalResolution = Mathf.Clamp(verticalResolution, 1, 2048);
			RenderTexture scaled = RenderTexture.GetTemporary(horizontalResolution, verticalResolution);
			scaled.filterMode = FilterMode.Point;
			Graphics.Blit(src, scaled);
			Graphics.Blit(scaled, dest);
			RenderTexture.ReleaseTemporary(scaled);
		}
	}
}