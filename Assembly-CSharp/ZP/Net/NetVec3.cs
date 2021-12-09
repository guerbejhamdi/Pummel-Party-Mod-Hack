using System;
using UnityEngine;

namespace ZP.Net
{
	// Token: 0x02000638 RID: 1592
	public class NetVec3 : NetVarBase
	{
		// Token: 0x060029D7 RID: 10711 RVA: 0x0001D543 File Offset: 0x0001B743
		public NetVec3()
		{
			this.val = Vector3.zero;
			this.netvar_type = NetVarType.VEC3;
		}

		// Token: 0x060029D8 RID: 10712 RVA: 0x0001D55E File Offset: 0x0001B75E
		public NetVec3(Vector3 new_val)
		{
			this.val = new_val;
			this.netvar_type = NetVarType.VEC3;
		}

		// Token: 0x17000534 RID: 1332
		// (get) Token: 0x060029D9 RID: 10713 RVA: 0x0001D575 File Offset: 0x0001B775
		// (set) Token: 0x060029DA RID: 10714 RVA: 0x000F6738 File Offset: 0x000F4938
		public Vector3 Value
		{
			get
			{
				return this.val;
			}
			set
			{
				if (this.val.x != value.x || this.val.y != value.y || this.val.z != value.z)
				{
					this.val = value;
					this.changed = true;
				}
			}
		}

		// Token: 0x17000535 RID: 1333
		// (get) Token: 0x060029DB RID: 10715 RVA: 0x0001D57D File Offset: 0x0001B77D
		// (set) Token: 0x060029DC RID: 10716 RVA: 0x0001D58A File Offset: 0x0001B78A
		public float x
		{
			get
			{
				return this.val.x;
			}
			set
			{
				this.val.x = value;
				this.changed = true;
			}
		}

		// Token: 0x17000536 RID: 1334
		// (get) Token: 0x060029DD RID: 10717 RVA: 0x0001D59F File Offset: 0x0001B79F
		// (set) Token: 0x060029DE RID: 10718 RVA: 0x0001D5AC File Offset: 0x0001B7AC
		public float y
		{
			get
			{
				return this.val.y;
			}
			set
			{
				this.val.y = value;
				this.changed = true;
			}
		}

		// Token: 0x17000537 RID: 1335
		// (get) Token: 0x060029DF RID: 10719 RVA: 0x0001D5C1 File Offset: 0x0001B7C1
		// (set) Token: 0x060029E0 RID: 10720 RVA: 0x0001D5CE File Offset: 0x0001B7CE
		public float z
		{
			get
			{
				return this.val.z;
			}
			set
			{
				this.val.z = value;
				this.changed = true;
			}
		}

		// Token: 0x17000538 RID: 1336
		// (get) Token: 0x060029E1 RID: 10721 RVA: 0x0001D5E3 File Offset: 0x0001B7E3
		public override object Object
		{
			get
			{
				return this.val;
			}
		}

		// Token: 0x060029E2 RID: 10722 RVA: 0x000F678C File Offset: 0x000F498C
		public override void SnapshotSet(object _val)
		{
			if (NetSystem.IsServer)
			{
				this.changed = true;
			}
			if (this.recieve_method != null && (this.val != (Vector3)_val || (this.flags & NetSendFlags.ALWAYS_SEND) == NetSendFlags.ALWAYS_SEND))
			{
				this.val = (Vector3)_val;
				try
				{
					this.recieve_method(_val);
					return;
				}
				catch (Exception ex)
				{
					Debug.LogError(ex.ToString());
					return;
				}
			}
			this.val = (Vector3)_val;
		}

		// Token: 0x060029E3 RID: 10723 RVA: 0x000F6810 File Offset: 0x000F4A10
		public override void AlwaysSendSet(object _val)
		{
			if (this.recieve_method != null)
			{
				this.val = (Vector3)_val;
				try
				{
					this.recieve_method(_val);
					return;
				}
				catch (Exception ex)
				{
					Debug.LogError(ex.ToString());
					return;
				}
			}
			this.val = (Vector3)_val;
		}

		// Token: 0x060029E4 RID: 10724 RVA: 0x0001D5F0 File Offset: 0x0001B7F0
		public void Set(Vector3 _val)
		{
			if (this.val.x != _val.x || this.val.y != _val.y)
			{
				this.val = _val;
				this.changed = true;
			}
		}

		// Token: 0x060029E5 RID: 10725 RVA: 0x0001D575 File Offset: 0x0001B775
		public Vector3 Get()
		{
			return this.val;
		}

		// Token: 0x04002C45 RID: 11333
		private Vector3 val;
	}
}
