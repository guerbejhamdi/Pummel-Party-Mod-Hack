using System;
using UnityEngine;
using ZP.Net;

// Token: 0x020001EC RID: 492
public class NetworkTestPlayer1 : CharacterBase
{
	// Token: 0x06000E54 RID: 3668 RVA: 0x00072980 File Offset: 0x00070B80
	public override void OnNetInitialize()
	{
		this.minigame_controller = GameManager.Minigame;
		this.minigame_controller.AddPlayer(this);
		if (base.IsOwner)
		{
			this.mover = base.GetComponent<CharacterMover>();
			UnityEngine.Object.Instantiate<GameObject>(this.camera_prefab, Vector3.zero, Quaternion.identity).transform.parent = base.transform;
			this.player_cam = base.gameObject.GetComponent<ThirdPersonCamera>();
			this.player_cam.SetTargetCamera(base.GetComponentInChildren<Camera>());
			this.player_cam.PositionalTargetTransform = base.transform;
			this.player_cam.YRotation = base.transform.rotation.eulerAngles.y;
			this.player_cam.ZRotation = 10f;
			this.player_cam.RotateCamera();
			this.player_cam.UpdateCamera();
			this.spectating_player = this;
		}
		if (base.IsOwner)
		{
			this.position.Value = base.transform.position;
		}
		else
		{
			this.position.Recieve = new RecieveProxy(this.RecievePosition);
		}
		base.OnNetInitialize();
	}

	// Token: 0x06000E55 RID: 3669 RVA: 0x00072AA0 File Offset: 0x00070CA0
	private new void Start()
	{
		base.transform.Find("Cube").GetComponent<MeshRenderer>().material.color = GameManager.GetColorAtIndex((int)base.OwnerSlot).skinColor1;
		if (base.OwnerSlot != 1)
		{
			ushort ownerSlot = base.OwnerSlot;
		}
	}

	// Token: 0x06000E56 RID: 3670 RVA: 0x0000CB3A File Offset: 0x0000AD3A
	private void Update()
	{
		this.DoUpdate();
		this.position_end = base.transform.position;
	}

	// Token: 0x06000E57 RID: 3671 RVA: 0x0000398C File Offset: 0x00001B8C
	private void FixedUpdate()
	{
	}

	// Token: 0x06000E58 RID: 3672 RVA: 0x00072AF0 File Offset: 0x00070CF0
	private void LateUpdate()
	{
		if (base.transform.position != this.position_end)
		{
			Debug.Log("Not Equal Late update");
		}
		if (base.IsOwner && this.minigame_controller.Playable && !GameManager.IsGamePaused && GameManager.PollInput && !Input.GetKey(KeyCode.Q))
		{
			this.player_cam.RotateCamera();
			this.player_cam.UpdateCamera();
		}
	}

	// Token: 0x06000E59 RID: 3673 RVA: 0x0000CB53 File Offset: 0x0000AD53
	private void OnPreRender()
	{
		if (base.transform.position != this.position_end)
		{
			Debug.Log("Not Equal pre render");
		}
	}

	// Token: 0x06000E5A RID: 3674 RVA: 0x00072B64 File Offset: 0x00070D64
	private void DoUpdate()
	{
		if (base.IsOwner)
		{
			if (Input.GetKeyDown(KeyCode.U))
			{
				this.player_cam.PositionalTargetTransform = this.minigame_controller.Root.transform.Find("WatchCube");
			}
			if (Input.GetKeyDown(KeyCode.Space))
			{
				int i = 0;
				int num = (int)this.spectating_player.OwnerSlot;
				while (i < this.minigame_controller.GetPlayerCount())
				{
					num++;
					if (num >= this.minigame_controller.GetPlayerCount())
					{
						num = 0;
					}
					NetworkTestPlayer1 networkTestPlayer = (NetworkTestPlayer1)this.minigame_controller.GetPlayerInSlot((short)num);
					if (networkTestPlayer != null && !networkTestPlayer.isDead)
					{
						this.spectating_player = networkTestPlayer;
						this.player_cam.PositionalTargetTransform = this.spectating_player.transform;
						break;
					}
					i++;
				}
			}
			this.DoMovement2();
			this.position.Value = base.transform.position;
			return;
		}
		if (Input.GetKeyDown(KeyCode.P))
		{
			this.set_instant = !this.set_instant;
		}
		if (!this.set_instant && this.got_position)
		{
			Vector3 b = base.transform.position;
			base.transform.position = this.position.Value;
			Vector3.Dot((base.transform.position - b).normalized, this.velocity.Value.normalized);
			this.got_position = false;
			return;
		}
		if (!this.set_instant)
		{
			base.transform.position += this.velocity.Value * Time.deltaTime;
		}
	}

	// Token: 0x06000E5B RID: 3675 RVA: 0x0000CB77 File Offset: 0x0000AD77
	public void RecievePosition(object _pos)
	{
		if (this.set_instant)
		{
			base.transform.position = this.position.Value;
			return;
		}
		this.got_position = true;
	}

	// Token: 0x06000E5C RID: 3676 RVA: 0x00072D08 File Offset: 0x00070F08
	private void DoMovement2()
	{
		Vector3 a = this.startPosition + this.dir * this.range;
		Vector3 vector = a - base.transform.position;
		Vector3 b = vector.normalized * this.speed * Time.deltaTime;
		if (vector.magnitude - b.magnitude < 0.01f)
		{
			base.transform.position = a;
			this.dir *= -1f;
		}
		else
		{
			base.transform.position += b;
		}
		this.velocity.Value = vector.normalized * this.speed;
	}

	// Token: 0x06000E5D RID: 3677 RVA: 0x00072DD0 File Offset: 0x00070FD0
	private void DoMovement()
	{
		Vector3 lookPos = this.player_cam.GetLookPos();
		this.mover.SetForwardVector(new Vector3(lookPos.x, 0f, lookPos.z).normalized);
		CharacterMoverInput input = new CharacterMoverInput(Input.GetKey(KeyCode.W), Input.GetKey(KeyCode.S), Input.GetKey(KeyCode.A), Input.GetKey(KeyCode.D), Input.GetKeyDown(KeyCode.Space));
		input.left = this.left;
		input.right = !this.left;
		if (Time.time - this.last_switch > this.switch_interval)
		{
			this.last_switch = Time.time;
			this.left = !this.left;
		}
		if (!this.minigame_controller.Playable || GameManager.IsGamePaused || !GameManager.PollInput)
		{
			input.NullInput();
		}
		this.mover.CalculateVelocity(input, Time.deltaTime);
		this.mover.DoMovement(Time.deltaTime);
		this.mover.SmoothSlope();
		if (this.minigame_controller.Playable || GameManager.IsGamePaused || !GameManager.PollInput)
		{
			this.player_cam.RotateCamera();
			this.player_cam.UpdateCamera();
		}
		this.velocity.Value = this.mover.Velocity;
	}

	// Token: 0x04000DC9 RID: 3529
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.ALWAYS_SEND)]
	public NetVec3 position = new NetVec3(Vector3.zero);

	// Token: 0x04000DCA RID: 3530
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.ALWAYS_SEND)]
	public NetVec3 velocity = new NetVec3(Vector3.zero);

	// Token: 0x04000DCB RID: 3531
	public GameObject camera_prefab;

	// Token: 0x04000DCC RID: 3532
	private bool got_position;

	// Token: 0x04000DCD RID: 3533
	private MinigameController minigame_controller;

	// Token: 0x04000DCE RID: 3534
	private bool set_instant;

	// Token: 0x04000DCF RID: 3535
	private CharacterMover mover;

	// Token: 0x04000DD0 RID: 3536
	private ThirdPersonCamera player_cam;

	// Token: 0x04000DD1 RID: 3537
	private NetworkTestPlayer1 spectating_player;

	// Token: 0x04000DD2 RID: 3538
	private Vector3 position_end = Vector3.zero;

	// Token: 0x04000DD3 RID: 3539
	private float range = 5f;

	// Token: 0x04000DD4 RID: 3540
	private Vector3 dir = new Vector3(1f, 0f, 0f);

	// Token: 0x04000DD5 RID: 3541
	private float speed = 5f;

	// Token: 0x04000DD6 RID: 3542
	private float last_switch;

	// Token: 0x04000DD7 RID: 3543
	private float switch_interval = 0.5f;

	// Token: 0x04000DD8 RID: 3544
	private bool left;
}
