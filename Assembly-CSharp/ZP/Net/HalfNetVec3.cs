using System;
using UnityEngine;

namespace ZP.Net
{
	// Token: 0x020005F5 RID: 1525
	public class HalfNetVec3 : NetVarBase
	{
		// Token: 0x0600277C RID: 10108 RVA: 0x000ECB04 File Offset: 0x000EAD04
		public HalfNetVec3(float _min, float _max)
		{
			this.netvar_type = NetVarType.HALFVEC3;
			this.f_val = Vector3.zero;
			this.c_val = new HalfVec3(0, 0, 0);
			this.min = new Vector3(_min, _min, _min);
			this.max = new Vector3(_max, _max, _max);
			this.Compress(this.f_val);
		}

		// Token: 0x0600277D RID: 10109 RVA: 0x0001C10E File Offset: 0x0001A30E
		public HalfNetVec3(Vector3 _min, Vector3 _max)
		{
			this.netvar_type = NetVarType.HALFVEC3;
			this.f_val = Vector3.zero;
			this.min = _min;
			this.max = _max;
			this.Compress(this.f_val);
		}

		// Token: 0x0600277E RID: 10110 RVA: 0x0001C143 File Offset: 0x0001A343
		public HalfNetVec3(Vector3 new_val, float _min, float _max)
		{
			this.netvar_type = NetVarType.HALFVEC3;
			this.f_val = new_val;
			this.min = new Vector3(_min, _min, _min);
			this.max = new Vector3(_max, _max, _max);
			this.Compress(this.f_val);
		}

		// Token: 0x0600277F RID: 10111 RVA: 0x0001C182 File Offset: 0x0001A382
		public HalfNetVec3(Vector3 new_val, Vector3 _min, Vector3 _max)
		{
			this.netvar_type = NetVarType.HALFVEC3;
			this.f_val = new_val;
			this.min = _min;
			this.max = _max;
			this.Compress(this.f_val);
		}

		// Token: 0x170004A6 RID: 1190
		// (get) Token: 0x06002780 RID: 10112 RVA: 0x0001C1B3 File Offset: 0x0001A3B3
		public Vector3 Value
		{
			get
			{
				return this.f_val;
			}
		}

		// Token: 0x170004A7 RID: 1191
		// (get) Token: 0x06002781 RID: 10113 RVA: 0x0001C1BB File Offset: 0x0001A3BB
		public float X
		{
			get
			{
				return this.f_val.x;
			}
		}

		// Token: 0x170004A8 RID: 1192
		// (get) Token: 0x06002782 RID: 10114 RVA: 0x0001C1C8 File Offset: 0x0001A3C8
		public float Y
		{
			get
			{
				return this.f_val.y;
			}
		}

		// Token: 0x170004A9 RID: 1193
		// (get) Token: 0x06002783 RID: 10115 RVA: 0x0001C1D5 File Offset: 0x0001A3D5
		public float Z
		{
			get
			{
				return this.f_val.z;
			}
		}

		// Token: 0x170004AA RID: 1194
		// (get) Token: 0x06002784 RID: 10116 RVA: 0x0001C1E2 File Offset: 0x0001A3E2
		public override object Object
		{
			get
			{
				return this.c_val;
			}
		}

		// Token: 0x06002785 RID: 10117 RVA: 0x000ECB60 File Offset: 0x000EAD60
		public void SnapshotSet(HalfVec3 hvec3)
		{
			if (NetSystem.IsServer)
			{
				this.changed = true;
			}
			this.c_val.x = hvec3.x;
			this.c_val.y = hvec3.y;
			this.c_val.z = hvec3.z;
			if (this.recieve_method != null)
			{
				try
				{
					this.recieve_method(this);
				}
				catch (Exception ex)
				{
					Debug.LogError(ex.ToString());
				}
			}
		}

		// Token: 0x06002786 RID: 10118 RVA: 0x000ECBE0 File Offset: 0x000EADE0
		public void AlwaysSendSet(HalfVec3 hvec3)
		{
			this.c_val.x = hvec3.x;
			this.c_val.y = hvec3.y;
			this.c_val.z = hvec3.z;
			if (this.recieve_method != null)
			{
				try
				{
					this.recieve_method(this);
				}
				catch (Exception ex)
				{
					Debug.LogError(ex.ToString());
				}
			}
		}

		// Token: 0x06002787 RID: 10119 RVA: 0x000ECC54 File Offset: 0x000EAE54
		public void Compress(Vector3 new_val)
		{
			ushort num = this.CompressFloat(this.f_val.x, this.min.x, this.max.x);
			ushort num2 = this.CompressFloat(this.f_val.y, this.min.y, this.max.y);
			ushort num3 = this.CompressFloat(this.f_val.z, this.min.z, this.max.z);
			if (num != this.c_val.x || num2 != this.c_val.y || num3 != this.c_val.z)
			{
				this.c_val.x = num;
				this.c_val.y = num2;
				this.c_val.z = num3;
				this.changed = true;
			}
		}

		// Token: 0x06002788 RID: 10120 RVA: 0x000ECD30 File Offset: 0x000EAF30
		public void Decompress()
		{
			this.f_val.x = this.DecompressFloat(this.c_val.x, this.min.x, this.max.x);
			this.f_val.y = this.DecompressFloat(this.c_val.y, this.min.y, this.max.y);
			this.f_val.z = this.DecompressFloat(this.c_val.z, this.min.z, this.max.z);
		}

		// Token: 0x06002789 RID: 10121 RVA: 0x000ECDD4 File Offset: 0x000EAFD4
		private ushort CompressFloat(float x, float min, float max)
		{
			float num = Mathf.Abs(min - max);
			float num2 = Mathf.Abs(min - Mathf.Clamp(x, min, max));
			return (ushort)(65535f * (num2 / num));
		}

		// Token: 0x0600278A RID: 10122 RVA: 0x000ECE04 File Offset: 0x000EB004
		private float DecompressFloat(ushort x, float min, float max)
		{
			float num = Mathf.Abs(min - max);
			return (float)x / 65535f * num - Mathf.Abs(min);
		}

		// Token: 0x04002AA6 RID: 10918
		private HalfVec3 c_val;

		// Token: 0x04002AA7 RID: 10919
		private Vector3 f_val;

		// Token: 0x04002AA8 RID: 10920
		private Vector3 min;

		// Token: 0x04002AA9 RID: 10921
		private Vector3 max;
	}
}
