using System;
using Rewired;
using ZP.Net;

// Token: 0x020003F8 RID: 1016
public class GamePlayer
{
	// Token: 0x17000324 RID: 804
	// (get) Token: 0x06001BF1 RID: 7153 RVA: 0x0001475B File Offset: 0x0001295B
	// (set) Token: 0x06001BF2 RID: 7154 RVA: 0x00014763 File Offset: 0x00012963
	public string Name
	{
		get
		{
			return this.player_name;
		}
		set
		{
			this.player_name = value;
		}
	}

	// Token: 0x17000325 RID: 805
	// (get) Token: 0x06001BF3 RID: 7155 RVA: 0x0001476C File Offset: 0x0001296C
	// (set) Token: 0x06001BF4 RID: 7156 RVA: 0x00014774 File Offset: 0x00012974
	public bool IsLocalPlayer
	{
		get
		{
			return this.local_player;
		}
		set
		{
			this.local_player = value;
		}
	}

	// Token: 0x17000326 RID: 806
	// (get) Token: 0x06001BF5 RID: 7157 RVA: 0x0001477D File Offset: 0x0001297D
	// (set) Token: 0x06001BF6 RID: 7158 RVA: 0x00014785 File Offset: 0x00012985
	public short LocalID
	{
		get
		{
			return this.local_id;
		}
		set
		{
			this.local_id = value;
		}
	}

	// Token: 0x17000327 RID: 807
	// (get) Token: 0x06001BF7 RID: 7159 RVA: 0x0001478E File Offset: 0x0001298E
	public short LocalIDAndAI
	{
		get
		{
			if (!this.is_ai && this.IsLocalPlayer)
			{
				return this.local_id;
			}
			return -1;
		}
	}

	// Token: 0x17000328 RID: 808
	// (get) Token: 0x06001BF8 RID: 7160 RVA: 0x000147A8 File Offset: 0x000129A8
	// (set) Token: 0x06001BF9 RID: 7161 RVA: 0x000147B0 File Offset: 0x000129B0
	public short GlobalID
	{
		get
		{
			return this.global_id;
		}
		set
		{
			this.global_id = value;
		}
	}

	// Token: 0x17000329 RID: 809
	// (get) Token: 0x06001BFA RID: 7162 RVA: 0x000147B9 File Offset: 0x000129B9
	public ushort ColorIndex
	{
		get
		{
			return this.color_index;
		}
	}

	// Token: 0x1700032A RID: 810
	// (get) Token: 0x06001BFB RID: 7163 RVA: 0x000147C1 File Offset: 0x000129C1
	public PlayerColor Color
	{
		get
		{
			return this.playerColor;
		}
	}

	// Token: 0x1700032B RID: 811
	// (get) Token: 0x06001BFC RID: 7164 RVA: 0x000147C9 File Offset: 0x000129C9
	public ushort SkinIndex
	{
		get
		{
			return this.skin_index;
		}
	}

	// Token: 0x1700032C RID: 812
	// (get) Token: 0x06001BFD RID: 7165 RVA: 0x000147D1 File Offset: 0x000129D1
	public PlayerSkin Skin
	{
		get
		{
			return this.playerSkin;
		}
	}

	// Token: 0x1700032D RID: 813
	// (get) Token: 0x06001BFE RID: 7166 RVA: 0x000147D9 File Offset: 0x000129D9
	public byte HatIndex
	{
		get
		{
			return this.hat_index;
		}
	}

	// Token: 0x1700032E RID: 814
	// (get) Token: 0x06001BFF RID: 7167 RVA: 0x000147E1 File Offset: 0x000129E1
	public CharacterHat Hat
	{
		get
		{
			return this.playerHat;
		}
	}

	// Token: 0x1700032F RID: 815
	// (get) Token: 0x06001C00 RID: 7168 RVA: 0x000147E9 File Offset: 0x000129E9
	public byte CapeIndex
	{
		get
		{
			return this.cape_index;
		}
	}

	// Token: 0x17000330 RID: 816
	// (get) Token: 0x06001C01 RID: 7169 RVA: 0x000147F1 File Offset: 0x000129F1
	// (set) Token: 0x06001C02 RID: 7170 RVA: 0x000147F9 File Offset: 0x000129F9
	public bool IsAI
	{
		get
		{
			return this.is_ai;
		}
		set
		{
			this.is_ai = value;
		}
	}

	// Token: 0x17000331 RID: 817
	// (get) Token: 0x06001C03 RID: 7171 RVA: 0x00014802 File Offset: 0x00012A02
	// (set) Token: 0x06001C04 RID: 7172 RVA: 0x0001480A File Offset: 0x00012A0A
	public int MinigameTeam
	{
		get
		{
			return this.minigame_team;
		}
		set
		{
			this.minigame_team = value;
		}
	}

	// Token: 0x17000332 RID: 818
	// (get) Token: 0x06001C05 RID: 7173 RVA: 0x00014813 File Offset: 0x00012A13
	// (set) Token: 0x06001C06 RID: 7174 RVA: 0x0001481B File Offset: 0x00012A1B
	public int MinigameScore
	{
		get
		{
			return this.minigame_score;
		}
		set
		{
			this.minigame_score = value;
		}
	}

	// Token: 0x17000333 RID: 819
	// (get) Token: 0x06001C07 RID: 7175 RVA: 0x00014824 File Offset: 0x00012A24
	// (set) Token: 0x06001C08 RID: 7176 RVA: 0x0001482C File Offset: 0x00012A2C
	public NetPlayer NetOwner
	{
		get
		{
			return this.net_owner;
		}
		set
		{
			this.net_owner = value;
		}
	}

	// Token: 0x17000334 RID: 820
	// (get) Token: 0x06001C09 RID: 7177 RVA: 0x00014835 File Offset: 0x00012A35
	// (set) Token: 0x06001C0A RID: 7178 RVA: 0x0001483D File Offset: 0x00012A3D
	public BoardPlayer BoardObject
	{
		get
		{
			return this.board_player;
		}
		set
		{
			this.board_player = value;
		}
	}

	// Token: 0x17000335 RID: 821
	// (get) Token: 0x06001C0B RID: 7179 RVA: 0x00014846 File Offset: 0x00012A46
	// (set) Token: 0x06001C0C RID: 7180 RVA: 0x0001484E File Offset: 0x00012A4E
	public Player RewiredPlayer
	{
		get
		{
			return this.rewiredPlayer;
		}
		set
		{
			this.rewiredPlayer = value;
		}
	}

	// Token: 0x17000336 RID: 822
	// (get) Token: 0x06001C0D RID: 7181 RVA: 0x00014857 File Offset: 0x00012A57
	public BotDifficulty Difficulty
	{
		get
		{
			if (this.IsAI)
			{
				return this.difficulty;
			}
			return BotDifficulty.Hard;
		}
	}

	// Token: 0x06001C0E RID: 7182 RVA: 0x00014869 File Offset: 0x00012A69
	public GamePlayer()
	{
		this.player_name = "Player";
		this.local_player = true;
		this.local_id = 0;
	}

	// Token: 0x06001C0F RID: 7183 RVA: 0x000BAF58 File Offset: 0x000B9158
	public GamePlayer(string name, bool local, short _local_id, short _global_id, ushort color, ushort skin, byte hat, byte cape, bool _is_ai, BotDifficulty _difficulty, NetPlayer _client_owner)
	{
		this.player_name = name;
		this.local_player = local;
		this.local_id = _local_id;
		this.global_id = _global_id;
		this.net_owner = _client_owner;
		this.color_index = color;
		this.skin_index = skin;
		this.hat_index = hat;
		this.cape_index = cape;
		this.is_ai = _is_ai;
		this.difficulty = _difficulty;
		this.playerColor = GameManager.GetColorAtIndex((int)this.color_index);
		this.playerSkin = GameManager.GetSkinAtIndex((int)this.skin_index);
		this.playerHat = GameManager.GetHatAtIndex((int)this.hat_index);
		if (this.IsLocalPlayer && !this.is_ai)
		{
			this.rewiredPlayer = ReInput.players.GetPlayer((int)this.local_id);
		}
	}

	// Token: 0x17000337 RID: 823
	// (get) Token: 0x06001C10 RID: 7184 RVA: 0x0001488A File Offset: 0x00012A8A
	// (set) Token: 0x06001C11 RID: 7185 RVA: 0x00014892 File Offset: 0x00012A92
	public byte Placement { get; set; }

	// Token: 0x04001E1C RID: 7708
	private string player_name;

	// Token: 0x04001E1D RID: 7709
	private bool local_player;

	// Token: 0x04001E1E RID: 7710
	private short local_id;

	// Token: 0x04001E1F RID: 7711
	private short global_id;

	// Token: 0x04001E20 RID: 7712
	private ushort color_index;

	// Token: 0x04001E21 RID: 7713
	private ushort skin_index;

	// Token: 0x04001E22 RID: 7714
	private byte hat_index;

	// Token: 0x04001E23 RID: 7715
	private byte cape_index;

	// Token: 0x04001E24 RID: 7716
	private bool gender;

	// Token: 0x04001E25 RID: 7717
	private bool is_ai;

	// Token: 0x04001E26 RID: 7718
	private int minigame_team;

	// Token: 0x04001E27 RID: 7719
	private int minigame_score;

	// Token: 0x04001E28 RID: 7720
	private NetPlayer net_owner;

	// Token: 0x04001E29 RID: 7721
	private BoardPlayer board_player;

	// Token: 0x04001E2A RID: 7722
	private PlayerColor playerColor;

	// Token: 0x04001E2B RID: 7723
	private PlayerSkin playerSkin;

	// Token: 0x04001E2C RID: 7724
	private CharacterHat playerHat;

	// Token: 0x04001E2D RID: 7725
	private Player rewiredPlayer;

	// Token: 0x04001E2E RID: 7726
	private BotDifficulty difficulty;
}
