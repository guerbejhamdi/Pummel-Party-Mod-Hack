using System;
using UnityEngine;

namespace Kino
{
	// Token: 0x020005A8 RID: 1448
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Kino Image Effects/Analog Glitch")]
	public class AnalogGlitch : MonoBehaviour
	{
		// Token: 0x17000473 RID: 1139
		// (get) Token: 0x0600259C RID: 9628 RVA: 0x0001AE7F File Offset: 0x0001907F
		// (set) Token: 0x0600259D RID: 9629 RVA: 0x0001AE87 File Offset: 0x00019087
		public float scanLineJitter
		{
			get
			{
				return this._scanLineJitter;
			}
			set
			{
				this._scanLineJitter = value;
			}
		}

		// Token: 0x17000474 RID: 1140
		// (get) Token: 0x0600259E RID: 9630 RVA: 0x0001AE90 File Offset: 0x00019090
		// (set) Token: 0x0600259F RID: 9631 RVA: 0x0001AE98 File Offset: 0x00019098
		public float verticalJump
		{
			get
			{
				return this._verticalJump;
			}
			set
			{
				this._verticalJump = value;
			}
		}

		// Token: 0x17000475 RID: 1141
		// (get) Token: 0x060025A0 RID: 9632 RVA: 0x0001AEA1 File Offset: 0x000190A1
		// (set) Token: 0x060025A1 RID: 9633 RVA: 0x0001AEA9 File Offset: 0x000190A9
		public float horizontalShake
		{
			get
			{
				return this._horizontalShake;
			}
			set
			{
				this._horizontalShake = value;
			}
		}

		// Token: 0x17000476 RID: 1142
		// (get) Token: 0x060025A2 RID: 9634 RVA: 0x0001AEB2 File Offset: 0x000190B2
		// (set) Token: 0x060025A3 RID: 9635 RVA: 0x0001AEBA File Offset: 0x000190BA
		public float colorDrift
		{
			get
			{
				return this._colorDrift;
			}
			set
			{
				this._colorDrift = value;
			}
		}

		// Token: 0x060025A4 RID: 9636 RVA: 0x000E3460 File Offset: 0x000E1660
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (this._material == null)
			{
				this._material = new Material(this._shader);
				this._material.hideFlags = HideFlags.DontSave;
			}
			this._verticalJumpTime += Time.deltaTime * this._verticalJump * 11.3f;
			float y = Mathf.Clamp01(1f - this._scanLineJitter * 1.2f);
			float x = 0.002f + Mathf.Pow(this._scanLineJitter, 3f) * 0.05f;
			this._material.SetVector("_ScanLineJitter", new Vector2(x, y));
			Vector2 v = new Vector2(this._verticalJump, this._verticalJumpTime);
			this._material.SetVector("_VerticalJump", v);
			this._material.SetFloat("_HorizontalShake", this._horizontalShake * 0.2f);
			Vector2 v2 = new Vector2(this._colorDrift * 0.04f, Time.time * 606.11f);
			this._material.SetVector("_ColorDrift", v2);
			Graphics.Blit(source, destination, this._material);
		}

		// Token: 0x0400291D RID: 10525
		[SerializeField]
		[Range(0f, 1f)]
		private float _scanLineJitter;

		// Token: 0x0400291E RID: 10526
		[SerializeField]
		[Range(0f, 1f)]
		private float _verticalJump;

		// Token: 0x0400291F RID: 10527
		[SerializeField]
		[Range(0f, 1f)]
		private float _horizontalShake;

		// Token: 0x04002920 RID: 10528
		[SerializeField]
		[Range(0f, 1f)]
		private float _colorDrift;

		// Token: 0x04002921 RID: 10529
		[SerializeField]
		private Shader _shader;

		// Token: 0x04002922 RID: 10530
		private Material _material;

		// Token: 0x04002923 RID: 10531
		private float _verticalJumpTime;
	}
}
