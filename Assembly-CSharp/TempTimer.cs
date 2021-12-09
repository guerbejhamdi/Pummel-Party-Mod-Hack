using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020004D1 RID: 1233
public class TempTimer : MonoBehaviour
{
	// Token: 0x060020AE RID: 8366 RVA: 0x0000398C File Offset: 0x00001B8C
	private void Start()
	{
	}

	// Token: 0x060020AF RID: 8367 RVA: 0x000CCAA4 File Offset: 0x000CACA4
	private void Update()
	{
		TimeSpan timeSpan = DateTime.Now.Subtract(GameManager.startTime);
		this.text.text = string.Concat(new string[]
		{
			"Total Turns: ",
			GameManager.totalTurns.ToString(),
			" Time: ",
			timeSpan.Hours.ToString(),
			"h ",
			timeSpan.Minutes.ToString(),
			"m"
		});
	}

	// Token: 0x0400237A RID: 9082
	public Text text;
}
