using System;

// Token: 0x0200007C RID: 124
public class GobletChallenge
{
	// Token: 0x1700005F RID: 95
	// (get) Token: 0x06000274 RID: 628 RVA: 0x00005218 File Offset: 0x00003418
	// (set) Token: 0x06000275 RID: 629 RVA: 0x00005220 File Offset: 0x00003420
	public int NumPlayers { get; set; }

	// Token: 0x17000060 RID: 96
	// (get) Token: 0x06000276 RID: 630 RVA: 0x00005229 File Offset: 0x00003429
	// (set) Token: 0x06000277 RID: 631 RVA: 0x00005231 File Offset: 0x00003431
	public StatChallengeExtent Extent { get; set; }

	// Token: 0x17000061 RID: 97
	// (get) Token: 0x06000278 RID: 632 RVA: 0x0000523A File Offset: 0x0000343A
	// (set) Token: 0x06000279 RID: 633 RVA: 0x00005242 File Offset: 0x00003442
	public StatType Stat { get; set; }

	// Token: 0x17000062 RID: 98
	// (get) Token: 0x0600027A RID: 634 RVA: 0x0000524B File Offset: 0x0000344B
	// (set) Token: 0x0600027B RID: 635 RVA: 0x00005253 File Offset: 0x00003453
	public StatChallengeBoardEvent Event { get; set; }

	// Token: 0x0600027C RID: 636 RVA: 0x00035C84 File Offset: 0x00033E84
	public string GetChallengeText()
	{
		string text = "";
		return string.Concat(new string[]
		{
			text,
			"Award for the ",
			this.Extent.ToString(),
			" ",
			PlayerStats.statNames[(int)this.Stat]
		});
	}
}
