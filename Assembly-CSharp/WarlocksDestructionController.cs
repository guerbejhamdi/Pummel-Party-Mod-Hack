using System;
using UnityEngine;

// Token: 0x02000295 RID: 661
public class WarlocksDestructionController : MonoBehaviour
{
	// Token: 0x06001379 RID: 4985 RVA: 0x0000F79A File Offset: 0x0000D99A
	public void Awake()
	{
		this.m_curDestructionLevel = this.m_startDestructionLevel;
		this.m_destructionListeners = base.GetComponentsInChildren<IWarlocksDestructionListener>();
	}

	// Token: 0x0600137A RID: 4986 RVA: 0x0000F7B4 File Offset: 0x0000D9B4
	public void Update()
	{
		if (this.m_destructionWaiting && Time.time > this.m_nextDestructionIncrease)
		{
			this.ApplyDestruction();
		}
	}

	// Token: 0x0600137B RID: 4987 RVA: 0x0000F7D1 File Offset: 0x0000D9D1
	public void IncreaseDestruction()
	{
		if (this.m_destructionWaiting)
		{
			return;
		}
		AudioSystem.PlayOneShot(this.m_destructionClip, 1f, 0f, 1f);
		this.m_nextDestructionIncrease = Time.time + 1.25f;
		this.m_destructionWaiting = true;
	}

	// Token: 0x0600137C RID: 4988 RVA: 0x00096280 File Offset: 0x00094480
	public void ResetDestruction()
	{
		this.m_curDestructionLevel = this.m_startDestructionLevel;
		IWarlocksDestructionListener[] destructionListeners = this.m_destructionListeners;
		for (int i = 0; i < destructionListeners.Length; i++)
		{
			destructionListeners[i].OnResetDestruction();
		}
	}

	// Token: 0x0600137D RID: 4989 RVA: 0x000962B8 File Offset: 0x000944B8
	private void ApplyDestruction()
	{
		this.m_curDestructionLevel--;
		IWarlocksDestructionListener[] destructionListeners = this.m_destructionListeners;
		for (int i = 0; i < destructionListeners.Length; i++)
		{
			destructionListeners[i].OnDestructionLevelChanged(this.m_curDestructionLevel);
		}
		this.m_camerShake.AddShake(1f);
		this.m_destructionWaiting = false;
	}

	// Token: 0x040014CA RID: 5322
	[SerializeField]
	private int m_startDestructionLevel;

	// Token: 0x040014CB RID: 5323
	[SerializeField]
	private AudioClip m_destructionClip;

	// Token: 0x040014CC RID: 5324
	[SerializeField]
	private CameraShake m_camerShake;

	// Token: 0x040014CD RID: 5325
	private int m_curDestructionLevel;

	// Token: 0x040014CE RID: 5326
	private float m_nextDestructionIncrease;

	// Token: 0x040014CF RID: 5327
	private bool m_destructionWaiting;

	// Token: 0x040014D0 RID: 5328
	private IWarlocksDestructionListener[] m_destructionListeners;
}
