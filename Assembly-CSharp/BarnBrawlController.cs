using System;
using System.Collections.Generic;
using LlockhamIndustries.Decals;
using UnityEngine;
using UnityEngine.UI;
using ZP.Net;

// Token: 0x02000147 RID: 327
public class BarnBrawlController : MinigameController
{
	// Token: 0x06000947 RID: 2375 RVA: 0x00053008 File Offset: 0x00051208
	public override void InitializeMinigame()
	{
		if (NetSystem.IsServer)
		{
			base.SpawnPlayers("BarnBrawlPlayer", null);
		}
		base.InitializeMinigame();
		Transform transform = base.Root.transform.Find("ShotgunSpawnPoints");
		this.shotgunSpawnPoints = new Transform[transform.childCount];
		this.ShotgunPickups = new BarnBrawlShotgunPickup[transform.childCount];
		for (int i = 0; i < transform.childCount; i++)
		{
			this.shotgunSpawnPoints[i] = transform.GetChild(i);
			if (NetSystem.IsServer && i < 4)
			{
				this.ShotgunPickups[i] = base.NetSpawn("BarnBrawlShotgunPickup", this.shotgunSpawnPoints[i].position, 0, null).GetComponent<BarnBrawlShotgunPickup>();
			}
		}
		this.killFeed = UnityEngine.Object.Instantiate<GameObject>(this.killFeedPrefab, GameManager.UIController.MinigameUIRoot);
	}

	// Token: 0x06000948 RID: 2376 RVA: 0x00009BEC File Offset: 0x00007DEC
	public override void StartMinigame()
	{
		base.CreateTimer(UIAnchorType.TopRight, new Vector2(75f, 75f), this.round_length);
		base.CreateScoreUI(UIAnchorType.TopLeft, new Vector2(145f, 45f), 68f, false);
		base.StartMinigame();
	}

	// Token: 0x06000949 RID: 2377 RVA: 0x0000A3A2 File Offset: 0x000085A2
	public override void OnPlayersReady()
	{
		base.OnPlayersReady();
		GameManager.CaptureInput = true;
	}

	// Token: 0x0600094A RID: 2378 RVA: 0x00009C34 File Offset: 0x00007E34
	public override void RoundStarting()
	{
		base.RoundStarting();
	}

	// Token: 0x0600094B RID: 2379 RVA: 0x00009C3C File Offset: 0x00007E3C
	public override void StartNewRound()
	{
		base.StartNewRound();
	}

	// Token: 0x0600094C RID: 2380 RVA: 0x00009C2C File Offset: 0x00007E2C
	public override void ResetRound()
	{
		base.ResetRound();
	}

	// Token: 0x0600094D RID: 2381 RVA: 0x0000A22E File Offset: 0x0000842E
	public override void RoundEnded()
	{
		base.RoundEnded();
	}

	// Token: 0x0600094E RID: 2382 RVA: 0x000530D4 File Offset: 0x000512D4
	private void Update()
	{
		if (base.State == MinigameControllerState.Playing && NetSystem.IsServer)
		{
			if (this.ui_timer.time_test <= 0f)
			{
				base.EndRound(1f, 3f, false);
			}
			int num = 0;
			for (int i = 0; i < this.ShotgunPickups.Length; i++)
			{
				if (this.ShotgunPickups[i] != null)
				{
					num++;
				}
			}
			if (this.shotgunSpawnTimer.Elapsed(true) || num < 2)
			{
				List<int> list = new List<int>();
				for (int j = 0; j < this.shotgunSpawnPoints.Length; j++)
				{
					list.Add(j);
				}
				while (list.Count > 0)
				{
					int index = GameManager.rand.Next(0, list.Count);
					if (this.ShotgunPickups[list[index]] == null)
					{
						this.ShotgunPickups[list[index]] = base.NetSpawn("BarnBrawlShotgunPickup", this.shotgunSpawnPoints[list[index]].position, 0, null).GetComponent<BarnBrawlShotgunPickup>();
						return;
					}
					list.RemoveAt(index);
				}
			}
		}
	}

	// Token: 0x0600094F RID: 2383 RVA: 0x0000A3B0 File Offset: 0x000085B0
	public override void ReleaseMinigame()
	{
		GameManager.CaptureInput = false;
		base.ReleaseMinigame();
	}

	// Token: 0x06000950 RID: 2384 RVA: 0x00009C4C File Offset: 0x00007E4C
	public override bool HasLoadedLocally()
	{
		return base.HasLoadedLocally();
	}

	// Token: 0x06000951 RID: 2385 RVA: 0x0000A236 File Offset: 0x00008436
	public override void BuildResults()
	{
		base.BuildResults();
	}

	// Token: 0x06000952 RID: 2386 RVA: 0x000531EC File Offset: 0x000513EC
	public void AddKillFeed(BarnBrawlPlayer killer, BarnBrawlPlayer victim)
	{
		if (this.killFeed == null)
		{
			return;
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.killFeedTextPrefab, this.killFeed.transform);
		Color32 c = killer.GamePlayer.Color.uiColor;
		Color32 c2 = victim.GamePlayer.Color.uiColor;
		c.a = 200;
		c2.a = 200;
		string text = ColorUtility.ToHtmlStringRGBA(c);
		string text2 = ColorUtility.ToHtmlStringRGBA(c2);
		Text component = gameObject.GetComponent<Text>();
		component.text = string.Concat(new string[]
		{
			"<color=#",
			text,
			">",
			killer.GamePlayer.Name,
			"</color>"
		});
		Text text3 = component;
		text3.text += " killed ";
		Text text4 = component;
		text4.text = string.Concat(new string[]
		{
			text4.text,
			"<color=#",
			text2,
			">",
			victim.GamePlayer.Name,
			"</color>"
		});
	}

	// Token: 0x06000953 RID: 2387 RVA: 0x00053320 File Offset: 0x00051520
	public void HandleImpact(RaycastHit hit)
	{
		if (hit.collider.sharedMaterial != null)
		{
			string name = hit.collider.sharedMaterial.name;
			if (name != null && name == "Wood")
			{
				this.SpawnDecal(hit, this.woodHitEffect);
			}
		}
	}

	// Token: 0x06000954 RID: 2388 RVA: 0x00053370 File Offset: 0x00051570
	private void SpawnDecal(RaycastHit hit, GameObject prefab)
	{
		Quaternion rotation = Quaternion.LookRotation(hit.normal);
		UnityEngine.Object.Instantiate<GameObject>(prefab, hit.point, rotation).transform.SetParent(hit.collider.transform);
		rotation = Quaternion.LookRotation(-hit.normal);
		this.printer.Print(hit.point, rotation, hit.collider.transform, 0);
	}

	// Token: 0x040007D8 RID: 2008
	[Header("Barn Brawl Attributes")]
	public Printer printer;

	// Token: 0x040007D9 RID: 2009
	public GameObject woodHitEffect;

	// Token: 0x040007DA RID: 2010
	public GameObject killFeedPrefab;

	// Token: 0x040007DB RID: 2011
	public GameObject killFeedTextPrefab;

	// Token: 0x040007DC RID: 2012
	public GameObject camPrefab;

	// Token: 0x040007DD RID: 2013
	public BarnBrawlShotgunPickup[] ShotgunPickups;

	// Token: 0x040007DE RID: 2014
	private Transform[] shotgunSpawnPoints;

	// Token: 0x040007DF RID: 2015
	private ActionTimer shotgunSpawnTimer = new ActionTimer(2f, 4f);

	// Token: 0x040007E0 RID: 2016
	private GameObject killFeed;
}
