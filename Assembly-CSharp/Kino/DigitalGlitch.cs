using System;
using UnityEngine;

namespace Kino
{
	// Token: 0x020005A9 RID: 1449
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Kino Image Effects/Digital Glitch")]
	public class DigitalGlitch : MonoBehaviour
	{
		// Token: 0x17000477 RID: 1143
		// (get) Token: 0x060025A6 RID: 9638 RVA: 0x0001AEC3 File Offset: 0x000190C3
		// (set) Token: 0x060025A7 RID: 9639 RVA: 0x0001AECB File Offset: 0x000190CB
		public float intensity
		{
			get
			{
				return this._intensity;
			}
			set
			{
				this._intensity = value;
			}
		}

		// Token: 0x060025A8 RID: 9640 RVA: 0x0001AED4 File Offset: 0x000190D4
		private static Color RandomColor()
		{
			return new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
		}

		// Token: 0x060025A9 RID: 9641 RVA: 0x000E3590 File Offset: 0x000E1790
		private void SetUpResources()
		{
			if (this._material != null)
			{
				return;
			}
			this._material = new Material(this._shader);
			this._material.hideFlags = HideFlags.DontSave;
			this._noiseTexture = new Texture2D(64, 32, TextureFormat.ARGB32, false);
			this._noiseTexture.hideFlags = HideFlags.DontSave;
			this._noiseTexture.wrapMode = TextureWrapMode.Clamp;
			this._noiseTexture.filterMode = FilterMode.Point;
			this._trashFrame1 = new RenderTexture(Screen.width, Screen.height, 0);
			this._trashFrame2 = new RenderTexture(Screen.width, Screen.height, 0);
			this._trashFrame1.hideFlags = HideFlags.DontSave;
			this._trashFrame2.hideFlags = HideFlags.DontSave;
			this.UpdateNoiseTexture();
		}

		// Token: 0x060025AA RID: 9642 RVA: 0x000E364C File Offset: 0x000E184C
		private void UpdateNoiseTexture()
		{
			Color color = DigitalGlitch.RandomColor();
			for (int i = 0; i < this._noiseTexture.height; i++)
			{
				for (int j = 0; j < this._noiseTexture.width; j++)
				{
					if (UnityEngine.Random.value > 0.89f)
					{
						color = DigitalGlitch.RandomColor();
					}
					this._noiseTexture.SetPixel(j, i, color);
				}
			}
			this._noiseTexture.Apply();
		}

		// Token: 0x060025AB RID: 9643 RVA: 0x0001AEEF File Offset: 0x000190EF
		private void Update()
		{
			if (UnityEngine.Random.value > Mathf.Lerp(0.9f, 0.5f, this._intensity))
			{
				this.SetUpResources();
				this.UpdateNoiseTexture();
			}
		}

		// Token: 0x060025AC RID: 9644 RVA: 0x000E36B8 File Offset: 0x000E18B8
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			this.SetUpResources();
			int frameCount = Time.frameCount;
			if (frameCount % 13 == 0)
			{
				Graphics.Blit(source, this._trashFrame1);
			}
			if (frameCount % 73 == 0)
			{
				Graphics.Blit(source, this._trashFrame2);
			}
			this._material.SetFloat("_Intensity", this._intensity);
			this._material.SetTexture("_NoiseTex", this._noiseTexture);
			RenderTexture value = (UnityEngine.Random.value > 0.5f) ? this._trashFrame1 : this._trashFrame2;
			this._material.SetTexture("_TrashTex", value);
			Graphics.Blit(source, destination, this._material);
		}

		// Token: 0x04002924 RID: 10532
		[SerializeField]
		[Range(0f, 1f)]
		private float _intensity;

		// Token: 0x04002925 RID: 10533
		[SerializeField]
		private Shader _shader;

		// Token: 0x04002926 RID: 10534
		private Material _material;

		// Token: 0x04002927 RID: 10535
		private Texture2D _noiseTexture;

		// Token: 0x04002928 RID: 10536
		private RenderTexture _trashFrame1;

		// Token: 0x04002929 RID: 10537
		private RenderTexture _trashFrame2;
	}
}
