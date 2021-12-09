using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000221 RID: 545
public class RhythmUIPanel : MonoBehaviour
{
	// Token: 0x06000FD4 RID: 4052 RVA: 0x0007D520 File Offset: 0x0007B720
	public void Init(RhythmController controller)
	{
		this.m_controller = controller;
		short num = 0;
		foreach (Transform transform in this.m_trackPanels)
		{
			RhythmTrack rhythmTrack = new RhythmTrack();
			rhythmTrack.spawn = (RectTransform)transform.Find("SpawnPoint");
			rhythmTrack.hit = (RectTransform)transform.Find("HitPoint");
			rhythmTrack.comboBonus = (RectTransform)transform.Find("ComboBonus");
			rhythmTrack.buttonContainer = (RectTransform)transform;
			Transform transform2 = (RectTransform)transform.Find("ComboTxt");
			rhythmTrack.comboTxt = transform2.GetComponent<Text>();
			rhythmTrack.comboTxt.text = "0";
			Image component = transform.GetComponent<Image>();
			GamePlayer playerWithID = GameManager.GetPlayerWithID(num);
			if (playerWithID != null && component != null)
			{
				Color uiColor = playerWithID.Color.uiColor;
				component.color = new Color(uiColor.r, uiColor.g, uiColor.b, 0.5f);
				this.m_tracks.Add(rhythmTrack);
			}
			num += 1;
		}
	}

	// Token: 0x06000FD5 RID: 4053 RVA: 0x0000D885 File Offset: 0x0000BA85
	public void SetCombo(int trackIndex, short combo)
	{
		if (trackIndex < 0 || trackIndex >= this.m_tracks.Count)
		{
			return;
		}
		this.m_tracks[trackIndex].comboTxt.text = combo.ToString();
	}

	// Token: 0x06000FD6 RID: 4054 RVA: 0x0007D644 File Offset: 0x0007B844
	public void ShowComboBonus(int trackIndex, int bonus)
	{
		if (trackIndex < 0 || trackIndex >= this.m_tracks.Count)
		{
			return;
		}
		RhythmTrack rhythmTrack = this.m_tracks[trackIndex];
		rhythmTrack.comboBonus.GetComponent<Text>().text = "Combo +" + bonus.ToString();
		rhythmTrack.comboBonus.gameObject.SetActive(true);
	}

	// Token: 0x06000FD7 RID: 4055 RVA: 0x0007D6A4 File Offset: 0x0007B8A4
	public void Show()
	{
		Transform[] trackPanels = this.m_trackPanels;
		for (int i = 0; i < trackPanels.Length; i++)
		{
			trackPanels[i].gameObject.SetActive(true);
		}
	}

	// Token: 0x06000FD8 RID: 4056 RVA: 0x0000D8B7 File Offset: 0x0000BAB7
	public Transform GetScoreUIParent(int index)
	{
		return this.m_trackPanels[index].Find("ScoreUIAnchor");
	}

	// Token: 0x06000FD9 RID: 4057 RVA: 0x0007D6D4 File Offset: 0x0007B8D4
	public void SpawnButton(RhythmHit hit)
	{
		foreach (RhythmTrack rhythmTrack in this.m_tracks)
		{
			RhythmButtonUI component = UnityEngine.Object.Instantiate<GameObject>(this.m_hitButtonUI, rhythmTrack.buttonContainer).GetComponent<RhythmButtonUI>();
			component.Init(hit, rhythmTrack, this.m_btnIndex);
			component.SetPosition(rhythmTrack.spawn.anchoredPosition);
			component.SetInputType(rhythmTrack.inputType);
			rhythmTrack.activeButtons.Add(component);
		}
		this.m_btnIndex++;
	}

	// Token: 0x06000FDA RID: 4058 RVA: 0x0007D77C File Offset: 0x0007B97C
	public void UpdatePanel()
	{
		foreach (RhythmTrack rhythmTrack in this.m_tracks)
		{
			for (int i = rhythmTrack.activeButtons.Count - 1; i >= 0; i--)
			{
				RhythmButtonUI rhythmButtonUI = rhythmTrack.activeButtons[i];
				Vector2 anchoredPosition = rhythmButtonUI.Track.spawn.anchoredPosition;
				Vector2 anchoredPosition2 = rhythmButtonUI.Track.hit.anchoredPosition;
				float num = (this.m_controller.CurrentSongTime - (rhythmButtonUI.HitInfo.startTime - RhythmController.LeadTime)) / RhythmController.LeadTime;
				Vector2 position = Vector2.LerpUnclamped(anchoredPosition, anchoredPosition2, num);
				rhythmButtonUI.SetPosition(position);
				if (num > 1.075f)
				{
					rhythmTrack.activeButtons.RemoveAt(i);
					UnityEngine.Object.Destroy(rhythmButtonUI.gameObject);
				}
			}
		}
	}

	// Token: 0x06000FDB RID: 4059 RVA: 0x0007D874 File Offset: 0x0007BA74
	public RhythmHitResult TryHitTrack(int trackIndex, out RhythmHitButton button, out float hitTime)
	{
		button = RhythmHitButton.None;
		hitTime = 3f;
		if (trackIndex < 0 || trackIndex >= this.m_tracks.Count)
		{
			Debug.LogError("invalid rhyhtm track index");
			return RhythmHitResult.Miss;
		}
		RhythmTrack rhythmTrack = this.m_tracks[trackIndex];
		if (rhythmTrack.activeButtons.Count <= 0)
		{
			return RhythmHitResult.Miss;
		}
		RhythmButtonUI rhythmButtonUI = rhythmTrack.activeButtons[0];
		if (rhythmButtonUI == null)
		{
			return RhythmHitResult.Miss;
		}
		button = rhythmButtonUI.HitInfo.button;
		return this.GetNextHitResult(rhythmButtonUI, out hitTime, 0.25f);
	}

	// Token: 0x06000FDC RID: 4060 RVA: 0x0007D8F8 File Offset: 0x0007BAF8
	public RhythmHitResult HitTrack(int trackIndex, RhythmHitButton button, out int btnIndex, bool useAudio)
	{
		btnIndex = -1;
		if (useAudio)
		{
			AudioSystem.PlayOneShot(this.m_hitClip, 1f, 0f, 1f);
		}
		if (trackIndex < 0 || trackIndex >= this.m_tracks.Count)
		{
			return RhythmHitResult.Miss;
		}
		RhythmTrack rhythmTrack = this.m_tracks[trackIndex];
		if (rhythmTrack.activeButtons.Count <= 0)
		{
			return RhythmHitResult.Miss;
		}
		RhythmButtonUI rhythmButtonUI = rhythmTrack.activeButtons[0];
		btnIndex = rhythmButtonUI.Index;
		if (rhythmButtonUI == null || rhythmButtonUI.HitInfo.button != button)
		{
			return RhythmHitResult.Miss;
		}
		float num = 0f;
		return this.GetNextHitResult(rhythmButtonUI, out num, 1f);
	}

	// Token: 0x06000FDD RID: 4061 RVA: 0x0007D99C File Offset: 0x0007BB9C
	public void HitButton(int trackIndex, int btnIndex)
	{
		if (trackIndex < 0 || trackIndex >= this.m_tracks.Count)
		{
			return;
		}
		RhythmTrack rhythmTrack = this.m_tracks[trackIndex];
		for (int i = rhythmTrack.activeButtons.Count - 1; i >= 0; i--)
		{
			RhythmButtonUI rhythmButtonUI = rhythmTrack.activeButtons[i];
			if (rhythmButtonUI != null && rhythmButtonUI.Index == btnIndex)
			{
				rhythmButtonUI.Hit();
				rhythmTrack.activeButtons.Remove(rhythmButtonUI);
			}
		}
	}

	// Token: 0x06000FDE RID: 4062 RVA: 0x0007DA14 File Offset: 0x0007BC14
	private RhythmHitResult GetNextHitResult(RhythmButtonUI nextButton, out float hitTime, float perfectZoneModifier = 1f)
	{
		float num = this.m_controller.CurrentSongTime - nextButton.HitInfo.startTime;
		hitTime = num;
		float num2 = RhythmController.LeadTime * RhythmController.PerfectZonePercent * perfectZoneModifier;
		float num3 = RhythmController.LeadTime * RhythmController.GreatZonePercent;
		float num4 = RhythmController.LeadTime * RhythmController.GoodZonePercent;
		RhythmHitResult result = RhythmHitResult.Miss;
		if (num >= -num2 && num <= num2)
		{
			result = RhythmHitResult.Perfect;
		}
		else if (num >= -num3 && num <= num3)
		{
			result = RhythmHitResult.Great;
		}
		else if (num >= -num4 && num <= num4)
		{
			result = RhythmHitResult.Good;
		}
		return result;
	}

	// Token: 0x06000FDF RID: 4063 RVA: 0x0007DA90 File Offset: 0x0007BC90
	public void SetInputType(int trackIndex, RhythmInputType inputType)
	{
		if (trackIndex < 0 || trackIndex >= this.m_tracks.Count)
		{
			return;
		}
		RhythmTrack rhythmTrack = this.m_tracks[trackIndex];
		rhythmTrack.inputType = inputType;
		for (int i = rhythmTrack.activeButtons.Count - 1; i >= 0; i--)
		{
			rhythmTrack.activeButtons[i].SetInputType(inputType);
		}
	}

	// Token: 0x06000FE0 RID: 4064 RVA: 0x0007DAF0 File Offset: 0x0007BCF0
	public void OnDrawGizmos()
	{
		foreach (Transform tr in this.m_trackPanels)
		{
			this.DrawPanelGizmos(tr);
		}
	}

	// Token: 0x06000FE1 RID: 4065 RVA: 0x0007DB20 File Offset: 0x0007BD20
	private void DrawPanelGizmos(Transform tr)
	{
		Vector3 position = tr.Find("SpawnPoint").position;
		Vector3 position2 = tr.Find("HitPoint").position;
		float d = Vector3.Distance(position, position2);
		Gizmos.color = Color.yellow;
		Gizmos.DrawLine(position, position2);
		Vector3 a = position2 + Vector3.up * 20f;
		Vector3 a2 = position2 - Vector3.up * 20f;
		Gizmos.color = Color.red;
		Vector3 b = Vector3.right * d * RhythmController.PerfectZonePercent;
		Gizmos.DrawLine(a + b, a2 + b);
		Gizmos.DrawLine(a - b, a2 - b);
		Gizmos.color = Color.blue;
		b = Vector3.right * d * RhythmController.GreatZonePercent;
		Gizmos.DrawLine(a + b, a2 + b);
		Gizmos.DrawLine(a - b, a2 - b);
		Gizmos.color = Color.green;
		b = Vector3.right * d * RhythmController.GoodZonePercent;
		Gizmos.DrawLine(a + b, a2 + b);
		Gizmos.DrawLine(a - b, a2 - b);
	}

	// Token: 0x0400100B RID: 4107
	[SerializeField]
	public Transform[] m_trackPanels;

	// Token: 0x0400100C RID: 4108
	[SerializeField]
	public GameObject m_hitButtonUI;

	// Token: 0x0400100D RID: 4109
	[SerializeField]
	public AudioClip m_hitClip;

	// Token: 0x0400100E RID: 4110
	private List<RhythmTrack> m_tracks = new List<RhythmTrack>();

	// Token: 0x0400100F RID: 4111
	private RhythmController m_controller;

	// Token: 0x04001010 RID: 4112
	private int m_btnIndex;
}
