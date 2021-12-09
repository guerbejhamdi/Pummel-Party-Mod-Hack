using System;
using UnityEngine;

namespace ZP.Net
{
	// Token: 0x02000637 RID: 1591
	public class NetVec2 : NetVarBase
	{
		// Token: 0x060029CA RID: 10698 RVA: 0x0001D482 File Offset: 0x0001B682
		public NetVec2()
		{
			this.val = Vector2.zero;
			this.netvar_type = NetVarType.VEC2;
		}

		// Token: 0x060029CB RID: 10699 RVA: 0x0001D49D File Offset: 0x0001B69D
		public NetVec2(Vector2 new_val)
		{
			this.val = new_val;
			this.netvar_type = NetVarType.VEC2;
		}

		// Token: 0x17000530 RID: 1328
		// (get) Token: 0x060029CC RID: 10700 RVA: 0x0001D4B4 File Offset: 0x0001B6B4
		// (set) Token: 0x060029CD RID: 10701 RVA: 0x0001D4BC File Offset: 0x0001B6BC
		public Vector2 Value
		{
			get
			{
				return this.val;
			}
			set
			{
				if (this.val.x != value.x || this.val.y != value.y)
				{
					this.val = value;
					this.changed = true;
				}
			}
		}

		// Token: 0x17000531 RID: 1329
		// (get) Token: 0x060029CE RID: 10702 RVA: 0x0001D4F2 File Offset: 0x0001B6F2
		// (set) Token: 0x060029CF RID: 10703 RVA: 0x0001D4FF File Offset: 0x0001B6FF
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

		// Token: 0x17000532 RID: 1330
		// (get) Token: 0x060029D0 RID: 10704 RVA: 0x0001D514 File Offset: 0x0001B714
		// (set) Token: 0x060029D1 RID: 10705 RVA: 0x0001D521 File Offset: 0x0001B721
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

		// Token: 0x17000533 RID: 1331
		// (get) Token: 0x060029D2 RID: 10706 RVA: 0x0001D536 File Offset: 0x0001B736
		public override object Object
		{
			get
			{
				return this.val;
			}
		}

		// Token: 0x060029D3 RID: 10707 RVA: 0x000F6678 File Offset: 0x000F4878
		public override void SnapshotSet(object _val)
		{
			if (NetSystem.IsServer)
			{
				this.changed = true;
			}
			if (this.recieve_method != null)
			{
				this.val = (Vector2)_val;
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
			this.val = (Vector2)_val;
		}

		// Token: 0x060029D4 RID: 10708 RVA: 0x000F66E0 File Offset: 0x000F48E0
		public override void AlwaysSendSet(object _val)
		{
			if (this.recieve_method != null)
			{
				this.val = (Vector2)_val;
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
			this.val = (Vector2)_val;
		}

		// Token: 0x060029D5 RID: 10709 RVA: 0x0001D4BC File Offset: 0x0001B6BC
		public void Set(Vector2 _val)
		{
			if (this.val.x != _val.x || this.val.y != _val.y)
			{
				this.val = _val;
				this.changed = true;
			}
		}

		// Token: 0x060029D6 RID: 10710 RVA: 0x0001D4B4 File Offset: 0x0001B6B4
		public Vector2 Get()
		{
			return this.val;
		}

		// Token: 0x04002C44 RID: 11332
		private Vector2 val;
	}
}
