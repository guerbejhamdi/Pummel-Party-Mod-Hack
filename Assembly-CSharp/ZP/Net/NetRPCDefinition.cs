using System;
using System.Collections.Generic;
using System.Reflection;
using Lidgren.Network;
using UnityEngine;

namespace ZP.Net
{
	// Token: 0x02000618 RID: 1560
	public class NetRPCDefinition
	{
		// Token: 0x060028D5 RID: 10453 RVA: 0x000F492C File Offset: 0x000F2B2C
		public NetRPCDefinition()
		{
			this.method_name = "NO_METHOD";
			this.rpc_id = 0;
			this.method_info = null;
			this.parameter_types = null;
			this.param_is_array = null;
			this.has_array_param = false;
			this.min_msg_size = 0;
			this.relay = true;
			this.send = NetRPCSecurity.ALL;
			this.recieve = NetRPCSecurity.ALL;
		}

		// Token: 0x060028D6 RID: 10454 RVA: 0x000F498C File Offset: 0x000F2B8C
		public NetRPCDefinition(string _method_name, byte _rpc_id, MethodInfo _method_info, NetRPCType[] _parameter_types, bool[] _param_is_array, bool _has_array_param, int _min_msg_size, bool _relay, NetRPCSecurity _send, NetRPCSecurity _recieve)
		{
			this.method_name = _method_name;
			this.rpc_id = _rpc_id;
			this.method_info = _method_info;
			this.parameter_types = _parameter_types;
			this.param_is_array = _param_is_array;
			this.has_array_param = _has_array_param;
			this.min_msg_size = _min_msg_size;
			this.relay = _relay;
			this.send = _send;
			this.recieve = _recieve;
		}

		// Token: 0x17000502 RID: 1282
		// (get) Token: 0x060028D7 RID: 10455 RVA: 0x0001CA59 File Offset: 0x0001AC59
		public string MethodName
		{
			get
			{
				return this.method_name;
			}
		}

		// Token: 0x17000503 RID: 1283
		// (get) Token: 0x060028D8 RID: 10456 RVA: 0x0001CA61 File Offset: 0x0001AC61
		public byte RPCid
		{
			get
			{
				return this.rpc_id;
			}
		}

		// Token: 0x17000504 RID: 1284
		// (get) Token: 0x060028D9 RID: 10457 RVA: 0x0001CA69 File Offset: 0x0001AC69
		public MethodInfo Method
		{
			get
			{
				return this.method_info;
			}
		}

		// Token: 0x17000505 RID: 1285
		// (get) Token: 0x060028DA RID: 10458 RVA: 0x0001CA71 File Offset: 0x0001AC71
		public bool HasArrayParam
		{
			get
			{
				return this.has_array_param;
			}
		}

		// Token: 0x17000506 RID: 1286
		// (get) Token: 0x060028DB RID: 10459 RVA: 0x0001CA79 File Offset: 0x0001AC79
		public int MinMsgSize
		{
			get
			{
				return this.min_msg_size;
			}
		}

		// Token: 0x17000507 RID: 1287
		// (get) Token: 0x060028DC RID: 10460 RVA: 0x0001CA81 File Offset: 0x0001AC81
		public bool Relay
		{
			get
			{
				return this.relay;
			}
		}

		// Token: 0x17000508 RID: 1288
		// (get) Token: 0x060028DD RID: 10461 RVA: 0x0001CA89 File Offset: 0x0001AC89
		public NetRPCSecurity Send
		{
			get
			{
				return this.send;
			}
		}

		// Token: 0x17000509 RID: 1289
		// (get) Token: 0x060028DE RID: 10462 RVA: 0x0001CA91 File Offset: 0x0001AC91
		public NetRPCSecurity Recieve
		{
			get
			{
				return this.recieve;
			}
		}

		// Token: 0x060028DF RID: 10463 RVA: 0x0001CA99 File Offset: 0x0001AC99
		public NetRPCType GetParameterType(int index)
		{
			return this.parameter_types[index];
		}

		// Token: 0x060028E0 RID: 10464 RVA: 0x0001CAA3 File Offset: 0x0001ACA3
		public bool IsParamaterArray(int index)
		{
			return this.param_is_array[index];
		}

		// Token: 0x060028E1 RID: 10465 RVA: 0x0001CAAD File Offset: 0x0001ACAD
		public int GetParameterCount()
		{
			if (this.parameter_types == null)
			{
				return 0;
			}
			return this.parameter_types.Length;
		}

		// Token: 0x060028E2 RID: 10466 RVA: 0x000F49EC File Offset: 0x000F2BEC
		public bool CanSendRPC(NetPlayer sender, INetComponent net_entity)
		{
			return NetSystem.IsServer || this.send == NetRPCSecurity.ALL || ((this.send & NetRPCSecurity.SERVER) == NetRPCSecurity.SERVER && sender.UserID == 0) || ((this.send & NetRPCSecurity.OWNER) == NetRPCSecurity.OWNER && sender == net_entity.Owner) || (this.send & NetRPCSecurity.PROXY) == NetRPCSecurity.PROXY;
		}

		// Token: 0x060028E3 RID: 10467 RVA: 0x000F4A48 File Offset: 0x000F2C48
		public bool CanRecieveRPC(NetPlayer sender, INetComponent entity)
		{
			if (!NetSystem.IsServer)
			{
				return true;
			}
			if (this.send == NetRPCSecurity.ALL)
			{
				return true;
			}
			if ((this.send & NetRPCSecurity.SERVER) == NetRPCSecurity.SERVER && sender.UserID == 0)
			{
				return true;
			}
			if ((this.send & NetRPCSecurity.OWNER) == NetRPCSecurity.OWNER && sender.UserID == entity.Owner.UserID)
			{
				return true;
			}
			Debug.Log("Security check for OWNER failed, sender.UserID = " + sender.UserID.ToString() + ", Owner.UserID = " + entity.Owner.UserID.ToString());
			return (this.send & NetRPCSecurity.PROXY) == NetRPCSecurity.PROXY;
		}

		// Token: 0x060028E4 RID: 10468 RVA: 0x000F4AE4 File Offset: 0x000F2CE4
		public void GetRPCRecievers(INetComponent net_entity, ref List<NetConnection> connections)
		{
			if (NetSystem.IsServer)
			{
				if ((this.recieve & NetRPCSecurity.ALL) == NetRPCSecurity.ALL)
				{
					return;
				}
				if ((this.recieve & NetRPCSecurity.PROXY) != NetRPCSecurity.PROXY)
				{
					connections.Clear();
					if ((this.recieve & NetRPCSecurity.OWNER) == NetRPCSecurity.OWNER)
					{
						connections.Add(net_entity.Owner.Connection);
					}
					return;
				}
				if ((this.recieve & NetRPCSecurity.OWNER) != NetRPCSecurity.OWNER)
				{
					connections.Remove(net_entity.Owner.Connection);
					return;
				}
			}
			else
			{
				Debug.LogError("GetRPCRecievers should not be called on the client!, clients only send RPC's to the server.");
			}
		}

		// Token: 0x060028E5 RID: 10469 RVA: 0x000F4B60 File Offset: 0x000F2D60
		public int GetMsgSize(object[] parameters)
		{
			if (!this.has_array_param)
			{
				return this.min_msg_size;
			}
			int num = 4;
			for (int i = 0; i < this.parameter_types.Length; i++)
			{
				switch (this.parameter_types[i])
				{
				case NetRPCType.ARRAY_BOOL:
					num += 2 + ((bool[])parameters[i]).Length;
					break;
				case NetRPCType.ARRAY_BYTE:
					num += 2 + ((byte[])parameters[i]).Length;
					break;
				case NetRPCType.ARRAY_CHAR:
					num += 2 + ((char[])parameters[i]).Length * 2;
					break;
				case NetRPCType.ARRAY_SHORT:
					num += 2 + ((short[])parameters[i]).Length * 2;
					break;
				case NetRPCType.ARRAY_USHORT:
					num += 2 + ((ushort[])parameters[i]).Length * 2;
					break;
				case NetRPCType.ARRAY_INT:
					num += 2 + ((int[])parameters[i]).Length * 4;
					break;
				case NetRPCType.ARRAY_UINT:
					num += 2 + ((uint[])parameters[i]).Length * 4;
					break;
				case NetRPCType.ARRAY_LONG:
					num += 2 + ((long[])parameters[i]).Length * 8;
					break;
				case NetRPCType.ARRAY_ULONG:
					num += 2 + ((ulong[])parameters[i]).Length * 8;
					break;
				case NetRPCType.ARRAY_FLOAT:
					num += 2 + ((float[])parameters[i]).Length * 4;
					break;
				case NetRPCType.ARRAY_DOUBLE:
					num += 2 + ((double[])parameters[i]).Length * 8;
					break;
				case NetRPCType.ARRAY_STRING:
				{
					num += 2;
					string[] array = (string[])parameters[i];
					for (int j = 0; j < array.Length; j++)
					{
						num += 2;
						num += array[j].Length * 2;
					}
					break;
				}
				case NetRPCType.ARRAY_VECTOR2:
					num += 2 + ((Vector2[])parameters[i]).Length * 8;
					break;
				case NetRPCType.ARRAY_VECTOR3:
					num += 2 + ((Vector3[])parameters[i]).Length * 12;
					break;
				default:
					num += this.parameter_types[i].GetByteSize();
					break;
				}
			}
			return num;
		}

		// Token: 0x04002B89 RID: 11145
		private string method_name;

		// Token: 0x04002B8A RID: 11146
		private byte rpc_id;

		// Token: 0x04002B8B RID: 11147
		private MethodInfo method_info;

		// Token: 0x04002B8C RID: 11148
		private bool has_array_param;

		// Token: 0x04002B8D RID: 11149
		private int min_msg_size;

		// Token: 0x04002B8E RID: 11150
		private NetRPCType[] parameter_types;

		// Token: 0x04002B8F RID: 11151
		private bool[] param_is_array;

		// Token: 0x04002B90 RID: 11152
		private bool relay;

		// Token: 0x04002B91 RID: 11153
		private NetRPCSecurity send;

		// Token: 0x04002B92 RID: 11154
		private NetRPCSecurity recieve;
	}
}
