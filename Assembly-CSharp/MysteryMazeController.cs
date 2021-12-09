using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using ZP.Net;

// Token: 0x020001E5 RID: 485
public class MysteryMazeController : MinigameController
{
	// Token: 0x06000E16 RID: 3606 RVA: 0x0000398C File Offset: 0x00001B8C
	public void Awake()
	{
	}

	// Token: 0x06000E17 RID: 3607 RVA: 0x00071588 File Offset: 0x0006F788
	public MysteryMazeAITarget GetRandomAITarget(int groupIndex)
	{
		List<MysteryMazeAITarget> list = null;
		if (!this.m_targets.TryGetValue(groupIndex, out list))
		{
			if (this.m_finalTargets.Count <= 0)
			{
				return null;
			}
			list = this.m_finalTargets;
		}
		int index = UnityEngine.Random.Range(0, list.Count);
		return list[index];
	}

	// Token: 0x17000142 RID: 322
	// (get) Token: 0x06000E18 RID: 3608 RVA: 0x0000C997 File Offset: 0x0000AB97
	// (set) Token: 0x06000E19 RID: 3609 RVA: 0x0000C99F File Offset: 0x0000AB9F
	public int FinishedPlayers { get; set; }

	// Token: 0x06000E1A RID: 3610 RVA: 0x000715D4 File Offset: 0x0006F7D4
	public override void InitializeMinigame()
	{
		base.InitializeMinigame();
		this.rand = new System.Random(UnityEngine.Random.Range(0, int.MaxValue));
		if (NetSystem.IsServer)
		{
			base.SpawnPlayers("MysteryMazePlayer", null);
			int num = 0;
			if (RBPrefs.HasKey(this.m_mazeIndexKey))
			{
				num = RBPrefs.GetInt(this.m_mazeIndexKey, 0);
				RBPrefs.SetInt(this.m_mazeIndexKey, num + 1);
			}
			else
			{
				RBPrefs.SetInt(this.m_mazeIndexKey, 0);
			}
			base.StartCoroutine(this.SetupMaze(num));
		}
		this.m_events.Add(new MysteryMazeVisibilityEvent(75f, 74f));
		this.m_events.Add(new MysteryMazeVisibilityEvent(60f, 59f));
		this.m_events.Add(new MysteryMazeVisibilityEvent(45f, 44f));
		this.m_events.Add(new MysteryMazeVisibilityEvent(30f, 29f));
		this.m_events.Add(new MysteryMazeVisibilityEvent(15f, 14f));
		this.m_events.Add(new MysteryMazeVisibilityEvent(5f, 4f));
	}

	// Token: 0x06000E1B RID: 3611 RVA: 0x000716F4 File Offset: 0x0006F8F4
	public override void StartMinigame()
	{
		base.CreateScoreUI(UIAnchorType.TopLeft, new Vector2(145f, 45f), 68f, false);
		base.CreateTimer(UIAnchorType.TopRight, new Vector2(75f, 75f), this.round_length);
		this.m_clipRadiusTarget = 0f;
		AudioSystem.PlayOneShot(this.m_hideClip, 1f, 0f, 1f);
		base.StartMinigame();
	}

	// Token: 0x06000E1C RID: 3612 RVA: 0x0000A22E File Offset: 0x0000842E
	public override void RoundEnded()
	{
		base.RoundEnded();
	}

	// Token: 0x06000E1D RID: 3613 RVA: 0x00009C2C File Offset: 0x00007E2C
	public override void ResetRound()
	{
		base.ResetRound();
	}

	// Token: 0x06000E1E RID: 3614 RVA: 0x00009C34 File Offset: 0x00007E34
	public override void RoundStarting()
	{
		base.RoundStarting();
	}

	// Token: 0x06000E1F RID: 3615 RVA: 0x00009C3C File Offset: 0x00007E3C
	public override void StartNewRound()
	{
		base.StartNewRound();
	}

	// Token: 0x06000E20 RID: 3616 RVA: 0x0000C9A8 File Offset: 0x0000ABA8
	private IEnumerator SetupMaze(int mazeIndex)
	{
		if (NetSystem.IsServer)
		{
			base.SendRPC("RPCSetupMaze", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				mazeIndex
			});
		}
		int index = mazeIndex % this.m_pathPrefabs.Count;
		NavMeshSurface m_surface = UnityEngine.Object.FindObjectOfType<NavMeshSurface>();
		while (m_surface == null)
		{
			yield return new WaitForSeconds(0.1f);
			m_surface = UnityEngine.Object.FindObjectOfType<NavMeshSurface>();
		}
		if (m_surface != null)
		{
			base.Spawn(this.m_pathPrefabs[index], Vector3.zero, Quaternion.identity).transform.parent = m_surface.gameObject.transform;
			yield return null;
			m_surface.UpdateNavMesh(m_surface.navMeshData);
		}
		foreach (MysteryMazeAITarget mysteryMazeAITarget in UnityEngine.Object.FindObjectsOfType<MysteryMazeAITarget>())
		{
			if (mysteryMazeAITarget.FinalTarget)
			{
				this.m_finalTargets.Add(mysteryMazeAITarget);
			}
			else
			{
				if (!this.m_targets.ContainsKey(mysteryMazeAITarget.GroupIndex))
				{
					this.m_targets.Add(mysteryMazeAITarget.GroupIndex, new List<MysteryMazeAITarget>());
				}
				this.m_targets[mysteryMazeAITarget.GroupIndex].Add(mysteryMazeAITarget);
			}
		}
		yield return null;
		yield break;
	}

	// Token: 0x06000E21 RID: 3617 RVA: 0x0000C9BE File Offset: 0x0000ABBE
	[NetRPC(false, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void RPCSetupMaze(NetPlayer sender, int mazeIndex)
	{
		base.StartCoroutine(this.SetupMaze(mazeIndex));
	}

	// Token: 0x06000E22 RID: 3618 RVA: 0x00071764 File Offset: 0x0006F964
	private void Update()
	{
		this.m_clipRadius = Mathf.SmoothDamp(this.m_clipRadius, this.m_clipRadiusTarget, ref this.m_clipRadiusVelocity, 0.5f);
		Shader.SetGlobalFloat("_ClipRadiusOverride", this.m_clipRadius);
		if (base.State == MinigameControllerState.Playing)
		{
			float time_test = this.ui_timer.time_test;
			foreach (MysteryMazeVisibilityEvent mysteryMazeVisibilityEvent in this.m_events)
			{
				if (!mysteryMazeVisibilityEvent.completed)
				{
					if (!mysteryMazeVisibilityEvent.started)
					{
						if (time_test <= mysteryMazeVisibilityEvent.startTime)
						{
							mysteryMazeVisibilityEvent.started = true;
							this.m_clipRadiusTarget = 50f;
							AudioSystem.PlayOneShot(this.m_showClip, 1f, 0f, 1f);
						}
					}
					else if (time_test <= mysteryMazeVisibilityEvent.endTime)
					{
						mysteryMazeVisibilityEvent.completed = true;
						this.m_clipRadiusTarget = 0f;
						AudioSystem.PlayOneShot(this.m_hideClip, 1f, 0f, 1f);
					}
				}
			}
			if (NetSystem.IsServer)
			{
				int num = 0;
				for (int i = 0; i < this.players.Count; i++)
				{
					if (((MysteryMazePlayer)this.players[i]).IsFinished)
					{
						num++;
					}
				}
				if (num >= this.players.Count - 1 || this.ui_timer.time_test <= 0f)
				{
					base.EndRound(3f, 1f, false);
				}
			}
		}
	}

	// Token: 0x06000E23 RID: 3619 RVA: 0x00009C44 File Offset: 0x00007E44
	public override void ReleaseMinigame()
	{
		base.ReleaseMinigame();
	}

	// Token: 0x06000E24 RID: 3620 RVA: 0x00009C4C File Offset: 0x00007E4C
	public override bool HasLoadedLocally()
	{
		return base.HasLoadedLocally();
	}

	// Token: 0x04000D8B RID: 3467
	private System.Random rand;

	// Token: 0x04000D8C RID: 3468
	private NavMeshSurface m_surface;

	// Token: 0x04000D8D RID: 3469
	[SerializeField]
	private AudioClip m_showClip;

	// Token: 0x04000D8E RID: 3470
	[SerializeField]
	private AudioClip m_hideClip;

	// Token: 0x04000D8F RID: 3471
	[SerializeField]
	private List<GameObject> m_pathPrefabs;

	// Token: 0x04000D90 RID: 3472
	private float m_clipRadius = 50f;

	// Token: 0x04000D91 RID: 3473
	private float m_clipRadiusTarget = 50f;

	// Token: 0x04000D92 RID: 3474
	private float m_clipRadiusVelocity;

	// Token: 0x04000D93 RID: 3475
	private List<MysteryMazeVisibilityEvent> m_events = new List<MysteryMazeVisibilityEvent>();

	// Token: 0x04000D94 RID: 3476
	private Dictionary<int, List<MysteryMazeAITarget>> m_targets = new Dictionary<int, List<MysteryMazeAITarget>>();

	// Token: 0x04000D95 RID: 3477
	private List<MysteryMazeAITarget> m_finalTargets = new List<MysteryMazeAITarget>();

	// Token: 0x04000D97 RID: 3479
	private string m_mazeIndexKey = "MINIGAME_MYSTERYMAZE_MAZE_IDX";

	// Token: 0x04000D98 RID: 3480
	private int count;
}
