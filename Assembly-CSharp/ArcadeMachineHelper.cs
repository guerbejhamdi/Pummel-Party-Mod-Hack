using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

// Token: 0x02000317 RID: 791
public class ArcadeMachineHelper : MonoBehaviour
{
	// Token: 0x060015B5 RID: 5557 RVA: 0x0000398C File Offset: 0x00001B8C
	public void Despawn()
	{
	}

	// Token: 0x060015B6 RID: 5558 RVA: 0x0009C4D0 File Offset: 0x0009A6D0
	public void SetSelectedGame(int index)
	{
		foreach (Image image in this.m_gameIcons)
		{
			image.color = Color.white;
			image.transform.localScale = Vector3.one;
		}
		this.m_gameIcons[index].color = this.m_selectedColor;
		this.m_gameIcons[index].transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
	}

	// Token: 0x060015B7 RID: 5559 RVA: 0x000106F8 File Offset: 0x0000E8F8
	public void SetPlayerReady(int index)
	{
		this.m_readyIcons[index].sprite = this.m_readyIconTrue;
		this.m_readyIcons[index].color = this.m_readyColorTrue;
	}

	// Token: 0x060015B8 RID: 5560 RVA: 0x0009C548 File Offset: 0x0009A748
	public void SetGameTime(float value)
	{
		this.m_timer.text = Mathf.CeilToInt(value).ToString();
	}

	// Token: 0x060015B9 RID: 5561 RVA: 0x0009C570 File Offset: 0x0009A770
	public void SetCountDown(float time)
	{
		int num = Mathf.CeilToInt(time) - 1;
		if (num >= 0 && num < this.m_countdownNumbers.Length)
		{
			if (this.m_lastCountDownIndex != num)
			{
				AudioSystem.PlayOneShot(this.m_countdownSound, 1f, 0f, 1f);
				this.m_lastCountDownIndex = num;
			}
			if (!this.m_countdown.gameObject.activeSelf)
			{
				this.m_countdown.gameObject.SetActive(true);
			}
			this.m_countdown.sprite = this.m_countdownNumbers[num];
			return;
		}
		if (this.m_countdown.gameObject.activeSelf)
		{
			this.m_countdown.gameObject.SetActive(false);
		}
	}

	// Token: 0x040016BE RID: 5822
	[SerializeField]
	public Transform m_cameraTransform;

	// Token: 0x040016BF RID: 5823
	[SerializeField]
	public PostProcessVolume m_postProcessVolume;

	// Token: 0x040016C0 RID: 5824
	[SerializeField]
	public Transform[] m_JoystickTransforms;

	// Token: 0x040016C1 RID: 5825
	[SerializeField]
	public MeshRenderer m_screenMeshRenderer;

	// Token: 0x040016C2 RID: 5826
	[SerializeField]
	public Camera m_screenCamera;

	// Token: 0x040016C3 RID: 5827
	[SerializeField]
	public Text[] m_PlayerNames;

	// Token: 0x040016C4 RID: 5828
	[SerializeField]
	public Text[] m_PlayerScores;

	// Token: 0x040016C5 RID: 5829
	[SerializeField]
	public Text m_timer;

	// Token: 0x040016C6 RID: 5830
	[SerializeField]
	public RectTransform[] m_PlayerObjects;

	// Token: 0x040016C7 RID: 5831
	[SerializeField]
	public Image[] m_gameIcons;

	// Token: 0x040016C8 RID: 5832
	[SerializeField]
	public Color m_selectedColor;

	// Token: 0x040016C9 RID: 5833
	[SerializeField]
	public Text m_readyWindowDescriptionTxt;

	// Token: 0x040016CA RID: 5834
	[SerializeField]
	public Text[] m_readyWindowPlayerNameTxt;

	// Token: 0x040016CB RID: 5835
	[SerializeField]
	public Text m_readyWindowHeaderTxt;

	// Token: 0x040016CC RID: 5836
	[SerializeField]
	public Image[] m_readyIcons;

	// Token: 0x040016CD RID: 5837
	[SerializeField]
	public Sprite m_readyIconTrue;

	// Token: 0x040016CE RID: 5838
	[SerializeField]
	public Color m_readyColorTrue;

	// Token: 0x040016CF RID: 5839
	[SerializeField]
	public GameObject[] m_gameScenes;

	// Token: 0x040016D0 RID: 5840
	[SerializeField]
	public SpriteRenderer m_countdown;

	// Token: 0x040016D1 RID: 5841
	[SerializeField]
	public Sprite[] m_countdownNumbers;

	// Token: 0x040016D2 RID: 5842
	[SerializeField]
	public AudioClip m_countdownSound;

	// Token: 0x040016D3 RID: 5843
	[Header("Windows")]
	[SerializeField]
	public GameObject m_gameSelectWindow;

	// Token: 0x040016D4 RID: 5844
	[SerializeField]
	public GameObject m_gameReadyWindow;

	// Token: 0x040016D5 RID: 5845
	[SerializeField]
	public GameObject m_scoreboardWindow;

	// Token: 0x040016D6 RID: 5846
	[SerializeField]
	public ArcadeSpawnPoint[] spawnPoints;

	// Token: 0x040016D7 RID: 5847
	[SerializeField]
	public RectTransform m_paddleBall;

	// Token: 0x040016D8 RID: 5848
	private int m_lastCountDownIndex = -1;
}
