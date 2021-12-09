using System;
using System.IO;

namespace I2.Loc.SimpleJSON
{
	// Token: 0x0200085D RID: 2141
	public class JSONData : JSONNode
	{
		// Token: 0x17000A5A RID: 2650
		// (get) Token: 0x06003CAF RID: 15535 RVA: 0x00028876 File Offset: 0x00026A76
		// (set) Token: 0x06003CB0 RID: 15536 RVA: 0x0002887E File Offset: 0x00026A7E
		public override string Value
		{
			get
			{
				return this.m_Data;
			}
			set
			{
				this.m_Data = value;
			}
		}

		// Token: 0x06003CB1 RID: 15537 RVA: 0x00028887 File Offset: 0x00026A87
		public JSONData(string aData)
		{
			this.m_Data = aData;
		}

		// Token: 0x06003CB2 RID: 15538 RVA: 0x00028896 File Offset: 0x00026A96
		public JSONData(float aData)
		{
			this.AsFloat = aData;
		}

		// Token: 0x06003CB3 RID: 15539 RVA: 0x000288A5 File Offset: 0x00026AA5
		public JSONData(double aData)
		{
			this.AsDouble = aData;
		}

		// Token: 0x06003CB4 RID: 15540 RVA: 0x000288B4 File Offset: 0x00026AB4
		public JSONData(bool aData)
		{
			this.AsBool = aData;
		}

		// Token: 0x06003CB5 RID: 15541 RVA: 0x000288C3 File Offset: 0x00026AC3
		public JSONData(int aData)
		{
			this.AsInt = aData;
		}

		// Token: 0x06003CB6 RID: 15542 RVA: 0x000288D2 File Offset: 0x00026AD2
		public override string ToString()
		{
			return "\"" + JSONNode.Escape(this.m_Data) + "\"";
		}

		// Token: 0x06003CB7 RID: 15543 RVA: 0x000288D2 File Offset: 0x00026AD2
		public override string ToString(string aPrefix)
		{
			return "\"" + JSONNode.Escape(this.m_Data) + "\"";
		}

		// Token: 0x06003CB8 RID: 15544 RVA: 0x0012FD68 File Offset: 0x0012DF68
		public override void Serialize(BinaryWriter aWriter)
		{
			JSONData jsondata = new JSONData("");
			jsondata.AsInt = this.AsInt;
			if (jsondata.m_Data == this.m_Data)
			{
				aWriter.Write(4);
				aWriter.Write(this.AsInt);
				return;
			}
			jsondata.AsFloat = this.AsFloat;
			if (jsondata.m_Data == this.m_Data)
			{
				aWriter.Write(7);
				aWriter.Write(this.AsFloat);
				return;
			}
			jsondata.AsDouble = this.AsDouble;
			if (jsondata.m_Data == this.m_Data)
			{
				aWriter.Write(5);
				aWriter.Write(this.AsDouble);
				return;
			}
			jsondata.AsBool = this.AsBool;
			if (jsondata.m_Data == this.m_Data)
			{
				aWriter.Write(6);
				aWriter.Write(this.AsBool);
				return;
			}
			aWriter.Write(3);
			aWriter.Write(this.m_Data);
		}

		// Token: 0x040039BF RID: 14783
		private string m_Data;
	}
}
