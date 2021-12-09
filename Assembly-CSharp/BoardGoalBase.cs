using System;
using UnityEngine;

// Token: 0x02000339 RID: 825
public class BoardGoalBase : MonoBehaviour
{
	// Token: 0x06001671 RID: 5745 RVA: 0x0000398C File Offset: 0x00001B8C
	public virtual void Open()
	{
	}

	// Token: 0x06001672 RID: 5746 RVA: 0x0000398C File Offset: 0x00001B8C
	public virtual void Despawn()
	{
	}

	// Token: 0x06001673 RID: 5747 RVA: 0x0000398C File Offset: 0x00001B8C
	public virtual void Spawn()
	{
	}

	// Token: 0x06001674 RID: 5748 RVA: 0x0000539F File Offset: 0x0000359F
	public virtual bool IsFake()
	{
		return false;
	}
}
