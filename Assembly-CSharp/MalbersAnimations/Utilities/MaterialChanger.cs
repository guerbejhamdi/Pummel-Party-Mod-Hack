using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MalbersAnimations.Utilities
{
	// Token: 0x020007A5 RID: 1957
	public class MaterialChanger : MonoBehaviour
	{
		// Token: 0x170009D3 RID: 2515
		// (get) Token: 0x060037B4 RID: 14260 RVA: 0x00119EA0 File Offset: 0x001180A0
		// (set) Token: 0x060037B3 RID: 14259 RVA: 0x00119E40 File Offset: 0x00118040
		public string AllIndex
		{
			get
			{
				string text = "";
				for (int i = 0; i < this.materialList.Count; i++)
				{
					text = text + this.materialList[i].current.ToString() + " ";
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
				for (int i = 0; i < this.materialList.Count; i++)
				{
					int num;
					if (array.Length > i && int.TryParse(array[i], out num) && num != -1)
					{
						this.materialList[i].ChangeMaterial(num);
					}
				}
			}
		}

		// Token: 0x060037B5 RID: 14261 RVA: 0x00119EFC File Offset: 0x001180FC
		private void Awake()
		{
			foreach (MaterialItem materialItem in this.materialList)
			{
				if (materialItem.Linked && materialItem.Master >= 0 && materialItem.Master < this.materialList.Count)
				{
					this.materialList[materialItem.Master].OnMaterialChanged.AddListener(new UnityAction<int>(materialItem.ChangeMaterial));
				}
			}
			if (this.random)
			{
				this.Randomize();
			}
		}

		// Token: 0x060037B6 RID: 14262 RVA: 0x00119FA4 File Offset: 0x001181A4
		public virtual void Randomize()
		{
			foreach (MaterialItem materialItem in this.materialList)
			{
				materialItem.ChangeMaterial(UnityEngine.Random.Range(0, materialItem.materials.Length));
			}
		}

		// Token: 0x060037B7 RID: 14263 RVA: 0x0011A004 File Offset: 0x00118204
		public virtual void SetAllMaterials(bool Next = true)
		{
			foreach (MaterialItem materialItem in this.materialList)
			{
				materialItem.ChangeMaterial(Next);
			}
		}

		// Token: 0x060037B8 RID: 14264 RVA: 0x0011A058 File Offset: 0x00118258
		public virtual void SetAllMaterials(int index)
		{
			foreach (MaterialItem materialItem in this.materialList)
			{
				materialItem.ChangeMaterial(index);
			}
		}

		// Token: 0x060037B9 RID: 14265 RVA: 0x00025E47 File Offset: 0x00024047
		public virtual void SetMaterial(int indexList, int indexCurrent)
		{
			if (indexList < 0)
			{
				indexList = 0;
			}
			indexList %= this.materialList.Count;
			if (this.materialList[indexList] != null)
			{
				this.materialList[indexList].ChangeMaterial(indexCurrent);
			}
		}

		// Token: 0x060037BA RID: 14266 RVA: 0x00025E7F File Offset: 0x0002407F
		public virtual void SetMaterial(int index, bool next = true)
		{
			if (index < 0)
			{
				index = 0;
			}
			index %= this.materialList.Count;
			if (this.materialList[index] != null)
			{
				this.materialList[index].ChangeMaterial(next);
			}
		}

		// Token: 0x060037BB RID: 14267 RVA: 0x0011A0AC File Offset: 0x001182AC
		public virtual void SetMaterial(string name, int Index)
		{
			MaterialItem materialItem = this.materialList.Find((MaterialItem item) => item.Name == name);
			if (materialItem != null)
			{
				materialItem.ChangeMaterial(Index);
				return;
			}
			Debug.LogWarning("No material Item Found with the name: " + name);
		}

		// Token: 0x060037BC RID: 14268 RVA: 0x0011A100 File Offset: 0x00118300
		public virtual void SetMaterial(string name, bool next = true)
		{
			MaterialItem materialItem = this.materialList.Find((MaterialItem item) => item.Name == name);
			if (materialItem != null)
			{
				materialItem.ChangeMaterial(next);
				return;
			}
			Debug.LogWarning("No material Item Found with the name: " + name);
		}

		// Token: 0x060037BD RID: 14269 RVA: 0x0011A154 File Offset: 0x00118354
		public virtual void SetAllMaterials(Material mat)
		{
			foreach (MaterialItem materialItem in this.materialList)
			{
				materialItem.ChangeMaterial(mat);
			}
		}

		// Token: 0x060037BE RID: 14270 RVA: 0x00025EB7 File Offset: 0x000240B7
		public virtual void NextMaterialItem(int index)
		{
			if (index < 0)
			{
				index = 0;
			}
			index %= this.materialList.Count;
			this.materialList[index].NextMaterial();
		}

		// Token: 0x060037BF RID: 14271 RVA: 0x0011A1A8 File Offset: 0x001183A8
		public virtual void NextMaterialItem(string name)
		{
			MaterialItem materialItem = this.materialList.Find((MaterialItem item) => item.Name.ToUpper() == name.ToUpper());
			if (materialItem != null)
			{
				materialItem.NextMaterial();
			}
		}

		// Token: 0x060037C0 RID: 14272 RVA: 0x00025EE0 File Offset: 0x000240E0
		public virtual int CurrentMaterialIndex(int index)
		{
			return this.materialList[index].current;
		}

		// Token: 0x060037C1 RID: 14273 RVA: 0x0011A1E4 File Offset: 0x001183E4
		public virtual int CurrentMaterialIndex(string name)
		{
			int index = this.materialList.FindIndex((MaterialItem item) => item.Name == name);
			return this.materialList[index].current;
		}

		// Token: 0x040036AC RID: 13996
		[SerializeField]
		public List<MaterialItem> materialList = new List<MaterialItem>();

		// Token: 0x040036AD RID: 13997
		[HideInInspector]
		[SerializeField]
		public bool showMeshesList = true;

		// Token: 0x040036AE RID: 13998
		public bool random;

		// Token: 0x040036AF RID: 13999
		private MaterialItem Active;
	}
}
