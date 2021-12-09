using System;

namespace I2.Loc.SimpleJSON
{
	// Token: 0x0200085E RID: 2142
	internal class JSONLazyCreator : JSONNode
	{
		// Token: 0x06003CB9 RID: 15545 RVA: 0x000288EE File Offset: 0x00026AEE
		public JSONLazyCreator(JSONNode aNode)
		{
			this.m_Node = aNode;
			this.m_Key = null;
		}

		// Token: 0x06003CBA RID: 15546 RVA: 0x00028904 File Offset: 0x00026B04
		public JSONLazyCreator(JSONNode aNode, string aKey)
		{
			this.m_Node = aNode;
			this.m_Key = aKey;
		}

		// Token: 0x06003CBB RID: 15547 RVA: 0x0002891A File Offset: 0x00026B1A
		private void Set(JSONNode aVal)
		{
			if (this.m_Key == null)
			{
				this.m_Node.Add(aVal);
			}
			else
			{
				this.m_Node.Add(this.m_Key, aVal);
			}
			this.m_Node = null;
		}

		// Token: 0x17000A5B RID: 2651
		public override JSONNode this[int aIndex]
		{
			get
			{
				return new JSONLazyCreator(this);
			}
			set
			{
				this.Set(new JSONArray
				{
					value
				});
			}
		}

		// Token: 0x17000A5C RID: 2652
		public override JSONNode this[string aKey]
		{
			get
			{
				return new JSONLazyCreator(this, aKey);
			}
			set
			{
				this.Set(new JSONClass
				{
					{
						aKey,
						value
					}
				});
			}
		}

		// Token: 0x06003CC0 RID: 15552 RVA: 0x0012FEA8 File Offset: 0x0012E0A8
		public override void Add(JSONNode aItem)
		{
			this.Set(new JSONArray
			{
				aItem
			});
		}

		// Token: 0x06003CC1 RID: 15553 RVA: 0x0012FE84 File Offset: 0x0012E084
		public override void Add(string aKey, JSONNode aItem)
		{
			this.Set(new JSONClass
			{
				{
					aKey,
					aItem
				}
			});
		}

		// Token: 0x06003CC2 RID: 15554 RVA: 0x00028954 File Offset: 0x00026B54
		public static bool operator ==(JSONLazyCreator a, object b)
		{
			return b == null || a == b;
		}

		// Token: 0x06003CC3 RID: 15555 RVA: 0x0002895F File Offset: 0x00026B5F
		public static bool operator !=(JSONLazyCreator a, object b)
		{
			return !(a == b);
		}

		// Token: 0x06003CC4 RID: 15556 RVA: 0x00028954 File Offset: 0x00026B54
		public override bool Equals(object obj)
		{
			return obj == null || this == obj;
		}

		// Token: 0x06003CC5 RID: 15557 RVA: 0x0002896B File Offset: 0x00026B6B
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06003CC6 RID: 15558 RVA: 0x0001B956 File Offset: 0x00019B56
		public override string ToString()
		{
			return "";
		}

		// Token: 0x06003CC7 RID: 15559 RVA: 0x0001B956 File Offset: 0x00019B56
		public override string ToString(string aPrefix)
		{
			return "";
		}

		// Token: 0x17000A5D RID: 2653
		// (get) Token: 0x06003CC8 RID: 15560 RVA: 0x0012FECC File Offset: 0x0012E0CC
		// (set) Token: 0x06003CC9 RID: 15561 RVA: 0x0012FEE8 File Offset: 0x0012E0E8
		public override int AsInt
		{
			get
			{
				JSONData aVal = new JSONData(0);
				this.Set(aVal);
				return 0;
			}
			set
			{
				JSONData aVal = new JSONData(value);
				this.Set(aVal);
			}
		}

		// Token: 0x17000A5E RID: 2654
		// (get) Token: 0x06003CCA RID: 15562 RVA: 0x0012FF04 File Offset: 0x0012E104
		// (set) Token: 0x06003CCB RID: 15563 RVA: 0x0012FF28 File Offset: 0x0012E128
		public override float AsFloat
		{
			get
			{
				JSONData aVal = new JSONData(0f);
				this.Set(aVal);
				return 0f;
			}
			set
			{
				JSONData aVal = new JSONData(value);
				this.Set(aVal);
			}
		}

		// Token: 0x17000A5F RID: 2655
		// (get) Token: 0x06003CCC RID: 15564 RVA: 0x0012FF44 File Offset: 0x0012E144
		// (set) Token: 0x06003CCD RID: 15565 RVA: 0x0012FF70 File Offset: 0x0012E170
		public override double AsDouble
		{
			get
			{
				JSONData aVal = new JSONData(0.0);
				this.Set(aVal);
				return 0.0;
			}
			set
			{
				JSONData aVal = new JSONData(value);
				this.Set(aVal);
			}
		}

		// Token: 0x17000A60 RID: 2656
		// (get) Token: 0x06003CCE RID: 15566 RVA: 0x0012FF8C File Offset: 0x0012E18C
		// (set) Token: 0x06003CCF RID: 15567 RVA: 0x0012FFA8 File Offset: 0x0012E1A8
		public override bool AsBool
		{
			get
			{
				JSONData aVal = new JSONData(false);
				this.Set(aVal);
				return false;
			}
			set
			{
				JSONData aVal = new JSONData(value);
				this.Set(aVal);
			}
		}

		// Token: 0x17000A61 RID: 2657
		// (get) Token: 0x06003CD0 RID: 15568 RVA: 0x0012FFC4 File Offset: 0x0012E1C4
		public override JSONArray AsArray
		{
			get
			{
				JSONArray jsonarray = new JSONArray();
				this.Set(jsonarray);
				return jsonarray;
			}
		}

		// Token: 0x17000A62 RID: 2658
		// (get) Token: 0x06003CD1 RID: 15569 RVA: 0x0012FFE0 File Offset: 0x0012E1E0
		public override JSONClass AsObject
		{
			get
			{
				JSONClass jsonclass = new JSONClass();
				this.Set(jsonclass);
				return jsonclass;
			}
		}

		// Token: 0x040039C0 RID: 14784
		private JSONNode m_Node;

		// Token: 0x040039C1 RID: 14785
		private string m_Key;
	}
}
