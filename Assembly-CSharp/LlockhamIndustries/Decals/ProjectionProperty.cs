using System;
using UnityEngine;

namespace LlockhamIndustries.Decals
{
	// Token: 0x020008A5 RID: 2213
	[Serializable]
	public struct ProjectionProperty
	{
		// Token: 0x06003ED1 RID: 16081 RVA: 0x0002A5C2 File Offset: 0x000287C2
		public ProjectionProperty(string Name, int ID, Color Color)
		{
			this.name = Name;
			this.nameID = ID;
			this.type = PropertyType.Color;
			this.color = Color;
			this.value = 0f;
			this.enabled = false;
		}

		// Token: 0x06003ED2 RID: 16082 RVA: 0x0002A5F2 File Offset: 0x000287F2
		public ProjectionProperty(string Name, int ID, float Value)
		{
			this.name = Name;
			this.nameID = ID;
			this.type = PropertyType.Float;
			this.color = Color.white;
			this.value = Value;
			this.enabled = false;
		}

		// Token: 0x06003ED3 RID: 16083 RVA: 0x0002A622 File Offset: 0x00028822
		public ProjectionProperty(string Name, int ID, Color Color, float Value)
		{
			this.name = Name;
			this.nameID = ID;
			this.type = PropertyType.Combo;
			this.color = Color;
			this.value = Value;
			this.enabled = false;
		}

		// Token: 0x04003B00 RID: 15104
		public string name;

		// Token: 0x04003B01 RID: 15105
		public int nameID;

		// Token: 0x04003B02 RID: 15106
		public PropertyType type;

		// Token: 0x04003B03 RID: 15107
		public Color color;

		// Token: 0x04003B04 RID: 15108
		public float value;

		// Token: 0x04003B05 RID: 15109
		public bool enabled;
	}
}
