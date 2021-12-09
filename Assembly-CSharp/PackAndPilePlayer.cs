using System;
using UnityEngine;
using ZP.Net;

// Token: 0x020001F1 RID: 497
public class PackAndPilePlayer : CharacterBase
{
	// Token: 0x1700014A RID: 330
	// (get) Token: 0x06000E76 RID: 3702 RVA: 0x0000CC38 File Offset: 0x0000AE38
	// (set) Token: 0x06000E77 RID: 3703 RVA: 0x0000CC40 File Offset: 0x0000AE40
	public int curY { get; set; }

	// Token: 0x1700014B RID: 331
	// (get) Token: 0x06000E78 RID: 3704 RVA: 0x0000CC49 File Offset: 0x0000AE49
	// (set) Token: 0x06000E79 RID: 3705 RVA: 0x0000CC51 File Offset: 0x0000AE51
	public bool finished { get; set; }

	// Token: 0x06000E7A RID: 3706 RVA: 0x00073750 File Offset: 0x00071950
	public override void OnNetInitialize()
	{
		base.OnNetInitialize();
		this.minigameController = (PackAndPileController)GameManager.Minigame;
		this.minigameController.AddPlayer(this);
		if (GameManager.GetPlayerCount() <= 4)
		{
			this.minigameController.Root.transform.Find("CameraParent").position = new Vector3(20.6f, 7.5f, 0f);
			this.minigameController.Root.transform.Find("CameraParent").rotation = Quaternion.Euler(6f, -90f, 0f);
		}
		else
		{
			this.minigameController.Root.transform.Find("CameraParent").position = new Vector3(22f, 11.5f, 0f);
			this.minigameController.Root.transform.Find("CameraParent").rotation = Quaternion.Euler(6f, -90f, 0f);
		}
		if (!base.IsOwner)
		{
			NetVar<byte> netVar = this.curPosition;
			netVar.Recieve = (RecieveProxy)Delegate.Combine(netVar.Recieve, new RecieveProxy(this.RecievePosition));
		}
		this.playerBoxMaterial = new Material(this.boxBaseMaterial);
		this.playerBoxMaterial.SetColor("_Color", this.player.Color.skinColor1);
	}

	// Token: 0x06000E7B RID: 3707 RVA: 0x000738B8 File Offset: 0x00071AB8
	private void Update()
	{
		if (this.minigameController.Playable)
		{
			if (!this.targeter.activeSelf)
			{
				this.targeter.SetActive(true);
			}
			if (Time.time - this.lastDrop >= this.dropWaitTime && !this.finished && base.IsOwner)
			{
				if (this.respawning)
				{
					this.respawning = false;
					this.curX = UnityEngine.Random.Range(0, 6);
					this.pingPong = (this.curX != 0 && (this.curX == 6 || UnityEngine.Random.value > 0.5f));
				}
				bool flag = false;
				this.elapsed += Time.deltaTime;
				if (this.elapsed >= this.interval)
				{
					this.elapsed -= this.interval;
					this.curX += (this.pingPong ? -1 : 1);
					if (this.curX == 6 || this.curX == 0)
					{
						this.pingPong = !this.pingPong;
					}
					if (this.AiPlaceTimer.Elapsed(false) && (((this.curY == 0 || this.array[this.curX, this.curY - 1]) && UnityEngine.Random.value < this.difficultyChance[(int)base.GamePlayer.Difficulty]) || UnityEngine.Random.value < this.difficultyChance2[(int)base.GamePlayer.Difficulty]))
					{
						flag = true;
						this.AiPlaceTimer.Start();
					}
				}
				if (!base.GamePlayer.IsAI)
				{
					if (base.GamePlayer.RewiredPlayer.GetButtonDown(InputActions.Accept))
					{
						this.PlaceBoxLocal();
					}
				}
				else if (flag)
				{
					this.PlaceBoxLocal();
				}
				this.curPosition.Value = (byte)(this.curX + this.curY * 7);
			}
			this.targeter.transform.position = this.GetPosition(this.curX, this.curY, false);
			if (this.curPosition.Value != this.lastPosition)
			{
				this.boxAnim.SetTrigger("Pulse");
				this.lastPosition = this.curPosition.Value;
			}
		}
	}

	// Token: 0x06000E7C RID: 3708 RVA: 0x00073AE8 File Offset: 0x00071CE8
	private Vector3 GetPosition(int x, int y, bool forcePosition = false)
	{
		if ((Time.time - this.lastDrop >= this.dropWaitTime && !this.finished) || forcePosition)
		{
			return ((base.OwnerSlot > 3) ? this.startPosTop : this.startPosBot) + (float)(base.OwnerSlot % 4) * Vector3.forward * 8f + Vector3.forward * (float)x + Vector3.up * (float)y;
		}
		return new Vector3(-20f, 0f, 0f);
	}

	// Token: 0x06000E7D RID: 3709 RVA: 0x00073B8C File Offset: 0x00071D8C
	private void PlaceBoxLocal()
	{
		int num = this.curY;
		while (num > 0 && !this.array[this.curX, num - 1])
		{
			num--;
		}
		base.SendRPC("RPCPlaceBox", NetRPCDelivery.RELIABLE_UNORDERED, new object[]
		{
			(byte)this.curX,
			(byte)this.curY,
			(byte)num
		});
		this.PlaceBox(this.curX, this.curY, num);
	}

	// Token: 0x06000E7E RID: 3710 RVA: 0x00073C10 File Offset: 0x00071E10
	private void PlaceBox(int startX, int startY, int targetY)
	{
		Vector3 position = this.GetPosition(startX, startY, true);
		Vector3 position2 = this.GetPosition(startX, targetY, true);
		this.minigameController.Spawn(this.placedBoxPrefab, position, Quaternion.identity).GetComponent<PackAndPileBox>().Spawn(position, position2, this.playerBoxMaterial);
		this.lastDrop = Time.time;
		if (base.IsOwner)
		{
			this.array[startX, targetY] = true;
			for (int i = 0; i < this.array.GetLength(0); i++)
			{
				for (int j = 0; j < this.array.GetLength(1); j++)
				{
					if (this.array[i, j])
					{
						this.curY = Mathf.Max(j + 1, this.curY);
					}
				}
			}
			this.interval = this.baseInterval - (float)this.curY * 0.0075f;
			this.respawning = true;
			if (base.IsOwner && this.curY >= 10)
			{
				this.Finish();
			}
		}
	}

	// Token: 0x06000E7F RID: 3711 RVA: 0x0000CC5A File Offset: 0x0000AE5A
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCPlaceBox(NetPlayer sender, byte startX, byte startY, byte targetY)
	{
		this.PlaceBox((int)startX, (int)startY, (int)targetY);
	}

	// Token: 0x06000E80 RID: 3712 RVA: 0x0000CC66 File Offset: 0x0000AE66
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCFinish(NetPlayer sender)
	{
		this.Finish();
	}

	// Token: 0x06000E81 RID: 3713 RVA: 0x00073D08 File Offset: 0x00071F08
	private void Finish()
	{
		if (this.finished)
		{
			return;
		}
		if (base.IsOwner)
		{
			base.SendRPC("RPCFinish", NetRPCDelivery.RELIABLE_ORDERED, Array.Empty<object>());
		}
		GameManager.UIController.SpawnWorldText("FINISHED", base.transform.position, 5f, WorldTextType.SwiftShootersGood, 0f, this.minigameController.MinigameCamera);
		if (NetSystem.IsServer)
		{
			this.Score += (short)(25 * (8 - this.minigameController.playersFinished));
			this.minigameController.playersFinished++;
		}
		this.finished = true;
	}

	// Token: 0x06000E82 RID: 3714 RVA: 0x0000CC6E File Offset: 0x0000AE6E
	private void RecievePosition(object pos)
	{
		this.curX = (int)((byte)pos % 7);
		this.curY = (int)((byte)pos / 7);
	}

	// Token: 0x04000DF6 RID: 3574
	public GameObject targeter;

	// Token: 0x04000DF7 RID: 3575
	public GameObject placedBoxPrefab;

	// Token: 0x04000DF8 RID: 3576
	public Animator boxAnim;

	// Token: 0x04000DF9 RID: 3577
	public Material boxBaseMaterial;

	// Token: 0x04000DFA RID: 3578
	private PackAndPileController minigameController;

	// Token: 0x04000DFB RID: 3579
	private Vector3 startPosBot = new Vector3(0f, 0f, -15f);

	// Token: 0x04000DFC RID: 3580
	private Vector3 startPosTop = new Vector3(0f, 11f, -15f);

	// Token: 0x04000DFD RID: 3581
	private float baseInterval = 0.1f;

	// Token: 0x04000DFE RID: 3582
	private float[] difficultyChance = new float[]
	{
		0.05f,
		0.1f,
		0.2f
	};

	// Token: 0x04000DFF RID: 3583
	private float[] difficultyChance2 = new float[]
	{
		0.05f,
		0.1f,
		0.2f
	};

	// Token: 0x04000E00 RID: 3584
	private ActionTimer AiPlaceTimer = new ActionTimer(1f, 4f);

	// Token: 0x04000E01 RID: 3585
	private bool pingPong;

	// Token: 0x04000E02 RID: 3586
	private bool[,] array = new bool[7, 10];

	// Token: 0x04000E03 RID: 3587
	private float elapsed;

	// Token: 0x04000E04 RID: 3588
	private float interval = 0.1f;

	// Token: 0x04000E05 RID: 3589
	private float lastDrop;

	// Token: 0x04000E06 RID: 3590
	private float dropWaitTime = 1.5f;

	// Token: 0x04000E07 RID: 3591
	private bool respawning;

	// Token: 0x04000E08 RID: 3592
	private int curX;

	// Token: 0x04000E0B RID: 3595
	public byte lastPosition;

	// Token: 0x04000E0C RID: 3596
	private Material playerBoxMaterial;

	// Token: 0x04000E0D RID: 3597
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.CHANGES_OFTEN)]
	public NetVar<byte> curPosition = new NetVar<byte>();
}
