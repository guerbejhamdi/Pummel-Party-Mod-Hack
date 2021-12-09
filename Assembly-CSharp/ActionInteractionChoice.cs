using System;
using ZP.Utility;

// Token: 0x020003AF RID: 943
public class ActionInteractionChoice : BoardAction
{
	// Token: 0x17000294 RID: 660
	// (get) Token: 0x0600192A RID: 6442 RVA: 0x00012AC9 File Offset: 0x00010CC9
	public byte InteractionChoice
	{
		get
		{
			return this.interaction_choice;
		}
	}

	// Token: 0x17000295 RID: 661
	// (get) Token: 0x0600192B RID: 6443 RVA: 0x00012AD1 File Offset: 0x00010CD1
	public int Seed
	{
		get
		{
			return this.seed;
		}
	}

	// Token: 0x0600192C RID: 6444 RVA: 0x00012AD9 File Offset: 0x00010CD9
	public ActionInteractionChoice(byte _interaction_choice, int _seed)
	{
		this.action_type = BoardActionType.InteractionChoice;
		this.interaction_choice = _interaction_choice;
		this.seed = _seed;
	}

	// Token: 0x0600192D RID: 6445 RVA: 0x00012AF7 File Offset: 0x00010CF7
	public override void SerializeAction(ZPBitStream bs, bool write)
	{
		if (write)
		{
			bs.Write(this.interaction_choice);
			bs.Write(this.seed);
			return;
		}
		this.interaction_choice = bs.ReadByte();
		this.seed = bs.ReadInt();
	}

	// Token: 0x04001ACB RID: 6859
	private byte interaction_choice;

	// Token: 0x04001ACC RID: 6860
	private int seed;

	// Token: 0x04001ACD RID: 6861
	public Interaction interactionScript;
}
