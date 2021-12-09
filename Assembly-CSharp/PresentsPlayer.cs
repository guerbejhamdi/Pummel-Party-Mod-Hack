using System;
using System.Collections.Generic;
using UnityEngine;
using ZP.Net;

// Token: 0x0200020B RID: 523
public class PresentsPlayer : CharacterBase
{
	// Token: 0x06000F62 RID: 3938 RVA: 0x0000D439 File Offset: 0x0000B639
	public override void OnNetInitialize()
	{
		base.OnNetInitialize();
		if (!base.IsOwner)
		{
			this.n_targetPos.Recieve = new RecieveProxy(this.RecieveTargetPos);
		}
	}

	// Token: 0x06000F63 RID: 3939 RVA: 0x00079C08 File Offset: 0x00077E08
	protected override void Start()
	{
		base.Start();
		this.m_minigameController = (PresentsController)GameManager.Minigame;
		this.m_minigameController.AddPlayer(this);
		this.m_minigameController.Root.GetComponentInChildren<CameraFollow>().AddTarget(this);
		this.cameraShake = this.m_minigameController.Root.GetComponentInChildren<CameraShake>();
		this.m_minigameController.SetConveyorColor((int)base.GamePlayer.GlobalID, base.GamePlayer.Color.skinColor1);
		this.m_anim.SetBool("Carrying", true);
	}

	// Token: 0x06000F64 RID: 3940 RVA: 0x00079C9C File Offset: 0x00077E9C
	private void Update()
	{
		if (this.m_minigameController.Playable)
		{
			if (!this.m_gotStartPos && this.m_minigameController.SpawnPoints != null && base.Owner.Slot < this.m_minigameController.SpawnPoints.Length)
			{
				this.m_startPos = this.m_minigameController.SpawnPoints[(int)base.GamePlayer.GlobalID].position;
				this.m_gotStartPos = true;
			}
			this.m_anim.SetBool("Carrying", true);
			if (base.IsOwner)
			{
				if (!this.player.IsAI)
				{
					float axis = base.GamePlayer.RewiredPlayer.GetAxis(InputActions.Horizontal);
					bool flag = false;
					bool flag2 = false;
					if (Mathf.Abs(axis) > this.m_sensitivity && (Mathf.Sign(axis) != Mathf.Sign(this.m_lastAxis) || this.m_moveTime >= this.m_totalMoveTime))
					{
						flag = (axis < 0f);
						flag2 = (axis > 0f);
					}
					this.m_lastAxis = axis;
					if (flag && this.m_targetPos > 0)
					{
						this.m_targetPos--;
						this.m_move = true;
					}
					if (flag2 && this.m_targetPos <= 1)
					{
						this.m_targetPos++;
						this.m_move = true;
					}
				}
				else if (this.m_minigameController.GetNextGroup() != this.m_nextGroup)
				{
					this.m_nextGroup = this.m_minigameController.GetNextGroup();
					if (this.m_nextGroup != null)
					{
						List<PresentInfo> presentList = this.m_nextGroup.GetPresentList();
						float num = float.NegativeInfinity;
						int targetPos = -1;
						for (int i = 0; i < presentList.Count; i++)
						{
							if (presentList[i] != null && (float)presentList[i].value > num)
							{
								num = (float)presentList[i].value;
								targetPos = i;
							}
						}
						if (UnityEngine.Random.value > 0.25f)
						{
							this.m_targetPos = targetPos;
						}
						else
						{
							this.m_targetPos = UnityEngine.Random.Range(0, 2);
						}
						this.m_move = true;
					}
				}
				this.n_targetPos.Value = (byte)this.m_targetPos;
			}
			if (this.m_move)
			{
				float num2 = Vector3.Distance(base.transform.position, this.GetTargetPos());
				this.m_moveTime = 0f;
				this.m_totalMoveTime = num2 / this.m_moveSpeed;
				this.m_moveStartPos = base.transform.position;
				this.m_move = false;
			}
			if (this.m_moveTime < this.m_totalMoveTime)
			{
				float t = Mathf.Clamp01(this.m_moveTime / this.m_totalMoveTime);
				base.transform.position = Vector3.Lerp(this.m_moveStartPos, this.GetTargetPos(), t);
				this.m_moveTime += Time.deltaTime;
				this.m_axis = Mathf.MoveTowards(this.m_axis, (this.GetTargetPos() - this.m_moveStartPos).normalized.x, Time.deltaTime * 8f);
				this.m_vel = Mathf.MoveTowards(this.m_vel, 1f, Time.deltaTime * 8f);
			}
			else
			{
				base.transform.position = this.GetTargetPos();
				this.m_axis = Mathf.MoveTowards(this.m_axis, 0f, Time.deltaTime * 4f);
				this.m_vel = Mathf.MoveTowards(this.m_vel, 0f, Time.deltaTime * 2f);
			}
			this.m_anim.SetFloat("MovementAxisX", this.m_axis);
			this.m_anim.SetFloat("Velocity", this.m_vel);
			this.m_anim.SetBool("MovementAxisZero", Mathf.Abs(this.m_axis) < 0.5f);
			return;
		}
		this.m_axis = Mathf.MoveTowards(this.m_axis, 0f, Time.deltaTime * 4f);
		this.m_vel = Mathf.MoveTowards(this.m_vel, 0f, Time.deltaTime * 2f);
	}

	// Token: 0x06000F65 RID: 3941 RVA: 0x0007A0AC File Offset: 0x000782AC
	private Vector3 GetTargetPos()
	{
		switch (this.m_targetPos)
		{
		case 0:
			return this.m_startPos - this.m_moveDistance * base.transform.right;
		case 1:
			return this.m_startPos;
		case 2:
			return this.m_startPos + this.m_moveDistance * base.transform.right;
		default:
			return Vector3.zero;
		}
	}

	// Token: 0x06000F66 RID: 3942 RVA: 0x0007A124 File Offset: 0x00078324
	public void GivePresent(PresentInfo info)
	{
		if (this.m_minigameController.GetTime() <= 0f)
		{
			return;
		}
		if (info.value >= 0)
		{
			AudioSystem.PlayOneShot(this.m_goodPresentClip, 1f, 0.1f, 1f);
		}
		else
		{
			this.gotCoal = true;
			AudioSystem.PlayOneShot(this.m_badPresentClip, 1f, 0.1f, 1f);
		}
		if (!base.IsOwner)
		{
			return;
		}
		if (NetSystem.IsServer)
		{
			this.Score += (short)info.value;
			if (this.m_minigameController != null && this.m_minigameController.MinigameCamera != null)
			{
				if (info.value >= 0)
				{
					GameManager.UIController.SpawnWorldText(info.value.ToString(), base.transform.position + Vector3.up * 0.5f, 2f, WorldTextType.GiveGold, 0.5f, this.m_minigameController.MinigameCamera);
					return;
				}
				GameManager.UIController.SpawnWorldText(info.value.ToString(), base.transform.position + Vector3.up * 0.5f, 2f, WorldTextType.Damage, 0.5f, this.m_minigameController.MinigameCamera);
				return;
			}
		}
		else
		{
			base.SendRPC("GotPresentRPC", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				(short)info.value
			});
		}
	}

	// Token: 0x06000F67 RID: 3943 RVA: 0x0000D460 File Offset: 0x0000B660
	[NetRPC(false, NetRPCSecurity.OWNER, NetRPCSecurity.SERVER)]
	public void GotPresentRPC(NetPlayer sender, short value)
	{
		this.Score += value;
	}

	// Token: 0x06000F68 RID: 3944 RVA: 0x0000D471 File Offset: 0x0000B671
	public void RecieveTargetPos(object _pos)
	{
		this.m_targetPos = (int)((byte)_pos);
		this.m_move = true;
	}

	// Token: 0x04000F2E RID: 3886
	[SerializeField]
	protected Animator m_anim;

	// Token: 0x04000F2F RID: 3887
	[SerializeField]
	protected AudioClip m_goodPresentClip;

	// Token: 0x04000F30 RID: 3888
	[SerializeField]
	protected AudioClip m_badPresentClip;

	// Token: 0x04000F31 RID: 3889
	private PresentsController m_minigameController;

	// Token: 0x04000F32 RID: 3890
	private CameraShake cameraShake;

	// Token: 0x04000F33 RID: 3891
	private int m_targetPos = 1;

	// Token: 0x04000F34 RID: 3892
	private int m_nextMove;

	// Token: 0x04000F35 RID: 3893
	private bool m_isMoving;

	// Token: 0x04000F36 RID: 3894
	private float m_totalMoveTime;

	// Token: 0x04000F37 RID: 3895
	private float m_moveTime;

	// Token: 0x04000F38 RID: 3896
	private float m_moveDistance = 1f;

	// Token: 0x04000F39 RID: 3897
	private float m_moveSpeed = 6f;

	// Token: 0x04000F3A RID: 3898
	private Vector3 m_moveStartPos = Vector3.zero;

	// Token: 0x04000F3B RID: 3899
	private Vector3 m_startPos;

	// Token: 0x04000F3C RID: 3900
	private float m_axis;

	// Token: 0x04000F3D RID: 3901
	private float m_vel;

	// Token: 0x04000F3E RID: 3902
	private PresentsGroup m_nextGroup;

	// Token: 0x04000F3F RID: 3903
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.NONE)]
	private NetVar<byte> n_targetPos = new NetVar<byte>(1);

	// Token: 0x04000F40 RID: 3904
	private bool m_gotStartPos;

	// Token: 0x04000F41 RID: 3905
	private bool m_move;

	// Token: 0x04000F42 RID: 3906
	private float m_lastAxis;

	// Token: 0x04000F43 RID: 3907
	private float m_sensitivity = 0.3f;

	// Token: 0x04000F44 RID: 3908
	public bool gotCoal;
}
