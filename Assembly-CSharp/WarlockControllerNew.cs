using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using ZP.Net;

// Token: 0x0200028D RID: 653
public class WarlockControllerNew : MinigameController
{
	// Token: 0x06001332 RID: 4914 RVA: 0x0000F501 File Offset: 0x0000D701
	public override void OnNetInitialize()
	{
		base.OnNetInitialize();
	}

	// Token: 0x06001333 RID: 4915 RVA: 0x0009456C File Offset: 0x0009276C
	public override void InitializeMinigame()
	{
		base.InitializeMinigame();
		this.navMeshSurface = base.Root.GetComponent<NavMeshSurface>();
		this.destructionController = base.Root.GetComponent<WarlocksDestructionController>();
		this.innerCylinder = base.Root.transform.Find("CylinderCollider").gameObject;
		this.layerParents[0] = base.Root.transform.Find("PlatformLayer_02");
		this.layerParents[1] = base.Root.transform.Find("PlatformLayer_03");
		this.layerParents[2] = base.Root.transform.Find("PlatformLayer_04");
		for (int i = 0; i < GameManager.GetPlayerCount(); i++)
		{
			MeshRenderer component = base.Root.transform.Find("PlatformLayer_04/PlayerSpawnPlatform_0" + i.ToString()).GetComponent<MeshRenderer>();
			component.material = new Material(component.material);
			component.sharedMaterial.color = GameManager.GetPlayerWithID((short)i).Color.skinColor1;
		}
		for (int j = 0; j < this.layerParents.Length; j++)
		{
			this.layerNavMeshModifiers[j] = this.layerParents[j].GetComponentsInChildren<NavMeshModifier>();
			this.layerColliders[j] = this.layerParents[j].GetComponentsInChildren<MeshCollider>();
		}
		if (NetSystem.IsServer)
		{
			base.SpawnPlayers("WarlocksMinigamePlayer", null);
		}
	}

	// Token: 0x06001334 RID: 4916 RVA: 0x000946CC File Offset: 0x000928CC
	public override void StartMinigame()
	{
		base.CreateScoreUI(UIAnchorType.TopLeft, new Vector2(145f, 45f), 68f, false);
		base.CreateTimer(UIAnchorType.TopRight, new Vector2(75f, 75f), this.round_length);
		for (int i = 0; i < this.ui_score.Length; i++)
		{
			if (!(this.ui_score[i] == null))
			{
				this.ui_score[i].scoreUpdateSpeed = 100;
				this.ui_score[i].minChangeText = 5;
				this.ui_score[i].showChanges = false;
				this.ui_score[i].SetScore(70);
				((WarlockPlayerNew1)this.players[i]).Health = 70;
				this.ui_score[i].showChanges = true;
			}
		}
		this.healthBars = new GameObject[GameManager.GetPlayerCount()];
		this.healthFill = new Image[GameManager.GetPlayerCount()];
		for (int j = 0; j < this.healthBars.Length; j++)
		{
			this.healthBars[j] = UnityEngine.Object.Instantiate<GameObject>(this.healthbarPrefab);
			this.healthBars[j].transform.SetParent(GameManager.UIController.MinigameUIRoot, false);
			this.healthFill[j] = this.healthBars[j].transform.Find("HealthFill").GetComponent<Image>();
		}
		base.StartMinigame();
	}

	// Token: 0x06001335 RID: 4917 RVA: 0x00094824 File Offset: 0x00092A24
	public override void RoundEnded()
	{
		base.StopCoroutine(this.destructionCoroutine);
		if (NetSystem.IsServer)
		{
			for (int i = 0; i < this.players.Count; i++)
			{
				if (!((WarlockPlayerNew1)this.players[i]).IsDead)
				{
					CharacterBase characterBase = this.players[i];
					characterBase.Score += (short)((this.players.Count - 1) * 25);
				}
				else
				{
					CharacterBase characterBase2 = this.players[i];
					characterBase2.Score += this.players[i].RoundScore * 25;
					this.players[i].RoundScore = 0;
				}
			}
		}
		for (int j = 0; j < this.players.Count; j++)
		{
			if (this.players[j].IsOwner && !this.players[j].GamePlayer.IsAI && !((WarlockPlayerNew1)this.players[j]).HitLava)
			{
				PlatformAchievementManager.Instance.TriggerAchievement("ACH_MAGMA_MAGES");
			}
		}
		this.ResetNavElements();
		base.RoundEnded();
	}

	// Token: 0x06001336 RID: 4918 RVA: 0x0009495C File Offset: 0x00092B5C
	private void ResetNavElements()
	{
		for (int i = 0; i < this.layerParents.Length; i++)
		{
			this.SetAsLava(i, true);
		}
		for (int j = 0; j < this.layerColliders[0].Length; j++)
		{
			this.layerColliders[0][j].enabled = false;
		}
		this.innerCylinder.transform.localScale = new Vector3(14.5f, 0.5f, 14.5f);
	}

	// Token: 0x06001337 RID: 4919 RVA: 0x0000F509 File Offset: 0x0000D709
	public override void ResetRound()
	{
		base.ResetRound();
		this.fire_ball_counter = 0;
		while (this.fireballs.Count > 0)
		{
			this.fireballs[0].DestroyFireball(false, 254);
		}
	}

	// Token: 0x06001338 RID: 4920 RVA: 0x0000F53F File Offset: 0x0000D73F
	private IEnumerator NavMeshBuildDelay()
	{
		yield return new WaitForSeconds(0.15f);
		this.navMeshSurface.BuildNavMesh();
		yield return new WaitForSeconds(0.1f);
		yield break;
	}

	// Token: 0x06001339 RID: 4921 RVA: 0x0000F54E File Offset: 0x0000D74E
	public override void StartNewRound()
	{
		this.destructionCoroutine = base.StartCoroutine(this.DoDestruction());
		Debug.Log("Starting New Round");
		base.StartNewRound();
	}

	// Token: 0x0600133A RID: 4922 RVA: 0x0000F572 File Offset: 0x0000D772
	private IEnumerator DoDestruction()
	{
		yield return new WaitForSeconds(10f);
		this.destructionController.IncreaseDestruction();
		this.SetAsLava(2, false);
		this.navMeshSurface.BuildNavMesh();
		yield return new WaitForSeconds(15f);
		this.destructionController.IncreaseDestruction();
		this.SetAsLava(1, false);
		this.innerCylinder.transform.localScale = new Vector3(6.5f, 0.5f, 6.5f);
		for (int i = 0; i < this.layerColliders[0].Length; i++)
		{
			this.layerColliders[0][i].enabled = true;
		}
		this.navMeshSurface.BuildNavMesh();
		yield return new WaitForSeconds(15f);
		this.destructionController.IncreaseDestruction();
		this.SetAsLava(0, false);
		this.navMeshSurface.BuildNavMesh();
		yield break;
	}

	// Token: 0x0600133B RID: 4923 RVA: 0x000949D0 File Offset: 0x00092BD0
	private void SetAsLava(int layer, bool reset)
	{
		for (int i = 0; i < this.layerNavMeshModifiers[layer].Length; i++)
		{
			if (reset)
			{
				this.layerNavMeshModifiers[layer][i].ignoreFromBuild = false;
				this.layerNavMeshModifiers[layer][i].gameObject.tag = "Untagged";
			}
			else
			{
				this.layerNavMeshModifiers[layer][i].ignoreFromBuild = true;
				this.layerNavMeshModifiers[layer][i].gameObject.tag = "MinigameCustom02";
			}
		}
	}

	// Token: 0x0600133C RID: 4924 RVA: 0x00094A4C File Offset: 0x00092C4C
	private void Update()
	{
		if (base.State == MinigameControllerState.Playing)
		{
			if (NetSystem.IsServer && (this.players_alive <= 1 || this.ui_timer.time_test <= 0f))
			{
				base.EndRound(1f, 1f, false);
			}
			for (int i = 0; i < this.players.Count; i++)
			{
				WarlockPlayerNew1 warlockPlayerNew = (WarlockPlayerNew1)this.players[i];
				Vector3 zero = new Vector3(0f, 0f, 0f);
				zero = Vector3.zero;
				this.healthBars[i].transform.position = this.minigameCameras[0].WorldToScreenPoint(this.players[i].transform.position + zero) + new Vector3(0f, 80f, 0f);
				this.healthFill[i].fillAmount = (float)warlockPlayerNew.Health / 70f;
				if (this.healthBars[i].activeInHierarchy != !warlockPlayerNew.IsDead)
				{
					this.healthBars[i].SetActive(!warlockPlayerNew.IsDead);
				}
			}
		}
	}

	// Token: 0x0600133D RID: 4925 RVA: 0x00009C44 File Offset: 0x00007E44
	public override void ReleaseMinigame()
	{
		base.ReleaseMinigame();
	}

	// Token: 0x0600133E RID: 4926 RVA: 0x0000A7A0 File Offset: 0x000089A0
	public void PlayerDied(WarlockPlayerNew1 player)
	{
		if (NetSystem.IsServer)
		{
			player.RoundScore = (short)(this.players.Count - this.players_alive);
		}
		this.players_alive--;
	}

	// Token: 0x0600133F RID: 4927 RVA: 0x00094B84 File Offset: 0x00092D84
	public void DestroyFireball(short id, byte hitPlayer)
	{
		WarlocksFireballNew warlocksFireballNew = this.FindFireball(id);
		if (warlocksFireballNew != null)
		{
			warlocksFireballNew.DestroyFireball(false, hitPlayer);
			bool isServer = NetSystem.IsServer;
		}
	}

	// Token: 0x06001340 RID: 4928 RVA: 0x00094BB0 File Offset: 0x00092DB0
	private WarlocksFireballNew FindFireball(short id)
	{
		for (int i = 0; i < this.fireballs.Count; i++)
		{
			if (this.fireballs[i].ID == id)
			{
				return this.fireballs[i];
			}
		}
		return null;
	}

	// Token: 0x06001341 RID: 4929 RVA: 0x00094BF8 File Offset: 0x00092DF8
	[NetRPC(false, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void RPCDestroyFireball(NetPlayer sender, short id, byte hitPlayer)
	{
		WarlocksFireballNew warlocksFireballNew = this.FindFireball(id);
		if (warlocksFireballNew != null)
		{
			warlocksFireballNew.DestroyFireball(false, hitPlayer);
		}
	}

	// Token: 0x04001480 RID: 5248
	[Header("Minigame specific attributes")]
	public List<WarlocksFireballNew> fireballs = new List<WarlocksFireballNew>();

	// Token: 0x04001481 RID: 5249
	public AudioClip end_whistle;

	// Token: 0x04001482 RID: 5250
	public short fire_ball_counter;

	// Token: 0x04001483 RID: 5251
	private NavMeshSurface navMeshSurface;

	// Token: 0x04001484 RID: 5252
	private WarlocksDestructionController destructionController;

	// Token: 0x04001485 RID: 5253
	private GameObject innerCylinder;

	// Token: 0x04001486 RID: 5254
	private Transform[] layerParents = new Transform[3];

	// Token: 0x04001487 RID: 5255
	private MeshCollider[][] layerColliders = new MeshCollider[3][];

	// Token: 0x04001488 RID: 5256
	private NavMeshModifier[][] layerNavMeshModifiers = new NavMeshModifier[3][];

	// Token: 0x04001489 RID: 5257
	private Coroutine destructionCoroutine;

	// Token: 0x0400148A RID: 5258
	public GameObject healthbarPrefab;

	// Token: 0x0400148B RID: 5259
	private GameObject[] healthBars;

	// Token: 0x0400148C RID: 5260
	private Image[] healthFill;
}
