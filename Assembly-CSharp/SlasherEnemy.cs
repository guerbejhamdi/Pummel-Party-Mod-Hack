using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.UI;
using ZP.Net;

// Token: 0x020004A3 RID: 1187
public class SlasherEnemy : BoardActor
{
	// Token: 0x170003BE RID: 958
	// (get) Token: 0x06001FAF RID: 8111 RVA: 0x00017379 File Offset: 0x00015579
	// (set) Token: 0x06001FB0 RID: 8112 RVA: 0x00017381 File Offset: 0x00015581
	public int DeathTurnsRemaining { get; set; }

	// Token: 0x06001FB1 RID: 8113 RVA: 0x0001738A File Offset: 0x0001558A
	private IEnumerator Start()
	{
		yield return new WaitUntil(() => GameManager.Board != null);
		GameManager.Board.slasherEnemy = this;
		GameManager.Board.OnLoadMinigameFadeout.AddListener(new UnityAction(this.OnStartMinigame));
		for (int i = 0; i < this.renderers.Length; i++)
		{
			this.renderers[i].sharedMaterial.SetFloat("_DissolveAmount", 0f);
		}
		this.healthBar = UnityEngine.Object.Instantiate<GameObject>(this.healthBarPrefab);
		this.healthBar.transform.SetParent(GameManager.UIController.board_ui_root, false);
		this.healthFill = this.healthBar.transform.Find("HealthFill").GetComponent<Image>();
		this.healthBar.SetActive(false);
		base.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
		if (!NetSystem.IsServer)
		{
			this.Initialize();
		}
		yield break;
	}

	// Token: 0x06001FB2 RID: 8114 RVA: 0x000C8BE8 File Offset: 0x000C6DE8
	public override void Initialize()
	{
		Transform parent = GameManager.BoardRoot.transform.Find("Events/KillerSpawnPoint");
		base.transform.parent = parent;
		base.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
	}

	// Token: 0x06001FB3 RID: 8115 RVA: 0x00017399 File Offset: 0x00015599
	public void OnDestroy()
	{
		if (GameManager.Board != null)
		{
			GameManager.Board.OnLoadMinigameFadeout.RemoveListener(new UnityAction(this.OnStartMinigame));
		}
	}

	// Token: 0x06001FB4 RID: 8116 RVA: 0x000173C3 File Offset: 0x000155C3
	private void OnStartMinigame()
	{
		this.healthBar.SetActive(false);
	}

	// Token: 0x06001FB5 RID: 8117 RVA: 0x000173D1 File Offset: 0x000155D1
	public void OnEnable()
	{
		if (this.healthBar != null && this.health.Value > 0)
		{
			this.healthBar.SetActive(true);
		}
	}

	// Token: 0x06001FB6 RID: 8118 RVA: 0x000173FB File Offset: 0x000155FB
	public void OnDisable()
	{
		if (this.healthBar != null)
		{
			this.healthBar.SetActive(false);
		}
	}

	// Token: 0x06001FB7 RID: 8119 RVA: 0x00017417 File Offset: 0x00015617
	public void Step(int foot)
	{
		AudioSystem.PlayOneShot(this.steps[UnityEngine.Random.Range(0, this.steps.Length)], 0.6f, 0f, 1f);
	}

	// Token: 0x06001FB8 RID: 8120 RVA: 0x000C8C38 File Offset: 0x000C6E38
	private void Update()
	{
		this.animator.SetFloat("Velocity", this.navMeshAgent.velocity.magnitude / this.navMeshAgent.speed);
		float num = 1f - 0.9f * Vector3.Angle(base.transform.forward, this.navMeshAgent.steeringTarget - base.transform.position) / 180f;
		this.navMeshAgent.speed = 2.5f * num;
		if (GameManager.Board.boardCamera != null && GameManager.Board.boardCamera.Cam != null && this.healthBar != null)
		{
			Vector3 zero = new Vector3(0f, 0f, 0f);
			zero = Vector3.zero;
			Vector3 position = GameManager.Board.boardCamera.Cam.WorldToScreenPoint(base.transform.position + zero) + new Vector3(0f, 80f, 0f);
			position.z = 0f;
			this.healthBar.transform.position = position;
			this.healthFill.fillAmount = (float)this.ProxyHealth / (float)this.maxHealth;
		}
	}

	// Token: 0x06001FB9 RID: 8121 RVA: 0x000C8D98 File Offset: 0x000C6F98
	public override void ApplyDamage(DamageInstance d)
	{
		base.ApplyDamage(d);
		GameManager.UIController.SpawnWorldText("-" + d.damage.ToString(), base.transform.position + Vector3.up, 2f, WorldTextType.Damage, 0.5f, null);
		base.SpawnBlood(d);
		if (!this.dead)
		{
			if ((int)this.ProxyHealth <= d.damage)
			{
				this.ProxyHealth -= (short)d.damage;
				base.StartCoroutine(this.Kill());
			}
			else
			{
				this.ProxyHealth -= (short)d.damage;
				this.animator.SetTrigger("Hit");
			}
		}
		if (d.sound)
		{
			AudioSystem.PlayOneShot(this.sfxBloodyDamage, d.volume, 0f, 1f);
		}
		AudioSystem.PlayOneShot(this.hitSound, 1f, 0f, 1f);
	}

	// Token: 0x06001FBA RID: 8122 RVA: 0x000C8E90 File Offset: 0x000C7090
	public BoardActor GetAttackTarget()
	{
		BoardActor result = null;
		float num = float.MaxValue;
		for (int i = 0; i < GameManager.Board.GetActorCount(); i++)
		{
			BoardActor actor = GameManager.Board.GetActor(i);
			if (actor.GetType() == typeof(BoardPlayer) && this.lastKilledActor != actor)
			{
				Vector3 position = base.transform.position;
				Vector3 position2 = actor.transform.position;
				NavMeshPath path = new NavMeshPath();
				if (NavMesh.CalculatePath(position, position2, -1, path))
				{
					bool flag = false;
					float pathLength = this.GetPathLength(path, out flag);
					if (flag && pathLength < this.moveDist && pathLength < num)
					{
						result = actor;
						num = pathLength;
					}
				}
			}
		}
		return result;
	}

	// Token: 0x06001FBB RID: 8123 RVA: 0x000C8F44 File Offset: 0x000C7144
	public float GetPathLength(NavMeshPath path, out bool valid)
	{
		float num = 0f;
		if (path.status != NavMeshPathStatus.PathInvalid && path.corners.Length > 1)
		{
			valid = true;
			for (int i = 1; i < path.corners.Length; i++)
			{
				num += Vector3.Distance(path.corners[i - 1], path.corners[i]);
			}
		}
		else
		{
			valid = false;
		}
		return num;
	}

	// Token: 0x06001FBC RID: 8124 RVA: 0x00017442 File Offset: 0x00015642
	public bool FinishedMoving()
	{
		return this.navMeshAgent.remainingDistance < this.navMeshAgent.stoppingDistance || Time.time - this.moveStartTime > 7f;
	}

	// Token: 0x06001FBD RID: 8125 RVA: 0x00017471 File Offset: 0x00015671
	public void MoveTo(Vector3 point)
	{
		this.moveStartTime = Time.time;
		this.navMeshAgent.SetDestination(point);
	}

	// Token: 0x06001FBE RID: 8126 RVA: 0x0001748B File Offset: 0x0001568B
	public void Attack(byte actorID)
	{
		base.StartCoroutine(this.AttackRoutine(actorID));
	}

	// Token: 0x06001FBF RID: 8127 RVA: 0x0001749B File Offset: 0x0001569B
	private IEnumerator AttackRoutine(byte actorID)
	{
		this.attackFinished = false;
		yield return new WaitForSeconds(1f);
		this.animator.SetTrigger("Swing");
		yield return new WaitForSeconds(0.55f);
		BoardActor actor = GameManager.Board.GetActor(actorID);
		DamageInstance d = new DamageInstance
		{
			damage = 30,
			origin = actor.MidPoint - base.transform.right,
			blood = true,
			ragdoll = true,
			ragdollVel = 9f,
			bloodVel = 13f,
			bloodAmount = 1f,
			sound = false,
			volume = 0.75f,
			details = "Halloween Killer",
			removeKeys = true
		};
		actor.ApplyDamage(d);
		this.lastKilledActor = actor;
		AudioSystem.PlayOneShot(this.hit, 0.5f, 0f, 1f);
		yield return new WaitForSeconds(0.5f);
		AudioSystem.PlayOneShot(this.growl, 0.5f, 0f, 1f);
		yield return new WaitForSeconds(3.5f);
		this.attackFinished = true;
		yield break;
	}

	// Token: 0x06001FC0 RID: 8128 RVA: 0x000C8FAC File Offset: 0x000C71AC
	public Vector3 GetMovePoint(float moveDist, Vector3 target)
	{
		NavMeshPath navMeshPath = new NavMeshPath();
		NavMesh.CalculatePath(base.transform.position, target, -1, navMeshPath);
		Vector3 vector = base.transform.position;
		float num = moveDist;
		for (int i = 0; i < navMeshPath.corners.Length; i++)
		{
			Debug.DrawLine(vector, navMeshPath.corners[i], Color.green, 5f);
			Vector3 vector2 = navMeshPath.corners[i] - vector;
			float magnitude = vector2.magnitude;
			if (magnitude >= num)
			{
				return vector + vector2.normalized * Mathf.Clamp(num, 0f, magnitude);
			}
			num -= magnitude;
			vector = navMeshPath.corners[i];
		}
		return target;
	}

	// Token: 0x06001FC1 RID: 8129 RVA: 0x000174B1 File Offset: 0x000156B1
	public void Reveal()
	{
		base.StartCoroutine(this.RevealEvent());
	}

	// Token: 0x06001FC2 RID: 8130 RVA: 0x000174C0 File Offset: 0x000156C0
	private IEnumerator RevealEvent()
	{
		this.dead = false;
		this.ProxyHealth = this.maxHealth;
		this.animator.SetBool("Dead", false);
		yield return null;
		yield return base.StartCoroutine(this.Fade(true));
		this.healthBar.SetActive(true);
		this.animator.SetTrigger("Flex");
		yield return new WaitForSeconds(0.3f);
		AudioSystem.PlayOneShot(this.growl, 1f, 0f, 1f);
		yield break;
	}

	// Token: 0x06001FC3 RID: 8131 RVA: 0x000C906C File Offset: 0x000C726C
	private void SetRenderers(bool enabled)
	{
		for (int i = 0; i < this.renderers.Length; i++)
		{
			this.renderers[i].enabled = enabled;
		}
	}

	// Token: 0x06001FC4 RID: 8132 RVA: 0x000174CF File Offset: 0x000156CF
	public IEnumerator Fade(bool fadeIn)
	{
		if (fadeIn)
		{
			this.SetRenderers(true);
			this.triggerCollider.enabled = true;
		}
		AudioSystem.PlayOneShot(this.spawnClip, 1f, 0f, 1f);
		float target = fadeIn ? 1.05f : 0f;
		float start = fadeIn ? 0f : 1.05f;
		float duration = 2f;
		float elapsedTime = 0f;
		yield return new WaitUntil(delegate()
		{
			float value = Mathf.Lerp(start, target, Mathf.Clamp01(elapsedTime / duration));
			for (int i = 0; i < this.renderers.Length; i++)
			{
				this.renderers[i].sharedMaterial.SetFloat("_DissolveAmount", value);
			}
			if (elapsedTime >= duration)
			{
				return true;
			}
			elapsedTime += Time.deltaTime;
			return false;
		});
		if (!fadeIn)
		{
			this.SetRenderers(false);
			this.triggerCollider.enabled = false;
		}
		yield break;
	}

	// Token: 0x06001FC5 RID: 8133 RVA: 0x000174E5 File Offset: 0x000156E5
	private IEnumerator Kill()
	{
		this.dead = true;
		this.DeathTurnsRemaining = 3;
		this.animator.SetBool("Dead", true);
		GameManager.Board.QueueAction(new ActionWait(4.5f), false, true);
		yield return new WaitForSeconds(2f);
		this.healthBar.SetActive(false);
		yield return base.StartCoroutine(this.Fade(false));
		yield break;
	}

	// Token: 0x06001FC6 RID: 8134 RVA: 0x000174F4 File Offset: 0x000156F4
	public void StartMusic()
	{
		AudioSystem.PlayMusic(this.activeMusic, 2.5f, 1f);
	}

	// Token: 0x06001FC7 RID: 8135 RVA: 0x0001750B File Offset: 0x0001570B
	public void StopMusic()
	{
		AudioSystem.PlayMusic(this.calmMusic, 2.5f, 1f);
	}

	// Token: 0x170003BF RID: 959
	// (get) Token: 0x06001FC8 RID: 8136 RVA: 0x00012D07 File Offset: 0x00010F07
	public override Vector3 MidPoint
	{
		get
		{
			return base.transform.position;
		}
	}

	// Token: 0x04002290 RID: 8848
	public NavMeshAgent navMeshAgent;

	// Token: 0x04002291 RID: 8849
	public Animator animator;

	// Token: 0x04002292 RID: 8850
	public float moveDist = 7f;

	// Token: 0x04002293 RID: 8851
	public AudioClip hit;

	// Token: 0x04002294 RID: 8852
	public AudioClip growl;

	// Token: 0x04002295 RID: 8853
	public GameObject healthBarPrefab;

	// Token: 0x04002296 RID: 8854
	public AudioClip[] steps;

	// Token: 0x04002297 RID: 8855
	public AudioClip sfxBloodyDamage;

	// Token: 0x04002298 RID: 8856
	public AudioClip hitSound;

	// Token: 0x04002299 RID: 8857
	public Renderer[] renderers;

	// Token: 0x0400229A RID: 8858
	public AudioClip spawnClip;

	// Token: 0x0400229B RID: 8859
	public Collider triggerCollider;

	// Token: 0x0400229C RID: 8860
	public AudioClip calmMusic;

	// Token: 0x0400229D RID: 8861
	public AudioClip activeMusic;

	// Token: 0x0400229E RID: 8862
	private GameObject healthBar;

	// Token: 0x0400229F RID: 8863
	private Image healthFill;

	// Token: 0x040022A0 RID: 8864
	[HideInInspector]
	public BoardActor lastKilledActor;

	// Token: 0x040022A2 RID: 8866
	private float moveStartTime;

	// Token: 0x040022A3 RID: 8867
	public bool attackFinished;

	// Token: 0x040022A4 RID: 8868
	private bool dead;
}
