using System;
using I2.Loc;
using UnityEngine;
using ZP.Net;

// Token: 0x02000274 RID: 628
public class SwiftShootersPlayer : CharacterBase
{
	// Token: 0x06001244 RID: 4676 RVA: 0x0000EC48 File Offset: 0x0000CE48
	public override void OnNetInitialize()
	{
		base.OnNetInitialize();
		if (!base.IsOwner)
		{
			this.n_targetPos.Recieve = new RecieveProxy(this.RecieveTargetPos);
			this.TargetPos = this.n_targetPos.Value;
		}
	}

	// Token: 0x06001245 RID: 4677 RVA: 0x0008CD74 File Offset: 0x0008AF74
	protected override void Start()
	{
		base.Start();
		this.m_minigameController = (SwiftShootersController)GameManager.Minigame;
		this.m_minigameController.AddPlayer(this);
		this.m_minigameController.Root.GetComponentInChildren<CameraFollow>().AddTarget(this);
		this.cameraShake = this.m_minigameController.Root.GetComponentInChildren<CameraShake>();
		this.playerAnim.Animator.SetFloat("ShotgunStrength", 1f);
		foreach (SwiftShooterTargetSpawner swiftShooterTargetSpawner in UnityEngine.Object.FindObjectsOfType<SwiftShooterTargetSpawner>())
		{
			if (swiftShooterTargetSpawner.PlayerIndex == (int)base.GamePlayer.GlobalID)
			{
				this.m_spawner = swiftShooterTargetSpawner;
			}
		}
		if (this.m_spawner != null)
		{
			this.m_spawner.SetColor(base.GamePlayer.Color.skinColor1);
		}
	}

	// Token: 0x06001246 RID: 4678 RVA: 0x0000EC80 File Offset: 0x0000CE80
	private void DoFireEffects()
	{
		this.m_recoilStartTime = Time.time;
		AudioSystem.PlayOneShot(this.m_fireClip, 0.5f, 0f, 1f);
		this.m_muzzleFlash.Stop();
		this.m_muzzleFlash.Play();
	}

	// Token: 0x06001247 RID: 4679 RVA: 0x0008CE44 File Offset: 0x0008B044
	private void Fire()
	{
		this.DoFireEffects();
		if (base.IsOwner)
		{
			this.m_lastShootTime = Time.time;
			int mask = LayerMask.GetMask(new string[]
			{
				"MinigameUtil1"
			});
			Vector3 position = this.m_gunMuzzle.position;
			Vector3 forward = this.m_gunMuzzle.forward;
			RaycastHit raycastHit;
			if (Physics.Raycast(position, forward, out raycastHit, 100f, mask, QueryTriggerInteraction.Ignore))
			{
				SwiftShooterTarget componentInParent = raycastHit.collider.gameObject.GetComponentInParent<SwiftShooterTarget>();
				TargetHitResult targetHitResult;
				if (componentInParent != null)
				{
					targetHitResult = componentInParent.HitTarget();
					base.SendRPC("RPCFire", NetRPCDelivery.RELIABLE_ORDERED, new object[]
					{
						(short)targetHitResult.score,
						(byte)targetHitResult.type,
						raycastHit.point,
						(byte)(componentInParent.TargetIndex + 1)
					});
				}
				else
				{
					targetHitResult = new TargetHitResult(-2, TargetHitResultType.Miss, 0f);
					base.SendRPC("RPCFire", NetRPCDelivery.RELIABLE_ORDERED, new object[]
					{
						(short)targetHitResult.score,
						(byte)targetHitResult.type,
						raycastHit.point,
						0
					});
				}
				if (NetSystem.IsServer)
				{
					this.Score += (short)targetHitResult.score;
				}
				this.ShowFireHitResult(targetHitResult, raycastHit.point);
				if (targetHitResult.type == TargetHitResultType.Bomb)
				{
					this.Explode();
				}
			}
		}
	}

	// Token: 0x06001248 RID: 4680 RVA: 0x0008CFBC File Offset: 0x0008B1BC
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	private void RPCFire(NetPlayer sender, short score, byte type, Vector3 position, byte targetIndex)
	{
		this.DoFireEffects();
		this.ShowFireHitResult(new TargetHitResult((int)score, (TargetHitResultType)type, 0f), position);
		if (type == 4)
		{
			this.Explode();
		}
		if (targetIndex != 0)
		{
			SwiftShooterTarget target = this.m_spawner.GetTarget((int)(targetIndex - 1));
			if (target != null)
			{
				target.BreakTarget();
			}
		}
		if (NetSystem.IsServer)
		{
			this.Score += score;
		}
	}

	// Token: 0x06001249 RID: 4681 RVA: 0x0008D028 File Offset: 0x0008B228
	private void Explode()
	{
		UnityEngine.Object.Instantiate<GameObject>(this.m_explosionEffectPfb, base.transform.position - Vector3.up * 0.8f, Quaternion.identity);
		if (Settings.BloodEffects)
		{
			UnityEngine.Object.Instantiate<GameObject>(this.m_bloodEffectPfb, base.transform.position, Quaternion.LookRotation(Vector3.up));
		}
		AudioSystem.PlayOneShot(this.m_explosionClip, 1f, 0f, 1f);
		this.cameraShake.AddShake(0.15f);
		this.playerAnim.FireDeathTrigger();
		this.m_unstunnedTime = Time.time + 2.6f;
		this.m_laser.gameObject.SetActive(false);
	}

	// Token: 0x0600124A RID: 4682 RVA: 0x0008D0E4 File Offset: 0x0008B2E4
	private void ShowFireHitResult(TargetHitResult result, Vector3 position)
	{
		if (this.m_minigameController == null)
		{
			return;
		}
		Vector3 position2 = position + Vector3.up * 0.65f;
		switch (result.type)
		{
		case TargetHitResultType.Excellent:
			GameManager.UIController.SpawnWorldText("+" + result.score.ToString() + " " + LocalizationManager.GetTranslation("Excellent", true, 0, true, false, null, null, true), position2, 3f, WorldTextType.SwiftShootersExcellent, 0f, this.m_minigameController.MinigameCamera);
			return;
		case TargetHitResultType.Good:
			GameManager.UIController.SpawnWorldText("+" + result.score.ToString() + " " + LocalizationManager.GetTranslation("Good", true, 0, true, false, null, null, true), position2, 3f, WorldTextType.SwiftShootersGood, 0f, this.m_minigameController.MinigameCamera);
			return;
		case TargetHitResultType.Bad:
			GameManager.UIController.SpawnWorldText("+" + result.score.ToString() + " " + LocalizationManager.GetTranslation("Slow", true, 0, true, false, null, null, true), position2, 3f, WorldTextType.SwiftShootersBad, 0f, this.m_minigameController.MinigameCamera);
			return;
		case TargetHitResultType.Miss:
			GameManager.UIController.SpawnWorldText(result.score.ToString() + " " + LocalizationManager.GetTranslation("Miss", true, 0, true, false, null, null, true), position2, 3f, WorldTextType.SwiftShootersMiss, 0f, this.m_minigameController.MinigameCamera);
			return;
		case TargetHitResultType.Bomb:
			GameManager.UIController.SpawnWorldText(result.score.ToString() + " " + LocalizationManager.GetTranslation("Bang", true, 0, true, false, null, null, true), position2, 3f, WorldTextType.SwiftShootersBomb, 0f, this.m_minigameController.MinigameCamera);
			return;
		default:
			return;
		}
	}

	// Token: 0x0600124B RID: 4683 RVA: 0x0008D2B4 File Offset: 0x0008B4B4
	private void Update()
	{
		Vector3 position = this.m_gunMuzzle.position;
		Vector3 forward = this.m_gunMuzzle.forward;
		RaycastHit raycastHit;
		if (Physics.Raycast(position, forward, out raycastHit, 100f))
		{
			this.m_lineRenderer.SetPosition(1, new Vector3(0f, 0f, raycastHit.distance));
		}
		if (this.m_minigameController.Playable)
		{
			this.m_curTargetOffset = Vector3.MoveTowards(this.m_curTargetOffset, this.m_targetOffsetTarget, 280f * Time.deltaTime);
			if (Time.time > this.m_unstunnedTime)
			{
				this.m_laser.gameObject.SetActive(true);
			}
			if (base.IsOwner && Time.time > this.m_unstunnedTime)
			{
				if (!this.player.IsAI)
				{
					float axis = base.GamePlayer.RewiredPlayer.GetAxis(InputActions.Horizontal);
					bool flag = false;
					bool flag2 = false;
					if (axis < -this.m_sensitivity)
					{
						flag = true;
					}
					if (axis > this.m_sensitivity)
					{
						flag2 = true;
					}
					if (flag && !flag2)
					{
						this.TargetPos = 0;
					}
					else if (flag2 && !flag)
					{
						this.TargetPos = 2;
					}
					else
					{
						this.TargetPos = 1;
					}
				}
				else
				{
					if (this.m_aiTarget == null)
					{
						this.SetNewAITarget();
					}
					else if (!this.m_aiTarget.IsTargetUp())
					{
						this.m_aiTarget = null;
						this.SetNewAITarget();
					}
					if (this.m_aiTarget != null && Time.time >= this.m_aiAimWaitTime)
					{
						this.TargetPos = (byte)this.m_aiTarget.TargetIndex;
					}
				}
				if (Time.time - this.m_lastShootTime > this.m_shootCooldown)
				{
					if (this.player.IsAI)
					{
						if (this.m_aiFireQueued && Time.time > this.m_aiFireWaitTime && this.m_aiTarget != null && (this.m_aiTarget.GetTargetType() != TargetType.Bomb || this.m_aiShouldShootBomb))
						{
							this.m_aiFireQueued = false;
							this.m_aiTarget = null;
							this.Fire();
							return;
						}
					}
					else if (this.player.RewiredPlayer.GetButtonDown(InputActions.Accept) || this.player.RewiredPlayer.GetButtonDown(InputActions.UseItemShoot))
					{
						this.Fire();
					}
				}
			}
		}
	}

	// Token: 0x0600124C RID: 4684 RVA: 0x0008D4DC File Offset: 0x0008B6DC
	private void SetNewAITarget()
	{
		this.m_aiFireQueued = false;
		this.m_aiShouldShootBomb = false;
		for (int i = 0; i < 3; i++)
		{
			SwiftShooterTarget target = this.m_spawner.GetTarget(i);
			if (target.GetTargetType() != TargetType.None)
			{
				this.m_aiTarget = target;
				break;
			}
		}
		if (this.m_aiTarget != null)
		{
			this.m_aiAimWaitTime = Time.time + UnityEngine.Random.Range(0.1f, 0.8f);
			this.m_aiFireWaitTime = this.m_aiAimWaitTime + UnityEngine.Random.Range(0.1f, 0.25f);
			this.m_aiShouldShootBomb = (UnityEngine.Random.value > 0.8f);
			this.m_aiFireQueued = true;
		}
	}

	// Token: 0x0600124D RID: 4685 RVA: 0x0008D580 File Offset: 0x0008B780
	public void LateUpdate()
	{
		int num = Mathf.Max(this.m_bones.Length, this.m_boneOffsets.Length);
		for (int i = 0; i < num; i++)
		{
			this.m_bones[i].localRotation = Quaternion.Euler(this.m_bones[i].localRotation.eulerAngles + this.m_boneOffsets[i]);
		}
		this.m_targetOffsetBone.localRotation = Quaternion.Euler(this.m_targetOffsetBone.localRotation.eulerAngles + this.m_curTargetOffset);
		float num2 = Time.time - this.m_recoilStartTime;
		if (num2 < this.m_fireRecoilAnimLength)
		{
			float d = this.m_fireRecoilAnim.Evaluate(num2 / this.m_fireRecoilAnimLength);
			Vector3 euler = this.m_recoilRotationOffset * d;
			this.m_recoilBone.localRotation *= Quaternion.Euler(euler);
		}
	}

	// Token: 0x17000193 RID: 403
	// (get) Token: 0x0600124E RID: 4686 RVA: 0x0000ECBD File Offset: 0x0000CEBD
	// (set) Token: 0x0600124F RID: 4687 RVA: 0x0008D66C File Offset: 0x0008B86C
	public byte TargetPos
	{
		get
		{
			return this.n_targetPos.Value;
		}
		set
		{
			if (value != this.n_targetPos.Value)
			{
				this.m_targetOffsetVel = Vector3.zero;
				this.m_targetChangeTime = Time.time;
			}
			if (value >= 0 && (int)value < this.m_targetOffsets.Length)
			{
				this.m_targetOffsetTarget = this.m_targetOffsets[(int)value];
			}
			if (this.m_spawner != null)
			{
				this.m_spawner.SetTargetIndex((int)value);
			}
			this.n_targetPos.Value = value;
		}
	}

	// Token: 0x06001250 RID: 4688 RVA: 0x0000ECCA File Offset: 0x0000CECA
	public void RecieveTargetPos(object _pos)
	{
		this.TargetPos = (byte)_pos;
	}

	// Token: 0x04001325 RID: 4901
	[SerializeField]
	protected Animator m_anim;

	// Token: 0x04001326 RID: 4902
	[SerializeField]
	protected Transform[] m_bones;

	// Token: 0x04001327 RID: 4903
	[SerializeField]
	protected Vector3[] m_boneOffsets;

	// Token: 0x04001328 RID: 4904
	[SerializeField]
	private Transform m_targetOffsetBone;

	// Token: 0x04001329 RID: 4905
	[SerializeField]
	private Vector3[] m_targetOffsets;

	// Token: 0x0400132A RID: 4906
	[SerializeField]
	private AnimationCurve m_targetAnimRemap;

	// Token: 0x0400132B RID: 4907
	[SerializeField]
	private AudioClip m_fireClip;

	// Token: 0x0400132C RID: 4908
	[SerializeField]
	private ParticleSystem m_muzzleFlash;

	// Token: 0x0400132D RID: 4909
	[SerializeField]
	private GameObject m_explosionEffectPfb;

	// Token: 0x0400132E RID: 4910
	[SerializeField]
	private GameObject m_bloodEffectPfb;

	// Token: 0x0400132F RID: 4911
	[SerializeField]
	private AudioClip m_explosionClip;

	// Token: 0x04001330 RID: 4912
	[Header("Recoil")]
	[SerializeField]
	private AnimationCurve m_fireRecoilAnim;

	// Token: 0x04001331 RID: 4913
	[SerializeField]
	private float m_fireRecoilAnimLength = 0.25f;

	// Token: 0x04001332 RID: 4914
	[SerializeField]
	private Vector3 m_recoilRotationOffset = Vector3.zero;

	// Token: 0x04001333 RID: 4915
	[SerializeField]
	private Transform m_recoilBone;

	// Token: 0x04001334 RID: 4916
	[SerializeField]
	private Transform m_gunMuzzle;

	// Token: 0x04001335 RID: 4917
	[SerializeField]
	private GameObject m_laser;

	// Token: 0x04001336 RID: 4918
	[SerializeField]
	private LineRenderer m_lineRenderer;

	// Token: 0x04001337 RID: 4919
	private float m_recoilStartTime;

	// Token: 0x04001338 RID: 4920
	private Vector3 m_curTargetOffset = Vector3.zero;

	// Token: 0x04001339 RID: 4921
	private Vector3 m_targetOffsetTarget = Vector3.zero;

	// Token: 0x0400133A RID: 4922
	private Vector3 m_targetOffsetVel = Vector3.zero;

	// Token: 0x0400133B RID: 4923
	private float m_targetOffsetSmoothTime = 0.05f;

	// Token: 0x0400133C RID: 4924
	private float m_targetChangeTime;

	// Token: 0x0400133D RID: 4925
	private SwiftShootersController m_minigameController;

	// Token: 0x0400133E RID: 4926
	private CameraShake cameraShake;

	// Token: 0x0400133F RID: 4927
	private float m_sensitivity = 0.3f;

	// Token: 0x04001340 RID: 4928
	private SwiftShooterTargetSpawner m_spawner;

	// Token: 0x04001341 RID: 4929
	private float m_lastShootTime;

	// Token: 0x04001342 RID: 4930
	private float m_shootCooldown = 0.25f;

	// Token: 0x04001343 RID: 4931
	private float m_unstunnedTime;

	// Token: 0x04001344 RID: 4932
	private SwiftShooterTarget m_aiTarget;

	// Token: 0x04001345 RID: 4933
	private float m_aiAimWaitTime;

	// Token: 0x04001346 RID: 4934
	private float m_aiFireWaitTime;

	// Token: 0x04001347 RID: 4935
	private bool m_aiFireQueued;

	// Token: 0x04001348 RID: 4936
	private bool m_aiShouldShootBomb;

	// Token: 0x04001349 RID: 4937
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.NONE)]
	private NetVar<byte> n_targetPos = new NetVar<byte>(1);
}
