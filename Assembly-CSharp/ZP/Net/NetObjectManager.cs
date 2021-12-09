using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Lidgren.Network;
using UnityEngine;
using ZP.Utility;

namespace ZP.Net
{
	// Token: 0x02000614 RID: 1556
	public class NetObjectManager
	{
		// Token: 0x170004EF RID: 1263
		// (get) Token: 0x06002889 RID: 10377 RVA: 0x0001C87F File Offset: 0x0001AA7F
		public int NetEntityCount
		{
			get
			{
				return this.binded_entities.Count;
			}
		}

		// Token: 0x0600288A RID: 10378 RVA: 0x000EFDE0 File Offset: 0x000EDFE0
		public NetObjectManager()
		{
			this.net_type_definitions = new Dictionary<ushort, NetTypeDefinition>();
			this.net_prefab_map = new Dictionary<string, NetPrefabDefinition>();
			this.net_prefab_list = new List<string>();
			this.net_type_id_map = new Dictionary<Type, ushort>();
			this.binded_entities = new List<BindedNetEntity>();
			this.binded_entity_map = new Dictionary<ushort, BindedNetEntity>();
			this.net_gameobj_list = new Dictionary<ushort, NetGameObject>();
			this.tick_new_entities = new List<BindedNetEntity>();
			this.tick_destroyed_entities = new List<BindedNetEntity>();
			this.destroyed_entities = new List<BindedNetEntity>();
			this.queued_rpc_list = new List<QueuedRPC>();
			this.proxy_prefabs = new Dictionary<string, NetPrefab>();
			this.owner_prefabs = new Dictionary<string, NetPrefab>();
			this.host_prefabs = new Dictionary<string, NetPrefab>();
			this.dedicated_prefabs = new Dictionary<string, NetPrefab>();
			this.last_snapshot_tick = -1;
			this.bsw = new ZPBitStream();
			this.bsw.Reserve(8096);
		}

		// Token: 0x0600288B RID: 10379 RVA: 0x000EFEFC File Offset: 0x000EE0FC
		public void Initialize()
		{
			this.binded_entities = new List<BindedNetEntity>();
			this.binded_entity_map = new Dictionary<ushort, BindedNetEntity>();
			this.net_gameobj_list = new Dictionary<ushort, NetGameObject>();
			this.tick_new_entities = new List<BindedNetEntity>();
			this.tick_destroyed_entities = new List<BindedNetEntity>();
			this.destroyed_entities = new List<BindedNetEntity>();
			this.queued_rpc_list = new List<QueuedRPC>();
			this.proxy_prefabs = new Dictionary<string, NetPrefab>();
			this.owner_prefabs = new Dictionary<string, NetPrefab>();
			this.host_prefabs = new Dictionary<string, NetPrefab>();
			this.dedicated_prefabs = new Dictionary<string, NetPrefab>();
			this.bsw = new ZPBitStream();
			this.bsw.Reserve(8096);
		}

		// Token: 0x0600288C RID: 10380 RVA: 0x000EFFA0 File Offset: 0x000EE1A0
		public void Reset()
		{
			this.binded_entities.Clear();
			this.binded_entity_map.Clear();
			this.net_gameobj_list.Clear();
			this.tick_new_entities.Clear();
			this.tick_destroyed_entities.Clear();
			this.destroyed_entities.Clear();
			this.queued_rpc_list.Clear();
			this.proxy_prefabs.Clear();
			this.owner_prefabs.Clear();
			this.host_prefabs.Clear();
			this.dedicated_prefabs.Clear();
			this.bsw.Clear();
		}

		// Token: 0x0600288D RID: 10381 RVA: 0x000F0034 File Offset: 0x000EE234
		public void Destroy()
		{
			for (int i = 0; i < this.binded_entities.Count; i++)
			{
				this.Kill(this.binded_entities[i]);
			}
			for (int j = this.destroyed_entities.Count - 1; j >= 0; j--)
			{
				BindedNetEntity bindedNetEntity = this.destroyed_entities[j];
				this.ReleaseNetworkID(bindedNetEntity.net_entity_id);
				if (bindedNetEntity.is_game_obj)
				{
					this.ReleaseGameObjectID(bindedNetEntity.game_obj_id);
				}
				this.destroyed_entities.RemoveAt(j);
			}
			this.binded_entities.Clear();
			this.binded_entity_map.Clear();
			foreach (KeyValuePair<ushort, NetGameObject> keyValuePair in this.net_gameobj_list)
			{
				if (keyValuePair.Value.game_obj != null)
				{
					UnityEngine.Object.Destroy(keyValuePair.Value.game_obj);
				}
			}
			this.net_gameobj_list.Clear();
		}

		// Token: 0x0600288E RID: 10382 RVA: 0x000F0140 File Offset: 0x000EE340
		public void UpdateTick()
		{
			if (NetSystem.IsServer)
			{
				int i = 0;
				while (i < this.binded_entities.Count)
				{
					if (this.binded_entities[i].game_obj == null)
					{
						this.Kill(this.binded_entities[i]);
					}
					else
					{
						i++;
					}
				}
			}
			for (int j = this.destroyed_entities.Count - 1; j >= 0; j--)
			{
				BindedNetEntity bindedNetEntity = this.destroyed_entities[j];
				int num = NetSystem.IsServer ? 60 : 10;
				if (NetSystem.CurrentTick - this.destroyed_entities[j].destroyed_tick >= 60 * num)
				{
					if (NetSystem.IsServer)
					{
						this.ReleaseNetworkID(bindedNetEntity.net_entity_id);
						if (bindedNetEntity.is_game_obj)
						{
							this.ReleaseGameObjectID(bindedNetEntity.game_obj_id);
						}
					}
					this.destroyed_entities.RemoveAt(j);
				}
			}
			INetVar[] array = null;
			for (int k = 0; k < this.binded_entities.Count; k++)
			{
				BindedNetEntity bindedNetEntity2 = this.binded_entities[k];
				bindedNetEntity2.did_recieve = false;
				if (bindedNetEntity2 != null && !(bindedNetEntity2.game_obj == null) && bindedNetEntity2.game_obj.activeInHierarchy && bindedNetEntity2.net_component.FieldState != NetFieldState.NONE && bindedNetEntity2.net_component.FieldState != NetFieldState.RECIEVE && bindedNetEntity2.net_component.TransmitState == NetTransmitState.ALWAYS_ALL)
				{
					if (NetSystem.IsServer)
					{
						array = bindedNetEntity2.server_send;
					}
					else if (bindedNetEntity2.net_component.IsOwner)
					{
						array = bindedNetEntity2.owner_send;
					}
					bool flag = false;
					for (int l = 0; l < array.Length; l++)
					{
						if (array[l].HasChanged)
						{
							array[l].LastChangeTick = NetSystem.CurrentTick;
							if (array[l].IsArray && array[l].SizeChanged)
							{
								array[l].LastSizeChangeTick = NetSystem.CurrentTick;
							}
							array[l].ResetDelta();
							flag = true;
						}
						else if ((array[l].Flags & NetSendFlags.ALWAYS_SEND) == NetSendFlags.ALWAYS_SEND)
						{
							flag = true;
						}
					}
					if (flag)
					{
						bindedNetEntity2.last_change_tick = NetSystem.CurrentTick;
					}
				}
			}
			int m = 0;
			while (m < this.queued_rpc_list.Count)
			{
				QueuedRPC queuedRPC = this.queued_rpc_list[m];
				queuedRPC.wait_time += Time.deltaTime;
				if (this.InvokeRPC(queuedRPC.sender, queuedRPC.data, queuedRPC.bit_length, true))
				{
					this.queued_rpc_list.RemoveAt(m);
				}
				else if ((double)queuedRPC.wait_time > 8.0)
				{
					Debug.LogError(string.Concat(new string[]
					{
						"Queued RPC failed after waiting 8 seconds : ",
						queuedRPC.sender.Name,
						", entid:",
						queuedRPC.entity_id.ToString(),
						" rpcid:",
						queuedRPC.rpc_id.ToString()
					}));
					this.queued_rpc_list.RemoveAt(m);
				}
				else
				{
					m++;
				}
			}
			if (Time.time >= this.next_netobj_sort)
			{
				this.next_netobj_sort = Time.time + 2.5f;
				List<BindedNetEntity> list = (from o in this.binded_entities
				orderby o.net_component.NetEntityID
				select o).ToList<BindedNetEntity>();
				this.binded_entities = list;
			}
		}

		// Token: 0x0600288F RID: 10383 RVA: 0x000F0494 File Offset: 0x000EE694
		public GameObject CreateGameObject(GameObject prefab, out ushort gameobj_id)
		{
			gameobj_id = this.GetFreeGameObjectID();
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab, Vector3.zero, Quaternion.identity);
			NetGameObject value = new NetGameObject(gameObject, false, Vector3.zero, false, Vector3.zero);
			this.net_gameobj_list.Add(gameobj_id, value);
			return gameObject;
		}

		// Token: 0x06002890 RID: 10384 RVA: 0x000F04DC File Offset: 0x000EE6DC
		public GameObject CreateGameObject(GameObject prefab, Vector3 pos, Quaternion rotation, out ushort gameobj_id)
		{
			gameobj_id = this.GetFreeGameObjectID();
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab, pos, rotation);
			NetGameObject value = new NetGameObject(gameObject, true, pos, true, rotation.eulerAngles);
			this.net_gameobj_list.Add(gameobj_id, value);
			return gameObject;
		}

		// Token: 0x06002891 RID: 10385 RVA: 0x000F051C File Offset: 0x000EE71C
		public void DeleteGameObject(NetBehaviour net_behaviour)
		{
			if (net_behaviour == null)
			{
				Debug.LogWarning("Cannot delete NULL GameObject!");
				return;
			}
			NetGameObject netGameObject = null;
			if (this.net_gameobj_list.TryGetValue(net_behaviour.GameObjID, out netGameObject))
			{
				this.net_gameobj_list.Remove(net_behaviour.GameObjID);
				if (netGameObject.game_obj != null)
				{
					UnityEngine.Object.Destroy(netGameObject.game_obj);
					return;
				}
			}
			else
			{
				Debug.LogWarning("Unable to delete GameObject with id = " + net_behaviour.GameObjID.ToString() + " it does not exist.");
			}
		}

		// Token: 0x06002892 RID: 10386 RVA: 0x000F05A4 File Offset: 0x000EE7A4
		public bool CreateNetEntityChangesMessage(NetOutgoingMessage om)
		{
			if (this.tick_new_entities.Count <= 0 && this.tick_destroyed_entities.Count <= 0)
			{
				return false;
			}
			this.bsw.Reset();
			ushort num = 0;
			for (int i = 0; i < this.tick_new_entities.Count; i++)
			{
				if (this.tick_new_entities[i].net_component.IsAlive)
				{
					num += 1;
				}
			}
			this.bsw.Write(num);
			for (int j = 0; j < this.tick_new_entities.Count; j++)
			{
				BindedNetEntity bindedNetEntity = this.tick_new_entities[j];
				INetComponent net_component = bindedNetEntity.net_component;
				if (net_component.IsAlive)
				{
					this.bsw.Write(net_component.NetTypeID);
					this.bsw.Write(net_component.NetEntityID);
					this.bsw.Write(net_component.Owner.UserID);
					this.bsw.Write(net_component.OwnerSlot);
					bool isGameObject = net_component.IsGameObject;
					this.bsw.Write(isGameObject);
					if (isGameObject)
					{
						NetGameObject netGameObject = this.net_gameobj_list[net_component.GameObjID];
						this.bsw.Write(net_component.GameObjID);
						this.bsw.Write(net_component.PrefabID);
						this.bsw.Write(netGameObject.initial_pos);
						if (netGameObject.initial_pos)
						{
							this.bsw.Write(netGameObject.position.x);
							this.bsw.Write(netGameObject.position.y);
							this.bsw.Write(netGameObject.position.z);
						}
						this.bsw.Write(netGameObject.initial_rotation);
						if (netGameObject.initial_rotation)
						{
							this.bsw.Write(netGameObject.rotation.x);
							this.bsw.Write(netGameObject.rotation.y);
							this.bsw.Write(netGameObject.rotation.z);
						}
					}
					INetVar[] server_send = bindedNetEntity.server_send;
					for (int k = 0; k < server_send.Length; k++)
					{
						this.WriteNetVar(server_send[k], server_send[k].Bits, this.bsw, true, null);
					}
				}
			}
			this.bsw.Write((ushort)this.tick_destroyed_entities.Count);
			for (int l = 0; l < this.tick_destroyed_entities.Count; l++)
			{
				this.bsw.Write(this.tick_destroyed_entities[l].net_component.NetEntityID);
			}
			om.Write((ushort)this.bsw.GetByteLength());
			om.Write(this.bsw.GetBuffer(), 0, this.bsw.GetByteLength());
			return true;
		}

		// Token: 0x06002893 RID: 10387 RVA: 0x0001C88C File Offset: 0x0001AA8C
		public void ClearEntityChanges()
		{
			this.tick_new_entities.Clear();
			this.tick_destroyed_entities.Clear();
		}

		// Token: 0x06002894 RID: 10388 RVA: 0x000F0878 File Offset: 0x000EEA78
		public void ReadNetEntityChangesMessage(NetIncomingMessage im)
		{
			this.bsw.Reset();
			ushort num = im.ReadUInt16();
			if (num <= 0)
			{
				Debug.LogError("Entity changes message size is 0!");
				return;
			}
			try
			{
				this.bsw.Reserve((int)num);
				im.ReadBytes(this.bsw.GetBuffer(), 0, (int)num);
			}
			catch (Exception ex)
			{
				Debug.LogError("Error reading bytes in NetEntityChangesMessage | message_size = " + num.ToString() + ", buffer size = " + im.Data.Length.ToString());
				Debug.LogError(ex.Message);
				return;
			}
			this.bsw.SetDataLength((int)num);
			ushort num2 = this.bsw.ReadUShort();
			for (int i = 0; i < (int)num2; i++)
			{
				ushort key = this.bsw.ReadUShort();
				ushort entity_id = this.bsw.ReadUShort();
				ushort uid = this.bsw.ReadUShort();
				ushort owner_slot = this.bsw.ReadUShort();
				ushort num3 = 0;
				ushort num4 = 0;
				NetPlayer player = NetSystem.GetPlayer(uid);
				NetTypeDefinition netTypeDefinition = this.net_type_definitions[key];
				BindedNetEntity bindedNetEntity = null;
				if (this.bsw.ReadBool())
				{
					GameObject gameObject = null;
					Vector3 zero = Vector3.zero;
					Vector3 zero2 = Vector3.zero;
					Quaternion rotation = Quaternion.identity;
					num3 = this.bsw.ReadUShort();
					num4 = this.bsw.ReadUShort();
					bool flag = this.bsw.ReadBool();
					if (flag)
					{
						zero.x = this.bsw.ReadFloat();
						zero.y = this.bsw.ReadFloat();
						zero.z = this.bsw.ReadFloat();
					}
					bool flag2 = this.bsw.ReadBool();
					if (flag2)
					{
						zero2.x = this.bsw.ReadFloat();
						zero2.y = this.bsw.ReadFloat();
						zero2.z = this.bsw.ReadFloat();
						rotation = Quaternion.Euler(zero2);
					}
					if (!this.net_gameobj_list.ContainsKey(num3))
					{
						NetPrefab netPrefab = null;
						if (NetSystem.IsServer)
						{
							Debug.LogWarning("ReadNetObjectChangesMessage On Server!!");
						}
						else if (player == NetSystem.MyPlayer)
						{
							netPrefab = this.GetNetPrefab(this.net_prefab_list[(int)num4], PrefabType.PREFAB_OWNER);
						}
						else
						{
							netPrefab = this.GetNetPrefab(this.net_prefab_list[(int)num4], PrefabType.PREFAB_PROXY);
						}
						try
						{
							gameObject = UnityEngine.Object.Instantiate<GameObject>(netPrefab.game_object, zero, rotation);
							NetGameObject value = new NetGameObject(gameObject, flag, zero, flag2, zero2);
							this.net_gameobj_list.Add(num3, value);
						}
						catch (Exception ex2)
						{
							Debug.LogError("Error instantiating NetGameObject : " + ex2.ToString());
						}
					}
					Component component = gameObject.GetComponent(netTypeDefinition.object_type);
					if (component != null)
					{
						NetBehaviour netBehaviour = (NetBehaviour)Convert.ChangeType(component, netTypeDefinition.object_type);
						if (netBehaviour != null)
						{
							bindedNetEntity = this.CreateNetEntity(netBehaviour, num3, gameObject, num4, player, owner_slot, (int)entity_id);
						}
						else
						{
							Debug.Log("could not get NetBehaviour from gameobject");
						}
					}
					else
					{
						Debug.LogError("Unable to get component of type : " + netTypeDefinition.object_type.ToString());
					}
				}
				INetVar[] server_send = bindedNetEntity.server_send;
				for (int j = 0; j < server_send.Length; j++)
				{
					this.ReadNetVar(server_send[j], server_send[j].Bits, this.bsw, true);
				}
				try
				{
					bindedNetEntity.net_component.OnNetInitialize();
				}
				catch (Exception exception)
				{
					Debug.LogException(exception);
				}
			}
			ushort num5 = this.bsw.ReadUShort();
			for (int k = 0; k < (int)num5; k++)
			{
				ushort net_entity_id = this.bsw.ReadUShort();
				this.Kill(net_entity_id);
			}
		}

		// Token: 0x06002895 RID: 10389 RVA: 0x000F0C30 File Offset: 0x000EEE30
		public void Kill(ushort net_entity_id)
		{
			BindedNetEntity binded_entity = null;
			if (this.binded_entity_map.TryGetValue(net_entity_id, out binded_entity))
			{
				this.Kill(binded_entity);
			}
		}

		// Token: 0x06002896 RID: 10390 RVA: 0x000F0C58 File Offset: 0x000EEE58
		public void Kill(BindedNetEntity binded_entity)
		{
			if (binded_entity == null)
			{
				return;
			}
			NetBehaviour netBehaviour = (NetBehaviour)binded_entity.net_component;
			if (netBehaviour != null)
			{
				try
				{
					netBehaviour.OnNetDestroy();
				}
				catch (Exception exception)
				{
					Debug.LogError("Unable to properly destroy net entity, error in call to OnNetDestroy");
					Debug.LogException(exception);
				}
				if (netBehaviour.IsGameObject)
				{
					this.DeleteGameObject(netBehaviour);
				}
			}
			else
			{
				this.net_gameobj_list.Remove(netBehaviour.GameObjID);
			}
			this.ReleaseNetEntity(binded_entity);
		}

		// Token: 0x06002897 RID: 10391 RVA: 0x000F0CD4 File Offset: 0x000EEED4
		public void Kill(INetComponent net_component)
		{
			NetBehaviour netBehaviour = (NetBehaviour)net_component;
			if (!(netBehaviour != null))
			{
				Debug.LogError("Could not kill net component, component is null!");
				return;
			}
			if (!netBehaviour.IsAlive)
			{
				Debug.LogWarning("Cannot kill NetEntity that is not alive!");
				return;
			}
			this.Kill(net_component.BindedEntity);
		}

		// Token: 0x06002898 RID: 10392 RVA: 0x000F0D1C File Offset: 0x000EEF1C
		public int ReadSnapshot(NetIncomingMessage im, bool full_snapshot, ref int last_server_ack)
		{
			this.bsw.Reset();
			if (im.LengthBytes < 6)
			{
				Debug.LogWarning("Cannot read snapshot, incoming message size not large enough to define snapshot header!");
				return 0;
			}
			NetPlayer player = NetSystem.GetPlayer(0);
			if (NetSystem.IsServer)
			{
				player = NetSystem.GetPlayer(im.SenderEndPoint);
			}
			int num = im.ReadInt32(24);
			bool flag = false;
			float remote_time = 0f;
			if (!NetSystem.IsServer)
			{
				if (im.ReadBoolean())
				{
					last_server_ack = im.ReadInt32(24);
				}
				flag = im.ReadBoolean();
				if (flag)
				{
					remote_time = im.ReadFloat();
				}
			}
			int num2 = im.LengthBytes - (im.PositionInBytes + 1);
			if (NetSystem.IsServer)
			{
				if (num <= player.LastSnapshotTick)
				{
					return num;
				}
				player.LastSnapshotTick = num;
			}
			else
			{
				if (num <= this.last_snapshot_tick && !full_snapshot)
				{
					return num;
				}
				this.last_snapshot_tick = num;
				if (flag)
				{
					NetSystem.NetTime.UpdateOffset(remote_time, NetSystem.MyPlayer.Connection.AverageRoundtripTime);
				}
			}
			this.bsw.Reserve(num2);
			im.ReadBytes(this.bsw.GetBuffer(), 0, num2);
			this.bsw.SetDataLength(num2);
			SnapshotType snapshotType = full_snapshot ? SnapshotType.FULL : SnapshotType.DELTA;
			if (snapshotType != SnapshotType.FULL)
			{
				if (snapshotType == SnapshotType.DELTA)
				{
					this.ReadDeltaSnapshot(player);
				}
			}
			else if (NetSystem.IsServer)
			{
				this.ReadFullSnapshotServer();
			}
			else
			{
				this.ReadFullSnapshot();
			}
			return num;
		}

		// Token: 0x06002899 RID: 10393 RVA: 0x000F0E60 File Offset: 0x000EF060
		public void CreateFullSnapshot(NetPlayer player, ref ZPBitStream stream)
		{
			stream.Write((ushort)this.binded_entities.Count);
			for (int i = 0; i < this.binded_entities.Count; i++)
			{
				BindedNetEntity bindedNetEntity = this.binded_entities[i];
				INetComponent net_component = bindedNetEntity.net_component;
				if (net_component.IsAlive && net_component.TransmitState != NetTransmitState.NEVER)
				{
					stream.Write(net_component.NetTypeID);
					stream.Write(net_component.NetEntityID);
					stream.Write(net_component.Owner.UserID);
					stream.Write(net_component.OwnerSlot);
					bool isGameObject = net_component.IsGameObject;
					stream.Write(isGameObject);
					if (isGameObject)
					{
						NetGameObject netGameObject = this.net_gameobj_list[net_component.GameObjID];
						stream.Write(net_component.GameObjID);
						stream.Write(net_component.PrefabID);
						stream.Write(netGameObject.initial_pos);
						if (netGameObject.initial_pos)
						{
							stream.Write(netGameObject.position.x);
							stream.Write(netGameObject.position.y);
							stream.Write(netGameObject.position.z);
						}
						stream.Write(netGameObject.initial_rotation);
						if (netGameObject.initial_pos)
						{
							stream.Write(netGameObject.rotation.x);
							stream.Write(netGameObject.rotation.y);
							stream.Write(netGameObject.rotation.z);
						}
					}
					INetVar[] array = null;
					if (player.UserID == bindedNetEntity.net_component.Owner.UserID)
					{
						array = bindedNetEntity.owner_recieve;
					}
					else
					{
						array = bindedNetEntity.proxy_recieve;
					}
					for (int j = 0; j < array.Length; j++)
					{
						try
						{
							this.WriteNetVar(array[j], array[j].Bits, stream, true, player);
						}
						catch (Exception ex)
						{
							Debug.LogError("Failed Writing Netvar: " + ((NetBehaviour)net_component).gameObject.name + " -- " + ex.ToString());
							Debug.LogException(ex);
						}
					}
				}
			}
		}

		// Token: 0x0600289A RID: 10394 RVA: 0x000F1084 File Offset: 0x000EF284
		public void CreateFullSnapshotClient(NetPlayer player, ref ZPBitStream stream)
		{
			ushort num = 0;
			for (int i = 0; i < this.binded_entities.Count; i++)
			{
				INetComponent net_component = this.binded_entities[i].net_component;
				if (net_component.FieldState != NetFieldState.NONE && net_component.FieldState != NetFieldState.RECIEVE && net_component.IsAlive && net_component.TransmitState != NetTransmitState.NEVER)
				{
					num += 1;
				}
			}
			stream.Write(num);
			for (int j = 0; j < this.binded_entities.Count; j++)
			{
				BindedNetEntity bindedNetEntity = this.binded_entities[j];
				INetComponent net_component2 = bindedNetEntity.net_component;
				if (net_component2.FieldState != NetFieldState.NONE && net_component2.FieldState != NetFieldState.RECIEVE && net_component2.IsAlive && net_component2.TransmitState != NetTransmitState.NEVER)
				{
					stream.Write(net_component2.NetEntityID);
					INetVar[] server_send = bindedNetEntity.server_send;
					for (int k = 0; k < server_send.Length; k++)
					{
						try
						{
							this.WriteNetVar(server_send[k], server_send[k].Bits, stream, true, player);
						}
						catch (Exception ex)
						{
							Debug.LogError("Failed Writing Netvar: " + ((NetBehaviour)net_component2).gameObject.name + " -- " + ex.ToString());
						}
					}
				}
			}
		}

		// Token: 0x0600289B RID: 10395 RVA: 0x000F11D4 File Offset: 0x000EF3D4
		private void ReadFullSnapshot()
		{
			ushort num = this.bsw.ReadUShort();
			for (int i = 0; i < (int)num; i++)
			{
				ushort key = this.bsw.ReadUShort();
				ushort num2 = this.bsw.ReadUShort();
				ushort uid = this.bsw.ReadUShort();
				ushort owner_slot = this.bsw.ReadUShort();
				NetPlayer player = NetSystem.GetPlayer(uid);
				NetTypeDefinition netTypeDefinition = this.net_type_definitions[key];
				BindedNetEntity bindedNetEntity = null;
				bool flag = false;
				if (this.bsw.ReadBool())
				{
					Vector3 zero = Vector3.zero;
					Vector3 zero2 = Vector3.zero;
					Quaternion rotation = Quaternion.identity;
					ushort num3 = this.bsw.ReadUShort();
					ushort num4 = this.bsw.ReadUShort();
					bool flag2 = this.bsw.ReadBool();
					if (flag2)
					{
						zero.x = this.bsw.ReadFloat();
						zero.y = this.bsw.ReadFloat();
						zero.z = this.bsw.ReadFloat();
					}
					bool flag3 = this.bsw.ReadBool();
					if (flag3)
					{
						zero2.x = this.bsw.ReadFloat();
						zero2.y = this.bsw.ReadFloat();
						zero2.z = this.bsw.ReadFloat();
						rotation = Quaternion.Euler(zero2);
					}
					if (!this.binded_entity_map.ContainsKey(num2))
					{
						flag = true;
						GameObject gameObject = null;
						if (!this.net_gameobj_list.ContainsKey(num3))
						{
							NetPrefab netPrefab = null;
							if (NetSystem.IsServer)
							{
								Debug.LogWarning("ReadFullSnapshot!! on server!! (shouldn't happen)");
							}
							else if (player == NetSystem.MyPlayer)
							{
								netPrefab = this.GetNetPrefab(this.net_prefab_list[(int)num4], PrefabType.PREFAB_OWNER);
							}
							else
							{
								netPrefab = this.GetNetPrefab(this.net_prefab_list[(int)num4], PrefabType.PREFAB_PROXY);
							}
							try
							{
								gameObject = UnityEngine.Object.Instantiate<GameObject>(netPrefab.game_object, zero, rotation);
								NetGameObject value = new NetGameObject(gameObject, flag2, zero, flag3, zero2);
								this.net_gameobj_list.Add(num3, value);
							}
							catch (Exception ex)
							{
								Debug.LogError("Error instantiating NetGameObject : " + ex.ToString());
							}
						}
						NetBehaviour netBehaviour = (NetBehaviour)Convert.ChangeType(gameObject.GetComponent(netTypeDefinition.object_type), netTypeDefinition.object_type);
						if (netBehaviour != null)
						{
							bindedNetEntity = this.CreateNetEntity(netBehaviour, num3, gameObject, num4, player, owner_slot, (int)num2);
						}
						else
						{
							Debug.Log("could not get NetBehaviour from gameobject");
						}
					}
					else
					{
						bindedNetEntity = this.binded_entity_map[num2].net_component.BindedEntity;
					}
				}
				INetVar[] array;
				if (bindedNetEntity.net_component.IsOwner)
				{
					array = bindedNetEntity.owner_recieve;
				}
				else
				{
					array = bindedNetEntity.proxy_recieve;
				}
				for (int j = 0; j < array.Length; j++)
				{
					this.ReadNetVar(array[j], array[j].Bits, this.bsw, true);
				}
				if (flag)
				{
					try
					{
						bindedNetEntity.net_component.OnNetInitialize();
					}
					catch (Exception exception)
					{
						Debug.LogException(exception);
					}
				}
			}
		}

		// Token: 0x0600289C RID: 10396 RVA: 0x000F14DC File Offset: 0x000EF6DC
		private void ReadFullSnapshotServer()
		{
			ushort num = this.bsw.ReadUShort();
			for (int i = 0; i < (int)num; i++)
			{
				ushort key = this.bsw.ReadUShort();
				BindedNetEntity bindedNetEntity = null;
				if (this.binded_entity_map.TryGetValue(key, out bindedNetEntity))
				{
					INetComponent net_component = bindedNetEntity.net_component;
					BindedNetEntity bindedEntity = net_component.BindedEntity;
					if (net_component.FieldState == NetFieldState.RECIEVE || net_component.FieldState == NetFieldState.SEND_AND_RECIEVE)
					{
						INetVar[] owner_send = bindedEntity.owner_send;
						if (owner_send.Length != 0)
						{
							for (int j = 0; j < owner_send.Length; j++)
							{
								this.ReadNetVar(owner_send[j], owner_send[j].Bits, this.bsw, true);
							}
						}
					}
				}
			}
		}

		// Token: 0x0600289D RID: 10397 RVA: 0x000F1588 File Offset: 0x000EF788
		public bool CreateDeltaSnapshot(NetPlayer player, ref ZPBitStream stream)
		{
			ushort num = 0;
			ushort num2 = 0;
			for (int i = 0; i < this.binded_entities.Count; i++)
			{
				BindedNetEntity bindedNetEntity = this.binded_entities[i];
				INetComponent net_component = bindedNetEntity.net_component;
				bindedNetEntity.should_send = false;
				if (bindedNetEntity == null || bindedNetEntity.game_obj == null)
				{
					if (bindedNetEntity != null)
					{
						Debug.LogError("GameObject Null: " + bindedNetEntity.game_obj_name);
					}
				}
				else if (bindedNetEntity.game_obj.activeInHierarchy && net_component.FieldState != NetFieldState.NONE && net_component.FieldState != NetFieldState.RECIEVE && net_component.IsAlive && net_component.TransmitState != NetTransmitState.NEVER && bindedNetEntity.last_change_tick > player.LastAck)
				{
					bindedNetEntity.should_send = true;
					num += 1;
					num2 = ((bindedNetEntity.net_component.NetEntityID > num2) ? bindedNetEntity.net_component.NetEntityID : num2);
				}
			}
			bool flag = num2 < 255;
			if (flag)
			{
				stream.Write(true);
				stream.Write((byte)num);
			}
			else
			{
				stream.Write(false);
				stream.Write(num);
			}
			bool flag2 = true;
			ushort num3 = 0;
			foreach (BindedNetEntity bindedNetEntity2 in this.binded_entities)
			{
				INetComponent net_component2 = bindedNetEntity2.net_component;
				if (bindedNetEntity2.should_send)
				{
					bool flag3 = num3 + 1 != net_component2.NetEntityID || flag2;
					flag2 = false;
					num3 = net_component2.NetEntityID;
					if (flag3)
					{
						stream.Write(true);
						if (flag)
						{
							stream.Write((byte)net_component2.NetEntityID);
						}
						else
						{
							stream.Write(net_component2.NetEntityID);
						}
					}
					else
					{
						stream.Write(false);
					}
					INetVar[] array = null;
					if (NetSystem.IsServer)
					{
						if (player.UserID == net_component2.Owner.UserID)
						{
							array = bindedNetEntity2.owner_recieve;
						}
						else
						{
							array = bindedNetEntity2.proxy_recieve;
						}
					}
					else if (NetSystem.MyPlayer.UserID == net_component2.Owner.UserID)
					{
						array = bindedNetEntity2.owner_send;
					}
					int j = 0;
					while (j < array.Length)
					{
						if (array[j].LastChangeTick > player.LastAck)
						{
							stream.Write(true);
							try
							{
								this.WriteNetVar(array[j], array[j].Bits, stream, false, player);
								goto IL_263;
							}
							catch (Exception ex)
							{
								Debug.LogError("Failed Writing Netvar: " + ((NetBehaviour)net_component2).gameObject.name + " -- " + ex.ToString());
								goto IL_263;
							}
							goto IL_25B;
						}
						goto IL_25B;
						IL_263:
						j++;
						continue;
						IL_25B:
						stream.Write(false);
						goto IL_263;
					}
				}
			}
			return true;
		}

		// Token: 0x0600289E RID: 10398 RVA: 0x000F1858 File Offset: 0x000EFA58
		private void ReadDeltaSnapshot(NetPlayer sender)
		{
			bool flag = this.bsw.ReadBool();
			ushort num;
			if (flag)
			{
				num = (ushort)this.bsw.ReadByte();
			}
			else
			{
				num = this.bsw.ReadUShort();
			}
			bool flag2 = true;
			ushort num2 = 0;
			for (int i = 0; i < (int)num; i++)
			{
				ushort num3;
				if (this.bsw.ReadBool() || flag2)
				{
					if (flag)
					{
						num3 = (ushort)this.bsw.ReadByte();
					}
					else
					{
						num3 = this.bsw.ReadUShort();
					}
				}
				else
				{
					num3 = num2 + 1;
				}
				flag2 = false;
				num2 = num3;
				BindedNetEntity bindedNetEntity = null;
				if (!this.binded_entity_map.TryGetValue(num3, out bindedNetEntity))
				{
					for (int j = 0; j < this.destroyed_entities.Count; j++)
					{
						if (this.destroyed_entities[j].net_entity_id == num3)
						{
							bindedNetEntity = this.destroyed_entities[j];
							break;
						}
					}
					if (bindedNetEntity == null)
					{
						return;
					}
				}
				INetComponent net_component = bindedNetEntity.net_component;
				bindedNetEntity.did_recieve = true;
				INetVar[] array;
				if (NetSystem.IsServer)
				{
					array = bindedNetEntity.server_recieve;
				}
				else if (net_component.IsOwner)
				{
					array = bindedNetEntity.owner_recieve;
				}
				else
				{
					array = bindedNetEntity.proxy_recieve;
				}
				for (int k = 0; k < array.Length; k++)
				{
					if (this.bsw.ReadBool())
					{
						this.ReadNetVar(array[k], array[k].Bits, this.bsw, false);
						array[k].DidRecieve = true;
					}
				}
			}
			if (this.bsw.GetReadBytePosition() < this.bsw.GetByteLength())
			{
				Debug.LogError("Read delta snapshot underflow : read " + this.bsw.GetReadBytePosition().ToString() + " of " + this.bsw.GetByteLength().ToString());
			}
			else if (this.bsw.GetReadBytePosition() > this.bsw.GetByteLength())
			{
				Debug.LogError("Read delta snapshot overflow : read " + this.bsw.GetReadBytePosition().ToString() + " of " + this.bsw.GetByteLength().ToString());
			}
			int l = 0;
			while (l < this.binded_entities.Count)
			{
				BindedNetEntity bindedNetEntity2 = this.binded_entities[l];
				INetVar[] array2;
				if (NetSystem.IsServer)
				{
					if (bindedNetEntity2.net_component.Owner == sender)
					{
						array2 = bindedNetEntity2.server_recieve;
						goto IL_274;
					}
				}
				else
				{
					if (bindedNetEntity2.net_component.IsOwner)
					{
						array2 = bindedNetEntity2.owner_recieve;
						goto IL_274;
					}
					array2 = bindedNetEntity2.proxy_recieve;
					goto IL_274;
				}
				IL_2C5:
				l++;
				continue;
				IL_274:
				int num4 = array2.Length;
				for (int m = 0; m < num4; m++)
				{
					if ((array2[m].Flags & NetSendFlags.ALWAYS_SEND) == NetSendFlags.ALWAYS_SEND && !array2[m].DidRecieve)
					{
						array2[m].AlwaysSendSet(array2[m].Object);
					}
					array2[m].DidRecieve = false;
				}
				goto IL_2C5;
			}
		}

		// Token: 0x0600289F RID: 10399 RVA: 0x000F1B44 File Offset: 0x000EFD44
		private void WriteNetVar(INetVar net_var, int bits, ZPBitStream zpb, bool full, NetPlayer player)
		{
			NetVarType varType = net_var.VarType;
			object @object = net_var.Object;
			if (net_var.IsArray)
			{
				if (player == null || net_var.LastSizeChangeTick > player.LastAck)
				{
					zpb.Write(true);
					zpb.Write((ushort)net_var.Length);
				}
				else
				{
					zpb.Write(false);
				}
			}
			switch (varType)
			{
			case NetVarType.BOOL:
				zpb.Write((bool)@object);
				return;
			case NetVarType.BYTE:
				zpb.Write((byte)@object, bits);
				return;
			case NetVarType.CHAR:
				zpb.Write((char)@object, bits);
				return;
			case NetVarType.SHORT:
				zpb.Write((short)@object, bits);
				return;
			case NetVarType.USHORT:
				zpb.Write((ushort)@object, bits);
				return;
			case NetVarType.INT:
				zpb.Write((int)@object, bits);
				return;
			case NetVarType.UINT:
				zpb.Write((uint)@object, bits);
				return;
			case NetVarType.LONG:
				zpb.Write((long)@object, bits);
				return;
			case NetVarType.ULONG:
				zpb.Write((ulong)@object, bits);
				return;
			case NetVarType.FLOAT:
				zpb.Write((float)@object, bits);
				return;
			case NetVarType.DOUBLE:
				zpb.Write((double)@object, bits);
				return;
			case NetVarType.VEC2:
			{
				Vector2 vector = (Vector2)@object;
				zpb.Write(vector.x, bits);
				zpb.Write(vector.y, bits);
				return;
			}
			case NetVarType.VEC3:
			{
				Vector3 vector2 = (Vector3)@object;
				zpb.Write(vector2.x, bits);
				zpb.Write(vector2.y, bits);
				zpb.Write(vector2.z, bits);
				return;
			}
			case NetVarType.HALFVEC2:
				return;
			case NetVarType.HALFVEC3:
			{
				HalfVec3 halfVec = (HalfVec3)@object;
				zpb.Write(halfVec.x, bits);
				zpb.Write(halfVec.y, bits);
				zpb.Write(halfVec.z, bits);
				return;
			}
			case NetVarType.ARRAY_BOOL:
			{
				NetArray<bool> netArray = (NetArray<bool>)net_var;
				for (int i = 0; i < netArray.Length; i++)
				{
					zpb.Write(netArray[i]);
				}
				return;
			}
			case NetVarType.ARRAY_BYTE:
			{
				NetArray<byte> netArray2 = (NetArray<byte>)net_var;
				if (full)
				{
					for (int j = 0; j < netArray2.Length; j++)
					{
						zpb.Write(netArray2[j], bits);
					}
					return;
				}
				for (int k = 0; k < netArray2.Length; k++)
				{
					if (player == null || netArray2.SlotChanged(k) > player.LastAck)
					{
						zpb.Write(true);
						zpb.Write(netArray2[k], bits);
					}
					else
					{
						zpb.Write(false);
					}
				}
				return;
			}
			case NetVarType.ARRAY_CHAR:
			{
				NetArray<char> netArray3 = (NetArray<char>)net_var;
				if (full)
				{
					for (int l = 0; l < netArray3.Length; l++)
					{
						zpb.Write(netArray3[l], bits);
					}
					return;
				}
				for (int m = 0; m < netArray3.Length; m++)
				{
					if (player == null || netArray3.SlotChanged(m) > player.LastAck)
					{
						zpb.Write(true);
						zpb.Write(netArray3[m], bits);
					}
					else
					{
						zpb.Write(false);
					}
				}
				return;
			}
			case NetVarType.ARRAY_SHORT:
			{
				NetArray<short> netArray4 = (NetArray<short>)net_var;
				if (full)
				{
					for (int n = 0; n < netArray4.Length; n++)
					{
						zpb.Write(netArray4[n], bits);
					}
					return;
				}
				for (int num = 0; num < netArray4.Length; num++)
				{
					if (player == null || netArray4.SlotChanged(num) > player.LastAck)
					{
						zpb.Write(true);
						zpb.Write(netArray4[num], bits);
					}
					else
					{
						zpb.Write(false);
					}
				}
				return;
			}
			case NetVarType.ARRAY_USHORT:
			{
				NetArray<ushort> netArray5 = (NetArray<ushort>)net_var;
				if (full)
				{
					for (int num2 = 0; num2 < netArray5.Length; num2++)
					{
						zpb.Write(netArray5[num2], bits);
					}
					return;
				}
				for (int num3 = 0; num3 < netArray5.Length; num3++)
				{
					if (player == null || netArray5.SlotChanged(num3) > player.LastAck)
					{
						zpb.Write(true);
						zpb.Write(netArray5[num3], bits);
					}
					else
					{
						zpb.Write(false);
					}
				}
				return;
			}
			case NetVarType.ARRAY_INT:
			{
				NetArray<int> netArray6 = (NetArray<int>)net_var;
				if (full)
				{
					for (int num4 = 0; num4 < netArray6.Length; num4++)
					{
						zpb.Write(netArray6[num4], bits);
					}
					return;
				}
				for (int num5 = 0; num5 < netArray6.Length; num5++)
				{
					if (player == null || netArray6.SlotChanged(num5) > player.LastAck)
					{
						zpb.Write(true);
						zpb.Write(netArray6[num5], bits);
					}
					else
					{
						zpb.Write(false);
					}
				}
				return;
			}
			case NetVarType.ARRAY_UINT:
			{
				NetArray<uint> netArray7 = (NetArray<uint>)net_var;
				if (full)
				{
					for (int num6 = 0; num6 < netArray7.Length; num6++)
					{
						zpb.Write(netArray7[num6], bits);
					}
					return;
				}
				for (int num7 = 0; num7 < netArray7.Length; num7++)
				{
					if (player == null || netArray7.SlotChanged(num7) > player.LastAck)
					{
						zpb.Write(true);
						zpb.Write(netArray7[num7], bits);
					}
					else
					{
						zpb.Write(false);
					}
				}
				return;
			}
			case NetVarType.ARRAY_LONG:
			{
				NetArray<long> netArray8 = (NetArray<long>)net_var;
				if (full)
				{
					for (int num8 = 0; num8 < netArray8.Length; num8++)
					{
						zpb.Write(netArray8[num8], bits);
					}
					return;
				}
				for (int num9 = 0; num9 < netArray8.Length; num9++)
				{
					if (player == null || netArray8.SlotChanged(num9) > player.LastAck)
					{
						zpb.Write(true);
						zpb.Write(netArray8[num9], bits);
					}
					else
					{
						zpb.Write(false);
					}
				}
				return;
			}
			case NetVarType.ARRAY_ULONG:
			{
				NetArray<ulong> netArray9 = (NetArray<ulong>)net_var;
				if (full)
				{
					for (int num10 = 0; num10 < netArray9.Length; num10++)
					{
						zpb.Write(netArray9[num10], bits);
					}
					return;
				}
				for (int num11 = 0; num11 < netArray9.Length; num11++)
				{
					if (player == null || netArray9.SlotChanged(num11) > player.LastAck)
					{
						zpb.Write(true);
						zpb.Write(netArray9[num11], bits);
					}
					else
					{
						zpb.Write(false);
					}
				}
				return;
			}
			case NetVarType.ARRAY_FLOAT:
			{
				NetArray<float> netArray10 = (NetArray<float>)net_var;
				if (full)
				{
					for (int num12 = 0; num12 < netArray10.Length; num12++)
					{
						zpb.Write(netArray10[num12], bits);
					}
					return;
				}
				for (int num13 = 0; num13 < netArray10.Length; num13++)
				{
					if (player == null || netArray10.SlotChanged(num13) > player.LastAck)
					{
						zpb.Write(true);
						zpb.Write(netArray10[num13], bits);
					}
					else
					{
						zpb.Write(false);
					}
				}
				return;
			}
			case NetVarType.ARRAY_DOUBLE:
			{
				NetArray<double> netArray11 = (NetArray<double>)net_var;
				if (full)
				{
					for (int num14 = 0; num14 < netArray11.Length; num14++)
					{
						zpb.Write(netArray11[num14], bits);
					}
					return;
				}
				for (int num15 = 0; num15 < netArray11.Length; num15++)
				{
					if (player == null || netArray11.SlotChanged(num15) > player.LastAck)
					{
						zpb.Write(true);
						zpb.Write(netArray11[num15], bits);
					}
					else
					{
						zpb.Write(false);
					}
				}
				return;
			}
			case NetVarType.ARRAY_STRING:
			{
				NetArray<string> netArray12 = (NetArray<string>)net_var;
				if (full)
				{
					for (int num16 = 0; num16 < netArray12.Length; num16++)
					{
						zpb.Write(netArray12[num16]);
					}
					return;
				}
				for (int num17 = 0; num17 < netArray12.Length; num17++)
				{
					if (player == null || netArray12.SlotChanged(num17) > player.LastAck)
					{
						zpb.Write(true);
						zpb.Write(netArray12[num17]);
					}
					else
					{
						zpb.Write(false);
					}
				}
				return;
			}
			}
			Debug.LogError("Unhandled type in CreateDeltaSnapshot!");
		}

		// Token: 0x060028A0 RID: 10400 RVA: 0x000F22E8 File Offset: 0x000F04E8
		private void ReadNetVar(INetVar net_var, int bits, ZPBitStream zpb, bool full)
		{
			NetVarType varType = net_var.VarType;
			int num = 0;
			bool flag = false;
			if (net_var.IsArray)
			{
				flag = zpb.ReadBool();
				if (flag)
				{
					num = (int)zpb.ReadUShort();
					net_var.Resize(num);
				}
				else
				{
					num = net_var.Length;
				}
			}
			switch (varType)
			{
			case NetVarType.BOOL:
				net_var.SnapshotSet(zpb.ReadBool());
				goto IL_670;
			case NetVarType.BYTE:
				net_var.SnapshotSet(zpb.ReadByte(bits));
				goto IL_670;
			case NetVarType.CHAR:
				net_var.SnapshotSet(zpb.ReadChar(bits));
				goto IL_670;
			case NetVarType.SHORT:
				net_var.SnapshotSet(zpb.ReadShort(bits));
				goto IL_670;
			case NetVarType.USHORT:
				net_var.SnapshotSet(zpb.ReadUShort(bits));
				goto IL_670;
			case NetVarType.INT:
				net_var.SnapshotSet(zpb.ReadInt(bits));
				goto IL_670;
			case NetVarType.UINT:
				net_var.SnapshotSet(zpb.ReadUInt(bits));
				goto IL_670;
			case NetVarType.LONG:
				net_var.SnapshotSet(zpb.ReadLong(bits));
				goto IL_670;
			case NetVarType.ULONG:
				net_var.SnapshotSet(zpb.ReadULong(bits));
				goto IL_670;
			case NetVarType.FLOAT:
				net_var.SnapshotSet(zpb.ReadFloat(bits));
				goto IL_670;
			case NetVarType.DOUBLE:
				net_var.SnapshotSet(zpb.ReadDouble(bits));
				goto IL_670;
			case NetVarType.VEC2:
			{
				float x = zpb.ReadFloat(bits);
				float y = zpb.ReadFloat(bits);
				net_var.SnapshotSet(new Vector2(x, y));
				goto IL_670;
			}
			case NetVarType.VEC3:
			{
				float x2 = zpb.ReadFloat(bits);
				float y2 = zpb.ReadFloat(bits);
				float z = zpb.ReadFloat(bits);
				net_var.SnapshotSet(new Vector3(x2, y2, z));
				goto IL_670;
			}
			case NetVarType.HALFVEC2:
				goto IL_670;
			case NetVarType.HALFVEC3:
			{
				ushort x3 = zpb.ReadUShort(bits);
				ushort y3 = zpb.ReadUShort(bits);
				ushort z2 = zpb.ReadUShort(bits);
				net_var.SnapshotSet(new HalfVec3(x3, y3, z2));
				goto IL_670;
			}
			case NetVarType.ARRAY_BOOL:
			{
				NetArray<bool> netArray = (NetArray<bool>)net_var;
				for (int i = 0; i < num; i++)
				{
					netArray.SnapshotSetIndex(i, zpb.ReadBool());
				}
				goto IL_670;
			}
			case NetVarType.ARRAY_BYTE:
			{
				NetArray<byte> netArray2 = (NetArray<byte>)net_var;
				if (full)
				{
					for (int j = 0; j < num; j++)
					{
						netArray2.SnapshotSetIndex(j, zpb.ReadByte(bits));
					}
					goto IL_670;
				}
				for (int k = 0; k < num; k++)
				{
					if (zpb.ReadBool())
					{
						netArray2.SnapshotSetIndex(k, zpb.ReadByte(bits));
					}
				}
				goto IL_670;
			}
			case NetVarType.ARRAY_CHAR:
			{
				NetArray<char> netArray3 = (NetArray<char>)net_var;
				if (full)
				{
					for (int l = 0; l < num; l++)
					{
						netArray3.SnapshotSetIndex(l, zpb.ReadChar(bits));
					}
					goto IL_670;
				}
				for (int m = 0; m < num; m++)
				{
					if (zpb.ReadBool())
					{
						netArray3.SnapshotSetIndex(m, zpb.ReadChar(bits));
					}
				}
				goto IL_670;
			}
			case NetVarType.ARRAY_SHORT:
			{
				NetArray<short> netArray4 = (NetArray<short>)net_var;
				if (full)
				{
					for (int n = 0; n < num; n++)
					{
						netArray4.SnapshotSetIndex(n, zpb.ReadShort(bits));
					}
					goto IL_670;
				}
				for (int num2 = 0; num2 < num; num2++)
				{
					if (zpb.ReadBool())
					{
						netArray4.SnapshotSetIndex(num2, zpb.ReadShort(bits));
					}
				}
				goto IL_670;
			}
			case NetVarType.ARRAY_USHORT:
			{
				NetArray<ushort> netArray5 = (NetArray<ushort>)net_var;
				if (full)
				{
					for (int num3 = 0; num3 < num; num3++)
					{
						netArray5.SnapshotSetIndex(num3, zpb.ReadUShort(bits));
					}
					goto IL_670;
				}
				for (int num4 = 0; num4 < num; num4++)
				{
					if (zpb.ReadBool())
					{
						netArray5.SnapshotSetIndex(num4, zpb.ReadUShort(bits));
					}
				}
				goto IL_670;
			}
			case NetVarType.ARRAY_INT:
			{
				NetArray<int> netArray6 = (NetArray<int>)net_var;
				if (full)
				{
					for (int num5 = 0; num5 < num; num5++)
					{
						netArray6.SnapshotSetIndex(num5, zpb.ReadInt(bits));
					}
					goto IL_670;
				}
				for (int num6 = 0; num6 < num; num6++)
				{
					if (zpb.ReadBool())
					{
						netArray6.SnapshotSetIndex(num6, zpb.ReadInt(bits));
					}
				}
				goto IL_670;
			}
			case NetVarType.ARRAY_UINT:
			{
				NetArray<uint> netArray7 = (NetArray<uint>)net_var;
				if (full)
				{
					for (int num7 = 0; num7 < num; num7++)
					{
						netArray7.SnapshotSetIndex(num7, zpb.ReadUInt(bits));
					}
					goto IL_670;
				}
				for (int num8 = 0; num8 < num; num8++)
				{
					if (zpb.ReadBool())
					{
						netArray7.SnapshotSetIndex(num8, zpb.ReadUInt(bits));
					}
				}
				goto IL_670;
			}
			case NetVarType.ARRAY_LONG:
			{
				NetArray<long> netArray8 = (NetArray<long>)net_var;
				if (full)
				{
					for (int num9 = 0; num9 < num; num9++)
					{
						netArray8.SnapshotSetIndex(num9, zpb.ReadLong(bits));
					}
					goto IL_670;
				}
				for (int num10 = 0; num10 < num; num10++)
				{
					if (zpb.ReadBool())
					{
						netArray8.SnapshotSetIndex(num10, zpb.ReadLong(bits));
					}
				}
				goto IL_670;
			}
			case NetVarType.ARRAY_ULONG:
			{
				NetArray<ulong> netArray9 = (NetArray<ulong>)net_var;
				if (full)
				{
					for (int num11 = 0; num11 < num; num11++)
					{
						netArray9.SnapshotSetIndex(num11, zpb.ReadULong(bits));
					}
					goto IL_670;
				}
				for (int num12 = 0; num12 < num; num12++)
				{
					if (zpb.ReadBool())
					{
						netArray9.SnapshotSetIndex(num12, zpb.ReadULong(bits));
					}
				}
				goto IL_670;
			}
			case NetVarType.ARRAY_FLOAT:
			{
				NetArray<float> netArray10 = (NetArray<float>)net_var;
				if (full)
				{
					for (int num13 = 0; num13 < num; num13++)
					{
						netArray10.SnapshotSetIndex(num13, zpb.ReadFloat(bits));
					}
					goto IL_670;
				}
				for (int num14 = 0; num14 < num; num14++)
				{
					if (zpb.ReadBool())
					{
						netArray10.SnapshotSetIndex(num14, zpb.ReadFloat(bits));
					}
				}
				goto IL_670;
			}
			case NetVarType.ARRAY_DOUBLE:
			{
				NetArray<double> netArray11 = (NetArray<double>)net_var;
				if (full)
				{
					for (int num15 = 0; num15 < num; num15++)
					{
						netArray11.SnapshotSetIndex(num15, zpb.ReadDouble(bits));
					}
					goto IL_670;
				}
				for (int num16 = 0; num16 < num; num16++)
				{
					if (zpb.ReadBool())
					{
						netArray11.SnapshotSetIndex(num16, zpb.ReadDouble(bits));
					}
				}
				goto IL_670;
			}
			case NetVarType.ARRAY_STRING:
			{
				NetArray<string> netArray12 = (NetArray<string>)net_var;
				if (full)
				{
					for (int num17 = 0; num17 < num; num17++)
					{
						netArray12.SnapshotSetIndex(num17, zpb.ReadString());
					}
					goto IL_670;
				}
				for (int num18 = 0; num18 < num; num18++)
				{
					if (zpb.ReadBool())
					{
						netArray12.SnapshotSetIndex(num18, zpb.ReadString());
					}
				}
				goto IL_670;
			}
			}
			Debug.LogError("Unhandled type in ReadNetObjectChangesMessage!");
			IL_670:
			if (flag)
			{
				net_var.SnapshotResize(num);
			}
		}

		// Token: 0x060028A1 RID: 10401 RVA: 0x000F2970 File Offset: 0x000F0B70
		public BindedNetEntity CreateNetEntity(INetComponent entity, ushort gameobj_id, GameObject g_obj, ushort prefab_id, NetPlayer owner, ushort owner_slot, int entity_id = -1)
		{
			ushort netEntityID;
			if (entity_id == -1)
			{
				netEntityID = this.GetFreeNetworkID();
			}
			else
			{
				netEntityID = (ushort)entity_id;
			}
			entity.NetEntityID = netEntityID;
			entity.NetTypeID = this.net_type_id_map[entity.GetType()];
			entity.GameObjID = gameobj_id;
			entity.PrefabID = prefab_id;
			entity.IsAlive = true;
			entity.IsOwner = false;
			entity.OwnerSlot = owner_slot;
			entity.Owner = owner;
			if (owner == NetSystem.MyPlayer)
			{
				entity.IsOwner = true;
			}
			return this.BindNetVars(entity, g_obj);
		}

		// Token: 0x060028A2 RID: 10402 RVA: 0x000F29F4 File Offset: 0x000F0BF4
		public void ReleaseNetEntity(BindedNetEntity binded_entity)
		{
			INetComponent net_component = binded_entity.net_component;
			ushort net_entity_id = binded_entity.net_entity_id;
			net_component.IsAlive = false;
			binded_entity.destroyed_tick = NetSystem.CurrentTick;
			if (!this.binded_entity_map.Remove(net_entity_id))
			{
				Debug.LogWarning("Failed to remove binded entity from binded entity map!");
			}
			if (!this.binded_entities.Remove(binded_entity))
			{
				Debug.LogWarning("Failed to remove binded entity from binded entity list!");
			}
			if (NetSystem.IsServer)
			{
				this.destroyed_entities.Add(binded_entity);
				this.tick_destroyed_entities.Add(binded_entity);
				return;
			}
			this.destroyed_entities.Add(binded_entity);
		}

		// Token: 0x060028A3 RID: 10403 RVA: 0x000F2A7C File Offset: 0x000F0C7C
		public BindedNetEntity BindNetVars(INetComponent net_component, GameObject game_obj)
		{
			Type type = net_component.GetType();
			ushort key = this.net_type_id_map[type];
			NetTypeDefinition netTypeDefinition = this.net_type_definitions[key];
			List<INetVar> list = new List<INetVar>();
			List<INetVar> list2 = new List<INetVar>();
			List<INetVar> list3 = new List<INetVar>();
			List<INetVar> list4 = new List<INetVar>();
			List<INetVar> list5 = new List<INetVar>();
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < netTypeDefinition.fields.Length; i++)
			{
				INetVar netVar = (INetVar)netTypeDefinition.fields[i].field_info.GetValue(net_component);
				netVar.Bits = netTypeDefinition.fields[i].bits;
				netVar.Flags = netTypeDefinition.fields[i].flags;
				if (netVar.Bits == -1)
				{
					netVar.Bits = netVar.VarType.GetBitSize();
				}
				net_component.FieldState = NetFieldState.NONE;
				if (netTypeDefinition.fields[i].owner == NetSendOwner.OWNER)
				{
					if (net_component.Owner.UserID == 0)
					{
						list2.Add(netVar);
						list5.Add(netVar);
					}
					else
					{
						list.Add(netVar);
						list2.Add(netVar);
						list4.Add(netVar);
						list5.Add(netVar);
					}
					if (NetSystem.IsServer)
					{
						if (net_component.IsOwner)
						{
							num++;
						}
						else
						{
							num++;
							num2++;
						}
					}
					else if (net_component.IsOwner)
					{
						num++;
					}
					else
					{
						num2++;
					}
				}
				else if (netTypeDefinition.fields[i].owner == NetSendOwner.SERVER)
				{
					list2.Add(netVar);
					list5.Add(netVar);
					list3.Add(netVar);
					if (NetSystem.IsServer)
					{
						num++;
					}
					else
					{
						num2++;
					}
				}
			}
			if (num > 0 && num2 > 0)
			{
				net_component.FieldState = NetFieldState.SEND_AND_RECIEVE;
			}
			else if (num > 0)
			{
				net_component.FieldState = NetFieldState.SEND;
			}
			else if (num2 > 0)
			{
				net_component.FieldState = NetFieldState.RECIEVE;
			}
			else
			{
				net_component.FieldState = NetFieldState.NONE;
			}
			BindedNetEntity bindedNetEntity = new BindedNetEntity(net_component, netTypeDefinition.net_type_id, net_component.GameObjID, net_component.NetEntityID, net_component.IsGameObject, list.ToArray(), list2.ToArray(), list3.ToArray(), list4.ToArray(), list5.ToArray());
			bindedNetEntity.game_obj = game_obj;
			if (game_obj != null)
			{
				bindedNetEntity.game_obj_name = game_obj.name;
			}
			net_component.BindedEntity = bindedNetEntity;
			this.binded_entities.Add(bindedNetEntity);
			this.binded_entity_map.Add(bindedNetEntity.net_entity_id, bindedNetEntity);
			if (NetSystem.IsServer)
			{
				this.tick_new_entities.Add(bindedNetEntity);
			}
			return bindedNetEntity;
		}

		// Token: 0x060028A4 RID: 10404 RVA: 0x000F2D04 File Offset: 0x000F0F04
		public NetRPCDefinition GetRPCDefinition(INetComponent entity, string method_name)
		{
			NetTypeDefinition netTypeDefinition = null;
			if (!this.net_type_definitions.TryGetValue(entity.NetTypeID, out netTypeDefinition))
			{
				Debug.LogError("Could not get RPC Definition, entity type '" + entity.NetTypeID.ToString() + "' not present!");
				return null;
			}
			NetRPCDefinition result = null;
			if (!netTypeDefinition.rpc_string_table.TryGetValue(method_name, out result))
			{
				Debug.LogError("NetTypeID = " + entity.NetTypeID.ToString());
				Debug.LogError("NetType = " + netTypeDefinition.net_type_id.ToString());
				foreach (KeyValuePair<string, NetRPCDefinition> keyValuePair in netTypeDefinition.rpc_string_table)
				{
					Debug.LogError(keyValuePair.Key + ", " + keyValuePair.Value.MethodName);
				}
				Debug.LogError("Could not find RPC definition " + method_name);
				return null;
			}
			return result;
		}

		// Token: 0x060028A5 RID: 10405 RVA: 0x000F2E10 File Offset: 0x000F1010
		public bool CreateRPCPacket(ref NetOutgoingMessage om, NetRPCDefinition rpc_definition, INetComponent entity, object[] parameters)
		{
			ushort netEntityID = entity.NetEntityID;
			NetPlayer myPlayer = NetSystem.MyPlayer;
			if (!rpc_definition.CanSendRPC(myPlayer, entity))
			{
				Debug.LogError(string.Concat(new string[]
				{
					"User '",
					myPlayer.Name,
					"' attempted to send RPC '",
					rpc_definition.RPCid.ToString(),
					"' when they do not have permission to!"
				}));
				return false;
			}
			if (rpc_definition.GetParameterCount() != parameters.Length)
			{
				Debug.LogError(string.Concat(new string[]
				{
					"Incorrect number of parameters for RPC Method [",
					rpc_definition.MethodName,
					"], requires ",
					rpc_definition.GetParameterCount().ToString(),
					" parameters!"
				}));
				return false;
			}
			this.bsw.Reset();
			if (NetSystem.IsServer)
			{
				this.bsw.Write(6);
			}
			else
			{
				this.bsw.Write(2);
			}
			this.bsw.Write(netEntityID);
			this.bsw.Write(rpc_definition.RPCid);
			for (int i = 0; i < rpc_definition.GetParameterCount(); i++)
			{
				switch (rpc_definition.GetParameterType(i))
				{
				case NetRPCType.BOOL:
					this.bsw.Write((bool)parameters[i]);
					break;
				case NetRPCType.BYTE:
					this.bsw.Write((byte)parameters[i]);
					break;
				case NetRPCType.CHAR:
					this.bsw.Write((char)parameters[i]);
					break;
				case NetRPCType.SHORT:
					this.bsw.Write((short)parameters[i]);
					break;
				case NetRPCType.USHORT:
					this.bsw.Write((ushort)parameters[i]);
					break;
				case NetRPCType.INT:
					this.bsw.Write((int)parameters[i]);
					break;
				case NetRPCType.UINT:
					this.bsw.Write((uint)parameters[i]);
					break;
				case NetRPCType.LONG:
					this.bsw.Write((long)parameters[i]);
					break;
				case NetRPCType.ULONG:
					this.bsw.Write((ulong)parameters[i]);
					break;
				case NetRPCType.FLOAT:
					this.bsw.Write((float)parameters[i]);
					break;
				case NetRPCType.DOUBLE:
					this.bsw.Write((double)parameters[i]);
					break;
				case NetRPCType.STRING:
					this.bsw.Write((string)parameters[i]);
					break;
				case NetRPCType.VECTOR2:
				{
					Vector2 vector = (Vector2)parameters[i];
					this.bsw.Write(vector.x);
					this.bsw.Write(vector.y);
					break;
				}
				case NetRPCType.VECTOR3:
				{
					Vector3 vector2 = (Vector3)parameters[i];
					this.bsw.Write(vector2.x);
					this.bsw.Write(vector2.y);
					this.bsw.Write(vector2.z);
					break;
				}
				case NetRPCType.ARRAY_BOOL:
				{
					bool[] array = (bool[])parameters[i];
					this.bsw.Write((ushort)array.Length);
					for (int j = 0; j < array.Length; j++)
					{
						this.bsw.Write(array[j]);
					}
					break;
				}
				case NetRPCType.ARRAY_BYTE:
				{
					byte[] array2 = (byte[])parameters[i];
					this.bsw.Write((ushort)array2.Length);
					for (int k = 0; k < array2.Length; k++)
					{
						this.bsw.Write(array2[k]);
					}
					break;
				}
				case NetRPCType.ARRAY_CHAR:
				{
					char[] array3 = (char[])parameters[i];
					this.bsw.Write((ushort)array3.Length);
					for (int l = 0; l < array3.Length; l++)
					{
						this.bsw.Write(array3[l]);
					}
					break;
				}
				case NetRPCType.ARRAY_SHORT:
				{
					short[] array4 = (short[])parameters[i];
					this.bsw.Write((ushort)array4.Length);
					for (int m = 0; m < array4.Length; m++)
					{
						this.bsw.Write(array4[m]);
					}
					break;
				}
				case NetRPCType.ARRAY_USHORT:
				{
					ushort[] array5 = (ushort[])parameters[i];
					this.bsw.Write((ushort)array5.Length);
					for (int n = 0; n < array5.Length; n++)
					{
						this.bsw.Write(array5[n]);
					}
					break;
				}
				case NetRPCType.ARRAY_INT:
				{
					int[] array6 = (int[])parameters[i];
					this.bsw.Write((ushort)array6.Length);
					for (int num = 0; num < array6.Length; num++)
					{
						this.bsw.Write(array6[num]);
					}
					break;
				}
				case NetRPCType.ARRAY_UINT:
				{
					uint[] array7 = (uint[])parameters[i];
					this.bsw.Write((ushort)array7.Length);
					for (int num2 = 0; num2 < array7.Length; num2++)
					{
						this.bsw.Write(array7[num2]);
					}
					break;
				}
				case NetRPCType.ARRAY_LONG:
				{
					long[] array8 = (long[])parameters[i];
					this.bsw.Write((ushort)array8.Length);
					for (int num3 = 0; num3 < array8.Length; num3++)
					{
						this.bsw.Write(array8[num3]);
					}
					break;
				}
				case NetRPCType.ARRAY_ULONG:
				{
					ulong[] array9 = (ulong[])parameters[i];
					this.bsw.Write((ushort)array9.Length);
					for (int num4 = 0; num4 < array9.Length; num4++)
					{
						this.bsw.Write(array9[num4]);
					}
					break;
				}
				case NetRPCType.ARRAY_FLOAT:
				{
					float[] array10 = (float[])parameters[i];
					this.bsw.Write((ushort)array10.Length);
					for (int num5 = 0; num5 < array10.Length; num5++)
					{
						this.bsw.Write(array10[num5]);
					}
					break;
				}
				case NetRPCType.ARRAY_DOUBLE:
				{
					double[] array11 = (double[])parameters[i];
					this.bsw.Write((double)array11.Length);
					for (int num6 = 0; num6 < array11.Length; num6++)
					{
						this.bsw.Write(array11[num6]);
					}
					break;
				}
				case NetRPCType.ARRAY_STRING:
				{
					string[] array12 = (string[])parameters[i];
					this.bsw.Write((ushort)array12.Length);
					for (int num7 = 0; num7 < array12.Length; num7++)
					{
						this.bsw.Write(array12[num7]);
					}
					break;
				}
				case NetRPCType.ARRAY_VECTOR2:
				{
					Vector2[] array13 = (Vector2[])parameters[i];
					this.bsw.Write((ushort)array13.Length);
					for (int num8 = 0; num8 < array13.Length; num8++)
					{
						this.bsw.Write(array13[num8].x);
						this.bsw.Write(array13[num8].y);
					}
					break;
				}
				case NetRPCType.ARRAY_VECTOR3:
				{
					Vector3[] array14 = (Vector3[])parameters[i];
					this.bsw.Write((ushort)array14.Length);
					for (int num9 = 0; num9 < array14.Length; num9++)
					{
						this.bsw.Write(array14[num9].x);
						this.bsw.Write(array14[num9].y);
						this.bsw.Write(array14[num9].z);
					}
					break;
				}
				}
			}
			om.Write(this.bsw.GetBuffer(), 0, this.bsw.GetByteLength());
			return true;
		}

		// Token: 0x060028A6 RID: 10406 RVA: 0x000F3594 File Offset: 0x000F1794
		public bool InvokeRPC(NetPlayer sender, byte[] msg_data, int msg_bit_length, bool queued = false)
		{
			BindedNetEntity bindedNetEntity = null;
			INetComponent netComponent = null;
			this.bsw.Reset();
			this.bsw.SetData(msg_data, msg_bit_length);
			ushort num = this.bsw.ReadUShort();
			byte b = this.bsw.ReadByte();
			if (!this.binded_entity_map.TryGetValue(num, out bindedNetEntity))
			{
				for (int i = 0; i < this.destroyed_entities.Count; i++)
				{
					if (num == this.destroyed_entities[i].net_entity_id)
					{
						return false;
					}
				}
				if (!queued)
				{
					Debug.LogWarning("Cannot invoke RPC on entity with id '" + num.ToString() + "' entity does not currently exist, queueing rpc to be called later!");
					this.queued_rpc_list.Insert(0, new QueuedRPC(sender, num, b, msg_data, msg_bit_length));
				}
				return false;
			}
			netComponent = bindedNetEntity.net_component;
			NetTypeDefinition netTypeDefinition = this.net_type_definitions[netComponent.NetTypeID];
			if ((int)b < netTypeDefinition.rpc_id_table.Length)
			{
				NetRPCDefinition netRPCDefinition = netTypeDefinition.rpc_id_table[(int)b];
				if (!netRPCDefinition.CanRecieveRPC(sender, netComponent))
				{
					Debug.LogWarning(string.Concat(new string[]
					{
						"User '",
						sender.Name,
						"' attempted to invoke RPC '",
						b.ToString(),
						"' when they do not have permission to!"
					}));
					return false;
				}
				object[] array = new object[netRPCDefinition.GetParameterCount() + 1];
				array[0] = sender;
				ushort num2 = 0;
				for (int j = 1; j < netRPCDefinition.GetParameterCount() + 1; j++)
				{
					if (netRPCDefinition.IsParamaterArray(j - 1))
					{
						num2 = this.bsw.ReadUShort();
					}
					switch (netRPCDefinition.GetParameterType(j - 1))
					{
					case NetRPCType.BOOL:
						array[j] = this.bsw.ReadBool();
						break;
					case NetRPCType.BYTE:
						array[j] = this.bsw.ReadByte();
						break;
					case NetRPCType.CHAR:
						array[j] = (char)this.bsw.ReadUShort();
						break;
					case NetRPCType.SHORT:
						array[j] = this.bsw.ReadShort();
						break;
					case NetRPCType.USHORT:
						array[j] = this.bsw.ReadUShort();
						break;
					case NetRPCType.INT:
						array[j] = this.bsw.ReadInt();
						break;
					case NetRPCType.UINT:
						array[j] = this.bsw.ReadUInt();
						break;
					case NetRPCType.LONG:
						array[j] = this.bsw.ReadLong();
						break;
					case NetRPCType.ULONG:
						array[j] = this.bsw.ReadULong();
						break;
					case NetRPCType.FLOAT:
						array[j] = this.bsw.ReadFloat();
						break;
					case NetRPCType.DOUBLE:
						array[j] = this.bsw.ReadDouble();
						break;
					case NetRPCType.STRING:
						array[j] = this.bsw.ReadString();
						break;
					case NetRPCType.VECTOR2:
						array[j] = new Vector2
						{
							x = this.bsw.ReadFloat(),
							y = this.bsw.ReadFloat()
						};
						break;
					case NetRPCType.VECTOR3:
						array[j] = new Vector3
						{
							x = this.bsw.ReadFloat(),
							y = this.bsw.ReadFloat(),
							z = this.bsw.ReadFloat()
						};
						break;
					case NetRPCType.ARRAY_BOOL:
					{
						bool[] array2 = new bool[(int)num2];
						for (int k = 0; k < (int)num2; k++)
						{
							array2[k] = this.bsw.ReadBool();
						}
						array[j] = array2;
						break;
					}
					case NetRPCType.ARRAY_BYTE:
					{
						byte[] array3 = new byte[(int)num2];
						for (int l = 0; l < (int)num2; l++)
						{
							array3[l] = this.bsw.ReadByte();
						}
						array[j] = array3;
						break;
					}
					case NetRPCType.ARRAY_CHAR:
					{
						char[] array4 = new char[(int)num2];
						for (int m = 0; m < (int)num2; m++)
						{
							array4[m] = (char)this.bsw.ReadUShort();
						}
						array[j] = array4;
						break;
					}
					case NetRPCType.ARRAY_SHORT:
					{
						short[] array5 = new short[(int)num2];
						for (int n = 0; n < (int)num2; n++)
						{
							array5[n] = this.bsw.ReadShort();
						}
						array[j] = array5;
						break;
					}
					case NetRPCType.ARRAY_USHORT:
					{
						ushort[] array6 = new ushort[(int)num2];
						for (int num3 = 0; num3 < (int)num2; num3++)
						{
							array6[num3] = this.bsw.ReadUShort();
						}
						array[j] = array6;
						break;
					}
					case NetRPCType.ARRAY_INT:
					{
						int[] array7 = new int[(int)num2];
						for (int num4 = 0; num4 < (int)num2; num4++)
						{
							array7[num4] = this.bsw.ReadInt();
						}
						array[j] = array7;
						break;
					}
					case NetRPCType.ARRAY_UINT:
					{
						uint[] array8 = new uint[(int)num2];
						for (int num5 = 0; num5 < (int)num2; num5++)
						{
							array8[num5] = this.bsw.ReadUInt();
						}
						array[j] = array8;
						break;
					}
					case NetRPCType.ARRAY_LONG:
					{
						long[] array9 = new long[(int)num2];
						for (int num6 = 0; num6 < (int)num2; num6++)
						{
							array9[num6] = this.bsw.ReadLong();
						}
						array[j] = array9;
						break;
					}
					case NetRPCType.ARRAY_ULONG:
					{
						ulong[] array10 = new ulong[(int)num2];
						for (int num7 = 0; num7 < (int)num2; num7++)
						{
							array10[num7] = this.bsw.ReadULong();
						}
						array[j] = array10;
						break;
					}
					case NetRPCType.ARRAY_FLOAT:
					{
						float[] array11 = new float[(int)num2];
						for (int num8 = 0; num8 < (int)num2; num8++)
						{
							array11[num8] = this.bsw.ReadFloat();
						}
						array[j] = array11;
						break;
					}
					case NetRPCType.ARRAY_DOUBLE:
					{
						double[] array12 = new double[(int)num2];
						for (int num9 = 0; num9 < (int)num2; num9++)
						{
							array12[num9] = this.bsw.ReadDouble();
						}
						array[j] = array12;
						break;
					}
					case NetRPCType.ARRAY_STRING:
					{
						string[] array13 = new string[(int)num2];
						for (int num10 = 0; num10 < (int)num2; num10++)
						{
							array13[num10] = this.bsw.ReadString();
						}
						array[j] = array13;
						break;
					}
					case NetRPCType.ARRAY_VECTOR2:
					{
						Vector2[] array14 = new Vector2[(int)num2];
						for (int num11 = 0; num11 < (int)num2; num11++)
						{
							array14[num11].x = this.bsw.ReadFloat();
							array14[num11].y = this.bsw.ReadFloat();
						}
						array[j] = array14;
						break;
					}
					case NetRPCType.ARRAY_VECTOR3:
					{
						Vector3[] array15 = new Vector3[(int)num2];
						for (int num12 = 0; num12 < (int)num2; num12++)
						{
							array15[num12].x = this.bsw.ReadFloat();
							array15[num12].y = this.bsw.ReadFloat();
							array15[num12].z = this.bsw.ReadFloat();
						}
						array[j] = array15;
						break;
					}
					}
				}
				try
				{
					netRPCDefinition.Method.Invoke(netComponent, array);
				}
				catch (Exception ex)
				{
					Debug.LogError("Exception Invoking RPC : " + netRPCDefinition.MethodName);
					Debug.LogError(ex.StackTrace);
					Debug.LogError(ex.Message);
				}
				if (NetSystem.IsServer && !queued)
				{
					object[] array16 = new object[array.Length - 1];
					for (int num13 = 1; num13 < array.Length; num13++)
					{
						array16[num13 - 1] = array[num13];
					}
					NetGameServer.RelayRPC(sender, msg_data, msg_bit_length, netComponent, netRPCDefinition, array16);
				}
			}
			else
			{
				Debug.LogWarning(string.Concat(new string[]
				{
					"RPC with id '",
					b.ToString(),
					"' does not exist on entity '",
					netTypeDefinition.object_type.ToString(),
					"', cannot invoke!"
				}));
			}
			return true;
		}

		// Token: 0x060028A7 RID: 10407 RVA: 0x000F3D88 File Offset: 0x000F1F88
		public void InitiateNetSendObjects()
		{
			NetPrefabList netPrefabList = Resources.Load<NetPrefabList>("Net/NetPrefabList");
			if (netPrefabList != null)
			{
				for (int i = 0; i < netPrefabList.net_prefabs.Length; i++)
				{
					this.net_prefab_list.Add("");
				}
				for (int j = 0; j < netPrefabList.net_prefabs.Length; j++)
				{
					NetPrefabDefinition netPrefabDefinition = netPrefabList.net_prefabs[j];
					if (!this.net_prefab_map.ContainsKey(netPrefabDefinition.prefab_name))
					{
						this.net_prefab_map.Add(netPrefabDefinition.prefab_name, netPrefabDefinition);
						this.net_prefab_list[netPrefabDefinition.prefab_id] = netPrefabDefinition.prefab_name;
					}
					else
					{
						Debug.LogWarning("There are 2 or more NetPrefabs with the name " + netPrefabDefinition.prefab_name + ", please give all NetPrefabs unique names.");
					}
				}
			}
			else
			{
				Debug.LogError("NetPrefabList does not exist, maybe it hasn't been generated? go to 'ZP>Networking>Update NetPrefabs'");
			}
			foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
			{
				if (type.IsSubclassOf(typeof(NetBehaviour)) || type.IsSubclassOf(typeof(NetClass)))
				{
					NetTypeDefinition netTypeDefinition = new NetTypeDefinition();
					ushort num = (ushort)this.net_type_definitions.Count;
					netTypeDefinition.net_type_id = num;
					netTypeDefinition.object_type = type;
					netTypeDefinition.is_behaviour = type.IsSubclassOf(typeof(NetBehaviour));
					List<NetFieldDefinition> list = new List<NetFieldDefinition>();
					List<NetFieldDefinition> list2 = new List<NetFieldDefinition>();
					List<NetFieldDefinition> list3 = new List<NetFieldDefinition>();
					List<NetRPCDefinition> list4 = new List<NetRPCDefinition>();
					Dictionary<string, NetRPCDefinition> dictionary = new Dictionary<string, NetRPCDefinition>();
					foreach (FieldInfo fieldInfo in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
					{
						foreach (object obj in fieldInfo.GetCustomAttributes(false))
						{
							if (obj.GetType() == typeof(NetSend))
							{
								NetSend netSend = (NetSend)obj;
								NetFieldDefinition item = new NetFieldDefinition(Type.GetTypeCode(type), netSend.Owner, fieldInfo, (int)netSend.Bits, netSend.Flags);
								NetSendOwner owner = netSend.Owner;
								if (owner != NetSendOwner.SERVER)
								{
									if (owner == NetSendOwner.OWNER)
									{
										list3.Add(item);
									}
								}
								else
								{
									list2.Add(item);
								}
								list.Add(item);
							}
						}
					}
					netTypeDefinition.fields = list.ToArray();
					Dictionary<Type, NetRPCType> dictionary2 = new Dictionary<Type, NetRPCType>();
					dictionary2.Add(typeof(bool), NetRPCType.BOOL);
					dictionary2.Add(typeof(byte), NetRPCType.BYTE);
					dictionary2.Add(typeof(char), NetRPCType.CHAR);
					dictionary2.Add(typeof(short), NetRPCType.SHORT);
					dictionary2.Add(typeof(ushort), NetRPCType.USHORT);
					dictionary2.Add(typeof(int), NetRPCType.INT);
					dictionary2.Add(typeof(uint), NetRPCType.UINT);
					dictionary2.Add(typeof(long), NetRPCType.LONG);
					dictionary2.Add(typeof(ulong), NetRPCType.ULONG);
					dictionary2.Add(typeof(float), NetRPCType.FLOAT);
					dictionary2.Add(typeof(double), NetRPCType.DOUBLE);
					dictionary2.Add(typeof(string), NetRPCType.STRING);
					dictionary2.Add(typeof(Vector2), NetRPCType.VECTOR2);
					dictionary2.Add(typeof(Vector3), NetRPCType.VECTOR3);
					dictionary2.Add(typeof(bool[]), NetRPCType.ARRAY_BOOL);
					dictionary2.Add(typeof(byte[]), NetRPCType.ARRAY_BYTE);
					dictionary2.Add(typeof(char[]), NetRPCType.ARRAY_CHAR);
					dictionary2.Add(typeof(short[]), NetRPCType.ARRAY_SHORT);
					dictionary2.Add(typeof(ushort[]), NetRPCType.ARRAY_USHORT);
					dictionary2.Add(typeof(int[]), NetRPCType.ARRAY_INT);
					dictionary2.Add(typeof(uint[]), NetRPCType.ARRAY_UINT);
					dictionary2.Add(typeof(long[]), NetRPCType.ARRAY_LONG);
					dictionary2.Add(typeof(ulong[]), NetRPCType.ARRAY_ULONG);
					dictionary2.Add(typeof(float[]), NetRPCType.ARRAY_FLOAT);
					dictionary2.Add(typeof(double[]), NetRPCType.ARRAY_DOUBLE);
					dictionary2.Add(typeof(string[]), NetRPCType.ARRAY_STRING);
					dictionary2.Add(typeof(Vector2[]), NetRPCType.ARRAY_VECTOR2);
					dictionary2.Add(typeof(Vector3[]), NetRPCType.ARRAY_VECTOR3);
					foreach (MethodInfo methodInfo in type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
					{
						foreach (object obj2 in methodInfo.GetCustomAttributes(false))
						{
							if (obj2.GetType() == typeof(NetRPC))
							{
								NetRPC netRPC = (NetRPC)obj2;
								ParameterInfo[] parameters = methodInfo.GetParameters();
								bool flag = true;
								if (parameters.Length < 1)
								{
									Debug.LogError("RPC method must have at least one parameter of NetPlayer! : " + methodInfo.Name);
								}
								else
								{
									NetRPCType[] array = null;
									bool[] array2 = null;
									int num2 = 4;
									bool has_array_param = false;
									if (parameters.Length > 1)
									{
										array = new NetRPCType[parameters.Length - 1];
										array2 = new bool[parameters.Length - 1];
										for (int n = 0; n < parameters.Length; n++)
										{
											if (n == 0)
											{
												if (parameters[n].ParameterType != typeof(NetPlayer))
												{
													Debug.LogError("First parameter of RPC method must be NetPlayer! : " + methodInfo.Name);
													flag = false;
													break;
												}
											}
											else
											{
												Type parameterType = parameters[n].ParameterType;
												NetRPCType netRPCType = NetRPCType.UNKNOWN;
												dictionary2.TryGetValue(parameterType, out netRPCType);
												if (netRPCType == NetRPCType.UNKNOWN)
												{
													Debug.LogError("NetObjectManager : InitializeNetObjects failed, rpc found with unknown or unsupported type! : " + methodInfo.Name);
													flag = false;
												}
												else
												{
													array[n - 1] = netRPCType;
													array2[n - 1] = parameterType.IsArray;
													if (parameterType.IsArray)
													{
														num2 += 2;
														has_array_param = true;
													}
													else
													{
														num2 += netRPCType.GetByteSize();
													}
												}
											}
										}
									}
									else if (parameters[0].ParameterType != typeof(NetPlayer))
									{
										Debug.LogError("First parameter of RPC method must be NetPlayer! : " + methodInfo.Name);
										flag = false;
									}
									if (flag)
									{
										if (list4.Count > 255)
										{
											Debug.LogError("Too many RPC methods on Type [" + type.Name + "] maximum 255 rpc methods per class : " + methodInfo.Name);
										}
										else
										{
											bool relay = netRPC.Relay;
											NetRPCSecurity netRPCSecurity = netRPC.Send;
											NetRPCSecurity netRPCSecurity2 = netRPC.Recieve;
											if ((netRPCSecurity & NetRPCSecurity.OWNER) == NetRPCSecurity.OWNER & (netRPCSecurity & NetRPCSecurity.PROXY) == NetRPCSecurity.PROXY & (netRPCSecurity & NetRPCSecurity.SERVER) == NetRPCSecurity.SERVER)
											{
												netRPCSecurity = NetRPCSecurity.ALL;
											}
											if ((netRPCSecurity2 & NetRPCSecurity.OWNER) == NetRPCSecurity.OWNER & (netRPCSecurity2 & NetRPCSecurity.PROXY) == NetRPCSecurity.PROXY & (netRPCSecurity2 & NetRPCSecurity.SERVER) == NetRPCSecurity.SERVER)
											{
												netRPCSecurity2 = NetRPCSecurity.ALL;
											}
											byte rpc_id = (byte)list4.Count;
											NetRPCDefinition netRPCDefinition = new NetRPCDefinition(methodInfo.Name, rpc_id, methodInfo, array, array2, has_array_param, num2, relay, netRPCSecurity, netRPCSecurity2);
											list4.Add(netRPCDefinition);
											dictionary.Add(methodInfo.Name, netRPCDefinition);
										}
									}
								}
							}
						}
					}
					netTypeDefinition.rpc_id_table = list4.ToArray();
					netTypeDefinition.rpc_string_table = dictionary;
					this.net_type_id_map.Add(type, num);
					this.net_type_definitions.Add(num, netTypeDefinition);
				}
			}
		}

		// Token: 0x060028A8 RID: 10408 RVA: 0x000F44D4 File Offset: 0x000F26D4
		public NetPrefab GetNetPrefab(string name, PrefabType prefab_type)
		{
			Dictionary<string, NetPrefab> dictionary = null;
			NetPrefab netPrefab = new NetPrefab(null, 0);
			if (!this.net_prefab_map.ContainsKey(name))
			{
				Debug.LogError("Prefab with name [" + name + "] does not exist");
				return null;
			}
			netPrefab.prefab_id = (ushort)this.net_prefab_map[name].prefab_id;
			string resource_location = this.net_prefab_map[name].resource_location;
			switch (prefab_type)
			{
			case PrefabType.PREFAB_PROXY:
				dictionary = this.proxy_prefabs;
				break;
			case PrefabType.PREFAB_OWNER:
				dictionary = this.owner_prefabs;
				break;
			case PrefabType.PREFAB_HOST:
				dictionary = this.host_prefabs;
				break;
			case PrefabType.PREFAB_DEDICATED:
				dictionary = this.dedicated_prefabs;
				break;
			}
			if (dictionary.ContainsKey(name))
			{
				netPrefab = dictionary[name];
			}
			else
			{
				string text = "";
				string path = resource_location;
				switch (prefab_type)
				{
				case PrefabType.PREFAB_PROXY:
					text = resource_location + name + "@proxy";
					break;
				case PrefabType.PREFAB_OWNER:
					text = resource_location + name + "@owner";
					break;
				case PrefabType.PREFAB_HOST:
					text = resource_location + name + "@host";
					break;
				case PrefabType.PREFAB_DEDICATED:
					text = resource_location + name + "@dedicated";
					break;
				}
				GameObject gameObject = Resources.Load<GameObject>(text);
				GameObject gameObject2;
				if (gameObject != null)
				{
					gameObject2 = gameObject;
				}
				else
				{
					gameObject2 = Resources.Load<GameObject>(path);
					if (gameObject2 == null)
					{
						Debug.LogError("Attempt to load prefab at [" + text + "] failed!");
						return null;
					}
				}
				netPrefab.game_object = gameObject2;
				dictionary.Add(name, netPrefab);
			}
			if (netPrefab.game_object == null)
			{
				Debug.LogError(string.Concat(new string[]
				{
					"Could not load prefab [",
					name,
					":",
					prefab_type.ToString(),
					"]!"
				}));
				return null;
			}
			return netPrefab;
		}

		// Token: 0x060028A9 RID: 10409 RVA: 0x000F4690 File Offset: 0x000F2890
		public GameObject GetGameObject(ushort gameobj_id)
		{
			NetGameObject netGameObject = null;
			if (this.net_gameobj_list.TryGetValue(gameobj_id, out netGameObject))
			{
				return netGameObject.game_obj;
			}
			Debug.LogWarning("NetGameObject with id " + gameobj_id.ToString() + " does not exist!");
			return null;
		}

		// Token: 0x060028AA RID: 10410 RVA: 0x000F46D4 File Offset: 0x000F28D4
		private ushort GetFreeNetworkID()
		{
			if (this.released_network_ids.Count > 0)
			{
				ushort num = this.released_network_ids[0];
				this.released_network_ids.RemoveAt(0);
				this.network_id_status[(int)num] = true;
				return num;
			}
			ushort num2 = this.last_network_id + 1;
			while (num2 != this.last_network_id)
			{
				if (!this.network_id_status[(int)num2])
				{
					this.last_network_id = num2;
					this.network_id_status[(int)num2] = true;
					return num2;
				}
				if (num2 >= 65534)
				{
					num2 = 0;
				}
				else
				{
					num2 += 1;
				}
			}
			return 0;
		}

		// Token: 0x060028AB RID: 10411 RVA: 0x0001C8A4 File Offset: 0x0001AAA4
		private void ReleaseNetworkID(ushort id)
		{
			this.released_network_ids.Add(id);
			this.network_id_status[(int)id] = false;
		}

		// Token: 0x060028AC RID: 10412 RVA: 0x000F4758 File Offset: 0x000F2958
		private ushort GetFreeGameObjectID()
		{
			if (this.released_gameobj_ids.Count > 0)
			{
				ushort num = this.released_gameobj_ids[0];
				this.released_gameobj_ids.RemoveAt(0);
				if (!this.gameobj_id_status[(int)num])
				{
					this.gameobj_id_status[(int)num] = true;
					this.net_gameobj_list.Remove(num);
					return num;
				}
			}
			ushort num2 = this.last_gameobj_id + 1;
			while (num2 != this.last_gameobj_id)
			{
				if (!this.gameobj_id_status[(int)num2])
				{
					this.last_gameobj_id = num2;
					this.gameobj_id_status[(int)num2] = true;
					this.net_gameobj_list.Remove(num2);
					return num2;
				}
				if (num2 >= 65534)
				{
					num2 = 0;
				}
				else
				{
					num2 += 1;
				}
			}
			return 0;
		}

		// Token: 0x060028AD RID: 10413 RVA: 0x0001C8BB File Offset: 0x0001AABB
		private void ReleaseGameObjectID(ushort id)
		{
			if (this.gameobj_id_status[(int)id])
			{
				this.released_gameobj_ids.Add(id);
				this.gameobj_id_status[(int)id] = false;
				return;
			}
			Debug.LogError("Tried to release gameobj id that is not in use!");
		}

		// Token: 0x060028AE RID: 10414 RVA: 0x000F4800 File Offset: 0x000F2A00
		public void ChangeOwner(NetPlayer curOwner, NetPlayer newOwner)
		{
			Debug.Log(this.binded_entity_map.Count);
			foreach (KeyValuePair<ushort, BindedNetEntity> keyValuePair in this.binded_entity_map)
			{
				if (keyValuePair.Value.net_component.Owner == curOwner)
				{
					keyValuePair.Value.net_component.Owner = newOwner;
					keyValuePair.Value.net_component.IsOwner = (NetSystem.MyPlayer == newOwner);
					keyValuePair.Value.net_component.OnOwnerChanged();
					Debug.Log("Changing Entity ");
				}
			}
		}

		// Token: 0x04002B5B RID: 11099
		public const int MAX_NETWORK_IDS = 65535;

		// Token: 0x04002B5C RID: 11100
		public const int MAX_GAMEOBJ_IDS = 65535;

		// Token: 0x04002B5D RID: 11101
		private Dictionary<ushort, NetTypeDefinition> net_type_definitions;

		// Token: 0x04002B5E RID: 11102
		private Dictionary<string, NetPrefabDefinition> net_prefab_map;

		// Token: 0x04002B5F RID: 11103
		private List<string> net_prefab_list;

		// Token: 0x04002B60 RID: 11104
		private Dictionary<Type, ushort> net_type_id_map;

		// Token: 0x04002B61 RID: 11105
		private List<BindedNetEntity> binded_entities;

		// Token: 0x04002B62 RID: 11106
		private Dictionary<ushort, BindedNetEntity> binded_entity_map;

		// Token: 0x04002B63 RID: 11107
		private Dictionary<ushort, NetGameObject> net_gameobj_list;

		// Token: 0x04002B64 RID: 11108
		private List<BindedNetEntity> tick_new_entities;

		// Token: 0x04002B65 RID: 11109
		private List<BindedNetEntity> tick_destroyed_entities;

		// Token: 0x04002B66 RID: 11110
		private List<BindedNetEntity> destroyed_entities;

		// Token: 0x04002B67 RID: 11111
		private bool[] network_id_status = new bool[65535];

		// Token: 0x04002B68 RID: 11112
		private List<ushort> released_network_ids = new List<ushort>();

		// Token: 0x04002B69 RID: 11113
		private ushort last_network_id;

		// Token: 0x04002B6A RID: 11114
		private bool[] gameobj_id_status = new bool[65535];

		// Token: 0x04002B6B RID: 11115
		private List<ushort> released_gameobj_ids = new List<ushort>();

		// Token: 0x04002B6C RID: 11116
		private ushort last_gameobj_id;

		// Token: 0x04002B6D RID: 11117
		private List<QueuedRPC> queued_rpc_list;

		// Token: 0x04002B6E RID: 11118
		private Dictionary<string, NetPrefab> proxy_prefabs;

		// Token: 0x04002B6F RID: 11119
		private Dictionary<string, NetPrefab> owner_prefabs;

		// Token: 0x04002B70 RID: 11120
		private Dictionary<string, NetPrefab> host_prefabs;

		// Token: 0x04002B71 RID: 11121
		private Dictionary<string, NetPrefab> dedicated_prefabs;

		// Token: 0x04002B72 RID: 11122
		private int last_snapshot_tick;

		// Token: 0x04002B73 RID: 11123
		private ZPBitStream bsw;

		// Token: 0x04002B74 RID: 11124
		private float next_netobj_sort = Time.time;
	}
}
