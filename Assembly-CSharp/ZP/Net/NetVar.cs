using System;
using System.Collections.Generic;
using UnityEngine;

namespace ZP.Net
{
	// Token: 0x02000633 RID: 1587
	public class NetVar<T> : NetVarBase
	{
		// Token: 0x060029A1 RID: 10657 RVA: 0x0001D349 File Offset: 0x0001B549
		public NetVar()
		{
			this.DetermineType();
		}

		// Token: 0x060029A2 RID: 10658 RVA: 0x0001D357 File Offset: 0x0001B557
		public NetVar(T new_val)
		{
			this.val = new_val;
			this.DetermineType();
		}

		// Token: 0x1700051F RID: 1311
		// (get) Token: 0x060029A3 RID: 10659 RVA: 0x0001D36C File Offset: 0x0001B56C
		// (set) Token: 0x060029A4 RID: 10660 RVA: 0x0001D374 File Offset: 0x0001B574
		public T Value
		{
			get
			{
				return this.val;
			}
			set
			{
				if (!this.comparer.Equals(value, this.val))
				{
					this.changed = true;
					this.val = value;
				}
			}
		}

		// Token: 0x17000520 RID: 1312
		// (get) Token: 0x060029A5 RID: 10661 RVA: 0x0001D398 File Offset: 0x0001B598
		public override object Object
		{
			get
			{
				return this.val;
			}
		}

		// Token: 0x060029A6 RID: 10662 RVA: 0x0001D374 File Offset: 0x0001B574
		public void Set(T _val)
		{
			if (!this.comparer.Equals(_val, this.val))
			{
				this.changed = true;
				this.val = _val;
			}
		}

		// Token: 0x060029A7 RID: 10663 RVA: 0x0001D36C File Offset: 0x0001B56C
		public T Get()
		{
			return this.val;
		}

		// Token: 0x060029A8 RID: 10664 RVA: 0x000F63D0 File Offset: 0x000F45D0
		public override void SnapshotSet(object _val)
		{
			if (!this.comparer.Equals((T)((object)_val), this.val) || (this.flags & NetSendFlags.ALWAYS_SEND) == NetSendFlags.ALWAYS_SEND)
			{
				if (NetSystem.IsServer)
				{
					this.changed = true;
				}
				if (this.recieve_method != null)
				{
					this.val = (T)((object)_val);
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
				this.val = (T)((object)_val);
			}
		}

		// Token: 0x060029A9 RID: 10665 RVA: 0x000F645C File Offset: 0x000F465C
		public override void AlwaysSendSet(object _val)
		{
			if (this.recieve_method != null)
			{
				this.val = (T)((object)_val);
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
			this.val = (T)((object)_val);
		}

		// Token: 0x060029AA RID: 10666 RVA: 0x000F64B4 File Offset: 0x000F46B4
		private void DetermineType()
		{
			this.comparer = EqualityComparer<T>.Default;
			TypeCode typeCode = Type.GetTypeCode(typeof(T));
			switch (typeCode)
			{
			case TypeCode.Boolean:
				this.netvar_type = NetVarType.BOOL;
				return;
			case TypeCode.Char:
				this.netvar_type = NetVarType.CHAR;
				return;
			case TypeCode.Byte:
				this.netvar_type = NetVarType.BYTE;
				return;
			case TypeCode.Int16:
				this.netvar_type = NetVarType.SHORT;
				return;
			case TypeCode.UInt16:
				this.netvar_type = NetVarType.USHORT;
				return;
			case TypeCode.Int32:
				this.netvar_type = NetVarType.INT;
				return;
			case TypeCode.UInt32:
				this.netvar_type = NetVarType.UINT;
				return;
			case TypeCode.Int64:
				this.netvar_type = NetVarType.LONG;
				return;
			case TypeCode.UInt64:
				this.netvar_type = NetVarType.ULONG;
				return;
			case TypeCode.Single:
				this.netvar_type = NetVarType.FLOAT;
				return;
			case TypeCode.Double:
				this.netvar_type = NetVarType.DOUBLE;
				return;
			}
			Debug.LogError("Unhandled type '" + typeCode.ToString() + "' used for NetVar!");
		}

		// Token: 0x04002C18 RID: 11288
		private T val;

		// Token: 0x04002C19 RID: 11289
		private EqualityComparer<T> comparer;
	}
}
