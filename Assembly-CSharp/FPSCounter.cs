using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Token: 0x02000456 RID: 1110
public class FPSCounter : MonoBehaviour
{
	// Token: 0x06001E6B RID: 7787 RVA: 0x000C425C File Offset: 0x000C245C
	private void Start()
	{
		this.uiText = base.GetComponent<Text>();
		SceneManager.sceneLoaded += this.SceneManager_sceneLoaded;
		this.frameTimes = new List<float>[SceneManager.sceneCountInBuildSettings];
		this.playCount = new int[SceneManager.sceneCountInBuildSettings];
		for (int i = 0; i < this.frameTimes.Length; i++)
		{
			this.frameTimes[i] = new List<float>();
		}
	}

	// Token: 0x06001E6C RID: 7788 RVA: 0x000C42C8 File Offset: 0x000C24C8
	private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (mode == LoadSceneMode.Additive)
		{
			this.curIndex = scene.buildIndex;
			this.playCount[this.curIndex]++;
			string path = Application.persistentDataPath + "/FrameRates.csv";
			try
			{
				string text = "";
				for (int i = 0; i < this.frameTimes.Length; i++)
				{
					if (this.frameTimes[i].Count != 0)
					{
						text += this.GetLine(this.frameTimes[i], SceneManager.GetSceneByBuildIndex(i).name, this.playCount[i]);
					}
				}
				text += this.GetLine(this.boardFrameTimes, GameManager.CurMap.name, 1);
				if (File.Exists(path))
				{
					File.Delete(path);
				}
				File.WriteAllText(path, text);
			}
			catch (Exception ex)
			{
				Debug.LogError("Saving turn failed: " + ex.ToString());
			}
		}
	}

	// Token: 0x06001E6D RID: 7789 RVA: 0x000C43C4 File Offset: 0x000C25C4
	private string GetLine(List<float> frameTimes, string name, int count)
	{
		float num = float.MaxValue;
		float num2 = float.MinValue;
		float num3 = 0f;
		for (int i = 0; i < frameTimes.Count; i++)
		{
			num3 += frameTimes[i];
			if (frameTimes[i] < num)
			{
				num = frameTimes[i];
			}
			if (frameTimes[i] > num2)
			{
				num2 = frameTimes[i];
			}
		}
		float num4 = num3 / (float)frameTimes.Count;
		return string.Concat(new string[]
		{
			name,
			",",
			count.ToString(),
			",",
			(1f / num4).ToString(),
			",",
			(1f / num2).ToString(),
			",",
			(1f / num).ToString(),
			"\n"
		});
	}

	// Token: 0x06001E6E RID: 7790 RVA: 0x000C44B8 File Offset: 0x000C26B8
	private void Update()
	{
		this.deltaTime += (Time.deltaTime - this.deltaTime) * 0.1f;
		if (Time.time - this.lastChange > this.interval)
		{
			this.uiText.text = string.Format(this.format, this.deltaTime * 1000f, 1f / this.deltaTime);
			this.uiText.text = FPSCounter.fpsStrings[Mathf.Clamp((int)(1f / this.deltaTime), 0, 299)];
			this.lastChange = Time.time;
			if (this.curIndex != -1 && GameManager.Board.BoardState == GameBoardState.Minigame && GameManager.Minigame != null && GameManager.Minigame.State >= MinigameControllerState.FadeIn)
			{
				this.frameTimes[this.curIndex].Add(this.deltaTime);
				return;
			}
			if (GameManager.Board != null && GameManager.Board.BoardState == GameBoardState.PlayTurns)
			{
				this.boardFrameTimes.Add(this.deltaTime);
			}
		}
	}

	// Token: 0x04002151 RID: 8529
	private float deltaTime;

	// Token: 0x04002152 RID: 8530
	private Text uiText;

	// Token: 0x04002153 RID: 8531
	private float interval = 0.2f;

	// Token: 0x04002154 RID: 8532
	private float lastChange;

	// Token: 0x04002155 RID: 8533
	private string format = "{0:0.0} ms ({1:0.} fps)";

	// Token: 0x04002156 RID: 8534
	private static readonly string[] fpsStrings = new string[]
	{
		"00",
		"01",
		"02",
		"03",
		"04",
		"05",
		"06",
		"07",
		"08",
		"09",
		"10",
		"11",
		"12",
		"13",
		"14",
		"15",
		"16",
		"17",
		"18",
		"19",
		"20",
		"21",
		"22",
		"23",
		"24",
		"25",
		"26",
		"27",
		"28",
		"29",
		"30",
		"31",
		"32",
		"33",
		"34",
		"35",
		"36",
		"37",
		"38",
		"39",
		"40",
		"41",
		"42",
		"43",
		"44",
		"45",
		"46",
		"47",
		"48",
		"49",
		"50",
		"51",
		"52",
		"53",
		"54",
		"55",
		"56",
		"57",
		"58",
		"59",
		"60",
		"61",
		"62",
		"63",
		"64",
		"65",
		"66",
		"67",
		"68",
		"69",
		"70",
		"71",
		"72",
		"73",
		"74",
		"75",
		"76",
		"77",
		"78",
		"79",
		"80",
		"81",
		"82",
		"83",
		"84",
		"85",
		"86",
		"87",
		"88",
		"89",
		"90",
		"91",
		"92",
		"93",
		"94",
		"95",
		"96",
		"97",
		"98",
		"99",
		"100",
		"101",
		"102",
		"103",
		"104",
		"105",
		"106",
		"107",
		"108",
		"109",
		"110",
		"111",
		"112",
		"113",
		"114",
		"115",
		"116",
		"117",
		"118",
		"119",
		"120",
		"121",
		"122",
		"123",
		"124",
		"125",
		"126",
		"127",
		"128",
		"129",
		"130",
		"131",
		"132",
		"133",
		"134",
		"135",
		"136",
		"137",
		"138",
		"139",
		"140",
		"141",
		"142",
		"143",
		"144",
		"145",
		"146",
		"147",
		"148",
		"149",
		"150",
		"151",
		"152",
		"153",
		"154",
		"155",
		"156",
		"157",
		"158",
		"159",
		"160",
		"161",
		"162",
		"163",
		"164",
		"165",
		"166",
		"167",
		"168",
		"169",
		"170",
		"171",
		"172",
		"173",
		"174",
		"175",
		"176",
		"177",
		"178",
		"179",
		"180",
		"181",
		"182",
		"183",
		"184",
		"185",
		"186",
		"187",
		"188",
		"189",
		"190",
		"191",
		"192",
		"193",
		"194",
		"195",
		"196",
		"197",
		"198",
		"199",
		"200",
		"201",
		"202",
		"203",
		"204",
		"205",
		"206",
		"207",
		"208",
		"209",
		"210",
		"211",
		"212",
		"213",
		"214",
		"215",
		"216",
		"217",
		"218",
		"219",
		"220",
		"221",
		"222",
		"223",
		"224",
		"225",
		"226",
		"227",
		"228",
		"229",
		"230",
		"231",
		"232",
		"233",
		"234",
		"235",
		"236",
		"237",
		"238",
		"239",
		"240",
		"241",
		"242",
		"243",
		"244",
		"245",
		"246",
		"247",
		"248",
		"249",
		"250",
		"251",
		"252",
		"253",
		"254",
		"255",
		"256",
		"257",
		"258",
		"259",
		"260",
		"261",
		"262",
		"263",
		"264",
		"265",
		"266",
		"267",
		"268",
		"269",
		"270",
		"271",
		"272",
		"273",
		"274",
		"275",
		"276",
		"277",
		"278",
		"279",
		"280",
		"281",
		"282",
		"283",
		"284",
		"285",
		"286",
		"287",
		"288",
		"289",
		"290",
		"291",
		"292",
		"293",
		"294",
		"295",
		"296",
		"297",
		"298",
		"299"
	};

	// Token: 0x04002157 RID: 8535
	private List<float>[] frameTimes;

	// Token: 0x04002158 RID: 8536
	private List<float> boardFrameTimes = new List<float>();

	// Token: 0x04002159 RID: 8537
	private int[] playCount;

	// Token: 0x0400215A RID: 8538
	private int curIndex = -1;
}
