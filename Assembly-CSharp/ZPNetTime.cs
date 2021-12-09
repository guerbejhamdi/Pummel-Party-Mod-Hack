using System;
using UnityEngine;

// Token: 0x02000487 RID: 1159
public class ZPNetTime
{
	// Token: 0x170003B0 RID: 944
	// (get) Token: 0x06001F3D RID: 7997 RVA: 0x00016EF6 File Offset: 0x000150F6
	public float TimeOffset
	{
		get
		{
			return this.offset;
		}
	}

	// Token: 0x170003B1 RID: 945
	// (get) Token: 0x06001F3E RID: 7998 RVA: 0x00016EFE File Offset: 0x000150FE
	public float GameTime
	{
		get
		{
			return Time.time + this.offset;
		}
	}

	// Token: 0x170003B2 RID: 946
	// (get) Token: 0x06001F3F RID: 7999 RVA: 0x00016F0C File Offset: 0x0001510C
	public float LocalTime
	{
		get
		{
			return Time.time;
		}
	}

	// Token: 0x06001F40 RID: 8000 RVA: 0x00016F13 File Offset: 0x00015113
	public void UpdateTime()
	{
		this.local_time = Time.time;
		this.game_time = this.local_time + this.offset;
	}

	// Token: 0x06001F41 RID: 8001 RVA: 0x000C78E4 File Offset: 0x000C5AE4
	public void UpdateOffset(float remote_time, float rtt)
	{
		float num = remote_time - Time.time + rtt * 0.5f;
		if (this.offset == 0f)
		{
			this.offset = num;
		}
		else
		{
			this.offset = this.offset * 0.95f + num * 0.05f;
		}
		this.UpdateTime();
	}

	// Token: 0x04002228 RID: 8744
	private float offset;

	// Token: 0x04002229 RID: 8745
	private float game_time;

	// Token: 0x0400222A RID: 8746
	private float local_time;
}
