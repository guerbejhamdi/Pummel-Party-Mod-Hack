using System;
using UnityEngine;

// Token: 0x020000F2 RID: 242
public class LeanAudioStream
{
	// Token: 0x06000645 RID: 1605 RVA: 0x00008460 File Offset: 0x00006660
	public LeanAudioStream(float[] audioArr)
	{
		this.audioArr = audioArr;
	}

	// Token: 0x06000646 RID: 1606 RVA: 0x00045C20 File Offset: 0x00043E20
	public void OnAudioRead(float[] data)
	{
		for (int i = 0; i < data.Length; i++)
		{
			data[i] = this.audioArr[this.position];
			this.position++;
		}
	}

	// Token: 0x06000647 RID: 1607 RVA: 0x0000846F File Offset: 0x0000666F
	public void OnAudioSetPosition(int newPosition)
	{
		this.position = newPosition;
	}

	// Token: 0x0400056B RID: 1387
	public int position;

	// Token: 0x0400056C RID: 1388
	public AudioClip audioClip;

	// Token: 0x0400056D RID: 1389
	public float[] audioArr;
}
