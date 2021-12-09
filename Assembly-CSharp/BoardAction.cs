using System;
using UnityEngine;
using ZP.Utility;

// Token: 0x0200039C RID: 924
public class BoardAction
{
	// Token: 0x1700026B RID: 619
	// (get) Token: 0x060018D6 RID: 6358 RVA: 0x0001254B File Offset: 0x0001074B
	public BoardActionType ActionType
	{
		get
		{
			return this.action_type;
		}
	}

	// Token: 0x1700026C RID: 620
	// (get) Token: 0x060018D7 RID: 6359 RVA: 0x00012553 File Offset: 0x00010753
	// (set) Token: 0x060018D8 RID: 6360 RVA: 0x0001255B File Offset: 0x0001075B
	public bool Initialized
	{
		get
		{
			return this.action_initialized;
		}
		set
		{
			this.action_initialized = value;
		}
	}

	// Token: 0x1700026D RID: 621
	// (get) Token: 0x060018D9 RID: 6361 RVA: 0x00012564 File Offset: 0x00010764
	// (set) Token: 0x060018DA RID: 6362 RVA: 0x0001256C File Offset: 0x0001076C
	public int Step
	{
		get
		{
			return this.step;
		}
		set
		{
			this.step = value;
			Debug.Log(this.action_type.ToString() + " Step: " + value.ToString());
		}
	}

	// Token: 0x060018DB RID: 6363 RVA: 0x0001259C File Offset: 0x0001079C
	public BoardAction()
	{
		this.action_type = BoardActionType.NoAction;
		this.action_initialized = false;
		this.step = 0;
	}

	// Token: 0x060018DC RID: 6364 RVA: 0x000125B9 File Offset: 0x000107B9
	public virtual void SerializeAction(ZPBitStream bs, bool write)
	{
		Debug.LogError("BoardAction : " + this.action_type.ToString() + " serialize not implemented.");
	}

	// Token: 0x04001A9A RID: 6810
	protected BoardActionType action_type;

	// Token: 0x04001A9B RID: 6811
	protected bool action_initialized;

	// Token: 0x04001A9C RID: 6812
	protected int step;
}
