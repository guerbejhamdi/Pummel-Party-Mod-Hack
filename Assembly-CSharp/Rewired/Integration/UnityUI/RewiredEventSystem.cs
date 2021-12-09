using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Rewired.Integration.UnityUI
{
	// Token: 0x020006AE RID: 1710
	[AddComponentMenu("Rewired/Rewired Event System")]
	public class RewiredEventSystem : EventSystem
	{
		// Token: 0x170008BC RID: 2236
		// (get) Token: 0x06003183 RID: 12675 RVA: 0x000219AD File Offset: 0x0001FBAD
		// (set) Token: 0x06003184 RID: 12676 RVA: 0x000219B5 File Offset: 0x0001FBB5
		public bool alwaysUpdate
		{
			get
			{
				return this._alwaysUpdate;
			}
			set
			{
				this._alwaysUpdate = value;
			}
		}

		// Token: 0x06003185 RID: 12677 RVA: 0x001034E8 File Offset: 0x001016E8
		protected override void Update()
		{
			if (this.alwaysUpdate)
			{
				EventSystem current = EventSystem.current;
				if (current != this)
				{
					EventSystem.current = this;
				}
				try
				{
					base.Update();
					return;
				}
				finally
				{
					if (current != this)
					{
						EventSystem.current = current;
					}
				}
			}
			base.Update();
		}

		// Token: 0x04003068 RID: 12392
		[Tooltip("If enabled, the Event System will be updated every frame even if other Event Systems are enabled. Otherwise, only EventSystem.current will be updated.")]
		[SerializeField]
		private bool _alwaysUpdate;
	}
}
