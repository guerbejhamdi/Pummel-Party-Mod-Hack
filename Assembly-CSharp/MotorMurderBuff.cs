using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001DD RID: 477
public class MotorMurderBuff : MonoBehaviour
{
	// Token: 0x17000137 RID: 311
	// (get) Token: 0x06000DD2 RID: 3538 RVA: 0x0000C68E File Offset: 0x0000A88E
	// (set) Token: 0x06000DD3 RID: 3539 RVA: 0x0000C696 File Offset: 0x0000A896
	public byte Type
	{
		get
		{
			return this.type;
		}
		set
		{
			this.type = value;
			this.visuals[(int)(this.type - 1)].SetActive(true);
		}
	}

	// Token: 0x17000138 RID: 312
	// (get) Token: 0x06000DD4 RID: 3540 RVA: 0x0000C6B4 File Offset: 0x0000A8B4
	// (set) Token: 0x06000DD5 RID: 3541 RVA: 0x0000C6BC File Offset: 0x0000A8BC
	public byte ID { get; set; }

	// Token: 0x17000139 RID: 313
	// (get) Token: 0x06000DD6 RID: 3542 RVA: 0x0000C6C5 File Offset: 0x0000A8C5
	// (set) Token: 0x06000DD7 RID: 3543 RVA: 0x0000C6CD File Offset: 0x0000A8CD
	public bool Despawning { get; set; }

	// Token: 0x1700013A RID: 314
	// (get) Token: 0x06000DD8 RID: 3544 RVA: 0x0000C6D6 File Offset: 0x0000A8D6
	// (set) Token: 0x06000DD9 RID: 3545 RVA: 0x0000C6DE File Offset: 0x0000A8DE
	public byte SpawnPointID { get; set; }

	// Token: 0x06000DDA RID: 3546 RVA: 0x0000C6E7 File Offset: 0x0000A8E7
	private void Start()
	{
		base.StartCoroutine(this.Animate(this.spawnTime, this.spawnCurve, false));
	}

	// Token: 0x06000DDB RID: 3547 RVA: 0x0000398C File Offset: 0x00001B8C
	private void Update()
	{
	}

	// Token: 0x06000DDC RID: 3548 RVA: 0x0000C703 File Offset: 0x0000A903
	public void Despawn()
	{
		if (this.Despawning)
		{
			return;
		}
		this.Despawning = true;
		this.sphereCollider.enabled = false;
		base.StartCoroutine(this.Animate(this.despawnTime, this.despawnCurve, true));
	}

	// Token: 0x06000DDD RID: 3549 RVA: 0x0000C73B File Offset: 0x0000A93B
	private IEnumerator Animate(float length, AnimationCurve curve, bool destroy)
	{
		float startTime = Time.time;
		while (Time.time - startTime < length)
		{
			base.transform.localScale = Vector3.one * curve.Evaluate((Time.time - startTime) / length);
			yield return null;
		}
		if (destroy)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		yield break;
	}

	// Token: 0x04000D3C RID: 3388
	public SphereCollider sphereCollider;

	// Token: 0x04000D3D RID: 3389
	public GameObject[] visuals;

	// Token: 0x04000D3E RID: 3390
	public float despawnTime = 1f;

	// Token: 0x04000D3F RID: 3391
	public AnimationCurve despawnCurve;

	// Token: 0x04000D40 RID: 3392
	public float spawnTime = 1f;

	// Token: 0x04000D41 RID: 3393
	public AnimationCurve spawnCurve;

	// Token: 0x04000D42 RID: 3394
	private byte type;
}
