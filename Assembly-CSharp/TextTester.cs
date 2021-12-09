using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004D8 RID: 1240
public class TextTester : MonoBehaviour
{
	// Token: 0x060020C6 RID: 8390 RVA: 0x00017D6A File Offset: 0x00015F6A
	private IEnumerator Start()
	{
		yield return null;
		yield break;
	}

	// Token: 0x060020C7 RID: 8391 RVA: 0x000CCDF4 File Offset: 0x000CAFF4
	private void Spawn(GameObject g)
	{
		GameObject item = UnityEngine.Object.Instantiate<GameObject>(g, g.transform.position, Quaternion.identity, base.transform);
		this.spawned.Add(item);
		this.spawnTimes.Add(Time.time);
	}

	// Token: 0x060020C8 RID: 8392 RVA: 0x000CCE3C File Offset: 0x000CB03C
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			this.tmpParent.SetActive(!this.tmpParent.activeSelf);
			this.unityParent.SetActive(!this.unityParent.activeSelf);
		}
	}

	// Token: 0x0400238D RID: 9101
	public GameObject tmp;

	// Token: 0x0400238E RID: 9102
	public GameObject unity;

	// Token: 0x0400238F RID: 9103
	public bool spawnTmp = true;

	// Token: 0x04002390 RID: 9104
	public bool spawnUnity = true;

	// Token: 0x04002391 RID: 9105
	public float spawnInterval = 0.1f;

	// Token: 0x04002392 RID: 9106
	public float upSpeed = 1f;

	// Token: 0x04002393 RID: 9107
	public AnimationCurve scaleCurve;

	// Token: 0x04002394 RID: 9108
	public float lifeTime = 3f;

	// Token: 0x04002395 RID: 9109
	public List<GameObject> spawned = new List<GameObject>();

	// Token: 0x04002396 RID: 9110
	public List<float> spawnTimes = new List<float>();

	// Token: 0x04002397 RID: 9111
	public GameObject unityParent;

	// Token: 0x04002398 RID: 9112
	public GameObject tmpParent;
}
