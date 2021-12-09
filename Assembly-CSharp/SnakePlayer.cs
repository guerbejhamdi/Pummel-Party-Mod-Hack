using System;
using System.Collections.Generic;
using Rewired;
using UnityEngine;
using ZP.Net;
using ZP.Utility;

// Token: 0x02000247 RID: 583
public class SnakePlayer : Movement1
{
	// Token: 0x060010D6 RID: 4310 RVA: 0x00083854 File Offset: 0x00081A54
	public override void OnNetInitialize()
	{
		base.OnNetInitialize();
		this.minigameController = (SnakeController)GameManager.Minigame;
		this.minigameController.AddPlayer(this);
		this.minigameController.Root.GetComponentInChildren<CameraFollow>().AddTarget(this);
		this.mover = base.GetComponent<CharacterMover>();
		this.trail = this.minigameController.Spawn(this.trail_prefab, new Vector3(0f, 0.78f, 0f), Quaternion.identity);
		this.rootStartPos = this.player_root.transform.localPosition;
		this.isInitialized = true;
		if (!NetSystem.IsServer)
		{
			this.score.Recieve = new RecieveProxy(base.ScoreRecieve);
		}
		this.playerRadiusSqr = this.playerRadius * this.playerRadius;
		this.avoidDistanceSqr = this.avoidDistance * this.avoidDistance;
		switch (base.GamePlayer.Difficulty)
		{
		case BotDifficulty.Easy:
			this.avoidDistance = 1.5f;
			return;
		case BotDifficulty.Normal:
			this.avoidDistance = 2.9f;
			return;
		case BotDifficulty.Hard:
			this.avoidDistance = 4f;
			return;
		default:
			return;
		}
	}

	// Token: 0x060010D7 RID: 4311 RVA: 0x0000DFAB File Offset: 0x0000C1AB
	private void Awake()
	{
		base.InitializeController();
		this.lights = base.GetComponentsInChildren<Light>();
	}

	// Token: 0x060010D8 RID: 4312 RVA: 0x00083978 File Offset: 0x00081B78
	protected override void Start()
	{
		this.cameraShake = UnityEngine.Object.FindObjectOfType<CameraShake>();
		MeshRenderer component = this.trail.GetComponent<MeshRenderer>();
		component.materials[0].SetColor("_TintColor", base.GamePlayer.Color.skinColor1);
		component.materials[1].SetColor("_TintColor", base.GamePlayer.Color.skinColor1);
		Light[] array = this.lights;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].color = base.GamePlayer.Color.skinColor1;
		}
		MeshRenderer component2 = this.hoverBoard.GetComponent<MeshRenderer>();
		component2.material.SetColor("_EmissionColor", base.GamePlayer.Color.skinColor1);
		component2.material.SetColor("_BoardColor", base.GamePlayer.Color.skinColor1);
		this.boardSparks.material.SetColor("_TintColor", base.GamePlayer.Color.skinColor1);
		base.Start();
	}

	// Token: 0x060010D9 RID: 4313 RVA: 0x00083A80 File Offset: 0x00081C80
	private void Update()
	{
		base.UpdateController();
		if (this.isInitialized)
		{
			if (!base.IsOwner || this.player.IsAI)
			{
				Vector3 normalized = new Vector3(this.velocity.Value.x, 0f, this.velocity.Value.z).normalized;
				if (normalized == this.lastVelocity)
				{
					this.targetLeanAngle = 0f;
				}
				else
				{
					Vector3 vector = Vector3.Cross(this.lastVelocity, Vector3.up);
					float num = Vector3.Dot(normalized, vector);
					float num2 = Vector3.Dot(normalized, -vector);
					this.targetLeanAngle = ((num > num2) ? this.maxLeanAngle : (-this.maxLeanAngle));
				}
				this.lastVelocity = normalized;
			}
			this.curLeanAngle = Mathf.MoveTowardsAngle(this.curLeanAngle, this.targetLeanAngle, this.leanSpeed * Time.deltaTime);
			Vector3 eulerAngles = this.player_root.transform.rotation.eulerAngles;
			eulerAngles.z = this.curLeanAngle;
			this.player_root.transform.rotation = Quaternion.Euler(eulerAngles);
			Vector3 localPosition = this.rootStartPos;
			localPosition.y += this.board_height_bob.Evaluate(Time.time * this.bob_speed) * this.height_bob_amount;
			this.player_root.transform.localPosition = localPosition;
		}
	}

	// Token: 0x060010DA RID: 4314 RVA: 0x00083BF4 File Offset: 0x00081DF4
	private bool CheckLine(Vector2 p1, Vector2 p2, Vector2 pos2D, bool draw)
	{
		Vector2 closestPointOnLineSegment = ZPMath.GetClosestPointOnLineSegment(p1, p2, pos2D);
		Vector2 vector = pos2D - closestPointOnLineSegment;
		if (draw)
		{
			Debug.DrawLine(new Vector3(pos2D.x, 0f, pos2D.y), new Vector3(closestPointOnLineSegment.x, 0f, closestPointOnLineSegment.y), Color.blue);
		}
		return vector.sqrMagnitude < this.playerRadiusSqr;
	}

	// Token: 0x060010DB RID: 4315 RVA: 0x00083C5C File Offset: 0x00081E5C
	public void FixedUpdate()
	{
		Vector2 pos2D = new Vector2(base.transform.position.x, base.transform.position.z);
		if (base.IsOwner && this.minigameController.Playable && !this.isDead)
		{
			bool flag = false;
			short num = 0;
			while ((int)num < this.minigameController.GetPlayerCount())
			{
				SnakePlayer snakePlayer = (SnakePlayer)this.minigameController.GetPlayerInSlot(num);
				for (int i = 0; i < snakePlayer.points.Count - 1; i++)
				{
					if (this.CheckLine(snakePlayer.points[i].pos, snakePlayer.points[i + 1].pos, pos2D, false))
					{
						this.KillPlayer(true);
						flag = true;
						break;
					}
				}
				if (flag)
				{
					break;
				}
				num += 1;
			}
			if (this.minigameController.curTrain != null && !flag)
			{
				for (int j = 0; j < this.minigameController.trainPoints.Length; j++)
				{
					Vector2 vector = this.minigameController.trainPoints[j];
					Vector2 vector2 = (j == this.minigameController.trainPoints.Length - 1) ? this.minigameController.trainPoints[0] : this.minigameController.trainPoints[j + 1];
					Debug.DrawLine(new Vector3(vector.x, 0f, vector.y), new Vector3(vector2.x, 0f, vector2.y), Color.red);
					if (this.CheckLine(vector, vector2, pos2D, true))
					{
						if (!this.player.IsAI)
						{
							PlatformAchievementManager.Instance.TriggerAchievement("ACH_TEMPORAL_TRAILS");
						}
						this.KillPlayer(true);
						break;
					}
				}
			}
		}
		if (this.minigameController.Playable)
		{
			if (!base.IsDead)
			{
				Vector3 position = this.trailSource.position;
				this.points.Add(new SnakePlayer.TrailPoint(new Vector2(position.x, position.z), base.transform.right, Time.time, this.minigameController.cur_max_age));
			}
			int num2 = 0;
			while (num2 < this.points.Count && Time.time - this.points[num2].time > this.points[num2].life)
			{
				this.points.RemoveAt(num2);
			}
			this.UpdateTrail();
		}
	}

	// Token: 0x060010DC RID: 4316 RVA: 0x00083EEC File Offset: 0x000820EC
	private void UpdateTrail()
	{
		if (this.trailMesh == null)
		{
			this.trailMesh = new Mesh();
			this.trailMesh.MarkDynamic();
		}
		else
		{
			this.trailMesh.Clear();
		}
		this.verts.Clear();
		this.vIndices.Clear();
		this.hIndices.Clear();
		this.uvs.Clear();
		for (int i = 0; i < this.points.Count; i++)
		{
			float num = this.trail_size.Evaluate((Time.time - this.points[i].time) / this.points[i].life) * this.trail_size_scale;
			float num2 = this.trailHeight * num;
			this.verts.Add(new Vector3(this.points[i].pos.x, 0f, this.points[i].pos.y) - new Vector3(0f, num2 / 2f, 0f));
			this.verts.Add(this.verts[this.verts.Count - 1] + new Vector3(0f, num2, 0f));
			this.uvs.Add(new Vector2((i % 2 == 0) ? 0f : 1f, 0f));
			this.uvs.Add(new Vector2((i % 2 == 0) ? 0f : 1f, 0.9f));
		}
		int count = this.verts.Count;
		for (int j = 0; j < this.points.Count; j++)
		{
			float num3 = this.trail_size.Evaluate((Time.time - this.points[j].time) / this.points[j].life) * this.trail_size_scale;
			float num4 = this.trailHeight * num3;
			this.verts.Add(new Vector3(this.points[j].pos.x, 0f, this.points[j].pos.y) - this.points[j].tangent * (num4 / 2f));
			this.verts.Add(this.verts[this.verts.Count - 1] + this.points[j].tangent * num4);
			this.uvs.Add(new Vector2((j % 2 == 0) ? 0f : 1f, 0f));
			this.uvs.Add(new Vector2((j % 2 == 0) ? 0f : 1f, 0.9f));
		}
		for (int k = 0; k < this.points.Count - 1; k++)
		{
			int num5 = k * 2;
			int num6 = (k == this.points.Count - 1) ? 0 : (num5 + 2);
			int num7 = (k == this.points.Count - 1) ? 1 : (num5 + 3);
			this.vIndices.Add(num5);
			this.vIndices.Add(num5 + 1);
			this.vIndices.Add(num6);
			this.vIndices.Add(num6);
			this.vIndices.Add(num5 + 1);
			this.vIndices.Add(num7);
			this.hIndices.Add(count + num5);
			this.hIndices.Add(count + num5 + 1);
			this.hIndices.Add(count + num6);
			this.hIndices.Add(count + num6);
			this.hIndices.Add(count + num5 + 1);
			this.hIndices.Add(count + num7);
		}
		this.trailMesh.subMeshCount = 2;
		this.trailMesh.SetVertices(this.verts);
		this.trailMesh.SetUVs(0, this.uvs);
		this.trailMesh.SetIndices(this.vIndices.ToArray(), MeshTopology.Triangles, 0);
		this.trailMesh.SetIndices(this.hIndices.ToArray(), MeshTopology.Triangles, 1);
		this.trail.GetComponent<MeshFilter>().sharedMesh = this.trailMesh;
	}

	// Token: 0x060010DD RID: 4317 RVA: 0x00084384 File Offset: 0x00082584
	protected override void DoMovement()
	{
		if (!this.isDead)
		{
			this.targetLeanAngle = 0f;
			float num = 0f;
			if (!this.player.IsAI)
			{
				Vector3 vector = Vector3.zero;
				if (!this.player.IsAI && this.player.RewiredPlayer != null && this.player.RewiredPlayer.controllers.GetLastActiveController() != null && this.player.RewiredPlayer.controllers.GetLastActiveController().type == ControllerType.Joystick)
				{
					Vector3 vector2 = new Vector3(this.player.RewiredPlayer.GetAxis(InputActions.LookHorizontal), 0f, this.player.RewiredPlayer.GetAxis(InputActions.LookVertical));
					Vector3 vector3 = new Vector3(this.player.RewiredPlayer.GetAxis(InputActions.Horizontal), 0f, this.player.RewiredPlayer.GetAxis(InputActions.Vertical));
					vector = ((vector2.magnitude > vector3.magnitude) ? vector2 : vector3);
				}
				else
				{
					vector = new Vector3(this.player.RewiredPlayer.GetAxis(InputActions.Horizontal), 0f, this.player.RewiredPlayer.GetAxis(InputActions.Vertical));
				}
				if (vector.sqrMagnitude > 0.3f && Vector3.Dot(vector, base.transform.forward) > -0.9f)
				{
					this.rotationSpeed = 1000f;
					base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, Quaternion.LookRotation(vector), this.rotationSpeed * Time.deltaTime);
				}
				this.targetLeanAngle = this.maxLeanAngle * -num;
			}
			else
			{
				num = this.GetAIInput();
			}
			this.mover.SetForwardVector(base.transform.forward);
			CharacterMoverInput input = new CharacterMoverInput(true, false, false, false, false);
			input.NullInput(!this.minigameController.Playable || this.isDead);
			this.mover.CalculateVelocity(input, Time.deltaTime);
			this.mover.DoMovement(Time.deltaTime);
		}
		this.velocity.Value = this.mover.Velocity;
	}

	// Token: 0x060010DE RID: 4318 RVA: 0x000845C4 File Offset: 0x000827C4
	private float GetAIInput()
	{
		float d = 2.5f;
		Vector2 vector = new Vector2(base.transform.position.x, base.transform.position.z);
		if (this.dodgeTimer.Elapsed(true))
		{
			short num = 0;
			while ((int)num < this.minigameController.GetPlayerCount())
			{
				SnakePlayer snakePlayer = (SnakePlayer)this.minigameController.GetPlayerInSlot(num);
				for (int i = 0; i < snakePlayer.points.Count; i++)
				{
					if (!(snakePlayer == this) || (i != snakePlayer.points.Count - 1 && i <= snakePlayer.points.Count - 26))
					{
						Vector2 pos = snakePlayer.points[i].pos;
						Vector2 p = Vector2.zero;
						if (i >= snakePlayer.points.Count - 1)
						{
							if (snakePlayer.isDead)
							{
								goto IL_12A;
							}
							Vector3 vector2 = snakePlayer.transform.position + snakePlayer.transform.forward * d;
							p = new Vector2(vector2.x, vector2.z);
						}
						else
						{
							p = snakePlayer.points[i + 1].pos;
						}
						this.AICheckLine(pos, p, vector);
					}
					IL_12A:;
				}
				num += 1;
			}
			if (this.minigameController.curTrain != null)
			{
				for (int j = 0; j < this.minigameController.trainPoints.Length; j++)
				{
					Vector2 p2 = this.minigameController.trainPoints[j];
					Vector2 p3 = (j == this.minigameController.trainPoints.Length - 1) ? this.minigameController.trainPoints[0] : this.minigameController.trainPoints[j + 1];
					this.AICheckLine(p2, p3, vector);
				}
			}
		}
		if (this.possibleHits.Count > 0)
		{
			Vector2 vector3 = Vector2.zero;
			new Vector2(base.transform.forward.x, base.transform.forward.z);
			for (int k = 0; k < this.possibleHits.Count; k++)
			{
				Vector2 normalized = (vector - this.possibleHits[k]).normalized;
				vector3 += (vector - this.possibleHits[k]).normalized;
			}
			vector3.Normalize();
			float num2 = Vector2.Distance(Vector2.zero, vector);
			Vector2 normalized2 = (Vector2.zero - vector).normalized;
			vector3 = Vector2.Lerp(vector3, normalized2, num2 / 30f);
			this.currentTurnDir = new Vector3(vector3.x, 0f, vector3.y);
			this.randomDirectionTimer.SetInterval(0.3f, true);
			this.possibleHits.Clear();
		}
		else if (this.randomDirectionTimer.Elapsed(true))
		{
			this.randomDirectionTimer.SetInterval(0.65f, 1.1f, true);
			Vector3 b = (GameManager.rand.NextDouble() > 0.5) ? base.transform.right : (-base.transform.right);
			this.currentTurnDir = Vector3.Lerp(base.transform.forward, b, (float)GameManager.rand.NextDouble());
		}
		Quaternion to = Quaternion.LookRotation(this.currentTurnDir);
		base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, to, this.rotationSpeed * Time.deltaTime);
		return 0f;
	}

	// Token: 0x060010DF RID: 4319 RVA: 0x00084980 File Offset: 0x00082B80
	private void AICheckLine(Vector2 p1, Vector2 p2, Vector2 curPos2D)
	{
		Vector2 closestPointOnLineSegment = ZPMath.GetClosestPointOnLineSegment(p1, p2, curPos2D);
		if ((curPos2D - closestPointOnLineSegment).sqrMagnitude < this.avoidDistanceSqr)
		{
			this.possibleHits.Add(closestPointOnLineSegment);
		}
	}

	// Token: 0x060010E0 RID: 4320 RVA: 0x000849BC File Offset: 0x00082BBC
	public void OnControllerColliderHit(ControllerColliderHit hit)
	{
		if (hit.gameObject.name == "Barrier")
		{
			Vector3 vector = Vector3.Cross(hit.normal, Vector3.up);
			Vector3 vector2 = -vector;
			float num = Vector3.Dot(base.transform.forward, vector2);
			float num2 = Vector3.Dot(base.transform.forward, vector);
			float num3 = 100000000f;
			if (num > num2)
			{
				base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, Quaternion.LookRotation(new Vector3(vector2.x, 0f, vector2.z)), num3 * Time.deltaTime);
				return;
			}
			base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, Quaternion.LookRotation(new Vector3(vector.x, 0f, vector.z)), num3 * Time.deltaTime);
		}
	}

	// Token: 0x060010E1 RID: 4321 RVA: 0x0000DFBF File Offset: 0x0000C1BF
	public override void ResetPlayer()
	{
		this.points.Clear();
		this.UpdateTrail();
		base.ResetPlayer();
	}

	// Token: 0x060010E2 RID: 4322 RVA: 0x0000DFD8 File Offset: 0x0000C1D8
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCKillPlayer(NetPlayer sender)
	{
		this.KillPlayer(false);
	}

	// Token: 0x060010E3 RID: 4323 RVA: 0x00084AA4 File Offset: 0x00082CA4
	public void KillPlayer(bool send_rpc)
	{
		if (!this.isDead)
		{
			Vector3 force = this.velocity.Value * 0.6f + Vector3.up * 3.25f;
			this.playerAnim.SpawnRagdoll(force);
			UnityEngine.Object.Instantiate<GameObject>(this.player_death_effect, base.transform.position, Quaternion.LookRotation(Vector3.up));
			AudioSystem.PlayOneShot(this.explosion_clip, 0.5f, 0.1f, 1f);
			this.isDead = true;
			this.Deactivate();
			this.mover.Velocity = Vector3.zero;
			this.velocity.Value = Vector3.zero;
			this.cameraShake.AddShake(0.25f);
			if (NetSystem.IsServer)
			{
				this.minigameController.PlayerDied(this);
			}
			if (base.IsOwner && send_rpc)
			{
				base.SendRPC("RPCKillPlayer", NetRPCDelivery.RELIABLE_UNORDERED, Array.Empty<object>());
			}
		}
	}

	// Token: 0x060010E4 RID: 4324 RVA: 0x0000DFE1 File Offset: 0x0000C1E1
	private void OnDestroy()
	{
		UnityEngine.Object.Destroy(this.trailMesh);
	}

	// Token: 0x0400115D RID: 4445
	public GameObject player_death_effect;

	// Token: 0x0400115E RID: 4446
	public GameObject trail_prefab;

	// Token: 0x0400115F RID: 4447
	public AudioClip explosion_clip;

	// Token: 0x04001160 RID: 4448
	public List<SnakePlayer.TrailPoint> points = new List<SnakePlayer.TrailPoint>();

	// Token: 0x04001161 RID: 4449
	public AnimationCurve board_height_bob;

	// Token: 0x04001162 RID: 4450
	public float height_bob_amount = 0.25f;

	// Token: 0x04001163 RID: 4451
	public float bob_speed = 2f;

	// Token: 0x04001164 RID: 4452
	public AnimationCurve trail_size;

	// Token: 0x04001165 RID: 4453
	public float trail_size_scale = 0.4f;

	// Token: 0x04001166 RID: 4454
	public Transform trailSource;

	// Token: 0x04001167 RID: 4455
	public GameObject hoverBoard;

	// Token: 0x04001168 RID: 4456
	public Renderer boardSparks;

	// Token: 0x04001169 RID: 4457
	private CharacterMover mover;

	// Token: 0x0400116A RID: 4458
	private float rotationSpeed = 375f;

	// Token: 0x0400116B RID: 4459
	private SnakeController minigameController;

	// Token: 0x0400116C RID: 4460
	private float trailHeight = 1f;

	// Token: 0x0400116D RID: 4461
	private GameObject trail;

	// Token: 0x0400116E RID: 4462
	private float playerRadius = 0.6f;

	// Token: 0x0400116F RID: 4463
	private float playerRadiusSqr;

	// Token: 0x04001170 RID: 4464
	private CameraShake cameraShake;

	// Token: 0x04001171 RID: 4465
	private Mesh trailMesh;

	// Token: 0x04001172 RID: 4466
	private Vector3 rootStartPos;

	// Token: 0x04001173 RID: 4467
	private bool isInitialized;

	// Token: 0x04001174 RID: 4468
	private float maxLeanAngle = 45f;

	// Token: 0x04001175 RID: 4469
	private float curLeanAngle;

	// Token: 0x04001176 RID: 4470
	private float targetLeanAngle;

	// Token: 0x04001177 RID: 4471
	private float leanSpeed = 180f;

	// Token: 0x04001178 RID: 4472
	private Vector3 lastVelocity = Vector3.zero;

	// Token: 0x04001179 RID: 4473
	private List<Vector3> verts = new List<Vector3>();

	// Token: 0x0400117A RID: 4474
	private List<int> vIndices = new List<int>();

	// Token: 0x0400117B RID: 4475
	private List<int> hIndices = new List<int>();

	// Token: 0x0400117C RID: 4476
	private List<Vector2> uvs = new List<Vector2>();

	// Token: 0x0400117D RID: 4477
	private float inputDelayTimer = 0.15f;

	// Token: 0x0400117E RID: 4478
	private float rightStrength;

	// Token: 0x0400117F RID: 4479
	private float leftStrength;

	// Token: 0x04001180 RID: 4480
	private ActionTimer dodgeTimer = new ActionTimer(0.03f, 0.035f);

	// Token: 0x04001181 RID: 4481
	private ActionTimer randomDirectionTimer = new ActionTimer(0.2f, 0.4f);

	// Token: 0x04001182 RID: 4482
	private Vector3 currentTurnDir = Vector3.zero;

	// Token: 0x04001183 RID: 4483
	private List<Vector2> possibleHits = new List<Vector2>();

	// Token: 0x04001184 RID: 4484
	private float avoidDistance = 1.5f;

	// Token: 0x04001185 RID: 4485
	private float avoidDistanceSqr;

	// Token: 0x04001186 RID: 4486
	private Light[] lights;

	// Token: 0x02000248 RID: 584
	public struct TrailPoint
	{
		// Token: 0x060010E6 RID: 4326 RVA: 0x0000DFEE File Offset: 0x0000C1EE
		public TrailPoint(Vector2 _pos, Vector3 _tangent, float _time, float _life)
		{
			this.pos = _pos;
			this.time = _time;
			this.tangent = _tangent;
			this.life = _life;
		}

		// Token: 0x04001187 RID: 4487
		public Vector2 pos;

		// Token: 0x04001188 RID: 4488
		public float time;

		// Token: 0x04001189 RID: 4489
		public Vector3 tangent;

		// Token: 0x0400118A RID: 4490
		public float life;
	}
}
