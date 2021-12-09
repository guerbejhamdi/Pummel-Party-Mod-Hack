using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001F7 RID: 503
public class CameraCanvasScaler : CanvasScaler
{
	// Token: 0x06000EB3 RID: 3763 RVA: 0x0000CDB8 File Offset: 0x0000AFB8
	protected override void OnEnable()
	{
		this.m_Canvas = base.GetComponent<Canvas>();
		base.OnEnable();
	}

	// Token: 0x06000EB4 RID: 3764 RVA: 0x00075700 File Offset: 0x00073900
	protected override void HandleScaleWithScreenSize()
	{
		Vector2 vector = new Vector2((float)Screen.width, (float)Screen.height);
		if (this.m_Canvas.renderMode == RenderMode.ScreenSpaceCamera && this.m_Canvas.worldCamera != null)
		{
			vector.x *= this.m_Canvas.worldCamera.rect.width;
			vector.y *= this.m_Canvas.worldCamera.rect.height;
		}
		float scaleFactor = 0f;
		switch (this.m_ScreenMatchMode)
		{
		case CanvasScaler.ScreenMatchMode.MatchWidthOrHeight:
		{
			float a = Mathf.Log(vector.x / this.m_ReferenceResolution.x, 2f);
			float b = Mathf.Log(vector.y / this.m_ReferenceResolution.y, 2f);
			float p = Mathf.Lerp(a, b, this.m_MatchWidthOrHeight);
			scaleFactor = Mathf.Pow(2f, p);
			break;
		}
		case CanvasScaler.ScreenMatchMode.Expand:
			scaleFactor = Mathf.Min(vector.x / this.m_ReferenceResolution.x, vector.y / this.m_ReferenceResolution.y);
			break;
		case CanvasScaler.ScreenMatchMode.Shrink:
			scaleFactor = Mathf.Max(vector.x / this.m_ReferenceResolution.x, vector.y / this.m_ReferenceResolution.y);
			break;
		}
		base.SetScaleFactor(scaleFactor);
		base.SetReferencePixelsPerUnit(this.m_ReferencePixelsPerUnit);
	}

	// Token: 0x04000E4A RID: 3658
	public const float kLogBase = 2f;

	// Token: 0x04000E4B RID: 3659
	private Canvas m_Canvas;
}
