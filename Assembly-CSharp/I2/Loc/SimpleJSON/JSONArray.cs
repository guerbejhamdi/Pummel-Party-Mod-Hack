using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace I2.Loc.SimpleJSON
{
	// Token: 0x02000856 RID: 2134
	public class JSONArray : JSONNode, IEnumerable
	{
		// Token: 0x17000A4A RID: 2634
		public override JSONNode this[int aIndex]
		{
			get
			{
				if (aIndex < 0 || aIndex >= this.m_List.Count)
				{
					return new JSONLazyCreator(this);
				}
				return this.m_List[aIndex];
			}
			set
			{
				if (aIndex < 0 || aIndex >= this.m_List.Count)
				{
					this.m_List.Add(value);
					return;
				}
				this.m_List[aIndex] = value;
			}
		}

		// Token: 0x17000A4B RID: 2635
		public override JSONNode this[string aKey]
		{
			get
			{
				return new JSONLazyCreator(this);
			}
			set
			{
				this.m_List.Add(value);
			}
		}

		// Token: 0x17000A4C RID: 2636
		// (get) Token: 0x06003C74 RID: 15476 RVA: 0x0002863D File Offset: 0x0002683D
		public override int Count
		{
			get
			{
				return this.m_List.Count;
			}
		}

		// Token: 0x06003C75 RID: 15477 RVA: 0x0002862F File Offset: 0x0002682F
		public override void Add(string aKey, JSONNode aItem)
		{
			this.m_List.Add(aItem);
		}

		// Token: 0x06003C76 RID: 15478 RVA: 0x0002864A File Offset: 0x0002684A
		public override JSONNode Remove(int aIndex)
		{
			if (aIndex < 0 || aIndex >= this.m_List.Count)
			{
				return null;
			}
			JSONNode result = this.m_List[aIndex];
			this.m_List.RemoveAt(aIndex);
			return result;
		}

		// Token: 0x06003C77 RID: 15479 RVA: 0x00028678 File Offset: 0x00026878
		public override JSONNode Remove(JSONNode aNode)
		{
			this.m_List.Remove(aNode);
			return aNode;
		}

		// Token: 0x17000A4D RID: 2637
		// (get) Token: 0x06003C78 RID: 15480 RVA: 0x00028688 File Offset: 0x00026888
		public override IEnumerable<JSONNode> Childs
		{
			get
			{
				foreach (JSONNode jsonnode in this.m_List)
				{
					yield return jsonnode;
				}
				List<JSONNode>.Enumerator enumerator = default(List<JSONNode>.Enumerator);
				yield break;
				yield break;
			}
		}

		// Token: 0x06003C79 RID: 15481 RVA: 0x00028698 File Offset: 0x00026898
		public IEnumerator GetEnumerator()
		{
			foreach (JSONNode jsonnode in this.m_List)
			{
				yield return jsonnode;
			}
			List<JSONNode>.Enumerator enumerator = default(List<JSONNode>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06003C7A RID: 15482 RVA: 0x0012F430 File Offset: 0x0012D630
		public override string ToString()
		{
			string text = "[ ";
			foreach (JSONNode jsonnode in this.m_List)
			{
				if (text.Length > 2)
				{
					text += ", ";
				}
				text += jsonnode.ToString();
			}
			text += " ]";
			return text;
		}

		// Token: 0x06003C7B RID: 15483 RVA: 0x0012F4B4 File Offset: 0x0012D6B4
		public override string ToString(string aPrefix)
		{
			string text = "[ ";
			foreach (JSONNode jsonnode in this.m_List)
			{
				if (text.Length > 3)
				{
					text += ", ";
				}
				text = text + "\n" + aPrefix + "   ";
				text += jsonnode.ToString(aPrefix + "   ");
			}
			text = text + "\n" + aPrefix + "]";
			return text;
		}

		// Token: 0x06003C7C RID: 15484 RVA: 0x0012F558 File Offset: 0x0012D758
		public override void Serialize(BinaryWriter aWriter)
		{
			aWriter.Write(1);
			aWriter.Write(this.m_List.Count);
			for (int i = 0; i < this.m_List.Count; i++)
			{
				this.m_List[i].Serialize(aWriter);
			}
		}

		// Token: 0x040039AA RID: 14762
		private List<JSONNode> m_List = new List<JSONNode>();
	}
}
