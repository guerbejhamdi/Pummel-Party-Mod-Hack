using System;
using UnityEngine;
using UnityEngine.AI;

// Token: 0x020003CF RID: 975
public class CharacterMover : MonoBehaviour
{
	// Token: 0x170002D3 RID: 723
	// (get) Token: 0x06001A20 RID: 6688 RVA: 0x00013509 File Offset: 0x00011709
	// (set) Token: 0x06001A21 RID: 6689 RVA: 0x00013511 File Offset: 0x00011711
	public bool IsAI { get; set; }

	// Token: 0x06001A22 RID: 6690 RVA: 0x0001351A File Offset: 0x0001171A
	private void Start()
	{
		this.controller = base.GetComponent<CharacterController>();
		this.navMeshAgent = base.GetComponent<NavMeshAgent>();
	}

	// Token: 0x06001A23 RID: 6691 RVA: 0x0000398C File Offset: 0x00001B8C
	private void Update()
	{
	}

	// Token: 0x06001A24 RID: 6692 RVA: 0x000ADA2C File Offset: 0x000ABC2C
	public void CalculateVelocity(CharacterMoverInput input, float deltaTime)
	{
		if (this.useGravity)
		{
			this.velocity.y = this.velocity.y - this.gravity * deltaTime;
		}
		this.controlMovement = Vector3.zero;
		this.movementAxis = Vector2.zero;
		if (input.joystick)
		{
			this.controlMovement += this.strafe * input.axis.x;
			this.controlMovement += this.forward * input.axis.y;
			float d = Mathf.Clamp(this.controlMovement.magnitude, -1f, 1f);
			this.controlMovement = this.controlMovement.normalized * d;
		}
		else
		{
			if (input.forward)
			{
				this.controlMovement += this.forward;
				this.movementAxis += Vector2.up;
			}
			if (input.back)
			{
				this.controlMovement -= this.forward;
				this.movementAxis -= Vector2.up;
			}
			if (input.left)
			{
				this.controlMovement -= this.strafe;
				this.movementAxis += Vector2.left;
			}
			if (input.right)
			{
				this.controlMovement += this.strafe;
				this.movementAxis -= Vector2.left;
			}
			this.controlMovement.Normalize();
		}
		this.movementAxis = new Vector2(this.controlMovement.x, this.controlMovement.z);
		Vector3 vector = this.controlMovement * ((!this.allowFastMove) ? this.maxSpeed : (input.fastMove ? this.fastMoveMaxSpeed : this.maxSpeed));
		float num;
		float num2;
		if (this.grounded)
		{
			num = ((vector.x != 0f) ? this.acceleration : this.deceleration);
			num2 = ((vector.z != 0f) ? this.acceleration : this.deceleration);
		}
		else
		{
			num = ((vector.x != 0f) ? this.airAcceleration : this.airDeceleration);
			num2 = ((vector.z != 0f) ? this.airAcceleration : this.airDeceleration);
		}
		Vector3 normalized = (vector - new Vector3(this.velocity.x, 0f, this.velocity.z)).normalized;
		float f = vector.x - this.velocity.x;
		float f2 = vector.z - this.velocity.z;
		float num3 = normalized.x * num * deltaTime;
		float num4 = normalized.z * num2 * deltaTime;
		this.velocity.x = ((Mathf.Abs(f) > Mathf.Abs(num3)) ? (this.velocity.x + num3) : vector.x);
		this.velocity.z = ((Mathf.Abs(f2) > Mathf.Abs(num4)) ? (this.velocity.z + num4) : vector.z);
		if (this.maxJumps > 0 && input.jump && (this.grounded || (this.jumpCount == 0 && Time.time - this.lastGroundedTime < this.jumpLeewayTime) || (this.jumpCount < this.maxJumps && this.jumpCount > 0)))
		{
			this.jumpCount++;
			this.velocity.y = this.jumpVelocity;
			if (this.OnJump != null)
			{
				this.OnJump();
			}
			this.jumpSound != null;
		}
	}

	// Token: 0x06001A25 RID: 6693 RVA: 0x000ADE14 File Offset: 0x000AC014
	public void DoMovement(float deltaTime)
	{
		Vector3 vector = this.velocity * deltaTime;
		if (!this.IsAI)
		{
			if ((this.controller.Move(vector) & CollisionFlags.Above) != CollisionFlags.None && this.velocity.y > 0f)
			{
				this.velocity.y = 0f;
			}
			this.grounded = this.controller.isGrounded;
		}
		else
		{
			if (this.navMeshAgent.isActiveAndEnabled && this.navMeshAgent.isOnNavMesh)
			{
				this.navMeshAgent.Move(vector);
			}
			this.grounded = this.AIGrounded();
		}
		this.CheckGrounded();
	}

	// Token: 0x06001A26 RID: 6694 RVA: 0x000ADEB4 File Offset: 0x000AC0B4
	public void SmoothSlope()
	{
		if (this.jumpCount == 0 && this.wasGrounded)
		{
			Vector3 zero = Vector3.zero;
			float num = (this.IsAI ? this.navMeshAgent.height : this.controller.height) * this.playerScale / 2f;
			RaycastHit raycastHit = default(RaycastHit);
			if (Physics.Raycast(base.transform.position, -Vector3.up, out raycastHit, 3.4028235E+38f, this.slopeAdjustLayerMask))
			{
				float num2 = raycastHit.distance - num;
				if (num2 < this.slodeAdjustMax && num2 > this.slopeAdjustMin)
				{
					zero.y = raycastHit.distance - num + 0.05f;
					if (!this.IsAI)
					{
						this.controller.Move(-zero);
					}
					else
					{
						this.navMeshAgent.Move(-zero);
					}
				}
			}
		}
		if (!this.IsAI)
		{
			this.wasGrounded = this.controller.isGrounded;
			this.grounded = this.controller.isGrounded;
		}
		else
		{
			this.wasGrounded = this.AIGrounded();
			this.grounded = this.wasGrounded;
		}
		this.CheckGrounded();
	}

	// Token: 0x06001A27 RID: 6695 RVA: 0x00005651 File Offset: 0x00003851
	private bool AIGrounded()
	{
		return true;
	}

	// Token: 0x06001A28 RID: 6696 RVA: 0x00013534 File Offset: 0x00011734
	private void CheckGrounded()
	{
		if (this.grounded)
		{
			this.jumpCount = 0;
			this.velocity.y = 0f;
			this.lastGroundedTime = Time.time;
		}
	}

	// Token: 0x06001A29 RID: 6697 RVA: 0x000ADFEC File Offset: 0x000AC1EC
	public void SetForwardVector(Vector3 forward)
	{
		this.forward = forward;
		this.strafe = Vector3.Cross(Vector3.up, this.forward).normalized;
	}

	// Token: 0x170002D4 RID: 724
	// (get) Token: 0x06001A2A RID: 6698 RVA: 0x00013560 File Offset: 0x00011760
	public Vector2 MovementAxis
	{
		get
		{
			return this.movementAxis;
		}
	}

	// Token: 0x170002D5 RID: 725
	// (get) Token: 0x06001A2B RID: 6699 RVA: 0x00013568 File Offset: 0x00011768
	// (set) Token: 0x06001A2C RID: 6700 RVA: 0x00013570 File Offset: 0x00011770
	public Vector3 Velocity
	{
		get
		{
			return this.velocity;
		}
		set
		{
			this.velocity = value;
		}
	}

	// Token: 0x170002D6 RID: 726
	// (get) Token: 0x06001A2D RID: 6701 RVA: 0x00013579 File Offset: 0x00011779
	// (set) Token: 0x06001A2E RID: 6702 RVA: 0x00013581 File Offset: 0x00011781
	public bool Grounded
	{
		get
		{
			return this.grounded;
		}
		set
		{
			this.grounded = value;
		}
	}

	// Token: 0x04001BD4 RID: 7124
	[Header("Movement")]
	public float maxSpeed = 5f;

	// Token: 0x04001BD5 RID: 7125
	public float acceleration = 15f;

	// Token: 0x04001BD6 RID: 7126
	public float deceleration = 15f;

	// Token: 0x04001BD7 RID: 7127
	public float airAcceleration = 1f;

	// Token: 0x04001BD8 RID: 7128
	public float airDeceleration = 15f;

	// Token: 0x04001BD9 RID: 7129
	[Header("Gravity")]
	public bool useGravity = true;

	// Token: 0x04001BDA RID: 7130
	public float gravity = 15f;

	// Token: 0x04001BDB RID: 7131
	[Header("Jumping")]
	public int maxJumps = 1;

	// Token: 0x04001BDC RID: 7132
	public float jumpLeewayTime = 0.3f;

	// Token: 0x04001BDD RID: 7133
	public float jumpVelocity = 10f;

	// Token: 0x04001BDE RID: 7134
	public float jumpVolume = 0.15f;

	// Token: 0x04001BDF RID: 7135
	public AudioClip jumpSound;

	// Token: 0x04001BE0 RID: 7136
	[Header("Fast Move")]
	public bool allowFastMove;

	// Token: 0x04001BE1 RID: 7137
	public float fastMoveMaxSpeed = 15f;

	// Token: 0x04001BE2 RID: 7138
	[Header("Slope")]
	public float slodeAdjustMax = 0.3f;

	// Token: 0x04001BE3 RID: 7139
	public float slopeAdjustMin;

	// Token: 0x04001BE4 RID: 7140
	public LayerMask slopeAdjustLayerMask;

	// Token: 0x04001BE5 RID: 7141
	[Header("Other")]
	public float playerScale = 1f;

	// Token: 0x04001BE6 RID: 7142
	private CharacterController controller;

	// Token: 0x04001BE7 RID: 7143
	private NavMeshAgent navMeshAgent;

	// Token: 0x04001BE8 RID: 7144
	private Vector3 velocity = Vector3.zero;

	// Token: 0x04001BE9 RID: 7145
	private Vector3 forward = Vector3.forward;

	// Token: 0x04001BEA RID: 7146
	private Vector3 strafe = Vector3.right;

	// Token: 0x04001BEB RID: 7147
	private Vector3 controlMovement;

	// Token: 0x04001BEC RID: 7148
	private Vector2 movementAxis = Vector2.zero;

	// Token: 0x04001BED RID: 7149
	private float lastGroundedTime;

	// Token: 0x04001BEE RID: 7150
	private bool wasGrounded;

	// Token: 0x04001BEF RID: 7151
	private bool grounded;

	// Token: 0x04001BF0 RID: 7152
	private int jumpCount;

	// Token: 0x04001BF1 RID: 7153
	public CharacterMover.OnJumpDelegate OnJump;

	// Token: 0x020003D0 RID: 976
	// (Invoke) Token: 0x06001A31 RID: 6705
	public delegate void OnJumpDelegate();
}
