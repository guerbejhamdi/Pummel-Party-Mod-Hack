using System;
using System.Collections.Generic;
using UnityEngine;
using ZP.Net;
using ZP.Utility;

// Token: 0x0200025C RID: 604
public class SpeedySpotlightsController : MinigameController
{
	// Token: 0x06001197 RID: 4503 RVA: 0x0000E6DD File Offset: 0x0000C8DD
	public void AddLight(Light light, float intensity)
	{
		this.lights.Add(light);
		this.lightIntensities.Add(intensity);
	}

	// Token: 0x06001198 RID: 4504 RVA: 0x00088D24 File Offset: 0x00086F24
	public override void InitializeMinigame()
	{
		base.InitializeMinigame();
		this.baseAmbientColor = RenderSettings.ambientLight;
		if (NetSystem.IsServer)
		{
			base.SpawnPlayers("SpeedySpotlightsPlayer", null);
		}
		this.cameraShake = base.Root.GetComponentInChildren<CameraShake>();
		this.lightSpawnTimer.SetInterval(this.spawnInterval, true);
		this.lights.AddRange(base.Root.transform.Find("Lighting").GetComponentsInChildren<Light>());
		for (int i = 0; i < this.lights.Count; i++)
		{
			this.lightIntensities.Add(this.lights[i].intensity);
		}
	}

	// Token: 0x06001199 RID: 4505 RVA: 0x00088DD0 File Offset: 0x00086FD0
	public override void StartMinigame()
	{
		base.CreateTimer(UIAnchorType.TopRight, new Vector2(75f, 75f), this.round_length);
		base.CreateScoreUI(UIAnchorType.TopLeft, new Vector2(145f, 45f), 68f, false);
		GameManager.SetGlobalPlayerEmission(0f);
		for (int i = 0; i < this.ui_score.Length; i++)
		{
			if (!(this.ui_score[i] == null))
			{
				this.ui_score[i].scoreUpdateSpeed = 100;
				this.ui_score[i].minChangeText = 10;
				this.ui_score[i].showChanges = false;
				this.ui_score[i].SetScore(100);
				((SpeedySpotlightsPlayer)this.players[i]).Health = 100;
				this.ui_score[i].showChanges = true;
			}
		}
		this.startTime = Time.time;
		base.StartMinigame();
	}

	// Token: 0x0600119A RID: 4506 RVA: 0x00088EB4 File Offset: 0x000870B4
	public override void RoundEnded()
	{
		for (int i = 0; i < this.players.Count; i++)
		{
			if (this.players[i].GamePlayer.IsLocalPlayer && !this.players[i].GamePlayer.IsAI && ((SpeedySpotlightsPlayer)this.players[i]).Health >= 100)
			{
				PlatformAchievementManager.Instance.TriggerAchievement("ACH_DAMAGE_SPOTLIGHTS");
			}
			if (NetSystem.IsServer)
			{
				if (!((SpeedySpotlightsPlayer)this.players[i]).IsDead)
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
		base.RoundEnded();
	}

	// Token: 0x0600119B RID: 4507 RVA: 0x00009C2C File Offset: 0x00007E2C
	public override void ResetRound()
	{
		base.ResetRound();
	}

	// Token: 0x0600119C RID: 4508 RVA: 0x00009C34 File Offset: 0x00007E34
	public override void RoundStarting()
	{
		base.RoundStarting();
	}

	// Token: 0x0600119D RID: 4509 RVA: 0x0000E6F7 File Offset: 0x0000C8F7
	public override void StartNewRound()
	{
		base.StartNewRound();
		this.pulseTimer.Start();
	}

	// Token: 0x0600119E RID: 4510 RVA: 0x00088FCC File Offset: 0x000871CC
	private void Update()
	{
		if (base.State == MinigameControllerState.Playing || base.State == MinigameControllerState.PreScoreScreen)
		{
			if (this.pulseTimer.Elapsed(true) || base.State == MinigameControllerState.PreScoreScreen)
			{
				this.pulseStartTime = Time.time;
				this.pulsing = true;
			}
			if (this.pulsing)
			{
				float num = (Time.time - this.pulseStartTime) / this.pulseLength;
				if (num >= 1f)
				{
					this.pulsing = false;
					this.SetIntensity(0f);
					RenderSettings.ambientLight = Color.black;
				}
				else
				{
					float num2 = this.pulseCurve.Evaluate(num);
					this.SetIntensity(num2);
					for (int i = 0; i < this.lights.Count; i++)
					{
						this.lights[i].intensity = num2 * this.lightIntensities[i];
					}
					RenderSettings.ambientLight = this.baseAmbientColor * num2;
				}
			}
			else
			{
				float num3 = Mathf.Clamp01((Time.time - this.startTime) / 2f);
				for (int j = 0; j < this.lights.Count; j++)
				{
					this.lights[j].intensity = (1f - num3) * this.lightIntensities[j];
				}
				RenderSettings.ambientLight = (1f - num3) * this.baseAmbientColor;
			}
		}
		if (base.State == MinigameControllerState.Playing)
		{
			if (NetSystem.IsServer)
			{
				if (this.ui_timer.time_test <= 0f || this.players_alive <= 1)
				{
					base.EndRound(1f, 3f, false);
				}
				if (this.lightSpawnTimer.Elapsed(false))
				{
					float time = (this.round_length - this.ui_timer.time_test) / this.round_length;
					this.lightSpawnTimer.SetInterval(this.spawningIntervalCurve.Evaluate(time) * this.spawnInterval, true);
					float angle = ZPMath.RandomFloat(GameManager.rand, 11f, 15f);
					float yRot = ZPMath.RandomFloat(GameManager.rand, -360f, 360f);
					float yRotSpeed = ZPMath.RandomFloat(GameManager.rand, -15f, 15f);
					float xRotSpeed = ZPMath.RandomFloat(GameManager.rand, 8f, 25f);
					this.SpawnSpotLight(this.lightSpawnPoint, angle, yRot, yRotSpeed, xRotSpeed);
				}
			}
			int k = 0;
			while (k < this.minigameSpotlights.Count)
			{
				MinigameSpotlight minigameSpotlight = this.minigameSpotlights[k];
				Transform transform = minigameSpotlight.gameObject.transform;
				minigameSpotlight.xRot += this.minigameSpotlights[k].xRotSpeed * Time.deltaTime;
				minigameSpotlight.yRot += this.minigameSpotlights[k].yRotSpeed * Time.deltaTime;
				if (minigameSpotlight.yRot > 360f)
				{
					minigameSpotlight.yRot -= 360f;
				}
				else if (minigameSpotlight.yRot < 0f)
				{
					minigameSpotlight.yRot += 360f;
				}
				Quaternion rotation = Quaternion.Euler(minigameSpotlight.xRot, minigameSpotlight.yRot, 0f);
				this.minigameSpotlights[k].gameObject.transform.rotation = rotation;
				if (minigameSpotlight.xRot > 155f)
				{
					UnityEngine.Object.Destroy(this.minigameSpotlights[k].gameObject);
					this.minigameSpotlights.RemoveAt(k);
				}
				else
				{
					k++;
				}
			}
		}
	}

	// Token: 0x0600119F RID: 4511 RVA: 0x00089360 File Offset: 0x00087560
	private void FixedUpdate()
	{
		if (base.State == MinigameControllerState.Playing || base.State == MinigameControllerState.PreScoreScreen)
		{
			for (int i = 0; i < this.players.Count; i++)
			{
				SpeedySpotlightsPlayer speedySpotlightsPlayer = (SpeedySpotlightsPlayer)this.players[i];
				if (speedySpotlightsPlayer.IsOwner && !speedySpotlightsPlayer.IsDead)
				{
					bool flag = false;
					for (int j = 0; j < this.minigameSpotlights.Count; j++)
					{
						MinigameSpotlight minigameSpotlight = this.minigameSpotlights[j];
						if (minigameSpotlight != null && Vector3.Angle(speedySpotlightsPlayer.transform.position - this.lightSpawnPoint, minigameSpotlight.gameObject.transform.forward) < minigameSpotlight.angle * 0.9f * 0.5f)
						{
							flag = true;
						}
					}
					speedySpotlightsPlayer.BeingHit = flag;
					if (flag)
					{
						speedySpotlightsPlayer.Health = (byte)Mathf.Clamp((int)(speedySpotlightsPlayer.Health - 2), 0, 100);
					}
				}
			}
		}
	}

	// Token: 0x060011A0 RID: 4512 RVA: 0x00089454 File Offset: 0x00087654
	private void SetIntensity(float intensity)
	{
		for (int i = 0; i < this.players.Count; i++)
		{
			SpeedySpotlightsPlayer speedySpotlightsPlayer = (SpeedySpotlightsPlayer)this.players[i];
			speedySpotlightsPlayer.pointLight.intensity = intensity * speedySpotlightsPlayer.lightIntensity;
		}
	}

	// Token: 0x060011A1 RID: 4513 RVA: 0x0000A7A0 File Offset: 0x000089A0
	public void PlayerDied(SpeedySpotlightsPlayer player)
	{
		if (NetSystem.IsServer)
		{
			player.RoundScore = (short)(this.players.Count - this.players_alive);
		}
		this.players_alive--;
	}

	// Token: 0x060011A2 RID: 4514 RVA: 0x0000BACF File Offset: 0x00009CCF
	public override void ReleaseMinigame()
	{
		GameManager.SetGlobalPlayerEmission(1f);
		base.ReleaseMinigame();
	}

	// Token: 0x060011A3 RID: 4515 RVA: 0x00009C4C File Offset: 0x00007E4C
	public override bool HasLoadedLocally()
	{
		return base.HasLoadedLocally();
	}

	// Token: 0x060011A4 RID: 4516 RVA: 0x0000E70A File Offset: 0x0000C90A
	[NetRPC(false, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void RPCSpawnSpotLight(NetPlayer player, Vector3 pos, float angle, float yRot, float yRotSpeed, float xRotSpeed)
	{
		this.SpawnSpotLight(pos, angle, yRot, yRotSpeed, xRotSpeed);
	}

	// Token: 0x060011A5 RID: 4517 RVA: 0x0008949C File Offset: 0x0008769C
	private void SpawnSpotLight(Vector3 pos, float angle, float yRot, float yRotSpeed, float xRotSpeed)
	{
		GameObject gameObject = base.Spawn(this.spotlightPrefab, pos, Quaternion.Euler(45f, yRot, 0f));
		MinigameSpotlight item = new MinigameSpotlight(pos, angle, yRot, yRotSpeed, xRotSpeed, gameObject);
		this.minigameSpotlights.Add(item);
		if (NetSystem.IsServer)
		{
			base.SendRPC("RPCSpawnSpotLight", NetRPCDelivery.RELIABLE_UNORDERED, new object[]
			{
				pos,
				angle,
				yRot,
				yRotSpeed,
				xRotSpeed
			});
		}
	}

	// Token: 0x04001247 RID: 4679
	[Header("Minigame specific attributes")]
	public GameObject spotlightPrefab;

	// Token: 0x04001248 RID: 4680
	public AnimationCurve spawningIntervalCurve;

	// Token: 0x04001249 RID: 4681
	public float spawnInterval = 2f;

	// Token: 0x0400124A RID: 4682
	public Vector3 lightSpawnPoint = new Vector3(0f, 20f, 0f);

	// Token: 0x0400124B RID: 4683
	private CameraShake cameraShake;

	// Token: 0x0400124C RID: 4684
	[HideInInspector]
	public List<MinigameSpotlight> minigameSpotlights = new List<MinigameSpotlight>();

	// Token: 0x0400124D RID: 4685
	private ActionTimer lightSpawnTimer = new ActionTimer(2f);

	// Token: 0x0400124E RID: 4686
	public AnimationCurve pulseCurve;

	// Token: 0x0400124F RID: 4687
	private ActionTimer pulseTimer = new ActionTimer(9f);

	// Token: 0x04001250 RID: 4688
	[HideInInspector]
	public bool pulsing;

	// Token: 0x04001251 RID: 4689
	private float pulseStartTime;

	// Token: 0x04001252 RID: 4690
	private float pulseLength = 2f;

	// Token: 0x04001253 RID: 4691
	private List<Light> lights = new List<Light>();

	// Token: 0x04001254 RID: 4692
	private List<float> lightIntensities = new List<float>();

	// Token: 0x04001255 RID: 4693
	private float startTime;

	// Token: 0x04001256 RID: 4694
	private Color baseAmbientColor;
}
