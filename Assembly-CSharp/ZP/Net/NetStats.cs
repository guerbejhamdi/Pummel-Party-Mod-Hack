using System;
using UnityEngine;

namespace ZP.Net
{
	// Token: 0x0200062F RID: 1583
	public class NetStats
	{
		// Token: 0x06002918 RID: 10520 RVA: 0x0001CB22 File Offset: 0x0001AD22
		public NetStats(string name)
		{
			this.name = name;
		}

		// Token: 0x06002919 RID: 10521 RVA: 0x000F4E08 File Offset: 0x000F3008
		public void Update(int objOut, int objIn, int rpcOut, int rpcIn)
		{
			if (objOut == 0 && objIn == 0 && rpcOut == 0 && rpcIn == 0)
			{
				return;
			}
			this.maxObjOut = Mathf.Max(this.maxObjOut, objOut);
			this.maxObjIn = Mathf.Max(this.maxObjIn, objIn);
			this.maxRpcOut = Mathf.Max(this.maxRpcOut, rpcOut);
			this.maxRpcIn = Mathf.Max(this.maxRpcIn, rpcIn);
			this.totalObjOut += objOut;
			this.totalObjIn += objIn;
			this.totalRpcOut += rpcOut;
			this.totalRpcIn += rpcIn;
			this.count++;
		}

		// Token: 0x0600291A RID: 10522 RVA: 0x000F4EB4 File Offset: 0x000F30B4
		public string GetRow()
		{
			string text = "";
			int num = (int)((float)this.totalObjOut / (float)this.count);
			int num2 = (int)((float)this.totalObjIn / (float)this.count);
			int num3 = (int)((float)this.totalRpcOut / (float)this.count);
			int num4 = (int)((float)this.totalRpcIn / (float)this.count);
			text = string.Concat(new string[]
			{
				text,
				this.name,
				",",
				num.ToString(),
				",",
				num2.ToString(),
				",",
				num3.ToString(),
				",",
				num4.ToString(),
				","
			});
			return string.Concat(new string[]
			{
				text,
				this.maxObjOut.ToString(),
				",",
				this.maxObjIn.ToString(),
				",",
				this.maxRpcOut.ToString(),
				",",
				this.maxRpcIn.ToString()
			});
		}

		// Token: 0x04002BD2 RID: 11218
		private string name = "UNKNOWN";

		// Token: 0x04002BD3 RID: 11219
		private int maxObjOut;

		// Token: 0x04002BD4 RID: 11220
		private int maxObjIn;

		// Token: 0x04002BD5 RID: 11221
		private int maxRpcOut;

		// Token: 0x04002BD6 RID: 11222
		private int maxRpcIn;

		// Token: 0x04002BD7 RID: 11223
		private int totalObjOut;

		// Token: 0x04002BD8 RID: 11224
		private int totalObjIn;

		// Token: 0x04002BD9 RID: 11225
		private int totalRpcOut;

		// Token: 0x04002BDA RID: 11226
		private int totalRpcIn;

		// Token: 0x04002BDB RID: 11227
		private int count;
	}
}
