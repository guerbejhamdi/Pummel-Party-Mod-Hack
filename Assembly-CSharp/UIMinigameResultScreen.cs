using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000555 RID: 1365
public class UIMinigameResultScreen : MonoBehaviour
{
	// Token: 0x060023EC RID: 9196 RVA: 0x000D8954 File Offset: 0x000D6B54
	public void Awake()
	{
		this.winner_text.gameObject.SetActive(false);
		for (int i = 0; i < this.placements.Length; i++)
		{
			this.placements[i].SetActive(false);
		}
		for (int j = 0; j < this.goldItem.Length; j++)
		{
			this.goldItem[j].SetActive(false);
		}
		int num = GameManager.GetPlayerCount();
		if (ResultSceenScene.NUM_TEST_PLAYERS != 0)
		{
			num = ResultSceenScene.NUM_TEST_PLAYERS;
		}
		PlacementPos placementPos = this.placement_positions[num - 1];
		for (int k = 0; k < num; k++)
		{
			this.placements[k].GetComponent<RectTransform>().anchoredPosition = placementPos.pos[k];
			this.goldItem[k].GetComponent<RectTransform>().anchoredPosition = placementPos.pos[k] + new Vector3(0f, 140f, 0f);
		}
		this.placementContainer.anchoredPosition += this.offsetScale[num - 1].offset;
		this.placementContainer.localScale = this.offsetScale[num - 1].scale;
	}

	// Token: 0x060023ED RID: 9197 RVA: 0x00019E08 File Offset: 0x00018008
	public void OnEnable()
	{
		this.next_state = Time.time + 1f;
	}

	// Token: 0x060023EE RID: 9198 RVA: 0x00019E1B File Offset: 0x0001801B
	public void SetWinnerText(string text)
	{
		this.winner_text.text = text;
	}

	// Token: 0x060023EF RID: 9199 RVA: 0x000D8A84 File Offset: 0x000D6C84
	public void SetupPlayer(int index, GamePlayer player, byte placement, short gold, byte itemID)
	{
		if (index < this.player_names.Length && index >= 0)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.placementTextPrefabs[(int)placement]);
			gameObject.transform.SetParent(this.placements[index].transform, false);
			Text component = gameObject.transform.Find("PlayerName").GetComponent<Text>();
			string text = ColorUtility.ToHtmlStringRGBA(player.Color.uiColor);
			component.text = string.Concat(new string[]
			{
				"<color=#",
				text,
				">",
				player.Name,
				"</color>"
			});
			if (GameManager.partyGameMode == PartyGameMode.BoardGame)
			{
				this.playerRecieveGold[index].text = gold.ToString();
				player.BoardObject.GiveGold((int)gold, false);
				if (itemID != 255)
				{
					this.playerRecievedItems[index].gameObject.SetActive(true);
					this.playerRecievedItems[index].sprite = GameManager.ItemList.items[(int)itemID].icon;
					player.BoardObject.postMinigameItem = itemID;
					return;
				}
				this.playerRecievedItems[index].gameObject.SetActive(false);
				return;
			}
			else
			{
				GameManager.UIController.minigameOnlySceneController.AddScore((int)player.GlobalID, this.scores[(int)placement]);
				this.goldItem[index].SetActive(false);
				this.playerRecieveGold[index].gameObject.SetActive(false);
				this.playerRecievedItems[index].gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x060023F0 RID: 9200 RVA: 0x000D8C00 File Offset: 0x000D6E00
	public void Update()
	{
		switch (this.cur_state)
		{
		case 0:
			if (Time.time >= this.next_state)
			{
				this.winner_text.gameObject.SetActive(true);
				AudioSystem.PlayOneShot("CinematicImpact01", 0.5f, 0f);
				this.cur_state++;
				return;
			}
			break;
		case 1:
			if (Time.time > this.next_placement_show)
			{
				this.placements[this.cur_placement].SetActive(true);
				if (GameManager.partyGameMode == PartyGameMode.BoardGame)
				{
					this.goldItem[this.cur_placement].SetActive(true);
				}
				this.cur_placement++;
				this.next_placement_show = Time.time + this.placement_delay;
				if (ResultSceenScene.NUM_TEST_PLAYERS == 0)
				{
					if (this.cur_placement >= GameManager.GetPlayerCount())
					{
						this.cur_state++;
						return;
					}
				}
				else if (this.cur_placement >= ResultSceenScene.NUM_TEST_PLAYERS)
				{
					this.cur_state++;
				}
			}
			break;
		case 2:
		case 3:
			break;
		default:
			return;
		}
	}

	// Token: 0x040026EF RID: 9967
	public Text winner_text;

	// Token: 0x040026F0 RID: 9968
	public GameObject[] placementTextPrefabs = new GameObject[4];

	// Token: 0x040026F1 RID: 9969
	public GameObject[] placements;

	// Token: 0x040026F2 RID: 9970
	public GameObject[] goldItem;

	// Token: 0x040026F3 RID: 9971
	public Text[] player_names;

	// Token: 0x040026F4 RID: 9972
	public Image[] playerRecievedItems;

	// Token: 0x040026F5 RID: 9973
	public Text[] playerRecieveGold;

	// Token: 0x040026F6 RID: 9974
	public float placement_delay = 0.25f;

	// Token: 0x040026F7 RID: 9975
	public PlacementPos[] placement_positions;

	// Token: 0x040026F8 RID: 9976
	public ResultUIOffsetScale[] offsetScale;

	// Token: 0x040026F9 RID: 9977
	public RectTransform placementContainer;

	// Token: 0x040026FA RID: 9978
	private int cur_state;

	// Token: 0x040026FB RID: 9979
	private float next_state;

	// Token: 0x040026FC RID: 9980
	private int cur_placement;

	// Token: 0x040026FD RID: 9981
	private float next_placement_show;

	// Token: 0x040026FE RID: 9982
	private int[] scores = new int[]
	{
		20,
		16,
		13,
		10,
		7,
		5,
		3,
		1
	};
}
