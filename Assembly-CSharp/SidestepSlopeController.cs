using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using LlockhamIndustries.Decals;
using UnityEngine;
using ZP.Net;
using ZP.Utility;

// Token: 0x02000239 RID: 569
public class SidestepSlopeController : MinigameController
{
	// Token: 0x17000174 RID: 372
	// (get) Token: 0x06001073 RID: 4211 RVA: 0x0000DCF1 File Offset: 0x0000BEF1
	// (set) Token: 0x06001074 RID: 4212 RVA: 0x0000DCF9 File Offset: 0x0000BEF9
	public Transform ScoreTransform { get; set; }

	// Token: 0x17000175 RID: 373
	// (get) Token: 0x06001075 RID: 4213 RVA: 0x0000DD02 File Offset: 0x0000BF02
	public List<SidestepSlopeController.SpawnedVehicle> SpawnedVehicles
	{
		get
		{
			return this.spawnedVehicles;
		}
	}

	// Token: 0x06001076 RID: 4214 RVA: 0x0000DD0A File Offset: 0x0000BF0A
	public void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireCube(this.b.center, this.b.extents * 2f);
	}

	// Token: 0x06001077 RID: 4215 RVA: 0x000812C4 File Offset: 0x0007F4C4
	public override void InitializeMinigame()
	{
		if (NetSystem.IsServer)
		{
			base.SpawnPlayers("SidestepSlopePlayer", null);
		}
		for (int i = 0; i < 8; i++)
		{
			if (GameManager.IsPlayerInSlot(i))
			{
				this.scoreLines[i] = this.minigame_root.transform.Find("Environment/Tilted/Line" + i.ToString());
				ProjectionRenderer component = this.scoreLines[i].gameObject.GetComponent<ProjectionRenderer>();
				component.SetColor(1, GameManager.GetPlayerAt(i).Color.skinColor1);
				component.UpdateProperties();
			}
		}
		this.ScoreTransform = this.minigame_root.transform.Find("Environment/Tilted");
		base.InitializeMinigame();
	}

	// Token: 0x06001078 RID: 4216 RVA: 0x0007DE74 File Offset: 0x0007C074
	public override void StartMinigame()
	{
		base.CreateTimer(UIAnchorType.TopRight, new Vector2(75f, 75f), this.round_length);
		base.CreateScoreUI(UIAnchorType.TopLeft, new Vector2(145f, 45f), 68f, false);
		for (int i = 0; i < this.ui_score.Length; i++)
		{
			if (!(this.ui_score[i] == null))
			{
				this.ui_score[i].scoreUpdateSpeed = 100;
				this.ui_score[i].minChangeText = 10;
			}
		}
		base.StartMinigame();
	}

	// Token: 0x06001079 RID: 4217 RVA: 0x00081370 File Offset: 0x0007F570
	public override void OnPlayersReady()
	{
		if (NetSystem.IsServer)
		{
			int num = GameManager.rand.Next(0, this.binaryFiles.Length);
			base.StartCoroutine(this.DoReplay(num));
			base.SendRPC("RPCStartReplay", NetRPCDelivery.RELIABLE_UNORDERED, new object[]
			{
				num
			});
		}
		base.OnPlayersReady();
		GameManager.CaptureInput = true;
	}

	// Token: 0x0600107A RID: 4218 RVA: 0x0000DD3B File Offset: 0x0000BF3B
	[NetRPC(false, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void RPCStartReplay(NetPlayer sender, int index)
	{
		base.StartCoroutine(this.DoReplay(index));
	}

	// Token: 0x0600107B RID: 4219 RVA: 0x00009C34 File Offset: 0x00007E34
	public override void RoundStarting()
	{
		base.RoundStarting();
	}

	// Token: 0x0600107C RID: 4220 RVA: 0x00009C3C File Offset: 0x00007E3C
	public override void StartNewRound()
	{
		base.StartNewRound();
	}

	// Token: 0x0600107D RID: 4221 RVA: 0x00009C2C File Offset: 0x00007E2C
	public override void ResetRound()
	{
		base.ResetRound();
	}

	// Token: 0x0600107E RID: 4222 RVA: 0x0000A22E File Offset: 0x0000842E
	public override void RoundEnded()
	{
		base.RoundEnded();
	}

	// Token: 0x0600107F RID: 4223 RVA: 0x000813D0 File Offset: 0x0007F5D0
	private void Update()
	{
		if (base.State == MinigameControllerState.Playing)
		{
			for (int i = 0; i < 8; i++)
			{
				if (GameManager.IsPlayerInSlot(i))
				{
					this.scoreLines[i].transform.localPosition = new Vector3(((SidestepSlopePlayer)base.GetPlayer(i)).furthest.Value, 0.09f, 0f);
				}
			}
			if (NetSystem.IsServer && this.ui_timer.time_test <= 0f)
			{
				base.EndRound(1f, 3f, false);
			}
		}
		if (this.doingReplay)
		{
			foreach (SidestepSlopeController.SpawnedVehicle spawnedVehicle in this.spawnedVehicles)
			{
				if (!(spawnedVehicle.g == null) && !(spawnedVehicle.nextFrame.pos == Vector3.zero) && !(spawnedVehicle.lastFrame.pos == Vector3.zero))
				{
					float t = Mathf.Clamp01((Time.time - spawnedVehicle.lastUpdate) / (Time.fixedDeltaTime * 4f));
					spawnedVehicle.t.position = Vector3.Lerp(spawnedVehicle.lastFrame.pos, spawnedVehicle.nextFrame.pos, t);
					spawnedVehicle.t.rotation = Quaternion.Lerp(Quaternion.Euler(spawnedVehicle.lastFrame.rot), Quaternion.Euler(spawnedVehicle.nextFrame.rot), t);
				}
			}
		}
	}

	// Token: 0x06001080 RID: 4224 RVA: 0x0000DD4B File Offset: 0x0000BF4B
	private IEnumerator DoReplay(int file)
	{
		this.frame = 0;
		this.doingReplay = true;
		this.spawnedVehicles.Clear();
		BinaryReader br = new BinaryReader(new MemoryStream(this.binaryFiles[file].bytes));
		while (br.PeekChar() != -1 && base.State != MinigameControllerState.RoundResetWait && base.State != MinigameControllerState.ShowScoreScreen)
		{
			this.frame++;
			if (this.frame % 4 != 0)
			{
				yield return new WaitForFixedUpdate();
			}
			else
			{
				int num = (int)br.ReadByte();
				for (int i = 0; i < num; i++)
				{
					GameObject gameObject = this.SpawnVehicle(new SidestepSlopeController.VehSpawn(br));
					gameObject.GetComponent<Rigidbody>().isKinematic = true;
					gameObject.GetComponent<CollisionHandler>().replaying = true;
				}
				int num2 = (int)br.ReadByte();
				for (int j = 0; j < num2; j++)
				{
					byte index = br.ReadByte();
					if (this.spawnedVehicles[(int)index].ch != null)
					{
						this.spawnedVehicles[(int)index].ch.StartDestroy();
					}
					this.spawnedVehicles.RemoveAt((int)index);
				}
				int num3 = (int)br.ReadByte();
				for (int k = 0; k < num3; k++)
				{
					this.DoCollision(new SidestepSlopeController.VehContact(br));
				}
				int num4 = (int)br.ReadByte();
				for (int l = 0; l < num4; l++)
				{
					if (l < this.spawnedVehicles.Count)
					{
						SidestepSlopeController.VehFrame vehFrame = new SidestepSlopeController.VehFrame(br);
						SidestepSlopeController.SpawnedVehicle value = this.spawnedVehicles[l];
						value.Update(vehFrame);
						this.spawnedVehicles[l] = value;
					}
				}
				yield return new WaitForFixedUpdate();
			}
		}
		this.doingReplay = true;
		yield break;
	}

	// Token: 0x06001081 RID: 4225 RVA: 0x00081564 File Offset: 0x0007F764
	private void DoCollision(SidestepSlopeController.VehContact contact)
	{
		UnityEngine.Object.Instantiate<GameObject>(this.sparksParticle, contact.pos + contact.norm * 0.05f, Quaternion.LookRotation(contact.norm, Vector3.forward));
		AudioSystem.PlayOneShot(this.hit, contact.pos, 0.3f, AudioRolloffMode.Logarithmic, 12f, 120f, 0f);
	}

	// Token: 0x06001082 RID: 4226 RVA: 0x000815D0 File Offset: 0x0007F7D0
	private GameObject SpawnVehicle(SidestepSlopeController.VehSpawn spawn)
	{
		GameObject gameObject;
		if (GameManager.Minigame != null)
		{
			gameObject = base.Spawn(this.prefabs[(int)spawn.index], spawn.pos, spawn.rot);
		}
		else
		{
			gameObject = UnityEngine.Object.Instantiate<GameObject>(this.prefabs[(int)spawn.index], spawn.pos, spawn.rot);
		}
		this.spawnedVehicles.Add(new SidestepSlopeController.SpawnedVehicle(gameObject));
		return gameObject;
	}

	// Token: 0x06001083 RID: 4227 RVA: 0x0000A3B0 File Offset: 0x000085B0
	public override void ReleaseMinigame()
	{
		GameManager.CaptureInput = false;
		base.ReleaseMinigame();
	}

	// Token: 0x06001084 RID: 4228 RVA: 0x00009C4C File Offset: 0x00007E4C
	public override bool HasLoadedLocally()
	{
		return base.HasLoadedLocally();
	}

	// Token: 0x06001085 RID: 4229 RVA: 0x0000A236 File Offset: 0x00008436
	public override void BuildResults()
	{
		base.BuildResults();
	}

	// Token: 0x040010DD RID: 4317
	[Header("Sidestep Slope Attributes")]
	public GameObject camPrefab;

	// Token: 0x040010DE RID: 4318
	[Header("Spawning")]
	public float minSpawnInterval = 0.2f;

	// Token: 0x040010DF RID: 4319
	public float maxSpawnInterval = 0.1f;

	// Token: 0x040010E0 RID: 4320
	public Bounds b;

	// Token: 0x040010E1 RID: 4321
	public Vector3 maxVel;

	// Token: 0x040010E2 RID: 4322
	public Vector3 minVel;

	// Token: 0x040010E3 RID: 4323
	public Vector3 maxAngVel;

	// Token: 0x040010E4 RID: 4324
	public Vector3 minAngVel;

	// Token: 0x040010E5 RID: 4325
	public float[] spawnChances;

	// Token: 0x040010E6 RID: 4326
	public GameObject[] prefabs;

	// Token: 0x040010E7 RID: 4327
	public GameObject sparksParticle;

	// Token: 0x040010E8 RID: 4328
	public AudioClip hit;

	// Token: 0x040010E9 RID: 4329
	public TextAsset[] binaryFiles;

	// Token: 0x040010EA RID: 4330
	private Transform[] scoreLines = new Transform[8];

	// Token: 0x040010EC RID: 4332
	private BinaryTree bt;

	// Token: 0x040010ED RID: 4333
	private float totalChance;

	// Token: 0x040010EE RID: 4334
	private List<SidestepSlopeController.SpawnedVehicle> spawnedVehicles = new List<SidestepSlopeController.SpawnedVehicle>();

	// Token: 0x040010EF RID: 4335
	private bool doingReplay;

	// Token: 0x040010F0 RID: 4336
	private int frame;

	// Token: 0x0200023A RID: 570
	public struct SpawnedVehicle
	{
		// Token: 0x06001087 RID: 4231 RVA: 0x00081640 File Offset: 0x0007F840
		public SpawnedVehicle(GameObject g)
		{
			this.g = g;
			this.t = g.transform;
			this.ch = g.GetComponent<CollisionHandler>();
			this.lastPos = this.t.position;
			this.lastRot = this.t.rotation.eulerAngles;
			this.lastFrame = default(SidestepSlopeController.VehFrame);
			this.nextFrame = default(SidestepSlopeController.VehFrame);
			this.lastUpdate = Time.time;
		}

		// Token: 0x06001088 RID: 4232 RVA: 0x000816BC File Offset: 0x0007F8BC
		public void Update(SidestepSlopeController.VehFrame frame)
		{
			if (this.ch != null)
			{
				this.ch.colliding = frame.contacting;
			}
			if (this.nextFrame.pos != Vector3.zero && this.t != null)
			{
				this.t.position = this.nextFrame.pos;
				this.t.rotation = Quaternion.Euler(this.nextFrame.rot);
			}
			this.lastFrame = this.nextFrame;
			this.nextFrame = frame;
			this.lastUpdate = Time.time;
		}

		// Token: 0x040010F1 RID: 4337
		public GameObject g;

		// Token: 0x040010F2 RID: 4338
		public Transform t;

		// Token: 0x040010F3 RID: 4339
		public CollisionHandler ch;

		// Token: 0x040010F4 RID: 4340
		public Vector3 lastPos;

		// Token: 0x040010F5 RID: 4341
		public Vector3 lastRot;

		// Token: 0x040010F6 RID: 4342
		public SidestepSlopeController.VehFrame lastFrame;

		// Token: 0x040010F7 RID: 4343
		public SidestepSlopeController.VehFrame nextFrame;

		// Token: 0x040010F8 RID: 4344
		public float lastUpdate;
	}

	// Token: 0x0200023B RID: 571
	private struct VehSpawn
	{
		// Token: 0x06001089 RID: 4233 RVA: 0x0008175C File Offset: 0x0007F95C
		public VehSpawn(BinaryReader br)
		{
			this.index = br.ReadByte();
			this.pos = new Vector3(ZPMath.UShortToFloat(br.ReadUInt16(), SidestepSlopeController.VehSpawn.minPos.x, SidestepSlopeController.VehSpawn.maxPos.x), ZPMath.UShortToFloat(br.ReadUInt16(), SidestepSlopeController.VehSpawn.minPos.y, SidestepSlopeController.VehSpawn.maxPos.y), ZPMath.UShortToFloat(br.ReadUInt16(), SidestepSlopeController.VehSpawn.minPos.z, SidestepSlopeController.VehSpawn.maxPos.z));
			this.rot = Quaternion.Euler(new Vector3(ZPMath.ByteToFloat(br.ReadByte(), SidestepSlopeController.VehSpawn.minRot.x, SidestepSlopeController.VehSpawn.maxRot.x), ZPMath.ByteToFloat(br.ReadByte(), SidestepSlopeController.VehSpawn.minRot.y, SidestepSlopeController.VehSpawn.maxRot.y), ZPMath.ByteToFloat(br.ReadByte(), SidestepSlopeController.VehSpawn.minRot.z, SidestepSlopeController.VehSpawn.maxRot.z)));
		}

		// Token: 0x0600108A RID: 4234 RVA: 0x0008184C File Offset: 0x0007FA4C
		public void Serialize(BinaryWriter bw)
		{
			bw.Write(this.index);
			bw.Write(ZPMath.FloatToUShort(this.pos.x, SidestepSlopeController.VehSpawn.minPos.x, SidestepSlopeController.VehSpawn.maxPos.x));
			bw.Write(ZPMath.FloatToUShort(this.pos.y, SidestepSlopeController.VehSpawn.minPos.y, SidestepSlopeController.VehSpawn.maxPos.y));
			bw.Write(ZPMath.FloatToUShort(this.pos.z, SidestepSlopeController.VehSpawn.minPos.z, SidestepSlopeController.VehSpawn.maxPos.z));
			Vector3 eulerAngles = this.rot.eulerAngles;
			bw.Write(ZPMath.FloatToByte(eulerAngles.x, SidestepSlopeController.VehSpawn.minRot.x, SidestepSlopeController.VehSpawn.maxRot.x));
			bw.Write(ZPMath.FloatToByte(eulerAngles.y, SidestepSlopeController.VehSpawn.minRot.y, SidestepSlopeController.VehSpawn.maxRot.y));
			bw.Write(ZPMath.FloatToByte(eulerAngles.z, SidestepSlopeController.VehSpawn.minRot.z, SidestepSlopeController.VehSpawn.maxRot.z));
		}

		// Token: 0x040010F9 RID: 4345
		public byte index;

		// Token: 0x040010FA RID: 4346
		public Vector3 pos;

		// Token: 0x040010FB RID: 4347
		public Quaternion rot;

		// Token: 0x040010FC RID: 4348
		private static Vector3 minPos = new Vector3(-140f, -8f, -30f);

		// Token: 0x040010FD RID: 4349
		private static Vector3 maxPos = new Vector3(30f, 100f, 30f);

		// Token: 0x040010FE RID: 4350
		private static Vector3 minRot = new Vector3(0f, 0f, 0f);

		// Token: 0x040010FF RID: 4351
		private static Vector3 maxRot = new Vector3(360f, 360f, 360f);
	}

	// Token: 0x0200023C RID: 572
	private struct VehContact
	{
		// Token: 0x0600108C RID: 4236 RVA: 0x000819D4 File Offset: 0x0007FBD4
		public VehContact(BinaryReader br)
		{
			this.pos = new Vector3(ZPMath.UShortToFloat(br.ReadUInt16(), SidestepSlopeController.VehContact.minPos.x, SidestepSlopeController.VehContact.maxPos.x), ZPMath.UShortToFloat(br.ReadUInt16(), SidestepSlopeController.VehContact.minPos.y, SidestepSlopeController.VehContact.maxPos.y), ZPMath.UShortToFloat(br.ReadUInt16(), SidestepSlopeController.VehContact.minPos.z, SidestepSlopeController.VehContact.maxPos.z));
			this.norm = new Vector3(ZPMath.ByteToFloat(br.ReadByte(), -1f, 1f), ZPMath.ByteToFloat(br.ReadByte(), -1f, 1f), ZPMath.ByteToFloat(br.ReadByte(), -1f, 1f));
		}

		// Token: 0x0600108D RID: 4237 RVA: 0x00081A94 File Offset: 0x0007FC94
		public void Serialize(BinaryWriter bw)
		{
			bw.Write(ZPMath.FloatToUShort(this.pos.x, SidestepSlopeController.VehContact.minPos.x, SidestepSlopeController.VehContact.maxPos.x));
			bw.Write(ZPMath.FloatToUShort(this.pos.y, SidestepSlopeController.VehContact.minPos.y, SidestepSlopeController.VehContact.maxPos.y));
			bw.Write(ZPMath.FloatToUShort(this.pos.z, SidestepSlopeController.VehContact.minPos.z, SidestepSlopeController.VehContact.maxPos.z));
			bw.Write(ZPMath.FloatToByte(this.pos.x, -1f, 1f));
			bw.Write(ZPMath.FloatToByte(this.pos.y, -1f, 1f));
			bw.Write(ZPMath.FloatToByte(this.pos.z, -1f, 1f));
		}

		// Token: 0x04001100 RID: 4352
		public Vector3 pos;

		// Token: 0x04001101 RID: 4353
		public Vector3 norm;

		// Token: 0x04001102 RID: 4354
		private static Vector3 minPos = new Vector3(-140f, -8f, -30f);

		// Token: 0x04001103 RID: 4355
		private static Vector3 maxPos = new Vector3(30f, 100f, 30f);
	}

	// Token: 0x0200023D RID: 573
	public struct VehFrame
	{
		// Token: 0x0600108F RID: 4239 RVA: 0x00081B80 File Offset: 0x0007FD80
		public VehFrame(SidestepSlopeController.SpawnedVehicle veh)
		{
			this.contacting = veh.ch.colliding;
			this.pos = veh.t.position;
			this.rot = veh.t.rotation.eulerAngles;
		}

		// Token: 0x06001090 RID: 4240 RVA: 0x00081BC8 File Offset: 0x0007FDC8
		public VehFrame(BinaryReader br)
		{
			this.contacting = br.ReadBoolean();
			this.pos = Vector3.zero;
			this.rot = Vector3.zero;
			this.pos = new Vector3(ZPMath.UShortToFloat(br.ReadUInt16(), SidestepSlopeController.VehFrame.minPos.x, SidestepSlopeController.VehFrame.maxPos.x), ZPMath.UShortToFloat(br.ReadUInt16(), SidestepSlopeController.VehFrame.minPos.y, SidestepSlopeController.VehFrame.maxPos.y), ZPMath.UShortToFloat(br.ReadUInt16(), SidestepSlopeController.VehFrame.minPos.z, SidestepSlopeController.VehFrame.maxPos.z));
			this.rot = new Vector3(ZPMath.ByteToFloat(br.ReadByte(), SidestepSlopeController.VehFrame.minRot.x, SidestepSlopeController.VehFrame.maxRot.x), ZPMath.ByteToFloat(br.ReadByte(), SidestepSlopeController.VehFrame.minRot.y, SidestepSlopeController.VehFrame.maxRot.y), ZPMath.ByteToFloat(br.ReadByte(), SidestepSlopeController.VehFrame.minRot.z, SidestepSlopeController.VehFrame.maxRot.z));
		}

		// Token: 0x06001091 RID: 4241 RVA: 0x00081CC8 File Offset: 0x0007FEC8
		public void Serialize(BinaryWriter bw)
		{
			bw.Write(this.contacting);
			bw.Write(ZPMath.FloatToUShort(this.pos.x, SidestepSlopeController.VehFrame.minPos.x, SidestepSlopeController.VehFrame.maxPos.x));
			bw.Write(ZPMath.FloatToUShort(this.pos.y, SidestepSlopeController.VehFrame.minPos.y, SidestepSlopeController.VehFrame.maxPos.y));
			bw.Write(ZPMath.FloatToUShort(this.pos.z, SidestepSlopeController.VehFrame.minPos.z, SidestepSlopeController.VehFrame.maxPos.z));
			bw.Write(ZPMath.FloatToByte(this.rot.x, SidestepSlopeController.VehFrame.minRot.x, SidestepSlopeController.VehFrame.maxRot.x));
			bw.Write(ZPMath.FloatToByte(this.rot.y, SidestepSlopeController.VehFrame.minRot.y, SidestepSlopeController.VehFrame.maxRot.y));
			bw.Write(ZPMath.FloatToByte(this.rot.z, SidestepSlopeController.VehFrame.minRot.z, SidestepSlopeController.VehFrame.maxRot.z));
		}

		// Token: 0x04001104 RID: 4356
		public bool contacting;

		// Token: 0x04001105 RID: 4357
		public Vector3 pos;

		// Token: 0x04001106 RID: 4358
		public Vector3 rot;

		// Token: 0x04001107 RID: 4359
		private static Vector3 minPos = new Vector3(-140f, -8f, -30f);

		// Token: 0x04001108 RID: 4360
		private static Vector3 maxPos = new Vector3(30f, 100f, 30f);

		// Token: 0x04001109 RID: 4361
		private static Vector3 minRot = new Vector3(0f, 0f, 0f);

		// Token: 0x0400110A RID: 4362
		private static Vector3 maxRot = new Vector3(360f, 360f, 360f);
	}
}
