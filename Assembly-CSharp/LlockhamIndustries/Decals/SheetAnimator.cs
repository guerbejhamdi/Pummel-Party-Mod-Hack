using System;
using UnityEngine;

namespace LlockhamIndustries.Decals
{
	// Token: 0x02000893 RID: 2195
	[RequireComponent(typeof(ProjectionRenderer))]
	public class SheetAnimator : MonoBehaviour
	{
		// Token: 0x06003E7A RID: 15994 RVA: 0x0002A03C File Offset: 0x0002823C
		private void Awake()
		{
			this.projection = base.GetComponent<ProjectionRenderer>();
		}

		// Token: 0x06003E7B RID: 15995 RVA: 0x00133D18 File Offset: 0x00131F18
		private void Update()
		{
			int num = this.collumns * this.rows - (this.skipFirst + this.skipLast);
			if (!this.paused)
			{
				this.time += Time.deltaTime * this.speed;
			}
			if (this.time > (float)num)
			{
				if (this.destroyOnComplete)
				{
					this.projection.Destroy();
					return;
				}
				this.time -= (float)num;
			}
			int num2 = this.skipFirst + Mathf.FloorToInt(this.time);
			Vector2 vector = new Vector2(1f / (float)this.collumns, 1f / (float)this.rows);
			int num3 = num2 / this.collumns;
			int num4 = num2 % this.collumns;
			float x = vector.x * (float)num4;
			float num5 = vector.y * (float)num3;
			if (!this.invertY)
			{
				num5 = 1f - vector.y - num5;
			}
			this.projection.Tiling = new Vector2(vector.x, vector.y);
			this.projection.Offset = new Vector2(x, num5);
			this.projection.UpdateProperties();
		}

		// Token: 0x06003E7C RID: 15996 RVA: 0x0002A04A File Offset: 0x0002824A
		public void Play()
		{
			this.paused = false;
		}

		// Token: 0x06003E7D RID: 15997 RVA: 0x0002A053 File Offset: 0x00028253
		public void Pause()
		{
			this.paused = true;
		}

		// Token: 0x06003E7E RID: 15998 RVA: 0x0002A05C File Offset: 0x0002825C
		public void Stop()
		{
			this.paused = true;
			this.time = 0f;
		}

		// Token: 0x04003A9E RID: 15006
		[Header("Basics")]
		[Tooltip("The number of collumns in the sprite sheet being sampled.")]
		public int collumns = 1;

		// Token: 0x04003A9F RID: 15007
		[Tooltip("The number of rows in the sprite sheet being sampled.")]
		public int rows = 1;

		// Token: 0x04003AA0 RID: 15008
		[Tooltip("The playback speed, in frames per second.")]
		public float speed = 30f;

		// Token: 0x04003AA1 RID: 15009
		[Header("Advanced")]
		[Tooltip("Skip the first x frames of the animation.")]
		public int skipFirst;

		// Token: 0x04003AA2 RID: 15010
		[Tooltip("Skip the last x frames of the animation.")]
		public int skipLast;

		// Token: 0x04003AA3 RID: 15011
		[Tooltip("Sample frames from the bottom instead of the top.")]
		public bool invertY;

		// Token: 0x04003AA4 RID: 15012
		[Tooltip("Destroy the projection when the animator has finished its first loop.")]
		public bool destroyOnComplete;

		// Token: 0x04003AA5 RID: 15013
		private ProjectionRenderer projection;

		// Token: 0x04003AA6 RID: 15014
		private float time;

		// Token: 0x04003AA7 RID: 15015
		private bool paused;
	}
}
