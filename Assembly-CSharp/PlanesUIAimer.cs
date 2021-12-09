using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001FD RID: 509
public class PlanesUIAimer : MonoBehaviour
{
	// Token: 0x06000EEF RID: 3823 RVA: 0x0000CFDB File Offset: 0x0000B1DB
	public void SetCamera(Camera cam)
	{
		this.m_camera = cam;
		this.m_canvas.worldCamera = this.m_camera;
		this.m_canvas.planeDistance = this.m_camera.nearClipPlane + 0.01f;
	}

	// Token: 0x06000EF0 RID: 3824 RVA: 0x00077780 File Offset: 0x00075980
	public void SetPosition(Vector3 worldPosition)
	{
		Vector2 vector = -Vector2.one;
		if (this.m_camera != null)
		{
			vector = this.m_camera.WorldToViewportPoint(worldPosition);
		}
		Vector2 anchoredPosition = new Vector2(vector.x * this.m_canvasRect.sizeDelta.x - this.m_canvasRect.sizeDelta.x * 0.5f, vector.y * this.m_canvasRect.sizeDelta.y - this.m_canvasRect.sizeDelta.y * 0.5f);
		this.m_aimerParent.anchoredPosition = anchoredPosition;
	}

	// Token: 0x06000EF1 RID: 3825 RVA: 0x00077828 File Offset: 0x00075A28
	public void SetLockScale(float scale)
	{
		this.m_aimerImage.rectTransform.localScale = Vector3.Lerp(Vector3.one * 0.25f, Vector3.one, scale);
		this.m_aimerImage.color = Color.Lerp(this.m_lockColor, this.m_unlockColor, scale);
	}

	// Token: 0x06000EF2 RID: 3826 RVA: 0x0000D011 File Offset: 0x0000B211
	public void SetPlayerHealth(int cur, int max)
	{
		this.m_playerHealthFill.fillAmount = this.GetFill(cur, max);
	}

	// Token: 0x06000EF3 RID: 3827 RVA: 0x0000D026 File Offset: 0x0000B226
	public void SetEnemyHealth(int cur, int max)
	{
		this.m_enemyHealthFill.fillAmount = this.GetFill(cur, max);
	}

	// Token: 0x06000EF4 RID: 3828 RVA: 0x0007787C File Offset: 0x00075A7C
	public void SetHasTarget(bool hasTarget)
	{
		if (!hasTarget)
		{
			if (this.m_enemyHealthFill.transform.parent.gameObject.activeSelf)
			{
				this.m_enemyHealthFill.transform.parent.gameObject.SetActive(false);
			}
			Color unlockColor = this.m_unlockColor;
			unlockColor.a = 0.5f;
			this.m_aimerImage.color = unlockColor;
			return;
		}
		if (!this.m_enemyHealthFill.transform.parent.gameObject.activeSelf)
		{
			this.m_enemyHealthFill.transform.parent.gameObject.SetActive(true);
		}
	}

	// Token: 0x06000EF5 RID: 3829 RVA: 0x0000D03B File Offset: 0x0000B23B
	private float GetFill(int val, int max)
	{
		return (float)val / (float)max;
	}

	// Token: 0x04000EB0 RID: 3760
	[SerializeField]
	private Canvas m_canvas;

	// Token: 0x04000EB1 RID: 3761
	[SerializeField]
	private RectTransform m_canvasRect;

	// Token: 0x04000EB2 RID: 3762
	[SerializeField]
	private RectTransform m_aimerParent;

	// Token: 0x04000EB3 RID: 3763
	[SerializeField]
	private Image m_aimerImage;

	// Token: 0x04000EB4 RID: 3764
	[SerializeField]
	private Image m_enemyHealthFill;

	// Token: 0x04000EB5 RID: 3765
	[SerializeField]
	private Image m_playerHealthFill;

	// Token: 0x04000EB6 RID: 3766
	[SerializeField]
	private Color m_lockColor;

	// Token: 0x04000EB7 RID: 3767
	[SerializeField]
	private Color m_unlockColor;

	// Token: 0x04000EB8 RID: 3768
	private Camera m_camera;
}
