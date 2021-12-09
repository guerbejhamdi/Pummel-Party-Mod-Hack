using System;
using System.Collections.Generic;
using Rewired;
using UnityEngine;
using ZP.Net;

// Token: 0x02000277 RID: 631
public class TanksPlayer : CharacterBase
{
	// Token: 0x06001261 RID: 4705 RVA: 0x0008D998 File Offset: 0x0008BB98
	public override void OnNetInitialize()
	{
		base.OnNetInitialize();
		if (!base.IsOwner)
		{
			this.position.Recieve = new RecieveProxy(this.RecievePosition);
			this.velocity.Recieve = new RecieveProxy(this.RecieveVelocity);
			this.angularVelocity.Recieve = new RecieveProxy(this.RecieveAngularVelocity);
			this.n_rotX.Recieve = new RecieveProxy(this.RecieveRotX);
			this.n_rotY.Recieve = new RecieveProxy(this.RecieveRotY);
			this.n_rotZ.Recieve = new RecieveProxy(this.RecieveRotZ);
			this.n_aimDirection.Recieve = new RecieveProxy(this.RecieveAimDirection);
			return;
		}
		this.position.Value = base.transform.position;
		this.SetNetRotation();
	}

	// Token: 0x06001262 RID: 4706 RVA: 0x0008DA7C File Offset: 0x0008BC7C
	protected override void Start()
	{
		base.Start();
		this.minigameController = (TanksController)GameManager.Minigame;
		Canvas canvas;
		if (this.minigameController != null)
		{
			this.minigameController.AddPlayer(this);
			canvas = this.minigameController.Root.GetComponentInChildren<Canvas>();
			this.m_cam = this.minigameController.MinigameCamera;
		}
		else
		{
			canvas = UnityEngine.Object.FindObjectOfType<Canvas>();
			this.m_cam = UnityEngine.Object.FindObjectOfType<Camera>();
		}
		if (canvas != null && this.player != null && !this.player.IsAI && base.IsOwner)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_aimerPrefab);
			gameObject.transform.SetParent(canvas.transform, false);
			this.m_aimer = gameObject.GetComponent<UITankAimer>();
			this.m_aimer.SetColor(this.player.Color.uiColor);
			this.m_aimer.gameObject.SetActive(false);
		}
		if (this.player != null)
		{
			this.m_uniqueMaterial.SetColor("_Color", this.player.Color.skinColor1);
		}
	}

	// Token: 0x06001263 RID: 4707 RVA: 0x0008DB94 File Offset: 0x0008BD94
	public void Awake()
	{
		this.m_body = base.GetComponent<Rigidbody>();
		this.m_uniqueMaterial = new Material(Shader.Find("Standard"));
		this.m_uniqueMaterial.CopyPropertiesFromMaterial(this.m_material);
		foreach (MeshRenderer meshRenderer in base.GetComponentsInChildren<MeshRenderer>())
		{
			if (meshRenderer.sharedMaterial == this.m_material)
			{
				meshRenderer.sharedMaterial = this.m_uniqueMaterial;
			}
		}
		this.m_body.centerOfMass = this.m_centerOfGravity.localPosition;
	}

	// Token: 0x06001264 RID: 4708 RVA: 0x0008DC24 File Offset: 0x0008BE24
	private void Update()
	{
		if (this.m_isDead)
		{
			return;
		}
		if (!this.gotAchievement && base.IsOwner && !this.player.IsAI && base.transform.position.y > 13f)
		{
			PlatformAchievementManager.Instance.TriggerAchievement("ACH_TUNNELING_TANKS");
			this.gotAchievement = true;
		}
		bool flag = !(this.minigameController == null) && this.minigameController.Playable;
		if (flag && this.m_body.isKinematic)
		{
			this.m_body.isKinematic = false;
		}
		Vector3 vector = Vector3.zero;
		Vector3 vector2 = Vector3.zero;
		bool flag2 = true;
		Vector2 vector3 = Vector3.zero;
		bool flag3 = false;
		Player player = null;
		if (this.m_aimer != null && !this.m_aimer.gameObject.activeSelf && this.minigameController.Playable)
		{
			this.m_aimer.gameObject.SetActive(true);
		}
		if (base.IsOwner && this.minigameController.Playable && !this.m_isDead)
		{
			if (this.player != null)
			{
				player = this.player.RewiredPlayer;
			}
			if (!this.player.IsAI)
			{
				if (player != null)
				{
					if (player.controllers.GetLastActiveController() != null && player.controllers.GetLastActiveController().type == ControllerType.Joystick)
					{
						float axis = this.player.RewiredPlayer.GetAxis(InputActions.LookHorizontal);
						float axis2 = this.player.RewiredPlayer.GetAxis(InputActions.LookVertical);
						Vector3 vector4 = new Vector3(axis, axis2, 0f);
						if (vector4.magnitude < 0.1f)
						{
							vector4 = this.lastAimDir;
						}
						vector2 = base.transform.position + vector4.normalized * 5f;
						vector2.z = 0f;
						vector = this.m_cam.WorldToScreenPoint(vector2);
						this.lastAimDir = vector4;
						flag2 = false;
					}
					else
					{
						vector = Input.mousePosition;
						vector2.z = 0f;
					}
				}
				else if (this.player == null || !this.player.IsAI)
				{
					vector = Input.mousePosition;
					vector2 = this.m_cam.ScreenToWorldPoint(Input.mousePosition);
				}
				if (flag2)
				{
					Ray ray = this.m_cam.ScreenPointToRay(Input.mousePosition);
					RaycastHit raycastHit;
					if (Physics.Raycast(ray, out raycastHit, 100f, LayerMask.GetMask(new string[]
					{
						"WorldUI"
					})))
					{
						vector2 = raycastHit.point;
					}
					Debug.DrawRay(ray.origin, ray.direction, Color.green);
				}
				if (player != null)
				{
					vector3 = new Vector2(this.player.RewiredPlayer.GetAxis(InputActions.Horizontal), this.player.RewiredPlayer.GetAxis(InputActions.Vertical));
					if (Mathf.Abs(vector3.x) > 0.01f)
					{
						flag3 = true;
					}
					vector3.x *= 10000f;
				}
				else if (this.player == null || !this.player.IsAI)
				{
					if (Input.GetKey(KeyCode.A))
					{
						vector3.x -= 100000f;
						flag3 = true;
					}
					if (Input.GetKey(KeyCode.D))
					{
						vector3.x += 100000f;
						flag3 = true;
					}
				}
			}
			else
			{
				if (Time.time > this.m_nextMoveDirectionChange)
				{
					int num = UnityEngine.Random.Range(0, 5);
					this.m_aiMoveDirection = 0f;
					if (num == 0)
					{
						this.m_aiMoveDirection = -1f;
					}
					else if (num == 1)
					{
						this.m_aiMoveDirection = 1f;
					}
					this.m_nextMoveDirectionChange = Time.time + UnityEngine.Random.Range(0.5f, 1f);
					if (this.m_players == null)
					{
						this.m_players = UnityEngine.Object.FindObjectsOfType<TanksPlayer>();
					}
					TanksPlayer tanksPlayer = null;
					if (this.m_players != null && this.m_players.Length != 0)
					{
						int num2 = UnityEngine.Random.Range(0, this.m_players.Length);
						tanksPlayer = this.m_players[num2];
						if (tanksPlayer != this)
						{
							vector2 = this.m_players[num2].transform.position + UnityEngine.Random.insideUnitSphere;
						}
						else
						{
							tanksPlayer = null;
						}
					}
					RaycastHit raycastHit2;
					if (tanksPlayer != null && UnityEngine.Random.value > 0.25f && Physics.Raycast(tanksPlayer.transform.position, Vector3.down, out raycastHit2, (float)LayerMask.GetMask(new string[]
					{
						"Default"
					})) && raycastHit2.distance < 1f)
					{
						this.Fire(this.m_fireSource.position, (vector2 - this.m_fireSource.position).normalized);
					}
				}
				float num3 = float.NegativeInfinity;
				float num4 = 1f;
				RaycastHit raycastHit3;
				if (Physics.Raycast(base.transform.position + new Vector3(0f, 100f, 0f), Vector3.down, out raycastHit3, (float)LayerMask.GetMask(new string[]
				{
					"Default"
				})))
				{
					num3 = raycastHit3.point.y;
				}
				if (Physics.Raycast(base.transform.position + new Vector3((this.m_aiMoveDirection < 0f) ? -1f : 1f, 0f, 0f), Vector3.down, out raycastHit3, (float)LayerMask.GetMask(new string[]
				{
					"Default"
				})))
				{
					if (num3 - raycastHit3.point.y > 0f)
					{
						num4 = 0f;
					}
				}
				else
				{
					num4 = 0f;
				}
				vector3.x = 2500f * num4 * this.m_aiMoveDirection;
				if (this.m_aiMoveDirection != 0f && num4 != 0f)
				{
					flag3 = true;
				}
			}
		}
		if (flag && base.IsOwner)
		{
			Vector3 vector5 = this.m_body.angularVelocity;
			float num5 = flag3 ? vector3.x : this.lastInput.x;
			float b = 5f;
			float num6 = Mathf.Min(Mathf.Abs(90f - base.transform.eulerAngles.y), b);
			float num7 = Mathf.Min(Mathf.Abs(270f - base.transform.eulerAngles.y), b);
			if (num5 > 0f)
			{
				vector5.y = ((base.transform.eulerAngles.y < 90f) ? num6 : (-num6));
			}
			else if (num5 < 0f)
			{
				vector5.y = ((base.transform.eulerAngles.y < 270f) ? num7 : (-num7));
			}
			this.m_body.angularVelocity = vector5;
			if (flag3)
			{
				this.lastInput = vector3;
			}
			if (this.m_leftWheels != null)
			{
				foreach (WheelCollider wheelCollider in this.m_leftWheels)
				{
					wheelCollider.motorTorque = Mathf.Abs(vector3.x);
					wheelCollider.brakeTorque = ((vector3.x == 0f) ? 10000000f : 0f);
				}
			}
			if (this.m_rightWheels != null)
			{
				foreach (WheelCollider wheelCollider2 in this.m_rightWheels)
				{
					wheelCollider2.motorTorque = Mathf.Abs(vector3.x);
					wheelCollider2.brakeTorque = ((vector3.x == 0f) ? 10000000f : 0f);
				}
			}
			RaycastHit raycastHit4;
			if (Mathf.Abs(vector3.x) > 0.1f && Physics.Raycast(base.transform.position, Vector3.down, out raycastHit4, (float)LayerMask.GetMask(new string[]
			{
				"Default"
			})) && raycastHit4.distance < 1.5f)
			{
				float d = Time.deltaTime * 60f;
				foreach (Transform transform in this.m_upwardForcePositions)
				{
					this.m_body.AddForceAtPosition(Vector3.up * 30f * d, transform.position);
					Debug.DrawLine(transform.position, transform.position + Vector3.up, Color.cyan);
				}
				this.m_body.AddForceAtPosition(new Vector3(vector3.x, 0f, 0f) * 7f * d, base.transform.position);
				Debug.DrawLine(base.transform.position, base.transform.position + new Vector3(vector3.x, 0f, 0f), Color.magenta);
			}
		}
		if (!base.IsOwner)
		{
			vector2 = base.transform.position + this.recievedAimDirection;
		}
		Vector3 normalized = (vector2 - base.transform.position).normalized;
		if (Vector3.Distance(vector2, base.transform.position) < 0.01f)
		{
			normalized = new Vector3(1f, 0f, 0f);
		}
		Debug.DrawLine(this.m_barrel.position, this.m_barrel.position + normalized * 5f, Color.yellow);
		Vector3 normalized2 = new Vector3(normalized.x, 0f, normalized.z).normalized;
		Vector3 forward = base.transform.InverseTransformDirection(normalized2);
		forward.y = 0f;
		forward = forward.normalized;
		this.m_turret.localRotation = Quaternion.LookRotation(forward);
		Vector3 normalized3 = (vector2 - this.m_barrel.position).normalized;
		Vector3 normalized4 = new Vector3(normalized3.x, normalized3.y, 0f).normalized;
		Debug.DrawLine(this.m_barrel.position, this.m_barrel.position + normalized4 * 5f, Color.green);
		Debug.DrawLine(this.m_barrel.position, this.m_barrel.position + normalized2 * 5f, Color.red);
		Vector3 forward2 = this.m_turret.InverseTransformDirection(normalized4);
		forward2.x = 0f;
		forward2 = forward2.normalized;
		Vector3 eulerAngles = Quaternion.LookRotation(forward2).eulerAngles;
		if (eulerAngles.x > 15f && eulerAngles.x < 270f)
		{
			eulerAngles.x = 15f;
		}
		this.m_barrel.localRotation = Quaternion.Euler(eulerAngles);
		if (base.IsOwner)
		{
			if (player != null)
			{
				if (flag && !GameManager.IsGamePaused && player.GetButtonDown(InputActions.UseItemShoot))
				{
					this.Fire(this.m_fireSource.position, (vector2 - this.m_fireSource.position).normalized);
				}
			}
			else if ((this.player == null || !this.player.IsAI) && flag && Input.GetMouseButtonDown(0))
			{
				this.Fire(this.m_fireSource.position, (vector2 - this.m_fireSource.position).normalized);
			}
			if (this.m_aimer != null)
			{
				this.m_aimer.transform.position = vector;
				this.m_aimer.SetFill(Mathf.Clamp01((Time.time - this.m_lastFire) / this.m_fireRate));
			}
			this.position.Value = base.transform.position;
			this.velocity.Value = this.m_body.velocity;
			this.SetNetRotation();
			float num8 = Vector3.SignedAngle(Vector3.up, normalized, new Vector3(0f, 0f, 1f)) + 180f;
			this.n_aimDirection.Value = (byte)(num8 / 360f * 255f);
			if (flag && base.transform.position.y < -12f)
			{
				this.Explode();
			}
		}
	}

	// Token: 0x06001265 RID: 4709 RVA: 0x0008E864 File Offset: 0x0008CA64
	private void Fire(Vector3 source, Vector3 direction)
	{
		if (this.minigameController == null || !this.minigameController.Playable || Time.time - this.m_lastFire < this.m_fireRate || this.m_isDead)
		{
			return;
		}
		this.m_lastFire = Time.time;
		float num = 1000f;
		RaycastHit raycastHit;
		if (Physics.Raycast(source, direction, out raycastHit, num, LayerMask.GetMask(new string[]
		{
			"Default",
			"Players"
		})))
		{
			this.OnFire(source, raycastHit.point, true);
			return;
		}
		this.OnFire(source, source + direction * num, false);
	}

	// Token: 0x06001266 RID: 4710 RVA: 0x0008E908 File Offset: 0x0008CB08
	private void OnFire(Vector3 source, Vector3 hitPoint, bool didHit)
	{
		if (base.IsOwner)
		{
			base.SendRPC("OnFireRPC", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				hitPoint,
				didHit
			});
		}
		AudioSystem.PlayOneShot(this.m_fireSound, source, 0.5f, AudioRolloffMode.Linear, 10f, 30f, 0f);
		this.m_barrelFlash.Emit(3);
		if (didHit)
		{
			this.minigameController.Edit(hitPoint);
			UnityEngine.Object.Instantiate<GameObject>(this.m_hitExplosionPrefab, hitPoint, Quaternion.identity);
			Collider[] array = Physics.OverlapSphere(hitPoint, this.m_explosionCollisionRadius, LayerMask.GetMask(new string[]
			{
				"Players"
			}));
			HashSet<Rigidbody> hashSet = new HashSet<Rigidbody>();
			foreach (Collider collider in array)
			{
				if (collider.attachedRigidbody != null && !hashSet.Contains(collider.attachedRigidbody))
				{
					hashSet.Add(collider.attachedRigidbody);
				}
			}
			foreach (Rigidbody rigidbody in hashSet)
			{
				rigidbody.AddExplosionForce(this.m_explosionForce, hitPoint, this.m_explosionForceRadius, this.m_upwardsModifier);
			}
		}
	}

	// Token: 0x06001267 RID: 4711 RVA: 0x0008EA48 File Offset: 0x0008CC48
	private void FixedUpdate()
	{
		if (base.IsOwner)
		{
			base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y, 0f);
			Vector3 vector = this.m_body.angularVelocity;
			if (base.transform.rotation.eulerAngles.z > 70f)
			{
				base.transform.rotation = Quaternion.Euler(base.transform.rotation.eulerAngles.x, base.transform.rotation.eulerAngles.y, 0f);
				return;
			}
			if (base.transform.rotation.eulerAngles.z < 70f)
			{
				base.transform.rotation = Quaternion.Euler(base.transform.rotation.eulerAngles.x, base.transform.rotation.eulerAngles.y, 0f);
				return;
			}
		}
		else
		{
			if (this.rotChanged)
			{
				base.transform.rotation = Quaternion.Euler(this.recievedRotation);
				this.rotChanged = false;
			}
			if (this.recievedPosition)
			{
				base.transform.position = this.position.Value;
				this.recievedPosition = false;
			}
			if (this.recievedVelocity)
			{
				this.m_body.velocity = this.velocity.Value;
				this.recievedVelocity = false;
			}
			if (this.recievedAngularVelocity)
			{
				this.m_body.angularVelocity = this.angularVelocity.Value;
				this.recievedAngularVelocity = false;
			}
		}
	}

	// Token: 0x06001268 RID: 4712 RVA: 0x0000ED7F File Offset: 0x0000CF7F
	public void RecievePosition(object _pos)
	{
		this.recievedPosition = true;
	}

	// Token: 0x06001269 RID: 4713 RVA: 0x0000ED88 File Offset: 0x0000CF88
	public void RecieveVelocity(object _vel)
	{
		this.recievedVelocity = true;
	}

	// Token: 0x0600126A RID: 4714 RVA: 0x0000ED91 File Offset: 0x0000CF91
	public void RecieveAngularVelocity(object _angVel)
	{
		this.recievedAngularVelocity = true;
	}

	// Token: 0x0600126B RID: 4715 RVA: 0x0008EC0C File Offset: 0x0008CE0C
	private void SetNetRotation()
	{
		Vector3 eulerAngles = base.transform.eulerAngles;
		float num = Mathf.Repeat(eulerAngles.x, 360f);
		float num2 = Mathf.Repeat(eulerAngles.y, 360f);
		float num3 = Mathf.Repeat(eulerAngles.z, 360f);
		this.n_rotX.Value = (byte)(num / 360f * 255f);
		this.n_rotY.Value = (byte)(num2 / 360f * 255f);
		this.n_rotZ.Value = (byte)(num3 / 360f * 255f);
	}

	// Token: 0x0600126C RID: 4716 RVA: 0x0000ED9A File Offset: 0x0000CF9A
	public void RecieveRotX(object _val)
	{
		this.recievedRotation.x = (float)((byte)_val) / 255f * 360f;
		this.rotChanged = true;
	}

	// Token: 0x0600126D RID: 4717 RVA: 0x0000EDC1 File Offset: 0x0000CFC1
	public void RecieveRotY(object _val)
	{
		this.recievedRotation.y = (float)((byte)_val) / 255f * 360f;
		this.rotChanged = true;
	}

	// Token: 0x0600126E RID: 4718 RVA: 0x0000EDE8 File Offset: 0x0000CFE8
	public void RecieveRotZ(object _val)
	{
		this.recievedRotation.z = (float)((byte)_val) / 255f * 360f;
		this.rotChanged = true;
	}

	// Token: 0x0600126F RID: 4719 RVA: 0x0008ECA4 File Offset: 0x0008CEA4
	public void RecieveAimDirection(object _val)
	{
		float angle = (float)((byte)_val) / 255f * 360f - 180f;
		this.recievedAimDirection = Quaternion.AngleAxis(angle, new Vector3(0f, 0f, 1f)) * Vector3.up;
	}

	// Token: 0x06001270 RID: 4720 RVA: 0x0008ECF8 File Offset: 0x0008CEF8
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void OnFireRPC(NetPlayer sender, Vector3 hitPos, bool didHit)
	{
		try
		{
			this.OnFire(this.m_barrel.position, hitPos, didHit);
		}
		catch (Exception ex)
		{
			Debug.LogError("TanksPlayer Error : " + ex.Message);
		}
	}

	// Token: 0x06001271 RID: 4721 RVA: 0x0008ED44 File Offset: 0x0008CF44
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void ExplodeRPC(NetPlayer sender)
	{
		try
		{
			this.Explode();
		}
		catch (Exception ex)
		{
			Debug.LogError("TanksPlayer Error : " + ex.Message);
		}
	}

	// Token: 0x06001272 RID: 4722 RVA: 0x0008ED84 File Offset: 0x0008CF84
	private void Explode()
	{
		if (this.m_isDead)
		{
			return;
		}
		if (base.IsOwner)
		{
			base.SendRPC("ExplodeRPC", NetRPCDelivery.RELIABLE_ORDERED, Array.Empty<object>());
		}
		if (NetSystem.IsServer)
		{
			this.minigameController.PlayerDied(this);
		}
		base.IsDead = true;
		this.m_isDead = true;
		AudioSystem.PlayOneShot(this.m_fireSound, base.transform.position, 0.5f, AudioRolloffMode.Linear, 10f, 30f, 0f);
		UnityEngine.Object.Instantiate<GameObject>(this.m_tankDeathPrefab, base.transform.position, Quaternion.identity);
		foreach (GameObject gameObject in this.m_explodeGibParts)
		{
			gameObject.transform.parent = null;
			gameObject.AddComponent<BoxCollider>();
			gameObject.AddComponent<Rigidbody>().AddExplosionForce(100f, base.transform.position, 5f, 25f);
			UnityEngine.Object.Destroy(gameObject, 4f);
		}
		foreach (object obj in base.transform)
		{
			((Transform)obj).gameObject.SetActive(false);
		}
		if (this.m_aimer != null)
		{
			this.m_aimer.gameObject.SetActive(false);
		}
		this.m_uniqueMaterial.SetTexture("_MainTex", this.m_destroyedTexture);
	}

	// Token: 0x04001355 RID: 4949
	[SerializeField]
	protected Rigidbody m_moverBody;

	// Token: 0x04001356 RID: 4950
	[SerializeField]
	protected Transform m_tankBody;

	// Token: 0x04001357 RID: 4951
	[SerializeField]
	protected SphereCollider m_moverCollider;

	// Token: 0x04001358 RID: 4952
	[SerializeField]
	protected Transform m_centerOfGravity;

	// Token: 0x04001359 RID: 4953
	private TanksController minigameController;

	// Token: 0x0400135A RID: 4954
	[SerializeField]
	protected WheelCollider[] m_leftWheels;

	// Token: 0x0400135B RID: 4955
	[SerializeField]
	protected WheelCollider[] m_rightWheels;

	// Token: 0x0400135C RID: 4956
	[Header("Properties")]
	[SerializeField]
	protected float m_fireRate = 2f;

	// Token: 0x0400135D RID: 4957
	[SerializeField]
	protected float m_explosionCollisionRadius = 10f;

	// Token: 0x0400135E RID: 4958
	[SerializeField]
	protected float m_explosionForceRadius = 10f;

	// Token: 0x0400135F RID: 4959
	[SerializeField]
	protected float m_explosionForce = 1000f;

	// Token: 0x04001360 RID: 4960
	[SerializeField]
	protected float m_upwardsModifier = 10f;

	// Token: 0x04001361 RID: 4961
	[Header("References")]
	[SerializeField]
	protected GameObject m_aimerPrefab;

	// Token: 0x04001362 RID: 4962
	[SerializeField]
	protected Material m_material;

	// Token: 0x04001363 RID: 4963
	[SerializeField]
	protected Transform m_turret;

	// Token: 0x04001364 RID: 4964
	[SerializeField]
	protected Transform m_barrel;

	// Token: 0x04001365 RID: 4965
	[SerializeField]
	protected Transform m_fireSource;

	// Token: 0x04001366 RID: 4966
	[SerializeField]
	protected Light m_playerColorLight;

	// Token: 0x04001367 RID: 4967
	[SerializeField]
	protected Transform[] m_upwardForcePositions;

	// Token: 0x04001368 RID: 4968
	[Header("Effects")]
	[SerializeField]
	protected GameObject m_hitExplosionPrefab;

	// Token: 0x04001369 RID: 4969
	[SerializeField]
	protected GameObject m_tankDeathPrefab;

	// Token: 0x0400136A RID: 4970
	[SerializeField]
	protected ParticleSystem m_barrelFlash;

	// Token: 0x0400136B RID: 4971
	[Header("Audio")]
	[SerializeField]
	protected AudioClip m_fireSound;

	// Token: 0x0400136C RID: 4972
	[Header("Break")]
	[SerializeField]
	protected GameObject[] m_explodeGibParts;

	// Token: 0x0400136D RID: 4973
	[SerializeField]
	protected Texture2D m_destroyedTexture;

	// Token: 0x0400136E RID: 4974
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.ALWAYS_SEND)]
	public NetVec2 position = new NetVec2(Vector2.zero);

	// Token: 0x0400136F RID: 4975
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.ALWAYS_SEND)]
	public NetVec2 velocity = new NetVec2(Vector2.zero);

	// Token: 0x04001370 RID: 4976
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.ALWAYS_SEND)]
	public NetVec3 angularVelocity = new NetVec3(Vector3.zero);

	// Token: 0x04001371 RID: 4977
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.ALWAYS_SEND)]
	public NetVar<byte> n_aimDirection = new NetVar<byte>(0);

	// Token: 0x04001372 RID: 4978
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.ALWAYS_SEND)]
	public NetVar<byte> n_rotX = new NetVar<byte>(0);

	// Token: 0x04001373 RID: 4979
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.ALWAYS_SEND)]
	public NetVar<byte> n_rotY = new NetVar<byte>(0);

	// Token: 0x04001374 RID: 4980
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.ALWAYS_SEND)]
	public NetVar<byte> n_rotZ = new NetVar<byte>(0);

	// Token: 0x04001375 RID: 4981
	public HalfNetVec3 a;

	// Token: 0x04001376 RID: 4982
	private bool gotPosition;

	// Token: 0x04001377 RID: 4983
	private Rigidbody m_body;

	// Token: 0x04001378 RID: 4984
	private UITankAimer m_aimer;

	// Token: 0x04001379 RID: 4985
	private Material m_uniqueMaterial;

	// Token: 0x0400137A RID: 4986
	private float m_lastFire = -10f;

	// Token: 0x0400137B RID: 4987
	private Camera m_cam;

	// Token: 0x0400137C RID: 4988
	private bool m_isDead;

	// Token: 0x0400137D RID: 4989
	private bool gotAchievement;

	// Token: 0x0400137E RID: 4990
	private Vector3 lastInput = new Vector3(1f, 0f, 0f);

	// Token: 0x0400137F RID: 4991
	private Vector3 lastAimDir = new Vector3(1f, 0f, 0f);

	// Token: 0x04001380 RID: 4992
	private float turretTurnVelocity;

	// Token: 0x04001381 RID: 4993
	private float m_aiMoveDirection = 1f;

	// Token: 0x04001382 RID: 4994
	private float m_nextMoveDirectionChange;

	// Token: 0x04001383 RID: 4995
	private TanksPlayer[] m_players;

	// Token: 0x04001384 RID: 4996
	private bool recievedPosition;

	// Token: 0x04001385 RID: 4997
	private bool recievedVelocity;

	// Token: 0x04001386 RID: 4998
	private bool recievedAngularVelocity;

	// Token: 0x04001387 RID: 4999
	private Vector3 recievedRotation = Vector3.zero;

	// Token: 0x04001388 RID: 5000
	private bool rotChanged;

	// Token: 0x04001389 RID: 5001
	private Vector3 recievedAimDirection = new Vector3(1f, 0f, 0f);
}
