using System;
using UnityEngine;
using UnityEngine.UI;
using ZP.Net;

// Token: 0x020001B3 RID: 435
public class HorseFighterController : MinigameController
{
	// Token: 0x06000C79 RID: 3193 RVA: 0x00068908 File Offset: 0x00066B08
	public override void InitializeMinigame()
	{
		if (NetSystem.IsServer)
		{
			base.SpawnPlayers("HorseFighterPlayer", null);
		}
		if (GameManager.GetLocalNonAIPlayers().Count == 0)
		{
			Vector3 position = new Vector3(-6.677f, 4.41f, -6.73f);
			Quaternion transform = Quaternion.Euler(19.1f, 43.6f, 0f);
			GameObject gameObject = base.Spawn(this.camPrefab, position, transform);
			Camera componentInChildren = gameObject.GetComponentInChildren<Camera>();
			gameObject.GetComponent<AudioListener>().enabled = true;
			this.minigameCameras.Add(componentInChildren);
		}
		base.InitializeMinigame();
		this.killFeed = UnityEngine.Object.Instantiate<GameObject>(this.killFeedPrefab, GameManager.UIController.Canvas.transform);
	}

	// Token: 0x06000C7A RID: 3194 RVA: 0x00009BEC File Offset: 0x00007DEC
	public override void StartMinigame()
	{
		base.CreateTimer(UIAnchorType.TopRight, new Vector2(75f, 75f), this.round_length);
		base.CreateScoreUI(UIAnchorType.TopLeft, new Vector2(145f, 45f), 68f, false);
		base.StartMinigame();
	}

	// Token: 0x06000C7B RID: 3195 RVA: 0x0000A3A2 File Offset: 0x000085A2
	public override void OnPlayersReady()
	{
		base.OnPlayersReady();
		GameManager.CaptureInput = true;
	}

	// Token: 0x06000C7C RID: 3196 RVA: 0x00009C34 File Offset: 0x00007E34
	public override void RoundStarting()
	{
		base.RoundStarting();
	}

	// Token: 0x06000C7D RID: 3197 RVA: 0x00009C3C File Offset: 0x00007E3C
	public override void StartNewRound()
	{
		base.StartNewRound();
	}

	// Token: 0x06000C7E RID: 3198 RVA: 0x00009C2C File Offset: 0x00007E2C
	public override void ResetRound()
	{
		base.ResetRound();
	}

	// Token: 0x06000C7F RID: 3199 RVA: 0x0000A22E File Offset: 0x0000842E
	public override void RoundEnded()
	{
		base.RoundEnded();
	}

	// Token: 0x06000C80 RID: 3200 RVA: 0x0000AB1C File Offset: 0x00008D1C
	private void Update()
	{
		if (base.State == MinigameControllerState.Playing && NetSystem.IsServer && this.ui_timer.time_test <= 0f)
		{
			base.EndRound(1f, 3f, false);
		}
	}

	// Token: 0x06000C81 RID: 3201 RVA: 0x0000A3B0 File Offset: 0x000085B0
	public override void ReleaseMinigame()
	{
		GameManager.CaptureInput = false;
		base.ReleaseMinigame();
	}

	// Token: 0x06000C82 RID: 3202 RVA: 0x00009C4C File Offset: 0x00007E4C
	public override bool HasLoadedLocally()
	{
		return base.HasLoadedLocally();
	}

	// Token: 0x06000C83 RID: 3203 RVA: 0x0000A236 File Offset: 0x00008436
	public override void BuildResults()
	{
		base.BuildResults();
	}

	// Token: 0x06000C84 RID: 3204 RVA: 0x000689B4 File Offset: 0x00066BB4
	public void AddKillFeed(HorseFighterPlayer killer, HorseFighterPlayer victim)
	{
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

	// Token: 0x04000BD6 RID: 3030
	[Header("Horse Fighter Attributes")]
	public GameObject killFeedPrefab;

	// Token: 0x04000BD7 RID: 3031
	public GameObject killFeedTextPrefab;

	// Token: 0x04000BD8 RID: 3032
	public GameObject camPrefab;

	// Token: 0x04000BD9 RID: 3033
	private GameObject killFeed;
}
