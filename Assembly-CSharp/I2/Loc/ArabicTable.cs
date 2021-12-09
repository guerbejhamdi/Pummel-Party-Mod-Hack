using System;
using System.Collections.Generic;

namespace I2.Loc
{
	// Token: 0x02000848 RID: 2120
	internal class ArabicTable
	{
		// Token: 0x06003C00 RID: 15360 RVA: 0x0012D220 File Offset: 0x0012B420
		private ArabicTable()
		{
			ArabicTable.mapList = new List<ArabicMapping>();
			ArabicTable.mapList.Add(new ArabicMapping(1569, 65152));
			ArabicTable.mapList.Add(new ArabicMapping(1575, 65165));
			ArabicTable.mapList.Add(new ArabicMapping(1571, 65155));
			ArabicTable.mapList.Add(new ArabicMapping(1572, 65157));
			ArabicTable.mapList.Add(new ArabicMapping(1573, 65159));
			ArabicTable.mapList.Add(new ArabicMapping(1609, 64508));
			ArabicTable.mapList.Add(new ArabicMapping(1574, 65161));
			ArabicTable.mapList.Add(new ArabicMapping(1576, 65167));
			ArabicTable.mapList.Add(new ArabicMapping(1578, 65173));
			ArabicTable.mapList.Add(new ArabicMapping(1579, 65177));
			ArabicTable.mapList.Add(new ArabicMapping(1580, 65181));
			ArabicTable.mapList.Add(new ArabicMapping(1581, 65185));
			ArabicTable.mapList.Add(new ArabicMapping(1582, 65189));
			ArabicTable.mapList.Add(new ArabicMapping(1583, 65193));
			ArabicTable.mapList.Add(new ArabicMapping(1584, 65195));
			ArabicTable.mapList.Add(new ArabicMapping(1585, 65197));
			ArabicTable.mapList.Add(new ArabicMapping(1586, 65199));
			ArabicTable.mapList.Add(new ArabicMapping(1587, 65201));
			ArabicTable.mapList.Add(new ArabicMapping(1588, 65205));
			ArabicTable.mapList.Add(new ArabicMapping(1589, 65209));
			ArabicTable.mapList.Add(new ArabicMapping(1590, 65213));
			ArabicTable.mapList.Add(new ArabicMapping(1591, 65217));
			ArabicTable.mapList.Add(new ArabicMapping(1592, 65221));
			ArabicTable.mapList.Add(new ArabicMapping(1593, 65225));
			ArabicTable.mapList.Add(new ArabicMapping(1594, 65229));
			ArabicTable.mapList.Add(new ArabicMapping(1601, 65233));
			ArabicTable.mapList.Add(new ArabicMapping(1602, 65237));
			ArabicTable.mapList.Add(new ArabicMapping(1603, 65241));
			ArabicTable.mapList.Add(new ArabicMapping(1604, 65245));
			ArabicTable.mapList.Add(new ArabicMapping(1605, 65249));
			ArabicTable.mapList.Add(new ArabicMapping(1606, 65253));
			ArabicTable.mapList.Add(new ArabicMapping(1607, 65257));
			ArabicTable.mapList.Add(new ArabicMapping(1608, 65261));
			ArabicTable.mapList.Add(new ArabicMapping(1610, 65265));
			ArabicTable.mapList.Add(new ArabicMapping(1570, 65153));
			ArabicTable.mapList.Add(new ArabicMapping(1577, 65171));
			ArabicTable.mapList.Add(new ArabicMapping(1662, 64342));
			ArabicTable.mapList.Add(new ArabicMapping(1670, 64378));
			ArabicTable.mapList.Add(new ArabicMapping(1688, 64394));
			ArabicTable.mapList.Add(new ArabicMapping(1711, 64402));
			ArabicTable.mapList.Add(new ArabicMapping(1705, 64398));
		}

		// Token: 0x17000A38 RID: 2616
		// (get) Token: 0x06003C01 RID: 15361 RVA: 0x00028349 File Offset: 0x00026549
		internal static ArabicTable ArabicMapper
		{
			get
			{
				if (ArabicTable.arabicMapper == null)
				{
					ArabicTable.arabicMapper = new ArabicTable();
				}
				return ArabicTable.arabicMapper;
			}
		}

		// Token: 0x06003C02 RID: 15362 RVA: 0x0012D640 File Offset: 0x0012B840
		internal int Convert(int toBeConverted)
		{
			foreach (ArabicMapping arabicMapping in ArabicTable.mapList)
			{
				if (arabicMapping.from == toBeConverted)
				{
					return arabicMapping.to;
				}
			}
			return toBeConverted;
		}

		// Token: 0x0400398D RID: 14733
		private static List<ArabicMapping> mapList;

		// Token: 0x0400398E RID: 14734
		private static ArabicTable arabicMapper;
	}
}
