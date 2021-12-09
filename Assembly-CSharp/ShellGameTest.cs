using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200049E RID: 1182
public class ShellGameTest : MonoBehaviour
{
	// Token: 0x06001F96 RID: 8086 RVA: 0x000172C1 File Offset: 0x000154C1
	private void Start()
	{
		this.curSwapTime = this.swapTime;
	}

	// Token: 0x06001F97 RID: 8087 RVA: 0x000172CF File Offset: 0x000154CF
	public void DoShellGame(ActionShellGame _shellGame)
	{
		this.m_shellGame = _shellGame;
		base.StartCoroutine(this.StartShellGame());
	}

	// Token: 0x06001F98 RID: 8088 RVA: 0x000172E5 File Offset: 0x000154E5
	private IEnumerator StartShellGame()
	{
		foreach (FakeChestController fakeChestController in this.m_activeChests)
		{
			UnityEngine.Object.Destroy(fakeChestController.gameObject);
		}
		this.m_activeChests.Clear();
		AudioSystem.PlayOneShot(this.spawnClip, this.spawnVolume, 0f, 1f);
		foreach (GameObject gameObject in this.shells)
		{
			GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.m_shellGameChest, gameObject.transform);
			FakeChestController component = gameObject2.GetComponent<FakeChestController>();
			this.m_activeChests.Add(component);
			gameObject2.transform.localScale = Vector3.zero;
			gameObject2.transform.localPosition = Vector3.zero;
			LeanTween.scale(gameObject2, Vector3.one * 1.5f, 0.25f).setEaseOutBack();
			yield return new WaitForSeconds(0.1f);
		}
		GameObject[] array = null;
		this.m_activeChests[this.m_shellGame.RealChestIndex].SetRealChest(true, false);
		yield return new WaitForSeconds(1f);
		AudioSystem.PlayOneShot(this.openClip, this.openVolume, 0f, 1f);
		foreach (FakeChestController fakeChestController2 in this.m_activeChests)
		{
			fakeChestController2.OpenAnim();
		}
		yield return new WaitForSeconds(0.75f);
		yield return new WaitForSeconds(2f);
		AudioSystem.PlayOneShot(this.closeClip, this.closeVolume, 0f, 1f);
		foreach (FakeChestController fakeChestController3 in this.m_activeChests)
		{
			fakeChestController3.CloseAnim();
		}
		yield return new WaitForSeconds(0.75f);
		int num;
		for (int i = 0; i < this.swaps; i = num)
		{
			yield return base.StartCoroutine(this.Swap());
			this.curSwapTime *= 0.95f;
			num = i + 1;
		}
		this.curSwapTime = this.swapTime;
		yield return new WaitForSeconds(1f);
		AudioSystem.PlayOneShot(this.coloringClip, this.coloringClipVolume, 0f, 1f);
		List<int> list = new List<int>
		{
			0,
			1,
			2
		};
		for (int j = 0; j < this.m_activeChests.Count; j++)
		{
			int index = this.m_shellGame.Rand.Next(0, list.Count);
			int num2 = list[index];
			this.chestColors[j] = num2;
			this.m_activeChests[j].SetChestIndex(num2);
			list.RemoveAt(index);
		}
		yield return new WaitForSeconds(5f);
		foreach (FakeChestController fakeChestController4 in this.m_activeChests)
		{
			fakeChestController4.DespawnAnim();
			yield return new WaitForSeconds(0.1f);
		}
		List<FakeChestController>.Enumerator enumerator2 = default(List<FakeChestController>.Enumerator);
		yield return new WaitForSeconds(1f);
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
		yield break;
	}

	// Token: 0x06001F99 RID: 8089 RVA: 0x000172F4 File Offset: 0x000154F4
	private IEnumerator Swap()
	{
		AudioSystem.PlayOneShot(this.swapClip, this.swapVolume, 0f, 1f);
		List<int> list = new List<int>
		{
			0,
			1,
			2
		};
		int index = this.m_shellGame.Rand.Next(0, list.Count);
		int shell = list[index];
		list.RemoveAt(index);
		int index2 = this.m_shellGame.Rand.Next(0, list.Count);
		int shell2 = list[index2];
		float startTime = Time.time;
		Vector3 target = this.shells[shell].transform.position;
		Vector3 target2 = this.shells[shell2].transform.position;
		while (Time.time - startTime < this.curSwapTime)
		{
			float t = (Time.time - startTime) / this.curSwapTime;
			this.SetPosition(shell, shell2, target, target2, t);
			yield return null;
		}
		this.SetPosition(shell, shell2, target, target2, 1f);
		yield break;
	}

	// Token: 0x06001F9A RID: 8090 RVA: 0x000C83B8 File Offset: 0x000C65B8
	private void SetPosition(int shell1, int shell2, Vector3 target1, Vector3 target2, float t)
	{
		this.shells[shell1].transform.position = Vector3.Lerp(target1, target2, this.movementCurve.Evaluate(t)) + this.offsetCurve.Evaluate(t) * this.axisOffset;
		this.shells[shell2].transform.position = Vector3.Lerp(target2, target1, this.movementCurve.Evaluate(t)) - this.offsetCurve.Evaluate(t) * this.axisOffset;
	}

	// Token: 0x0400226B RID: 8811
	[SerializeField]
	private GameObject m_shellGameChest;

	// Token: 0x0400226C RID: 8812
	public GameObject[] shells;

	// Token: 0x0400226D RID: 8813
	public Vector3 axisOffset = new Vector3(1f, 0f, 0f);

	// Token: 0x0400226E RID: 8814
	public AnimationCurve offsetCurve;

	// Token: 0x0400226F RID: 8815
	public AnimationCurve movementCurve;

	// Token: 0x04002270 RID: 8816
	public float swapTime = 0.5f;

	// Token: 0x04002271 RID: 8817
	public int swaps = 20;

	// Token: 0x04002272 RID: 8818
	[Header("Audio")]
	public AudioClip spawnClip;

	// Token: 0x04002273 RID: 8819
	public float spawnVolume = 1f;

	// Token: 0x04002274 RID: 8820
	public AudioClip swapClip;

	// Token: 0x04002275 RID: 8821
	public float swapVolume = 1f;

	// Token: 0x04002276 RID: 8822
	public AudioClip openClip;

	// Token: 0x04002277 RID: 8823
	public float openVolume = 1f;

	// Token: 0x04002278 RID: 8824
	public AudioClip closeClip;

	// Token: 0x04002279 RID: 8825
	public float closeVolume = 1f;

	// Token: 0x0400227A RID: 8826
	public AudioClip coloringClip;

	// Token: 0x0400227B RID: 8827
	public float coloringClipVolume = 1f;

	// Token: 0x0400227C RID: 8828
	private float curSwapTime;

	// Token: 0x0400227D RID: 8829
	private List<FakeChestController> m_activeChests = new List<FakeChestController>();

	// Token: 0x0400227E RID: 8830
	private ActionShellGame m_shellGame;

	// Token: 0x0400227F RID: 8831
	public int[] chestColors = new int[]
	{
		0,
		1,
		2
	};
}
