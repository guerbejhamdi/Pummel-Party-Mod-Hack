using System;

// Token: 0x020002AF RID: 687
public abstract class GameModifier
{
	// Token: 0x170001B0 RID: 432
	// (get) Token: 0x060013F3 RID: 5107 RVA: 0x0000FB7D File Offset: 0x0000DD7D
	// (set) Token: 0x060013F4 RID: 5108 RVA: 0x0000FB85 File Offset: 0x0000DD85
	public GameModifierType ModifierType { get; set; }

	// Token: 0x060013F5 RID: 5109 RVA: 0x0000FB8E File Offset: 0x0000DD8E
	public int GetGameModifierID()
	{
		return GameModifier.m_baseID[(int)this.ModifierType] + this.GetModifierID();
	}

	// Token: 0x060013F6 RID: 5110
	protected abstract int GetModifierID();

	// Token: 0x0400152C RID: 5420
	private static int[] m_baseID = new int[]
	{
		0,
		10000,
		20000
	};
}
