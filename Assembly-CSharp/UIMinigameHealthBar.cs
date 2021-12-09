using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000550 RID: 1360
public class UIMinigameHealthBar : MonoBehaviour
{
	// Token: 0x060023DB RID: 9179 RVA: 0x000D86B0 File Offset: 0x000D68B0
	private void Update()
	{
		if (this.m_target != null)
		{
			Vector3 b = new Vector3(0f, this.m_height, 0f);
			base.transform.position = this.m_cam.WorldToScreenPoint(this.m_target.position + b);
		}
		this.m_curFill = Mathf.SmoothDamp(this.m_curFill, this.m_targetFill, ref this.m_fillVelocity, 0.5f);
		if (this.m_curFill != this.m_fillImg.fillAmount)
		{
			this.m_fillImg.fillAmount = this.m_curFill;
		}
		this.m_alpha = Mathf.Clamp(this.m_alpha - Time.deltaTime * 0.5f, 0.25f, 4f);
		float num = Mathf.Clamp(this.m_alpha, 0.25f, 1f);
		if (this.m_group.alpha != num)
		{
			this.m_group.alpha = num;
		}
	}

	// Token: 0x060023DC RID: 9180 RVA: 0x00019D64 File Offset: 0x00017F64
	public void Initialize(Transform target, float height, Camera cam)
	{
		this.m_target = target;
		this.m_height = height;
		this.m_cam = cam;
	}

	// Token: 0x060023DD RID: 9181 RVA: 0x00019D7B File Offset: 0x00017F7B
	public void SetHealth(float val)
	{
		this.m_targetFill = val;
		this.m_alpha = 4f;
	}

	// Token: 0x040026D5 RID: 9941
	[Header("References")]
	[SerializeField]
	private Image m_fillImg;

	// Token: 0x040026D6 RID: 9942
	[SerializeField]
	private CanvasGroup m_group;

	// Token: 0x040026D7 RID: 9943
	private Transform m_target;

	// Token: 0x040026D8 RID: 9944
	private float m_height;

	// Token: 0x040026D9 RID: 9945
	private Camera m_cam;

	// Token: 0x040026DA RID: 9946
	private float m_curFill = 1f;

	// Token: 0x040026DB RID: 9947
	private float m_targetFill = 1f;

	// Token: 0x040026DC RID: 9948
	private float m_fillVelocity;

	// Token: 0x040026DD RID: 9949
	private float m_alpha = 4f;
}
