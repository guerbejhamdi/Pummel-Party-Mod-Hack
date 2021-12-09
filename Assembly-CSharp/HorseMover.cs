using System;
using UnityEngine;
using ZP.Net;

// Token: 0x020000AE RID: 174
public class HorseMover : NetBehaviour
{
	// Token: 0x060003A2 RID: 930 RVA: 0x0003A43C File Offset: 0x0003863C
	public override void OnNetInitialize()
	{
		base.OnNetInitialize();
		this.moverID.Recieve = new RecieveProxy(this.RecieveMoverID);
		this.jumperID.Recieve = new RecieveProxy(this.RecieveJumperID);
		this.minigameController = (HorsesController)GameManager.Minigame;
		this.minigameController.minigameCameras.Add(this.cam);
		this.cMover = base.GetComponent<CharacterMover>();
	}

	// Token: 0x060003A3 RID: 931 RVA: 0x0000398C File Offset: 0x00001B8C
	private void Start()
	{
	}

	// Token: 0x060003A4 RID: 932 RVA: 0x0003A4B0 File Offset: 0x000386B0
	private void Update()
	{
		if (this.minigameController.Playable)
		{
			if (this.mover != null && this.mover.IsLocalPlayer)
			{
				CharacterMoverInput input = default(CharacterMoverInput);
				if (!this.mover.IsAI)
				{
					Vector2 axis = new Vector2(this.mover.RewiredPlayer.GetAxis(InputActions.Horizontal), 1f);
					input = new CharacterMoverInput(axis, false, this.jumper.IsLocalPlayer && this.jumper.RewiredPlayer.GetButton(InputActions.Accept));
				}
				input.NullInput(!this.minigameController.Playable || (GameManager.IsGamePaused && !this.mover.IsAI) || !GameManager.PollInput);
				this.cMover.CalculateVelocity(input, Time.deltaTime);
				this.cMover.DoMovement(Time.deltaTime);
			}
			if (this.jumper != null && this.jumper.IsLocalPlayer && !this.mover.IsLocalPlayer)
			{
				this.cMover.CalculateVelocity(new CharacterMoverInput(Vector2.zero, this.jumper.RewiredPlayer.GetButton(InputActions.Accept), false), Time.deltaTime);
				this.cMover.DoMovement(Time.deltaTime);
			}
		}
	}

	// Token: 0x060003A5 RID: 933 RVA: 0x00005F8C File Offset: 0x0000418C
	[NetRPC(false, NetRPCSecurity.ALL, NetRPCSecurity.ALL)]
	public void JumpRPC(NetPlayer sender)
	{
		this.Jump();
	}

	// Token: 0x060003A6 RID: 934 RVA: 0x00005F94 File Offset: 0x00004194
	private void Jump()
	{
		if (this.jumper != null && this.jumper.IsLocalPlayer)
		{
			base.SendRPC("JumpRPC", NetRPCDelivery.RELIABLE_ORDERED, Array.Empty<object>());
		}
	}

	// Token: 0x060003A7 RID: 935 RVA: 0x00005FBC File Offset: 0x000041BC
	public void RecieveMoverID(object o)
	{
		if (NetSystem.IsServer)
		{
			this.moverID.Value = (byte)o;
		}
		this.mover = GameManager.GetPlayerAt((int)((byte)o));
	}

	// Token: 0x060003A8 RID: 936 RVA: 0x00005FE7 File Offset: 0x000041E7
	public void RecieveJumperID(object o)
	{
		if (NetSystem.IsServer)
		{
			this.jumperID.Value = (byte)o;
		}
		this.jumper = GameManager.GetPlayerAt((int)((byte)o));
	}

	// Token: 0x040003BF RID: 959
	public Camera cam;

	// Token: 0x040003C0 RID: 960
	private GamePlayer mover;

	// Token: 0x040003C1 RID: 961
	private GamePlayer jumper;

	// Token: 0x040003C2 RID: 962
	private HorsesController minigameController;

	// Token: 0x040003C3 RID: 963
	private CharacterMover cMover;

	// Token: 0x040003C4 RID: 964
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.ALWAYS_SEND)]
	public NetVec2 position = new NetVec2(Vector2.zero);

	// Token: 0x040003C5 RID: 965
	[NetSend(-1, NetSendOwner.SERVER, NetSendFlags.NONE)]
	public NetVar<byte> moverID = new NetVar<byte>(0);

	// Token: 0x040003C6 RID: 966
	[NetSend(-1, NetSendOwner.SERVER, NetSendFlags.NONE)]
	public NetVar<byte> jumperID = new NetVar<byte>(0);
}
