using System;
using ZP.Utility;

// Token: 0x020003A0 RID: 928
public class ActionShellGame : BoardAction
{
	// Token: 0x17000271 RID: 625
	// (get) Token: 0x060018E6 RID: 6374 RVA: 0x00012696 File Offset: 0x00010896
	public int Seed
	{
		get
		{
			return this.seed;
		}
	}

	// Token: 0x17000272 RID: 626
	// (get) Token: 0x060018E7 RID: 6375 RVA: 0x0001269E File Offset: 0x0001089E
	// (set) Token: 0x060018E8 RID: 6376 RVA: 0x000126A6 File Offset: 0x000108A6
	public Random Rand { get; set; }

	// Token: 0x17000273 RID: 627
	// (get) Token: 0x060018E9 RID: 6377 RVA: 0x000126AF File Offset: 0x000108AF
	// (set) Token: 0x060018EA RID: 6378 RVA: 0x000126B7 File Offset: 0x000108B7
	public int RealChestIndex { get; set; }

	// Token: 0x060018EB RID: 6379 RVA: 0x000126C0 File Offset: 0x000108C0
	public ActionShellGame(int _seed)
	{
		this.action_type = BoardActionType.ShellGame;
		this.seed = _seed;
		this.Setup();
	}

	// Token: 0x060018EC RID: 6380 RVA: 0x000126DD File Offset: 0x000108DD
	public override void SerializeAction(ZPBitStream bs, bool write)
	{
		if (write)
		{
			bs.Write(this.seed);
			return;
		}
		this.seed = bs.ReadInt();
		this.Setup();
	}

	// Token: 0x060018ED RID: 6381 RVA: 0x00012701 File Offset: 0x00010901
	private void Setup()
	{
		this.Rand = new Random(this.Seed);
		this.RealChestIndex = this.Rand.Next(0, 3);
	}

	// Token: 0x04001AA0 RID: 6816
	private int seed;
}
