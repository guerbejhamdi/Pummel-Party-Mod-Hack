using System;
using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;
using ZP.Net;

// Token: 0x020001F8 RID: 504
public class PlaneBase : CharacterBase
{
	// Token: 0x1700014E RID: 334
	// (get) Token: 0x06000EB6 RID: 3766 RVA: 0x0000CDD4 File Offset: 0x0000AFD4
	// (set) Token: 0x06000EB7 RID: 3767 RVA: 0x0000CDE1 File Offset: 0x0000AFE1
	public byte Health
	{
		get
		{
			return this.health.Value;
		}
		set
		{
			if (NetSystem.IsServer)
			{
				this.health.Value = value;
				return;
			}
			Debug.LogError("Trying to set health for plane but not server");
		}
	}

	// Token: 0x06000EB8 RID: 3768 RVA: 0x0007586C File Offset: 0x00073A6C
	public void Awake()
	{
		this.m_sensorRightRot = Quaternion.Euler(0f, 45f, 0f);
		this.m_sensorLeftRot = Quaternion.Euler(0f, -45f, 0f);
		this.m_sensorMask = LayerMask.GetMask(new string[]
		{
			"WorldWall",
			"WorldGround"
		});
	}

	// Token: 0x06000EB9 RID: 3769 RVA: 0x0000CE01 File Offset: 0x0000B001
	public void OnDestroy()
	{
		UnityEngine.Object.Destroy(this.m_cameraParent);
	}

	// Token: 0x06000EBA RID: 3770 RVA: 0x000758D0 File Offset: 0x00073AD0
	public override void OnNetInitialize()
	{
		base.OnNetInitialize();
		this.minigameController = GameManager.Minigame;
		this.minigameController.AddPlayer(this);
		if (this.player.IsAI)
		{
			this.m_nextSensorUpdate += 0.025f * (float)this.player.GlobalID;
		}
		bool flag = true;
		using (List<GamePlayer>.Enumerator enumerator = GameManager.PlayerList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (!enumerator.Current.IsAI)
				{
					flag = false;
					break;
				}
			}
		}
		if (base.IsOwner)
		{
			this.position.Value = base.transform.position;
			this.rotation.Value = base.transform.rotation.eulerAngles;
			if (!this.player.IsAI || flag)
			{
				this.m_hasCamera = true;
				this.m_cameraParent.SetActive(true);
				this.m_cameraParent.transform.parent = null;
				this.cameraShake = this.m_cameraParent.GetComponentInChildren<CameraShake>();
				this.minigameController.minigameCameras.Add(this.m_cam);
				List<GamePlayer> list = GameManager.GetLocalNonAIPlayers();
				if (flag)
				{
					list = GameManager.GetLocalAIPlayers();
				}
				if (list.Count > 1)
				{
					if (!flag)
					{
						this.m_cam.rect = base.GetPlayerSplitScreenRect(this.player);
					}
					else
					{
						this.m_cam.rect = base.GetPlayerSplitScreenRectWithAI(this.player);
					}
				}
				if (list.Count > 0 && list[0] == this.player)
				{
					this.m_listener.enabled = true;
					return;
				}
			}
		}
		else
		{
			this.position.Recieve = new RecieveProxy(this.RecievePosition);
			this.interpolator = new Interpolator(base.transform, Interpolator.InterpolationType.PositionTransform);
		}
	}

	// Token: 0x06000EBB RID: 3771 RVA: 0x0000CE0E File Offset: 0x0000B00E
	public void RecievePosition(object _pos)
	{
		this.interpolator.NewPosition(_pos);
	}

	// Token: 0x06000EBC RID: 3772 RVA: 0x0000CE1C File Offset: 0x0000B01C
	protected override void Start()
	{
		base.Start();
		this.playerAnim.Driving = true;
		base.StartCoroutine(this.DelayMaterialCreation());
	}

	// Token: 0x06000EBD RID: 3773 RVA: 0x0000CE3D File Offset: 0x0000B03D
	private IEnumerator DelayMaterialCreation()
	{
		int j;
		for (int i = 0; i < (int)(base.GamePlayer.GlobalID * 2); i = j)
		{
			yield return null;
			j = i + 1;
		}
		Color col = this.player.Color.skinColor1;
		Material material = new Material(this.m_baseMaterial);
		material.SetColor("_Color", col);
		MeshRenderer[] renderers = this.m_renderers;
		for (j = 0; j < renderers.Length; j++)
		{
			renderers[j].sharedMaterial = material;
		}
		yield return null;
		Material material2 = new Material(this.m_smokeMat);
		col.r = Mathf.Max(0.25f, col.r);
		col.g = Mathf.Max(0.25f, col.g);
		col.b = Mathf.Max(0.25f, col.b);
		col *= 0.65f;
		material2.SetColor("_TintColor", col);
		this.m_smokeTrail.GetComponent<ParticleSystemRenderer>().sharedMaterial = material2;
		yield break;
	}

	// Token: 0x06000EBE RID: 3774 RVA: 0x0000B7B1 File Offset: 0x000099B1
	public override void FinishedSpawning()
	{
		base.FinishedSpawning();
	}

	// Token: 0x06000EBF RID: 3775 RVA: 0x0000CE4C File Offset: 0x0000B04C
	public void SetAITarget(Vector3 pos)
	{
		this.m_aiTargetPos = pos;
	}

	// Token: 0x06000EC0 RID: 3776 RVA: 0x0000CE55 File Offset: 0x0000B055
	private float DeadZone(float input, float zone)
	{
		if (Mathf.Abs(input) < zone)
		{
			input = 0f;
		}
		else if (input > 0f)
		{
			input = (input - zone) / (1f - zone);
		}
		else
		{
			input = (input + zone) / (1f - zone);
		}
		return input;
	}

	// Token: 0x06000EC1 RID: 3777 RVA: 0x00075AA8 File Offset: 0x00073CA8
	public void UpdatePlaneController()
	{
		this.m_planeEngine.volume = AudioSystem.GetVolume(SoundType.Effect, this.m_engineVolume);
		if (GameManager.Minigame != null && GameManager.Minigame.State >= MinigameControllerState.Countdown)
		{
			if (!this.m_planeEngine.gameObject.activeSelf)
			{
				this.m_planeEngine.gameObject.SetActive(true);
			}
			if (!this.isDead && !this.m_smokeTrail.isPlaying)
			{
				this.m_smokeTrail.Play();
			}
		}
		if (base.IsOwner)
		{
			if (this.minigameController.Playable && !this.isDead)
			{
				bool flag = true;
				float maxRollVelocity = this.m_maxRollVelocity;
				float maxPitchVelocity = this.m_maxPitchVelocity;
				float maxYawVelocity = this.m_maxYawVelocity;
				float num = 0f;
				Vector2 zero = Vector2.zero;
				float num2;
				float num3;
				if (!base.GamePlayer.IsAI)
				{
					num = this.player.RewiredPlayer.GetAxis(InputActions.PlaneYaw);
					num2 = this.player.RewiredPlayer.GetAxis(InputActions.PlanePitch);
					num3 = this.player.RewiredPlayer.GetAxis(InputActions.PlaneRoll);
					if (this.player.RewiredPlayer.controllers.GetLastActiveController().type == ControllerType.Joystick)
					{
						num = this.DeadZone(num, 0.1f);
						num2 = this.DeadZone(num2, 0.1f);
						num3 = this.DeadZone(num3, 0.3f);
					}
				}
				else
				{
					this.UpdateAI(ref zero, ref flag);
					num3 = zero.x;
					num2 = zero.y;
				}
				if (this.player.IsAI && flag)
				{
					if (this.player.IsAI || this.player.RewiredPlayer.controllers.GetLastActiveController().type != ControllerType.Joystick)
					{
						if (Mathf.Abs(zero.x) > 0.1f)
						{
							this.m_rollVelocity += zero.x * (Time.deltaTime / this.m_rollAccelerationTime) * maxRollVelocity;
						}
						else
						{
							this.m_rollVelocity = Mathf.SmoothDamp(this.m_rollVelocity, 0f, ref this.m_rollDissipateVelocity, this.m_rollDissipateTime);
						}
						this.m_rollVelocity = Mathf.Clamp(this.m_rollVelocity, -maxRollVelocity, maxRollVelocity);
						if (Mathf.Abs(zero.y) > 0.1f)
						{
							this.m_pitchVelocity += zero.y * (Time.deltaTime / this.m_pitchAccelerationTime) * maxPitchVelocity;
						}
						else
						{
							this.m_pitchVelocity = Mathf.SmoothDamp(this.m_pitchVelocity, 0f, ref this.m_pitchDissipateVelocity, this.m_pitchDissipateTime);
						}
						this.m_pitchVelocity = Mathf.Clamp(this.m_pitchVelocity, -maxPitchVelocity, maxPitchVelocity);
					}
					else
					{
						this.m_pitchVelocity = this.m_maxPitchVelocity * zero.y;
						this.m_rollVelocity = this.m_maxRollVelocity * zero.x;
					}
					base.transform.Rotate(base.transform.right, this.m_pitchVelocity * Time.deltaTime, Space.World);
					base.transform.Rotate(Vector3.forward, -this.m_rollVelocity * Time.deltaTime, Space.Self);
				}
				else if (!this.player.IsAI)
				{
					float num4 = (this.player.RewiredPlayer.controllers.GetLastActiveController().type == ControllerType.Joystick) ? 0.75f : 1f;
					if (Mathf.Abs(num3) > 0.1f)
					{
						this.m_rollVelocity += num3 * (Time.deltaTime / (this.m_rollAccelerationTime * num4)) * maxRollVelocity;
					}
					else
					{
						this.m_rollVelocity = Mathf.SmoothDamp(this.m_rollVelocity, 0f, ref this.m_rollDissipateVelocity, this.m_rollDissipateTime);
					}
					this.m_rollVelocity = Mathf.Clamp(this.m_rollVelocity, -maxRollVelocity, maxRollVelocity);
					if (Mathf.Abs(num2) > 0.1f)
					{
						this.m_pitchVelocity += num2 * (Time.deltaTime / (this.m_pitchAccelerationTime * num4)) * maxPitchVelocity;
					}
					else
					{
						this.m_pitchVelocity = Mathf.SmoothDamp(this.m_pitchVelocity, 0f, ref this.m_pitchDissipateVelocity, this.m_pitchDissipateTime);
					}
					this.m_pitchVelocity = Mathf.Clamp(this.m_pitchVelocity, -maxPitchVelocity, maxPitchVelocity);
					if (Mathf.Abs(num) > 0.1f)
					{
						this.m_yawVelocity += num * (Time.deltaTime / (this.m_pitchAccelerationTime * num4)) * this.m_maxYawVelocity;
					}
					else
					{
						this.m_yawVelocity = Mathf.SmoothDamp(this.m_yawVelocity, 0f, ref this.m_yawDissipateVelocity, this.m_yawDissipateTime);
					}
					this.m_yawVelocity = Mathf.Clamp(this.m_yawVelocity, -this.m_maxYawVelocity, this.m_maxYawVelocity);
					base.transform.Rotate(Vector3.up, this.m_yawVelocity * Time.deltaTime, Space.World);
					base.transform.Rotate(base.transform.right, -this.m_pitchVelocity * Time.deltaTime, Space.World);
					base.transform.Rotate(Vector3.forward, -this.m_rollVelocity * Time.deltaTime, Space.Self);
				}
				this.m_aileronAngle = Mathf.SmoothDamp(this.m_aileronAngle, num3 * 60f, ref this.m_aileronVelocity, 0.1f);
				this.m_leftAileron.localEulerAngles = new Vector3(-this.m_aileronAngle, this.m_leftAileron.localEulerAngles.y, this.m_leftAileron.localEulerAngles.z);
				this.m_rightAileron.localEulerAngles = new Vector3(this.m_aileronAngle, this.m_rightAileron.localEulerAngles.y, this.m_rightAileron.localEulerAngles.z);
				this.m_elevatorAngle = Mathf.SmoothDamp(this.m_elevatorAngle, num2 * 60f, ref this.m_elevatorVelocity, 0.1f);
				this.m_leftElevator.localEulerAngles = new Vector3(this.m_elevatorAngle, this.m_leftElevator.localEulerAngles.y, this.m_leftElevator.localEulerAngles.z);
				this.m_rightElevator.localEulerAngles = new Vector3(this.m_elevatorAngle, this.m_rightElevator.localEulerAngles.y, this.m_rightElevator.localEulerAngles.z);
				this.m_rudderAngle = Mathf.SmoothDamp(this.m_rudderAngle, num * -35f, ref this.m_rudderVelocity, 0.1f);
				this.m_rudder.localEulerAngles = new Vector3(this.m_rudder.localEulerAngles.x, this.m_rudderAngle, this.m_rudder.localEulerAngles.z);
			}
			float d = 1.5f;
			Vector3 up = Vector3.up;
			Vector3 vector = base.transform.position - base.transform.forward * this.m_camDistance + up * d;
			Quaternion b = Quaternion.LookRotation((base.transform.position + up * d - vector).normalized, Vector3.up);
			this.m_cameraParent.transform.position = Vector3.SmoothDamp(this.m_cameraParent.transform.position, vector, ref this.m_camPosVelocity, this.m_camDampTime);
			float num5 = Quaternion.Angle(this.m_cameraParent.transform.rotation, b);
			if (num5 > 0f)
			{
				float num6 = Mathf.SmoothDampAngle(num5, 0f, ref this.AngularVelocity, this.m_rotateSmoothTime);
				num6 = 1f - num6 / num5;
				this.m_cameraParent.transform.rotation = Quaternion.Slerp(this.m_cameraParent.transform.rotation, b, num6);
			}
			if (GameManager.Minigame != null && GameManager.Minigame.State >= MinigameControllerState.Countdown && !this.isDead)
			{
				base.transform.position += base.transform.forward * Time.deltaTime * this.m_planeVelocity;
			}
			if (this.isDead && Time.time > this.m_respawnTime)
			{
				this.RespawnPlayer();
			}
			this.rotation.Value = base.transform.rotation.eulerAngles;
			this.position.Value = base.transform.position;
			return;
		}
		this.interpolator.Update();
		base.transform.rotation = Quaternion.Euler(this.rotation.Value);
	}

	// Token: 0x06000EC2 RID: 3778 RVA: 0x00076318 File Offset: 0x00074518
	protected bool RequireAIPlaneRoll(ref Vector2 axisInput, ref bool AIUseInput, float roll)
	{
		Vector3 eulerAngles = base.transform.eulerAngles;
		if (Mathf.Abs(roll - eulerAngles.z) > 10f)
		{
			axisInput.x = -1f;
			eulerAngles.z = Mathf.MoveTowardsAngle(eulerAngles.z, roll, this.m_maxRollVelocity * Time.deltaTime);
			base.transform.eulerAngles = eulerAngles;
			AIUseInput = false;
			return false;
		}
		return true;
	}

	// Token: 0x06000EC3 RID: 3779 RVA: 0x00076384 File Offset: 0x00074584
	private bool RequireAIPlanePitch(ref Vector2 axisInput, ref bool AIUseInput, float pitch)
	{
		Vector3 eulerAngles = base.transform.eulerAngles;
		if (Mathf.Abs(pitch - eulerAngles.x) > 5f)
		{
			axisInput.y = -1f;
			eulerAngles.x = Mathf.MoveTowardsAngle(eulerAngles.x, pitch, this.m_maxRollVelocity * Time.deltaTime);
			base.transform.eulerAngles = eulerAngles;
			AIUseInput = false;
			return false;
		}
		return true;
	}

	// Token: 0x06000EC4 RID: 3780 RVA: 0x000763F0 File Offset: 0x000745F0
	private void UpdateAISensors()
	{
		this.m_collisionForward = false;
		this.m_collisionLeft = false;
		this.m_collisionRight = false;
		Vector3 origin = base.transform.position;
		this.m_collisionForward = Physics.Raycast(origin, base.transform.forward, this.m_sensorRayDistance, this.m_sensorMask);
		this.m_collisionLeft = Physics.Raycast(origin, this.m_sensorLeftRot * base.transform.forward, this.m_sensorRayDistance, this.m_sensorMask);
		this.m_collisionRight = Physics.Raycast(origin, this.m_sensorRightRot * base.transform.forward, this.m_sensorRayDistance, this.m_sensorMask);
	}

	// Token: 0x06000EC5 RID: 3781 RVA: 0x000764A0 File Offset: 0x000746A0
	protected virtual void UpdateAI(ref Vector2 axisInput, ref bool aiUseInput)
	{
		if (Time.time > this.m_nextSensorUpdate)
		{
			this.UpdateAISensors();
			this.m_nextSensorUpdate = Time.time + this.m_sensorUpdateInterval;
		}
		if (base.transform.position.y < 40f || this.m_aiAscending)
		{
			if (base.transform.position.y < 70f)
			{
				this.m_aiAscending = true;
			}
			else
			{
				this.m_aiAscending = false;
			}
			if (this.RequireAIPlaneRoll(ref axisInput, ref aiUseInput, 0f))
			{
				Vector3 eulerAngles = base.transform.eulerAngles;
				eulerAngles.x = Mathf.MoveTowardsAngle(eulerAngles.x, -70f, this.m_maxPitchVelocity * Time.deltaTime);
				base.transform.eulerAngles = eulerAngles;
				aiUseInput = false;
				return;
			}
		}
		else if (this.m_collisionForward || this.m_collisionLeft || this.m_collisionRight)
		{
			if (this.RequireAIPlaneRoll(ref axisInput, ref aiUseInput, 85f) && this.RequireAIPlanePitch(ref axisInput, ref aiUseInput, 0f))
			{
				Vector3 eulerAngles2 = base.transform.eulerAngles;
				eulerAngles2.y -= this.m_maxPitchVelocity * Time.deltaTime;
				base.transform.eulerAngles = eulerAngles2;
				aiUseInput = false;
				return;
			}
		}
		else if (base.transform.position.y > 200f || this.m_aiDescending)
		{
			if (base.transform.position.y > 120f)
			{
				this.m_aiDescending = true;
			}
			else
			{
				this.m_aiDescending = false;
			}
			if (this.RequireAIPlaneRoll(ref axisInput, ref aiUseInput, 0f))
			{
				Vector3 eulerAngles3 = base.transform.eulerAngles;
				eulerAngles3.x = Mathf.MoveTowardsAngle(eulerAngles3.x, 45f, this.m_maxPitchVelocity * Time.deltaTime);
				base.transform.eulerAngles = eulerAngles3;
				aiUseInput = false;
				return;
			}
		}
		else
		{
			Vector3 eulerAngles4 = Quaternion.LookRotation((this.m_aiTargetPos - base.transform.position).normalized, Vector3.up).eulerAngles;
			Vector3 eulerAngles5 = base.transform.eulerAngles;
			if (Mathf.Abs(eulerAngles4.y - eulerAngles5.y) > 10f)
			{
				if (eulerAngles5.z < 90f && eulerAngles5.z > 80f)
				{
					axisInput.y = -1f;
					eulerAngles5.y = Mathf.MoveTowardsAngle(eulerAngles5.y, eulerAngles4.y, this.m_maxPitchVelocity * Time.deltaTime);
					base.transform.eulerAngles = eulerAngles5;
					aiUseInput = false;
					return;
				}
				eulerAngles5.z = Mathf.MoveTowardsAngle(eulerAngles5.z, 85f, this.m_maxRollVelocity * Time.deltaTime);
				base.transform.eulerAngles = eulerAngles5;
				aiUseInput = false;
				axisInput.x = -1f;
				return;
			}
			else if (Mathf.Abs(eulerAngles4.x - eulerAngles5.x) > 10f)
			{
				if (this.RequireAIPlaneRoll(ref axisInput, ref aiUseInput, 0f))
				{
					eulerAngles5.x = Mathf.MoveTowardsAngle(eulerAngles5.x, eulerAngles4.x, this.m_maxPitchVelocity * Time.deltaTime);
					base.transform.eulerAngles = eulerAngles5;
					aiUseInput = false;
					return;
				}
			}
			else
			{
				this.RequireAIPlaneRoll(ref axisInput, ref aiUseInput, 0f);
			}
		}
	}

	// Token: 0x06000EC6 RID: 3782 RVA: 0x000767DC File Offset: 0x000749DC
	public void OnTriggerEnter(Collider other)
	{
		if (base.IsOwner && !this.isDead)
		{
			if (other.gameObject.layer == LayerMask.NameToLayer("WorldWall") || other.gameObject.layer == LayerMask.NameToLayer("WorldGround"))
			{
				this.KillPlayer(true);
				if (this.m_decreaseScoreOnCrash)
				{
					if (NetSystem.IsServer)
					{
						this.Score -= 1;
						return;
					}
					base.SendRPC("RPCChangeScore", NetRPCDelivery.RELIABLE_ORDERED, new object[]
					{
						-1
					});
					return;
				}
			}
			else
			{
				this.OnTriggerEnterOther(other);
			}
		}
	}

	// Token: 0x06000EC7 RID: 3783 RVA: 0x0000398C File Offset: 0x00001B8C
	protected virtual void OnTriggerEnterOther(Collider other)
	{
	}

	// Token: 0x06000EC8 RID: 3784 RVA: 0x00076870 File Offset: 0x00074A70
	public void KillPlayer(bool sendRPC = true)
	{
		if (this.isDead)
		{
			return;
		}
		if (base.IsOwner)
		{
			base.SendRPC("RPCKillPlayer", NetRPCDelivery.RELIABLE_ORDERED, Array.Empty<object>());
		}
		base.IsDead = true;
		this.m_respawnTime = Time.time + 3f;
		AudioSystem.PlayOneShot(this.m_explodeSound, base.transform.position, 1f, AudioRolloffMode.Logarithmic, 100f, 100f, 0f);
		UnityEngine.Object.Instantiate<GameObject>(this.m_tankDeathPrefab, base.transform.position, Quaternion.identity);
		GameObject[] disableObjects = this.m_disableObjects;
		for (int i = 0; i < disableObjects.Length; i++)
		{
			disableObjects[i].SetActive(false);
		}
		this.m_smokeTrail.Pause();
	}

	// Token: 0x06000EC9 RID: 3785 RVA: 0x00076928 File Offset: 0x00074B28
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCKillPlayer(NetPlayer sender)
	{
		try
		{
			this.KillPlayer(true);
		}
		catch (Exception ex)
		{
			Debug.LogError("PlanesPlayer Error : " + ex.Message);
		}
	}

	// Token: 0x06000ECA RID: 3786 RVA: 0x00076968 File Offset: 0x00074B68
	public void RespawnPlayer()
	{
		if (!this.isDead)
		{
			return;
		}
		if (base.IsOwner)
		{
			base.SendRPC("RPCRespawnPlayer", NetRPCDelivery.RELIABLE_ORDERED, Array.Empty<object>());
		}
		this.isDead = false;
		if (NetSystem.IsServer)
		{
			this.Health = 5;
		}
		this.m_rollDissipateVelocity = 0f;
		this.m_rollVelocity = 0f;
		this.m_pitchDissipateVelocity = 0f;
		this.m_pitchVelocity = 0f;
		Vector3 zero = Vector3.zero;
		Quaternion identity = Quaternion.identity;
		this.GetRespawnTransform(ref zero, ref identity);
		base.transform.position = zero;
		base.transform.rotation = identity;
		GameObject[] disableObjects = this.m_disableObjects;
		for (int i = 0; i < disableObjects.Length; i++)
		{
			disableObjects[i].SetActive(true);
		}
		this.m_smokeTrail.Play();
	}

	// Token: 0x06000ECB RID: 3787 RVA: 0x00076A30 File Offset: 0x00074C30
	protected virtual void GetRespawnTransform(ref Vector3 pos, ref Quaternion rot)
	{
		Vector3 a = new Vector3(0f, (float)(100 + UnityEngine.Random.Range(-10, 10)), 172f);
		Vector3 a2 = UnityEngine.Random.onUnitSphere;
		a2.y = 0f;
		a2 = a2.normalized;
		pos = a + a2 * 500f;
		rot = Quaternion.LookRotation(-a2, Vector3.up);
	}

	// Token: 0x06000ECC RID: 3788 RVA: 0x00076AA4 File Offset: 0x00074CA4
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCRespawnPlayer(NetPlayer sender)
	{
		try
		{
			this.RespawnPlayer();
		}
		catch (Exception ex)
		{
			Debug.LogError("PlanesPlayer Error : " + ex.Message);
		}
	}

	// Token: 0x06000ECD RID: 3789 RVA: 0x0000CE8E File Offset: 0x0000B08E
	[NetRPC(false, NetRPCSecurity.OWNER, NetRPCSecurity.SERVER)]
	public void RPCChangeScore(NetPlayer sender, short change)
	{
		if (NetSystem.IsServer)
		{
			this.Score += change;
		}
	}

	// Token: 0x04000E4C RID: 3660
	[SerializeField]
	protected GameObject m_cameraParent;

	// Token: 0x04000E4D RID: 3661
	[SerializeField]
	protected Camera m_cam;

	// Token: 0x04000E4E RID: 3662
	[SerializeField]
	protected AudioListener m_listener;

	// Token: 0x04000E4F RID: 3663
	[SerializeField]
	protected Transform m_planeBody;

	// Token: 0x04000E50 RID: 3664
	[SerializeField]
	protected float m_rotateSpeed = 10f;

	// Token: 0x04000E51 RID: 3665
	[SerializeField]
	protected float m_planeVelocity = 40f;

	// Token: 0x04000E52 RID: 3666
	[SerializeField]
	protected bool m_decreaseScoreOnCrash = true;

	// Token: 0x04000E53 RID: 3667
	[Header("Control Surfaces")]
	[SerializeField]
	protected Transform m_leftAileron;

	// Token: 0x04000E54 RID: 3668
	[SerializeField]
	protected Transform m_rightAileron;

	// Token: 0x04000E55 RID: 3669
	[SerializeField]
	protected Transform m_leftElevator;

	// Token: 0x04000E56 RID: 3670
	[SerializeField]
	protected Transform m_rightElevator;

	// Token: 0x04000E57 RID: 3671
	[SerializeField]
	protected Transform m_rudder;

	// Token: 0x04000E58 RID: 3672
	[SerializeField]
	protected float m_rotateSmoothTime = 0.2f;

	// Token: 0x04000E59 RID: 3673
	[Header("Yaw")]
	[SerializeField]
	protected float m_maxYawVelocity = 150f;

	// Token: 0x04000E5A RID: 3674
	[SerializeField]
	protected float m_yawAccelerationTime = 0.2f;

	// Token: 0x04000E5B RID: 3675
	[SerializeField]
	protected float m_yawDissipateTime = 0.2f;

	// Token: 0x04000E5C RID: 3676
	[Header("Roll")]
	[SerializeField]
	protected float m_maxRollVelocity = 150f;

	// Token: 0x04000E5D RID: 3677
	[SerializeField]
	protected float m_rollAccelerationTime = 0.2f;

	// Token: 0x04000E5E RID: 3678
	[SerializeField]
	protected float m_rollDissipateTime = 0.2f;

	// Token: 0x04000E5F RID: 3679
	[Header("Pitch")]
	[SerializeField]
	protected float m_maxPitchVelocity = 150f;

	// Token: 0x04000E60 RID: 3680
	[SerializeField]
	protected float m_pitchAccelerationTime = 0.2f;

	// Token: 0x04000E61 RID: 3681
	[SerializeField]
	protected float m_pitchDissipateTime = 0.2f;

	// Token: 0x04000E62 RID: 3682
	[Header("Player Color")]
	[SerializeField]
	protected MeshRenderer[] m_renderers;

	// Token: 0x04000E63 RID: 3683
	[SerializeField]
	protected Material m_baseMaterial;

	// Token: 0x04000E64 RID: 3684
	[Header("Effects")]
	[SerializeField]
	protected GameObject m_tankDeathPrefab;

	// Token: 0x04000E65 RID: 3685
	[SerializeField]
	protected GameObject m_hitSparkPfb;

	// Token: 0x04000E66 RID: 3686
	[Header("Audio")]
	[SerializeField]
	protected AudioClip m_explodeSound;

	// Token: 0x04000E67 RID: 3687
	[SerializeField]
	protected AudioSource m_planeEngine;

	// Token: 0x04000E68 RID: 3688
	[SerializeField]
	protected GameObject[] m_disableObjects;

	// Token: 0x04000E69 RID: 3689
	[Header("Smoke Trail")]
	[SerializeField]
	protected ParticleSystem m_smokeTrail;

	// Token: 0x04000E6A RID: 3690
	[SerializeField]
	protected Material m_smokeMat;

	// Token: 0x04000E6B RID: 3691
	[SerializeField]
	protected float m_engineVolume = 1f;

	// Token: 0x04000E6C RID: 3692
	private float m_rollDissipateVelocity;

	// Token: 0x04000E6D RID: 3693
	private float m_rollVelocity;

	// Token: 0x04000E6E RID: 3694
	private float m_pitchDissipateVelocity;

	// Token: 0x04000E6F RID: 3695
	private float m_pitchVelocity;

	// Token: 0x04000E70 RID: 3696
	private float m_yawDissipateVelocity;

	// Token: 0x04000E71 RID: 3697
	private float m_yawVelocity;

	// Token: 0x04000E72 RID: 3698
	protected MinigameController minigameController;

	// Token: 0x04000E73 RID: 3699
	private bool m_hasCamera;

	// Token: 0x04000E74 RID: 3700
	private CameraShake cameraShake;

	// Token: 0x04000E75 RID: 3701
	private float m_respawnTime;

	// Token: 0x04000E76 RID: 3702
	[NetSend(-1, NetSendOwner.SERVER, NetSendFlags.NONE)]
	public NetVar<byte> health = new NetVar<byte>(5);

	// Token: 0x04000E77 RID: 3703
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.ALWAYS_SEND)]
	public NetVec3 position = new NetVec3(Vector3.zero);

	// Token: 0x04000E78 RID: 3704
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.ALWAYS_SEND)]
	public NetVec3 rotation = new NetVec3(Vector3.zero);

	// Token: 0x04000E79 RID: 3705
	private Interpolator interpolator;

	// Token: 0x04000E7A RID: 3706
	private Quaternion m_sensorRightRot;

	// Token: 0x04000E7B RID: 3707
	private Quaternion m_sensorLeftRot;

	// Token: 0x04000E7C RID: 3708
	private int m_sensorMask;

	// Token: 0x04000E7D RID: 3709
	private float m_sensorRayDistance = 100f;

	// Token: 0x04000E7E RID: 3710
	private bool m_collisionForward;

	// Token: 0x04000E7F RID: 3711
	private bool m_collisionLeft;

	// Token: 0x04000E80 RID: 3712
	private bool m_collisionRight;

	// Token: 0x04000E81 RID: 3713
	private bool m_aiAscending;

	// Token: 0x04000E82 RID: 3714
	private bool m_aiDescending;

	// Token: 0x04000E83 RID: 3715
	private float m_nextSensorUpdate;

	// Token: 0x04000E84 RID: 3716
	private float m_sensorUpdateInterval = 0.25f;

	// Token: 0x04000E85 RID: 3717
	private Vector3 m_camPosVelocity;

	// Token: 0x04000E86 RID: 3718
	private float AngularVelocity;

	// Token: 0x04000E87 RID: 3719
	private float m_rudderAngle;

	// Token: 0x04000E88 RID: 3720
	private float m_rudderVelocity;

	// Token: 0x04000E89 RID: 3721
	private float m_aileronAngle;

	// Token: 0x04000E8A RID: 3722
	private float m_aileronVelocity;

	// Token: 0x04000E8B RID: 3723
	private float m_elevatorAngle;

	// Token: 0x04000E8C RID: 3724
	private float m_elevatorVelocity;

	// Token: 0x04000E8D RID: 3725
	private float m_nextAiPos;

	// Token: 0x04000E8E RID: 3726
	private Vector3 m_aiTargetPos;

	// Token: 0x04000E8F RID: 3727
	[SerializeField]
	protected float m_camDistance = 2.5f;

	// Token: 0x04000E90 RID: 3728
	protected float m_camDampTime = 0.2f;
}
