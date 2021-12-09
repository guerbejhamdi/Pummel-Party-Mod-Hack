using System;
using UnityEngine;
using UnityEngine.UI;

namespace MalbersAnimations
{
	// Token: 0x0200075E RID: 1886
	public class UIFollowTransform : MonoBehaviour
	{
		// Token: 0x170009A9 RID: 2473
		// (get) Token: 0x0600365C RID: 13916 RVA: 0x00024DFB File Offset: 0x00022FFB
		private Graphic Graph
		{
			get
			{
				if (this.graphic == null)
				{
					this.graphic = base.GetComponent<Graphic>();
				}
				return this.graphic;
			}
		}

		// Token: 0x0600365D RID: 13917 RVA: 0x00024E1D File Offset: 0x0002301D
		private void OnEnable()
		{
			this.Aling();
		}

		// Token: 0x0600365E RID: 13918 RVA: 0x00024E25 File Offset: 0x00023025
		public void SetTransform(Transform newTarget)
		{
			this.WorldTransform = newTarget;
		}

		// Token: 0x0600365F RID: 13919 RVA: 0x00024E1D File Offset: 0x0002301D
		private void Start()
		{
			this.Aling();
		}

		// Token: 0x06003660 RID: 13920 RVA: 0x00024E2E File Offset: 0x0002302E
		private void Awake()
		{
			this.main = Camera.main;
			this.graphic = base.GetComponent<Graphic>();
		}

		// Token: 0x06003661 RID: 13921 RVA: 0x00024E1D File Offset: 0x0002301D
		private void Update()
		{
			this.Aling();
		}

		// Token: 0x06003662 RID: 13922 RVA: 0x00024E47 File Offset: 0x00023047
		public void Aling()
		{
			if (!this.main || !this.WorldTransform)
			{
				return;
			}
			base.transform.position = this.main.WorldToScreenPoint(this.WorldTransform.position);
		}

		// Token: 0x06003663 RID: 13923 RVA: 0x00024E85 File Offset: 0x00023085
		public virtual void Fade_In_Out(bool value)
		{
			this.Graph.CrossFadeColor(value ? this.FadeIn : this.FadeOut, this.time, false, true);
		}

		// Token: 0x06003664 RID: 13924 RVA: 0x00024EAB File Offset: 0x000230AB
		public virtual void Fade_In(float time)
		{
			this.graphic.CrossFadeColor(this.FadeIn, time, false, true);
		}

		// Token: 0x06003665 RID: 13925 RVA: 0x00024EC1 File Offset: 0x000230C1
		public virtual void Fade_Out(float time)
		{
			this.graphic.CrossFadeColor(this.FadeOut, time, false, true);
		}

		// Token: 0x040035BF RID: 13759
		private Camera main;

		// Token: 0x040035C0 RID: 13760
		public Transform WorldTransform;

		// Token: 0x040035C1 RID: 13761
		public Color FadeOut;

		// Token: 0x040035C2 RID: 13762
		public Color FadeIn;

		// Token: 0x040035C3 RID: 13763
		public float time = 0.3f;

		// Token: 0x040035C4 RID: 13764
		private Graphic graphic;
	}
}
