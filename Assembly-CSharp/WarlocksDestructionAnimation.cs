using System;
using UnityEngine;

// Token: 0x02000294 RID: 660
public class WarlocksDestructionAnimation : MonoBehaviour, IWarlocksDestructionListener
{
	// Token: 0x06001375 RID: 4981 RVA: 0x0000F73A File Offset: 0x0000D93A
	public void Awake()
	{
		this.m_anim = base.GetComponent<Animator>();
	}

	// Token: 0x06001376 RID: 4982 RVA: 0x0000F748 File Offset: 0x0000D948
	public void OnDestructionLevelChanged(int level)
	{
		if (level == this.m_destructionLevel && this.m_anim != null)
		{
			this.m_anim.SetBool(this.m_animatorVariableName, true);
		}
	}

	// Token: 0x06001377 RID: 4983 RVA: 0x0000F773 File Offset: 0x0000D973
	public void OnResetDestruction()
	{
		this.m_anim.SetBool(this.m_animatorVariableName, false);
	}

	// Token: 0x040014C7 RID: 5319
	[SerializeField]
	protected int m_destructionLevel;

	// Token: 0x040014C8 RID: 5320
	[SerializeField]
	protected string m_animatorVariableName = "";

	// Token: 0x040014C9 RID: 5321
	private Animator m_anim;
}
