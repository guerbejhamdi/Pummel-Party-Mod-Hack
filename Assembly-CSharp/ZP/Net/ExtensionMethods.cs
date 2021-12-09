using System;
using System.ComponentModel;
using System.Reflection;

namespace ZP.Net
{
	// Token: 0x020005F3 RID: 1523
	public static class ExtensionMethods
	{
		// Token: 0x06002773 RID: 10099 RVA: 0x000ECAB0 File Offset: 0x000EACB0
		public static string GetDescription(this Enum value)
		{
			Type type = value.GetType();
			string name = Enum.GetName(type, value);
			if (name != null)
			{
				FieldInfo field = type.GetField(name);
				if (field != null)
				{
					DescriptionAttribute descriptionAttribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
					if (descriptionAttribute != null)
					{
						return descriptionAttribute.Description;
					}
				}
			}
			return null;
		}
	}
}
