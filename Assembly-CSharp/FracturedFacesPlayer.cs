using System;
using System.Collections.Generic;
using UnityEngine;
using ZP.Net;
using ZP.Utility;

// Token: 0x020001B0 RID: 432
public class FracturedFacesPlayer : Movement1
{
	// Token: 0x17000118 RID: 280
	// (get) Token: 0x06000C62 RID: 3170 RVA: 0x0000BC2A File Offset: 0x00009E2A
	// (set) Token: 0x06000C63 RID: 3171 RVA: 0x0000BC32 File Offset: 0x00009E32
	public bool Finished { get; set; }

	// Token: 0x06000C64 RID: 3172 RVA: 0x00067984 File Offset: 0x00065B84
	public override void OnNetInitialize()
	{
		base.OnNetInitialize();
		this.mover = base.GetComponent<CharacterMover>();
		CharacterMover characterMover = this.mover;
		characterMover.OnJump = (CharacterMover.OnJumpDelegate)Delegate.Combine(characterMover.OnJump, new CharacterMover.OnJumpDelegate(this.OnJump));
		if (base.IsOwner)
		{
			this.net_z_rotation.Value = ZPMath.CompressFloat(0f, -45f, 45f);
		}
		if (!base.IsOwner)
		{
			this.puzzlePiecePositions.ArrayRecieve = new ArrayRecieveProxy(this.RecievePositions);
			this.puzzlePieceRotations.ArrayRecieve = new ArrayRecieveProxy(this.RecieveRotations);
		}
		if (!base.IsOwner)
		{
			base.GetComponent<CharacterController>().enabled = false;
		}
	}

	// Token: 0x06000C65 RID: 3173 RVA: 0x0000BC3B File Offset: 0x00009E3B
	public void RecievePositions(int index, object _pos)
	{
		this.SetPiecePosition(index, (byte)_pos);
	}

	// Token: 0x06000C66 RID: 3174 RVA: 0x0000BC4A File Offset: 0x00009E4A
	public void RecieveRotations(int index, object _rot)
	{
		this.SetPieceRotation(index, (byte)_rot);
	}

	// Token: 0x06000C67 RID: 3175 RVA: 0x00067A3C File Offset: 0x00065C3C
	public void SetupPuzzlePieces(int seed)
	{
		System.Random random = new System.Random(seed);
		Material material = this.minigameController.puzzleMaterials[random.Next(0, this.minigameController.puzzleMaterials.Length)];
		Transform transform = this.minigameController.Root.transform.Find("PlayAreas/PlayArea" + ((GameManager.GetPlayerCount() > 4) ? ((int)(base.OwnerSlot + 1)) : this.playAreaMap[(int)base.OwnerSlot]).ToString());
		this.finalPuzzle = transform.Find("EndPuzzle").GetComponent<MeshRenderer>();
		this.finalPuzzle.material = material;
		float num = (this.minigameController.playAreaSize - this.minigameController.gap * (this.minigameController.gridSize + 1f)) / this.minigameController.gridSize;
		float num2 = -this.minigameController.playAreaSize / 2f + num / 2f + this.minigameController.gap;
		Vector3 localScale = new Vector3(num, 0.05f, num);
		Vector3 vector = transform.position + new Vector3(num2, 0.5f, num2);
		List<int> list = new List<int>();
		int num3 = 0;
		while ((float)num3 < this.minigameController.gridSize * this.minigameController.gridSize)
		{
			list.Add(num3);
			num3++;
		}
		float num4 = 1f / this.minigameController.gridSize;
		int num5 = 0;
		while ((float)num5 < this.minigameController.gridSize)
		{
			int num6 = 0;
			while ((float)num6 < this.minigameController.gridSize)
			{
				int num7 = random.Next(0, 4);
				int index = random.Next(0, list.Count);
				this.positions.Add(vector);
				FracturedFacesPuzzlePiece component = this.minigameController.Spawn(this.puzzlePrefab, vector, Quaternion.Euler(0f, this.rotations[num7], 0f)).GetComponent<FracturedFacesPuzzlePiece>();
				component.mr.material = material;
				component.id = (byte)list[index];
				int num8 = num5 * (int)this.minigameController.gridSize + num6;
				this.puzzlePiecePositions[num8] = (byte)num8;
				this.puzzlePieceRotations[num8] = (byte)num7;
				Mesh mesh = new Mesh();
				mesh.vertices = this.cube.vertices;
				mesh.triangles = this.cube.triangles;
				List<Vector2> list2 = new List<Vector2>(this.cube.uv);
				float num9 = (float)list[index] % this.minigameController.gridSize * num4;
				float num10 = (float)(list[index] / (int)this.minigameController.gridSize) * num4;
				list2[4] = new Vector2(num9 + num4, num10);
				list2[5] = new Vector2(num9, num10);
				list2[8] = new Vector2(num9 + num4, num10 + num4);
				list2[9] = new Vector2(num9, num10 + num4);
				mesh.uv = list2.ToArray();
				mesh.UploadMeshData(true);
				component.mf.mesh = mesh;
				this.puzzlePieces.Add(component);
				component.transform.localScale = localScale;
				list.RemoveAt(index);
				vector += new Vector3(num + this.minigameController.gap, 0f, 0f);
				num6++;
			}
			vector.x = transform.position.x + num2;
			vector += new Vector3(0f, 0f, num + this.minigameController.gap);
			num5++;
		}
	}

	// Token: 0x06000C68 RID: 3176 RVA: 0x0000AEDF File Offset: 0x000090DF
	public override void OnOwnerChanged()
	{
		if (NetSystem.IsServer)
		{
			base.GetComponent<CharacterController>().enabled = true;
		}
		base.OnOwnerChanged();
	}

	// Token: 0x06000C69 RID: 3177 RVA: 0x000047D3 File Offset: 0x000029D3
	public void Awake()
	{
		base.InitializeController();
	}

	// Token: 0x06000C6A RID: 3178 RVA: 0x00067E08 File Offset: 0x00066008
	protected override void Start()
	{
		base.Start();
		this.minigameController = (FracturedFacesController)GameManager.Minigame;
		this.minigameController.AddPlayer(this);
		this.minigameController.Root.GetComponentInChildren<CameraFollow>().AddTarget(this);
		this.cameraShake = this.minigameController.Root.GetComponentInChildren<CameraShake>();
		if (!this.player.IsAI)
		{
			this.mover.SetForwardVector(Vector3.left);
		}
	}

	// Token: 0x06000C6B RID: 3179 RVA: 0x00067E80 File Offset: 0x00066080
	private void Update()
	{
		base.UpdateController();
		if (this.minigameController.Playable)
		{
			if (base.IsOwner && !base.GamePlayer.IsAI && !this.Finished)
			{
				if (base.GamePlayer.RewiredPlayer.GetButtonDown(InputActions.Accept))
				{
					this.Pickup();
				}
				else if (base.GamePlayer.RewiredPlayer.GetButtonDown(InputActions.Action1))
				{
					this.Rotate();
				}
			}
			for (int i = 0; i < this.puzzlePieces.Count; i++)
			{
				if (this.puzzlePiecePositions[i] == 200)
				{
					this.puzzlePieces[i].transform.position = base.transform.position + Vector3.up * 1f;
					break;
				}
			}
			if (this.Score > 99 && !this.finalPuzzle.gameObject.activeSelf)
			{
				this.finalPuzzle.gameObject.SetActive(true);
			}
		}
	}

	// Token: 0x06000C6C RID: 3180 RVA: 0x00067F8C File Offset: 0x0006618C
	private void Rotate()
	{
		if (this.currentlyHeld == 200)
		{
			return;
		}
		byte b = this.puzzlePieceRotations[(int)this.currentlyHeld];
		byte rot = (b == 3) ? 0 : (b + 1);
		this.SetPieceRotation((int)this.currentlyHeld, rot);
	}

	// Token: 0x06000C6D RID: 3181 RVA: 0x00067FD4 File Offset: 0x000661D4
	private void Pickup()
	{
		byte b = 0;
		float num = float.MaxValue;
		for (int i = 0; i < this.positions.Count; i++)
		{
			float sqrMagnitude = (base.transform.position - this.positions[i]).sqrMagnitude;
			if (sqrMagnitude < num)
			{
				b = (byte)i;
				num = sqrMagnitude;
			}
		}
		byte b2 = 200;
		for (int j = 0; j < this.puzzlePieces.Count; j++)
		{
			if (this.puzzlePiecePositions[j] == b)
			{
				b2 = (byte)j;
				break;
			}
		}
		if (this.currentlyHeld != 200)
		{
			this.SetPiecePosition((int)this.currentlyHeld, b);
		}
		if (b2 != 200)
		{
			this.currentlyHeld = b2;
			this.puzzlePiecePositions[(int)b2] = 200;
			return;
		}
		this.currentlyHeld = 200;
	}

	// Token: 0x06000C6E RID: 3182 RVA: 0x000680B0 File Offset: 0x000662B0
	private void SetPiecePosition(int id, byte pos)
	{
		if (id < 0 || id >= this.puzzlePieces.Count)
		{
			return;
		}
		if (base.IsOwner)
		{
			this.puzzlePiecePositions[id] = pos;
		}
		if (pos != 200)
		{
			this.puzzlePieces[id].transform.position = this.positions[(int)pos];
		}
		AudioSystem.PlayOneShot(this.actionSound, 1f, 0f, 1f);
	}

	// Token: 0x06000C6F RID: 3183 RVA: 0x0006812C File Offset: 0x0006632C
	private void SetPieceRotation(int id, byte rot)
	{
		if (id < 0 || id >= this.puzzlePieces.Count)
		{
			return;
		}
		if (base.IsOwner)
		{
			this.puzzlePieceRotations[id] = rot;
		}
		this.puzzlePieces[id].transform.rotation = Quaternion.Euler(0f, this.rotations[(int)rot], 0f);
		AudioSystem.PlayOneShot(this.actionSound, 1f, 0f, 1f);
	}

	// Token: 0x06000C70 RID: 3184 RVA: 0x000681A8 File Offset: 0x000663A8
	protected override void DoMovement()
	{
		CharacterMoverInput input = default(CharacterMoverInput);
		bool val = !this.minigameController.Playable || (GameManager.IsGamePaused && !this.player.IsAI) || !GameManager.PollInput || this.isDead;
		if (!base.GamePlayer.IsAI)
		{
			Vector2 axis = new Vector2(this.player.RewiredPlayer.GetAxis(InputActions.Vertical), -this.player.RewiredPlayer.GetAxis(InputActions.Horizontal));
			input = new CharacterMoverInput(axis, false, false);
		}
		else
		{
			input = this.GetAIInput();
		}
		if (this.agent == null || !this.agent.isOnOffMeshLink)
		{
			input.NullInput(val);
			this.mover.CalculateVelocity(input, Time.deltaTime);
			this.mover.DoMovement(Time.deltaTime);
			this.mover.SmoothSlope();
			if (this.mover.MovementAxis != Vector2.zero)
			{
				base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, Quaternion.LookRotation(new Vector3(this.mover.MovementAxis.x, 0f, this.mover.MovementAxis.y), Vector3.up), 1500f * Time.deltaTime);
			}
		}
		this.velocity.Value = this.mover.Velocity;
		this.netIsGrounded.Value = (this.curOffMeshLinkTranslationType != OffMeshLinkTranslateType.Parabola && this.mover.Grounded);
		base.DoMovement();
	}

	// Token: 0x06000C71 RID: 3185 RVA: 0x00068344 File Offset: 0x00066544
	protected override void UpdateAnimationState(PlayerAnimation playerAnim)
	{
		Vector2 vector = new Vector2(this.velocity.Value.x, this.velocity.Value.z);
		float num = Mathf.Clamp01(vector.magnitude / this.mover.maxSpeed);
		playerAnim.Velocity = num;
		playerAnim.VelocityY = this.velocity.Value.y;
		playerAnim.MovementAxis = ((num > 0.01f) ? Vector2.up : Vector2.zero);
		playerAnim.Grounded = this.netIsGrounded.Value;
		playerAnim.SetPlayerRotation(this.rotation.Value);
	}

	// Token: 0x06000C72 RID: 3186 RVA: 0x000683EC File Offset: 0x000665EC
	private CharacterMoverInput GetAIInput()
	{
		float num = 0.040000003f;
		Vector2 b = new Vector2(base.transform.position.x, base.transform.position.z);
		bool flag = (new Vector2(this.targetPosition.x, this.targetPosition.z) - b).sqrMagnitude <= num;
		switch (this.AIState)
		{
		case FracturedFacesPlayer.FracturedFacesAIState.Waiting:
			if (this.minigameController.Playable)
			{
				this.AIState = FracturedFacesPlayer.FracturedFacesAIState.FindTarget;
			}
			break;
		case FracturedFacesPlayer.FracturedFacesAIState.FindTarget:
			if (this.currentlyHeld != 200)
			{
				this.AIState = FracturedFacesPlayer.FracturedFacesAIState.MovingToTarget;
			}
			else
			{
				List<int> list = new List<int>();
				for (int i = 0; i < this.puzzlePieces.Count; i++)
				{
					list.Add(i);
				}
				bool flag2 = false;
				while (list.Count > 0)
				{
					int index = GameManager.rand.Next(0, list.Count);
					int index2 = list[index];
					FracturedFacesPuzzlePiece fracturedFacesPuzzlePiece = this.puzzlePieces[index2];
					if (fracturedFacesPuzzlePiece.id != this.puzzlePiecePositions[index2] || this.puzzlePieceRotations[index2] != 0)
					{
						if (GameManager.rand.NextDouble() > 0.5)
						{
							this.targetPosition = fracturedFacesPuzzlePiece.transform.position;
						}
						else
						{
							this.targetPosition = this.positions[GameManager.rand.Next(0, this.positions.Count)];
						}
						flag2 = true;
						break;
					}
					list.RemoveAt(index);
				}
				if (flag2)
				{
					this.AIState = FracturedFacesPlayer.FracturedFacesAIState.MovingToTarget;
				}
				else
				{
					this.AIState = FracturedFacesPlayer.FracturedFacesAIState.Finished;
				}
			}
			break;
		case FracturedFacesPlayer.FracturedFacesAIState.MovingToTarget:
			if (flag)
			{
				if (this.startedDelay && this.aiDelayTimer.Elapsed(false))
				{
					if (this.currentlyHeld == 200)
					{
						this.Pickup();
					}
					if (GameManager.rand.NextDouble() > 0.4)
					{
						this.targetPosition = this.positions[(int)this.puzzlePieces[(int)this.currentlyHeld].id];
					}
					else
					{
						this.targetPosition = this.positions[GameManager.rand.Next(0, this.positions.Count)];
					}
					this.AIState = FracturedFacesPlayer.FracturedFacesAIState.MovingPiece;
					this.startedDelay = false;
				}
				else if (!this.startedDelay)
				{
					this.startedDelay = true;
					this.aiDelayTimer.SetInterval(0.8f, 1.2f, true);
				}
			}
			break;
		case FracturedFacesPlayer.FracturedFacesAIState.MovingPiece:
			if (flag)
			{
				if (this.startedDelay && this.aiDelayTimer.Elapsed(false))
				{
					this.AIState = FracturedFacesPlayer.FracturedFacesAIState.RotatingPiece;
					this.startedDelay = false;
				}
				else if (!this.startedDelay)
				{
					this.startedDelay = true;
					this.aiDelayTimer.SetInterval(0.3f, 0.6f, true);
				}
			}
			break;
		case FracturedFacesPlayer.FracturedFacesAIState.RotatingPiece:
			if (this.puzzlePieces[(int)this.currentlyHeld].transform.rotation.eulerAngles.y != 0f || GameManager.rand.NextDouble() > 0.5)
			{
				if (this.startedDelay && this.aiDelayTimer.Elapsed(false))
				{
					this.Rotate();
					this.startedDelay = false;
				}
				else if (!this.startedDelay)
				{
					this.startedDelay = true;
					this.aiDelayTimer.SetInterval(0.6f, 0.9f, true);
				}
			}
			else
			{
				this.Pickup();
				this.AIState = FracturedFacesPlayer.FracturedFacesAIState.FindTarget;
			}
			break;
		}
		CharacterMoverInput result = default(CharacterMoverInput);
		if (!flag)
		{
			Vector3 vector = this.targetPosition - base.transform.position;
			Vector2 normalized = new Vector2(vector.x, vector.z).normalized;
			result = new CharacterMoverInput(normalized, false, false);
		}
		else
		{
			result.NullInput();
		}
		return result;
	}

	// Token: 0x06000C73 RID: 3187 RVA: 0x0003203C File Offset: 0x0003023C
	private float GetPointValue(Vector3 bombPosition, Vector3 target)
	{
		float num = 20f;
		float num2 = 0.75f;
		float num3 = 0.25f;
		float magnitude = (target - bombPosition).magnitude;
		Vector3 normalized = (target - base.transform.position).normalized;
		Vector3 normalized2 = (bombPosition - base.transform.position).normalized;
		float num4 = 1f - (Vector3.Dot(normalized, normalized2) + 1f) / 2f;
		return magnitude / num * num2 + num4 * num3;
	}

	// Token: 0x06000C74 RID: 3188 RVA: 0x000687F0 File Offset: 0x000669F0
	public override void ResetPlayer()
	{
		this.playerAnim.SetPlayerRotationImmediate(this.startRotation.eulerAngles.y);
		if (base.IsOwner)
		{
			this.mover.Velocity = Vector3.zero;
			this.velocity.Value = Vector3.zero;
		}
		base.ResetPlayer();
	}

	// Token: 0x06000C75 RID: 3189 RVA: 0x0000BC59 File Offset: 0x00009E59
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCOnJump(NetPlayer sender)
	{
		this.OnJump();
	}

	// Token: 0x06000C76 RID: 3190 RVA: 0x0000480A File Offset: 0x00002A0A
	protected void OnJump()
	{
		this.playerAnim.FireJumpTrigger();
		if (base.IsOwner)
		{
			base.SendRPC("RPCOnJump", NetRPCDelivery.RELIABLE_UNORDERED, Array.Empty<object>());
		}
	}

	// Token: 0x04000BB7 RID: 2999
	public GameObject puzzlePrefab;

	// Token: 0x04000BB8 RID: 3000
	public float gridSize = 2.1f;

	// Token: 0x04000BB9 RID: 3001
	public Mesh cube;

	// Token: 0x04000BBA RID: 3002
	public AudioClip actionSound;

	// Token: 0x04000BBB RID: 3003
	private FracturedFacesController minigameController;

	// Token: 0x04000BBC RID: 3004
	private CharacterMover mover;

	// Token: 0x04000BBD RID: 3005
	private CameraShake cameraShake;

	// Token: 0x04000BBF RID: 3007
	public List<FracturedFacesPuzzlePiece> puzzlePieces = new List<FracturedFacesPuzzlePiece>();

	// Token: 0x04000BC0 RID: 3008
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.CHANGES_OFTEN)]
	public NetArray<byte> puzzlePiecePositions = new NetArray<byte>(9);

	// Token: 0x04000BC1 RID: 3009
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.CHANGES_OFTEN)]
	public NetArray<byte> puzzlePieceRotations = new NetArray<byte>(9);

	// Token: 0x04000BC2 RID: 3010
	private byte currentlyHeld = 200;

	// Token: 0x04000BC3 RID: 3011
	private int[] playAreaMap = new int[]
	{
		2,
		3,
		6,
		7
	};

	// Token: 0x04000BC4 RID: 3012
	private float[] rotations = new float[]
	{
		0f,
		90f,
		180f,
		270f
	};

	// Token: 0x04000BC5 RID: 3013
	private List<Vector3> positions = new List<Vector3>();

	// Token: 0x04000BC6 RID: 3014
	private MeshRenderer finalPuzzle;

	// Token: 0x04000BC7 RID: 3015
	private bool startedDelay;

	// Token: 0x04000BC8 RID: 3016
	private ActionTimer aiDelayTimer = new ActionTimer(0.08f, 0.1f);

	// Token: 0x04000BC9 RID: 3017
	private Vector3 targetPosition = Vector3.zero;

	// Token: 0x04000BCA RID: 3018
	private FracturedFacesPlayer.FracturedFacesAIState AIState;

	// Token: 0x04000BCB RID: 3019
	private int[] pointsToCheck = new int[]
	{
		3,
		6,
		18
	};

	// Token: 0x020001B1 RID: 433
	private enum FracturedFacesAIState
	{
		// Token: 0x04000BCD RID: 3021
		Waiting,
		// Token: 0x04000BCE RID: 3022
		FindTarget,
		// Token: 0x04000BCF RID: 3023
		MovingToTarget,
		// Token: 0x04000BD0 RID: 3024
		MovingPiece,
		// Token: 0x04000BD1 RID: 3025
		RotatingPiece,
		// Token: 0x04000BD2 RID: 3026
		Finished
	}
}
