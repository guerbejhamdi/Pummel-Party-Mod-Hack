using System;
using UnityEngine;

// Token: 0x020001F3 RID: 499
public class Effects_PoliceCar : MonoBehaviour
{
	// Token: 0x06000E85 RID: 3717 RVA: 0x00073E6C File Offset: 0x0007206C
	private void Start()
	{
		if (this.m_states != null && this.m_states.Length != 0)
		{
			this.m_curState = this.m_states[this.m_curStateIndex];
			this.m_nextStateChange = Time.time + this.m_timeBetweenStates;
			return;
		}
		this.m_nextStateChange = -1f;
	}

	// Token: 0x06000E86 RID: 3718 RVA: 0x00073EBC File Offset: 0x000720BC
	private void Update()
	{
		if (Time.time > this.m_nextStateChange)
		{
			this.m_curStateIndex = (this.m_curStateIndex + 1) % this.m_states.Length;
			if (this.m_curState.stateObjects)
			{
				this.m_curState.stateObjects.SetActive(false);
			}
			this.m_curState = this.m_states[this.m_curStateIndex];
			if (this.m_curState.stateObjects)
			{
				this.m_curState.stateObjects.SetActive(true);
			}
			if (this.m_targetRenderer)
			{
				this.m_targetRenderer.sharedMaterial = this.m_curState.stateMaterial;
			}
			this.m_nextStateChange = Time.time + this.m_timeBetweenStates;
		}
	}

	// Token: 0x04000E10 RID: 3600
	[SerializeField]
	protected float m_timeBetweenStates = 1f;

	// Token: 0x04000E11 RID: 3601
	[SerializeField]
	protected PoliceCarState[] m_states;

	// Token: 0x04000E12 RID: 3602
	[SerializeField]
	protected MeshRenderer m_targetRenderer;

	// Token: 0x04000E13 RID: 3603
	private int m_curStateIndex;

	// Token: 0x04000E14 RID: 3604
	private PoliceCarState m_curState;

	// Token: 0x04000E15 RID: 3605
	private float m_nextStateChange;
}
