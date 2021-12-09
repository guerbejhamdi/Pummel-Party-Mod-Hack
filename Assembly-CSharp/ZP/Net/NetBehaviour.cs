using System;
using System.Collections.Generic;
using UnityEngine;

namespace ZP.Net
{
	// Token: 0x020005FE RID: 1534
	public class NetBehaviour : MonoBehaviour, INetComponent
	{
		// Token: 0x060027DB RID: 10203 RVA: 0x000ED0A0 File Offset: 0x000EB2A0
		public NetBehaviour()
		{
			this.entity_id = 0;
			this.net_type_id = 0;
			this.gameobj_id = 0;
			this.prefab_id = -1;
			this.is_owner = false;
			this.is_alive = true;
			this.binded_entity = null;
			this.owner = null;
			this.transmit_state = NetTransmitState.ALWAYS_ALL;
			this.owner_state = NetFieldState.SEND_AND_RECIEVE;
		}

		// Token: 0x170004C7 RID: 1223
		// (get) Token: 0x060027DC RID: 10204 RVA: 0x0001C295 File Offset: 0x0001A495
		// (set) Token: 0x060027DD RID: 10205 RVA: 0x0001C29D File Offset: 0x0001A49D
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

		// Token: 0x170004C8 RID: 1224
		// (get) Token: 0x060027DE RID: 10206 RVA: 0x0001C2A6 File Offset: 0x0001A4A6
		// (set) Token: 0x060027DF RID: 10207 RVA: 0x0001C2AE File Offset: 0x0001A4AE
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

		// Token: 0x170004C9 RID: 1225
		// (get) Token: 0x060027E0 RID: 10208 RVA: 0x0001C2B7 File Offset: 0x0001A4B7
		// (set) Token: 0x060027E1 RID: 10209 RVA: 0x0001C2BF File Offset: 0x0001A4BF
		public ushort GameObjID
		{
			get
			{
				return this.gameobj_id;
			}
			set
			{
				this.gameobj_id = value;
			}
		}

		// Token: 0x170004CA RID: 1226
		// (get) Token: 0x060027E2 RID: 10210 RVA: 0x0001C2C8 File Offset: 0x0001A4C8
		// (set) Token: 0x060027E3 RID: 10211 RVA: 0x0001C2D1 File Offset: 0x0001A4D1
		public ushort PrefabID
		{
			get
			{
				return (ushort)this.prefab_id;
			}
			set
			{
				this.prefab_id = (int)value;
			}
		}

		// Token: 0x170004CB RID: 1227
		// (get) Token: 0x060027E4 RID: 10212 RVA: 0x00005651 File Offset: 0x00003851
		// (set) Token: 0x060027E5 RID: 10213 RVA: 0x0000398C File Offset: 0x00001B8C
		public bool IsGameObject
		{
			get
			{
				return true;
			}
			private set
			{
			}
		}

		// Token: 0x170004CC RID: 1228
		// (get) Token: 0x060027E6 RID: 10214 RVA: 0x0001C2DA File Offset: 0x0001A4DA
		// (set) Token: 0x060027E7 RID: 10215 RVA: 0x0001C2E2 File Offset: 0x0001A4E2
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

		// Token: 0x170004CD RID: 1229
		// (get) Token: 0x060027E8 RID: 10216 RVA: 0x0001C2EB File Offset: 0x0001A4EB
		// (set) Token: 0x060027E9 RID: 10217 RVA: 0x0000398C File Offset: 0x00001B8C
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

		// Token: 0x170004CE RID: 1230
		// (get) Token: 0x060027EA RID: 10218 RVA: 0x0001C2F3 File Offset: 0x0001A4F3
		// (set) Token: 0x060027EB RID: 10219 RVA: 0x0001C2FB File Offset: 0x0001A4FB
		public NetFieldState FieldState
		{
			get
			{
				return this.owner_state;
			}
			set
			{
				this.owner_state = value;
			}
		}

		// Token: 0x170004CF RID: 1231
		// (get) Token: 0x060027EC RID: 10220 RVA: 0x0001C304 File Offset: 0x0001A504
		// (set) Token: 0x060027ED RID: 10221 RVA: 0x0001C30C File Offset: 0x0001A50C
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

		// Token: 0x170004D0 RID: 1232
		// (get) Token: 0x060027EE RID: 10222 RVA: 0x0001C315 File Offset: 0x0001A515
		// (set) Token: 0x060027EF RID: 10223 RVA: 0x0001C31D File Offset: 0x0001A51D
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

		// Token: 0x170004D1 RID: 1233
		// (get) Token: 0x060027F0 RID: 10224 RVA: 0x0001C326 File Offset: 0x0001A526
		// (set) Token: 0x060027F1 RID: 10225 RVA: 0x0001C32E File Offset: 0x0001A52E
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

		// Token: 0x170004D2 RID: 1234
		// (get) Token: 0x060027F2 RID: 10226 RVA: 0x0001C337 File Offset: 0x0001A537
		// (set) Token: 0x060027F3 RID: 10227 RVA: 0x0001C33F File Offset: 0x0001A53F
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

		// Token: 0x060027F4 RID: 10228 RVA: 0x0001C348 File Offset: 0x0001A548
		public void SetTransmitState(NetTransmitState new_state)
		{
			this.transmit_state = new_state;
		}

		// Token: 0x060027F5 RID: 10229 RVA: 0x00005651 File Offset: 0x00003851
		public virtual bool ShouldTransmit()
		{
			return true;
		}

		// Token: 0x060027F6 RID: 10230 RVA: 0x0000398C File Offset: 0x00001B8C
		public virtual void OnNetInitialize()
		{
		}

		// Token: 0x060027F7 RID: 10231 RVA: 0x0000398C File Offset: 0x00001B8C
		public virtual void OnNetDestroy()
		{
		}

		// Token: 0x060027F8 RID: 10232 RVA: 0x0000398C File Offset: 0x00001B8C
		public virtual void OnOwnerChanged()
		{
		}

		// Token: 0x060027F9 RID: 10233 RVA: 0x0001C351 File Offset: 0x0001A551
		public void SendRPC(string method, NetRPCDelivery delivery, params object[] parameters)
		{
			NetSystem.SendRPC(this, method, delivery.GetLidgrenDelivery(), parameters);
		}

		// Token: 0x060027FA RID: 10234 RVA: 0x0001C361 File Offset: 0x0001A561
		public void SendRPCPlayers(string method, NetRPCDelivery delivery, List<NetPlayer> players, params object[] parameters)
		{
			NetSystem.SendRPCPlayers(this, method, delivery.GetLidgrenDelivery(), players, parameters);
		}

		// Token: 0x04002AB3 RID: 10931
		public int prefab_id;

		// Token: 0x04002AB4 RID: 10932
		private ushort entity_id;

		// Token: 0x04002AB5 RID: 10933
		private ushort net_type_id;

		// Token: 0x04002AB6 RID: 10934
		private ushort gameobj_id;

		// Token: 0x04002AB7 RID: 10935
		private bool is_owner;

		// Token: 0x04002AB8 RID: 10936
		private bool is_alive;

		// Token: 0x04002AB9 RID: 10937
		private ushort owner_slot;

		// Token: 0x04002ABA RID: 10938
		private BindedNetEntity binded_entity;

		// Token: 0x04002ABB RID: 10939
		private NetPlayer owner;

		// Token: 0x04002ABC RID: 10940
		private NetTransmitState transmit_state;

		// Token: 0x04002ABD RID: 10941
		private NetFieldState owner_state;
	}
}
