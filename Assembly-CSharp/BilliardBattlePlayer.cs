using System;
using LlockhamIndustries.Decals;
using Rewired;
using UnityEngine;
using ZP.Net;
using ZP.Utility;

// Token: 0x02000152 RID: 338
public class BilliardBattlePlayer : CharacterBase
{
	// Token: 0x170000DE RID: 222
	// (get) Token: 0x060009B1 RID: 2481 RVA: 0x00056984 File Offset: 0x00054B84
	public float HoldTime
	{
		get
		{
			if (!base.IsOwner)
			{
				return ZPMath.ByteToFloat(this.holdTime.Value, 0f, 1f);
			}
			if (this.minigameController.State != MinigameControllerState.Playing)
			{
				return 0f;
			}
			if (!this.IsCharging())
			{
				return 0f;
			}
			return Mathf.Clamp01((Time.time - this.holdStartTime) / this.maxHoldTime);
		}
	}

	// Token: 0x060009B2 RID: 2482 RVA: 0x0000A6D5 File Offset: 0x000088D5
	private bool IsCharging()
	{
		if (!base.GamePlayer.IsAI)
		{
			return base.GamePlayer.RewiredPlayer.GetButton(InputActions.UseItemShoot);
		}
		return this.aiCharging;
	}

	// Token: 0x060009B3 RID: 2483 RVA: 0x000569F0 File Offset: 0x00054BF0
	public override void OnNetInitialize()
	{
		base.OnNetInitialize();
		if (NetSystem.IsServer)
		{
			this.posX.Value = ZPMath.CompressFloatToShort(base.transform.position.x, BilliardBattleController.minX, BilliardBattleController.maxX);
			this.posY.Value = ZPMath.CompressFloatToByte(base.transform.position.y, BilliardBattleController.minY, BilliardBattleController.maxNetY);
			this.posZ.Value = ZPMath.CompressFloatToShort(base.transform.position.z, BilliardBattleController.minZ, BilliardBattleController.maxZ);
			return;
		}
		UnityEngine.Object.Destroy(this.rb);
		UnityEngine.Object.Destroy(this.sphereCollider);
	}

	// Token: 0x060009B4 RID: 2484 RVA: 0x0000A700 File Offset: 0x00008900
	public override void FinishedSpawning()
	{
		base.FinishedSpawning();
		this.holdStartTime = Time.time;
	}

	// Token: 0x060009B5 RID: 2485 RVA: 0x00056AA0 File Offset: 0x00054CA0
	protected override void Start()
	{
		base.Start();
		this.minigameController = (BilliardBattleController)GameManager.Minigame;
		this.minigameController.AddPlayer(this);
		this.minigameController.Root.GetComponentInChildren<CameraFollow>().AddTarget(this);
		Material material = new Material(this.playerMaterial);
		material.SetColor("_ReplaceColor", base.GamePlayer.Color.skinColor1);
		this.mr.sharedMaterial = material;
		Color skinColor = base.GamePlayer.Color.skinColor1;
		skinColor.a = 0.5f;
		this.projectionRenderer.SetColor(0, skinColor);
		this.projectionRenderer.UpdateProperties();
	}

	// Token: 0x060009B6 RID: 2486 RVA: 0x00056B50 File Offset: 0x00054D50
	private void Update()
	{
		Vector3 forward = Vector3.zero;
		if (base.IsOwner)
		{
			forward = this.DoInput();
		}
		if (NetSystem.IsServer)
		{
			this.posX.Value = ZPMath.CompressFloatToShort(base.transform.position.x, BilliardBattleController.minX, BilliardBattleController.maxX);
			this.posY.Value = ZPMath.CompressFloatToByte(base.transform.position.y, BilliardBattleController.minY, BilliardBattleController.maxNetY);
			this.posZ.Value = ZPMath.CompressFloatToShort(base.transform.position.z, BilliardBattleController.minZ, BilliardBattleController.maxZ);
			this.highestVelocity = Mathf.Max(this.highestVelocity, this.rb.velocity.magnitude);
			if (this.rb.velocity.magnitude > this.maxVelocity)
			{
				this.rb.velocity = this.rb.velocity.normalized * this.maxVelocity;
			}
		}
		else
		{
			Vector3 position = base.transform.position;
			base.transform.position = new Vector3(ZPMath.DecompressShortToFloat(this.posX.Value, BilliardBattleController.minX, BilliardBattleController.maxX), ZPMath.DecompressByteToFloat(this.posY.Value, BilliardBattleController.minY, BilliardBattleController.maxNetY), ZPMath.DecompressShortToFloat(this.posZ.Value, BilliardBattleController.minZ, BilliardBattleController.maxZ));
			if (position != base.transform.position)
			{
				this.DoRotation(position);
			}
		}
		if (base.IsOwner)
		{
			this.holdTime.Value = ZPMath.CompressFloatToByte(this.HoldTime, 0f, 1f);
			this.directionArrowRoot.rotation = Quaternion.LookRotation(forward);
			this.direction.Value = ZPMath.CompressFloatToByte(this.directionArrowRoot.rotation.eulerAngles.y, 0f, 360f);
			return;
		}
		this.directionArrowRoot.rotation = Quaternion.Euler(0f, ZPMath.DecompressByteToFloat(this.direction.Value, 0f, 360f), 0f);
	}

	// Token: 0x060009B7 RID: 2487 RVA: 0x00056D90 File Offset: 0x00054F90
	private void FixedUpdate()
	{
		if (NetSystem.IsServer)
		{
			base.transform.position = new Vector3(Mathf.Clamp(base.transform.position.x, BilliardBattleController.minX, BilliardBattleController.maxX), Mathf.Clamp(base.transform.position.y, BilliardBattleController.minY, BilliardBattleController.maxY), Mathf.Clamp(base.transform.position.z, BilliardBattleController.minZ, BilliardBattleController.maxZ));
		}
	}

	// Token: 0x060009B8 RID: 2488 RVA: 0x00056E14 File Offset: 0x00055014
	private Vector3 DoInput()
	{
		Vector3 vector = Vector3.zero;
		if (!this.player.IsAI)
		{
			if (!GameManager.IsGamePaused)
			{
				Controller lastActiveController = this.player.RewiredPlayer.controllers.GetLastActiveController();
				if (lastActiveController != null && lastActiveController.type == ControllerType.Joystick)
				{
					vector = new Vector3(this.player.RewiredPlayer.GetAxis(InputActions.Horizontal), 0f, this.player.RewiredPlayer.GetAxis(InputActions.Vertical)).normalized;
				}
				else
				{
					Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
					float d = (0f - ray.origin.y) * (1f / ray.direction.y);
					vector = (ray.origin + ray.direction * d - base.transform.position).normalized;
				}
				if (this.minigameController.State == MinigameControllerState.Playing)
				{
					if (base.GamePlayer.RewiredPlayer.GetButtonDown(InputActions.UseItemShoot))
					{
						this.holdStartTime = Time.time;
					}
					if (base.GamePlayer.RewiredPlayer.GetButtonUp(InputActions.UseItemShoot))
					{
						this.DoHit(vector * Mathf.Clamp01((Time.time - this.holdStartTime) / this.maxHoldTime) * this.hitStrength);
					}
				}
			}
		}
		else if (this.minigameController.State == MinigameControllerState.Playing)
		{
			if (this.aiState == BilliardBattlePlayer.BilliardAIState.Targeting)
			{
				float num = float.MinValue;
				for (int i = 0; i < this.minigameController.balls.Count; i++)
				{
					if (!this.minigameController.balls[i].Pocketed)
					{
						Vector3 normalized = (this.minigameController.balls[i].transform.position - base.transform.position).normalized;
						for (int j = 0; j < this.minigameController.aiTargetPoints.Length; j++)
						{
							Vector3 normalized2 = (this.minigameController.aiTargetPoints[j].position - this.minigameController.balls[i].transform.position).normalized;
							float num2 = Vector3.Dot(normalized, normalized2);
							if (num2 > num)
							{
								num = num2;
								this.target = this.minigameController.balls[i];
							}
						}
					}
				}
				if (num == -3.4028235E+38f)
				{
					this.aiState = BilliardBattlePlayer.BilliardAIState.Finished;
				}
				else
				{
					this.aiState = BilliardBattlePlayer.BilliardAIState.Charging;
					this.aiCharging = true;
					this.holdStartTime = Time.time;
				}
			}
			else if (this.aiState == BilliardBattlePlayer.BilliardAIState.Charging)
			{
				vector = (this.target.transform.position - base.transform.position).normalized;
				if (this.HoldTime > 0.99f)
				{
					this.DoHit(vector * 1f * this.difficultyHitStrengths[(int)base.GamePlayer.Difficulty]);
					this.aiState = BilliardBattlePlayer.BilliardAIState.Waiting;
					this.aiCharging = false;
					this.hitTimer.Start();
				}
			}
			else if (this.aiState == BilliardBattlePlayer.BilliardAIState.Waiting && (this.rb == null || this.rb.velocity.sqrMagnitude < 2f) && this.hitTimer.Elapsed(false))
			{
				this.aiState = BilliardBattlePlayer.BilliardAIState.Targeting;
			}
		}
		return vector;
	}

	// Token: 0x060009B9 RID: 2489 RVA: 0x000571B0 File Offset: 0x000553B0
	private void DoHit(Vector3 dir)
	{
		if (NetSystem.IsServer)
		{
			this.rb.AddForce(dir, this.forceMode);
		}
		else if (base.IsOwner)
		{
			base.SendRPC("RPCDoHit", NetRPCDelivery.RELIABLE_UNORDERED, new object[]
			{
				dir
			});
		}
		AudioSystem.PlayOneShot(this.hitSound, 1f, 0.05f, 1f);
	}

	// Token: 0x060009BA RID: 2490 RVA: 0x00056238 File Offset: 0x00054438
	private void DoRotation(Vector3 prePosition)
	{
		Vector3 vector = base.transform.position - prePosition;
		vector.y = 0f;
		Vector3 axis = Vector3.Cross(vector.normalized, Vector3.up);
		float magnitude = vector.magnitude;
		float num = 7.0371675f;
		base.transform.RotateAround(base.transform.position, axis, -(magnitude / num * 360f));
	}

	// Token: 0x060009BB RID: 2491 RVA: 0x00057218 File Offset: 0x00055418
	private void OnCollisionEnter(Collision collision)
	{
		if (NetSystem.IsServer && collision.gameObject.CompareTag("BilliardBall"))
		{
			float magnitude = collision.relativeVelocity.magnitude;
			if (magnitude > 1f)
			{
				float num = Mathf.Lerp(0f, 1f, Mathf.Clamp(magnitude, 0f, 5f) / 5f);
				AudioSystem.PlayOneShot(this.hitSound, num, 0.05f, 1f);
				if (Time.time - this.lastTime > this.minNetSoundTime)
				{
					base.SendRPC("RPCPlayHitSound", NetRPCDelivery.UNRELIABLE, new object[]
					{
						num
					});
					this.lastTime = Time.time;
				}
			}
		}
	}

	// Token: 0x060009BC RID: 2492 RVA: 0x000572D4 File Offset: 0x000554D4
	private void OnTriggerEnter(Collider other)
	{
		if (NetSystem.IsServer)
		{
			base.transform.position = this.minigameController.GetFreePosition();
			base.transform.rotation = Quaternion.identity;
			this.rb.velocity = Vector3.zero;
			this.rb.angularVelocity = Vector3.zero;
		}
	}

	// Token: 0x060009BD RID: 2493 RVA: 0x0000A713 File Offset: 0x00008913
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCDoHit(NetPlayer sender, Vector3 dir)
	{
		this.DoHit(dir);
	}

	// Token: 0x060009BE RID: 2494 RVA: 0x0000A71C File Offset: 0x0000891C
	[NetRPC(false, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void RPCPlayHitSound(NetPlayer sender, float volume)
	{
		AudioSystem.PlayOneShot(this.hitSound, volume, 0.05f, 1f);
	}

	// Token: 0x0400086A RID: 2154
	public Rigidbody rb;

	// Token: 0x0400086B RID: 2155
	public SphereCollider sphereCollider;

	// Token: 0x0400086C RID: 2156
	public float hitStrength = 800f;

	// Token: 0x0400086D RID: 2157
	public ForceMode forceMode;

	// Token: 0x0400086E RID: 2158
	public AudioClip hitSound;

	// Token: 0x0400086F RID: 2159
	public float maxHoldTime = 2f;

	// Token: 0x04000870 RID: 2160
	public Material playerMaterial;

	// Token: 0x04000871 RID: 2161
	public MeshRenderer mr;

	// Token: 0x04000872 RID: 2162
	public Transform directionArrowRoot;

	// Token: 0x04000873 RID: 2163
	public float maxVelocity = 100000f;

	// Token: 0x04000874 RID: 2164
	public float highestVelocity = float.MinValue;

	// Token: 0x04000875 RID: 2165
	public ProjectionRenderer projectionRenderer;

	// Token: 0x04000876 RID: 2166
	private BilliardBattleController minigameController;

	// Token: 0x04000877 RID: 2167
	private float lastSoundTime;

	// Token: 0x04000878 RID: 2168
	private float minSoundInterval = 0.1f;

	// Token: 0x04000879 RID: 2169
	private bool gotPosition;

	// Token: 0x0400087A RID: 2170
	private float holdStartTime;

	// Token: 0x0400087B RID: 2171
	private BilliardBall target;

	// Token: 0x0400087C RID: 2172
	private bool aiCharging;

	// Token: 0x0400087D RID: 2173
	private BilliardBattlePlayer.BilliardAIState aiState = BilliardBattlePlayer.BilliardAIState.Targeting;

	// Token: 0x0400087E RID: 2174
	private ActionTimer hitTimer = new ActionTimer(1.5f, 3f);

	// Token: 0x0400087F RID: 2175
	private float[] difficultyHitStrengths = new float[]
	{
		3000f,
		3500f,
		4000f
	};

	// Token: 0x04000880 RID: 2176
	[NetSend(-1, NetSendOwner.SERVER, NetSendFlags.CHANGES_OFTEN)]
	public NetVar<short> posX = new NetVar<short>(0);

	// Token: 0x04000881 RID: 2177
	[NetSend(-1, NetSendOwner.SERVER, NetSendFlags.CHANGES_OFTEN)]
	public NetVar<byte> posY = new NetVar<byte>(0);

	// Token: 0x04000882 RID: 2178
	[NetSend(-1, NetSendOwner.SERVER, NetSendFlags.CHANGES_OFTEN)]
	public NetVar<short> posZ = new NetVar<short>(0);

	// Token: 0x04000883 RID: 2179
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.CHANGES_OFTEN)]
	public NetVar<byte> holdTime = new NetVar<byte>(0);

	// Token: 0x04000884 RID: 2180
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.CHANGES_OFTEN)]
	public NetVar<byte> direction = new NetVar<byte>(0);

	// Token: 0x04000885 RID: 2181
	private float minNetSoundTime = 0.05f;

	// Token: 0x04000886 RID: 2182
	private float lastTime;

	// Token: 0x02000153 RID: 339
	private enum BilliardAIState
	{
		// Token: 0x04000888 RID: 2184
		Waiting,
		// Token: 0x04000889 RID: 2185
		Targeting,
		// Token: 0x0400088A RID: 2186
		Charging,
		// Token: 0x0400088B RID: 2187
		Finished
	}
}
