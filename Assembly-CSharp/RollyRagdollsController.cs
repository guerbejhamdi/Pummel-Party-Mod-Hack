using System;
using System.Collections;
using UnityEngine;
using ZP.Net;

// Token: 0x02000224 RID: 548
public class RollyRagdollsController : MinigameController
{
	// Token: 0x17000164 RID: 356
	// (get) Token: 0x06000FE8 RID: 4072 RVA: 0x0000D942 File Offset: 0x0000BB42
	// (set) Token: 0x06000FE9 RID: 4073 RVA: 0x0000D94A File Offset: 0x0000BB4A
	public int DeadCount { get; set; }

	// Token: 0x17000165 RID: 357
	// (get) Token: 0x06000FEA RID: 4074 RVA: 0x0000D953 File Offset: 0x0000BB53
	// (set) Token: 0x06000FEB RID: 4075 RVA: 0x0000D95B File Offset: 0x0000BB5B
	public int FinishedCount { get; set; }

	// Token: 0x17000166 RID: 358
	// (get) Token: 0x06000FEC RID: 4076 RVA: 0x0000D964 File Offset: 0x0000BB64
	public float CurrentTime
	{
		get
		{
			if (NetSystem.IsServer)
			{
				return this.curTime;
			}
			if (!this.gotTime && base.State == MinigameControllerState.Playing)
			{
				this.clientTime += Time.fixedDeltaTime;
				this.gotTime = true;
			}
			return this.clientTime;
		}
	}

	// Token: 0x06000FED RID: 4077 RVA: 0x0007DD90 File Offset: 0x0007BF90
	public override void InitializeMinigame()
	{
		base.InitializeMinigame();
		if (NetSystem.IsServer)
		{
			base.SpawnPlayers("RollyRagdollsPlayer", null);
			this.Setup(GameManager.rand.Next());
			return;
		}
		NetVar<float> netVar = this.time;
		netVar.Recieve = (RecieveProxy)Delegate.Combine(netVar.Recieve, new RecieveProxy(this.RecieveTime));
	}

	// Token: 0x06000FEE RID: 4078 RVA: 0x0000D9A4 File Offset: 0x0000BBA4
	private void Setup(int seed)
	{
		if (NetSystem.IsServer)
		{
			base.SendRPC("RPCSetup", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				seed
			});
		}
		this.rand = new System.Random(seed);
		base.StartCoroutine(this.SetupRoutine());
	}

	// Token: 0x06000FEF RID: 4079 RVA: 0x0000D9E1 File Offset: 0x0000BBE1
	private IEnumerator SetupRoutine()
	{
		yield return new WaitUntil(() => base.Root != null);
		if (this.minigameCameras[0] != null)
		{
			this.camTransform = this.minigameCameras[0].transform.parent;
		}
		if (this.camTransform != null)
		{
			this.cameraXStart = this.camTransform.position.x;
		}
		this.fillerRoot = base.Root.transform.Find("World/FillerRoot");
		this.obstacleRoot = base.Root.transform.Find("World/ObstacleRoot");
		this.spikeTransform = base.Root.transform.Find("SpikeWall");
		this.spikeTransformParent = base.Root.transform.Find("SpikeWall/Parent");
		base.Root.transform.Find("World/EndArea").position = this.fillerStartPos + this.fillerOffset * (float)RollyRagdollsController.obstacleRows;
		LeanTween.moveLocalX(this.spikeTransformParent.gameObject, -0.5f, 0.5f).setLoopPingPong().setEaseInOutSine();
		for (int i = 0; i < RollyRagdollsController.obstacleRows; i++)
		{
			base.Spawn(this.fillerPrefabs[this.rand.Next(0, this.fillerPrefabs.Length)], this.fillerStartPos + this.fillerOffset * (float)i, Quaternion.identity).transform.parent = this.fillerRoot;
			this.AddObstacle(i, false);
			this.AddObstacle(i, true);
		}
		yield break;
	}

	// Token: 0x06000FF0 RID: 4080 RVA: 0x0007DDF0 File Offset: 0x0007BFF0
	private void AddObstacle(int row, bool top)
	{
		GameObject gameObject = base.Spawn(this.obstaclePrefabs[this.rand.Next(0, this.obstaclePrefabs.Length)], (top ? this.obstacleStartPosTop : this.obstacleStartPosBottom) + this.fillerOffset * (float)row, Quaternion.identity);
		gameObject.transform.parent = this.obstacleRoot;
		RollyRagdollObstacle component = gameObject.GetComponent<RollyRagdollObstacle>();
		if (component != null)
		{
			component.Setup(this.rand);
		}
	}

	// Token: 0x06000FF1 RID: 4081 RVA: 0x0007DE74 File Offset: 0x0007C074
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

	// Token: 0x06000FF2 RID: 4082 RVA: 0x0007DF00 File Offset: 0x0007C100
	public void Update()
	{
		if (base.State == MinigameControllerState.Playing && NetSystem.IsServer)
		{
			this.curTime += Time.deltaTime;
			this.time.Value = this.curTime;
			int num = this.FinishedCount + this.DeadCount;
			if (this.ui_timer.time_test <= 0f || num >= base.GetPlayerCount() - 1)
			{
				base.EndRound(1f, 3f, false);
			}
		}
	}

	// Token: 0x06000FF3 RID: 4083 RVA: 0x0007DF7C File Offset: 0x0007C17C
	public void FixedUpdate()
	{
		this.gotTime = false;
		if (base.State == MinigameControllerState.Playing && this.camTransform != null)
		{
			int num = (int)(this.CurrentTime / Time.fixedDeltaTime);
			float num2 = this.cameraXStart;
			float num3 = this.moveSpeed;
			for (int i = 0; i < num; i++)
			{
				num3 = Mathf.Clamp(num3 + 0.0005f, 0f, 2.35f);
				num2 += Time.fixedDeltaTime * num3;
			}
			float num4 = Mathf.Clamp(num2 - 14.65f, float.MinValue, 138.5f);
			num2 = Mathf.Clamp(num2, float.MinValue, 142.5f);
			this.camTransform.position = new Vector3(this.camTransform.position.x + (num2 - this.camTransform.position.x) * 0.05f, this.camTransform.position.y, this.camTransform.position.z);
			this.spikeTransform.position = new Vector3(this.spikeTransform.position.x + (num4 - this.spikeTransform.position.x) * 0.05f, this.spikeTransform.position.y, this.spikeTransform.position.z);
		}
	}

	// Token: 0x06000FF4 RID: 4084 RVA: 0x0000D9F0 File Offset: 0x0000BBF0
	public void RecieveTime(object val)
	{
		this.gotTime = true;
		this.clientTime = (float)val;
	}

	// Token: 0x06000FF5 RID: 4085 RVA: 0x0000DA05 File Offset: 0x0000BC05
	[NetRPC(true, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void RPCSetup(NetPlayer sender, int seed)
	{
		this.Setup(seed);
	}

	// Token: 0x04001020 RID: 4128
	private float clientTime;

	// Token: 0x04001021 RID: 4129
	private bool gotTime;

	// Token: 0x04001022 RID: 4130
	public GameObject[] obstaclePrefabs;

	// Token: 0x04001023 RID: 4131
	public GameObject[] fillerPrefabs;

	// Token: 0x04001024 RID: 4132
	public float distFromCameraDeath = 14f;

	// Token: 0x04001025 RID: 4133
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.ALWAYS_SEND)]
	public NetVar<float> time = new NetVar<float>(0f);

	// Token: 0x04001026 RID: 4134
	public static readonly float minX = -7.5f;

	// Token: 0x04001027 RID: 4135
	public static readonly float maxX = 150f;

	// Token: 0x04001028 RID: 4136
	public static readonly float minY = -10f;

	// Token: 0x04001029 RID: 4137
	public static readonly float maxY = 10f;

	// Token: 0x0400102A RID: 4138
	public static readonly float minZ = -6.5f;

	// Token: 0x0400102B RID: 4139
	public static readonly float maxZ = 6.5f;

	// Token: 0x0400102C RID: 4140
	public static readonly int obstacleRows = 20;

	// Token: 0x0400102D RID: 4141
	private Vector3 fillerStartPos = new Vector3(4f, 0f, 0f);

	// Token: 0x0400102E RID: 4142
	[HideInInspector]
	public Vector3 fillerOffset = new Vector3(7f, 0f, 0f);

	// Token: 0x0400102F RID: 4143
	private Vector3 obstacleStartPosTop = new Vector3(4f, -0.85f, -3f);

	// Token: 0x04001030 RID: 4144
	private Vector3 obstacleStartPosBottom = new Vector3(4f, -0.85f, 3f);

	// Token: 0x04001031 RID: 4145
	private Transform fillerRoot;

	// Token: 0x04001032 RID: 4146
	private Transform obstacleRoot;

	// Token: 0x04001033 RID: 4147
	private System.Random rand;

	// Token: 0x04001034 RID: 4148
	private Transform camTransform;

	// Token: 0x04001035 RID: 4149
	private float cameraXStart;

	// Token: 0x04001036 RID: 4150
	private float cameraMaxX;

	// Token: 0x04001037 RID: 4151
	[HideInInspector]
	public Transform spikeTransform;

	// Token: 0x04001038 RID: 4152
	private Transform spikeTransformParent;

	// Token: 0x04001039 RID: 4153
	private RollyRagdollObstacle[] obstacles = new RollyRagdollObstacle[RollyRagdollsController.obstacleRows * 2];

	// Token: 0x0400103A RID: 4154
	private float moveSpeed = 1.5f;

	// Token: 0x0400103B RID: 4155
	private float curTime;
}
