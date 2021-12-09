using System;
using System.Collections.Generic;
using UnityEngine;

namespace MalbersAnimations.Utilities
{
	// Token: 0x0200078A RID: 1930
	public class ActiveMeshes : MonoBehaviour
	{
		// Token: 0x170009BC RID: 2492
		// (get) Token: 0x06003707 RID: 14087 RVA: 0x00117F44 File Offset: 0x00116144
		// (set) Token: 0x06003706 RID: 14086 RVA: 0x00117EE4 File Offset: 0x001160E4
		public string AllIndex
		{
			get
			{
				string text = "";
				for (int i = 0; i < this.Meshes.Count; i++)
				{
					text = text + this.Meshes[i].Current.ToString() + " ";
				}
				text.Remove(text.Length - 1);
				return text;
			}
			set
			{
				string[] array = value.Split(new char[]
				{
					' '
				});
				for (int i = 0; i < this.Meshes.Count; i++)
				{
					int num;
					if (array.Length > i && int.TryParse(array[i], out num) && num != -1)
					{
						this.Meshes[i].ChangeMesh(num);
					}
				}
			}
		}

		// Token: 0x06003708 RID: 14088 RVA: 0x00117FA0 File Offset: 0x001161A0
		private void Awake()
		{
			if (this.random)
			{
				foreach (ActiveSMesh activeSMesh in this.Meshes)
				{
					activeSMesh.ChangeMesh(UnityEngine.Random.Range(0, activeSMesh.meshes.Length));
				}
			}
		}

		// Token: 0x06003709 RID: 14089 RVA: 0x00118008 File Offset: 0x00116208
		public void SetActiveMeshesIndex(int[] MeshesIndex)
		{
			if (MeshesIndex.Length != this.Meshes.Count)
			{
				Debug.LogError("Meshes Index array Lenghts don't match");
				return;
			}
			for (int i = 0; i < MeshesIndex.Length; i++)
			{
				this.Meshes[i].ChangeMesh(MeshesIndex[i]);
			}
		}

		// Token: 0x0600370A RID: 14090 RVA: 0x0002573C File Offset: 0x0002393C
		public virtual void ChangeMesh(int index)
		{
			if (this.Meshes.Count > index)
			{
				this.Meshes[index].ChangeMesh(true);
			}
		}

		// Token: 0x0600370B RID: 14091 RVA: 0x0002575E File Offset: 0x0002395E
		public virtual void ChangeMesh(int indexList, int IndexMesh)
		{
			if (indexList < 0)
			{
				indexList = 0;
			}
			indexList %= this.Meshes.Count;
			if (this.Meshes[indexList] != null)
			{
				this.Meshes[indexList].ChangeMesh(IndexMesh);
			}
		}

		// Token: 0x0600370C RID: 14092 RVA: 0x00118054 File Offset: 0x00116254
		public virtual void ChangeMesh(string name, bool next)
		{
			ActiveSMesh activeSMesh = this.Meshes.Find((ActiveSMesh item) => item.Name == name);
			if (activeSMesh != null)
			{
				activeSMesh.ChangeMesh(next);
			}
		}

		// Token: 0x0600370D RID: 14093 RVA: 0x00025796 File Offset: 0x00023996
		public virtual void ChangeMesh(string name)
		{
			this.ChangeMesh(name, true);
		}

		// Token: 0x0600370E RID: 14094 RVA: 0x00118090 File Offset: 0x00116290
		public virtual void ChangeMesh(string name, int CurrentIndex)
		{
			ActiveSMesh activeSMesh = this.Meshes.Find((ActiveSMesh item) => item.Name == name);
			if (activeSMesh != null)
			{
				activeSMesh.ChangeMesh(CurrentIndex);
			}
		}

		// Token: 0x0600370F RID: 14095 RVA: 0x000257A0 File Offset: 0x000239A0
		public virtual void ChangeMesh(int index, bool next)
		{
			this.Meshes[index].ChangeMesh(next);
		}

		// Token: 0x06003710 RID: 14096 RVA: 0x001180CC File Offset: 0x001162CC
		public virtual void ChangeMesh(bool next = true)
		{
			foreach (ActiveSMesh activeSMesh in this.Meshes)
			{
				activeSMesh.ChangeMesh(next);
			}
		}

		// Token: 0x06003711 RID: 14097 RVA: 0x00118120 File Offset: 0x00116320
		public virtual ActiveSMesh GetActiveMesh(string name)
		{
			if (this.Meshes.Count == 0)
			{
				return null;
			}
			return this.Meshes.Find((ActiveSMesh item) => item.Name == name);
		}

		// Token: 0x06003712 RID: 14098 RVA: 0x00118160 File Offset: 0x00116360
		public virtual ActiveSMesh GetActiveMesh(int index)
		{
			if (this.Meshes.Count == 0)
			{
				return null;
			}
			if (index >= this.Meshes.Count)
			{
				index = 0;
			}
			if (index < 0)
			{
				index = this.Meshes.Count - 1;
			}
			return this.Meshes[index];
		}

		// Token: 0x04003636 RID: 13878
		[SerializeField]
		public List<ActiveSMesh> Meshes = new List<ActiveSMesh>();

		// Token: 0x04003637 RID: 13879
		[HideInInspector]
		[SerializeField]
		public bool showMeshesList = true;

		// Token: 0x04003638 RID: 13880
		public bool random;
	}
}
