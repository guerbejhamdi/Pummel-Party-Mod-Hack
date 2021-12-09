using System;
using UnityEngine;

namespace ZP.Net
{
	// Token: 0x0200060D RID: 1549
	public class NetPrefab
	{
		// Token: 0x06002882 RID: 10370 RVA: 0x0001C784 File Offset: 0x0001A984
		public NetPrefab(GameObject _prefab, ushort _prefab_id)
		{
			this.game_object = _prefab;
			this.prefab_id = _prefab_id;
		}

		// Token: 0x04002B2E RID: 11054
		public GameObject game_object;

		// Token: 0x04002B2F RID: 11055
		public ushort prefab_id;
	}
}
