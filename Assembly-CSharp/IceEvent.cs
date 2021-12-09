using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000AF RID: 175
public class IceEvent : BoardNodeEvent
{
	// Token: 0x060003AA RID: 938 RVA: 0x00006042 File Offset: 0x00004242
	public void Start()
	{
		this.startHeight = this.waterFloat.floatHeight;
	}

	// Token: 0x060003AB RID: 939 RVA: 0x00006055 File Offset: 0x00004255
	public override IEnumerator DoEvent(BoardPlayer player, BoardNode node, int seed)
	{
		AudioSystem.PlayOneShot(this.breakClip, 1f, 0f, 1f);
		Vector3 startPosition = base.transform.position;
		float startTime = Time.time;
		float scale = 0.125f;
		float x = 0f;
		float y = 0f;
		player.GroundSnap = false;
		Vector3 playerOffset = player.transform.position - base.transform.position;
		while (Time.time < startTime + 1.5f)
		{
			x += Time.deltaTime * 9f;
			y += Time.deltaTime * 9f;
			float x2 = (Mathf.PerlinNoise(x, 0f) - 0.5f) * scale;
			float z = (Mathf.PerlinNoise(0f, y) - 0.5f) * scale;
			base.transform.position = startPosition + new Vector3(x2, 0f, z);
			player.transform.position = base.transform.position + playerOffset;
			yield return new WaitForEndOfFrame();
		}
		base.transform.position = startPosition;
		player.GroundSnap = true;
		float curHeight = this.startHeight;
		bool damaged = false;
		while (curHeight > this.startHeight - this.sinkLength)
		{
			curHeight -= Time.deltaTime * this.sinkSpeed;
			this.waterFloat.floatHeight = curHeight;
			if (!damaged && (double)curHeight < (double)this.startHeight - (double)this.sinkLength * 0.6)
			{
				AudioSystem.PlayOneShot(this.fallClip, 1.5f, 0f, 1f);
				DamageInstance d = new DamageInstance
				{
					damage = 6,
					origin = base.transform.position,
					blood = false,
					ragdoll = true,
					ragdollVel = 0f,
					bloodVel = 5f,
					bloodAmount = 1f,
					sound = true,
					volume = 0.3f,
					details = "Ice Hazard",
					removeKeys = true
				};
				player.ApplyDamage(d);
				damaged = true;
			}
			yield return null;
		}
		yield return new WaitForSeconds(1f);
		this.coroutine = base.StartCoroutine(this.Reset(curHeight));
		yield break;
	}

	// Token: 0x060003AC RID: 940 RVA: 0x0000606B File Offset: 0x0000426B
	private void OnDisable()
	{
		if (this.coroutine != null)
		{
			base.StopCoroutine(this.coroutine);
			this.coroutine = null;
			this.waterFloat.floatHeight = this.startHeight;
		}
	}

	// Token: 0x060003AD RID: 941 RVA: 0x00006099 File Offset: 0x00004299
	private IEnumerator Reset(float curHeight)
	{
		yield return new WaitForSeconds(1.5f);
		while (curHeight < this.startHeight)
		{
			curHeight += Time.deltaTime * this.sinkSpeed;
			this.waterFloat.floatHeight = curHeight;
			yield return null;
		}
		this.waterFloat.floatHeight = this.startHeight;
		this.coroutine = null;
		yield break;
	}

	// Token: 0x040003C7 RID: 967
	public AudioClip breakClip;

	// Token: 0x040003C8 RID: 968
	public AudioClip fallClip;

	// Token: 0x040003C9 RID: 969
	public LowPolyWaterFloat waterFloat;

	// Token: 0x040003CA RID: 970
	public float sinkLength = 0.9f;

	// Token: 0x040003CB RID: 971
	public float sinkSpeed = 2f;

	// Token: 0x040003CC RID: 972
	private float startHeight;

	// Token: 0x040003CD RID: 973
	private Coroutine coroutine;
}
