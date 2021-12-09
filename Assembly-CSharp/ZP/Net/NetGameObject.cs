using System;
using UnityEngine;

namespace ZP.Net
{
	// Token: 0x02000611 RID: 1553
	public class NetGameObject
	{
		// Token: 0x06002887 RID: 10375 RVA: 0x0001C81A File Offset: 0x0001AA1A
		public NetGameObject(GameObject _game_obj, bool _initial_pos, Vector3 _position, bool _initial_rotation, Vector3 _rotation)
		{
			this.game_obj = _game_obj;
			this.initial_pos = _initial_pos;
			this.position = _position;
			this.initial_rotation = _initial_rotation;
			this.rotation = _rotation;
		}

		// Token: 0x04002B4B RID: 11083
		public GameObject game_obj;

		// Token: 0x04002B4C RID: 11084
		public bool initial_pos;

		// Token: 0x04002B4D RID: 11085
		public Vector3 position;

		// Token: 0x04002B4E RID: 11086
		public bool initial_rotation;

		// Token: 0x04002B4F RID: 11087
		public Vector3 rotation;
	}
}
