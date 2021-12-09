using System;
using System.Collections;
using Rewired.UI.ControlMapper;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Rewired.Demos
{
	// Token: 0x020006E1 RID: 1761
	[AddComponentMenu("")]
	public class ControlMapperDemoMessage : MonoBehaviour
	{
		// Token: 0x06003356 RID: 13142 RVA: 0x00022F02 File Offset: 0x00021102
		private void Awake()
		{
			if (this.controlMapper != null)
			{
				this.controlMapper.ScreenClosedEvent += this.OnControlMapperClosed;
				this.controlMapper.ScreenOpenedEvent += this.OnControlMapperOpened;
			}
		}

		// Token: 0x06003357 RID: 13143 RVA: 0x00022F40 File Offset: 0x00021140
		private void Start()
		{
			this.SelectDefault();
		}

		// Token: 0x06003358 RID: 13144 RVA: 0x00022F48 File Offset: 0x00021148
		private void OnControlMapperClosed()
		{
			base.gameObject.SetActive(true);
			base.StartCoroutine(this.SelectDefaultDeferred());
		}

		// Token: 0x06003359 RID: 13145 RVA: 0x00012118 File Offset: 0x00010318
		private void OnControlMapperOpened()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x0600335A RID: 13146 RVA: 0x00022F63 File Offset: 0x00021163
		private void SelectDefault()
		{
			if (EventSystem.current == null)
			{
				return;
			}
			if (this.defaultSelectable != null)
			{
				EventSystem.current.SetSelectedGameObject(this.defaultSelectable.gameObject);
			}
		}

		// Token: 0x0600335B RID: 13147 RVA: 0x00022F96 File Offset: 0x00021196
		private IEnumerator SelectDefaultDeferred()
		{
			yield return null;
			this.SelectDefault();
			yield break;
		}

		// Token: 0x0400319B RID: 12699
		public ControlMapper controlMapper;

		// Token: 0x0400319C RID: 12700
		public Selectable defaultSelectable;
	}
}
