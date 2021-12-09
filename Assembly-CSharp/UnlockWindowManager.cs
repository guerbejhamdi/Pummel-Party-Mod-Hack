using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200056D RID: 1389
public class UnlockWindowManager : MonoBehaviour
{
	// Token: 0x0600247E RID: 9342 RVA: 0x000DB2C0 File Offset: 0x000D94C0
	public void Awake()
	{
		this.SetupUnlocks();
		this.SetupRT(ref this.player_rt);
		foreach (RawImage rawImage in this.player_rt_ui)
		{
			if (!(rawImage == null))
			{
				rawImage.texture = this.player_rt;
			}
		}
	}

	// Token: 0x0600247F RID: 9343 RVA: 0x0001A38E File Offset: 0x0001858E
	private void OnLoad()
	{
		this.SetupUnlocks();
	}

	// Token: 0x06002480 RID: 9344 RVA: 0x000DB310 File Offset: 0x000D9510
	private void SetupUnlocks()
	{
		GameManager.LoadUnlocks();
		if ((RBPrefs.HasKey("Unlock0") || RBPrefs.HasKey("Unlock1") || RBPrefs.HasKey("Unlock2") || RBPrefs.HasKey("Unlock3") || RBPrefs.GetInt("TrophyCount", 0) > 0) && RBPrefs.GetInt("HasReloadedUnlocks", 0) == 0)
		{
			GameManager.TrophyCount += RBPrefs.GetInt("TrophyCount", 0);
			for (int i = 0; i < 4; i++)
			{
				if (RBPrefs.GetInt("Unlock" + i.ToString(), 0) == 1)
				{
					GameManager.unlocked[i] = true;
				}
			}
			RBPrefs.SetInt("HasReloadedUnlocks", 1);
			GameManager.SaveUnlocks();
		}
		this.UpdateUI();
	}

	// Token: 0x06002481 RID: 9345 RVA: 0x000DB3C8 File Offset: 0x000D95C8
	public void Show()
	{
		this.mainMenuWindow.SetState(MainMenuWindowState.Visible);
		for (int i = 0; i < this.capeObjects.Length; i++)
		{
			this.capeObjects[i].SetActive(true);
		}
		this.cam.enabled = true;
		this.AddUnlock("Gold", 0, 5, new Rect(0f, 0.75f, 0.25f, 0.25f));
		this.AddUnlock("Rainbow", 1, 5, new Rect(0.5f, 0.75f, 0.25f, 0.25f));
		this.AddUnlock("Fire", 2, 5, new Rect(0.25f, 0.75f, 0.25f, 0.25f));
		this.AddUnlock("Money", 3, 5, new Rect(0.75f, 0.75f, 0.25f, 0.25f));
		this.AddUnlock("Forest", 4, 5, new Rect(0.5f, 0.5f, 0.25f, 0.25f));
		this.AddUnlock("Water", 5, 10, new Rect(0.75f, 0.5f, 0.25f, 0.25f));
		this.AddUnlock("Cosmic", 6, 15, new Rect(0f, 0.5f, 0.25f, 0.25f));
		this.AddUnlock("Wireframe", 7, 20, new Rect(0.25f, 0.5f, 0.25f, 0.25f));
		this.UpdateUI();
	}

	// Token: 0x06002482 RID: 9346 RVA: 0x000DB548 File Offset: 0x000D9748
	private void AddUnlock(string nameToken, int unlockID, int cost, Rect iconRect)
	{
		UnlockObject component = UnityEngine.Object.Instantiate<GameObject>(this.entryPrefab, this.entryRoot, false).GetComponent<UnlockObject>();
		component.Setup(nameToken, cost, unlockID, iconRect, this);
		component.rawImage.texture = this.player_rt;
		this.objects.Add(component);
	}

	// Token: 0x06002483 RID: 9347 RVA: 0x000DB598 File Offset: 0x000D9798
	public void Hide()
	{
		this.mainMenuWindow.SetState(MainMenuWindowState.Hidden);
		for (int i = 0; i < this.capeObjects.Length; i++)
		{
			this.capeObjects[i].SetActive(false);
		}
		this.cam.enabled = false;
		for (int j = 0; j < this.objects.Count; j++)
		{
			UnityEngine.Object.Destroy(this.objects[j].gameObject);
		}
		this.objects.Clear();
	}

	// Token: 0x06002484 RID: 9348 RVA: 0x000DB618 File Offset: 0x000D9818
	public void Update()
	{
		if (!GameManager.DEBUGGING)
		{
			return;
		}
		if (Input.GetKeyDown(KeyCode.Space))
		{
			GameManager.TrophyCount++;
			GameManager.SaveUnlocks();
			this.UpdateUI();
		}
		if (Input.GetKeyDown(KeyCode.R))
		{
			for (int i = 0; i < GameManager.unlocked.Length; i++)
			{
				GameManager.unlocked[i] = false;
			}
			GameManager.SaveUnlocks();
			this.UpdateUI();
		}
	}

	// Token: 0x06002485 RID: 9349 RVA: 0x0001A396 File Offset: 0x00018596
	public void OnUnlock(int unlockID)
	{
		GameManager.unlocked[unlockID] = true;
		this.UpdateUI();
		AudioSystem.PlayOneShot(this.unlockSound, 1f, 0f, 1f);
		GameManager.SaveUnlocks();
	}

	// Token: 0x06002486 RID: 9350 RVA: 0x000DB67C File Offset: 0x000D987C
	private void UpdateUI()
	{
		this.currentTrophyText.text = GameManager.TrophyCount.ToString();
		for (int i = 0; i < this.objects.Count; i++)
		{
			this.objects[i].UpdateButton(GameManager.TrophyCount);
		}
	}

	// Token: 0x06002487 RID: 9351 RVA: 0x0001A3C5 File Offset: 0x000185C5
	private void SetupRT(ref RenderTexture texture)
	{
		texture = new RenderTexture(1024, 1024, 0, RenderTextureFormat.ARGB32);
		texture.filterMode = FilterMode.Bilinear;
		this.cam.targetTexture = texture;
	}

	// Token: 0x06002488 RID: 9352 RVA: 0x0001A3EF File Offset: 0x000185EF
	private void OnDestroy()
	{
		this.ReleaseRenderTexture();
	}

	// Token: 0x06002489 RID: 9353 RVA: 0x0001A3F7 File Offset: 0x000185F7
	private void ReleaseRenderTexture()
	{
		if (this.player_rt != null)
		{
			if (this.cam != null)
			{
				this.cam.targetTexture = null;
			}
			this.player_rt.Release();
			UnityEngine.Object.Destroy(this.player_rt);
		}
	}

	// Token: 0x040027C7 RID: 10183
	public MainMenuWindow mainMenuWindow;

	// Token: 0x040027C8 RID: 10184
	public GameObject entryPrefab;

	// Token: 0x040027C9 RID: 10185
	public Camera cam;

	// Token: 0x040027CA RID: 10186
	public RawImage[] player_rt_ui;

	// Token: 0x040027CB RID: 10187
	public GameObject[] capeObjects;

	// Token: 0x040027CC RID: 10188
	public Transform entryRoot;

	// Token: 0x040027CD RID: 10189
	public Text currentTrophyText;

	// Token: 0x040027CE RID: 10190
	public AudioClip unlockSound;

	// Token: 0x040027CF RID: 10191
	private RenderTexture player_rt;

	// Token: 0x040027D0 RID: 10192
	private List<UnlockObject> objects = new List<UnlockObject>();
}
