using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000F7 RID: 247
public class LeanTester : MonoBehaviour
{
	// Token: 0x0600066E RID: 1646 RVA: 0x000085D1 File Offset: 0x000067D1
	public void Start()
	{
		base.StartCoroutine(this.timeoutCheck());
	}

	// Token: 0x0600066F RID: 1647 RVA: 0x000085E0 File Offset: 0x000067E0
	private IEnumerator timeoutCheck()
	{
		float pauseEndTime = Time.realtimeSinceStartup + this.timeout;
		while (Time.realtimeSinceStartup < pauseEndTime)
		{
			yield return 0;
		}
		if (!LeanTest.testsFinished)
		{
			Debug.Log(LeanTest.formatB("Tests timed out!"));
			LeanTest.overview();
		}
		yield break;
	}

	// Token: 0x04000580 RID: 1408
	public float timeout = 15f;
}
