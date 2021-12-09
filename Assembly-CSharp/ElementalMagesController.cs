using System;
using System.Collections.Generic;
using UnityEngine;
using ZP.Net;
using ZP.Utility;

// Token: 0x020001A2 RID: 418
public class ElementalMagesController : MinigameController
{
	// Token: 0x06000BF2 RID: 3058 RVA: 0x00064B50 File Offset: 0x00062D50
	public override void InitializeMinigame()
	{
		base.InitializeMinigame();
		if (NetSystem.IsServer)
		{
			base.SpawnPlayers("ElementalMagesPlayer", null);
		}
		this.territorySystem = base.Root.GetComponentInChildren<TerritorySystem>();
		for (int i = 0; i < 4; i++)
		{
			base.Root.transform.Find("StaticScene/Platforms/SM_Env_Tiles_Rune_01_" + i.ToString()).gameObject.GetComponent<MeshRenderer>().sharedMaterial = this.platformMats[i];
			base.Root.transform.Find("StaticScene/Platforms/PlatformLight" + i.ToString()).gameObject.GetComponent<Light>().color = this.platformLightColors[i];
		}
		this.cameraShake = base.Root.GetComponentInChildren<CameraShake>();
	}

	// Token: 0x06000BF3 RID: 3059 RVA: 0x0005B184 File Offset: 0x00059384
	public override void StartMinigame()
	{
		base.CreateTimer(UIAnchorType.TopRight, new Vector2(75f, 75f), this.round_length);
		base.CreateScoreUI(UIAnchorType.TopLeft, new Vector2(145f, 45f), 68f, false);
		for (int i = 0; i < this.ui_score.Length; i++)
		{
			if (!(this.ui_score[i] == null))
			{
				this.ui_score[i].scoreUpdateSpeed = 25;
			}
		}
		base.StartMinigame();
	}

	// Token: 0x06000BF4 RID: 3060 RVA: 0x0000A22E File Offset: 0x0000842E
	public override void RoundEnded()
	{
		base.RoundEnded();
	}

	// Token: 0x06000BF5 RID: 3061 RVA: 0x00009C2C File Offset: 0x00007E2C
	public override void ResetRound()
	{
		base.ResetRound();
	}

	// Token: 0x06000BF6 RID: 3062 RVA: 0x00009C34 File Offset: 0x00007E34
	public override void RoundStarting()
	{
		base.RoundStarting();
	}

	// Token: 0x06000BF7 RID: 3063 RVA: 0x00009C3C File Offset: 0x00007E3C
	public override void StartNewRound()
	{
		base.StartNewRound();
	}

	// Token: 0x06000BF8 RID: 3064 RVA: 0x00064C18 File Offset: 0x00062E18
	private void Update()
	{
		if (base.State == MinigameControllerState.Playing)
		{
			if (!this.gotAchievement)
			{
				for (int i = 0; i < GameManager.GetPlayerCount(); i++)
				{
					if (this.players[i].IsOwner && !this.players[i].GamePlayer.IsAI && this.territorySystem.GetTeamScore(i) > 50f)
					{
						PlatformAchievementManager.Instance.TriggerAchievement("ACH_ELEMENTAL_ESCALATION");
						this.gotAchievement = true;
					}
				}
			}
			if (NetSystem.IsServer)
			{
				if (this.ui_timer.time_test <= 0f)
				{
					base.EndRound(1f, 3f, false);
				}
				if (this.serverSendRate.Elapsed(true) && this.splats.Count > 0)
				{
					this.bitStream.Reset();
					this.bitStream.Write((short)this.splats.Count);
					this.bitStream.GetDataCopy();
					while (this.splats.Count > 0)
					{
						this.splats[0].WriteBytes(this.bitStream);
						this.splats.RemoveAt(0);
						this.count++;
					}
					byte[] dataCopy = this.bitStream.GetDataCopy();
					if (dataCopy.Length != 0)
					{
						base.SendRPC("RPCServerSendSplat", NetRPCDelivery.UNRELIABLE, new object[]
						{
							dataCopy
						});
					}
				}
				if (this.crystalSpawner.Elapsed(true))
				{
					this.SpawnCrystal(new Vector2(ZPMath.RandomFloat(GameManager.rand, -6f, 6f), ZPMath.RandomFloat(GameManager.rand, -6f, 6f)), this.crystalID);
					this.crystalID += 1;
				}
				float num = 2f;
				for (int j = 0; j < this.crystals.Count; j++)
				{
					bool flag = false;
					for (int k = 0; k < this.players.Count; k++)
					{
						if ((this.players[k].transform.position - this.crystals[j].gameObject.transform.position).sqrMagnitude < num)
						{
							flag = true;
							this.DespawnCrystal(this.crystals[j].id, (byte)this.players[k].OwnerSlot);
							break;
						}
					}
					if (flag)
					{
						break;
					}
				}
				if (this.scoreGetTimer.Elapsed(true))
				{
					for (int l = 0; l < GameManager.GetPlayerCount(); l++)
					{
						CharacterBase characterBase = this.players[l];
						characterBase.Score += (short)(0.5f * this.territorySystem.GetTeamScore(l));
					}
				}
			}
		}
	}

	// Token: 0x06000BF9 RID: 3065 RVA: 0x00009C44 File Offset: 0x00007E44
	public override void ReleaseMinigame()
	{
		base.ReleaseMinigame();
	}

	// Token: 0x06000BFA RID: 3066 RVA: 0x00009C4C File Offset: 0x00007E4C
	public override bool HasLoadedLocally()
	{
		return base.HasLoadedLocally();
	}

	// Token: 0x06000BFB RID: 3067 RVA: 0x0000B7FC File Offset: 0x000099FC
	[NetRPC(false, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void RPCSpawnCrystal(NetPlayer sender, Vector2 pos, ushort id)
	{
		this.SpawnCrystal(pos, id);
	}

	// Token: 0x06000BFC RID: 3068 RVA: 0x00064EE8 File Offset: 0x000630E8
	private void SpawnCrystal(Vector2 pos, ushort id)
	{
		GameObject gameObject = base.Spawn(this.crystalPrefab, new Vector3(pos.x, 1.25f, pos.y), Quaternion.identity);
		this.crystals.Add(new ElementalMagesController.Crystal(gameObject, id));
		if (NetSystem.IsServer)
		{
			base.SendRPC("RPCSpawnCrystal", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				pos,
				id
			});
		}
	}

	// Token: 0x06000BFD RID: 3069 RVA: 0x0000B806 File Offset: 0x00009A06
	[NetRPC(false, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void RPCDespawnCrystal(NetPlayer sender, ushort id, byte hitPlayerSlot)
	{
		this.DespawnCrystal(id, hitPlayerSlot);
	}

	// Token: 0x06000BFE RID: 3070 RVA: 0x00064F5C File Offset: 0x0006315C
	private void DespawnCrystal(ushort id, byte hitPlayerSlot)
	{
		for (int i = 0; i < this.crystals.Count; i++)
		{
			if (id == this.crystals[i].id)
			{
				this.crystals[i].gameObject.GetComponent<ElementalMagesCrystal>().Despawn(hitPlayerSlot);
				UnityEngine.Object.Instantiate<GameObject>(this.pickupParticle, this.crystals[i].gameObject.transform.position, Quaternion.identity);
				AudioSystem.PlayOneShot(this.crystalPickupClip, 0.85f, 0f, 1f);
				this.cameraShake.AddShake(0.35f);
				bool isServer = NetSystem.IsServer;
				this.crystals.RemoveAt(i);
				break;
			}
		}
		if (NetSystem.IsServer)
		{
			base.SendRPC("RPCDespawnCrystal", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				id,
				hitPlayerSlot
			});
		}
	}

	// Token: 0x06000BFF RID: 3071 RVA: 0x00065050 File Offset: 0x00063250
	[NetRPC(false, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void RPCServerSendSplat(NetPlayer sender, byte[] bytes)
	{
		ZPBitStream zpbitStream = new ZPBitStream(bytes, bytes.Length * 8);
		int num = (int)zpbitStream.ReadShort();
		for (int i = 0; i < num; i++)
		{
			ElementalMagesController.ElementalSplat splat = default(ElementalMagesController.ElementalSplat);
			splat.ReadBytes(zpbitStream);
			this.DoSplat(splat);
		}
	}

	// Token: 0x06000C00 RID: 3072 RVA: 0x00065094 File Offset: 0x00063294
	public void DoSplat(Vector3 position, float yRot, byte type, byte team)
	{
		ElementalMagesController.ElementalSplat splat = new ElementalMagesController.ElementalSplat(position, yRot, type, team);
		if (NetSystem.IsServer)
		{
			this.DoSplat(splat);
		}
	}

	// Token: 0x06000C01 RID: 3073 RVA: 0x000650BC File Offset: 0x000632BC
	public void DoSplat(ElementalMagesController.ElementalSplat splat)
	{
		if (NetSystem.IsServer)
		{
			this.splats.Add(splat);
		}
		Vector3 scale = Vector3.one * 4f;
		if (splat.Type == 1)
		{
			scale = this.attackSize / 2f;
		}
		else if (splat.Type == 2)
		{
			scale = Vector3.one * 15f;
		}
		this.territorySystem.ApplySplat(splat.Position, Quaternion.Euler(0f, 0f, -splat.YRot), scale, (int)splat.Team, splat.Type);
	}

	// Token: 0x06000C03 RID: 3075 RVA: 0x0000B810 File Offset: 0x00009A10
	// Note: this type is marked as 'beforefieldinit'.
	static ElementalMagesController()
	{
		float[] array = new float[4];
		array[0] = 5f;
		ElementalMagesController.possibleSizes = array;
		ElementalMagesController.min = -15f;
		ElementalMagesController.max = 15f;
		ElementalMagesController.splatY = 0.05f;
	}

	// Token: 0x04000B28 RID: 2856
	[Header("Minigame specific attributes")]
	public GameObject crystalPrefab;

	// Token: 0x04000B29 RID: 2857
	public Vector3 attackSize = new Vector3(0.9f, 10f, 1f);

	// Token: 0x04000B2A RID: 2858
	public AudioClip crystalPickupClip;

	// Token: 0x04000B2B RID: 2859
	public GameObject pickupParticle;

	// Token: 0x04000B2C RID: 2860
	public Material[] platformMats = new Material[4];

	// Token: 0x04000B2D RID: 2861
	public Color[] platformLightColors = new Color[4];

	// Token: 0x04000B2E RID: 2862
	private TerritorySystem territorySystem;

	// Token: 0x04000B2F RID: 2863
	private ActionTimer crystalSpawner = new ActionTimer(6f, 10f);

	// Token: 0x04000B30 RID: 2864
	private ActionTimer serverSendRate = new ActionTimer(0.01666667f);

	// Token: 0x04000B31 RID: 2865
	private CameraShake cameraShake;

	// Token: 0x04000B32 RID: 2866
	public List<ElementalMagesController.Crystal> crystals = new List<ElementalMagesController.Crystal>();

	// Token: 0x04000B33 RID: 2867
	private ushort crystalID;

	// Token: 0x04000B34 RID: 2868
	private ActionTimer scoreGetTimer = new ActionTimer(1f);

	// Token: 0x04000B35 RID: 2869
	private ZPBitStream bitStream = new ZPBitStream();

	// Token: 0x04000B36 RID: 2870
	private bool gotAchievement;

	// Token: 0x04000B37 RID: 2871
	private List<ElementalMagesController.ElementalSplat> splats = new List<ElementalMagesController.ElementalSplat>();

	// Token: 0x04000B38 RID: 2872
	private float time = float.MinValue;

	// Token: 0x04000B39 RID: 2873
	private int count;

	// Token: 0x04000B3A RID: 2874
	public static readonly float[] possibleSizes;

	// Token: 0x04000B3B RID: 2875
	public static readonly float min;

	// Token: 0x04000B3C RID: 2876
	public static readonly float max;

	// Token: 0x04000B3D RID: 2877
	public static readonly float splatY;

	// Token: 0x020001A3 RID: 419
	public class Crystal
	{
		// Token: 0x06000C04 RID: 3076 RVA: 0x0000B843 File Offset: 0x00009A43
		public Crystal(GameObject gameObject, ushort id)
		{
			this.gameObject = gameObject;
			this.id = id;
		}

		// Token: 0x04000B3E RID: 2878
		public GameObject gameObject;

		// Token: 0x04000B3F RID: 2879
		public ushort id;
	}

	// Token: 0x020001A4 RID: 420
	public struct ElementalSplat
	{
		// Token: 0x06000C05 RID: 3077 RVA: 0x00065204 File Offset: 0x00063404
		public ElementalSplat(Vector3 pos, float yRot, byte type, byte team)
		{
			this.teamAndType = 0;
			this.x = ZPMath.CompressFloatToByte(pos.x, ElementalMagesController.min, ElementalMagesController.max);
			this.z = ZPMath.CompressFloatToByte(pos.z, ElementalMagesController.min, ElementalMagesController.max);
			this.yRot = ZPMath.CompressFloatToShort(yRot, -360f, 360f);
			this.Type = type;
			this.Team = team;
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x06000C06 RID: 3078 RVA: 0x0000B859 File Offset: 0x00009A59
		public Vector3 Position
		{
			get
			{
				return new Vector3(ZPMath.DecompressByteToFloat(this.x, ElementalMagesController.min, ElementalMagesController.max), ElementalMagesController.splatY, ZPMath.DecompressByteToFloat(this.z, ElementalMagesController.min, ElementalMagesController.max));
			}
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06000C07 RID: 3079 RVA: 0x0000B88F File Offset: 0x00009A8F
		public float YRot
		{
			get
			{
				return ZPMath.DecompressShortToFloat(this.yRot, -360f, 360f);
			}
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06000C08 RID: 3080 RVA: 0x0000B8A6 File Offset: 0x00009AA6
		// (set) Token: 0x06000C09 RID: 3081 RVA: 0x0000B8B2 File Offset: 0x00009AB2
		public byte Type
		{
			get
			{
				return this.teamAndType & 15;
			}
			set
			{
				this.teamAndType = (byte)((int)this.Team << 4 | (int)(value & 15));
			}
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x06000C0A RID: 3082 RVA: 0x0000B8C8 File Offset: 0x00009AC8
		// (set) Token: 0x06000C0B RID: 3083 RVA: 0x0000B8D3 File Offset: 0x00009AD3
		public byte Team
		{
			get
			{
				return (byte)(this.teamAndType >> 4);
			}
			set
			{
				this.teamAndType = (byte)((int)value << 4 | (int)this.Type);
			}
		}

		// Token: 0x06000C0C RID: 3084 RVA: 0x0000B8E6 File Offset: 0x00009AE6
		public void WriteBytes(ZPBitStream bitStream)
		{
			bitStream.Write(this.x);
			bitStream.Write(this.z);
			bitStream.Write(this.teamAndType);
			if (this.Type == 1)
			{
				bitStream.Write(this.yRot);
			}
		}

		// Token: 0x06000C0D RID: 3085 RVA: 0x00065274 File Offset: 0x00063474
		public void ReadBytes(ZPBitStream bitStream)
		{
			this.x = bitStream.ReadByte();
			this.z = bitStream.ReadByte();
			this.teamAndType = bitStream.ReadByte();
			if (this.Type == 1)
			{
				this.yRot = bitStream.ReadShort();
				return;
			}
			this.yRot = ZPMath.CompressFloatToShort(0f, -360f, 360f);
		}

		// Token: 0x04000B40 RID: 2880
		private byte x;

		// Token: 0x04000B41 RID: 2881
		private byte z;

		// Token: 0x04000B42 RID: 2882
		private short yRot;

		// Token: 0x04000B43 RID: 2883
		private byte teamAndType;
	}
}
