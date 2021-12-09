using System;
using System.Collections.Generic;
using UnityEngine;
using ZP.Net;

// Token: 0x020001A0 RID: 416
public class CountingPlayer : CharacterBase
{
	// Token: 0x06000BE0 RID: 3040 RVA: 0x0000B76F File Offset: 0x0000996F
	public override void OnNetInitialize()
	{
		base.OnNetInitialize();
		this.minigameController = (CountingController)GameManager.Minigame;
		this.minigameController.AddPlayer(this);
	}

	// Token: 0x06000BE1 RID: 3041 RVA: 0x0000B793 File Offset: 0x00009993
	protected override void Start()
	{
		base.Start();
		this.playerAnim.Animator.SetBool("CountingMinigame", true);
	}

	// Token: 0x06000BE2 RID: 3042 RVA: 0x0000B7B1 File Offset: 0x000099B1
	public override void FinishedSpawning()
	{
		base.FinishedSpawning();
	}

	// Token: 0x06000BE3 RID: 3043 RVA: 0x00064974 File Offset: 0x00062B74
	private void Update()
	{
		if (this.minigameController.Playable && base.IsOwner)
		{
			if (!base.GamePlayer.IsAI)
			{
				if (base.GamePlayer.RewiredPlayer.GetButtonDown(InputActions.Accept) && this.minigameController.curState == CountingController.CountingMinigameState.DoingRound && this.guessCount.Value < 39)
				{
					this.PressButton();
					return;
				}
			}
			else if (this.minigameController.curState == CountingController.CountingMinigameState.DoingRound)
			{
				if (this.lastRound != this.minigameController.curRound)
				{
					this.startTime = Time.time;
					this.aiCountTimings.Clear();
					bool flag = UnityEngine.Random.value > 0.5f;
					int num = 0;
					if (!flag)
					{
						num = UnityEngine.Random.Range(-5, 6);
					}
					for (int i = 0; i < this.minigameController.curCorrectCount + num; i++)
					{
						this.aiCountTimings.Add(UnityEngine.Random.Range(0f, 10f));
					}
					this.lastRound = this.minigameController.curRound;
				}
				int j = 0;
				while (j < this.aiCountTimings.Count)
				{
					if (Time.time - this.startTime >= this.aiCountTimings[j])
					{
						this.PressButton();
						this.aiCountTimings.RemoveAt(j);
					}
					else
					{
						j++;
					}
				}
			}
		}
	}

	// Token: 0x06000BE4 RID: 3044 RVA: 0x0000B7B9 File Offset: 0x000099B9
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCPress(NetPlayer sender)
	{
		this.PressButton();
	}

	// Token: 0x06000BE5 RID: 3045 RVA: 0x00064AC8 File Offset: 0x00062CC8
	private void PressButton()
	{
		if (base.IsOwner)
		{
			base.SendRPC("RPCPress", NetRPCDelivery.RELIABLE_ORDERED, Array.Empty<object>());
			NetVar<byte> netVar = this.guessCount;
			netVar.Value += 1;
		}
		AudioSystem.PlayOneShot(this.pressClip, (base.IsOwner && !base.GamePlayer.IsAI) ? 3f : 1f, 0f, 1f);
		this.playerAnim.Animator.SetTrigger("PressButton");
	}

	// Token: 0x04000B22 RID: 2850
	public AudioClip pressClip;

	// Token: 0x04000B23 RID: 2851
	private CountingController minigameController;

	// Token: 0x04000B24 RID: 2852
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.CHANGES_OFTEN)]
	public NetVar<byte> guessCount = new NetVar<byte>();

	// Token: 0x04000B25 RID: 2853
	private byte lastRound;

	// Token: 0x04000B26 RID: 2854
	private List<float> aiCountTimings = new List<float>();

	// Token: 0x04000B27 RID: 2855
	private float startTime;
}
