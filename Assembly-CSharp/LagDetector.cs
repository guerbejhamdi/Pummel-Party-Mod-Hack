using System;
using UnityEngine;

// Token: 0x020000C2 RID: 194
public class LagDetector : MonoBehaviour
{
	// Token: 0x060003F6 RID: 1014 RVA: 0x00006303 File Offset: 0x00004503
	private void Start()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		this.lastTime = Time.realtimeSinceStartup;
	}

	// Token: 0x060003F7 RID: 1015 RVA: 0x0003BC44 File Offset: 0x00039E44
	private void Update()
	{
		float num = Time.realtimeSinceStartup - this.lastTime;
		if (num > 0.15f)
		{
			Debug.LogError("Lag Detected: " + num.ToString() + " Seconds");
		}
		this.lastTime = Time.realtimeSinceStartup;
	}

	// Token: 0x0400044D RID: 1101
	private float lastTime;
}
