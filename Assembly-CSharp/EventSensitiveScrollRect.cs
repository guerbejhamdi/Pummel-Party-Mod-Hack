using System;
using Rewired;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x020004F8 RID: 1272
public class EventSensitiveScrollRect : MonoBehaviour, IUpdateSelectedHandler, IEventSystemHandler
{
	// Token: 0x0600217C RID: 8572 RVA: 0x0001843E File Offset: 0x0001663E
	public void Awake()
	{
		this.sr = base.gameObject.GetComponent<ScrollRect>();
		this.player = ReInput.players.GetPlayer(0);
	}

	// Token: 0x0600217D RID: 8573 RVA: 0x000CF250 File Offset: 0x000CD450
	public void OnUpdateSelected(BaseEventData eventData)
	{
		if (this.player.controllers.GetLastActiveController() != null && this.player.controllers.GetLastActiveController().type != ControllerType.Joystick)
		{
			return;
		}
		float height = this.sr.content.rect.height;
		float height2 = this.sr.viewport.rect.height;
		float y = eventData.selectedObject.transform.localPosition.y;
		float num = y + eventData.selectedObject.GetComponent<RectTransform>().rect.height / 2f;
		float num2 = y - eventData.selectedObject.GetComponent<RectTransform>().rect.height / 2f;
		float num3 = (height - height2) * this.sr.normalizedPosition.y - height;
		float num4 = num3 + height2;
		float num5;
		if (num > num4)
		{
			num5 = num - height2 + eventData.selectedObject.GetComponent<RectTransform>().rect.height * EventSensitiveScrollRect.SCROLL_MARGIN;
		}
		else
		{
			if (num2 >= num3)
			{
				return;
			}
			num5 = num2 - eventData.selectedObject.GetComponent<RectTransform>().rect.height * EventSensitiveScrollRect.SCROLL_MARGIN;
		}
		float value = (num5 + height) / (height - height2);
		this.sr.normalizedPosition = new Vector2(0f, Mathf.Clamp01(value));
	}

	// Token: 0x04002426 RID: 9254
	private static float SCROLL_MARGIN = 0.3f;

	// Token: 0x04002427 RID: 9255
	private ScrollRect sr;

	// Token: 0x04002428 RID: 9256
	private Player player;
}
