using System;
using Rewired.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x02000659 RID: 1625
	[RequireComponent(typeof(CanvasScalerExt))]
	public class CanvasScalerFitter : MonoBehaviour
	{
		// Token: 0x06002CFC RID: 11516 RVA: 0x0001E5FB File Offset: 0x0001C7FB
		private void OnEnable()
		{
			this.canvasScaler = base.GetComponent<CanvasScalerExt>();
			this.Update();
			this.canvasScaler.ForceRefresh();
		}

		// Token: 0x06002CFD RID: 11517 RVA: 0x0001E61A File Offset: 0x0001C81A
		private void Update()
		{
			if (Screen.width != this.screenWidth || Screen.height != this.screenHeight)
			{
				this.screenWidth = Screen.width;
				this.screenHeight = Screen.height;
				this.UpdateSize();
			}
		}

		// Token: 0x06002CFE RID: 11518 RVA: 0x000F9364 File Offset: 0x000F7564
		private void UpdateSize()
		{
			if (this.canvasScaler.uiScaleMode != CanvasScaler.ScaleMode.ScaleWithScreenSize)
			{
				return;
			}
			if (this.breakPoints == null)
			{
				return;
			}
			float num = (float)Screen.width / (float)Screen.height;
			float num2 = float.PositiveInfinity;
			int num3 = 0;
			for (int i = 0; i < this.breakPoints.Length; i++)
			{
				float num4 = Mathf.Abs(num - this.breakPoints[i].screenAspectRatio);
				if ((num4 <= this.breakPoints[i].screenAspectRatio || MathTools.IsNear(this.breakPoints[i].screenAspectRatio, 0.01f)) && num4 < num2)
				{
					num2 = num4;
					num3 = i;
				}
			}
			this.canvasScaler.referenceResolution = this.breakPoints[num3].referenceResolution;
		}

		// Token: 0x04002E3C RID: 11836
		[SerializeField]
		private CanvasScalerFitter.BreakPoint[] breakPoints;

		// Token: 0x04002E3D RID: 11837
		private CanvasScalerExt canvasScaler;

		// Token: 0x04002E3E RID: 11838
		private int screenWidth;

		// Token: 0x04002E3F RID: 11839
		private int screenHeight;

		// Token: 0x04002E40 RID: 11840
		private Action ScreenSizeChanged;

		// Token: 0x0200065A RID: 1626
		[Serializable]
		private class BreakPoint
		{
			// Token: 0x04002E41 RID: 11841
			[SerializeField]
			public string name;

			// Token: 0x04002E42 RID: 11842
			[SerializeField]
			public float screenAspectRatio;

			// Token: 0x04002E43 RID: 11843
			[SerializeField]
			public Vector2 referenceResolution;
		}
	}
}
