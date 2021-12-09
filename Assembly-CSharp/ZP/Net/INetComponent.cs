using System;

namespace ZP.Net
{
	// Token: 0x020005F7 RID: 1527
	public interface INetComponent
	{
		// Token: 0x170004AB RID: 1195
		// (get) Token: 0x0600278B RID: 10123
		// (set) Token: 0x0600278C RID: 10124
		ushort NetEntityID { get; set; }

		// Token: 0x170004AC RID: 1196
		// (get) Token: 0x0600278D RID: 10125
		// (set) Token: 0x0600278E RID: 10126
		ushort NetTypeID { get; set; }

		// Token: 0x170004AD RID: 1197
		// (get) Token: 0x0600278F RID: 10127
		// (set) Token: 0x06002790 RID: 10128
		ushort GameObjID { get; set; }

		// Token: 0x170004AE RID: 1198
		// (get) Token: 0x06002791 RID: 10129
		// (set) Token: 0x06002792 RID: 10130
		ushort PrefabID { get; set; }

		// Token: 0x170004AF RID: 1199
		// (get) Token: 0x06002793 RID: 10131
		bool IsGameObject { get; }

		// Token: 0x170004B0 RID: 1200
		// (get) Token: 0x06002794 RID: 10132
		// (set) Token: 0x06002795 RID: 10133
		ushort OwnerSlot { get; set; }

		// Token: 0x170004B1 RID: 1201
		// (get) Token: 0x06002796 RID: 10134
		// (set) Token: 0x06002797 RID: 10135
		BindedNetEntity BindedEntity { get; set; }

		// Token: 0x170004B2 RID: 1202
		// (get) Token: 0x06002798 RID: 10136
		NetTransmitState TransmitState { get; }

		// Token: 0x170004B3 RID: 1203
		// (get) Token: 0x06002799 RID: 10137
		// (set) Token: 0x0600279A RID: 10138
		NetFieldState FieldState { get; set; }

		// Token: 0x170004B4 RID: 1204
		// (get) Token: 0x0600279B RID: 10139
		// (set) Token: 0x0600279C RID: 10140
		NetPlayer Owner { get; set; }

		// Token: 0x170004B5 RID: 1205
		// (get) Token: 0x0600279D RID: 10141
		// (set) Token: 0x0600279E RID: 10142
		bool IsOwner { get; set; }

		// Token: 0x170004B6 RID: 1206
		// (get) Token: 0x0600279F RID: 10143
		// (set) Token: 0x060027A0 RID: 10144
		bool IsAlive { get; set; }

		// Token: 0x060027A1 RID: 10145
		void SetTransmitState(NetTransmitState state);

		// Token: 0x060027A2 RID: 10146
		bool ShouldTransmit();

		// Token: 0x060027A3 RID: 10147
		void OnNetInitialize();

		// Token: 0x060027A4 RID: 10148
		void OnNetDestroy();

		// Token: 0x060027A5 RID: 10149
		void OnOwnerChanged();
	}
}
