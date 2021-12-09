using System;
using I2.Loc;
using Rewired;
using TMPro;
using UnityEngine;
using ZP.Net;
using ZP.Utility;

// Token: 0x0200020F RID: 527
public class RacerPlayer : Movement1
{
	// Token: 0x1700015A RID: 346
	// (get) Token: 0x06000F80 RID: 3968 RVA: 0x0000D4E3 File Offset: 0x0000B6E3
	public int LapCount
	{
		get
		{
			return this.lap_count;
		}
	}

	// Token: 0x1700015B RID: 347
	// (get) Token: 0x06000F81 RID: 3969 RVA: 0x0000D4EB File Offset: 0x0000B6EB
	public RaycastHit LastHit
	{
		get
		{
			return this.last_hit;
		}
	}

	// Token: 0x1700015C RID: 348
	// (get) Token: 0x06000F82 RID: 3970 RVA: 0x0000D4F3 File Offset: 0x0000B6F3
	public bool IsFinished
	{
		get
		{
			return this.finished;
		}
	}

	// Token: 0x06000F83 RID: 3971 RVA: 0x000047D3 File Offset: 0x000029D3
	public void Awake()
	{
		base.InitializeController();
	}

	// Token: 0x06000F84 RID: 3972 RVA: 0x0007ACD0 File Offset: 0x00078ED0
	protected override void Start()
	{
		base.Start();
		this.minigame_controller = (RacerController)GameManager.Minigame;
		this.minigame_controller.AddPlayer(this);
		this.minigame_controller.Root.GetComponentInChildren<CameraFollow>().AddTarget(this);
		this.drift_rotation = base.transform.rotation.eulerAngles.y;
		this.carRotation = base.transform.rotation.eulerAngles.y;
		this.respawnPoints = GameObject.Find("RaceTrack").GetComponent<RacerRespawnPointGenerator>();
		this.text_spawn_point = GameObject.Find("TextSpawnPoint").transform;
		AudioSource[] components = base.GetComponents<AudioSource>();
		this.engine_sound = components[0];
		this.drift_sound = components[1];
		if (base.IsOwner && this.player.IsAI)
		{
			this.FindPoint();
		}
		this.player_root != null;
		this.playerAnim.Driving = true;
	}

	// Token: 0x06000F85 RID: 3973 RVA: 0x0000D4FB File Offset: 0x0000B6FB
	public override void OnOwnerChanged()
	{
		if (NetSystem.IsServer)
		{
			this.FindPoint();
		}
		base.OnOwnerChanged();
	}

	// Token: 0x06000F86 RID: 3974 RVA: 0x0007ADC8 File Offset: 0x00078FC8
	private void Update()
	{
		this.engine_sound.volume = AudioSystem.GetVolume(SoundType.Effect, 0.35f);
		if (this.engine_sound != null)
		{
			this.engine_sound.enabled = (this.spawned && this.minigame_controller.State == MinigameControllerState.Playing);
		}
		this.last_position = base.transform.position;
		base.UpdateController();
		if (this.dead)
		{
			if (!this.spawned && Time.time - this.death_start_time > this.respawn_length)
			{
				this.SetRenderers(true);
				this.spawned = true;
				this.last_flicker_time = Time.time;
				if (base.IsOwner)
				{
					RespawnPoint respawnPoint = this.respawnPoints.GetRespawnPoint(this.last_hit.triangleIndex);
					base.transform.position = respawnPoint.spawn_point + this.spawn_offset;
					base.transform.rotation = Quaternion.Euler(0f, respawnPoint.spawn_y_rotation, 0f);
					base.transform.position -= base.transform.forward * 0.1f;
					this.carRotation = base.transform.rotation.eulerAngles.y;
					this.drift_rotation = base.transform.rotation.eulerAngles.y;
					this.curSpeed = this.min_speed;
					this.reachedSqrDist = 25f;
				}
			}
			else if (this.spawned && Time.time - this.death_start_time > this.death_length)
			{
				this.SetRenderers(true);
				this.dead = false;
				if (this.finished)
				{
					this.FindPoint();
				}
			}
			else if (this.spawned && Time.time - this.last_flicker_time > this.flicker_interval)
			{
				this.SetRenderers(!this.renderers_active);
				this.last_flicker_time = Time.time;
			}
		}
		this.GetHit();
		bool flag;
		if (base.IsOwner)
		{
			float num = Mathf.Abs(this.drift_rotation - this.carRotation);
			flag = (num > this.drifting_angle_dif && !this.dead && this.velocity.Value.magnitude > this.min_drift_velocity);
			this.engine_sound.pitch = this.curSpeed / 20f + 1f - num / 90f;
		}
		else
		{
			flag = (base.transform.position - this.last_position != Vector3.zero && this.velocity.Value.magnitude > this.min_drift_velocity && (Mathf.Abs(Quaternion.LookRotation(base.transform.position - this.last_position).eulerAngles.y - base.transform.rotation.eulerAngles.y) > this.drifting_angle_dif && !this.dead) && this.minigame_controller.State == MinigameControllerState.Playing);
		}
		bool flag2 = false;
		bool flag3 = false;
		if (flag)
		{
			RaycastHit raycastHit;
			if (Physics.Raycast(this.back_left_wheel.position, -Vector3.up, out raycastHit, 1f, 1024) && raycastHit.normal.y > 0.9f)
			{
				flag2 = true;
			}
			if (Physics.Raycast(this.back_right_wheel.position, -Vector3.up, out raycastHit, 1f, 1024) && raycastHit.normal.y > 0.9f)
			{
				flag3 = true;
			}
			ParticleSystem.EmissionModule emission = this.drift_smoke_left.emission;
			ParticleSystem.EmissionModule emission2 = this.drift_smoke_right.emission;
			ParticleSystem.EmissionModule emission3 = this.drift_left.emission;
			ParticleSystem.EmissionModule emission4 = this.drift_right.emission;
			emission3.enabled = (emission2.enabled = flag2);
			emission4.enabled = (emission.enabled = flag3);
		}
		else
		{
			this.drift_smoke_left.emission.enabled = (this.drift_smoke_right.emission.enabled = (this.drift_left.emission.enabled = (this.drift_right.emission.enabled = flag)));
		}
		if (flag && (flag2 || flag3) && !this.dead)
		{
			this.curDriftVol = Mathf.Clamp(this.curDriftVol + this.drift_volume_speed_increase * Time.deltaTime, 0f, 0.6f);
			this.drift_sound.volume = AudioSystem.GetVolume(SoundType.Effect, this.curDriftVol);
			return;
		}
		this.curDriftVol = Mathf.Clamp(this.curDriftVol - this.drift_volume_speed_decrease * Time.deltaTime, 0f, 0.6f);
		this.drift_sound.volume = AudioSystem.GetVolume(SoundType.Effect, this.curDriftVol);
	}

	// Token: 0x06000F87 RID: 3975 RVA: 0x0007B2CC File Offset: 0x000794CC
	private void GetHit()
	{
		RaycastHit raycastHit;
		if (Physics.Raycast(base.transform.position, -Vector3.up, out raycastHit, 2f, 1024) && raycastHit.normal.y > 0.9f)
		{
			this.last_hit = raycastHit;
		}
	}

	// Token: 0x06000F88 RID: 3976 RVA: 0x0007B31C File Offset: 0x0007951C
	protected override void DoMovement()
	{
		if (!this.dead)
		{
			if (!this.finished && this.minigame_controller.State == MinigameControllerState.Playing && !this.player.IsAI)
			{
				Vector2 zero = new Vector2(this.player.RewiredPlayer.GetAxis(InputActions.Horizontal), this.player.RewiredPlayer.GetAxis(InputActions.Vertical));
				if (GameManager.IsGamePaused)
				{
					zero = Vector2.zero;
				}
				Controller lastActiveController = this.player.RewiredPlayer.controllers.GetLastActiveController();
				bool flag = this.player.RewiredPlayer.GetButton(InputActions.Accept);
				bool flag2;
				if (lastActiveController != null && lastActiveController.type == ControllerType.Joystick)
				{
					flag = this.player.RewiredPlayer.GetButton(InputActions.UseItemShoot);
					flag2 = this.player.RewiredPlayer.GetButton(InputActions.Cancel);
				}
				else
				{
					flag = (zero.y > 0.5f);
					flag2 = (zero.y < -0.5f);
					if (zero.x < -0.5f)
					{
						this.rightStrength = 0f;
						this.leftStrength = Mathf.Clamp01(this.leftStrength + this.inputDelayTimer / Time.deltaTime);
						zero.x = -this.leftStrength;
					}
					else if (zero.x > 0.5f)
					{
						this.leftStrength = 0f;
						this.rightStrength = Mathf.Clamp01(this.rightStrength + this.inputDelayTimer / Time.deltaTime);
						zero.x = this.rightStrength;
					}
					else
					{
						this.leftStrength = 0f;
						this.leftStrength = 0f;
						zero.x = 0f;
					}
				}
				this.carRotation += this.turn_speed * Time.deltaTime * zero.x;
				this.curSpeed -= this.acceleration * Time.deltaTime * 1.2f * Mathf.Abs(zero.x);
				if (flag)
				{
					this.curSpeed += this.acceleration * Time.deltaTime;
				}
				if (flag2)
				{
					this.curSpeed -= this.break_deceleration * Time.deltaTime;
				}
				if (!flag2 && !flag)
				{
					this.curSpeed -= this.deceleration * Time.deltaTime;
				}
				this.curSpeed = Mathf.Clamp(this.curSpeed, this.min_speed, this.max_speed);
			}
			else if (this.minigame_controller.State == MinigameControllerState.Playing || this.minigame_controller.State == MinigameControllerState.RoundResetWait)
			{
				float d = 1f;
				Vector3 a = Vector3.Cross(Quaternion.Euler(0f, this.respawnPoints.respawn_points[this.curTargetID].spawn_y_rotation, 0f) * Vector3.forward, Vector3.up);
				Vector3 vector = ZPMath.ClosestPointOnLine(this.respawnPoints.respawn_points[this.curTargetID].spawn_point - a * d, this.respawnPoints.respawn_points[this.curTargetID].spawn_point + a * d, base.transform.position);
				if ((base.transform.position - vector).sqrMagnitude < this.reachedSqrDist)
				{
					if (!this.respawnPoints.flipped)
					{
						this.curTargetID++;
					}
					else
					{
						this.curTargetID--;
					}
					if (this.curTargetID >= this.respawnPoints.respawn_points.Count)
					{
						this.curTargetID = 0;
					}
					else if (this.curTargetID < 0)
					{
						this.curTargetID = this.respawnPoints.respawn_points.Count - 1;
					}
					float min = this.difficultyReachedDistMin[(int)base.GamePlayer.Difficulty];
					float max = this.difficultyReachedDistMax[(int)base.GamePlayer.Difficulty];
					this.reachedSqrDist = ZPMath.RandomFloat(GameManager.rand, min, max);
				}
				float y = Quaternion.LookRotation(vector - base.transform.position, Vector3.forward).eulerAngles.y;
				float num = Mathf.Abs(y - this.carRotation);
				float num2 = this.turn_speed * Time.deltaTime * (this.curSpeed / this.max_speed);
				if (num2 > num)
				{
					num2 = num;
				}
				if (ZPMath.CalcShortestRotDirection(this.carRotation, y))
				{
					this.carRotation += num2;
				}
				else
				{
					this.carRotation -= num2;
				}
				this.curSpeed = Mathf.Clamp(this.curSpeed + this.acceleration * Time.deltaTime, this.min_speed, this.max_speed);
			}
			this.carRotation = ZPMath.ClampRotation(this.carRotation);
			float f = ZPMath.CalcShortestRot(this.drift_rotation, this.carRotation);
			bool flag3 = ZPMath.CalcShortestRotDirection(this.drift_rotation, this.carRotation);
			float num3 = this.drift_lerp * this.turn_speed * Time.deltaTime;
			if (num3 > Mathf.Abs(f))
			{
				this.drift_rotation = this.carRotation;
			}
			else
			{
				this.drift_rotation += (flag3 ? num3 : (-num3));
			}
			this.drift_rotation = ZPMath.ClampRotation(this.drift_rotation);
			if (Mathf.Abs(ZPMath.CalcShortestRot(this.drift_rotation, this.carRotation)) > this.max_drift_offset)
			{
				this.drift_rotation = this.carRotation + (flag3 ? (-this.max_drift_offset) : this.max_drift_offset);
			}
			this.drift_rotation = ZPMath.ClampRotation(this.drift_rotation);
			Vector3 a2 = Quaternion.Euler(0f, this.drift_rotation, 0f) * Vector3.forward;
			this.velocity_y -= this.gravity * Time.deltaTime;
			Vector3 vector2 = a2 * this.curSpeed + new Vector3(0f, this.velocity_y, 0f);
			this.controller.Move(vector2 * Time.deltaTime);
			if (this.controller.isGrounded)
			{
				this.velocity_y = 0f;
			}
			this.velocity.Value = vector2;
			base.transform.rotation = Quaternion.Euler(0f, this.carRotation, 0f);
			return;
		}
		this.velocity.Value = Vector3.zero;
	}

	// Token: 0x06000F89 RID: 3977 RVA: 0x0000398C File Offset: 0x00001B8C
	protected override void UpdateAnimationState(PlayerAnimation player_anim)
	{
	}

	// Token: 0x06000F8A RID: 3978 RVA: 0x0007B9A0 File Offset: 0x00079BA0
	private void OnTriggerEnter(Collider c)
	{
		if (this.minigame_controller != null && this.minigame_controller.State == MinigameControllerState.Playing)
		{
			if (c.gameObject.name == "Death" && !this.dead)
			{
				this.achievementHasDied = true;
				GameObject gameObject = this.minigame_controller.Spawn(this.death_prefab, base.transform.position, base.transform.rotation);
				gameObject.GetComponent<RacerDestroyedCar>().player_slot = (int)base.OwnerSlot;
				Rigidbody component = gameObject.GetComponent<Rigidbody>();
				component.velocity = this.velocity.Value;
				component.angularVelocity = base.transform.rotation * new Vector3(2f, 0f, 0f);
				this.SetRenderers(false);
				this.spawned = false;
				this.dead = true;
				this.death_start_time = Time.time;
				return;
			}
			if (c.gameObject.name == "FinishLine" && this.hit_lap && !this.finished)
			{
				Physics.IgnoreCollision(this.racerController, this.minigame_controller.backBlocker, false);
				this.hit_lap = false;
				this.lap_count++;
				TextMeshPro component2 = this.minigame_controller.Spawn(this.lap_text, this.text_spawn_point.position, this.text_spawn_point.rotation).GetComponent<TextMeshPro>();
				component2.text = LocalizationManager.GetTranslation("Lap", true, 0, true, false, null, null, true).Replace("%Count%", this.lap_count.ToString());
				if (this.lap_count == this.laps_to_finish - 1)
				{
					component2.text = LocalizationManager.GetTranslation("Final Lap", true, 0, true, false, null, null, true);
				}
				else if (this.lap_count == this.laps_to_finish)
				{
					component2.text = LocalizationManager.GetTranslation("Finish", true, 0, true, false, null, null, true);
					if (base.IsOwner && !this.player.IsAI && !this.achievementHasDied)
					{
						PlatformAchievementManager.Instance.TriggerAchievement("ACH_RUSTY_RACERS");
					}
				}
				AudioSystem.PlayOneShot(this.lap_sound, 0.3f, 0f, 1f);
				if (this.lap_count >= this.laps_to_finish)
				{
					this.Finished();
					if (NetSystem.IsServer)
					{
						int num = 0;
						for (int i = 0; i < this.minigame_controller.GetPlayerCount(); i++)
						{
							if (((RacerPlayer)this.minigame_controller.GetPlayer(i)).IsFinished)
							{
								num++;
							}
						}
						this.Score = (short)(this.minigame_controller.GetPlayerCount() - num);
						if (num == 1)
						{
							this.minigame_controller.ShowWinnerText(this.player);
							return;
						}
					}
				}
			}
			else if (c.gameObject.name == "Lap" && !this.hit_lap)
			{
				this.hit_lap = true;
				Physics.IgnoreCollision(this.racerController, this.minigame_controller.backBlocker, true);
			}
		}
	}

	// Token: 0x06000F8B RID: 3979 RVA: 0x0000D510 File Offset: 0x0000B710
	private void Finished()
	{
		this.finished = true;
		this.FindPoint();
	}

	// Token: 0x06000F8C RID: 3980 RVA: 0x0007BC98 File Offset: 0x00079E98
	private void FindPoint()
	{
		float num = float.MaxValue;
		for (int i = 0; i < this.respawnPoints.respawn_points.Count; i++)
		{
			float sqrMagnitude = (this.respawnPoints.respawn_points[i].spawn_point - base.transform.position).sqrMagnitude;
			if (sqrMagnitude < num && Vector3.Dot(base.transform.forward, (this.respawnPoints.respawn_points[i].spawn_point - base.transform.position).normalized) < 0.8333333f)
			{
				this.curTargetID = i;
				num = sqrMagnitude;
			}
		}
	}

	// Token: 0x06000F8D RID: 3981 RVA: 0x0007BD50 File Offset: 0x00079F50
	public void SetRenderers(bool state)
	{
		this.renderers_active = state;
		MeshRenderer[] componentsInChildren = this.player_root.GetComponentsInChildren<MeshRenderer>();
		SkinnedMeshRenderer[] componentsInChildren2 = this.player_root.GetComponentsInChildren<SkinnedMeshRenderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].enabled = state;
		}
		for (int j = 0; j < componentsInChildren2.Length; j++)
		{
			componentsInChildren2[j].enabled = state;
		}
		this.outlineSource.enabled = state;
	}

	// Token: 0x06000F8E RID: 3982 RVA: 0x0000D51F File Offset: 0x0000B71F
	public override void ResetPlayer()
	{
		base.ResetPlayer();
	}

	// Token: 0x04000F63 RID: 3939
	public GameObject death_prefab;

	// Token: 0x04000F64 RID: 3940
	public AudioClip lap_sound;

	// Token: 0x04000F65 RID: 3941
	public GameObject lap_text;

	// Token: 0x04000F66 RID: 3942
	public ParticleSystem drift_left;

	// Token: 0x04000F67 RID: 3943
	public ParticleSystem drift_right;

	// Token: 0x04000F68 RID: 3944
	public ParticleSystem drift_smoke_left;

	// Token: 0x04000F69 RID: 3945
	public ParticleSystem drift_smoke_right;

	// Token: 0x04000F6A RID: 3946
	public Transform back_right_wheel;

	// Token: 0x04000F6B RID: 3947
	public Transform back_left_wheel;

	// Token: 0x04000F6C RID: 3948
	public float max_speed;

	// Token: 0x04000F6D RID: 3949
	public float gravity;

	// Token: 0x04000F6E RID: 3950
	public int laps_to_finish = 3;

	// Token: 0x04000F6F RID: 3951
	public OutlineSource outlineSource;

	// Token: 0x04000F70 RID: 3952
	public CharacterController racerController;

	// Token: 0x04000F71 RID: 3953
	private RacerController minigame_controller;

	// Token: 0x04000F72 RID: 3954
	private float turn_speed = 220f;

	// Token: 0x04000F73 RID: 3955
	private float min_speed = 2f;

	// Token: 0x04000F74 RID: 3956
	private float acceleration = 10f;

	// Token: 0x04000F75 RID: 3957
	private float break_deceleration = 15f;

	// Token: 0x04000F76 RID: 3958
	private float deceleration = 3f;

	// Token: 0x04000F77 RID: 3959
	private float drift_lerp = 0.7f;

	// Token: 0x04000F78 RID: 3960
	private float max_drift_offset = 90f;

	// Token: 0x04000F79 RID: 3961
	private float drifting_angle_dif = 12f;

	// Token: 0x04000F7A RID: 3962
	private float min_drift_velocity = 5f;

	// Token: 0x04000F7B RID: 3963
	private float drift_rotation;

	// Token: 0x04000F7C RID: 3964
	private float carRotation;

	// Token: 0x04000F7D RID: 3965
	private float curSpeed;

	// Token: 0x04000F7E RID: 3966
	private Vector3 last_position;

	// Token: 0x04000F7F RID: 3967
	private RaycastHit last_hit;

	// Token: 0x04000F80 RID: 3968
	private RacerRespawnPointGenerator respawnPoints;

	// Token: 0x04000F81 RID: 3969
	private int lap_count;

	// Token: 0x04000F82 RID: 3970
	private bool hit_lap;

	// Token: 0x04000F83 RID: 3971
	private bool finished;

	// Token: 0x04000F84 RID: 3972
	private bool spawned = true;

	// Token: 0x04000F85 RID: 3973
	private bool dead;

	// Token: 0x04000F86 RID: 3974
	private float respawn_length = 1f;

	// Token: 0x04000F87 RID: 3975
	private float death_length = 2.25f;

	// Token: 0x04000F88 RID: 3976
	private float death_start_time;

	// Token: 0x04000F89 RID: 3977
	private float flicker_interval = 0.07f;

	// Token: 0x04000F8A RID: 3978
	private float last_flicker_time;

	// Token: 0x04000F8B RID: 3979
	private Vector3 spawn_offset = new Vector3(0f, 0.275f, 0f);

	// Token: 0x04000F8C RID: 3980
	private Transform text_spawn_point;

	// Token: 0x04000F8D RID: 3981
	private float velocity_y;

	// Token: 0x04000F8E RID: 3982
	private bool renderers_active = true;

	// Token: 0x04000F8F RID: 3983
	private float drift_volume_speed_increase = 1.6f;

	// Token: 0x04000F90 RID: 3984
	private float drift_volume_speed_decrease = 3f;

	// Token: 0x04000F91 RID: 3985
	private int curTargetID;

	// Token: 0x04000F92 RID: 3986
	private AudioSource engine_sound;

	// Token: 0x04000F93 RID: 3987
	private AudioSource drift_sound;

	// Token: 0x04000F94 RID: 3988
	private float curDriftVol;

	// Token: 0x04000F95 RID: 3989
	private bool achievementHasDied;

	// Token: 0x04000F96 RID: 3990
	private float inputDelayTimer = 0.65f;

	// Token: 0x04000F97 RID: 3991
	private float rightStrength;

	// Token: 0x04000F98 RID: 3992
	private float leftStrength;

	// Token: 0x04000F99 RID: 3993
	private float reachedSqrDist = 25f;

	// Token: 0x04000F9A RID: 3994
	private float[] difficultyReachedDistMin = new float[]
	{
		20.25f,
		12.25f,
		4f
	};

	// Token: 0x04000F9B RID: 3995
	private float[] difficultyReachedDistMax = new float[]
	{
		121f,
		121f,
		121f
	};
}
