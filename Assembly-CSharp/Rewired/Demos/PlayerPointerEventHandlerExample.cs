using System;
using System.Collections.Generic;
using System.Text;
using Rewired.Integration.UnityUI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Rewired.Demos
{
	// Token: 0x020006D0 RID: 1744
	[AddComponentMenu("")]
	public sealed class PlayerPointerEventHandlerExample : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler, IPointerClickHandler, IScrollHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
	{
		// Token: 0x060032F6 RID: 13046 RVA: 0x00022A86 File Offset: 0x00020C86
		private void Log(string o)
		{
			this.log.Add(o);
			if (this.log.Count > 10)
			{
				this.log.RemoveAt(0);
			}
		}

		// Token: 0x060032F7 RID: 13047 RVA: 0x001090BC File Offset: 0x001072BC
		private void Update()
		{
			if (this.text != null)
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (string value in this.log)
				{
					stringBuilder.AppendLine(value);
				}
				this.text.text = stringBuilder.ToString();
			}
		}

		// Token: 0x060032F8 RID: 13048 RVA: 0x00109138 File Offset: 0x00107338
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (eventData is PlayerPointerEventData)
			{
				PlayerPointerEventData playerPointerEventData = (PlayerPointerEventData)eventData;
				this.Log(string.Concat(new string[]
				{
					"OnPointerEnter:  Player = ",
					playerPointerEventData.playerId.ToString(),
					", Pointer Index = ",
					playerPointerEventData.inputSourceIndex.ToString(),
					", Source = ",
					PlayerPointerEventHandlerExample.GetSourceName(playerPointerEventData)
				}));
			}
		}

		// Token: 0x060032F9 RID: 13049 RVA: 0x001091A8 File Offset: 0x001073A8
		public void OnPointerExit(PointerEventData eventData)
		{
			if (eventData is PlayerPointerEventData)
			{
				PlayerPointerEventData playerPointerEventData = (PlayerPointerEventData)eventData;
				this.Log(string.Concat(new string[]
				{
					"OnPointerExit:  Player = ",
					playerPointerEventData.playerId.ToString(),
					", Pointer Index = ",
					playerPointerEventData.inputSourceIndex.ToString(),
					", Source = ",
					PlayerPointerEventHandlerExample.GetSourceName(playerPointerEventData)
				}));
			}
		}

		// Token: 0x060032FA RID: 13050 RVA: 0x00109218 File Offset: 0x00107418
		public void OnPointerUp(PointerEventData eventData)
		{
			if (eventData is PlayerPointerEventData)
			{
				PlayerPointerEventData playerPointerEventData = (PlayerPointerEventData)eventData;
				this.Log(string.Concat(new string[]
				{
					"OnPointerUp:  Player = ",
					playerPointerEventData.playerId.ToString(),
					", Pointer Index = ",
					playerPointerEventData.inputSourceIndex.ToString(),
					", Source = ",
					PlayerPointerEventHandlerExample.GetSourceName(playerPointerEventData),
					", Button Index = ",
					playerPointerEventData.buttonIndex.ToString()
				}));
			}
		}

		// Token: 0x060032FB RID: 13051 RVA: 0x001092A4 File Offset: 0x001074A4
		public void OnPointerDown(PointerEventData eventData)
		{
			if (eventData is PlayerPointerEventData)
			{
				PlayerPointerEventData playerPointerEventData = (PlayerPointerEventData)eventData;
				this.Log(string.Concat(new string[]
				{
					"OnPointerDown:  Player = ",
					playerPointerEventData.playerId.ToString(),
					", Pointer Index = ",
					playerPointerEventData.inputSourceIndex.ToString(),
					", Source = ",
					PlayerPointerEventHandlerExample.GetSourceName(playerPointerEventData),
					", Button Index = ",
					playerPointerEventData.buttonIndex.ToString()
				}));
			}
		}

		// Token: 0x060032FC RID: 13052 RVA: 0x00109330 File Offset: 0x00107530
		public void OnPointerClick(PointerEventData eventData)
		{
			if (eventData is PlayerPointerEventData)
			{
				PlayerPointerEventData playerPointerEventData = (PlayerPointerEventData)eventData;
				this.Log(string.Concat(new string[]
				{
					"OnPointerClick:  Player = ",
					playerPointerEventData.playerId.ToString(),
					", Pointer Index = ",
					playerPointerEventData.inputSourceIndex.ToString(),
					", Source = ",
					PlayerPointerEventHandlerExample.GetSourceName(playerPointerEventData),
					", Button Index = ",
					playerPointerEventData.buttonIndex.ToString()
				}));
			}
		}

		// Token: 0x060032FD RID: 13053 RVA: 0x001093BC File Offset: 0x001075BC
		public void OnScroll(PointerEventData eventData)
		{
			if (eventData is PlayerPointerEventData)
			{
				PlayerPointerEventData playerPointerEventData = (PlayerPointerEventData)eventData;
				this.Log(string.Concat(new string[]
				{
					"OnScroll:  Player = ",
					playerPointerEventData.playerId.ToString(),
					", Pointer Index = ",
					playerPointerEventData.inputSourceIndex.ToString(),
					", Source = ",
					PlayerPointerEventHandlerExample.GetSourceName(playerPointerEventData)
				}));
			}
		}

		// Token: 0x060032FE RID: 13054 RVA: 0x0010942C File Offset: 0x0010762C
		public void OnBeginDrag(PointerEventData eventData)
		{
			if (eventData is PlayerPointerEventData)
			{
				PlayerPointerEventData playerPointerEventData = (PlayerPointerEventData)eventData;
				this.Log(string.Concat(new string[]
				{
					"OnBeginDrag:  Player = ",
					playerPointerEventData.playerId.ToString(),
					", Pointer Index = ",
					playerPointerEventData.inputSourceIndex.ToString(),
					", Source = ",
					PlayerPointerEventHandlerExample.GetSourceName(playerPointerEventData),
					", Button Index = ",
					playerPointerEventData.buttonIndex.ToString()
				}));
			}
		}

		// Token: 0x060032FF RID: 13055 RVA: 0x001094B8 File Offset: 0x001076B8
		public void OnDrag(PointerEventData eventData)
		{
			if (eventData is PlayerPointerEventData)
			{
				PlayerPointerEventData playerPointerEventData = (PlayerPointerEventData)eventData;
				this.Log(string.Concat(new string[]
				{
					"OnDrag:  Player = ",
					playerPointerEventData.playerId.ToString(),
					", Pointer Index = ",
					playerPointerEventData.inputSourceIndex.ToString(),
					", Source = ",
					PlayerPointerEventHandlerExample.GetSourceName(playerPointerEventData),
					", Button Index = ",
					playerPointerEventData.buttonIndex.ToString()
				}));
			}
		}

		// Token: 0x06003300 RID: 13056 RVA: 0x00109544 File Offset: 0x00107744
		public void OnEndDrag(PointerEventData eventData)
		{
			if (eventData is PlayerPointerEventData)
			{
				PlayerPointerEventData playerPointerEventData = (PlayerPointerEventData)eventData;
				this.Log(string.Concat(new string[]
				{
					"OnEndDrag:  Player = ",
					playerPointerEventData.playerId.ToString(),
					", Pointer Index = ",
					playerPointerEventData.inputSourceIndex.ToString(),
					", Source = ",
					PlayerPointerEventHandlerExample.GetSourceName(playerPointerEventData),
					", Button Index = ",
					playerPointerEventData.buttonIndex.ToString()
				}));
			}
		}

		// Token: 0x06003301 RID: 13057 RVA: 0x001095D0 File Offset: 0x001077D0
		private static string GetSourceName(PlayerPointerEventData playerEventData)
		{
			if (playerEventData.sourceType == PointerEventType.Mouse)
			{
				if (playerEventData.mouseSource is Behaviour)
				{
					return (playerEventData.mouseSource as Behaviour).name;
				}
			}
			else if (playerEventData.sourceType == PointerEventType.Touch && playerEventData.touchSource is Behaviour)
			{
				return (playerEventData.touchSource as Behaviour).name;
			}
			return null;
		}

		// Token: 0x04003148 RID: 12616
		public Text text;

		// Token: 0x04003149 RID: 12617
		private const int logLength = 10;

		// Token: 0x0400314A RID: 12618
		private List<string> log = new List<string>();
	}
}
