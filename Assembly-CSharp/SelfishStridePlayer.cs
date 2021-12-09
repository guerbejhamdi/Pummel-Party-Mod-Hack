using System;
using UnityEngine;
using ZP.Net;

// Token: 0x02000238 RID: 568
public class SelfishStridePlayer : CharacterBase
{
	// Token: 0x17000172 RID: 370
	// (get) Token: 0x06001066 RID: 4198 RVA: 0x0000DC91 File Offset: 0x0000BE91
	// (set) Token: 0x06001067 RID: 4199 RVA: 0x0000DC99 File Offset: 0x0000BE99
	public bool IsAlone { get; set; }

	// Token: 0x17000173 RID: 371
	// (get) Token: 0x06001068 RID: 4200 RVA: 0x0000DCA2 File Offset: 0x0000BEA2
	// (set) Token: 0x06001069 RID: 4201 RVA: 0x0000DCAA File Offset: 0x0000BEAA
	public int Subtarget { get; set; }

	// Token: 0x0600106A RID: 4202 RVA: 0x00080C14 File Offset: 0x0007EE14
	public override void OnNetInitialize()
	{
		base.OnNetInitialize();
		this.mover = base.GetComponent<CharacterMover>();
		this.minigameController = (SelfishStrideController)GameManager.Minigame;
		this.minigameController.AddPlayer(this);
		this.lockedInRenderer = this.minigameController.Spawn(this.lockedInPrefab, base.transform.position + Vector3.down * 0.97f, Quaternion.identity).GetComponentInChildren<MeshRenderer>();
	}

	// Token: 0x0600106B RID: 4203 RVA: 0x0000DCB3 File Offset: 0x0000BEB3
	protected override void Start()
	{
		base.Start();
	}

	// Token: 0x0600106C RID: 4204 RVA: 0x0000B7B1 File Offset: 0x000099B1
	public override void FinishedSpawning()
	{
		base.FinishedSpawning();
	}

	// Token: 0x0600106D RID: 4205 RVA: 0x00080C90 File Offset: 0x0007EE90
	private void Update()
	{
		if (this.minigameController.Playable)
		{
			if (this.minigameController.curState == SelfishStrideController.SelfishStrideState.GettingDirectionChoice && base.IsOwner)
			{
				if (!base.GamePlayer.IsAI)
				{
					if (base.GamePlayer.RewiredPlayer.GetButtonDown(InputActions.Accept))
					{
						this.PressButton(new Vector2(-base.GamePlayer.RewiredPlayer.GetAxis(InputActions.Vertical), base.GamePlayer.RewiredPlayer.GetAxis(InputActions.Horizontal)));
						return;
					}
				}
				else if (!this.hasAiPressedButton)
				{
					if (!this.aiTimerStarted)
					{
						this.AiPressTimer.Start();
						this.aiTimerStarted = true;
						return;
					}
					if (this.AiPressTimer.Elapsed(true))
					{
						this.target.Value = (byte)GameManager.rand.Next(0, this.minigameController.bridges.Count);
						this.PressButton((new Vector2(this.minigameController.bridges[(int)this.target.Value].endPosition.position.x, this.minigameController.bridges[(int)this.target.Value].endPosition.position.z) - Vector2.zero).normalized);
						this.hasAiPressedButton = true;
						return;
					}
				}
			}
			else if (this.minigameController.curState == SelfishStrideController.SelfishStrideState.DelayBeforeMovement)
			{
				if (this.target.Value == 250)
				{
					this.target.Value = (byte)GameManager.rand.Next(0, this.minigameController.bridges.Count);
					this.PressButton((new Vector2(this.minigameController.bridges[(int)this.target.Value].endPosition.position.x, this.minigameController.bridges[(int)this.target.Value].endPosition.position.z) - Vector2.zero).normalized);
					return;
				}
			}
			else if (this.minigameController.curState == SelfishStrideController.SelfishStrideState.MovingToBridge || this.minigameController.curState == SelfishStrideController.SelfishStrideState.CrossingBridge)
			{
				Transform transform = this.IsAlone ? this.minigameController.bridges[(int)this.target.Value].onePlayerStandPosition : this.minigameController.bridges[(int)this.target.Value].multiPlayerStandPositions[this.Subtarget];
				if (this.minigameController.curState == SelfishStrideController.SelfishStrideState.CrossingBridge)
				{
					transform = this.minigameController.bridges[(int)this.target.Value].endPosition;
				}
				Vector2 vector = new Vector2(transform.position.x, transform.position.z) - new Vector2(base.transform.position.x, base.transform.position.z);
				CharacterMoverInput input = new CharacterMoverInput(vector.normalized, false, false);
				if (vector.magnitude < 0.75f)
				{
					input.NullInput();
				}
				this.mover.CalculateVelocity(input, Time.deltaTime);
				this.mover.DoMovement(Time.deltaTime);
				this.mover.SmoothSlope();
				if (this.mover.MovementAxis != Vector2.zero)
				{
					Vector3 normalized = (this.minigameController.bridges[(int)this.target.Value].endPosition.transform.position - base.transform.position).normalized;
					base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, Quaternion.LookRotation(normalized, Vector3.up), 1500f * Time.deltaTime);
				}
				if (this.player_root.activeSelf)
				{
					this.UpdateAnimationState();
					this.playerAnim.UpdateAnimationState();
				}
			}
		}
	}

	// Token: 0x0600106E RID: 4206 RVA: 0x000810A0 File Offset: 0x0007F2A0
	public void UpdateAnimationState()
	{
		Vector2 vector = new Vector2(this.mover.Velocity.x, this.mover.Velocity.z);
		float num = Mathf.Clamp01(vector.magnitude / this.mover.maxSpeed);
		this.playerAnim.Velocity = num;
		this.playerAnim.VelocityY = this.mover.Velocity.y;
		this.playerAnim.MovementAxis = ((num > 0.01f) ? Vector2.up : Vector2.zero);
		this.playerAnim.Grounded = this.mover.Grounded;
		this.playerAnim.SetPlayerRotation(base.transform.rotation.eulerAngles.y);
	}

	// Token: 0x0600106F RID: 4207 RVA: 0x0000DCBB File Offset: 0x0000BEBB
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCPress(NetPlayer sender, Vector2 dir)
	{
		this.PressButton(dir);
	}

	// Token: 0x06001070 RID: 4208 RVA: 0x0008116C File Offset: 0x0007F36C
	private void PressButton(Vector2 dir)
	{
		AudioSystem.PlayOneShot(this.lockInSound, 1f, 0f, 1f);
		this.lockedInRenderer.material = this.lockedInMat;
		if (base.IsOwner)
		{
			base.SendRPC("RPCPress", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				dir
			});
			float num = float.MinValue;
			int num2 = 0;
			for (int i = 0; i < this.minigameController.bridges.Count; i++)
			{
				float num3 = Vector3.Dot((new Vector2(this.minigameController.bridges[i].endPosition.position.x, this.minigameController.bridges[i].endPosition.position.z) - Vector2.zero).normalized, dir.normalized);
				if (num3 > num)
				{
					num = num3;
					num2 = i;
				}
			}
			this.target.Value = (byte)num2;
		}
	}

	// Token: 0x06001071 RID: 4209 RVA: 0x00081274 File Offset: 0x0007F474
	public override void ResetPlayer()
	{
		base.ResetPlayer();
		this.Deactivate();
		this.lockedInRenderer.material = this.notLockedInMat;
		if (base.IsOwner)
		{
			this.target.Value = 250;
		}
		this.hasAiPressedButton = false;
		this.aiTimerStarted = false;
	}

	// Token: 0x040010D0 RID: 4304
	public AudioClip lockInSound;

	// Token: 0x040010D1 RID: 4305
	public GameObject lockedInPrefab;

	// Token: 0x040010D2 RID: 4306
	public Material lockedInMat;

	// Token: 0x040010D3 RID: 4307
	public Material notLockedInMat;

	// Token: 0x040010D4 RID: 4308
	private SelfishStrideController minigameController;

	// Token: 0x040010D5 RID: 4309
	private CharacterMover mover;

	// Token: 0x040010D6 RID: 4310
	private MeshRenderer lockedInRenderer;

	// Token: 0x040010D9 RID: 4313
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.CHANGES_OFTEN)]
	public NetVar<byte> target = new NetVar<byte>(250);

	// Token: 0x040010DA RID: 4314
	private ActionTimer AiPressTimer = new ActionTimer(1f, 4f);

	// Token: 0x040010DB RID: 4315
	private bool hasAiPressedButton;

	// Token: 0x040010DC RID: 4316
	private bool aiTimerStarted;
}
