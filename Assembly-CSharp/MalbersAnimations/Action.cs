using System;
using UnityEngine;

namespace MalbersAnimations
{
	// Token: 0x020006E8 RID: 1768
	[CreateAssetMenu(menuName = "Malbers Animations/Actions")]
	public class Action : ScriptableObject
	{
		// Token: 0x06003385 RID: 13189 RVA: 0x000231FD File Offset: 0x000213FD
		public static implicit operator int(Action reference)
		{
			return reference.ID;
		}

		// Token: 0x040031D7 RID: 12759
		[Tooltip("Value for the transitions IDAction on the Animator in order to Execute the desirable animation clip")]
		public int ID;
	}
}
