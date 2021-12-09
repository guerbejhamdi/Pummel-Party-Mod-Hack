using System;

namespace ZP.Net
{
	// Token: 0x020005FC RID: 1532
	public interface INetVar
	{
		// Token: 0x170004B7 RID: 1207
		// (get) Token: 0x060027B6 RID: 10166
		// (set) Token: 0x060027B7 RID: 10167
		NetSendFlags Flags { get; set; }

		// Token: 0x170004B8 RID: 1208
		// (get) Token: 0x060027B8 RID: 10168
		// (set) Token: 0x060027B9 RID: 10169
		int Bits { get; set; }

		// Token: 0x170004B9 RID: 1209
		// (get) Token: 0x060027BA RID: 10170
		object Object { get; }

		// Token: 0x170004BA RID: 1210
		// (get) Token: 0x060027BB RID: 10171
		int Length { get; }

		// Token: 0x170004BB RID: 1211
		// (get) Token: 0x060027BC RID: 10172
		bool SizeChanged { get; }

		// Token: 0x170004BC RID: 1212
		// (get) Token: 0x060027BD RID: 10173
		bool IsArray { get; }

		// Token: 0x170004BD RID: 1213
		// (get) Token: 0x060027BE RID: 10174
		bool HasChanged { get; }

		// Token: 0x170004BE RID: 1214
		// (get) Token: 0x060027BF RID: 10175
		NetVarType VarType { get; }

		// Token: 0x170004BF RID: 1215
		// (get) Token: 0x060027C0 RID: 10176
		// (set) Token: 0x060027C1 RID: 10177
		int LastChangeTick { get; set; }

		// Token: 0x170004C0 RID: 1216
		// (get) Token: 0x060027C2 RID: 10178
		// (set) Token: 0x060027C3 RID: 10179
		int LastSizeChangeTick { get; set; }

		// Token: 0x170004C1 RID: 1217
		// (get) Token: 0x060027C4 RID: 10180
		// (set) Token: 0x060027C5 RID: 10181
		bool DidRecieve { get; set; }

		// Token: 0x060027C6 RID: 10182
		void ResetDelta();

		// Token: 0x060027C7 RID: 10183
		void SnapshotSet(object _val);

		// Token: 0x060027C8 RID: 10184
		void SnapshotResize(int _size);

		// Token: 0x060027C9 RID: 10185
		void AlwaysSendSet(object _val);

		// Token: 0x060027CA RID: 10186
		void Resize(int size);
	}
}
