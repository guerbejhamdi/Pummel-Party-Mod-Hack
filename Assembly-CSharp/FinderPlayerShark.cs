using System;
using System.Collections;
using UnityEngine;
using ZP.Net;

// Token: 0x02000061 RID: 97
public class FinderPlayerShark : CharacterBase
{
	// Token: 0x060001D0 RID: 464 RVA: 0x000335A8 File Offset: 0x000317A8
	public override void OnNetInitialize()
	{
		if (base.IsOwner)
		{
			this.position.Value = base.transform.position;
		}
		else
		{
			this.position.Recieve = new RecieveProxy(this.RecievePosition);
			this.interpolator = new Interpolator(base.transform, Interpolator.InterpolationType.PositionTransform);
		}
		base.OnNetInitialize();
	}

	// Token: 0x060001D1 RID: 465 RVA: 0x00004B86 File Offset: 0x00002D86
	protected override void Start()
	{
		base.Start();
		this.minigameController = (FinderController)GameManager.Minigame;
		this.minigameController.AddPlayer(this);
	}

	// Token: 0x060001D2 RID: 466 RVA: 0x00033604 File Offset: 0x00031804
	private void Update()
	{
		if (this.minigameController.GameState >= FinderGameState.PlayGame)
		{
			this.targeter.SetActive(true);
			if (base.IsOwner)
			{
				if (!base.GamePlayer.IsAI)
				{
					Vector3 zero = new Vector3(this.player.RewiredPlayer.GetAxis(InputActions.Horizontal), 0f, this.player.RewiredPlayer.GetAxis(InputActions.Vertical));
					if (this.isAttacking)
					{
						zero = Vector3.zero;
					}
					this.velocity = Vector3.MoveTowards(this.velocity, zero, 8f * Time.deltaTime);
					this.targeter.transform.position += this.velocity * 12.5f * Time.deltaTime;
					if (this.player.RewiredPlayer.GetButtonDown(InputActions.Action1) && this.targetID != 255)
					{
						if (this.sharkAttackCooldown.Elapsed(true))
						{
							base.StartCoroutine(this.SharkAttack(this.targetID));
						}
						else
						{
							AudioSystem.PlayOneShot(this.cannotAttackSound, 1f, 0f, 1f);
						}
					}
				}
				else
				{
					if (this.AITarget == null || (this.AITargetID >= 200 && ((FinderPlayer)this.minigameController.players[(int)(this.AITargetID - 200)]).escaped))
					{
						int i = 0;
						int num = 1000;
						int maxValue = this.minigameController.fakePlayers.Count + this.minigameController.GetPlayerCount();
						int num2 = 0;
						while (i < num)
						{
							i++;
							num2 = GameManager.rand.Next(0, maxValue);
							if (num2 < this.minigameController.fakePlayers.Count)
							{
								if (!this.minigameController.fakePlayers[num2].isDead)
								{
									break;
								}
							}
							else
							{
								CharacterBase player = this.minigameController.GetPlayer(num2 - this.minigameController.fakePlayers.Count);
								if (!player.IsDead && !(player.GetType() == typeof(FinderPlayerShark)) && !((FinderPlayer)player).escaped)
								{
									break;
								}
							}
						}
						this.AITarget = ((num2 < this.minigameController.fakePlayers.Count) ? this.minigameController.fakePlayers[num2].transform : this.minigameController.players[num2 - this.minigameController.fakePlayers.Count].transform);
						this.AITargetID = (byte)((num2 < this.minigameController.fakePlayers.Count) ? num2 : (200 + (num2 - this.minigameController.fakePlayers.Count)));
					}
					Vector3 target = (this.AITarget == null) ? Vector3.zero : (this.AITarget.position - this.targeter.transform.position).normalized;
					target.y = 0f;
					if (this.isAttacking)
					{
						target = Vector3.zero;
					}
					this.velocity = Vector3.MoveTowards(this.velocity, target, 8f * Time.deltaTime);
					this.targeter.transform.position += this.velocity * 12.5f * Time.deltaTime;
					if (this.sharkAttackCooldown.Elapsed(true) && this.AITarget != null && (this.AITarget.transform.position - this.targeter.transform.position).sqrMagnitude < 3f)
					{
						this.AITarget = null;
						base.StartCoroutine(this.SharkAttack(this.AITargetID));
					}
				}
				this.position.Value = this.targeter.transform.position;
			}
			else
			{
				this.interpolator.Update();
			}
			if (!this.isAttacking)
			{
				float num3 = float.MaxValue;
				byte b = byte.MaxValue;
				float num4 = 20.25f;
				Vector3 b2 = this.targeter.transform.position;
				for (int j = 0; j < this.minigameController.fakePlayers.Count; j++)
				{
					if (!(this.minigameController.fakePlayers[j] == null) && !this.minigameController.fakePlayers[j].isDead)
					{
						Vector3 vector = this.minigameController.fakePlayers[j].transform.position - b2;
						Vector3 normalized = vector.normalized;
						float sqrMagnitude = vector.sqrMagnitude;
						if (sqrMagnitude < num4 && sqrMagnitude < num3)
						{
							b = (byte)j;
							num3 = sqrMagnitude;
						}
					}
				}
				for (int k = 0; k < this.minigameController.players.Count; k++)
				{
					if (!this.minigameController.players[k].IsDead && this.minigameController.players[k].GetType() != typeof(FinderPlayerShark))
					{
						Vector3 vector2 = this.minigameController.players[k].transform.position - b2;
						Vector3 normalized2 = vector2.normalized;
						float sqrMagnitude2 = vector2.sqrMagnitude;
						if (sqrMagnitude2 < num4 && sqrMagnitude2 < num3)
						{
							b = (byte)(200 + k);
							num3 = sqrMagnitude2;
						}
					}
				}
				Transform transform = (b >= 200) ? ((b == byte.MaxValue) ? null : this.minigameController.players[(int)(b - 200)].transform) : this.minigameController.fakePlayers[(int)b].transform;
				if (this.targetTransform != transform)
				{
					this.SetSources(this.targetTransform, false);
					this.SetSources(transform, true);
				}
				this.targetTransform = transform;
				this.targetID = b;
			}
		}
	}

	// Token: 0x060001D3 RID: 467 RVA: 0x00033C3C File Offset: 0x00031E3C
	private void SetSources(Transform t, bool state)
	{
		if (t == null)
		{
			return;
		}
		OutlineSource[] componentsInChildren = t.gameObject.GetComponentsInChildren<OutlineSource>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].enabled = state;
			componentsInChildren[i].outlineColor = Color.red;
		}
	}

	// Token: 0x060001D4 RID: 468 RVA: 0x00004BAA File Offset: 0x00002DAA
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCSharkAttack(NetPlayer sender, byte targetID)
	{
		base.StartCoroutine(this.SharkAttack(targetID));
	}

	// Token: 0x060001D5 RID: 469 RVA: 0x00004BBA File Offset: 0x00002DBA
	private IEnumerator SharkAttack(byte targetID)
	{
		if (base.IsOwner)
		{
			base.SendRPC("RPCSharkAttack", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				targetID
			});
		}
		Debug.Log("Shark Attack: " + targetID.ToString());
		this.isAttacking = true;
		this.isPlayer = (targetID >= 200);
		this.fp = (FinderPlayer)(this.isPlayer ? this.minigameController.players[(int)(targetID - 200)] : null);
		this.fpf = ((!this.isPlayer) ? this.minigameController.fakePlayers[(int)targetID] : null);
		if (this.isPlayer && this.fp.IsOwner)
		{
			this.fp.SharkAttack = true;
		}
		else if (!this.isPlayer)
		{
			this.fpf.SharkAttack = true;
		}
		yield return new WaitForSeconds(0.5f);
		AudioSystem.PlayOneShot(this.sharkBite, 0.15f, 0f, 1f);
		this.sharkRoot.SetActive(true);
		this.sharkRoot.transform.position = (this.isPlayer ? this.fp.transform.position : this.fpf.transform.position);
		this.sharkRoot.transform.rotation = Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f);
		this.sharkAnimator.SetTrigger("SharkAttack");
		if (this.isPlayer && NetSystem.IsServer)
		{
			this.Score += 10;
		}
		yield return null;
		yield return new WaitForSeconds(1f);
		this.isAttacking = false;
		yield break;
	}

	// Token: 0x060001D6 RID: 470 RVA: 0x00033C84 File Offset: 0x00031E84
	public void AttackHit()
	{
		if (this.isPlayer)
		{
			Vector3 normalized = (this.fp.transform.position - this.sharkRoot.transform.Find("Shark").position).normalized;
			this.fp.KillPlayer(normalized);
			return;
		}
		this.fpf.Kill();
	}

	// Token: 0x060001D7 RID: 471 RVA: 0x00004BD0 File Offset: 0x00002DD0
	public void RecievePosition(object _pos)
	{
		this.interpolator.NewPosition(_pos);
	}

	// Token: 0x04000229 RID: 553
	public GameObject targeter;

	// Token: 0x0400022A RID: 554
	public GameObject sharkRoot;

	// Token: 0x0400022B RID: 555
	public Animator sharkAnimator;

	// Token: 0x0400022C RID: 556
	public AudioClip cannotAttackSound;

	// Token: 0x0400022D RID: 557
	public AudioClip sharkBite;

	// Token: 0x0400022E RID: 558
	private FinderController minigameController;

	// Token: 0x0400022F RID: 559
	private Interpolator interpolator;

	// Token: 0x04000230 RID: 560
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.ALWAYS_SEND)]
	public NetVec3 position = new NetVec3(Vector3.zero);

	// Token: 0x04000231 RID: 561
	private ActionTimer lastTargetChange = new ActionTimer(0.15f);

	// Token: 0x04000232 RID: 562
	private ActionTimer sharkAttackCooldown = new ActionTimer(3.5f);

	// Token: 0x04000233 RID: 563
	private Transform targetTransform;

	// Token: 0x04000234 RID: 564
	private Vector3 velocity = Vector3.zero;

	// Token: 0x04000235 RID: 565
	private byte targetID = byte.MaxValue;

	// Token: 0x04000236 RID: 566
	private byte AITargetID;

	// Token: 0x04000237 RID: 567
	private Transform AITarget;

	// Token: 0x04000238 RID: 568
	private ActionTimer sharkAIAttackCooldown = new ActionTimer(0.75f, 1.25f);

	// Token: 0x04000239 RID: 569
	private bool isPlayer;

	// Token: 0x0400023A RID: 570
	private bool isAttacking;

	// Token: 0x0400023B RID: 571
	private FinderPlayer fp;

	// Token: 0x0400023C RID: 572
	private FinderPlayerFake fpf;
}
