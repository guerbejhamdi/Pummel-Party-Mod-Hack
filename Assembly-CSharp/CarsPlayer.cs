using System;
using System.Collections.Generic;
using Rewired;
using UnityEngine;
using ZP.Net;

// Token: 0x02000190 RID: 400
public class CarsPlayer : CharacterBase
{
	// Token: 0x06000B72 RID: 2930 RVA: 0x000618F8 File Offset: 0x0005FAF8
	public override void OnNetInitialize()
	{
		base.OnNetInitialize();
		this.minigameController = (CarsController)GameManager.Minigame;
		bool flag = true;
		using (List<GamePlayer>.Enumerator enumerator = GameManager.PlayerList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (!enumerator.Current.IsAI)
				{
					flag = false;
					break;
				}
			}
		}
		if (base.IsOwner)
		{
			List<GamePlayer> list = GameManager.GetLocalNonAIPlayers();
			if (!this.player.IsAI || flag)
			{
				if (flag)
				{
					list = GameManager.GetLocalAIPlayers();
				}
				this.minigameController.minigameCameras.Add(this.cam);
				if (list.Count > 1)
				{
					if (!flag)
					{
						this.cam.rect = base.GetPlayerSplitScreenRect(this.player);
					}
					else
					{
						this.cam.rect = base.GetPlayerSplitScreenRectWithAI(this.player);
					}
				}
				if (list.Count > 0 && list[0] == this.player)
				{
					this.audioListener.enabled = true;
				}
			}
			else if (list.Count == 0)
			{
				this.minigameController.minigameCameras.Add(this.cam);
			}
			this.position.Value = this.rollyCar.transform.position;
			this.rotation.Value = this.rollyCar.root.transform.rotation.eulerAngles;
		}
		else
		{
			this.position.Recieve = new RecieveProxy(this.RecievePosition);
			this.interpolator = new Interpolator(this.rollyCar.transform, Interpolator.InterpolationType.PositionTransform);
		}
		Material material = new Material(this.bodyRenderer.sharedMaterial);
		material.SetColor("_ReplaceColor", this.player.Color.skinColor1);
		this.bodyRenderer.sharedMaterial = material;
		if (!base.IsOwner)
		{
			this.rollyCar.rb.isKinematic = true;
		}
	}

	// Token: 0x06000B73 RID: 2931 RVA: 0x0000B3F0 File Offset: 0x000095F0
	public void RecievePosition(object _pos)
	{
		this.interpolator.NewPosition(_pos);
	}

	// Token: 0x06000B74 RID: 2932 RVA: 0x0000B3FE File Offset: 0x000095FE
	public override void OnOwnerChanged()
	{
		bool isServer = NetSystem.IsServer;
		base.OnOwnerChanged();
	}

	// Token: 0x06000B75 RID: 2933 RVA: 0x0000B40C File Offset: 0x0000960C
	protected override void Start()
	{
		base.Start();
		this.minigameController.AddPlayer(this);
	}

	// Token: 0x06000B76 RID: 2934 RVA: 0x00061AF0 File Offset: 0x0005FCF0
	private void Update()
	{
		if (base.IsOwner)
		{
			this.playable = (this.minigameController != null && this.minigameController.State >= MinigameControllerState.FadeIn);
			if (this.minigameController.Playable)
			{
				if (!this.player.IsAI)
				{
					this.vertical = this.player.RewiredPlayer.GetAxis(InputActions.Vertical);
					this.horizontal = this.player.RewiredPlayer.GetAxis(InputActions.Horizontal);
					this.isJoystick = false;
					if (this.player.RewiredPlayer.controllers.GetLastActiveController() != null)
					{
						this.isJoystick = (this.player.RewiredPlayer.controllers.GetLastActiveController().type == ControllerType.Joystick);
						if (this.isJoystick)
						{
							this.vertical = (this.player.RewiredPlayer.GetButton(InputActions.Accept) ? 1f : (this.player.RewiredPlayer.GetButton(InputActions.Cancel) ? -1f : 0f));
						}
					}
				}
				else
				{
					Vector3 vector = this.minigameController.navPoints[this.curNode].position - this.rollyCar.transform.position;
					if (vector.sqrMagnitude < this.reachSqrDistance)
					{
						this.curNode++;
						this.lastNodeReachTime = Time.time;
						if (this.curNode >= this.minigameController.navPoints.Length)
						{
							this.curNode = 0;
						}
					}
					this.isJoystick = true;
					this.vertical = 0.9f;
					float num = Vector3.Dot(this.rollyCar.root.right, vector.normalized);
					this.horizontal = ((num > 0f) ? 1f : -1f);
					if (Time.time - this.lastNodeReachTime > 3f)
					{
						Vector3 vector2 = this.minigameController.navPoints[this.curNode].position;
						vector2.y = 0.95f;
						this.rollyCar.transform.position = vector2;
						this.lastNodeReachTime = Time.time;
					}
				}
			}
			else
			{
				this.vertical = 0f;
				this.horizontal = 0f;
			}
			this.UpdateNetVals();
			this.drifting.Value = this.rollyCar.Drifting;
			return;
		}
		this.UpdateNetVals();
		this.rollyCar.SetDrift(this.drifting.Value);
	}

	// Token: 0x06000B77 RID: 2935 RVA: 0x00061D80 File Offset: 0x0005FF80
	private void UpdateNetVals()
	{
		if (base.IsOwner)
		{
			this.rotation.Value = this.rollyCar.root.transform.localRotation.eulerAngles;
			this.position.Value = this.rollyCar.transform.localPosition;
			return;
		}
		this.interpolator.Update();
		this.rollyCar.root.transform.localPosition = this.rollyCar.transform.localPosition;
		this.rollyCar.root.transform.localRotation = Quaternion.Euler(this.rotation.Value);
	}

	// Token: 0x06000B78 RID: 2936 RVA: 0x0000B420 File Offset: 0x00009620
	private void FixedUpdate()
	{
		if (base.IsOwner)
		{
			this.rollyCar.DoUpdate(this.vertical, this.horizontal, this.isJoystick, this.playable);
		}
		this.rollyCar.SetEngine(this.playable);
	}

	// Token: 0x17000103 RID: 259
	// (get) Token: 0x06000B79 RID: 2937 RVA: 0x0000B45E File Offset: 0x0000965E
	// (set) Token: 0x06000B7A RID: 2938 RVA: 0x0000B466 File Offset: 0x00009666
	public bool Finished { get; set; }

	// Token: 0x06000B7B RID: 2939 RVA: 0x00061E30 File Offset: 0x00060030
	public void PassCollision(Collider other)
	{
		if (this.Finished)
		{
			return;
		}
		if (other.gameObject.name.Equals("FinishCollider") && this.reachedHalfwayPoint)
		{
			this.reachedHalfwayPoint = false;
			AudioSystem.PlayOneShot(this.lapSound, 0.3f, 0f, 1f);
			this.lapCount++;
			if (this.lapCount >= this.minigameController.laps)
			{
				if (NetSystem.IsServer)
				{
					int num = 0;
					for (int i = 0; i < this.minigameController.GetPlayerCount(); i++)
					{
						if (((CarsPlayer)this.minigameController.GetPlayer(i)).Finished)
						{
							num++;
						}
					}
					this.Score = (short)(this.minigameController.GetPlayerCount() - num);
					if (num == 0)
					{
						this.minigameController.ShowWinnerText(this.player);
					}
				}
				this.Finished = true;
				return;
			}
		}
		else if (other.gameObject.name.Equals("HalfwayCollider"))
		{
			this.reachedHalfwayPoint = true;
		}
	}

	// Token: 0x04000A76 RID: 2678
	public RollyCar rollyCar;

	// Token: 0x04000A77 RID: 2679
	public RollyCarFollowCam rollyCam;

	// Token: 0x04000A78 RID: 2680
	public Camera cam;

	// Token: 0x04000A79 RID: 2681
	public MeshRenderer bodyRenderer;

	// Token: 0x04000A7A RID: 2682
	public AudioListener audioListener;

	// Token: 0x04000A7B RID: 2683
	private CarsController minigameController;

	// Token: 0x04000A7C RID: 2684
	private CameraShake cameraShake;

	// Token: 0x04000A7D RID: 2685
	private bool reachedHalfwayPoint;

	// Token: 0x04000A7E RID: 2686
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.ALWAYS_SEND)]
	public NetVec3 position = new NetVec3(Vector3.zero);

	// Token: 0x04000A7F RID: 2687
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.ALWAYS_SEND)]
	public NetVec3 rotation = new NetVec3(Vector3.zero);

	// Token: 0x04000A80 RID: 2688
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.ALWAYS_SEND)]
	public NetVar<bool> drifting = new NetVar<bool>(false);

	// Token: 0x04000A81 RID: 2689
	private Interpolator interpolator;

	// Token: 0x04000A82 RID: 2690
	private float vertical;

	// Token: 0x04000A83 RID: 2691
	private float horizontal;

	// Token: 0x04000A84 RID: 2692
	private bool isJoystick;

	// Token: 0x04000A85 RID: 2693
	private bool playable;

	// Token: 0x04000A86 RID: 2694
	private int curNode;

	// Token: 0x04000A87 RID: 2695
	private float reachSqrDistance = 200f;

	// Token: 0x04000A88 RID: 2696
	private float lastNodeReachTime;

	// Token: 0x04000A89 RID: 2697
	public AnimationCurve distSpeed;

	// Token: 0x04000A8A RID: 2698
	public AudioClip lapSound;

	// Token: 0x04000A8B RID: 2699
	private int lapCount;
}
