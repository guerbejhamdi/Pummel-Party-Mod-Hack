using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000543 RID: 1347
public class UICountdownText : MonoBehaviour
{
	// Token: 0x060023AF RID: 9135 RVA: 0x00019BE2 File Offset: 0x00017DE2
	private void Awake()
	{
		this.ui_text = base.GetComponent<Text>();
		AudioSystem.PlayOneShot("CountdownTick01", 0.5f, 0f);
	}

	// Token: 0x060023B0 RID: 9136 RVA: 0x00019C04 File Offset: 0x00017E04
	public void Init(string str)
	{
		this.ui_text.text = str;
	}

	// Token: 0x0400268C RID: 9868
	private Text ui_text;
}
