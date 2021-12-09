using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using ZP.Net;

// Token: 0x0200020C RID: 524
public class RacerController : MinigameController
{
	// Token: 0x06000F6A RID: 3946 RVA: 0x0007A2F0 File Offset: 0x000784F0
	public override void InitializeMinigame()
	{
		base.InitializeMinigame();
		if (NetSystem.IsServer)
		{
			RacerRespawnPointGenerator componentInChildren = this.minigame_root.GetComponentInChildren<RacerRespawnPointGenerator>();
			Transform[] array = new Transform[componentInChildren.spawn_points.Length];
			int num = 0;
			foreach (Vector3 position in componentInChildren.spawn_points)
			{
				array[num] = new GameObject("TempSpawnPoint")
				{
					transform = 
					{
						position = position,
						rotation = Quaternion.Euler(new Vector3(0f, componentInChildren.spawn_rotation, 0f))
					}
				}.transform;
				num++;
			}
			base.SpawnPlayers("RacerPlayer", array);
			for (int j = 0; j < array.Length; j++)
			{
				UnityEngine.Object.Destroy(array[j].gameObject, (float)j);
			}
		}
		this.backBlocker = base.Root.transform.Find("Colliders/BackBlocker").gameObject.GetComponent<BoxCollider>();
	}

	// Token: 0x06000F6B RID: 3947 RVA: 0x0007A3F0 File Offset: 0x000785F0
	public override void StartMinigame()
	{
		this.mainUIElement = UnityEngine.Object.Instantiate<GameObject>(this.mainUIElementPrefab);
		this.mainUIElement.transform.SetParent(GameManager.UIController.MinigameUIRoot, false);
		this.individualUIElements = new GameObject[GameManager.GetPlayerCount()];
		this.placeUIElements = new GameObject[GameManager.GetPlayerCount()];
		Transform parent = this.mainUIElement.transform.Find("Bar");
		for (int i = 0; i < this.individualUIElements.Length; i++)
		{
			this.individualUIElements[i] = UnityEngine.Object.Instantiate<GameObject>(this.individualUIElementPrefab);
			this.individualUIElements[i].transform.SetParent(parent, false);
			this.individualUIElements[i].GetComponent<RawImage>().uvRect = this.portraitUVs[i % 4];
			if (i >= 4)
			{
				this.individualUIElements[i].GetComponent<RawImage>().texture = this.portraitTexture2;
			}
			if (i < 4)
			{
				this.placeUIElements[i] = UnityEngine.Object.Instantiate<GameObject>(this.UIPlacePrefabs[i]);
				this.placeUIElements[i].transform.SetParent(GameManager.UIController.MinigameUIRoot, false);
				this.placeUIElements[i].transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
			}
		}
		this.respawnPoints = GameObject.Find("RaceTrack").GetComponent<RacerRespawnPointGenerator>();
		base.CreateTimer(UIAnchorType.TopRight, new Vector2(75f, 75f), this.round_length);
		base.StartMinigame();
	}

	// Token: 0x06000F6C RID: 3948 RVA: 0x00009C3C File Offset: 0x00007E3C
	public override void StartNewRound()
	{
		base.StartNewRound();
	}

	// Token: 0x06000F6D RID: 3949 RVA: 0x00009C2C File Offset: 0x00007E2C
	public override void ResetRound()
	{
		base.ResetRound();
	}

	// Token: 0x06000F6E RID: 3950 RVA: 0x0007A570 File Offset: 0x00078770
	private void Update()
	{
		if (base.State == MinigameControllerState.Playing)
		{
			if (NetSystem.IsServer)
			{
				int num = 0;
				for (int i = 0; i < this.players.Count; i++)
				{
					if (((RacerPlayer)this.players[i]).IsFinished)
					{
						num++;
					}
				}
				if (num >= this.players.Count - 1 || this.ui_timer.time_test <= 0f)
				{
					base.EndRound(3f, 1f, false);
				}
			}
			int spawn_index = this.respawnPoints.spawn_index;
			float[] array = new float[GameManager.GetPlayerCount()];
			for (int j = 0; j < this.players.Count; j++)
			{
				RacerPlayer racerPlayer = (RacerPlayer)this.players[j];
				if (this.lastLap[j] != racerPlayer.LapCount)
				{
					this.lastLap[j] = racerPlayer.LapCount;
					this.highestIndex[j] = 0;
				}
				float num2 = 1f / (float)racerPlayer.laps_to_finish;
				int num3 = this.respawnPoints.RespawnPointIndex(racerPlayer.LastHit.triangleIndex);
				if (num3 > spawn_index)
				{
					num3 -= spawn_index;
				}
				else
				{
					num3 += this.respawnPoints.respawn_points.Count - spawn_index;
				}
				num3 = this.respawnPoints.respawn_points.Count - num3;
				if (num3 - this.highestIndex[j] > 15)
				{
					num3 = this.highestIndex[j];
				}
				if (this.highestIndex[j] >= num3)
				{
					num3 = this.highestIndex[j];
				}
				else
				{
					this.highestIndex[j] = num3;
				}
				float num4 = (float)num3 / (float)this.respawnPoints.respawn_points.Count * num2;
				num4 += (float)racerPlayer.LapCount * num2;
				if (racerPlayer.LapCount == racerPlayer.laps_to_finish)
				{
					num4 = 1f;
				}
				array[j] = num4;
				float width = this.individualUIElements[j].transform.parent.GetComponent<RectTransform>().rect.width;
				Vector3 a = new Vector3(-width / 2f + width * num4, 37f, 0f) - this.individualUIElements[j].transform.localPosition;
				Vector3 localPosition = this.individualUIElements[j].transform.localPosition + a * 0.25f;
				this.individualUIElements[j].transform.localPosition = localPosition;
			}
			if (this.timer.Elapsed(true))
			{
				for (int k = 0; k < this.players.Count; k++)
				{
					int num5 = -1;
					float num6 = float.MinValue;
					for (int l = 0; l < this.players.Count; l++)
					{
						if (array[l] >= -100f)
						{
							float num7 = (this.players[l].Score > 0) ? ((float)this.players[l].Score) : array[l];
							if (num7 > num6)
							{
								num5 = l;
								num6 = num7;
							}
						}
					}
					array[num5] = float.MinValue;
					this.positions[k] = num5;
				}
			}
			for (int m = 0; m < Mathf.Min(this.players.Count, 4); m++)
			{
				Vector3 zero = new Vector3(-1.25f, 0f, 1.25f);
				zero = Vector3.zero;
				this.placeUIElements[m].transform.position = this.minigameCameras[0].WorldToScreenPoint(this.players[this.positions[m]].transform.position + zero) + new Vector3(40f, 5f, 0f);
			}
		}
	}

	// Token: 0x06000F6F RID: 3951 RVA: 0x0000A236 File Offset: 0x00008436
	public override void BuildResults()
	{
		base.BuildResults();
	}

	// Token: 0x06000F70 RID: 3952 RVA: 0x00009C44 File Offset: 0x00007E44
	public override void ReleaseMinigame()
	{
		base.ReleaseMinigame();
	}

	// Token: 0x06000F71 RID: 3953 RVA: 0x00009C4C File Offset: 0x00007E4C
	public override bool HasLoadedLocally()
	{
		return base.HasLoadedLocally();
	}

	// Token: 0x06000F72 RID: 3954 RVA: 0x00061870 File Offset: 0x0005FA70
	public void ShowWinnerText(GamePlayer player)
	{
		string text = string.Concat(new string[]
		{
			"<color=#",
			ColorUtility.ToHtmlStringRGBA(player.Color.uiColor),
			"> ",
			player.Name,
			"</color><color=#4FF051FF> Wins </color>"
		});
		GameManager.UIController.ShowLargeText(text, LargeTextType.PlayerWins, 4.5f, true, false);
		if (NetSystem.IsServer)
		{
			base.SendRPC("ShowWinnerRPC", NetRPCDelivery.RELIABLE_SEQUENCED, new object[]
			{
				player.GlobalID
			});
		}
	}

	// Token: 0x06000F73 RID: 3955 RVA: 0x0000D486 File Offset: 0x0000B686
	[NetRPC(true, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void ShowWinnerRPC(NetPlayer sender, short globalID)
	{
		this.ShowWinnerText(GameManager.GetPlayerAt((int)globalID));
	}

	// Token: 0x06000F74 RID: 3956 RVA: 0x0000D494 File Offset: 0x0000B694
	protected override IEnumerator DoSpawnPlayersAnimation()
	{
		for (int i = 0; i < this.players.Count; i++)
		{
			this.players[i].Activate();
		}
		this.finishedSpawning = true;
		yield return null;
		yield break;
	}

	// Token: 0x04000F45 RID: 3909
	[Header("Minigame specific attributes")]
	public GameObject mainUIElementPrefab;

	// Token: 0x04000F46 RID: 3910
	public GameObject individualUIElementPrefab;

	// Token: 0x04000F47 RID: 3911
	public GameObject[] UIPlacePrefabs;

	// Token: 0x04000F48 RID: 3912
	public RenderTexture portraitTexture2;

	// Token: 0x04000F49 RID: 3913
	public BoxCollider backBlocker;

	// Token: 0x04000F4A RID: 3914
	private GameObject mainUIElement;

	// Token: 0x04000F4B RID: 3915
	private GameObject[] individualUIElements;

	// Token: 0x04000F4C RID: 3916
	private GameObject[] placeUIElements;

	// Token: 0x04000F4D RID: 3917
	private RacerRespawnPointGenerator respawnPoints;

	// Token: 0x04000F4E RID: 3918
	private Rect[] portraitUVs = new Rect[]
	{
		new Rect(0f, 0f, 0.25f, 1f),
		new Rect(0.25f, 0f, 0.25f, 1f),
		new Rect(0.5f, 0f, 0.25f, 1f),
		new Rect(0.75f, 0f, 0.25f, 1f)
	};

	// Token: 0x04000F4F RID: 3919
	private int[] highestIndex = new int[8];

	// Token: 0x04000F50 RID: 3920
	private int[] lastLap = new int[8];

	// Token: 0x04000F51 RID: 3921
	private int[] positions = new int[8];

	// Token: 0x04000F52 RID: 3922
	private ActionTimer timer = new ActionTimer(0.25f);
}
