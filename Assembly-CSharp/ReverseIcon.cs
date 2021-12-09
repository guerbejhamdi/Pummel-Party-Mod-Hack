using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000320 RID: 800
public class ReverseIcon : MonoBehaviour
{
	// Token: 0x060015DB RID: 5595 RVA: 0x0001090B File Offset: 0x0000EB0B
	private void Start()
	{
		base.StartCoroutine(this.Spawn());
	}

	// Token: 0x060015DC RID: 5596 RVA: 0x0001091A File Offset: 0x0000EB1A
	private IEnumerator Spawn()
	{
		float startTime = Time.time;
		while (Time.time - startTime < 0.5f)
		{
			float d = this.inCurve.Evaluate((Time.time - startTime) * 2f);
			base.transform.localScale = Vector3.one * d;
			yield return null;
		}
		yield return new WaitForSeconds(1f);
		startTime = Time.time;
		while (startTime - Time.time < 0.5f)
		{
			float d2 = this.outCurve.Evaluate((Time.time - startTime) * 2f);
			base.transform.localScale = Vector3.one * d2;
			yield return null;
		}
		yield return new WaitForSeconds(0f);
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x060015DD RID: 5597 RVA: 0x00010929 File Offset: 0x0000EB29
	private void Update()
	{
		base.transform.Rotate(new Vector3(0f, 0f, 90f * Time.deltaTime));
	}

	// Token: 0x04001701 RID: 5889
	public AnimationCurve inCurve;

	// Token: 0x04001702 RID: 5890
	public AnimationCurve outCurve;
}
