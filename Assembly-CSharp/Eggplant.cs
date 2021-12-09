using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZP.Net;

// Token: 0x0200036C RID: 876
public class Eggplant : NetBehaviour
{
	// Token: 0x06001788 RID: 6024 RVA: 0x000A3338 File Offset: 0x000A1538
	public override void OnNetInitialize()
	{
		this.spawnTime = Time.time;
		if (base.IsOwner)
		{
			this.position.Value = base.transform.position;
			this.rotation.Value = base.transform.localRotation.eulerAngles;
		}
		else
		{
			this.position.Recieve = new RecieveProxy(this.RecievePosition);
			this.rotation.Recieve = new RecieveProxy(this.RecieveRotation);
			this.interpolator = new Interpolator(base.transform, Interpolator.InterpolationType.PositionTransform);
			this.rotInterpolator = new Interpolator(base.transform, Interpolator.InterpolationType.RotationTransform);
		}
		GameManager.Board.boardCamera.SetTrackedObject(base.transform, Vector3.zero);
		this.gamePlayer = GameManager.GetPlayerAt((int)base.OwnerSlot);
		this.itemController = (EggplantItem)this.gamePlayer.BoardObject.EquippedItem;
		this.itemController.NetObjectSpawned();
		this.currentNode = this.itemController.AttackPath;
		if (this.currentNode.next != null)
		{
			this.targetNode = this.itemController.AttackPath.next;
		}
		else
		{
			this.currentNode = this.targetNode;
		}
		this.fuseAudioSource.volume = AudioSystem.GetVolume(SoundType.Effect, 0.3f);
		base.OnNetInitialize();
	}

	// Token: 0x06001789 RID: 6025 RVA: 0x000A3490 File Offset: 0x000A1690
	private void Update()
	{
		if (this.isDead)
		{
			return;
		}
		if (base.IsOwner)
		{
			this.DoMovement();
			Vector3 vector = base.transform.position;
			Rect mapExtents = GameManager.Board.CurrentMap.mapExtents;
			vector.x = Mathf.Clamp(vector.x, mapExtents.xMin, mapExtents.xMax);
			vector.z = Mathf.Clamp(vector.z, mapExtents.yMin, mapExtents.yMax);
			base.transform.position = vector;
			this.rotation.Value = base.transform.rotation.eulerAngles;
			this.position.Value = base.transform.position;
			bool flag = Time.time - this.spawnTime > this.life;
			if (this.gamePlayer.IsAI)
			{
				float num = 1f;
				float num2 = num * num;
				float sqrMagnitude = (base.transform.position - this.itemController.Target.transform.position).sqrMagnitude;
				flag |= (sqrMagnitude < num2);
			}
			else
			{
				flag |= (!GameManager.IsGamePaused && this.gamePlayer.RewiredPlayer.GetButtonDown(InputActions.Action1));
			}
			if (flag)
			{
				this.OwnerExplode();
				return;
			}
		}
		else
		{
			this.interpolator.Update();
			this.rotInterpolator.Update();
		}
	}

	// Token: 0x0600178A RID: 6026 RVA: 0x000A35FC File Offset: 0x000A17FC
	private void DoMovement()
	{
		if (this.rb.maxAngularVelocity != this.maxAngularVelocity)
		{
			this.rb.maxAngularVelocity = this.maxAngularVelocity;
		}
		Vector3 vector = Vector3.zero;
		float d = 1f;
		if (base.IsOwner && this.gamePlayer.IsAI)
		{
			float num = 1f;
			float num2 = num * num;
			Vector3 vector2 = this.targetNode.node.transform.position - base.transform.position;
			float sqrMagnitude = vector2.sqrMagnitude;
			float magnitude = vector2.magnitude;
			if (sqrMagnitude <= num2)
			{
				this.currentNode = this.targetNode;
				if (this.currentNode.next == null)
				{
					this.targetNode = this.currentNode;
				}
				else
				{
					this.targetNode = this.currentNode.next;
				}
			}
			else if (magnitude < 3f)
			{
				d = Mathf.Lerp(0.5f, 1f, Mathf.Clamp01((magnitude - num) / 2f));
			}
			Vector3 zero = Vector3.zero;
			if (this.targetNode == this.currentNode)
			{
				zero = this.itemController.Target.transform.position;
			}
			else
			{
				zero = this.targetNode.node.transform.position;
			}
			vector = zero - base.transform.position;
			vector.y = 0f;
			vector.Normalize();
			Debug.DrawLine(base.transform.position, zero, Color.white);
		}
		else if (!GameManager.IsGamePaused)
		{
			vector = new Vector3(this.gamePlayer.RewiredPlayer.GetAxis(InputActions.Horizontal), 0f, this.gamePlayer.RewiredPlayer.GetAxis(InputActions.Vertical));
		}
		Vector3 normalized = vector.normalized;
		normalized.x *= Mathf.Abs(vector.x);
		normalized.z *= Mathf.Abs(vector.z);
		Vector3 vector3 = normalized;
		vector3.x = normalized.z;
		vector3.z = -normalized.x;
		this.rb.velocity += Vector3.up * this.gravity * Time.deltaTime;
		this.CheckGrounded();
		if (!this.gamePlayer.IsAI && !GameManager.IsGamePaused && this.gamePlayer.RewiredPlayer.GetButtonDown(InputActions.Accept) && this.CanJump())
		{
			this.rb.velocity = new Vector3(this.rb.velocity.x, this.jumpVelocity, this.rb.velocity.z);
			this.jumping = true;
		}
		if (vector3 != Vector3.zero)
		{
			Quaternion quaternion = Quaternion.LookRotation(vector3);
			Quaternion to = Quaternion.Euler(base.transform.rotation.eulerAngles.x, quaternion.eulerAngles.y, base.transform.rotation.eulerAngles.z);
			base.transform.rotation = Quaternion.RotateTowards(base.transform.rotation, to, this.turnSpeed * Time.deltaTime);
			this.rb.AddTorque(vector3 * this.torqueAcceleration * Time.deltaTime * d, ForceMode.Impulse);
		}
	}

	// Token: 0x0600178B RID: 6027 RVA: 0x0001188B File Offset: 0x0000FA8B
	private bool CanJump()
	{
		return this.isGrounded || (!this.jumping && Time.time - this.lastGroundedTime < this.jumpGraceTime);
	}

	// Token: 0x0600178C RID: 6028 RVA: 0x000A3968 File Offset: 0x000A1B68
	private void CheckGrounded()
	{
		if (this.rb.velocity.y > 0.5f)
		{
			this.isGrounded = false;
			this.lastGroundedTime = Time.time;
			return;
		}
		bool flag = this.isGrounded;
		float y = this.c.bounds.extents.y;
		this.isGrounded = Physics.Raycast(base.transform.position, -Vector3.up, y + 0.1f, this.hitMask, QueryTriggerInteraction.Ignore);
		Debug.DrawLine(base.transform.position, base.transform.position - Vector3.up * (y + 0.1f), Color.red);
		if (this.isGrounded)
		{
			this.lastGroundedTime = Time.time;
			this.jumping = false;
			if (!flag)
			{
				AudioSystem.PlayOneShot(this.hitSounds[UnityEngine.Random.Range(0, this.hitSounds.Length - 1)], 0.75f, 0.15f, 1f);
			}
		}
	}

	// Token: 0x0600178D RID: 6029 RVA: 0x000118B5 File Offset: 0x0000FAB5
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void ExplodeRPC(NetPlayer sender, byte[] hitPlayers, byte[] hitDamage)
	{
		this.Explode(hitPlayers, hitDamage);
	}

	// Token: 0x0600178E RID: 6030 RVA: 0x000A3A70 File Offset: 0x000A1C70
	public void OwnerExplode()
	{
		List<byte> list = new List<byte>();
		List<byte> list2 = new List<byte>();
		Collider[] array = Physics.OverlapSphere(base.transform.position, 5f, 256, QueryTriggerInteraction.Collide);
		for (int i = 0; i < array.Length; i++)
		{
			BoardActor componentInParent = array[i].gameObject.GetComponentInParent<BoardActor>();
			if (componentInParent != null && componentInParent != this.gamePlayer.BoardObject && componentInParent.LocalHealth > 0)
			{
				list.Add(componentInParent.ActorID);
				list2.Add((byte)this.itemController.Rand.Next(14, 16));
			}
		}
		if (list.Count == 3 && this.gamePlayer.IsLocalPlayer && !this.gamePlayer.IsAI)
		{
			PlatformAchievementManager.Instance.TriggerAchievement("ACH_THE_TRIFECTA");
		}
		this.Explode(list.ToArray(), list2.ToArray());
	}

	// Token: 0x0600178F RID: 6031 RVA: 0x000A3B58 File Offset: 0x000A1D58
	private void Explode(byte[] hitPlayers, byte[] hitDamage)
	{
		Debug.Log("Using Eggplant");
		if (base.IsOwner)
		{
			base.SendRPC("ExplodeRPC", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				hitPlayers,
				hitDamage
			});
		}
		this.isDead = true;
		for (int i = 0; i < hitPlayers.Length; i++)
		{
			DamageInstance d = new DamageInstance
			{
				damage = (int)hitDamage[i],
				origin = base.transform.position,
				blood = true,
				ragdoll = true,
				ragdollVel = 15f,
				bloodVel = 20f,
				bloodAmount = 1f,
				details = "Egg plant",
				killer = this.gamePlayer.BoardObject,
				removeKeys = true
			};
			GameManager.Board.GetActor(hitPlayers[i]).ApplyDamage(d);
		}
		this.rb.isKinematic = true;
		this.eggplantVisual.SetActive(false);
		UnityEngine.Object.Instantiate<GameObject>(this.explosionPrefab, base.transform.position, Quaternion.identity);
		if (GameManager.BoardRoot != null)
		{
			UnityEngine.Object.Instantiate<GameObject>(this.scorchMarkDecal, base.transform.position, Quaternion.Euler(90f, 0f, 0f), GameManager.BoardRoot.transform);
		}
		AudioSystem.PlayOneShot(this.explodeSound, 0.5f, 0f, 1f);
		this.fuseAudioSource.Stop();
		GameManager.Board.boardCamera.AddShake(0.6f);
		base.StartCoroutine(this.KillDelay());
	}

	// Token: 0x06001790 RID: 6032 RVA: 0x000118BF File Offset: 0x0000FABF
	private IEnumerator KillDelay()
	{
		yield return new WaitForSeconds(2f);
		this.itemController.Finish(false);
		yield return new WaitForSeconds(60f);
		if (NetSystem.IsServer)
		{
			NetSystem.Kill(this);
		}
		yield break;
	}

	// Token: 0x06001791 RID: 6033 RVA: 0x000118CE File Offset: 0x0000FACE
	public void RecievePosition(object _pos)
	{
		if ((Vector3)_pos != Vector3.zero)
		{
			this.interpolator.NewPosition(_pos);
		}
	}

	// Token: 0x06001792 RID: 6034 RVA: 0x000118EE File Offset: 0x0000FAEE
	public void RecieveRotation(object _rot)
	{
		if ((Vector3)_rot != Vector3.zero)
		{
			this.rotInterpolator.NewPosition(_rot);
		}
	}

	// Token: 0x040018FC RID: 6396
	[Header("Eggplant Variables")]
	public float life = 10f;

	// Token: 0x040018FD RID: 6397
	[Header("Movement")]
	public float torqueAcceleration = 90f;

	// Token: 0x040018FE RID: 6398
	public float maxAngularVelocity = 14f;

	// Token: 0x040018FF RID: 6399
	public float turnSpeed = 50f;

	// Token: 0x04001900 RID: 6400
	public float gravity = -30f;

	// Token: 0x04001901 RID: 6401
	public LayerMask hitMask;

	// Token: 0x04001902 RID: 6402
	public float jumpGraceTime = 0.25f;

	// Token: 0x04001903 RID: 6403
	public float jumpVelocity = 8f;

	// Token: 0x04001904 RID: 6404
	[Header("References")]
	public GameObject scorchMarkDecal;

	// Token: 0x04001905 RID: 6405
	public GameObject explosionPrefab;

	// Token: 0x04001906 RID: 6406
	public AudioClip explodeSound;

	// Token: 0x04001907 RID: 6407
	public AudioClip[] hitSounds;

	// Token: 0x04001908 RID: 6408
	public GameObject eggplantVisual;

	// Token: 0x04001909 RID: 6409
	public Rigidbody rb;

	// Token: 0x0400190A RID: 6410
	public Collider c;

	// Token: 0x0400190B RID: 6411
	public AudioSource fuseAudioSource;

	// Token: 0x0400190C RID: 6412
	private float lastGroundedTime;

	// Token: 0x0400190D RID: 6413
	private bool jumping;

	// Token: 0x0400190E RID: 6414
	private bool isGrounded;

	// Token: 0x0400190F RID: 6415
	private Interpolator interpolator;

	// Token: 0x04001910 RID: 6416
	private Interpolator rotInterpolator;

	// Token: 0x04001911 RID: 6417
	private GamePlayer gamePlayer;

	// Token: 0x04001912 RID: 6418
	private bool isDead;

	// Token: 0x04001913 RID: 6419
	private EggplantItem itemController;

	// Token: 0x04001914 RID: 6420
	private float spawnTime;

	// Token: 0x04001915 RID: 6421
	public SearchNode currentNode;

	// Token: 0x04001916 RID: 6422
	public SearchNode targetNode;

	// Token: 0x04001917 RID: 6423
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.ALWAYS_SEND)]
	public NetVec3 position = new NetVec3(Vector3.zero);

	// Token: 0x04001918 RID: 6424
	[NetSend(-1, NetSendOwner.OWNER, NetSendFlags.ALWAYS_SEND)]
	public NetVec3 rotation = new NetVec3(Vector3.zero);
}
