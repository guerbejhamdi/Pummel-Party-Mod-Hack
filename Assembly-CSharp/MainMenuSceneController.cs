using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000127 RID: 295
public class MainMenuSceneController : MonoBehaviour
{
	// Token: 0x060008AF RID: 2223 RVA: 0x0005052C File Offset: 0x0004E72C
	public void Awake()
	{
		Time.timeScale = 1f;
		switch (GameManager.GetCurrentEventTheme())
		{
		case GameEventTheme.None:
			this.m_nebulaScene.SetActive(false);
			SceneManager.LoadScene("ArsenalUpdateMenu", LoadSceneMode.Additive);
			return;
		case GameEventTheme.Halloween:
			this.m_nebulaScene.SetActive(false);
			SceneManager.LoadScene("HalloweenMenuScene", LoadSceneMode.Additive);
			return;
		case GameEventTheme.Christmas:
			this.m_nebulaScene.SetActive(false);
			SceneManager.LoadScene("WinterMenuScene", LoadSceneMode.Additive);
			return;
		default:
			return;
		}
	}

	// Token: 0x04000716 RID: 1814
	[SerializeField]
	protected GameObject m_nebulaScene;
}
