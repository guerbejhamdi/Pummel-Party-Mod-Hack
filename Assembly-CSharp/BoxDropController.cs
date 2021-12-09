using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using ZP.Net;
using ZP.Utility;

// Token: 0x0200016C RID: 364
public class BoxDropController : MinigameController
{
	// Token: 0x170000EC RID: 236
	// (get) Token: 0x06000A76 RID: 2678 RVA: 0x0000ACE1 File Offset: 0x00008EE1
	// (set) Token: 0x06000A77 RID: 2679 RVA: 0x0000ACE9 File Offset: 0x00008EE9
	public int DeadPlayerCount { get; set; }

	// Token: 0x06000A78 RID: 2680 RVA: 0x0005C220 File Offset: 0x0005A420
	public Vector3 GetRandomNavMeshPoint()
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
			float p = ZPMath.RandomFloat(this.rand, 0f, this.totalArea);
			int num7 = this.binaryTree.FindPoint(p) * 3;
			Vector3[] vertices = this.triangulation.vertices;
			int[] indices = this.triangulation.indices;
			return ZPMath.RandomTrianglePoint(vertices[indices[num7]], vertices[indices[num7 + 1]], vertices[indices[num7 + 2]], this.rand);
		}
		return Vector3.zero;
	}

	// Token: 0x06000A79 RID: 2681 RVA: 0x0005C3D8 File Offset: 0x0005A5D8
	public override void InitializeMinigame()
	{
		base.InitializeMinigame();
		this.rand = new System.Random(UnityEngine.Random.Range(0, int.MaxValue));
		if (NetSystem.IsServer)
		{
			base.SpawnPlayers("BoxDropPlayer", null);
		}
		this.triangulation = NavMesh.CalculateTriangulation();
		this.m_boxController = base.Root.GetComponentInChildren<BoxController>();
		this.m_shake = base.Root.GetComponentInChildren<CameraShake>();
	}

	// Token: 0x06000A7A RID: 2682 RVA: 0x0000ACF2 File Offset: 0x00008EF2
	public override void StartMinigame()
	{
		base.CreateScoreUI(UIAnchorType.TopLeft, new Vector2(145f, 45f), 68f, false);
		base.CreateTimer(UIAnchorType.TopRight, new Vector2(75f, 75f), this.round_length);
		base.StartMinigame();
	}

	// Token: 0x06000A7B RID: 2683 RVA: 0x0005C444 File Offset: 0x0005A644
	public override void BuildResults()
	{
		for (int i = 0; i < this.players.Count; i++)
		{
			this.players[i].Score = (short)(((BoxDropPlayer)this.players[i]).Placement * 10);
		}
		base.BuildResults();
	}

	// Token: 0x06000A7C RID: 2684 RVA: 0x00009C2C File Offset: 0x00007E2C
	public override void ResetRound()
	{
		base.ResetRound();
	}

	// Token: 0x06000A7D RID: 2685 RVA: 0x00009C34 File Offset: 0x00007E34
	public override void RoundStarting()
	{
		base.RoundStarting();
	}

	// Token: 0x06000A7E RID: 2686 RVA: 0x00009C3C File Offset: 0x00007E3C
	public override void StartNewRound()
	{
		base.StartNewRound();
	}

	// Token: 0x06000A7F RID: 2687 RVA: 0x0005C498 File Offset: 0x0005A698
	private void Update()
	{
		if (base.State == MinigameControllerState.Playing)
		{
			if (this.ui_timer.time_test <= 40f && !this.gotAchievement)
			{
				bool flag = false;
				for (int i = 0; i < this.players.Count; i++)
				{
					if (this.players[i].GamePlayer.IsLocalPlayer && !this.players[i].IsDead && !this.players[i].GamePlayer.IsAI)
					{
						flag = true;
					}
				}
				if (flag)
				{
					PlatformAchievementManager.Instance.TriggerAchievement("ACH_BREAKING_BLOCKS");
					this.gotAchievement = true;
				}
			}
			if (NetSystem.IsServer)
			{
				int num = 0;
				for (int j = 0; j < this.players.Count; j++)
				{
					if (((BoxDropPlayer)this.players[j]).IsDead)
					{
						num++;
					}
				}
				if (num >= this.players.Count - 1 || this.ui_timer.time_test <= 0f)
				{
					base.EndRound(3f, 1f, false);
				}
				if (!this.m_dropBoxesQueued)
				{
					float num2 = (this.round_length - this.ui_timer.time_test) / (this.round_length * 0.8f);
					float delay = Mathf.Clamp(1f - num2 * 0.9f, 0.3f, 1f);
					this.m_dropTimeElapsed += Time.deltaTime;
					if (this.m_dropTimeElapsed >= 2.5f)
					{
						this.DropBoxes(UnityEngine.Random.Range(0, BoxController.GetBoxDropPermuationCount()), delay, NetSystem.NetTime.GameTime + 1f);
						this.m_dropTimeElapsed = 0f;
					}
				}
			}
			if (this.m_dropBoxesQueued && NetSystem.NetTime.GameTime >= this.m_dropBoxesTime)
			{
				this.m_dropBoxesQueued = false;
				this.m_boxController.DropBoxes(this.m_dropBoxesPermutation, this.m_dropBoxesDelay);
				base.StartCoroutine(this.PlayShakeSound(this.m_dropBoxesDelay));
				this.OnDropBoxes.Invoke();
			}
		}
	}

	// Token: 0x06000A80 RID: 2688 RVA: 0x0000AD32 File Offset: 0x00008F32
	private IEnumerator PlayShakeSound(float delay)
	{
		this.m_shake.AddShake(0.25f);
		TempAudioSource source = AudioSystem.PlayLooping(this.m_shakeSound, 0.05f, 1f);
		TempAudioSource source2 = AudioSystem.PlayLooping(this.m_rumbleSound, 0.05f, 1f);
		yield return new WaitForSeconds(delay + 0.5f);
		source.FadeAudio(0.5f, FadeType.Out);
		source2.FadeAudio(0.5f, FadeType.Out);
		yield break;
	}

	// Token: 0x06000A81 RID: 2689 RVA: 0x0005C6A0 File Offset: 0x0005A8A0
	public void DropBoxes(int permutation, float delay, float dropTime)
	{
		if (this.m_dropBoxesQueued)
		{
			return;
		}
		this.m_dropBoxesQueued = true;
		this.m_dropBoxesTime = dropTime;
		this.m_dropBoxesPermutation = permutation;
		this.m_dropBoxesDelay = delay;
		if (NetSystem.IsServer)
		{
			base.SendRPC("RPCDropBoxes", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				permutation,
				delay,
				dropTime
			});
		}
	}

	// Token: 0x06000A82 RID: 2690 RVA: 0x0000AD48 File Offset: 0x00008F48
	[NetRPC(true, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void RPCDropBoxes(NetPlayer sender, int permutation, float delay, float dropTime)
	{
		this.DropBoxes(permutation, delay, dropTime);
	}

	// Token: 0x06000A83 RID: 2691 RVA: 0x0000AD54 File Offset: 0x00008F54
	public void PlayerDied(BoxDropPlayer player)
	{
		this.players_alive--;
	}

	// Token: 0x06000A84 RID: 2692 RVA: 0x00009C44 File Offset: 0x00007E44
	public override void ReleaseMinigame()
	{
		base.ReleaseMinigame();
	}

	// Token: 0x06000A85 RID: 2693 RVA: 0x00009C4C File Offset: 0x00007E4C
	public override bool HasLoadedLocally()
	{
		return base.HasLoadedLocally();
	}

	// Token: 0x0400095A RID: 2394
	[SerializeField]
	protected AudioClip m_shakeSound;

	// Token: 0x0400095B RID: 2395
	[SerializeField]
	protected AudioClip m_rumbleSound;

	// Token: 0x0400095C RID: 2396
	private System.Random rand;

	// Token: 0x0400095D RID: 2397
	private NavMeshTriangulation triangulation;

	// Token: 0x0400095E RID: 2398
	private BinaryTree binaryTree;

	// Token: 0x0400095F RID: 2399
	private float totalArea;

	// Token: 0x04000960 RID: 2400
	private BoxController m_boxController;

	// Token: 0x04000961 RID: 2401
	private bool m_dropBoxesQueued;

	// Token: 0x04000962 RID: 2402
	private float m_dropBoxesTime;

	// Token: 0x04000963 RID: 2403
	private int m_dropBoxesPermutation;

	// Token: 0x04000964 RID: 2404
	private float m_dropBoxesDelay;

	// Token: 0x04000965 RID: 2405
	private CameraShake m_shake;

	// Token: 0x04000966 RID: 2406
	public UnityEvent OnDropBoxes;

	// Token: 0x04000967 RID: 2407
	private bool gotAchievement;

	// Token: 0x04000969 RID: 2409
	private NavMeshSurface m_surface;

	// Token: 0x0400096A RID: 2410
	private int count;

	// Token: 0x0400096B RID: 2411
	private float m_dropTimeElapsed = 5f;
}
