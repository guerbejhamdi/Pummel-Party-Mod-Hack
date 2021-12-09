using System;
using System.Collections;
using UnityEngine;
using ZP.Net;
using ZP.Utility;

// Token: 0x0200014D RID: 333
public class BattyBatterPlayer : CharacterBase
{
	// Token: 0x170000D9 RID: 217
	// (get) Token: 0x06000983 RID: 2435 RVA: 0x0000A534 File Offset: 0x00008734
	// (set) Token: 0x06000984 RID: 2436 RVA: 0x0000A53C File Offset: 0x0000873C
	public bool canHit { get; set; }

	// Token: 0x06000985 RID: 2437 RVA: 0x000557B4 File Offset: 0x000539B4
	public override void OnNetInitialize()
	{
		base.OnNetInitialize();
		this.minigameController = (BattyBatterController)GameManager.Minigame;
		this.minigameController.AddPlayer(this);
		Transform transform = this.minigameController.Root.transform.Find("CameraParent");
		Transform transform2 = this.minigameController.Root.transform.Find("4PlayerCamTransform");
		Transform transform3 = this.minigameController.Root.transform.Find("8PlayerCamTransform");
		transform.position = ((GameManager.GetPlayerCount() <= 4) ? transform2.position : transform3.position);
		transform.rotation = ((GameManager.GetPlayerCount() <= 4) ? transform2.rotation : transform3.rotation);
		this.GetHitPoint();
	}

	// Token: 0x06000986 RID: 2438 RVA: 0x0000A545 File Offset: 0x00008745
	public override void FinishedSpawning()
	{
		base.FinishedSpawning();
		this.playerAnim.Animator.SetBool("Baseball", true);
	}

	// Token: 0x06000987 RID: 2439 RVA: 0x00055870 File Offset: 0x00053A70
	private void GetHitPoint()
	{
		this.aiHitOffset = UnityEngine.Random.Range(this.minHitRange[(int)base.GamePlayer.Difficulty], this.maxHitRange[(int)base.GamePlayer.Difficulty]);
		this.aiHitOffset *= ((UnityEngine.Random.value > 0.5f) ? -1f : 1f);
	}

	// Token: 0x06000988 RID: 2440 RVA: 0x000558D4 File Offset: 0x00053AD4
	private void Update()
	{
		if (this.minigameController.Playable && base.IsOwner && this.canHit)
		{
			if (!base.GamePlayer.IsAI)
			{
				if (base.GamePlayer.RewiredPlayer.GetButtonDown(InputActions.Accept))
				{
					this.LocalSwing();
					return;
				}
			}
			else
			{
				Vector3 vector = this.minigameController.ballHitPoints[(int)base.OwnerSlot];
				vector.y += this.aiHitOffset;
				if (this.minigameController.currentBalls[(int)base.OwnerSlot].transform.position.y <= vector.y)
				{
					this.LocalSwing();
					this.GetHitPoint();
				}
			}
		}
	}

	// Token: 0x06000989 RID: 2441 RVA: 0x0000A563 File Offset: 0x00008763
	[NetRPC(true, NetRPCSecurity.OWNER, NetRPCSecurity.ALL)]
	public void RPCSwing(NetPlayer sender, float hitY)
	{
		if (this.lastRoutine != null)
		{
			base.StopCoroutine(this.lastRoutine);
		}
		this.lastRoutine = base.StartCoroutine(this.Swing(hitY));
	}

	// Token: 0x0600098A RID: 2442 RVA: 0x00055990 File Offset: 0x00053B90
	private void LocalSwing()
	{
		this.canHit = false;
		if (this.lastRoutine != null)
		{
			base.StopCoroutine(this.lastRoutine);
		}
		this.lastRoutine = base.StartCoroutine(this.Swing(this.minigameController.currentBalls[(int)base.OwnerSlot].transform.position.y));
	}

	// Token: 0x0600098B RID: 2443 RVA: 0x0000A58C File Offset: 0x0000878C
	private IEnumerator Swing(float hitY)
	{
		this.batCollider.enabled = false;
		if (this.batRef.transform.parent == null)
		{
			this.ResetBat(true);
		}
		if (base.IsOwner)
		{
			base.SendRPC("RPCSwing", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				this.minigameController.currentBalls[(int)base.OwnerSlot].transform.position.y
			});
		}
		float num = Mathf.Abs(this.minigameController.ballHitPoints[(int)base.OwnerSlot].y - hitY);
		bool hit = num <= this.maxDistance;
		Vector3 position = this.minigameController.currentBalls[(int)base.OwnerSlot].transform.position;
		position.y = hitY;
		this.minigameController.Spawn(this.hitMarkerPrefab, position, Quaternion.identity).GetComponent<MeshRenderer>().material.SetColor("_TintColor", hit ? ((num < 0.1f) ? this.markerPerfect : this.markerGood) : this.markerBad);
		short num2 = 0;
		if (num < 0.3f)
		{
			if (num < 0.1f)
			{
				num2 = (short)(25f + (1f - num / 0.1f) * 75f);
			}
			else
			{
				float num3 = num - 0.1f;
				num2 = (short)((1f - num3 / 0.2f) * 25f);
			}
			if (NetSystem.IsServer)
			{
				this.Score += num2;
			}
		}
		GameManager.UIController.SpawnWorldText("+" + num2.ToString(), base.transform.position + new Vector3(0f, 1f, 0f), 3f, hit ? ((num < 0.1f) ? WorldTextType.SwiftShootersGood : WorldTextType.SwiftShootersExcellent) : WorldTextType.SwiftShootersBomb, 0f, this.minigameController.MinigameCamera);
		AudioSystem.PlayOneShot(this.swingClip, this.swingVol, 0f, 1f);
		this.playerAnim.Animator.SetTrigger("BaseballSwing");
		this.playerAnim.Animator.SetBool("Baseball", false);
		if (hit)
		{
			this.minigameController.currentBalls[(int)base.OwnerSlot].Hit();
		}
		yield return new WaitForSeconds(this.hitSoundTime);
		if (hit)
		{
			AudioSystem.PlayOneShot(this.hitClip, this.hitVol, 0f, 1f);
		}
		yield return new WaitForSeconds(this.batThrowTime - this.hitSoundTime);
		this.parent = this.batRef.transform.parent;
		this.prePos = this.batRef.transform.localPosition;
		this.preRot = this.batRef.transform.localRotation;
		this.batRef.transform.parent = null;
		this.batRb.isKinematic = false;
		this.batRb.velocity = Vector3.zero;
		this.batRb.angularVelocity = Vector3.zero;
		Vector3 normalized = ZPMath.RandomVec3(GameManager.rand, -1f, 1f).normalized;
		normalized = (-Vector3.right + normalized * 0.25f).normalized;
		this.batRb.AddForce(normalized * ZPMath.RandomFloat(GameManager.rand, this.minThrowStrength, this.maxThrowStrength));
		this.batRb.maxAngularVelocity = 1000f;
		this.batRb.AddRelativeTorque(ZPMath.RandomFloat(GameManager.rand, this.minRotStrength, this.maxRotStrength), 0f, 0f);
		yield return new WaitForSeconds(0.03f);
		yield return new WaitForSeconds(this.resetTime - 0.03f);
		this.ResetBat(false);
		yield break;
	}

	// Token: 0x0600098C RID: 2444 RVA: 0x000559EC File Offset: 0x00053BEC
	private void ResetBat(bool instant)
	{
		this.batRb.isKinematic = true;
		this.batRb.velocity = Vector3.zero;
		this.batRb.angularVelocity = Vector3.zero;
		this.batRef.transform.parent = this.parent;
		this.batRef.transform.localPosition = this.prePos;
		this.batRef.transform.localRotation = this.preRot;
		this.batGraphic.transform.localScale = Vector3.one * 0.1f;
		LeanTween.cancel(this.batGraphic);
		if (instant)
		{
			this.batGraphic.transform.localScale = Vector3.one;
			return;
		}
		LeanTween.scale(this.batGraphic, Vector3.one, this.resizeTime);
	}

	// Token: 0x0600098D RID: 2445 RVA: 0x00055AC4 File Offset: 0x00053CC4
	public BattyBatterPlayer()
	{
		float[] array = new float[3];
		array[0] = 0.1f;
		array[1] = 0.05f;
		this.minHitRange = array;
		this.maxHitRange = new float[]
		{
			0.6f,
			0.35f,
			0.2f
		};
		base..ctor();
	}

	// Token: 0x04000828 RID: 2088
	public GameObject hitMarkerPrefab;

	// Token: 0x04000829 RID: 2089
	public Color markerPerfect;

	// Token: 0x0400082A RID: 2090
	public Color markerGood;

	// Token: 0x0400082B RID: 2091
	public Color markerBad;

	// Token: 0x0400082C RID: 2092
	public float batThrowTime = 0.1f;

	// Token: 0x0400082D RID: 2093
	public float hitSoundTime = 0.05f;

	// Token: 0x0400082E RID: 2094
	public float resetTime = 1f;

	// Token: 0x0400082F RID: 2095
	public float resizeTime = 0.25f;

	// Token: 0x04000830 RID: 2096
	public GameObject batRef;

	// Token: 0x04000831 RID: 2097
	public GameObject batGraphic;

	// Token: 0x04000832 RID: 2098
	public Rigidbody batRb;

	// Token: 0x04000833 RID: 2099
	public AudioClip swingClip;

	// Token: 0x04000834 RID: 2100
	public AudioClip hitClip;

	// Token: 0x04000835 RID: 2101
	public float swingVol = 1f;

	// Token: 0x04000836 RID: 2102
	public float hitVol = 1f;

	// Token: 0x04000837 RID: 2103
	public CapsuleCollider batCollider;

	// Token: 0x04000838 RID: 2104
	public float minThrowStrength = 1200f;

	// Token: 0x04000839 RID: 2105
	public float maxThrowStrength = 1400f;

	// Token: 0x0400083A RID: 2106
	public float minRotStrength = -140f;

	// Token: 0x0400083B RID: 2107
	public float maxRotStrength = -110f;

	// Token: 0x0400083C RID: 2108
	private BattyBatterController minigameController;

	// Token: 0x0400083E RID: 2110
	private float maxDistance = 0.3f;

	// Token: 0x0400083F RID: 2111
	private float[] minHitRange;

	// Token: 0x04000840 RID: 2112
	private float[] maxHitRange;

	// Token: 0x04000841 RID: 2113
	private float aiHitOffset;

	// Token: 0x04000842 RID: 2114
	private Coroutine lastRoutine;

	// Token: 0x04000843 RID: 2115
	private Transform parent;

	// Token: 0x04000844 RID: 2116
	private Vector3 prePos;

	// Token: 0x04000845 RID: 2117
	private Quaternion preRot;
}
