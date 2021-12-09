using System;
using UnityEngine;
using ZP.Net;

// Token: 0x02000246 RID: 582
public class SnakeController : MinigameController
{
	// Token: 0x060010C9 RID: 4297 RVA: 0x0008341C File Offset: 0x0008161C
	public override void InitializeMinigame()
	{
		base.InitializeMinigame();
		QualitySettings.shadowDistance = 100f;
		if (NetSystem.IsServer)
		{
			base.SpawnPlayers("SnakePlayer", null);
		}
		GameManager.RagdollSettings.DespawnType = RagdollDespawnType.MaxRagdolls;
		GameManager.RagdollSettings.MaxRagdolls = 8;
		GameManager.RagdollSettings.DespawnEffect = RagdollDespawnEffect.Sink;
		this.trainSpawnPoint = base.Root.transform.Find("TrainSpawnPoint");
	}

	// Token: 0x060010CA RID: 4298 RVA: 0x0000DF6C File Offset: 0x0000C16C
	public override void StartMinigame()
	{
		base.CreateTimer(UIAnchorType.TopRight, new Vector2(75f, 75f), 120f);
		base.CreateScoreUI(UIAnchorType.TopLeft, new Vector2(145f, 45f), 68f, false);
		base.StartMinigame();
	}

	// Token: 0x060010CB RID: 4299 RVA: 0x00009C34 File Offset: 0x00007E34
	public override void RoundStarting()
	{
		base.RoundStarting();
	}

	// Token: 0x060010CC RID: 4300 RVA: 0x00083488 File Offset: 0x00081688
	public override void StartNewRound()
	{
		this.curTrain = base.Spawn(this.trainPrefab, this.trainSpawnPoint.position, this.trainSpawnPoint.rotation);
		UnityEngine.Object.Destroy(this.curTrain, this.trainKillTime);
		this.cur_max_age = this.startMaxAge;
		base.StartNewRound();
	}

	// Token: 0x060010CD RID: 4301 RVA: 0x00009C2C File Offset: 0x00007E2C
	public override void ResetRound()
	{
		base.ResetRound();
	}

	// Token: 0x060010CE RID: 4302 RVA: 0x000834E0 File Offset: 0x000816E0
	public override void RoundEnded()
	{
		if (NetSystem.IsServer)
		{
			for (int i = 0; i < this.players.Count; i++)
			{
				if (!((SnakePlayer)this.players[i]).IsDead)
				{
					CharacterBase characterBase = this.players[i];
					characterBase.Score += (short)((this.players.Count - 1) * 100);
				}
				else
				{
					CharacterBase characterBase2 = this.players[i];
					characterBase2.Score += this.players[i].RoundScore * 100;
					this.players[i].RoundScore = 0;
				}
			}
		}
		base.RoundEnded();
	}

	// Token: 0x060010CF RID: 4303 RVA: 0x000835A0 File Offset: 0x000817A0
	private void Update()
	{
		if (base.State == MinigameControllerState.Playing)
		{
			if (NetSystem.IsServer && this.players_alive <= 1)
			{
				base.EndRound(1.75f, 1f, false);
			}
			else
			{
				this.cur_max_age += this.maxeAgeGainSpeed * Time.deltaTime;
			}
			if (this.curTrain != null)
			{
				this.trainPoints[0] = new Vector2(this.curTrain.transform.position.x - this.trainSize.x, this.curTrain.transform.position.z + this.trainSize.y);
				this.trainPoints[1] = new Vector2(this.curTrain.transform.position.x + this.trainSize.x, this.curTrain.transform.position.z + this.trainSize.y);
				this.trainPoints[2] = new Vector2(this.curTrain.transform.position.x + this.trainSize.x, this.curTrain.transform.position.z - this.trainSize.y);
				this.trainPoints[3] = new Vector2(this.curTrain.transform.position.x - this.trainSize.x, this.curTrain.transform.position.z - this.trainSize.y);
				for (int i = 0; i < this.trainPoints.Length; i++)
				{
					Vector2 vector = this.trainPoints[i];
					Vector2 vector2 = (i == this.trainPoints.Length - 1) ? this.trainPoints[0] : this.trainPoints[i + 1];
					new Vector3(vector.x, 0f, vector.y);
					new Vector3(vector2.x, 0f, vector2.y);
				}
			}
			if (NetSystem.IsServer && this.deltaTotal >= 1f)
			{
				this.deltaTotal = 0f;
			}
		}
	}

	// Token: 0x060010D0 RID: 4304 RVA: 0x0000398C File Offset: 0x00001B8C
	public void FixedUpdate()
	{
	}

	// Token: 0x060010D1 RID: 4305 RVA: 0x00009C44 File Offset: 0x00007E44
	public override void ReleaseMinigame()
	{
		base.ReleaseMinigame();
	}

	// Token: 0x060010D2 RID: 4306 RVA: 0x00009C4C File Offset: 0x00007E4C
	public override bool HasLoadedLocally()
	{
		return base.HasLoadedLocally();
	}

	// Token: 0x060010D3 RID: 4307 RVA: 0x0000A236 File Offset: 0x00008436
	public override void BuildResults()
	{
		base.BuildResults();
	}

	// Token: 0x060010D4 RID: 4308 RVA: 0x0000A7A0 File Offset: 0x000089A0
	public void PlayerDied(SnakePlayer player)
	{
		if (NetSystem.IsServer)
		{
			player.RoundScore = (short)(this.players.Count - this.players_alive);
		}
		this.players_alive--;
	}

	// Token: 0x04001151 RID: 4433
	[Header("Minigame specific attributes")]
	public GameObject trainPrefab;

	// Token: 0x04001152 RID: 4434
	public float trainKillTime = 17f;

	// Token: 0x04001153 RID: 4435
	public int max_trail_length = 50;

	// Token: 0x04001154 RID: 4436
	public float cur_max_age = 5f;

	// Token: 0x04001155 RID: 4437
	public GameObject curTrain;

	// Token: 0x04001156 RID: 4438
	private float startMaxAge = 1f;

	// Token: 0x04001157 RID: 4439
	private float maxeAgeGainSpeed = 0.2f;

	// Token: 0x04001158 RID: 4440
	private Transform trainSpawnPoint;

	// Token: 0x04001159 RID: 4441
	public Vector2 trainSize = new Vector2(5f, 10f);

	// Token: 0x0400115A RID: 4442
	public Vector2[] trainPoints = new Vector2[4];

	// Token: 0x0400115B RID: 4443
	private float deltaTotal;

	// Token: 0x0400115C RID: 4444
	private int i;
}
