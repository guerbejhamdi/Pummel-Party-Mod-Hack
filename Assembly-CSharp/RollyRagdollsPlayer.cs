using System;
using UnityEngine;
using ZP.Net;
using ZP.Utility;

// Token: 0x02000227 RID: 551
public class RollyRagdollsPlayer : CharacterBase
{
	// Token: 0x17000169 RID: 361
	// (get) Token: 0x06001000 RID: 4096 RVA: 0x0000DA33 File Offset: 0x0000BC33
	// (set) Token: 0x06001001 RID: 4097 RVA: 0x0000DA3B File Offset: 0x0000BC3B
	public float FurthestX { get; private set; }

	// Token: 0x1700016A RID: 362
	// (get) Token: 0x06001002 RID: 4098 RVA: 0x0000DA44 File Offset: 0x0000BC44
	// (set) Token: 0x06001003 RID: 4099 RVA: 0x0000DA4C File Offset: 0x0000BC4C
	public bool Finished { get; set; }

	// Token: 0x06001004 RID: 4100 RVA: 0x0007E3B4 File Offset: 0x0007C5B4
	public override void OnNetInitialize()
	{
		base.OnNetInitialize();
		if (NetSystem.IsServer)
		{
			this.SetNetVars();
		}
		else
		{
			this.rb.isKinematic = true;
			UnityEngine.Object.Destroy(this.sphereCollider);
		}
		this.startPos = base.transform.position;
		this.startRot = base.transform.rotation;
	}

	// Token: 0x06001005 RID: 4101 RVA: 0x0000DA55 File Offset: 0x0000BC55
	private void Fix()
	{
		base.GetComponentsInChildren<Rigidbody>(true);
	}

	// Token: 0x06001006 RID: 4102 RVA: 0x0007E410 File Offset: 0x0007C610
	protected override void Start()
	{
		base.Start();
		this.minigameController = (RollyRagdollsController)GameManager.Minigame;
		this.minigameController.AddPlayer(this);
		this.minigameController.Root.GetComponentInChildren<CameraFollow>().AddTarget(this);
		if (NetSystem.IsServer)
		{
			this.rb.maxAngularVelocity = this.maxAngularVelocity;
		}
		CharacterJoint[] componentsInChildren = base.GetComponentsInChildren<CharacterJoint>(true);
		Debug.Log("Character Joints: " + componentsInChildren.Length.ToString());
		foreach (CharacterJoint characterJoint in componentsInChildren)
		{
			SoftJointLimitSpring softJointLimitSpring = characterJoint.twistLimitSpring;
			softJointLimitSpring.spring = 500.05f;
			softJointLimitSpring.damper = 50.8f;
			characterJoint.twistLimitSpring = softJointLimitSpring;
			softJointLimitSpring = characterJoint.swingLimitSpring;
			softJointLimitSpring.spring = 500.05f;
			softJointLimitSpring.damper = 50.8f;
			characterJoint.swingLimitSpring = softJointLimitSpring;
		}
		this.rigidBodies = base.GetComponentsInChildren<Rigidbody>(true);
		this.startPositions = new Vector3[this.rigidBodies.Length];
		this.startRotations = new Quaternion[this.rigidBodies.Length];
		for (int j = 0; j < this.rigidBodies.Length; j++)
		{
			if (this.rigidBodies[j] != this.rb)
			{
				this.rigidBodies[j].maxDepenetrationVelocity = 2f;
				this.rigidBodies[j].maxAngularVelocity = 2f;
			}
			this.startPositions[j] = this.rigidBodies[j].transform.localPosition;
			this.startRotations[j] = this.rigidBodies[j].transform.localRotation;
		}
	}

	// Token: 0x06001007 RID: 4103 RVA: 0x0007E5B8 File Offset: 0x0007C7B8
	private void Update()
	{
		if (this.minigameController.State == MinigameControllerState.Playing && base.IsOwner)
		{
			this.GetInput();
		}
		if (NetSystem.IsServer)
		{
			this.SetNetVars();
			if (!base.IsDead)
			{
				if (this.minigameController.spikeTransform != null && this.minigameController.MinigameCamera != null && base.transform.position.x < this.minigameController.spikeTransform.position.x + 0.6f)
				{
					this.Kill();
				}
				if (!this.Finished)
				{
					this.FurthestX = base.transform.position.x;
					float num = this.minigameController.fillerOffset.x * (float)RollyRagdollsController.obstacleRows;
					this.Score = (short)(Mathf.Clamp(this.FurthestX + 4.5f, 0f, this.minigameController.fillerOffset.x * (float)RollyRagdollsController.obstacleRows) * 10f);
					if (this.FurthestX >= num + 2.5f)
					{
						short num2 = 0;
						for (int i = 0; i < this.minigameController.GetPlayerCount(); i++)
						{
							CharacterBase playerInSlot = this.minigameController.GetPlayerInSlot((short)i);
							if (playerInSlot != this && ((RollyRagdollsPlayer)playerInSlot).Finished)
							{
								num2 += 1;
							}
						}
						this.Score += (8 - num2) * 10;
						this.Finish();
					}
				}
				for (int j = 0; j < this.rigidBodies.Length; j++)
				{
					if (!(this.rigidBodies[j] == this.rb) && this.rigidBodies[j].transform.parent != null && (this.rigidBodies[j].transform.position - this.rigidBodies[j].transform.parent.position).sqrMagnitude > 25f)
					{
						this.FixRagdoll();
					}
				}
			}
		}
	}

	// Token: 0x06001008 RID: 4104 RVA: 0x0007E7C4 File Offset: 0x0007C9C4
	private void BreakRagdoll()
	{
		for (int i = 0; i < this.rigidBodies.Length; i++)
		{
			if (!(this.rigidBodies[i] == this.rb))
			{
				this.rigidBodies[i].AddForce(ZPMath.RandomVec3(GameManager.rand, -50000f, 50000f));
			}
		}
	}

	// Token: 0x06001009 RID: 4105 RVA: 0x0007E81C File Offset: 0x0007CA1C
	private void FixRagdoll()
	{
		for (int i = 0; i < this.rigidBodies.Length; i++)
		{
			if (!(this.rigidBodies[i] == this.rb))
			{
				this.rigidBodies[i].velocity = Vector3.zero;
				this.rigidBodies[i].angularVelocity = Vector3.zero;
				this.rigidBodies[i].transform.localPosition = this.startPositions[i];
				this.rigidBodies[i].transform.localRotation = this.startRotations[i];
			}
		}
		this.rb.transform.position = new Vector3(this.rb.transform.position.x, 0.8f, this.rb.transform.position.z);
	}

	// Token: 0x0600100A RID: 4106 RVA: 0x0000DA5F File Offset: 0x0000BC5F
	private void FixedUpdate()
	{
		if (this.minigameController.State == MinigameControllerState.Playing && !base.IsDead)
		{
			this.DoInput();
		}
		if (!NetSystem.IsServer)
		{
			this.GetNetVars();
		}
	}

	// Token: 0x0600100B RID: 4107 RVA: 0x0007E8F8 File Offset: 0x0007CAF8
	private void GetNetVars()
	{
		this.rb.MovePosition(new Vector3(ZPMath.DecompressShortToFloat(this.posX.Value, RollyRagdollsController.minX, RollyRagdollsController.maxX), ZPMath.DecompressShortToFloat(this.posY.Value, RollyRagdollsController.minY, RollyRagdollsController.maxY), ZPMath.DecompressShortToFloat(this.posZ.Value, RollyRagdollsController.minZ, RollyRagdollsController.maxZ)));
		this.rb.MoveRotation(Quaternion.Euler(ZPMath.DecompressShortToFloat(this.rotX.Value, 0f, 360f), ZPMath.DecompressShortToFloat(this.rotY.Value, 0f, 360f), ZPMath.DecompressShortToFloat(this.rotZ.Value, 0f, 360f)));
	}

	// Token: 0x0600100C RID: 4108 RVA: 0x0007E9C4 File Offset: 0x0007CBC4
	private void SetNetVars()
	{
		this.posX.Value = ZPMath.CompressFloatToShort(base.transform.position.x, RollyRagdollsController.minX, RollyRagdollsController.maxX);
		this.posY.Value = ZPMath.CompressFloatToShort(base.transform.position.y, RollyRagdollsController.minY, RollyRagdollsController.maxY);
		this.posZ.Value = ZPMath.CompressFloatToShort(base.transform.position.z, RollyRagdollsController.minZ, RollyRagdollsController.maxZ);
		Vector3 eulerAngles = base.transform.rotation.eulerAngles;
		this.rotX.Value = ZPMath.CompressFloatToShort(eulerAngles.x, 0f, 360f);
		this.rotY.Value = ZPMath.CompressFloatToShort(eulerAngles.y, 0f, 360f);
		this.rotZ.Value = ZPMath.CompressFloatToShort(eulerAngles.z, 0f, 360f);
	}

	// Token: 0x0600100D RID: 4109 RVA: 0x0007EAC4 File Offset: 0x0007CCC4
	private void GetInput()
	{
		Vector3 zero = Vector3.zero;
		if (!base.GamePlayer.IsAI)
		{
			if (!GameManager.IsGamePaused)
			{
				zero = new Vector3(base.GamePlayer.RewiredPlayer.GetAxis(InputActions.Horizontal), 0f, base.GamePlayer.RewiredPlayer.GetAxis(InputActions.Vertical));
			}
		}
		else
		{
			if (this.positionCheckTimer.Elapsed(true))
			{
				this.stuck = (base.transform.position.x - this.lastPosition.x < 0.5f);
				this.lastPosition = Vector3.zero;
			}
			if (this.timer.Elapsed(true))
			{
				this.aiDir = new Vector3(this.botDifficultySpeeds[(int)base.GamePlayer.Difficulty], 0f, this.stuck ? UnityEngine.Random.Range(-1f, 1f) : 0f);
				this.aiDir.Normalize();
			}
			zero = this.aiDir;
		}
		Vector3 normalized = zero.normalized;
		Vector3 vector = new Vector3(normalized.x * Mathf.Abs(zero.x), 0f, normalized.z * Mathf.Abs(zero.z));
		this.inputX.Value = ZPMath.CompressFloatToByte(vector.x, -1f, 1f);
		this.inputZ.Value = ZPMath.CompressFloatToByte(vector.z, -1f, 1f);
	}

	// Token: 0x0600100E RID: 4110 RVA: 0x0007EC44 File Offset: 0x0007CE44
	private void DoInput()
	{
		if (NetSystem.IsServer)
		{
			Vector3 zero = new Vector3(ZPMath.DecompressByteToFloat(this.inputX.Value, -1f, 1f), 0f, ZPMath.DecompressByteToFloat(this.inputZ.Value, -1f, 1f));
			if (zero.magnitude < 0.1f)
			{
				zero = Vector3.zero;
			}
			if (zero != Vector3.zero)
			{
				this.rb.AddForce(zero * this.torqueAcceleration);
			}
		}
	}

	// Token: 0x0600100F RID: 4111 RVA: 0x0007ECD0 File Offset: 0x0007CED0
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.relativeVelocity.magnitude > 3.75f)
		{
			AudioSystem.PlayOneShot(this.m_Impact, 0.25f, 0.05f, 1f);
		}
		if (collision.gameObject.CompareTag("RollyLauncher"))
		{
			RollyRagdollsJumpPad component = collision.gameObject.GetComponent<RollyRagdollsJumpPad>();
			if (component != null && Time.time - this.lastLaunch > 0.5f)
			{
				this.rb.AddForce(component.strength);
				this.lastLaunch = Time.time;
			}
		}
	}

	// Token: 0x06001010 RID: 4112 RVA: 0x0000DA8A File Offset: 0x0000BC8A
	[NetRPC(true, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void RPCFinish(NetPlayer sender)
	{
		this.Finish();
	}

	// Token: 0x06001011 RID: 4113 RVA: 0x0007ED64 File Offset: 0x0007CF64
	private void Finish()
	{
		if (NetSystem.IsServer)
		{
			base.SendRPC("RPCFinish", NetRPCDelivery.RELIABLE_ORDERED, Array.Empty<object>());
		}
		this.Finished = true;
		RollyRagdollsController rollyRagdollsController = this.minigameController;
		int finishedCount = rollyRagdollsController.FinishedCount;
		rollyRagdollsController.FinishedCount = finishedCount + 1;
		AudioSystem.PlayOneShot(this.m_FinishSound, 1f, 0f, 1f);
	}

	// Token: 0x06001012 RID: 4114 RVA: 0x0000DA92 File Offset: 0x0000BC92
	[NetRPC(true, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void RPCKill(NetPlayer sender)
	{
		this.Kill();
	}

	// Token: 0x06001013 RID: 4115 RVA: 0x0007EDC0 File Offset: 0x0007CFC0
	public void Kill()
	{
		if (base.IsDead)
		{
			return;
		}
		if (NetSystem.IsServer)
		{
			RollyRagdollsController rollyRagdollsController = this.minigameController;
			int deadCount = rollyRagdollsController.DeadCount;
			rollyRagdollsController.DeadCount = deadCount + 1;
			this.rb.isKinematic = true;
			base.SendRPC("RPCKill", NetRPCDelivery.RELIABLE_ORDERED, Array.Empty<object>());
		}
		this.Deactivate();
		base.IsDead = true;
		UnityEngine.Object.Instantiate<GameObject>(this.playerDeathEffect, base.transform.position, Quaternion.LookRotation(Vector3.up));
		AudioSystem.PlayOneShot("DeathSplash01", 0.5f, 0.1f);
	}

	// Token: 0x04001042 RID: 4162
	public Rigidbody rb;

	// Token: 0x04001043 RID: 4163
	public SphereCollider sphereCollider;

	// Token: 0x04001044 RID: 4164
	public GameObject playerDeathEffect;

	// Token: 0x04001045 RID: 4165
	public AudioClip m_FinishSound;

	// Token: 0x04001046 RID: 4166
	public AudioClip m_Impact;

	// Token: 0x04001049 RID: 4169
	[Header("Movement")]
	public float maxAngularVelocity = 15f;

	// Token: 0x0400104A RID: 4170
	public float torqueAcceleration = 90f;

	// Token: 0x0400104B RID: 4171
	public float turnSpeed = 50f;

	// Token: 0x0400104C RID: 4172
	[NetSend(-1, NetSendOwner.SERVER, NetSendFlags.CHANGES_OFTEN)]
	public NetVar<short> posX = new NetVar<short>(0);

	// Token: 0x0400104D RID: 4173
	[NetSend(-1, NetSendOwner.SERVER, NetSendFlags.CHANGES_OFTEN)]
	public NetVar<short> posY = new NetVar<short>(0);

	// Token: 0x0400104E RID: 4174
	[NetSend(-1, NetSendOwner.SERVER, NetSendFlags.CHANGES_OFTEN)]
	public NetVar<short> posZ = new NetVar<short>(0);

	// Token: 0x0400104F RID: 4175
	[NetSend(-1, NetSendOwner.SERVER, NetSendFlags.CHANGES_OFTEN)]
	public NetVar<short> rotX = new NetVar<short>(0);

	// Token: 0x04001050 RID: 4176
	[NetSend(-1, NetSendOwner.SERVER, NetSendFlags.CHANGES_OFTEN)]
	public NetVar<short> rotY = new NetVar<short>(0);

	// Token: 0x04001051 RID: 4177
	[NetSend(-1, NetSendOwner.SERVER, NetSendFlags.CHANGES_OFTEN)]
	public NetVar<short> rotZ = new NetVar<short>(0);

	// Token: 0x04001052 RID: 4178
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.CHANGES_OFTEN)]
	public NetVar<byte> inputX = new NetVar<byte>(128);

	// Token: 0x04001053 RID: 4179
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.CHANGES_OFTEN)]
	public NetVar<byte> inputZ = new NetVar<byte>(128);

	// Token: 0x04001054 RID: 4180
	private RollyRagdollsController minigameController;

	// Token: 0x04001055 RID: 4181
	private Vector3 startPos;

	// Token: 0x04001056 RID: 4182
	private Quaternion startRot;

	// Token: 0x04001057 RID: 4183
	private Rigidbody[] rigidBodies;

	// Token: 0x04001058 RID: 4184
	private Vector3[] startPositions;

	// Token: 0x04001059 RID: 4185
	private Quaternion[] startRotations;

	// Token: 0x0400105A RID: 4186
	private ActionTimer timer = new ActionTimer(0.5f, 3f);

	// Token: 0x0400105B RID: 4187
	private ActionTimer positionCheckTimer = new ActionTimer(1f);

	// Token: 0x0400105C RID: 4188
	private bool stuck = true;

	// Token: 0x0400105D RID: 4189
	private Vector3 aiDir = Vector3.zero;

	// Token: 0x0400105E RID: 4190
	private Vector3 lastPosition = Vector3.zero;

	// Token: 0x0400105F RID: 4191
	private float[] botDifficultySpeeds = new float[]
	{
		0.7f,
		0.8f,
		0.9f
	};

	// Token: 0x04001060 RID: 4192
	private float lastLaunch;
}
