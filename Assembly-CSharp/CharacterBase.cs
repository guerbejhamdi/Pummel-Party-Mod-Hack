using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using ZP.Net;

// Token: 0x020003CD RID: 973
public class CharacterBase : NetBehaviour
{
	// Token: 0x170002CC RID: 716
	// (get) Token: 0x06001A03 RID: 6659 RVA: 0x0001331F File Offset: 0x0001151F
	// (set) Token: 0x06001A04 RID: 6660 RVA: 0x00013327 File Offset: 0x00011527
	public bool AIActive { get; set; }

	// Token: 0x170002CD RID: 717
	// (get) Token: 0x06001A05 RID: 6661 RVA: 0x00013330 File Offset: 0x00011530
	public GamePlayer GamePlayer
	{
		get
		{
			return this.player;
		}
	}

	// Token: 0x170002CE RID: 718
	// (get) Token: 0x06001A06 RID: 6662 RVA: 0x00013338 File Offset: 0x00011538
	// (set) Token: 0x06001A07 RID: 6663 RVA: 0x00013340 File Offset: 0x00011540
	public bool IsDead
	{
		get
		{
			return this.isDead;
		}
		set
		{
			this.isDead = value;
		}
	}

	// Token: 0x170002CF RID: 719
	// (get) Token: 0x06001A08 RID: 6664 RVA: 0x00013349 File Offset: 0x00011549
	// (set) Token: 0x06001A09 RID: 6665 RVA: 0x00013351 File Offset: 0x00011551
	public short RoundScore
	{
		get
		{
			return this.round_score;
		}
		set
		{
			this.round_score = value;
		}
	}

	// Token: 0x170002D0 RID: 720
	// (get) Token: 0x06001A0A RID: 6666 RVA: 0x0001335A File Offset: 0x0001155A
	// (set) Token: 0x06001A0B RID: 6667 RVA: 0x00013367 File Offset: 0x00011567
	public virtual short Score
	{
		get
		{
			return this.score.Value;
		}
		set
		{
			if (NetSystem.IsServer)
			{
				this.score.Value = value;
			}
			if (GameManager.Minigame != null)
			{
				GameManager.Minigame.UpdateScoreUI((int)base.OwnerSlot, this.score.Value);
			}
		}
	}

	// Token: 0x06001A0C RID: 6668 RVA: 0x000133A4 File Offset: 0x000115A4
	public void ScoreRecieve(object val)
	{
		this.Score = (short)val;
	}

	// Token: 0x06001A0D RID: 6669 RVA: 0x000AD20C File Offset: 0x000AB40C
	public override void OnNetInitialize()
	{
		this.startPosition = base.transform.position;
		this.startRotation = base.transform.rotation;
		this.controller = base.GetComponentInChildren<CharacterController>();
		this.player = GameManager.GetPlayerAt((int)base.OwnerSlot);
		this.playerAnim = base.GetComponentInChildren<PlayerAnimation>(true);
		this.playerCosmetics = base.GetComponentInChildren<PlayerCosmetics>(true);
		if (this.playerAnim != null)
		{
			this.playerAnim.Setup();
			this.playerAnim.SetPlayerRotationImmediate(base.transform.rotation.eulerAngles.y);
			this.playerAnim.SetPlayer(this.player);
		}
		if (this.playerCosmetics != null)
		{
			this.playerCosmetics.Setup();
			this.playerCosmetics.SetPlayer(this.player);
		}
		if (!NetSystem.IsServer)
		{
			this.score.Recieve = new RecieveProxy(this.ScoreRecieve);
		}
		this.Deactivate();
		base.OnNetInitialize();
	}

	// Token: 0x06001A0E RID: 6670 RVA: 0x000AD314 File Offset: 0x000AB514
	protected virtual void Start()
	{
		this.agent = base.gameObject.GetComponent<NavMeshAgent>();
		if (this.agent != null && this.GamePlayer.IsAI && this.GamePlayer.IsLocalPlayer)
		{
			this.agent.updatePosition = true;
			this.agent.updateRotation = false;
			this.agent.updateUpAxis = false;
			return;
		}
		if (this.agent != null)
		{
			this.agent.enabled = false;
		}
	}

	// Token: 0x06001A0F RID: 6671 RVA: 0x000133B2 File Offset: 0x000115B2
	public virtual void Activate()
	{
		this.root_active = true;
		if (this.player_root != null)
		{
			this.player_root.SetActive(true);
		}
	}

	// Token: 0x06001A10 RID: 6672 RVA: 0x000133D5 File Offset: 0x000115D5
	public virtual void Deactivate()
	{
		this.root_active = false;
		if (this.player_root != null)
		{
			this.player_root.SetActive(false);
		}
	}

	// Token: 0x06001A11 RID: 6673 RVA: 0x000AD39C File Offset: 0x000AB59C
	public virtual void ResetPlayer()
	{
		this.isDead = false;
		base.transform.position = this.startPosition;
		base.transform.rotation = this.startRotation;
		if (this.agent != null)
		{
			this.agent.velocity = Vector3.zero;
			this.agent.Warp(this.startPosition);
			this.agent.nextPosition = this.startPosition;
		}
		if (this.playerAnim != null)
		{
			this.playerAnim.SetPlayerRotationImmediate(base.transform.rotation.eulerAngles.y);
		}
	}

	// Token: 0x170002D1 RID: 721
	// (get) Token: 0x06001A12 RID: 6674 RVA: 0x00012D07 File Offset: 0x00010F07
	public Vector3 MidPoint
	{
		get
		{
			return base.transform.position;
		}
	}

	// Token: 0x06001A13 RID: 6675 RVA: 0x0000398C File Offset: 0x00001B8C
	public virtual void FinishedSpawning()
	{
	}

	// Token: 0x06001A14 RID: 6676 RVA: 0x000AD444 File Offset: 0x000AB644
	protected Rect GetPlayerSplitScreenRect(GamePlayer player)
	{
		List<GamePlayer> localNonAIPlayers = GameManager.GetLocalNonAIPlayers();
		for (int i = 0; i < localNonAIPlayers.Count; i++)
		{
			if (localNonAIPlayers[i] == player)
			{
				return this.SplitScreenSizes[localNonAIPlayers.Count - 1][i];
			}
		}
		Debug.LogError("GetPlayerSplitScreenRect FAILED!");
		return new Rect(0f, 0f, 1f, 1f);
	}

	// Token: 0x06001A15 RID: 6677 RVA: 0x000AD4AC File Offset: 0x000AB6AC
	protected Rect GetPlayerSplitScreenRectWithAI(GamePlayer player)
	{
		List<GamePlayer> localPlayers = GameManager.GetLocalPlayers();
		for (int i = 0; i < localPlayers.Count; i++)
		{
			if (localPlayers[i] == player)
			{
				return this.SplitScreenSizes[localPlayers.Count - 1][i];
			}
		}
		Debug.LogError("GetPlayerSplitScreenRect FAILED!");
		return new Rect(0f, 0f, 1f, 1f);
	}

	// Token: 0x06001A16 RID: 6678 RVA: 0x000AD514 File Offset: 0x000AB714
	protected int GetIndex(GamePlayer player)
	{
		List<GamePlayer> localPlayers = GameManager.GetLocalPlayers();
		for (int i = 0; i < localPlayers.Count; i++)
		{
			if (localPlayers[i] == player)
			{
				return i;
			}
		}
		return 0;
	}

	// Token: 0x04001BBA RID: 7098
	public GameObject player_root;

	// Token: 0x04001BBB RID: 7099
	public bool doSpawning = true;

	// Token: 0x04001BBC RID: 7100
	public bool enableOutlineSource = true;

	// Token: 0x04001BBD RID: 7101
	protected Vector3 startPosition;

	// Token: 0x04001BBE RID: 7102
	protected Quaternion startRotation;

	// Token: 0x04001BBF RID: 7103
	protected CharacterController controller;

	// Token: 0x04001BC0 RID: 7104
	protected PlayerAnimation playerAnim;

	// Token: 0x04001BC1 RID: 7105
	protected PlayerCosmetics playerCosmetics;

	// Token: 0x04001BC2 RID: 7106
	protected bool isDead;

	// Token: 0x04001BC3 RID: 7107
	protected bool root_active;

	// Token: 0x04001BC4 RID: 7108
	protected short round_score;

	// Token: 0x04001BC5 RID: 7109
	protected GamePlayer player;

	// Token: 0x04001BC6 RID: 7110
	protected NavMeshAgent agent;

	// Token: 0x04001BC8 RID: 7112
	[NetSend(-1, NetSendOwner.SERVER, NetSendFlags.NONE)]
	public NetVar<short> score = new NetVar<short>(0);

	// Token: 0x04001BC9 RID: 7113
	protected const float s3 = 0.33333334f;

	// Token: 0x04001BCA RID: 7114
	protected const float bW2 = 0f;

	// Token: 0x04001BCB RID: 7115
	protected const float bH2 = 0f;

	// Token: 0x04001BCC RID: 7116
	protected const float bW = 0f;

	// Token: 0x04001BCD RID: 7117
	protected const float bH = 0f;

	// Token: 0x04001BCE RID: 7118
	protected Rect[][] SplitScreenSizes = new Rect[][]
	{
		new Rect[0],
		new Rect[]
		{
			new Rect(0f, 0.5f, 1f, 0.5f),
			new Rect(0f, 0f, 1f, 0.5f)
		},
		new Rect[]
		{
			new Rect(0f, 0.5f, 1f, 0.5f),
			new Rect(0f, 0f, 0.5f, 0.5f),
			new Rect(0.5f, 0f, 0.5f, 0.5f)
		},
		new Rect[]
		{
			new Rect(0f, 0.5f, 0.5f, 0.5f),
			new Rect(0.5f, 0.5f, 0.5f, 0.5f),
			new Rect(0f, 0f, 0.5f, 0.5f),
			new Rect(0.5f, 0f, 0.5f, 0.5f)
		},
		new Rect[]
		{
			new Rect(0f, 0.5f, 0.5f, 0.5f),
			new Rect(0.5f, 0.5f, 0.5f, 0.5f),
			new Rect(0f, 0f, 0.33333334f, 0.5f),
			new Rect(0.33333334f, 0f, 0.33333334f, 0.5f),
			new Rect(0.6666667f, 0f, 0.33333334f, 0.5f)
		},
		new Rect[]
		{
			new Rect(0f, 0.6666667f, 0.5f, 0.33333334f),
			new Rect(0.5f, 0.6666667f, 0.5f, 0.33333334f),
			new Rect(0f, 0.33333334f, 0.5f, 0.33333334f),
			new Rect(0.5f, 0.33333334f, 0.5f, 0.33333334f),
			new Rect(0f, 0f, 0.5f, 0.33333334f),
			new Rect(0.5f, 0f, 0.5f, 0.33333334f)
		},
		new Rect[]
		{
			new Rect(0f, 0.6666667f, 0.5f, 0.33333334f),
			new Rect(0.5f, 0.6666667f, 0.5f, 0.33333334f),
			new Rect(0f, 0.33333334f, 0.5f, 0.33333334f),
			new Rect(0.5f, 0.33333334f, 0.5f, 0.33333334f),
			new Rect(0f, 0f, 0.33333334f, 0.33333334f),
			new Rect(0.33333334f, 0f, 0.33333334f, 0.33333334f),
			new Rect(0.6666667f, 0f, 0.33333334f, 0.33333334f)
		},
		new Rect[]
		{
			new Rect(0f, 0.6666667f, 0.5f, 0.33333334f),
			new Rect(0.5f, 0.6666667f, 0.5f, 0.33333334f),
			new Rect(0f, 0.33333334f, 0.33333334f, 0.33333334f),
			new Rect(0.33333334f, 0.33333334f, 0.33333334f, 0.33333334f),
			new Rect(0.6666667f, 0.33333334f, 0.33333334f, 0.33333334f),
			new Rect(0f, 0f, 0.33333334f, 0.33333334f),
			new Rect(0.33333334f, 0f, 0.33333334f, 0.33333334f),
			new Rect(0.6666667f, 0f, 0.33333334f, 0.33333334f)
		}
	};
}
