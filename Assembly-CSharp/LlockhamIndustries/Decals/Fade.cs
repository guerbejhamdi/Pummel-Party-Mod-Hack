using System;
using UnityEngine;

namespace LlockhamIndustries.Decals
{
	// Token: 0x0200088D RID: 2189
	[RequireComponent(typeof(ProjectionRenderer))]
	public class Fade : Modifier
	{
		// Token: 0x06003E5D RID: 15965 RVA: 0x00029F45 File Offset: 0x00028145
		private void Awake()
		{
			this.projection = base.GetComponent<ProjectionRenderer>();
		}

		// Token: 0x06003E5E RID: 15966 RVA: 0x00133270 File Offset: 0x00131470
		protected override void Begin()
		{
			this.timeElapsed = 0f;
			this.executing = true;
			this.originalAlpha = this.GetAlpha(this.projection);
			this.originalScale = this.projection.transform.localScale;
			switch (this.wrapMode)
			{
			case FadeWrapMode.Once:
			case FadeWrapMode.Clamp:
				this.fade.postWrapMode = WrapMode.ClampForever;
				break;
			case FadeWrapMode.Loop:
				this.fade.postWrapMode = WrapMode.Loop;
				break;
			case FadeWrapMode.PingPong:
				this.fade.postWrapMode = WrapMode.PingPong;
				break;
			}
			this.ApplyFade(this.projection, 0f);
		}

		// Token: 0x06003E5F RID: 15967 RVA: 0x00133310 File Offset: 0x00131510
		public override void Perform()
		{
			if (!this.executing || this.fade == null)
			{
				return;
			}
			if ((this.wrapMode != FadeWrapMode.Clamp && this.wrapMode != FadeWrapMode.Once) || this.timeElapsed <= this.fadeLength)
			{
				this.timeElapsed += base.UpdateRate;
				this.ApplyFade(this.projection, this.timeElapsed / this.fadeLength);
				return;
			}
			if (this.wrapMode == FadeWrapMode.Clamp)
			{
				this.ApplyFade(this.projection, 1f);
			}
			if (this.wrapMode == FadeWrapMode.Once)
			{
				this.projection.Destroy();
			}
			this.executing = false;
		}

		// Token: 0x06003E60 RID: 15968 RVA: 0x001333B0 File Offset: 0x001315B0
		private void ApplyFade(ProjectionRenderer Projection, float Time)
		{
			float num = this.fade.Evaluate(Time);
			switch (this.type)
			{
			case FadeType.Alpha:
				this.SetAlpha(Projection, this.originalAlpha * num);
				return;
			case FadeType.Scale:
				this.SetScale(Projection, this.originalScale * num);
				return;
			case FadeType.Both:
				this.SetAlpha(Projection, this.originalAlpha * num);
				this.SetScale(Projection, this.originalScale * num);
				return;
			default:
				return;
			}
		}

		// Token: 0x06003E61 RID: 15969 RVA: 0x0013342C File Offset: 0x0013162C
		private float GetAlpha(ProjectionRenderer Projection)
		{
			switch (Projection.Properties[0].type)
			{
			case PropertyType.Color:
				return Projection.Properties[0].color.a;
			case PropertyType.Float:
				return Projection.Properties[0].value;
			case PropertyType.Combo:
				return Projection.Properties[0].value;
			default:
				return 1f;
			}
		}

		// Token: 0x06003E62 RID: 15970 RVA: 0x001334A0 File Offset: 0x001316A0
		private void SetAlpha(ProjectionRenderer Projection, float Alpha)
		{
			PropertyType propertyType = Projection.Properties[0].type;
			if (propertyType != PropertyType.Color)
			{
				if (propertyType - PropertyType.Float <= 1)
				{
					Projection.SetFloat(0, Alpha);
				}
			}
			else
			{
				Color color = Projection.Properties[0].color;
				color.a = Alpha;
				Projection.SetColor(0, color);
			}
			Projection.UpdateProperties();
		}

		// Token: 0x06003E63 RID: 15971 RVA: 0x00029F53 File Offset: 0x00028153
		private void SetScale(ProjectionRenderer Projection, Vector3 Scale)
		{
			Projection.transform.localScale = Scale;
		}

		// Token: 0x04003A86 RID: 14982
		public FadeType type;

		// Token: 0x04003A87 RID: 14983
		public FadeWrapMode wrapMode;

		// Token: 0x04003A88 RID: 14984
		public AnimationCurve fade = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);

		// Token: 0x04003A89 RID: 14985
		public float fadeLength = 1f;

		// Token: 0x04003A8A RID: 14986
		private ProjectionRenderer projection;

		// Token: 0x04003A8B RID: 14987
		private float timeElapsed;

		// Token: 0x04003A8C RID: 14988
		private bool executing;

		// Token: 0x04003A8D RID: 14989
		private float originalAlpha;

		// Token: 0x04003A8E RID: 14990
		private Vector3 originalScale;
	}
}
