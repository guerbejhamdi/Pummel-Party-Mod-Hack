using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using ZP.Net;
using ZP.Utility;

// Token: 0x02000261 RID: 609
public class SpellingPlayer : Movement1
{
	// Token: 0x060011D7 RID: 4567 RVA: 0x0008A8D8 File Offset: 0x00088AD8
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
		if (!NetSystem.IsServer)
		{
			this.stunned.Recieve = new RecieveProxy(this.StunnedRecieve);
			this.score.Recieve = new RecieveProxy(base.ScoreRecieve);
		}
		if (!base.IsOwner)
		{
			this.n_wordProgress.Recieve = new RecieveProxy(this.WordProgressRecieve);
		}
		if (!base.IsOwner)
		{
			base.GetComponent<CharacterController>().enabled = false;
		}
		if (!this.player.IsAI || !base.IsOwner)
		{
			base.GetComponent<NavMeshAgent>().enabled = false;
		}
		if (this.player.IsAI && base.IsOwner)
		{
			this.mover.IsAI = true;
		}
	}

	// Token: 0x060011D8 RID: 4568 RVA: 0x0008A9F0 File Offset: 0x00088BF0
	public override void OnOwnerChanged()
	{
		if (NetSystem.IsServer)
		{
			base.GetComponent<CharacterController>().enabled = true;
			base.GetComponent<NavMeshAgent>().enabled = true;
			this.mover.IsAI = true;
			this.mover.SetForwardVector(Vector3.forward);
		}
		base.OnOwnerChanged();
	}

	// Token: 0x060011D9 RID: 4569 RVA: 0x0008AA40 File Offset: 0x00088C40
	public void Awake()
	{
		this.m_nextLetterOffset = UnityEngine.Random.Range(0.75f, 2f);
		this.m_aiWait = UnityEngine.Random.Range(0.5f, 1f);
		this.m_buttonTriggerLayer = LayerMask.GetMask(new string[]
		{
			"MinigameUtil1"
		});
		base.InitializeController();
	}

	// Token: 0x060011DA RID: 4570 RVA: 0x0000E8B2 File Offset: 0x0000CAB2
	public void OnDestroy()
	{
		if (this.m_wordBar != null)
		{
			UnityEngine.Object.Destroy(this.m_wordBar.gameObject);
		}
	}

	// Token: 0x060011DB RID: 4571 RVA: 0x0008AA98 File Offset: 0x00088C98
	protected override void Start()
	{
		base.Start();
		this.minigameController = (SpellingController)GameManager.Minigame;
		this.minigameController.AddPlayer(this);
		this.minigameController.Root.GetComponentInChildren<CameraFollow>().AddTarget(this);
		this.playerAnim.RegisterListener(new AnimationEventListener(this.PunchImpactEvent), AnimationEventType.PunchImpact);
		this.cameraShake = this.minigameController.Root.GetComponentInChildren<CameraShake>();
		if (!this.player.IsAI)
		{
			this.mover.SetForwardVector(Vector3.left);
		}
		this.CreateWordBar();
	}

	// Token: 0x060011DC RID: 4572 RVA: 0x0008AB30 File Offset: 0x00088D30
	private void CreateWordBar()
	{
		if (this.m_wordBar != null)
		{
			return;
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.spellingWordBarPfb);
		GameManager.UIController.AddCustomUIObject(gameObject);
		this.m_wordBar = gameObject.GetComponent<UISpellingWordBar>();
		this.m_wordBar.Initialize(base.transform, 1.5f, this.minigameController.MinigameCamera);
	}

	// Token: 0x060011DD RID: 4573 RVA: 0x0008AB90 File Offset: 0x00088D90
	private void Update()
	{
		base.UpdateController();
		if (base.IsOwner && this.minigameController.Playable)
		{
			if (Time.time - this.lastPressTime > this.pressInterval)
			{
				if (base.GamePlayer.IsAI)
				{
					if (this.m_targetButton != null && !this.m_aiPressInProgress)
					{
						Vector2 a = new Vector2(base.transform.position.x, base.transform.position.z);
						Vector2 b = new Vector2(this.m_targetButton.transform.position.x, this.m_targetButton.transform.position.z);
						if (Vector2.Distance(a, b) < 0.3f)
						{
							this.m_aiPressInProgress = true;
							base.StartCoroutine(this.AIPressButton());
						}
					}
				}
				else if (this.player.RewiredPlayer.GetButtonDown(InputActions.Accept))
				{
					this.PressButton();
				}
			}
			if (Time.time - this.lastPunchTime > this.punchInterval)
			{
				if (base.GamePlayer.IsAI)
				{
					if (this.curAIState != SpellingPlayer.SpellingAIState.Hiding && this.agent.remainingDistance < 0.5f)
					{
						this.StartPunch((double)UnityEngine.Random.value > 0.5);
					}
				}
				else if (!GameManager.IsGamePaused && base.GamePlayer.RewiredPlayer.GetButtonDown(InputActions.UseItemShoot))
				{
					this.StartPunch((double)UnityEngine.Random.value > 0.5);
					Debug.Log("Punch1");
				}
			}
		}
		if (NetSystem.IsServer && this.Stunned && Time.time - this.stunStartTime > this.stunLength)
		{
			this.Stunned = false;
		}
		this.playerAnim.Stunned = this.stunned.Value;
		this.stun_fx_obj.SetActive(this.stunned.Value);
	}

	// Token: 0x060011DE RID: 4574 RVA: 0x0000E8D2 File Offset: 0x0000CAD2
	private IEnumerator AIPressButton()
	{
		yield return new WaitForSeconds(this.m_aiWait);
		this.PressButton();
		yield return new WaitForSeconds(1f);
		this.m_aiPressInProgress = false;
		yield break;
	}

	// Token: 0x060011DF RID: 4575 RVA: 0x0000E8E1 File Offset: 0x0000CAE1
	public void SetWord(string word)
	{
		this.m_curWord = word;
		this.CreateWordBar();
		this.m_wordBar.SetWord(word);
		this.WordProgress = 0;
	}

	// Token: 0x060011E0 RID: 4576 RVA: 0x0008AD84 File Offset: 0x00088F84
	private void PressButton()
	{
		this.lastPressTime = Time.time;
		Collider[] array = Physics.OverlapBox(this.pressTarget.position, new Vector3(0.3f, 10f, 0.3f), Quaternion.identity, this.m_buttonTriggerLayer, QueryTriggerInteraction.Collide);
		SpellingButton spellingButton = null;
		Collider[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			spellingButton = array2[i].GetComponentInParent<SpellingButton>();
			if (spellingButton != null)
			{
				break;
			}
		}
		this.OnPressButton(spellingButton);
	}

	// Token: 0x060011E1 RID: 4577 RVA: 0x0008ADF8 File Offset: 0x00088FF8
	private void OnPressButton(SpellingButton button)
	{
		if (base.IsOwner)
		{
			if (button != null)
			{
				base.SendRPC("RPCOnPressButton", NetRPCDelivery.RELIABLE_ORDERED, new object[]
				{
					(short)button.LetterIndex
				});
			}
			else
			{
				base.SendRPC("RPCOnPressButton", NetRPCDelivery.RELIABLE_ORDERED, new object[]
				{
					-1
				});
			}
			if (this.m_curWord != null && button != null)
			{
				int value = (int)this.n_wordProgress.Value;
				if (value < this.m_curWord.Length - 1)
				{
					if ((int)(this.m_curWord[value] - 'a') == button.LetterIndex)
					{
						this.WordProgress += 1;
						AudioSystem.PlayOneShot(this.m_successSound, 0.5f, 0f, 1f);
						if (NetSystem.IsServer)
						{
							this.Score += 1;
						}
						else
						{
							base.SendRPC("IncrementScore", NetRPCDelivery.RELIABLE_ORDERED, Array.Empty<object>());
						}
					}
					else
					{
						AudioSystem.PlayOneShot(this.m_failSound, 0.5f, 0f, 1f);
					}
					if ((int)this.WordProgress == this.m_curWord.Length - 1)
					{
						this.minigameController.CompletedWord(this.m_curWord);
					}
				}
				else
				{
					AudioSystem.PlayOneShot(this.m_failSound, 0.5f, 0f, 1f);
				}
			}
		}
		this.playerAnim.Slam();
		if (button != null)
		{
			button.Press();
		}
	}

	// Token: 0x060011E2 RID: 4578 RVA: 0x0008AF70 File Offset: 0x00089170
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCOnPressButton(NetPlayer sender, short letterIndex)
	{
		SpellingButton spellingButton = this.minigameController.GetSpellingButton((int)letterIndex);
		if (spellingButton != null)
		{
			this.OnPressButton(spellingButton);
		}
	}

	// Token: 0x060011E3 RID: 4579 RVA: 0x0008AF9C File Offset: 0x0008919C
	protected override void DoMovement()
	{
		CharacterMoverInput input = default(CharacterMoverInput);
		bool val = !this.minigameController.Playable || (GameManager.IsGamePaused && !this.player.IsAI) || !GameManager.PollInput || this.Stunned || this.isDead;
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

	// Token: 0x060011E4 RID: 4580 RVA: 0x0008B140 File Offset: 0x00089340
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

	// Token: 0x060011E5 RID: 4581 RVA: 0x0008B1E8 File Offset: 0x000893E8
	private CharacterMoverInput GetAIInput()
	{
		CharacterMoverInput result = default(CharacterMoverInput);
		Vector2 b = new Vector2(base.transform.position.x, base.transform.position.z);
		if (this.agent.isOnOffMeshLink)
		{
			if (this.curOffMeshLinkTranslationType == OffMeshLinkTranslateType.None)
			{
				this.curOffMeshLinkTranslationType = OffMeshLinkTranslateType.Parabola;
				this.OnJump();
				float initialHorizontalVelocity = 5f;
				base.StartCoroutine(base.GetParabolicPath(this.mover, this.mover.gravity, 1500f, initialHorizontalVelocity, true));
			}
		}
		else
		{
			float num = 0.040000003f;
			if (this.m_targetButton == null && this.m_curWord != null && Time.time >= this.m_nextLetterTime && (int)this.WordProgress < this.m_curWord.Length)
			{
				int letterIndex = (int)(this.m_curWord[(int)this.WordProgress] - 'a');
				this.m_targetButton = this.minigameController.GetSpellingButton(letterIndex);
				if (this.m_targetButton != null)
				{
					this.targetPosition = this.m_targetButton.transform.position;
					this.targetPosition.x = this.targetPosition.x + UnityEngine.Random.Range(0.05f, 0.25f);
					this.targetPosition.z = this.targetPosition.z + UnityEngine.Random.Range(0.05f, 0.25f);
					if (this.agent.isActiveAndEnabled && this.agent.isOnNavMesh)
					{
						this.agent.SetDestination(this.targetPosition);
					}
				}
			}
			if (this.pathUpdateTimer.Elapsed(true) && this.agent.isActiveAndEnabled && this.agent.isOnNavMesh)
			{
				this.agent.SetDestination(this.targetPosition);
			}
			if ((new Vector2(this.targetPosition.x, this.targetPosition.z) - b).sqrMagnitude > num)
			{
				Vector3 vector = this.agent.steeringTarget - base.transform.position;
				Vector2 normalized = new Vector2(vector.x, vector.z).normalized;
				result = new CharacterMoverInput(normalized, false, false);
			}
			else
			{
				result.NullInput();
			}
		}
		return result;
	}

	// Token: 0x060011E6 RID: 4582 RVA: 0x0003203C File Offset: 0x0003023C
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

	// Token: 0x060011E7 RID: 4583 RVA: 0x0008B430 File Offset: 0x00089630
	public override void ResetPlayer()
	{
		this.playerAnim.SetPlayerRotationImmediate(this.startRotation.eulerAngles.y);
		this.Stunned = false;
		if (base.IsOwner)
		{
			this.mover.Velocity = Vector3.zero;
			this.velocity.Value = Vector3.zero;
		}
		base.ResetPlayer();
	}

	// Token: 0x060011E8 RID: 4584 RVA: 0x0000E903 File Offset: 0x0000CB03
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCOnJump(NetPlayer sender)
	{
		this.OnJump();
	}

	// Token: 0x060011E9 RID: 4585 RVA: 0x0000E90B File Offset: 0x0000CB0B
	[NetRPC(false, NetRPCSecurity.OWNER, NetRPCSecurity.SERVER)]
	public void IncrementScore(NetPlayer sender)
	{
		this.Score += 1;
	}

	// Token: 0x060011EA RID: 4586 RVA: 0x0000480A File Offset: 0x00002A0A
	protected void OnJump()
	{
		this.playerAnim.FireJumpTrigger();
		if (base.IsOwner)
		{
			base.SendRPC("RPCOnJump", NetRPCDelivery.RELIABLE_UNORDERED, Array.Empty<object>());
		}
	}

	// Token: 0x060011EB RID: 4587 RVA: 0x0000E91C File Offset: 0x0000CB1C
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void PunchRPC(NetPlayer sender, bool hand)
	{
		this.StartPunch(hand);
	}

	// Token: 0x060011EC RID: 4588 RVA: 0x0008B490 File Offset: 0x00089690
	private void StartPunch(bool hand)
	{
		if (!this.minigameController.Playable || this.Stunned || base.IsDead || !GameManager.PollInput || GameManager.IsGamePaused)
		{
			return;
		}
		Debug.Log("Punch2");
		if (base.IsOwner)
		{
			this.lastPunchTime = Time.time;
			base.SendRPC("PunchRPC", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				hand
			});
		}
		Debug.Log("Punch3");
		this.playerAnim.FirePunchTrigger(hand);
	}

	// Token: 0x060011ED RID: 4589 RVA: 0x0000E925 File Offset: 0x0000CB25
	private void PunchImpactEvent(PlayerAnimationEvent anim_event)
	{
		Debug.Log("Punch4");
		this.Punch();
	}

	// Token: 0x060011EE RID: 4590 RVA: 0x0008B518 File Offset: 0x00089718
	private void Punch()
	{
		if (!this.minigameController.Playable || this.Stunned || base.IsDead || !GameManager.PollInput || GameManager.IsGamePaused)
		{
			return;
		}
		Debug.Log("Punch5");
		AudioSystem.PlayOneShot(this.punch_miss, 0.75f, 0f, 1f);
		if (NetSystem.IsServer)
		{
			SpellingPlayer spellingPlayer = null;
			float num = float.MaxValue;
			for (int i = 0; i < this.minigameController.GetPlayerCount(); i++)
			{
				SpellingPlayer spellingPlayer2 = (SpellingPlayer)this.minigameController.GetPlayer(i);
				if (!(spellingPlayer2 == this) && !spellingPlayer2.Stunned && Time.time - this.stunStartTime > this.stunImmunityLength)
				{
					float num2 = Vector3.Distance(spellingPlayer2.transform.position, base.transform.position);
					if (num2 < this.punchHitDistance)
					{
						Vector3 b = base.transform.position - base.transform.forward * 0.75f;
						if (Vector3.Dot(base.transform.forward, (spellingPlayer2.transform.position - b).normalized) > 1f - this.punchAngle / 180f && num2 < num)
						{
							num = num2;
							spellingPlayer = spellingPlayer2;
						}
					}
				}
			}
			if (spellingPlayer != null)
			{
				spellingPlayer.Stunned = true;
			}
		}
	}

	// Token: 0x060011EF RID: 4591 RVA: 0x0000E937 File Offset: 0x0000CB37
	public void StunnedRecieve(object val)
	{
		this.Stunned = (bool)val;
	}

	// Token: 0x17000188 RID: 392
	// (get) Token: 0x060011F0 RID: 4592 RVA: 0x0000E945 File Offset: 0x0000CB45
	// (set) Token: 0x060011F1 RID: 4593 RVA: 0x0008B68C File Offset: 0x0008988C
	public bool Stunned
	{
		get
		{
			return this.stunned.Value;
		}
		set
		{
			this.stunned.Value = value;
			if (value)
			{
				this.stunStartTime = Time.time;
				this.mover.Velocity = Vector3.zero;
				this.velocity.Value = Vector3.zero;
				AudioSystem.PlayOneShot(this.punch_hit, 1f, 0f, 1f);
			}
		}
	}

	// Token: 0x060011F2 RID: 4594 RVA: 0x0000E952 File Offset: 0x0000CB52
	public void WordProgressRecieve(object val)
	{
		this.WordProgress = (byte)val;
	}

	// Token: 0x17000189 RID: 393
	// (get) Token: 0x060011F3 RID: 4595 RVA: 0x0000E960 File Offset: 0x0000CB60
	// (set) Token: 0x060011F4 RID: 4596 RVA: 0x0008B6F0 File Offset: 0x000898F0
	public byte WordProgress
	{
		get
		{
			return this.n_wordProgress.Value;
		}
		set
		{
			this.n_wordProgress.Value = value;
			this.m_wordBar.SetProgress((int)value);
			if (base.IsOwner && this.player.IsAI)
			{
				this.m_targetButton = null;
				this.m_nextLetterTime = Time.time + UnityEngine.Random.Range(0f, 5f) + this.m_nextLetterOffset;
				this.targetPosition = UnityEngine.Random.onUnitSphere * 5f;
				this.targetPosition.z = 0f;
			}
		}
	}

	// Token: 0x04001288 RID: 4744
	public GameObject spellingWordBarPfb;

	// Token: 0x04001289 RID: 4745
	public Transform pressTarget;

	// Token: 0x0400128A RID: 4746
	public AudioClip punch_miss;

	// Token: 0x0400128B RID: 4747
	public AudioClip punch_hit;

	// Token: 0x0400128C RID: 4748
	public GameObject stun_fx_obj;

	// Token: 0x0400128D RID: 4749
	public float base_speed = 6f;

	// Token: 0x0400128E RID: 4750
	[Header("Sounds")]
	public AudioClip m_successSound;

	// Token: 0x0400128F RID: 4751
	public AudioClip m_failSound;

	// Token: 0x04001290 RID: 4752
	private SpellingController minigameController;

	// Token: 0x04001291 RID: 4753
	private CharacterMover mover;

	// Token: 0x04001292 RID: 4754
	private float stunStartTime;

	// Token: 0x04001293 RID: 4755
	private float stunLength = 2.5f;

	// Token: 0x04001294 RID: 4756
	private float stunImmunityLength = 3.5f;

	// Token: 0x04001295 RID: 4757
	private float punchHitDistance = 3f;

	// Token: 0x04001296 RID: 4758
	private float punchAngle = 120f;

	// Token: 0x04001297 RID: 4759
	private float lastPunchTime;

	// Token: 0x04001298 RID: 4760
	private float punchInterval = 0.75f;

	// Token: 0x04001299 RID: 4761
	private float pressInterval = 0.3f;

	// Token: 0x0400129A RID: 4762
	private float lastPressTime;

	// Token: 0x0400129B RID: 4763
	private CameraShake cameraShake;

	// Token: 0x0400129C RID: 4764
	private SpellingPlayer.SpellingAIState curAIState = SpellingPlayer.SpellingAIState.Hiding;

	// Token: 0x0400129D RID: 4765
	private ActionTimer pathUpdateTimer = new ActionTimer(1f, 2f);

	// Token: 0x0400129E RID: 4766
	private ActionTimer hidePositionTimer = new ActionTimer(0.25f, 0.55f);

	// Token: 0x0400129F RID: 4767
	private ActionTimer stateChangeTimer = new ActionTimer(2.5f, 3.5f);

	// Token: 0x040012A0 RID: 4768
	private Vector3 targetPosition = Vector3.zero;

	// Token: 0x040012A1 RID: 4769
	private int m_buttonTriggerLayer;

	// Token: 0x040012A2 RID: 4770
	private UISpellingWordBar m_wordBar;

	// Token: 0x040012A3 RID: 4771
	private string m_curWord;

	// Token: 0x040012A4 RID: 4772
	private bool m_aiPressInProgress;

	// Token: 0x040012A5 RID: 4773
	private float m_nextLetterTime;

	// Token: 0x040012A6 RID: 4774
	private float m_nextLetterOffset;

	// Token: 0x040012A7 RID: 4775
	private float m_aiWait;

	// Token: 0x040012A8 RID: 4776
	private float[] followClosestChance = new float[]
	{
		0.35f,
		0.6f,
		1.1f
	};

	// Token: 0x040012A9 RID: 4777
	private float[] chanceToChase = new float[]
	{
		0.075f,
		0.15f,
		0.25f
	};

	// Token: 0x040012AA RID: 4778
	private ActionTimer followTimer = new ActionTimer(1f, 2f);

	// Token: 0x040012AB RID: 4779
	private bool followClosest;

	// Token: 0x040012AC RID: 4780
	private List<PassTheBombPlayer> targets = new List<PassTheBombPlayer>();

	// Token: 0x040012AD RID: 4781
	private SpellingButton m_targetButton;

	// Token: 0x040012AE RID: 4782
	private int[] pointsToCheck = new int[]
	{
		3,
		6,
		18
	};

	// Token: 0x040012AF RID: 4783
	[NetSend(-1, NetSendOwner.SERVER, NetSendFlags.NONE)]
	public NetVar<bool> stunned = new NetVar<bool>(false);

	// Token: 0x040012B0 RID: 4784
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.NONE)]
	public NetVar<byte> n_wordProgress = new NetVar<byte>(0);

	// Token: 0x02000262 RID: 610
	private enum SpellingAIState
	{
		// Token: 0x040012B2 RID: 4786
		HasBomb,
		// Token: 0x040012B3 RID: 4787
		Hiding,
		// Token: 0x040012B4 RID: 4788
		Chasing
	}
}
