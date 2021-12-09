using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000349 RID: 841
public class MainBoardEvent : BoardNodeEvent
{
	// Token: 0x1700020D RID: 525
	// (get) Token: 0x060016AE RID: 5806 RVA: 0x000110FC File Offset: 0x0000F2FC
	// (set) Token: 0x060016AF RID: 5807 RVA: 0x00011104 File Offset: 0x0000F304
	public bool Finished { get; set; }

	// Token: 0x060016B0 RID: 5808 RVA: 0x0001110D File Offset: 0x0000F30D
	public virtual void Start()
	{
		base.StartCoroutine(this.Setup());
	}

	// Token: 0x060016B1 RID: 5809 RVA: 0x0001111C File Offset: 0x0000F31C
	private IEnumerator Setup()
	{
		while (GameManager.Board == null)
		{
			yield return null;
		}
		GameManager.Board.mainBoardEvent = this;
		yield break;
	}

	// Token: 0x060016B2 RID: 5810 RVA: 0x0001112B File Offset: 0x0000F32B
	public override IEnumerator DoEvent(BoardPlayer player, BoardNode boardNode, int seed)
	{
		yield return null;
		yield break;
	}

	// Token: 0x060016B3 RID: 5811 RVA: 0x00011133 File Offset: 0x0000F333
	public virtual IEnumerator DoTurnStartEvent(BoardPlayer player)
	{
		yield return null;
		yield break;
	}

	// Token: 0x060016B4 RID: 5812 RVA: 0x0001113B File Offset: 0x0000F33B
	public virtual IEnumerator DoFirstTurnEvent(BoardPlayer player)
	{
		this.Finished = true;
		yield return null;
		yield break;
	}

	// Token: 0x060016B5 RID: 5813 RVA: 0x0001114A File Offset: 0x0000F34A
	protected void DoGenericBoardEventActions()
	{
		AudioSystem.PlayOneShot("MetalWoodVacumAirTrapLock_Pond5", 0.5f, 0.1f);
	}

	// Token: 0x060016B6 RID: 5814 RVA: 0x0000539F File Offset: 0x0000359F
	public virtual int GetEventValue1()
	{
		return 0;
	}

	// Token: 0x060016B7 RID: 5815 RVA: 0x0000539F File Offset: 0x0000359F
	public virtual int GetEventValue2()
	{
		return 0;
	}

	// Token: 0x060016B8 RID: 5816 RVA: 0x0000398C File Offset: 0x00001B8C
	public virtual void SetupFromLoad(int val1, int val2)
	{
	}

	// Token: 0x040017ED RID: 6125
	public Sprite eventSprite;

	// Token: 0x040017EE RID: 6126
	public Color spriteColor;

	// Token: 0x040017EF RID: 6127
	public bool loadLate;

	// Token: 0x040017F0 RID: 6128
	protected new System.Random rand;
}
