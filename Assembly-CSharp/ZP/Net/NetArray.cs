using System;
using System.Collections.Generic;
using UnityEngine;

namespace ZP.Net
{
	// Token: 0x020005FD RID: 1533
	public class NetArray<T> : NetVarBase
	{
		// Token: 0x060027CB RID: 10187 RVA: 0x0001C1EA File Offset: 0x0001A3EA
		public NetArray(int size)
		{
			this.array = new T[size];
			this.arr_changed = new int[size];
			this.DetermineType();
		}

		// Token: 0x170004C2 RID: 1218
		public T this[int index]
		{
			get
			{
				if (index < this.array.Length && index >= 0)
				{
					return this.array[index];
				}
				throw new IndexOutOfRangeException();
			}
			set
			{
				this.Set(index, value);
			}
		}

		// Token: 0x170004C3 RID: 1219
		// (get) Token: 0x060027CE RID: 10190 RVA: 0x0001C23D File Offset: 0x0001A43D
		public override object Object
		{
			get
			{
				return this.array;
			}
		}

		// Token: 0x170004C4 RID: 1220
		// (get) Token: 0x060027CF RID: 10191 RVA: 0x0001C245 File Offset: 0x0001A445
		public override int Length
		{
			get
			{
				return this.array.Length;
			}
		}

		// Token: 0x170004C5 RID: 1221
		// (get) Token: 0x060027D0 RID: 10192 RVA: 0x0001C24F File Offset: 0x0001A44F
		public override bool SizeChanged
		{
			get
			{
				return this.size_changed;
			}
		}

		// Token: 0x170004C6 RID: 1222
		// (get) Token: 0x060027D1 RID: 10193 RVA: 0x00005651 File Offset: 0x00003851
		public override bool IsArray
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060027D2 RID: 10194 RVA: 0x000ECE2C File Offset: 0x000EB02C
		public void Set(int index, T _val)
		{
			if (index >= this.array.Length || index < 0)
			{
				throw new IndexOutOfRangeException();
			}
			if (this.array[index] == null || !this.comparer.Equals(_val, this.array[index]))
			{
				this.array[index] = _val;
				this.arr_changed[index] = NetSystem.CurrentTick;
				this.changed = true;
				return;
			}
		}

		// Token: 0x060027D3 RID: 10195 RVA: 0x0001C257 File Offset: 0x0001A457
		public T Get(int index)
		{
			return this.array[index];
		}

		// Token: 0x060027D4 RID: 10196 RVA: 0x0001C23D File Offset: 0x0001A43D
		public T[] GetData()
		{
			return this.array;
		}

		// Token: 0x060027D5 RID: 10197 RVA: 0x000ECEA0 File Offset: 0x000EB0A0
		public override void Resize(int size)
		{
			this.size_changed = true;
			this.changed = true;
			this.arr_changed = new int[size];
			for (int i = this.array.Length; i < size; i++)
			{
				this.arr_changed[i] = NetSystem.CurrentTick;
			}
			T[] array = new T[size];
			int num = Mathf.Min(size, this.array.Length);
			for (int j = 0; j < num; j++)
			{
				array[j] = this.array[j];
			}
			this.array = array;
		}

		// Token: 0x060027D6 RID: 10198 RVA: 0x0001C265 File Offset: 0x0001A465
		public override void ResetDelta()
		{
			this.changed = false;
			this.size_changed = false;
		}

		// Token: 0x060027D7 RID: 10199 RVA: 0x0001C275 File Offset: 0x0001A475
		public override void SnapshotResize(int _size)
		{
			if (this.array_resize_method != null)
			{
				this.array_resize_method(_size);
			}
		}

		// Token: 0x060027D8 RID: 10200 RVA: 0x000ECF24 File Offset: 0x000EB124
		public void SnapshotSetIndex(int index, T val)
		{
			if (this.comparer.Equals(this.array[index], val))
			{
				return;
			}
			this.array[index] = val;
			if (NetSystem.IsServer)
			{
				this.arr_changed[index] = NetSystem.CurrentTick;
				this.changed = true;
			}
			if (this.array_recieve_method != null)
			{
				this.array_recieve_method(index, val);
			}
		}

		// Token: 0x060027D9 RID: 10201 RVA: 0x0001C28B File Offset: 0x0001A48B
		public int SlotChanged(int index)
		{
			return this.arr_changed[index];
		}

		// Token: 0x060027DA RID: 10202 RVA: 0x000ECF90 File Offset: 0x000EB190
		private void DetermineType()
		{
			this.comparer = EqualityComparer<T>.Default;
			TypeCode typeCode = Type.GetTypeCode(typeof(T));
			for (int i = 0; i < this.array.Length; i++)
			{
				this.array[i] = default(T);
			}
			switch (typeCode)
			{
			case TypeCode.Boolean:
				this.netvar_type = NetVarType.ARRAY_BOOL;
				return;
			case TypeCode.Char:
				this.netvar_type = NetVarType.ARRAY_CHAR;
				return;
			case TypeCode.Byte:
				this.netvar_type = NetVarType.ARRAY_BYTE;
				return;
			case TypeCode.Int16:
				this.netvar_type = NetVarType.ARRAY_SHORT;
				return;
			case TypeCode.UInt16:
				this.netvar_type = NetVarType.ARRAY_USHORT;
				return;
			case TypeCode.Int32:
				this.netvar_type = NetVarType.ARRAY_INT;
				return;
			case TypeCode.UInt32:
				this.netvar_type = NetVarType.ARRAY_UINT;
				return;
			case TypeCode.Int64:
				this.netvar_type = NetVarType.ARRAY_LONG;
				return;
			case TypeCode.UInt64:
				this.netvar_type = NetVarType.ARRAY_ULONG;
				return;
			case TypeCode.Single:
				this.netvar_type = NetVarType.ARRAY_FLOAT;
				return;
			case TypeCode.Double:
				this.netvar_type = NetVarType.ARRAY_DOUBLE;
				return;
			case TypeCode.String:
				this.netvar_type = NetVarType.ARRAY_STRING;
				return;
			}
			Debug.LogError("Unhandled type used for NetArray!");
		}

		// Token: 0x04002AAF RID: 10927
		private T[] array;

		// Token: 0x04002AB0 RID: 10928
		private EqualityComparer<T> comparer;

		// Token: 0x04002AB1 RID: 10929
		private bool size_changed;

		// Token: 0x04002AB2 RID: 10930
		private int[] arr_changed;
	}
}
