using System;
using I2.Loc;
using UnityEngine;

// Token: 0x02000068 RID: 104
public class GetGameVersion : MonoBehaviour
{
	// Token: 0x060001F1 RID: 497 RVA: 0x00004CA7 File Offset: 0x00002EA7
	private void Awake()
	{
		this.localize.TermSuffix = " " + GameManager.VERSION;
		Debug.Log("Version " + GameManager.VERSION);
	}

	// Token: 0x04000253 RID: 595
	public Localize localize;
}
