using System;
using UnityEngine;
using UnityEngine.UI;
using ZP.Net;

// Token: 0x020002F3 RID: 755
public class PlayerDisconnectUIMessage : MonoBehaviour
{
	// Token: 0x0600150C RID: 5388 RVA: 0x00099C88 File Offset: 0x00097E88
	public void Show(NetPlayer player)
	{
		this.messageText.text = "<color=white>" + player.Name + "</color> disconnected from the game and has been replaced by a bot, you can <color=white>LOAD A SAVE from the last turn</color> in the lobby menu.";
		LeanTween.cancel(base.gameObject);
		this.messageText.enabled = true;
		this.background.enabled = true;
		LeanTween.move(base.gameObject.GetComponent<RectTransform>(), this.show, 0.25f).setEase(LeanTweenType.easeInOutSine);
		LeanTween.move(base.gameObject.GetComponent<RectTransform>(), this.hidden, 0.25f).setEase(LeanTweenType.easeInOutSine).setDelay(7f).setOnComplete(delegate()
		{
			this.background.enabled = false;
			this.messageText.enabled = false;
		});
	}

	// Token: 0x0400160F RID: 5647
	public Text messageText;

	// Token: 0x04001610 RID: 5648
	public Image background;

	// Token: 0x04001611 RID: 5649
	public Animator animator;

	// Token: 0x04001612 RID: 5650
	private Vector3 hidden = new Vector3(0f, 55f, 0f);

	// Token: 0x04001613 RID: 5651
	private Vector3 show = new Vector3(0f, -95f, 0f);
}
