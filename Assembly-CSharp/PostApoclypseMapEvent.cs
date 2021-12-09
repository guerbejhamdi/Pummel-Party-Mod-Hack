using System;
using System.Collections;
using UnityEngine;
using ZP.Utility;

// Token: 0x020002F5 RID: 757
public class PostApoclypseMapEvent : MainBoardEvent
{
	// Token: 0x06001512 RID: 5394 RVA: 0x0001015C File Offset: 0x0000E35C
	public override IEnumerator DoEvent(BoardPlayer player, BoardNode boardNode, int seed)
	{
		base.DoGenericBoardEventActions();
		this.rand = new System.Random(seed);
		if (GameManager.Board.curMainBoardEvent == null)
		{
			GameManager.Board.curMainBoardEvent = this;
			this.turnsRemaining = 1;
		}
		else if (this.turnsRemaining != 0)
		{
			yield break;
		}
		Debug.Log("Doing Rusty Ruins Event");
		yield return this.DoTurnStartEvent(player);
		base.Finished = false;
		yield break;
	}

	// Token: 0x06001513 RID: 5395 RVA: 0x00010179 File Offset: 0x0000E379
	public override IEnumerator DoFirstTurnEvent(BoardPlayer player)
	{
		this.turnsRemaining--;
		if (this.turnsRemaining <= 0)
		{
			GameManager.Board.curMainBoardEvent = null;
			base.Finished = false;
		}
		else
		{
			base.Finished = true;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06001514 RID: 5396 RVA: 0x00010188 File Offset: 0x0000E388
	public override IEnumerator DoTurnStartEvent(BoardPlayer player)
	{
		Debug.Log("Doing Worm Event");
		float moveSpeed = 3.1f;
		float d = ZPMath.RandomFloat(this.rand, 3f, 4f);
		Vector3 a = new Vector3(ZPMath.RandomFloat(this.rand, -1f, 1f), 0f, ZPMath.RandomFloat(this.rand, -1f, 1f));
		a.Normalize();
		Vector3 offset = a * d;
		TempAudioSource audioSource = AudioSystem.PlayLooping(this.rumbleSound, 0.25f, 1f);
		GameObject rockChunkParticle = UnityEngine.Object.Instantiate<GameObject>(this.rockChurnParticlePrefab);
		while (offset != Vector3.zero)
		{
			RaycastHit raycastHit;
			if (Physics.Raycast(player.transform.position + offset + Vector3.up * 50f, Vector3.down, out raycastHit, 100f, 1024))
			{
				rockChunkParticle.transform.position = raycastHit.point;
				rockChunkParticle.transform.rotation = Quaternion.LookRotation(raycastHit.normal);
			}
			offset = Vector3.MoveTowards(offset, Vector3.zero, Time.deltaTime * moveSpeed);
			yield return null;
		}
		rockChunkParticle.GetComponent<ParticleSystem>().Stop(false, ParticleSystemStopBehavior.StopEmitting);
		audioSource.FadeAudio(0.5f, FadeType.Out);
		yield return new WaitForSeconds(0.2f);
		AudioSystem.PlayOneShot(this.wormSound, 0.45f, 0f, 1f);
		rockChunkParticle.GetComponentInChildren<Animator>().SetTrigger("Attack");
		yield return new WaitForSeconds(0.25f);
		DamageInstance d2 = new DamageInstance
		{
			damage = this.rand.Next(5, 7),
			origin = player.transform.position - Vector3.down,
			blood = true,
			ragdoll = true,
			ragdollVel = 0.25f,
			bloodAmount = 1f,
			details = "Post Apoc Map Event -- Worm",
			removeKeys = true
		};
		player.ApplyDamage(d2);
		GameManager.Board.boardCamera.AddShake(0.35f);
		ActionTimer timer = new ActionTimer(2.5f);
		timer.Start();
		yield return new WaitUntil(() => player.PlayerState == BoardPlayerState.Idle || player.PlayerState == BoardPlayerState.GetTurnInput || (player.PlayerState == BoardPlayerState.Dying && timer.Elapsed(false)));
		GameManager.Board.CameraTrackCurrentPlayer();
		UnityEngine.Object.Destroy(rockChunkParticle);
		base.Finished = true;
		yield break;
	}

	// Token: 0x06001515 RID: 5397 RVA: 0x0001019E File Offset: 0x0000E39E
	public override int GetEventValue1()
	{
		return this.turnsRemaining;
	}

	// Token: 0x06001516 RID: 5398 RVA: 0x000101A6 File Offset: 0x0000E3A6
	public override void SetupFromLoad(int val1, int val2)
	{
		GameManager.Board.curMainBoardEvent = this;
		this.turnsRemaining = val1;
	}

	// Token: 0x0400161B RID: 5659
	public GameObject rockChurnParticlePrefab;

	// Token: 0x0400161C RID: 5660
	public AudioClip wormSound;

	// Token: 0x0400161D RID: 5661
	public AudioClip rumbleSound;

	// Token: 0x0400161E RID: 5662
	private int turnsRemaining = -10;
}
