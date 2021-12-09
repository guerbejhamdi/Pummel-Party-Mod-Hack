using System;
using System.Collections.Generic;
using I2.Loc;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using ZP.Net;
using ZP.Utility;

// Token: 0x020001AC RID: 428
public class FinderController : MinigameController
{
	// Token: 0x17000114 RID: 276
	// (get) Token: 0x06000C26 RID: 3110 RVA: 0x0000BA9E File Offset: 0x00009C9E
	public FinderGameState GameState
	{
		get
		{
			return this.finderGameState;
		}
	}

	// Token: 0x06000C27 RID: 3111 RVA: 0x00066174 File Offset: 0x00064374
	public override void InitializeMinigame()
	{
		base.InitializeMinigame();
		this.rand = new System.Random(UnityEngine.Random.Range(0, int.MaxValue));
		this.cameraShake = base.Root.GetComponentInChildren<CameraShake>();
		this.triangulation = NavMesh.CalculateTriangulation();
		this.baseAmbientColor = RenderSettings.ambientLight;
		this.bloodHandler = base.Root.transform.Find("NonStatic/Water/Water").GetComponent<SharkBloodSplatterHandler>();
		this.helicopter = base.Root.transform.Find("NonStatic/Helicopter").gameObject;
		this.helicopterAnim = this.helicopter.GetComponent<Animator>();
		if (NetSystem.IsServer)
		{
			int num = GameManager.rand.Next(0, GameManager.GetPlayerCount());
			Vector3 vector = new Vector3(-10f, 1.45f, 0f);
			Vector3 a = new Vector3(10f, 1.45f, 0f) - vector;
			int num2 = 0;
			for (int i = 0; i < GameManager.GetPlayerCount(); i++)
			{
				int num3 = Mathf.Clamp(GameManager.GetPlayerCount() - 2, 1, int.MaxValue);
				Vector3 vector2 = vector + a / (float)num3 * (float)num2;
				Quaternion rotation = Quaternion.Euler(0f, 180f, 0f);
				base.NetSpawn((num != i) ? "FinderPlayer" : "FinderPlayerShark", (num != i) ? vector2 : new Vector3(0f, 1.45f, 0f), rotation, (ushort)i, GameManager.GetPlayerAt(i).NetOwner);
				if (num != i)
				{
					num2++;
				}
			}
			int num4 = 60 - 60 % GameManager.GetPlayerCount();
			this.SpawnFakes(GameManager.rand.Next(), (byte)num4, (byte)num);
		}
		this.lights.AddRange(base.Root.transform.Find("Lighting").GetComponentsInChildren<Light>());
		this.reflectionProbes.AddRange(base.Root.GetComponentsInChildren<ReflectionProbe>());
		for (int j = 0; j < this.lights.Count; j++)
		{
			this.lightIntensities.Add(this.lights[j].intensity);
		}
		Transform transform = this.minigame_root.transform.Find("BeaconSpawnPoints");
		this.beaconSpawnLocations = new Transform[transform.childCount];
		for (int k = 0; k < this.beaconSpawnLocations.Length; k++)
		{
			this.beaconSpawnLocations[k] = transform.GetChild(k);
		}
	}

	// Token: 0x06000C28 RID: 3112 RVA: 0x0000BAA6 File Offset: 0x00009CA6
	[NetRPC(true, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void RPCSpawnFakes(NetPlayer player, int seed, byte count, byte finderID)
	{
		this.SpawnFakes(seed, count, finderID);
	}

	// Token: 0x06000C29 RID: 3113 RVA: 0x000663F8 File Offset: 0x000645F8
	private void SpawnFakes(int seed, byte count, byte finderID)
	{
		if (NetSystem.IsServer)
		{
			base.SendRPC("RPCSpawnFakes", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				seed,
				count,
				finderID
			});
		}
		System.Random r = new System.Random(seed);
		for (int i = 0; i < (int)count; i++)
		{
			int num = i % GameManager.GetPlayerCount();
			if (num != (int)finderID)
			{
				Vector3 randomNavMeshPoint = this.GetRandomNavMeshPoint(r);
				FinderPlayerFake component = base.Spawn(this.fakePlayer, randomNavMeshPoint, Quaternion.Euler(0f, 180f, 0f)).GetComponent<FinderPlayerFake>();
				component.Setup(num);
				this.fakePlayers.Add(component);
			}
		}
		this.finderPlayerID = (int)finderID;
	}

	// Token: 0x06000C2A RID: 3114 RVA: 0x000664A4 File Offset: 0x000646A4
	public override void StartMinigame()
	{
		this.mainUIElement = UnityEngine.Object.Instantiate<GameObject>(this.mainUIElementPrefab);
		this.mainUIElement.transform.SetParent(GameManager.UIController.MinigameUIRoot, false);
		this.icons = new Image[this.mainUIElement.transform.childCount];
		for (int i = 0; i < this.icons.Length; i++)
		{
			this.icons[i] = this.mainUIElement.transform.GetChild(i).GetComponent<Image>();
		}
		base.CreateTimer(UIAnchorType.TopRight, new Vector2(75f, 75f), this.round_length);
		base.CreateScoreUI(UIAnchorType.TopLeft, new Vector2(145f, 45f), 68f, false);
		GameManager.SetGlobalPlayerEmission(0f);
		for (int j = 0; j < this.ui_score.Length; j++)
		{
			if (!(this.ui_score[j] == null))
			{
				this.ui_score[j].scoreUpdateSpeed = 25;
			}
		}
		this.finderGameState = FinderGameState.ShowSharkPlayer;
		this.gameStateTimer.SetInterval(0.1f, true);
		base.StartMinigame();
	}

	// Token: 0x06000C2B RID: 3115 RVA: 0x0000A22E File Offset: 0x0000842E
	public override void RoundEnded()
	{
		base.RoundEnded();
	}

	// Token: 0x06000C2C RID: 3116 RVA: 0x00009C2C File Offset: 0x00007E2C
	public override void ResetRound()
	{
		base.ResetRound();
	}

	// Token: 0x06000C2D RID: 3117 RVA: 0x00009C34 File Offset: 0x00007E34
	public override void RoundStarting()
	{
		base.RoundStarting();
	}

	// Token: 0x06000C2E RID: 3118 RVA: 0x00009C3C File Offset: 0x00007E3C
	public override void StartNewRound()
	{
		base.StartNewRound();
	}

	// Token: 0x06000C2F RID: 3119 RVA: 0x000665BC File Offset: 0x000647BC
	private float Fade(AnimationCurve curve, float length, bool fadeIn)
	{
		float num = (Time.time - this.startTime) / length;
		float num2 = curve.Evaluate(num);
		this.SetIntensity((num >= 1f) ? (fadeIn ? 1f : 0f) : num2);
		return num;
	}

	// Token: 0x06000C30 RID: 3120 RVA: 0x00066604 File Offset: 0x00064804
	private void SetIntensity(float intensity)
	{
		for (int i = 0; i < this.lights.Count; i++)
		{
			this.lights[i].intensity = intensity * this.lightIntensities[i];
		}
		for (int j = 0; j < this.reflectionProbes.Count; j++)
		{
			this.reflectionProbes[j].intensity = intensity;
		}
		RenderSettings.ambientLight = this.baseAmbientColor * intensity;
	}

	// Token: 0x06000C31 RID: 3121 RVA: 0x00066680 File Offset: 0x00064880
	private void Update()
	{
		if (base.State == MinigameControllerState.Playing)
		{
			switch (this.finderGameState)
			{
			case FinderGameState.ShowSharkPlayer:
				if (this.gameStateTimer.Elapsed(true))
				{
					this.gameStateTimer.SetInterval(4f, true);
					this.finderGameState = FinderGameState.PlayersMove;
					this.startTime = Time.time;
					string newValue = string.Concat(new string[]
					{
						"<color=#",
						ColorUtility.ToHtmlStringRGBA(GameManager.GetPlayerAt(this.finderPlayerID).Color.uiColor),
						"> ",
						GameManager.GetPlayerAt(this.finderPlayerID).Name,
						"</color>"
					});
					string text = "<color=#4FF051FF>" + LocalizationManager.GetTranslation("SharkPlayer", true, 0, true, false, null, null, true).Replace("%Player%", newValue) + "</color>";
					GameManager.UIController.ShowLargeText(text, LargeTextType.PlayerWins, 3f, true, true);
				}
				break;
			case FinderGameState.PlayersMove:
				if (this.gameStateTimer.Elapsed(true))
				{
					GameManager.UIController.ShowLargeText(LocalizationManager.GetTranslation("SharkySwimInstruction", true, 0, true, false, null, null, true), LargeTextType.PlayerStart, 3.5f, true, true);
					this.finderGameState = FinderGameState.FadeOut;
					this.startTime = Time.time;
				}
				break;
			case FinderGameState.FadeOut:
				if (this.Fade(this.fadeOutCurve, this.fadeOutLength, false) >= 1f)
				{
					this.finderGameState = FinderGameState.FadeWait;
					this.gameStateTimer.SetInterval(2f, true);
					for (int i = 0; i < this.fakePlayers.Count; i++)
					{
						this.fakePlayers[i].smr.enabled = true;
						this.fakePlayers[i].floatyRenderer.enabled = true;
					}
				}
				break;
			case FinderGameState.FadeWait:
				if (this.gameStateTimer.Elapsed(true))
				{
					this.finderGameState = FinderGameState.FadeIn;
					this.startTime = Time.time;
				}
				break;
			case FinderGameState.FadeIn:
				if (this.Fade(this.fadeInCurve, this.fadeInLength, true) >= 1f)
				{
					this.finderGameState = FinderGameState.StartGameWait;
					this.gameStateTimer.SetInterval(1f, true);
				}
				break;
			case FinderGameState.StartGameWait:
				if (this.gameStateTimer.Elapsed(true))
				{
					this.finderGameState = FinderGameState.PlayGame;
					this.startTime = Time.time;
				}
				break;
			case FinderGameState.PlayGame:
				if (NetSystem.IsServer && this.beaconSpawnTimer.Elapsed(false))
				{
					this.SpawnPickup(this.pickupCount, (byte)UnityEngine.Random.Range(0, this.beaconSpawnLocations.Length));
					this.beaconSpawnTimer.SetInterval(10f, true);
					this.beaconSpawnTimer.Start();
				}
				break;
			}
			if (NetSystem.IsServer && this.finderGameState > FinderGameState.FadeOut && this.setAITargets.Elapsed(true))
			{
				this.SetPositions(GameManager.rand.Next());
			}
			if (NetSystem.IsServer && (this.ui_timer.time_test <= 0f || this.playersEscaped + this.playersDead > GameManager.GetPlayerCount() - 2))
			{
				base.EndRound(1f, 3f, false);
			}
		}
	}

	// Token: 0x06000C32 RID: 3122 RVA: 0x0000BAB2 File Offset: 0x00009CB2
	[NetRPC(true, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void RPCSetpositions(NetPlayer player, int seed)
	{
		this.SetPositions(seed);
	}

	// Token: 0x06000C33 RID: 3123 RVA: 0x000669A0 File Offset: 0x00064BA0
	public void SetPositions(int seed)
	{
		if (NetSystem.IsServer)
		{
			base.SendRPC("RPCSetpositions", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				seed
			});
		}
		System.Random random = new System.Random(seed);
		for (int i = 0; i < this.fakePlayers.Count; i++)
		{
			this.fakePlayers[i].GetPositions(random.Next());
		}
	}

	// Token: 0x06000C34 RID: 3124 RVA: 0x0000BABB File Offset: 0x00009CBB
	[NetRPC(true, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void RPCSpawnPickup(NetPlayer sender, byte id, byte spawnPointID)
	{
		this.SpawnPickup(id, spawnPointID);
	}

	// Token: 0x06000C35 RID: 3125 RVA: 0x00066A04 File Offset: 0x00064C04
	public void SpawnPickup(byte id, byte spawnPointID)
	{
		if (NetSystem.IsServer)
		{
			base.SendRPC("RPCSpawnPickup", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				id,
				spawnPointID
			});
		}
		GameObject gameObject = base.Spawn(this.beaconPickup, this.beaconSpawnLocations[(int)spawnPointID].position, Quaternion.identity);
		this.beacons[(int)id] = gameObject.GetComponent<FinderBeaconPickup>();
		this.beacons[(int)id].beaconID = id;
		this.beacons[(int)id].controller = this;
		this.pickupCount += 1;
	}

	// Token: 0x06000C36 RID: 3126 RVA: 0x0000BAC5 File Offset: 0x00009CC5
	[NetRPC(true, NetRPCSecurity.ALL, NetRPCSecurity.ALL)]
	public void RPCPickupBeacon(NetPlayer sender, byte id)
	{
		this.PickupBeacon(id, true);
	}

	// Token: 0x06000C37 RID: 3127 RVA: 0x00066A94 File Offset: 0x00064C94
	public void PickupBeacon(byte id, bool rpc = false)
	{
		if (this.beaconsPickedup >= this.maxBeacons || this.beacons[(int)id].PickedUp)
		{
			return;
		}
		if (!rpc)
		{
			base.SendRPC("RPCPickupBeacon", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				id
			});
		}
		if (this.icons[this.beaconsPickedup] != null)
		{
			this.icons[this.beaconsPickedup].color = new Color(1f, 1f, 1f, 1f);
		}
		this.beaconsPickedup++;
		this.beacons[(int)id].OnPickup();
		if (this.beaconsPickedup >= this.maxBeacons)
		{
			this.finderGameState = FinderGameState.HelicopterActive;
			this.helicopter.SetActive(true);
			AudioSystem.PlayLooping(this.helicopterIdle, 1f, 3f);
		}
	}

	// Token: 0x06000C38 RID: 3128 RVA: 0x0000BACF File Offset: 0x00009CCF
	public override void ReleaseMinigame()
	{
		GameManager.SetGlobalPlayerEmission(1f);
		base.ReleaseMinigame();
	}

	// Token: 0x06000C39 RID: 3129 RVA: 0x0000BAE1 File Offset: 0x00009CE1
	public void PlayerDied(FinderPlayer player)
	{
		if (!player.escaped)
		{
			this.playersDead++;
		}
	}

	// Token: 0x06000C3A RID: 3130 RVA: 0x0000BAF9 File Offset: 0x00009CF9
	public void PlayerEscaped(FinderPlayer player)
	{
		if (!player.IsDead)
		{
			this.playersEscaped++;
		}
	}

	// Token: 0x06000C3B RID: 3131 RVA: 0x00009C4C File Offset: 0x00007E4C
	public override bool HasLoadedLocally()
	{
		return base.HasLoadedLocally();
	}

	// Token: 0x06000C3C RID: 3132 RVA: 0x00066B70 File Offset: 0x00064D70
	public Vector3 GetRandomNavMeshPoint(System.Random r = null)
	{
		if (this.binaryTree == null)
		{
			this.triangulation = NavMesh.CalculateTriangulation();
			if (this.triangulation.vertices.Length != 0)
			{
				List<float> list = new List<float>();
				for (int i = 0; i < this.triangulation.indices.Length / 3; i++)
				{
					int num = i * 3;
					Vector3 vector = this.triangulation.vertices[this.triangulation.indices[num]];
					Vector3 vector2 = this.triangulation.vertices[this.triangulation.indices[num + 1]];
					Vector3 vector3 = this.triangulation.vertices[this.triangulation.indices[num + 2]];
					float num2 = Vector3.Distance(vector, vector2);
					float num3 = Vector3.Distance(vector2, vector3);
					float num4 = Vector3.Distance(vector3, vector);
					float num5 = (num2 + num3 + num4) / 2f;
					float num6 = Mathf.Sqrt(num5 * (num5 - num2) * (num5 - num3) * (num5 - num4));
					list.Add(this.totalArea);
					this.totalArea += num6;
				}
				this.binaryTree = new BinaryTree(list.ToArray());
			}
		}
		if (this.binaryTree != null)
		{
			float p = ZPMath.RandomFloat((r == null) ? this.rand : r, 0f, this.totalArea);
			int num7 = this.binaryTree.FindPoint(p) * 3;
			Vector3[] vertices = this.triangulation.vertices;
			int[] indices = this.triangulation.indices;
			return ZPMath.RandomTrianglePoint(vertices[indices[num7]], vertices[indices[num7 + 1]], vertices[indices[num7 + 2]], (r == null) ? this.rand : r);
		}
		return Vector3.zero;
	}

	// Token: 0x04000B7D RID: 2941
	[Header("Minigame specific attributes")]
	public GameObject fakePlayer;

	// Token: 0x04000B7E RID: 2942
	public float fadeOutLength = 2f;

	// Token: 0x04000B7F RID: 2943
	public float fadeInLength = 1f;

	// Token: 0x04000B80 RID: 2944
	public AnimationCurve fadeOutCurve;

	// Token: 0x04000B81 RID: 2945
	public AnimationCurve fadeInCurve;

	// Token: 0x04000B82 RID: 2946
	public SharkBloodSplatterHandler bloodHandler;

	// Token: 0x04000B83 RID: 2947
	public GameObject beaconPickup;

	// Token: 0x04000B84 RID: 2948
	public GameObject mainUIElementPrefab;

	// Token: 0x04000B85 RID: 2949
	public AudioClip helicopterIdle;

	// Token: 0x04000B86 RID: 2950
	private CameraShake cameraShake;

	// Token: 0x04000B87 RID: 2951
	private FinderGameState finderGameState;

	// Token: 0x04000B88 RID: 2952
	private ActionTimer gameStateTimer = new ActionTimer(0f);

	// Token: 0x04000B89 RID: 2953
	private NavMeshTriangulation triangulation;

	// Token: 0x04000B8A RID: 2954
	private BinaryTree binaryTree;

	// Token: 0x04000B8B RID: 2955
	private float totalArea;

	// Token: 0x04000B8C RID: 2956
	private System.Random rand;

	// Token: 0x04000B8D RID: 2957
	private ActionTimer setAITargets = new ActionTimer(4f, 8f);

	// Token: 0x04000B8E RID: 2958
	public List<FinderPlayerFake> fakePlayers = new List<FinderPlayerFake>();

	// Token: 0x04000B8F RID: 2959
	private List<Light> lights = new List<Light>();

	// Token: 0x04000B90 RID: 2960
	private List<ReflectionProbe> reflectionProbes = new List<ReflectionProbe>();

	// Token: 0x04000B91 RID: 2961
	private List<float> lightIntensities = new List<float>();

	// Token: 0x04000B92 RID: 2962
	private float startTime;

	// Token: 0x04000B93 RID: 2963
	private Color baseAmbientColor;

	// Token: 0x04000B94 RID: 2964
	private int finderPlayerID;

	// Token: 0x04000B95 RID: 2965
	private Transform[] beaconSpawnLocations;

	// Token: 0x04000B96 RID: 2966
	private GameObject mainUIElement;

	// Token: 0x04000B97 RID: 2967
	private Image[] icons;

	// Token: 0x04000B98 RID: 2968
	private ActionTimer beaconSpawnTimer = new ActionTimer(0f);

	// Token: 0x04000B99 RID: 2969
	private GameObject helicopter;

	// Token: 0x04000B9A RID: 2970
	private Animator helicopterAnim;

	// Token: 0x04000B9B RID: 2971
	private byte pickupCount;

	// Token: 0x04000B9C RID: 2972
	public FinderBeaconPickup[] beacons = new FinderBeaconPickup[256];

	// Token: 0x04000B9D RID: 2973
	private int maxBeacons = 5;

	// Token: 0x04000B9E RID: 2974
	private int beaconsPickedup;

	// Token: 0x04000B9F RID: 2975
	public int playersDead;

	// Token: 0x04000BA0 RID: 2976
	public int playersEscaped;
}
