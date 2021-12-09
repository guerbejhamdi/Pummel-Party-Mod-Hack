using System;
using Lidgren.Network;

namespace ZP.Net
{
	// Token: 0x020005FF RID: 1535
	public class NetClass : INetComponent
	{
		// Token: 0x060027FB RID: 10235 RVA: 0x000ED0FC File Offset: 0x000EB2FC
		public NetClass()
		{
			this.entity_id = 0;
			this.net_type_id = 0;
			this.gameobj_id = 0;
			this.prefab_id = 0;
			this.is_owner = false;
			this.is_alive = true;
			this.owner_slot = 0;
			this.binded_entity = null;
			this.owner = null;
			this.transmit_state = NetTransmitState.ALWAYS_ALL;
			this.field_state = NetFieldState.NONE;
		}

		// Token: 0x170004D3 RID: 1235
		// (get) Token: 0x060027FC RID: 10236 RVA: 0x0001C373 File Offset: 0x0001A573
		// (set) Token: 0x060027FD RID: 10237 RVA: 0x0001C37B File Offset: 0x0001A57B
		public ushort NetEntityID
		{
			get
			{
				return this.entity_id;
			}
			set
			{
				this.entity_id = value;
			}
		}

		// Token: 0x170004D4 RID: 1236
		// (get) Token: 0x060027FE RID: 10238 RVA: 0x0001C384 File Offset: 0x0001A584
		// (set) Token: 0x060027FF RID: 10239 RVA: 0x0001C38C File Offset: 0x0001A58C
		public ushort NetTypeID
		{
			get
			{
				return this.net_type_id;
			}
			set
			{
				this.net_type_id = value;
			}
		}

		// Token: 0x170004D5 RID: 1237
		// (get) Token: 0x06002800 RID: 10240 RVA: 0x0001C395 File Offset: 0x0001A595
		// (set) Token: 0x06002801 RID: 10241 RVA: 0x0000398C File Offset: 0x00001B8C
		public ushort GameObjID
		{
			get
			{
				return this.gameobj_id;
			}
			set
			{
			}
		}

		// Token: 0x170004D6 RID: 1238
		// (get) Token: 0x06002802 RID: 10242 RVA: 0x0001C39D File Offset: 0x0001A59D
		// (set) Token: 0x06002803 RID: 10243 RVA: 0x0000398C File Offset: 0x00001B8C
		public ushort PrefabID
		{
			get
			{
				return this.prefab_id;
			}
			set
			{
			}
		}

		// Token: 0x170004D7 RID: 1239
		// (get) Token: 0x06002804 RID: 10244 RVA: 0x0000539F File Offset: 0x0000359F
		// (set) Token: 0x06002805 RID: 10245 RVA: 0x0000398C File Offset: 0x00001B8C
		public bool IsGameObject
		{
			get
			{
				return false;
			}
			private set
			{
			}
		}

		// Token: 0x170004D8 RID: 1240
		// (get) Token: 0x06002806 RID: 10246 RVA: 0x0001C3A5 File Offset: 0x0001A5A5
		// (set) Token: 0x06002807 RID: 10247 RVA: 0x0001C3AD File Offset: 0x0001A5AD
		public BindedNetEntity BindedEntity
		{
			get
			{
				return this.binded_entity;
			}
			set
			{
				this.binded_entity = value;
			}
		}

		// Token: 0x170004D9 RID: 1241
		// (get) Token: 0x06002808 RID: 10248 RVA: 0x0001C3B6 File Offset: 0x0001A5B6
		// (set) Token: 0x06002809 RID: 10249 RVA: 0x0000398C File Offset: 0x00001B8C
		public NetTransmitState TransmitState
		{
			get
			{
				return this.transmit_state;
			}
			private set
			{
			}
		}

		// Token: 0x170004DA RID: 1242
		// (get) Token: 0x0600280A RID: 10250 RVA: 0x0001C3BE File Offset: 0x0001A5BE
		// (set) Token: 0x0600280B RID: 10251 RVA: 0x0001C3C6 File Offset: 0x0001A5C6
		public NetFieldState FieldState
		{
			get
			{
				return this.field_state;
			}
			set
			{
				this.field_state = value;
			}
		}

		// Token: 0x170004DB RID: 1243
		// (get) Token: 0x0600280C RID: 10252 RVA: 0x0001C3CF File Offset: 0x0001A5CF
		// (set) Token: 0x0600280D RID: 10253 RVA: 0x0001C3D7 File Offset: 0x0001A5D7
		public NetPlayer Owner
		{
			get
			{
				return this.owner;
			}
			set
			{
				this.owner = value;
			}
		}

		// Token: 0x170004DC RID: 1244
		// (get) Token: 0x0600280E RID: 10254 RVA: 0x0001C3E0 File Offset: 0x0001A5E0
		// (set) Token: 0x0600280F RID: 10255 RVA: 0x0001C3E8 File Offset: 0x0001A5E8
		public bool IsOwner
		{
			get
			{
				return this.is_owner;
			}
			set
			{
				this.is_owner = value;
			}
		}

		// Token: 0x170004DD RID: 1245
		// (get) Token: 0x06002810 RID: 10256 RVA: 0x0001C3F1 File Offset: 0x0001A5F1
		// (set) Token: 0x06002811 RID: 10257 RVA: 0x0001C3F9 File Offset: 0x0001A5F9
		public bool IsAlive
		{
			get
			{
				return this.is_alive;
			}
			set
			{
				this.is_alive = value;
			}
		}

		// Token: 0x170004DE RID: 1246
		// (get) Token: 0x06002812 RID: 10258 RVA: 0x0001C402 File Offset: 0x0001A602
		// (set) Token: 0x06002813 RID: 10259 RVA: 0x0001C40A File Offset: 0x0001A60A
		public ushort OwnerSlot
		{
			get
			{
				return this.owner_slot;
			}
			set
			{
				this.owner_slot = value;
			}
		}

		// Token: 0x06002814 RID: 10260 RVA: 0x0001C413 File Offset: 0x0001A613
		public void SetTransmitState(NetTransmitState new_state)
		{
			this.transmit_state = new_state;
		}

		// Token: 0x06002815 RID: 10261 RVA: 0x0001C3CF File Offset: 0x0001A5CF
		public NetPlayer GetOwner()
		{
			return this.owner;
		}

		// Token: 0x06002816 RID: 10262 RVA: 0x00005651 File Offset: 0x00003851
		public virtual bool ShouldTransmit()
		{
			return true;
		}

		// Token: 0x06002817 RID: 10263 RVA: 0x0000398C File Offset: 0x00001B8C
		public virtual void OnNetInitialize()
		{
		}

		// Token: 0x06002818 RID: 10264 RVA: 0x0000398C File Offset: 0x00001B8C
		public virtual void OnNetDestroy()
		{
		}

		// Token: 0x06002819 RID: 10265 RVA: 0x0000398C File Offset: 0x00001B8C
		public virtual void OnOwnerChanged()
		{
		}

		// Token: 0x0600281A RID: 10266 RVA: 0x0001C41C File Offset: 0x0001A61C
		public void SendRPC(string method, NetDeliveryMethod delivery, params object[] parameters)
		{
			NetSystem.SendRPC(this, method, delivery, parameters);
		}

		// Token: 0x04002ABE RID: 10942
		private ushort entity_id;

		// Token: 0x04002ABF RID: 10943
		private ushort net_type_id;

		// Token: 0x04002AC0 RID: 10944
		private ushort gameobj_id;

		// Token: 0x04002AC1 RID: 10945
		private ushort prefab_id;

		// Token: 0x04002AC2 RID: 10946
		private bool is_owner;

		// Token: 0x04002AC3 RID: 10947
		private bool is_alive;

		// Token: 0x04002AC4 RID: 10948
		private ushort owner_slot;

		// Token: 0x04002AC5 RID: 10949
		private BindedNetEntity binded_entity;

		// Token: 0x04002AC6 RID: 10950
		private NetPlayer owner;

		// Token: 0x04002AC7 RID: 10951
		private NetTransmitState transmit_state;

		// Token: 0x04002AC8 RID: 10952
		private NetFieldState field_state;
	}
}
