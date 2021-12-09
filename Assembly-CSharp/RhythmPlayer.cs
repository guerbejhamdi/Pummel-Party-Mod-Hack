using System;
using Rewired;
using UnityEngine;
using ZP.Net;

// Token: 0x0200021E RID: 542
public class RhythmPlayer : CharacterBase
{
	// Token: 0x06000FC4 RID: 4036 RVA: 0x0000D7CC File Offset: 0x0000B9CC
	public void ComboRecieve(object val)
	{
		this.Combo = (short)val;
	}

	// Token: 0x17000161 RID: 353
	// (get) Token: 0x06000FC5 RID: 4037 RVA: 0x0000D7DA File Offset: 0x0000B9DA
	public short LargestCombo
	{
		get
		{
			return this.m_largestCombo;
		}
	}

	// Token: 0x17000162 RID: 354
	// (get) Token: 0x06000FC6 RID: 4038 RVA: 0x0000D7E2 File Offset: 0x0000B9E2
	// (set) Token: 0x06000FC7 RID: 4039 RVA: 0x0007D010 File Offset: 0x0007B210
	public short Combo
	{
		get
		{
			return this.n_combo.Value;
		}
		set
		{
			this.n_combo.Value = value;
			if (value > this.m_largestCombo)
			{
				this.m_largestCombo = value;
			}
			if (this.m_panel != null)
			{
				this.m_panel.SetCombo((int)this.player.GlobalID, this.n_combo.Value);
			}
		}
	}

	// Token: 0x06000FC8 RID: 4040 RVA: 0x0007D068 File Offset: 0x0007B268
	public override void OnNetInitialize()
	{
		base.OnNetInitialize();
		this.minigameController = (RhythmController)GameManager.Minigame;
		if (!NetSystem.IsServer)
		{
			this.score.Recieve = new RecieveProxy(base.ScoreRecieve);
		}
		if (!base.IsOwner)
		{
			this.n_combo.Recieve = new RecieveProxy(this.ComboRecieve);
		}
	}

	// Token: 0x06000FC9 RID: 4041 RVA: 0x00004FF6 File Offset: 0x000031F6
	public override void OnOwnerChanged()
	{
		base.OnOwnerChanged();
	}

	// Token: 0x06000FCA RID: 4042 RVA: 0x0000D7EF File Offset: 0x0000B9EF
	public void OnDestroy()
	{
		UnityEngine.Object.Destroy(this.m_cameraParent);
	}

	// Token: 0x06000FCB RID: 4043 RVA: 0x0000D7FC File Offset: 0x0000B9FC
	protected override void Start()
	{
		base.Start();
		this.minigameController.AddPlayer(this);
		this.m_anim = base.GetComponentInChildren<Animator>(true);
	}

	// Token: 0x06000FCC RID: 4044 RVA: 0x0007D0C8 File Offset: 0x0007B2C8
	private void Update()
	{
		if (!this.m_initialized && this.minigameController.State == MinigameControllerState.Playing)
		{
			this.m_initialized = true;
		}
		if (this.m_notDancing == 0 && this.minigameController.State == MinigameControllerState.Playing)
		{
			this.m_anim = base.GetComponentInChildren<Animator>(true);
			this.m_anim.SetTrigger("Dance");
			this.m_notDancing = 1;
		}
		else if (this.m_notDancing == 1 && this.minigameController.State != MinigameControllerState.Playing)
		{
			this.m_anim.SetTrigger("StopDancing");
			this.m_notDancing = 2;
		}
		if (base.IsOwner && this.minigameController.State == MinigameControllerState.Playing)
		{
			RhythmHitButton rhythmHitButton = RhythmHitButton.None;
			if (!this.player.IsAI)
			{
				ControllerType type = this.player.RewiredPlayer.controllers.GetLastActiveController().type;
				if (type != this.m_lastControllerType)
				{
					RhythmInputType inputType = RhythmInputType.PC;
					if (type == ControllerType.Joystick)
					{
						if (this.player.RewiredPlayer.controllers.GetLastActiveController().name.Contains("Sony") || this.player.RewiredPlayer.controllers.GetLastActiveController().hardwareName.Contains("Sony"))
						{
							inputType = RhythmInputType.Playstation;
						}
						else
						{
							inputType = RhythmInputType.Xbox;
						}
					}
					this.Panel.SetInputType((int)this.player.GlobalID, inputType);
					this.m_lastControllerType = type;
				}
				if (this.player.RewiredPlayer.GetButtonDown(InputActions.RhythmButtonUp))
				{
					rhythmHitButton = RhythmHitButton.ButtonUp;
				}
				if (this.player.RewiredPlayer.GetButtonDown(InputActions.RhythmButtonDown))
				{
					rhythmHitButton = RhythmHitButton.ButtonDown;
				}
				if (this.player.RewiredPlayer.GetButtonDown(InputActions.RhythmButtonLeft))
				{
					rhythmHitButton = RhythmHitButton.ButtonLeft;
				}
				if (this.player.RewiredPlayer.GetButtonDown(InputActions.RhythmButtonRight))
				{
					rhythmHitButton = RhythmHitButton.ButtonRight;
				}
				if (this.player.RewiredPlayer.GetButtonDown(InputActions.RhythmUp))
				{
					rhythmHitButton = RhythmHitButton.Up;
				}
				if (this.player.RewiredPlayer.GetButtonDown(InputActions.RhythmDown))
				{
					rhythmHitButton = RhythmHitButton.Down;
				}
				if (this.player.RewiredPlayer.GetButtonDown(InputActions.RhythmLeft))
				{
					rhythmHitButton = RhythmHitButton.Left;
				}
				if (this.player.RewiredPlayer.GetButtonDown(InputActions.RhythmRight))
				{
					rhythmHitButton = RhythmHitButton.Right;
				}
			}
			else
			{
				RhythmHitButton rhythmHitButton2 = RhythmHitButton.None;
				float num = 0f;
				if (this.Panel.TryHitTrack((int)this.player.GlobalID, out rhythmHitButton2, out num) == this.m_aiNextHit && num < 0.5f)
				{
					rhythmHitButton = rhythmHitButton2;
					float value = UnityEngine.Random.value;
					if (value < 0.65f)
					{
						this.m_aiNextHit = RhythmHitResult.Perfect;
					}
					else if (value < 0.75f)
					{
						this.m_aiNextHit = RhythmHitResult.Great;
					}
					else if (value < 0.95f)
					{
						this.m_aiNextHit = RhythmHitResult.Good;
					}
					else
					{
						this.m_aiNextHit = RhythmHitResult.Miss;
					}
				}
			}
			if (rhythmHitButton != RhythmHitButton.None)
			{
				int btnIndex = 0;
				RhythmHitResult rhythmHitResult = this.Panel.HitTrack((int)this.player.GlobalID, rhythmHitButton, out btnIndex, !this.player.IsAI);
				if (rhythmHitResult != RhythmHitResult.Miss)
				{
					this.HitButtonResult(rhythmHitResult, btnIndex);
					this.Combo += 1;
					return;
				}
				this.Combo = 0;
			}
		}
	}

	// Token: 0x06000FCD RID: 4045 RVA: 0x0007D3C0 File Offset: 0x0007B5C0
	private void HitButtonResult(RhythmHitResult result, int btnIndex)
	{
		this.Panel.HitButton((int)this.player.GlobalID, btnIndex);
		if (base.IsOwner)
		{
			base.SendRPC("RPCHitButtonResult", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				(byte)result,
				btnIndex
			});
		}
		if (NetSystem.IsServer)
		{
			this.Score += (short)this.m_pointResultMap[(int)result];
		}
	}

	// Token: 0x06000FCE RID: 4046 RVA: 0x0007D430 File Offset: 0x0007B630
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCHitButtonResult(NetPlayer sender, byte resultIdx, int btnIndex)
	{
		this.HitButtonResult((RhythmHitResult)resultIdx, btnIndex);
	}

	// Token: 0x06000FCF RID: 4047 RVA: 0x0007D448 File Offset: 0x0007B648
	public void AddCombo(int bonus)
	{
		try
		{
			if (NetSystem.IsServer)
			{
				base.SendRPC("RPCAddCombo", NetRPCDelivery.RELIABLE_ORDERED, new object[]
				{
					bonus
				});
			}
			if (base.IsOwner)
			{
				this.Combo = 0;
			}
			if (this.m_panel == null)
			{
				this.m_panel = UnityEngine.Object.FindObjectOfType<RhythmUIPanel>();
			}
			if (this.m_panel != null)
			{
				this.m_panel.SetCombo((int)this.player.GlobalID, 0);
				this.m_panel.ShowComboBonus((int)this.player.GlobalID, bonus);
			}
			AudioSystem.PlayOneShot(this.m_comboClip, 1f, 0.1f, 1f);
		}
		catch (Exception ex)
		{
			Debug.LogError(ex.Message);
			Debug.LogError(ex.StackTrace);
		}
	}

	// Token: 0x06000FD0 RID: 4048 RVA: 0x0000D81D File Offset: 0x0000BA1D
	[NetRPC(false, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void RPCAddCombo(NetPlayer sender, int bonus)
	{
		this.AddCombo(bonus);
	}

	// Token: 0x17000163 RID: 355
	// (get) Token: 0x06000FD1 RID: 4049 RVA: 0x0000D826 File Offset: 0x0000BA26
	private RhythmUIPanel Panel
	{
		get
		{
			if (this.m_panel == null)
			{
				this.m_panel = UnityEngine.Object.FindObjectOfType<RhythmUIPanel>();
			}
			return this.m_panel;
		}
	}

	// Token: 0x04000FF0 RID: 4080
	[SerializeField]
	protected GameObject m_cameraParent;

	// Token: 0x04000FF1 RID: 4081
	[SerializeField]
	private AudioClip m_comboClip;

	// Token: 0x04000FF2 RID: 4082
	[SerializeField]
	protected AudioListener m_listener;

	// Token: 0x04000FF3 RID: 4083
	private RhythmController minigameController;

	// Token: 0x04000FF4 RID: 4084
	private bool m_initialized;

	// Token: 0x04000FF5 RID: 4085
	private int[] m_pointResultMap = new int[]
	{
		50,
		30,
		10,
		0
	};

	// Token: 0x04000FF6 RID: 4086
	private RhythmHitResult m_aiNextHit;

	// Token: 0x04000FF7 RID: 4087
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.NONE)]
	public NetVar<short> n_combo = new NetVar<short>(0);

	// Token: 0x04000FF8 RID: 4088
	private short m_largestCombo;

	// Token: 0x04000FF9 RID: 4089
	private Animator m_anim;

	// Token: 0x04000FFA RID: 4090
	private float m_nextFreezeCheck;

	// Token: 0x04000FFB RID: 4091
	private int a;

	// Token: 0x04000FFC RID: 4092
	private ControllerType m_lastControllerType;

	// Token: 0x04000FFD RID: 4093
	private int m_notDancing;

	// Token: 0x04000FFE RID: 4094
	private RhythmUIPanel m_panel;
}
