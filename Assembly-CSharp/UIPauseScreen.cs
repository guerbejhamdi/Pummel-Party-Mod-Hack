using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200055F RID: 1375
public class UIPauseScreen : MonoBehaviour
{
	// Token: 0x06002442 RID: 9282 RVA: 0x000DA580 File Offset: 0x000D8780
	public void Start()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.optionsPrefab);
		gameObject.transform.SetParent(base.transform, false);
		this.optionsWindow = gameObject.GetComponent<OptionsWindow>();
		GameManager.OptionsWindow = this.optionsWindow;
		this.optionsWindow.MainMenu = false;
		base.StartCoroutine(this.PauseUnpause());
	}

	// Token: 0x06002443 RID: 9283 RVA: 0x0001A154 File Offset: 0x00018354
	private IEnumerator PauseUnpause()
	{
		yield return new WaitUntil(() => GameManager.UIController != null);
		GameManager.PauseGame(true);
		GameManager.UnpauseGame(true);
		yield return null;
		yield break;
	}

	// Token: 0x06002444 RID: 9284 RVA: 0x0001A15C File Offset: 0x0001835C
	public void HideScreen()
	{
		this.optionsWindow.DoButtonEvent(MainMenuButtonEventType.OptionsWindowBack);
	}

	// Token: 0x06002445 RID: 9285 RVA: 0x0001A16B File Offset: 0x0001836B
	public void ShowScreen()
	{
		this.optionsWindow.DoButtonEvent(MainMenuButtonEventType.GoOptionsWindow);
	}

	// Token: 0x04002772 RID: 10098
	public GameObject optionsPrefab;

	// Token: 0x04002773 RID: 10099
	public GameObject UIBlurBackgroundPrefab;

	// Token: 0x04002774 RID: 10100
	public Animator menuBorderAnimator;

	// Token: 0x04002775 RID: 10101
	private OptionsWindow optionsWindow;
}
