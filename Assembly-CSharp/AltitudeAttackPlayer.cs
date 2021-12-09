using System;
using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;
using ZP.Net;

// Token: 0x0200013E RID: 318
public class AltitudeAttackPlayer : CharacterBase
{
	// Token: 0x170000D3 RID: 211
	// (get) Token: 0x0600091C RID: 2332 RVA: 0x0000A23E File Offset: 0x0000843E
	public bool IsHeliAlive
	{
		get
		{
			return this.m_isAlive;
		}
	}

	// Token: 0x170000D4 RID: 212
	// (get) Token: 0x0600091D RID: 2333 RVA: 0x0000A246 File Offset: 0x00008446
	// (set) Token: 0x0600091E RID: 2334 RVA: 0x0000A24E File Offset: 0x0000844E
	public bool IsImmune { get; set; }

	// Token: 0x0600091F RID: 2335 RVA: 0x000515A0 File Offset: 0x0004F7A0
	public override void OnNetInitialize()
	{
		base.OnNetInitialize();
		if (!base.IsOwner)
		{
			this.position.Recieve = new RecieveProxy(this.RecievePosition);
			this.velocity.Recieve = new RecieveProxy(this.RecieveVelocity);
			return;
		}
		this.position.Value = base.transform.position;
	}

	// Token: 0x06000920 RID: 2336 RVA: 0x00051608 File Offset: 0x0004F808
	protected override void Start()
	{
		base.Start();
		this.minigameController = (AltitudeAttackController)GameManager.Minigame;
		this.minigameController.AddPlayer(this);
		this.m_spriteRenderer.color = this.player.Color.skinColor1;
		this.m_scoreText = this.minigameController.Root.transform.Find("GameScene/UI/ScoreText" + base.OwnerSlot.ToString()).GetComponent<RetroScoreText>();
		for (int i = 0; i < this.m_scoreText.transform.childCount; i++)
		{
			this.m_scoreText.transform.GetChild(i).gameObject.SetActive(true);
		}
		this.m_scoreText.transform.Find("Color").GetComponent<SpriteRenderer>().color = this.player.Color.skinColor1;
	}

	// Token: 0x06000921 RID: 2337 RVA: 0x000516F0 File Offset: 0x0004F8F0
	public void Awake()
	{
		this.m_renderer = base.GetComponent<SpriteRenderer>();
		this.m_rigidBody = base.GetComponent<Rigidbody2D>();
		this.m_startPos = base.transform.position;
		this.m_bulletHitMask = LayerMask.GetMask(new string[]
		{
			"WorldWall"
		});
		this.m_fireHitMask = LayerMask.GetMask(new string[]
		{
			"Players",
			"WorldWall"
		});
		this.m_navNodes = UnityEngine.Object.FindObjectsOfType<NavNode2D>();
	}

	// Token: 0x06000922 RID: 2338 RVA: 0x0005176C File Offset: 0x0004F96C
	private void Update()
	{
		if (this.m_scoreText != null)
		{
			this.m_scoreText.SetNumber((int)this.Score);
		}
		if (base.IsOwner && !this.player.IsAI && this.Score >= 10 && !this.gotAchievement)
		{
			PlatformAchievementManager.Instance.TriggerAchievement("ACH_ALTITUDE_ATTACK");
			this.gotAchievement = true;
		}
		this.m_bulletsAudioSource.volume = AudioSystem.GetVolume(SoundType.Effect, this.m_bulletsVolume);
		if (this.minigameController.Playable)
		{
			if (this.helicopterAudioSource == null)
			{
				this.helicopterAudioSource = AudioSystem.PlayLooping(this.m_helicopterClip, 0.5f, 0.2f);
			}
			if (base.IsOwner)
			{
				if (this.player.IsAI)
				{
					bool flag = false;
					for (int i = 0; i < this.minigameController.players.Count; i++)
					{
						Vector3 normalized = (((AltitudeAttackPlayer)this.minigameController.players[i]).transform.position - this.m_bulletSource.position).normalized;
						if (Vector3.Angle(this.m_bulletSource.right, normalized) < 10f)
						{
							flag = true;
							this.m_lastFire = Time.time;
							break;
						}
					}
					this.firing.Value = (flag || Time.time - this.m_lastFire < 0.5f);
				}
				else
				{
					Player rewiredPlayer = this.player.RewiredPlayer;
					bool button;
					if (rewiredPlayer != null && rewiredPlayer.controllers.GetLastActiveController() != null && rewiredPlayer.controllers.GetLastActiveController().type == ControllerType.Joystick)
					{
						button = this.player.RewiredPlayer.GetButton(InputActions.UseItemShoot);
					}
					else
					{
						button = this.player.RewiredPlayer.GetButton(InputActions.Accept);
					}
					this.firing.Value = (!GameManager.IsGamePaused && button);
				}
				this.DoMovement();
			}
			else if (this.gotPosition)
			{
				base.transform.position = new Vector3(this.position.Value.x, this.position.Value.y, this.startPosition.z);
				this.gotPosition = false;
			}
			else
			{
				base.transform.position += new Vector3(this.velocity.Value.x, this.velocity.Value.y, 0f) * Time.deltaTime;
			}
		}
		else
		{
			this.firing.Value = false;
		}
		this.UpdateFiring(this.firing.Value && this.m_isAlive && !this.IsImmune);
		float x = this.velocity.Value.x;
		float t = Mathf.Max(Mathf.Abs(x) / this.m_maxHorizontalMagnitude, Mathf.Abs(this.m_climbFallVelocity.y) / this.m_maxClimbFallMagnitude);
		if (this.helicopterAudioSource != null)
		{
			this.helicopterAudioSource.audio_source.pitch = Mathf.Lerp(0.8f, 1.2f, t);
		}
		if (x > 0f)
		{
			float num = x / this.m_maxHorizontalMagnitude;
			base.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, -num * this.m_horizontalRotation));
		}
		else
		{
			float num2 = Mathf.Abs(x) / this.m_maxHorizontalMagnitude;
			base.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, num2 * this.m_horizontalRotation));
		}
		float num3 = 1.5f;
		if (this.facingRight.Value)
		{
			base.transform.localScale = new Vector3(num3, num3, num3);
			return;
		}
		base.transform.localScale = new Vector3(-num3, num3, num3);
	}

	// Token: 0x06000923 RID: 2339 RVA: 0x00051B58 File Offset: 0x0004FD58
	public List<NavNode2D> GetVisibleNodes()
	{
		List<NavNode2D> list = new List<NavNode2D>();
		foreach (NavNode2D navNode2D in this.m_navNodes)
		{
			float num = Vector3.Distance(navNode2D.transform.position, base.transform.position);
			if (Physics2D.Raycast(base.transform.position, (navNode2D.transform.position - base.transform.position).normalized, num, this.m_bulletHitMask).collider == null)
			{
				if (list.Count > 0)
				{
					for (int j = 0; j < list.Count; j++)
					{
						if (Vector3.Distance(list[j].transform.position, base.transform.position) > num)
						{
							list.Insert(j, navNode2D);
							break;
						}
						if (j == list.Count - 1)
						{
							list.Add(navNode2D);
							break;
						}
					}
				}
				else
				{
					list.Add(navNode2D);
				}
			}
		}
		return list;
	}

	// Token: 0x06000924 RID: 2340 RVA: 0x00051C6C File Offset: 0x0004FE6C
	private void DoMovement()
	{
		if (!this.player.IsAI)
		{
			Vector2 vector = new Vector2(this.player.RewiredPlayer.GetAxis(InputActions.Horizontal), this.player.RewiredPlayer.GetAxis(InputActions.Vertical));
			vector.x = ((vector.x < 0.2f & vector.x > -0.2f) ? 0f : vector.x);
			vector.y = ((vector.y < 0.2f & vector.y > -0.2f) ? 0f : vector.y);
			this.UpdateMovement(vector);
			return;
		}
		if (Time.time > this.m_nextRargetSwap || !this.m_targetPlayer.IsHeliAlive)
		{
			this.m_targetPlayer = (AltitudeAttackPlayer)this.minigameController.players[UnityEngine.Random.Range(0, GameManager.GetPlayerCount())];
			this.m_nextRargetSwap = Time.time + 2f;
			this.m_targetOffset = UnityEngine.Random.onUnitSphere * 0.2f;
		}
		Vector3 normalized = (this.m_targetPlayer.transform.position - base.transform.position).normalized;
		if (Physics2D.CircleCast(base.transform.position + normalized * 0.1f, 0.1f, normalized, Vector3.Distance(this.m_targetPlayer.transform.position, base.transform.position), this.m_bulletHitMask).collider != null)
		{
			List<NavNode2D> visibleNodes = this.m_targetPlayer.GetVisibleNodes();
			List<NavNode2D> visibleNodes2 = this.GetVisibleNodes();
			if (visibleNodes2.Count > 0 && visibleNodes.Count > 0)
			{
				NavNode2D navNode2D = visibleNodes[0];
				NavNode2D navNode2D2 = visibleNodes2[0];
				this.m_targetTransform = null;
				using (List<NavNode2D>.Enumerator enumerator = visibleNodes2.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						NavNode2D navNode2D3 = enumerator.Current;
						foreach (NavNode2D y in visibleNodes)
						{
							if (navNode2D3 == y)
							{
								this.m_targetTransform = navNode2D3.transform;
								break;
							}
						}
						if (this.m_targetTransform != null)
						{
							break;
						}
					}
					goto IL_1EA;
				}
			}
			this.m_targetTransform = null;
		}
		else
		{
			this.m_targetTransform = this.m_targetPlayer.transform;
		}
		IL_1EA:
		if (this.m_targetTransform != null)
		{
			float distance = 0.2f;
			RaycastHit2D raycastHit2D = Physics2D.CircleCast(base.transform.position, 0.1f, new Vector3(1f, 0f, 0f), distance, this.m_bulletHitMask);
			RaycastHit2D raycastHit2D2 = Physics2D.CircleCast(base.transform.position, 0.1f, new Vector3(-1f, 0f, 0f), distance, this.m_bulletHitMask);
			RaycastHit2D raycastHit2D3 = Physics2D.CircleCast(base.transform.position, 0.1f, new Vector3(0f, 1f, 0f), distance, this.m_bulletHitMask);
			RaycastHit2D raycastHit2D4 = Physics2D.CircleCast(base.transform.position, 0.1f, new Vector3(0f, -1f, 0f), distance, this.m_bulletHitMask);
			Vector3 a = this.m_targetTransform.position + this.m_targetOffset;
			float num = Vector3.Distance(a, base.transform.position);
			Vector3 normalized2 = (a - base.transform.position).normalized;
			bool flag = (normalized2.y > 0.1f && this.m_climbFallVelocity.y < this.m_maxClimbFallMagnitude * 0.15f) || raycastHit2D4.collider != null;
			bool flag2 = (normalized2.y < -0.1f && (double)this.m_climbFallVelocity.y > (double)(-(double)this.m_maxClimbFallMagnitude) * 0.15) || raycastHit2D3.collider != null;
			bool flag3 = (normalized2.x < -0.1f && this.m_horizontalVelocity > -this.m_maxHorizontalMagnitude * 0.25f) || raycastHit2D.collider != null;
			bool flag4 = (normalized2.x > 0.1f && this.m_horizontalVelocity < this.m_maxHorizontalMagnitude * 0.25f) || raycastHit2D2.collider != null;
			if (flag && raycastHit2D3.collider != null)
			{
				flag = false;
			}
			if (flag2 && raycastHit2D4.collider != null)
			{
				flag2 = false;
			}
			if (flag3 && raycastHit2D2.collider != null)
			{
				flag3 = false;
			}
			if (flag4 && raycastHit2D3.collider != null)
			{
				flag4 = false;
			}
			Vector2 input = default(Vector2);
			if (flag)
			{
				input.y += 1f;
			}
			if (flag2)
			{
				input.y -= 1f;
			}
			if (flag3)
			{
				input.x -= 1f;
			}
			if (flag4)
			{
				input.x += 1f;
			}
			if (num > 0.15f)
			{
				this.UpdateMovement(input);
			}
			else
			{
				this.UpdateMovement(Vector2.zero);
			}
			Debug.DrawLine(base.transform.position, this.m_targetTransform.position, Color.green);
			return;
		}
		this.UpdateMovement(Vector2.zero);
	}

	// Token: 0x06000925 RID: 2341 RVA: 0x00052248 File Offset: 0x00050448
	private void UpdateFiring(bool isFiring)
	{
		if (isFiring)
		{
			if (!this.m_bullets.activeSelf)
			{
				this.m_bullets.SetActive(true);
			}
			float num = 1f / Mathf.Abs(base.transform.localScale.x);
			float d = Mathf.Sign(base.transform.localScale.x);
			RaycastHit2D raycastHit2D = Physics2D.Raycast(this.m_bulletSource.position, d * this.m_bulletSource.right, this.m_fireDistance, this.m_fireHitMask);
			Vector3 vector = this.m_bulletSource.position + d * this.m_bulletSource.right * this.m_fireDistance;
			if (raycastHit2D.collider != null)
			{
				AltitudeAttackPlayer componentInParent = raycastHit2D.collider.GetComponentInParent<AltitudeAttackPlayer>();
				if (componentInParent != null && componentInParent.IsHeliAlive && !componentInParent.IsImmune && componentInParent.IsOwner)
				{
					base.StartCoroutine(componentInParent.Explode((byte)base.OwnerSlot));
				}
				this.m_bulletsRenderer.size = new Vector2(raycastHit2D.distance * num, this.m_bulletsRenderer.size.y);
				vector = new Vector3(raycastHit2D.point.x, raycastHit2D.point.y, base.transform.position.z);
			}
			else
			{
				this.m_bulletsRenderer.size = new Vector2(this.m_fireDistance * num, this.m_bulletsRenderer.size.y);
			}
			if (Time.time - this.m_lastBulletHit > 0.1f)
			{
				UnityEngine.Object.Destroy(UnityEngine.Object.Instantiate<GameObject>(this.m_bulletHit, vector, Quaternion.identity), 0.5f);
				this.m_lastBulletHit = Time.time;
				return;
			}
		}
		else if (this.m_bullets.activeSelf)
		{
			this.m_bullets.SetActive(false);
		}
	}

	// Token: 0x06000926 RID: 2342 RVA: 0x0005243C File Offset: 0x0005063C
	private void UpdateMovement(Vector2 input)
	{
		Vector3 zero = Vector3.zero;
		if (this.m_isAlive)
		{
			this.m_climbFallVelocity.y = this.m_climbFallVelocity.y + this.m_climbFallAcceleration * Time.deltaTime * input.y;
			this.m_climbFallVelocity.y = Mathf.SmoothDamp(this.m_climbFallVelocity.y, 0f, ref this.vertVel, 0.3f);
			this.m_climbFallVelocity.y = Mathf.Clamp(this.m_climbFallVelocity.y, -this.m_maxClimbFallMagnitude, this.m_maxClimbFallMagnitude);
			zero.y += this.m_climbFallVelocity.y;
			this.m_horizontalVelocity += this.m_horizontalAcceleration * Time.deltaTime * input.x;
			this.m_horizontalVelocity = Mathf.SmoothDamp(this.m_horizontalVelocity, 0f, ref this.horVel, 0.3f);
			this.m_horizontalVelocity = Mathf.Clamp(this.m_horizontalVelocity, -this.m_maxHorizontalMagnitude, this.m_maxHorizontalMagnitude);
			zero.x += this.m_horizontalVelocity;
			base.GetComponent<Rigidbody2D>().velocity = zero;
			if (input.x < -0.2f || input.x > 0.2f)
			{
				this.facingRight.Value = (input.x > 0f);
			}
			float num = 2f;
			if (base.transform.position.x < -2.8f * num)
			{
				base.transform.position = new Vector3(2.7f * num, base.transform.position.y, base.transform.position.z);
			}
			else if (base.transform.position.x > 2.8f * num)
			{
				base.transform.position = new Vector3(-2.7f * num, base.transform.position.y, base.transform.position.z);
			}
			else if (base.transform.position.y < -2f * num)
			{
				base.transform.position = new Vector3(base.transform.position.x, 1.9f * num, base.transform.position.z);
			}
			else if (base.transform.position.y > 2f * num)
			{
				base.transform.position = new Vector3(base.transform.position.x, -1.9f * num, base.transform.position.z);
			}
		}
		this.position.Value = new Vector2(base.transform.position.x, base.transform.position.y);
		this.velocity.Value = new Vector2(zero.x, zero.y);
	}

	// Token: 0x06000927 RID: 2343 RVA: 0x0000A257 File Offset: 0x00008457
	public void OnTriggerEnter2D(Collider2D collision)
	{
		if (base.IsOwner && collision.CompareTag("MinigameCustom01"))
		{
			bool isAlive = this.m_isAlive;
		}
	}

	// Token: 0x06000928 RID: 2344 RVA: 0x00052738 File Offset: 0x00050938
	public void OnCollisionEnter2D(Collision2D collision)
	{
		if (base.IsOwner && collision.collider.CompareTag("MinigameCustom02") && this.m_isAlive)
		{
			if (this.lastExplode != null)
			{
				base.StopCoroutine(this.lastExplode);
			}
			this.lastExplode = base.StartCoroutine(this.Explode(254));
		}
	}

	// Token: 0x06000929 RID: 2345 RVA: 0x0000A275 File Offset: 0x00008475
	public void RecievePosition(object _pos)
	{
		this.gotPosition = true;
	}

	// Token: 0x0600092A RID: 2346 RVA: 0x0000398C File Offset: 0x00001B8C
	public void RecieveVelocity(object val)
	{
	}

	// Token: 0x0600092B RID: 2347 RVA: 0x0000A27E File Offset: 0x0000847E
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void ExplodeRPC(NetPlayer sender, byte killerSlot)
	{
		base.StartCoroutine(this.Explode(killerSlot));
	}

	// Token: 0x0600092C RID: 2348 RVA: 0x0000A28E File Offset: 0x0000848E
	private IEnumerator Explode(byte killerSlot)
	{
		if (this.minigameController != null && this.minigameController.Playable)
		{
			if (base.IsOwner)
			{
				base.SendRPC("ExplodeRPC", NetRPCDelivery.RELIABLE_UNORDERED, new object[]
				{
					killerSlot
				});
			}
			if (NetSystem.IsServer && killerSlot != 254)
			{
				CharacterBase playerInSlot = this.minigameController.GetPlayerInSlot((short)killerSlot);
				short score = playerInSlot.Score;
				playerInSlot.Score = score + 1;
			}
			this.IsImmune = true;
			this.m_isAlive = false;
			this.m_renderer.enabled = false;
			this.m_partsSpriteRenderer.enabled = false;
			this.m_rigidBody.simulated = false;
			float originalVolume = 0f;
			if (this.helicopterAudioSource != null)
			{
				originalVolume = this.helicopterAudioSource.audio_source.volume;
				this.helicopterAudioSource.audio_source.volume = 0f;
			}
			this.m_bullets.SetActive(false);
			this.firing.Value = false;
			this.m_climbFallVelocity = Vector3.zero;
			this.m_horizontalVelocity = 0f;
			UnityEngine.Object.Destroy(UnityEngine.Object.Instantiate<GameObject>(this.m_explosion, base.transform.position, Quaternion.identity), 3f);
			AudioSystem.PlayOneShot(this.m_explosionClip, 1.2f, 0f, 1f);
			yield return new WaitForSeconds(1f);
			if (base.IsOwner)
			{
				base.transform.position = this.m_navNodes[UnityEngine.Random.Range(0, this.m_navNodes.Length)].transform.position;
			}
			yield return new WaitForSeconds(1f);
			this.m_renderer.enabled = true;
			this.m_partsSpriteRenderer.enabled = true;
			this.m_rigidBody.simulated = true;
			if (this.helicopterAudioSource != null)
			{
				this.helicopterAudioSource.audio_source.volume = originalVolume;
			}
			this.m_isAlive = true;
			int num;
			for (int i = 0; i < 10; i = num)
			{
				this.m_renderer.enabled = !this.m_renderer.enabled;
				this.m_partsSpriteRenderer.enabled = !this.m_partsSpriteRenderer.enabled;
				yield return new WaitForSeconds(0.125f);
				num = i + 1;
			}
			this.IsImmune = false;
			this.m_renderer.enabled = true;
			this.m_partsSpriteRenderer.enabled = true;
		}
		yield break;
	}

	// Token: 0x0600092D RID: 2349 RVA: 0x0000A2A4 File Offset: 0x000084A4
	public void OnDestroy()
	{
		if (this.helicopterAudioSource != null)
		{
			this.helicopterAudioSource.FadeAudio(0.5f, FadeType.Out);
		}
	}

	// Token: 0x04000787 RID: 1927
	[SerializeField]
	protected float m_fallSpeed = 1f;

	// Token: 0x04000788 RID: 1928
	[Header("Vertical Movement")]
	[SerializeField]
	protected float m_climbFallAcceleration = 1f;

	// Token: 0x04000789 RID: 1929
	[SerializeField]
	protected float m_maxClimbFallMagnitude = 1f;

	// Token: 0x0400078A RID: 1930
	[Header("Horizontal Movement")]
	[SerializeField]
	protected float m_horizontalAcceleration = 1f;

	// Token: 0x0400078B RID: 1931
	[SerializeField]
	protected float m_maxHorizontalMagnitude = 1f;

	// Token: 0x0400078C RID: 1932
	[SerializeField]
	protected float m_horizontalRotation = 30f;

	// Token: 0x0400078D RID: 1933
	[Header("References")]
	[SerializeField]
	protected GameObject m_bullets;

	// Token: 0x0400078E RID: 1934
	[SerializeField]
	protected GameObject m_explosion;

	// Token: 0x0400078F RID: 1935
	[SerializeField]
	protected SpriteRenderer m_bulletsRenderer;

	// Token: 0x04000790 RID: 1936
	[SerializeField]
	protected GameObject m_bulletHit;

	// Token: 0x04000791 RID: 1937
	[SerializeField]
	protected Transform m_bulletSource;

	// Token: 0x04000792 RID: 1938
	[SerializeField]
	protected RetroScoreText m_scoreText;

	// Token: 0x04000793 RID: 1939
	[SerializeField]
	protected SpriteRenderer m_spriteRenderer;

	// Token: 0x04000794 RID: 1940
	[SerializeField]
	protected SpriteRenderer m_partsSpriteRenderer;

	// Token: 0x04000795 RID: 1941
	[SerializeField]
	protected AudioClip m_helicopterClip;

	// Token: 0x04000796 RID: 1942
	[SerializeField]
	protected AudioClip m_explosionClip;

	// Token: 0x04000797 RID: 1943
	[SerializeField]
	protected float m_bulletsVolume;

	// Token: 0x04000798 RID: 1944
	[SerializeField]
	protected AudioSource m_bulletsAudioSource;

	// Token: 0x04000799 RID: 1945
	private Vector3 m_climbFallVelocity;

	// Token: 0x0400079A RID: 1946
	private float m_horizontalVelocity;

	// Token: 0x0400079B RID: 1947
	private SpriteRenderer m_renderer;

	// Token: 0x0400079C RID: 1948
	private Rigidbody2D m_rigidBody;

	// Token: 0x0400079D RID: 1949
	private Vector3 m_startPos;

	// Token: 0x0400079E RID: 1950
	private bool m_isAlive = true;

	// Token: 0x0400079F RID: 1951
	private int m_bulletHitMask;

	// Token: 0x040007A0 RID: 1952
	private int m_fireHitMask;

	// Token: 0x040007A1 RID: 1953
	private NavNode2D[] m_navNodes;

	// Token: 0x040007A2 RID: 1954
	private AltitudeAttackPlayer m_targetPlayer;

	// Token: 0x040007A3 RID: 1955
	private Transform m_targetTransform;

	// Token: 0x040007A4 RID: 1956
	private float m_lastBulletHit;

	// Token: 0x040007A5 RID: 1957
	private float horVel;

	// Token: 0x040007A6 RID: 1958
	private float vertVel;

	// Token: 0x040007A7 RID: 1959
	private float m_nextRargetSwap;

	// Token: 0x040007A8 RID: 1960
	private Vector3 m_targetOffset = Vector3.zero;

	// Token: 0x040007A9 RID: 1961
	private float m_lastFire;

	// Token: 0x040007AA RID: 1962
	private bool gotPosition;

	// Token: 0x040007AB RID: 1963
	private AltitudeAttackController minigameController;

	// Token: 0x040007AC RID: 1964
	private TempAudioSource helicopterAudioSource;

	// Token: 0x040007AE RID: 1966
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.ALWAYS_SEND)]
	public NetVec2 position = new NetVec2(Vector2.zero);

	// Token: 0x040007AF RID: 1967
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.ALWAYS_SEND)]
	public NetVec2 velocity = new NetVec2(Vector2.zero);

	// Token: 0x040007B0 RID: 1968
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.CHANGES_OFTEN)]
	public NetVar<bool> firing = new NetVar<bool>(false);

	// Token: 0x040007B1 RID: 1969
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.CHANGES_OFTEN)]
	public NetVar<bool> facingRight = new NetVar<bool>(false);

	// Token: 0x040007B2 RID: 1970
	private bool gotAchievement;

	// Token: 0x040007B3 RID: 1971
	private float m_fireDistance = 1.5f;

	// Token: 0x040007B4 RID: 1972
	private Coroutine lastExplode;
}
