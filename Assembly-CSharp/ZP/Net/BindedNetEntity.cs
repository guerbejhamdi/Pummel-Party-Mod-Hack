using System;
using UnityEngine;

namespace ZP.Net
{
	// Token: 0x02000610 RID: 1552
	public class BindedNetEntity
	{
		// Token: 0x06002886 RID: 10374 RVA: 0x000EFD64 File Offset: 0x000EDF64
		public BindedNetEntity(INetComponent _net_entity, ushort _net_type_id, ushort _game_obj_id, ushort _net_entity_id, bool _is_game_obj, INetVar[] _owner_send, INetVar[] _server_send, INetVar[] _owner_recieve, INetVar[] _server_recieve, INetVar[] _proxy_recieve)
		{
			this.net_component = _net_entity;
			this.net_type_id = _net_type_id;
			this.game_obj_id = _game_obj_id;
			this.net_entity_id = _net_entity_id;
			this.is_game_obj = _is_game_obj;
			this.owner_send = _owner_send;
			this.server_send = _server_send;
			this.owner_recieve = _owner_recieve;
			this.server_recieve = _server_recieve;
			this.proxy_recieve = _proxy_recieve;
			this.should_send = false;
			this.did_recieve = false;
		}

		// Token: 0x04002B3B RID: 11067
		public INetComponent net_component;

		// Token: 0x04002B3C RID: 11068
		public ushort net_type_id;

		// Token: 0x04002B3D RID: 11069
		public ushort game_obj_id;

		// Token: 0x04002B3E RID: 11070
		public ushort net_entity_id;

		// Token: 0x04002B3F RID: 11071
		public bool is_game_obj;

		// Token: 0x04002B40 RID: 11072
		public int last_change_tick;

		// Token: 0x04002B41 RID: 11073
		public int destroyed_tick;

		// Token: 0x04002B42 RID: 11074
		public bool did_recieve;

		// Token: 0x04002B43 RID: 11075
		public GameObject game_obj;

		// Token: 0x04002B44 RID: 11076
		public string game_obj_name = "NONE";

		// Token: 0x04002B45 RID: 11077
		public bool should_send;

		// Token: 0x04002B46 RID: 11078
		public INetVar[] owner_recieve;

		// Token: 0x04002B47 RID: 11079
		public INetVar[] server_recieve;

		// Token: 0x04002B48 RID: 11080
		public INetVar[] proxy_recieve;

		// Token: 0x04002B49 RID: 11081
		public INetVar[] owner_send;

		// Token: 0x04002B4A RID: 11082
		public INetVar[] server_send;
	}
}
