using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200055A RID: 1370
public class UIMinigameTeam : MonoBehaviour
{
	// Token: 0x06002402 RID: 9218 RVA: 0x00019EDB File Offset: 0x000180DB
	public void Awake()
	{
		this.color_image = base.GetComponent<Image>();
		this.team_name = base.transform.Find("TeamName").GetComponent<Text>();
	}

	// Token: 0x06002403 RID: 9219 RVA: 0x00019F04 File Offset: 0x00018104
	public void Initialize(string name, MinigameTeamColor color)
	{
		this.team_name.text = name;
		this.color_image.color = this.team_colors[(int)color];
	}

	// Token: 0x0400271D RID: 10013
	public Color[] team_colors;

	// Token: 0x0400271E RID: 10014
	private Image color_image;

	// Token: 0x0400271F RID: 10015
	private Text team_name;
}
