using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001B9 RID: 441
public class IcebergFallingPlatform : MonoBehaviour
{
	// Token: 0x1700011D RID: 285
	// (get) Token: 0x06000CC1 RID: 3265 RVA: 0x0000BD76 File Offset: 0x00009F76
	public int Index
	{
		get
		{
			return this.m_platformIndex;
		}
	}

	// Token: 0x06000CC2 RID: 3266 RVA: 0x0000BD7E File Offset: 0x00009F7E
	private void Start()
	{
		this.m_startPos = base.transform.position;
	}

	// Token: 0x06000CC3 RID: 3267 RVA: 0x0000BD91 File Offset: 0x00009F91
	public void Init(IcebergController controller)
	{
		this.m_controller = controller;
	}

	// Token: 0x06000CC4 RID: 3268 RVA: 0x0006A5FC File Offset: 0x000687FC
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			IcebergPlayer component = collision.gameObject.GetComponent<IcebergPlayer>();
			if (!component.IsFinished && component.IsOwner)
			{
				if (!this.m_isFalling)
				{
					this.m_controller.PlatformFall(this.m_platformIndex, false);
				}
				this.Fall();
			}
		}
	}

	// Token: 0x06000CC5 RID: 3269 RVA: 0x0000BD9A File Offset: 0x00009F9A
	public void Fall()
	{
		if (!this.m_isFalling)
		{
			this.m_isFalling = true;
			base.StartCoroutine(this.FallRoutine());
		}
	}

	// Token: 0x06000CC6 RID: 3270 RVA: 0x0000BDB8 File Offset: 0x00009FB8
	private IEnumerator FallRoutine()
	{
		float time = 0f;
		while (time < this.m_fallTime)
		{
			time += Time.deltaTime;
			float time2 = Mathf.Clamp01(time / this.m_fallTime);
			float d = this.m_moveCurve.Evaluate(time2);
			base.transform.position = this.m_startPos + Vector3.down * d * 2f;
			yield return null;
		}
		this.m_isFalling = false;
		yield break;
	}

	// Token: 0x04000C15 RID: 3093
	[SerializeField]
	protected int m_platformIndex;

	// Token: 0x04000C16 RID: 3094
	[SerializeField]
	protected float m_fallTime;

	// Token: 0x04000C17 RID: 3095
	[SerializeField]
	protected AnimationCurve m_moveCurve;

	// Token: 0x04000C18 RID: 3096
	private Vector3 m_startPos;

	// Token: 0x04000C19 RID: 3097
	private bool m_isFalling;

	// Token: 0x04000C1A RID: 3098
	private IcebergController m_controller;
}
