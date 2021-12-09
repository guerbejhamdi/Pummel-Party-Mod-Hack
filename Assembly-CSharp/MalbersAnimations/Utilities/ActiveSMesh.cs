using System;
using MalbersAnimations.Events;
using UnityEngine;

namespace MalbersAnimations.Utilities
{
	// Token: 0x0200078E RID: 1934
	[Serializable]
	public class ActiveSMesh
	{
		// Token: 0x0600371A RID: 14106 RVA: 0x001181AC File Offset: 0x001163AC
		public virtual void ChangeMesh(bool next = true)
		{
			if (next)
			{
				this.Current++;
			}
			else
			{
				this.Current--;
			}
			if (this.Current >= this.meshes.Length)
			{
				this.Current = 0;
			}
			if (this.Current < 0)
			{
				this.Current = this.meshes.Length - 1;
			}
			foreach (Transform transform in this.meshes)
			{
				if (transform)
				{
					transform.gameObject.SetActive(false);
				}
			}
			if (this.meshes[this.Current])
			{
				this.meshes[this.Current].gameObject.SetActive(true);
				this.OnActiveMesh.Invoke(this.meshes[this.Current]);
			}
		}

		// Token: 0x0600371B RID: 14107 RVA: 0x00025807 File Offset: 0x00023A07
		public virtual Transform GetCurrentActiveMesh()
		{
			return this.meshes[this.Current];
		}

		// Token: 0x0600371C RID: 14108 RVA: 0x00025816 File Offset: 0x00023A16
		public virtual void ChangeMesh(int Index)
		{
			this.Current = Index - 1;
			this.ChangeMesh(true);
		}

		// Token: 0x0600371D RID: 14109 RVA: 0x0011827C File Offset: 0x0011647C
		public void Set_by_BinaryIndex(int binaryCurrent)
		{
			int index = 0;
			for (int i = 0; i < this.meshes.Length; i++)
			{
				if (MalbersTools.IsBitActive(binaryCurrent, i))
				{
					index = i;
					break;
				}
			}
			this.ChangeMesh(index);
		}

		// Token: 0x0400363C RID: 13884
		[HideInInspector]
		public string Name = "NameHere";

		// Token: 0x0400363D RID: 13885
		public Transform[] meshes;

		// Token: 0x0400363E RID: 13886
		[HideInInspector]
		[SerializeField]
		public int Current;

		// Token: 0x0400363F RID: 13887
		[Space]
		[Header("Invoked when the Active mesh is changed")]
		public TransformEvent OnActiveMesh = new TransformEvent();
	}
}
