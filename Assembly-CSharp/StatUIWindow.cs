using System;
using System.Collections.Generic;
using I2.Loc;
using Rewired;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020004BF RID: 1215
public class StatUIWindow : MonoBehaviour
{
	// Token: 0x170003D4 RID: 980
	// (get) Token: 0x06002041 RID: 8257 RVA: 0x000178D6 File Offset: 0x00015AD6
	// (set) Token: 0x06002042 RID: 8258 RVA: 0x000CA4DC File Offset: 0x000C86DC
	public int ShowCount
	{
		get
		{
			return this.m_showCount;
		}
		set
		{
			Debug.LogError("Setting show count to " + value.ToString());
			if (value <= 0 && this.m_showCount > 0)
			{
				this.Hide();
			}
			if (value > 0 && this.m_showCount <= 0)
			{
				this.Show();
			}
			this.m_showCount = value;
			if (this.m_showCount < 0)
			{
				this.m_showCount = 0;
			}
		}
	}

	// Token: 0x06002043 RID: 8259 RVA: 0x0000398C File Offset: 0x00001B8C
	public void Awake()
	{
	}

	// Token: 0x06002044 RID: 8260 RVA: 0x000CA53C File Offset: 0x000C873C
	public void Start()
	{
		if (GameManager.IsInitialized)
		{
			this.m_playerCount = GameManager.GetPlayerCount();
		}
		else
		{
			this.m_playerCount = 8;
		}
		this.columns = Enum.GetNames(typeof(StatType));
		for (int i = 0; i < this.columns.Length; i++)
		{
			this.columns[i] = LocalizationManager.GetTranslation("Challenge_Statistic_" + this.columns[i], true, 0, true, false, null, null, true);
		}
		int cells = this.columns.Length + 1;
		this.m_rowPfb.SetActive(false);
		for (int j = 0; j < this.m_playerCount + 1; j++)
		{
			GameObject obj = UnityEngine.Object.Instantiate<GameObject>(this.m_rowPfb, this.m_rowParent);
			this.SetupRow(obj, cells, j == 0, j > 0 && j % 2 == 0);
		}
		for (int k = 0; k < this.columns.Length; k++)
		{
			this.m_rows[0].GetCell(k + 1).SetValue(this.columns[k]);
		}
		this.InitTest();
		this.PrintStats(true);
		this.SetSortType(StatType.MinigamesWon);
		if (GameManager.GetLocalNonAIPlayers().Count <= 0)
		{
			this.Show();
		}
	}

	// Token: 0x06002045 RID: 8261 RVA: 0x000178DE File Offset: 0x00015ADE
	private void OnDestroy()
	{
		StatTracker.OnStatChanged.RemoveListener(new UnityAction<StatType>(this.OnStatChanged));
	}

	// Token: 0x06002046 RID: 8262 RVA: 0x000CA668 File Offset: 0x000C8868
	private void SetupRow(GameObject obj, int cells, bool isHeader, bool isAlt)
	{
		StatUIRow component = obj.GetComponent<StatUIRow>();
		if (component != null)
		{
			component.Setup(cells, isHeader, isAlt);
			this.m_rows.Add(component);
			component.gameObject.SetActive(true);
		}
	}

	// Token: 0x06002047 RID: 8263 RVA: 0x000178F6 File Offset: 0x00015AF6
	private void Update()
	{
		this.UpdateTest();
		if (GameManager.DEBUGGING && Input.GetKeyDown(KeyCode.F10))
		{
			if (this.m_hidden)
			{
				this.Show();
				return;
			}
			this.Hide();
		}
	}

	// Token: 0x06002048 RID: 8264 RVA: 0x00017926 File Offset: 0x00015B26
	public void Show()
	{
		if (!this.m_hidden)
		{
			return;
		}
		this.m_hidden = false;
		this.m_anim.SetBool("Hidden", this.m_hidden);
		Debug.Log("Show Stats Window!");
	}

	// Token: 0x06002049 RID: 8265 RVA: 0x00017958 File Offset: 0x00015B58
	public void Hide()
	{
		if (this.m_hidden)
		{
			return;
		}
		this.m_hidden = true;
		this.m_anim.SetBool("Hidden", this.m_hidden);
	}

	// Token: 0x0600204A RID: 8266 RVA: 0x000CA6A8 File Offset: 0x000C88A8
	public void SetSortType(StatType type)
	{
		this.m_sortType = type;
		for (int i = 1; i < this.m_rows[0].CellCount; i++)
		{
			this.m_rows[0].GetCell(i).SetSelected(type);
		}
		this.PrintStats(false);
	}

	// Token: 0x0600204B RID: 8267 RVA: 0x000CA6F8 File Offset: 0x000C88F8
	private void InitTest()
	{
		StatTracker.OnStatChanged.AddListener(new UnityAction<StatType>(this.OnStatChanged));
		this.m_lastUpdate = Time.time;
		this.columns = Enum.GetNames(typeof(StatType));
		for (int i = 0; i < this.columns.Length; i++)
		{
			this.columns[i] = LocalizationManager.GetTranslation("Challenge_Statistic_" + this.columns[i], true, 0, true, false, null, null, true);
		}
	}

	// Token: 0x0600204C RID: 8268 RVA: 0x00017980 File Offset: 0x00015B80
	private void UpdateTest()
	{
		if (this.m_statsChanged)
		{
			this.PrintStats(false);
			this.m_statsChanged = false;
		}
	}

	// Token: 0x0600204D RID: 8269 RVA: 0x000CA774 File Offset: 0x000C8974
	private void PrintStats(bool setup = false)
	{
		bool isInitialized = GameManager.IsInitialized;
		List<PlayerStats> playerStats = StatTracker.GetPlayerStats(this.m_playerCount, this.m_playerCount, this.m_sortType, false, StatSortType.Descending);
		int num = 1;
		foreach (PlayerStats playerStats2 in playerStats)
		{
			StatUIRow statUIRow = this.m_rows[num];
			StatUICell cell = statUIRow.GetCell(0);
			if (isInitialized)
			{
				GamePlayer playerWithID = GameManager.GetPlayerWithID(playerStats2.PlayerID);
				cell.SetValue(playerWithID.Name);
				Color uiColor = playerWithID.Color.uiColor;
				uiColor.a = 0.75f;
				cell.SetBackgroundColor(uiColor);
				if (setup)
				{
					cell.AddShadow();
					cell.SetFontStyle(FontStyle.Bold);
				}
			}
			else
			{
				cell.SetValue("Player " + playerStats2.PlayerID.ToString());
			}
			for (int i = 0; i < this.columns.Length; i++)
			{
				TrackedStat stat = playerStats2.GetStat((StatType)i);
				statUIRow.GetCell(i + 1).SetValue(stat.Value.ToString("0"));
				if (setup)
				{
					cell.SetFontStyle(FontStyle.Bold);
				}
			}
			num++;
		}
	}

	// Token: 0x0600204E RID: 8270 RVA: 0x00017998 File Offset: 0x00015B98
	private void OnStatChanged(StatType type)
	{
		this.m_statsChanged = true;
	}

	// Token: 0x0400230D RID: 8973
	[SerializeField]
	private RectTransform m_rowParent;

	// Token: 0x0400230E RID: 8974
	[SerializeField]
	private Animator m_anim;

	// Token: 0x0400230F RID: 8975
	[Header("Prefabs")]
	[SerializeField]
	private GameObject m_rowPfb;

	// Token: 0x04002310 RID: 8976
	private List<StatUIRow> m_rows = new List<StatUIRow>();

	// Token: 0x04002311 RID: 8977
	private int m_playerCount = 4;

	// Token: 0x04002312 RID: 8978
	private bool m_hidden = true;

	// Token: 0x04002313 RID: 8979
	private ControllerType m_lastControllerType = ControllerType.Custom;

	// Token: 0x04002314 RID: 8980
	private int m_showCount;

	// Token: 0x04002315 RID: 8981
	private StatType m_sortType;

	// Token: 0x04002316 RID: 8982
	private string[] columns;

	// Token: 0x04002317 RID: 8983
	private float m_lastUpdate;

	// Token: 0x04002318 RID: 8984
	private bool m_statsChanged;
}
