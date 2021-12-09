using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using ZP.Net;
using ZP.Utility;

// Token: 0x02000260 RID: 608
public class SpellingController : MinigameController
{
	// Token: 0x060011C3 RID: 4547 RVA: 0x0000E7F6 File Offset: 0x0000C9F6
	private void FindLetterButtonController()
	{
		if (this.letterButtonController == null)
		{
			this.letterButtonController = UnityEngine.Object.FindObjectOfType<LetterButtonController>();
		}
	}

	// Token: 0x060011C4 RID: 4548 RVA: 0x0000E811 File Offset: 0x0000CA11
	public void Awake()
	{
		this.FindLetterButtonController();
		this.SetupWords();
	}

	// Token: 0x060011C5 RID: 4549 RVA: 0x0000E81F File Offset: 0x0000CA1F
	public SpellingButton GetSpellingButton(int letterIndex)
	{
		if (this.letterButtonController == null)
		{
			this.letterButtonController = UnityEngine.Object.FindObjectOfType<LetterButtonController>();
		}
		if (this.letterButtonController != null)
		{
			return this.letterButtonController.GetLetter(letterIndex);
		}
		return null;
	}

	// Token: 0x060011C6 RID: 4550 RVA: 0x0008A280 File Offset: 0x00088480
	private void SetupWords()
	{
		if (this.m_allWords != null)
		{
			return;
		}
		string[] separator = new string[]
		{
			"\n"
		};
		List<string> list = new List<string>(this.m_words.Split(separator, StringSplitOptions.RemoveEmptyEntries));
		List<string> list2 = new List<string>(this.m_fWords.Split(separator, StringSplitOptions.RemoveEmptyEntries));
		this.m_allWords = new List<string>();
		this.m_wordMap = new Dictionary<string, int>();
		this.m_wordIndexMap = new Dictionary<int, string>();
		int num = 0;
		foreach (string text in list)
		{
			this.m_allWords.Add(text);
			this.m_wordMap.Add(text, num);
			this.m_wordIndexMap.Add(num, text);
			num++;
		}
		foreach (string text2 in list2)
		{
			this.m_allWords.Add(text2);
			this.m_wordMap.Add(text2, num);
			this.m_wordIndexMap.Add(num, text2);
			num++;
		}
		if (NetSystem.IsServer)
		{
			while (list.Count > 0 || list2.Count > 0)
			{
				if (list2.Count > 0)
				{
					int index = UnityEngine.Random.Range(0, list2.Count);
					this.m_randomWordList.Add(list2[index]);
					list2.RemoveAt(index);
				}
				if (list.Count > 0)
				{
					int index2 = UnityEngine.Random.Range(0, list.Count);
					this.m_randomWordList.Add(list[index2]);
					list.RemoveAt(index2);
				}
			}
		}
	}

	// Token: 0x060011C7 RID: 4551 RVA: 0x0008A43C File Offset: 0x0008863C
	private void SetNextActiveWord()
	{
		if (!NetSystem.IsServer)
		{
			return;
		}
		string activeWord = this.m_randomWordList[this.m_nextWordIndex];
		this.SetActiveWord(activeWord);
		this.m_nextWordIndex++;
	}

	// Token: 0x060011C8 RID: 4552 RVA: 0x0008A478 File Offset: 0x00088678
	private void SetActiveWord(string word)
	{
		this.m_curWord = word;
		if (NetSystem.IsServer)
		{
			short num = (short)this.m_wordMap[word];
			this.m_curWordIndex = (int)num;
			base.SendRPC("SetActiveWordRPC", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				num
			});
		}
		for (int i = 0; i < this.players.Count; i++)
		{
			((SpellingPlayer)this.players[i]).SetWord(word);
		}
	}

	// Token: 0x060011C9 RID: 4553 RVA: 0x0008A4F0 File Offset: 0x000886F0
	public void CompletedWord(string word)
	{
		if (NetSystem.IsServer)
		{
			if (word == this.m_curWord)
			{
				this.SetNextActiveWord();
				return;
			}
		}
		else
		{
			short num = (short)this.m_wordMap[word];
			base.SendRPC("CompletedWordRPC", NetRPCDelivery.RELIABLE_ORDERED, new object[]
			{
				num
			});
		}
	}

	// Token: 0x060011CA RID: 4554 RVA: 0x0008A544 File Offset: 0x00088744
	[NetRPC(false, NetRPCSecurity.ALL, NetRPCSecurity.SERVER)]
	public void CompletedWordRPC(NetPlayer sender, short wordIndex)
	{
		string word = "";
		if (this.m_wordIndexMap.TryGetValue((int)wordIndex, out word))
		{
			this.CompletedWord(word);
		}
	}

	// Token: 0x060011CB RID: 4555 RVA: 0x0008A570 File Offset: 0x00088770
	public Vector3 GetRandomNavMeshPoint()
	{
		if (this.binaryTree == null)
		{
			this.triangulation = NavMesh.CalculateTriangulation();
			if (this.triangulation.vertices.Length != 0)
			{
				List<float> list = new List<float>();
				for (int i = 0; i < this.triangulation.indices.Length / 3; i++)
				{
					int num = i * 3;
					Vector3 vector = this.triangulation.vertices[this.triangulation.indices[num]];
					Vector3 vector2 = this.triangulation.vertices[this.triangulation.indices[num + 1]];
					Vector3 vector3 = this.triangulation.vertices[this.triangulation.indices[num + 2]];
					float num2 = Vector3.Distance(vector, vector2);
					float num3 = Vector3.Distance(vector2, vector3);
					float num4 = Vector3.Distance(vector3, vector);
					float num5 = (num2 + num3 + num4) / 2f;
					float num6 = Mathf.Sqrt(num5 * (num5 - num2) * (num5 - num3) * (num5 - num4));
					list.Add(this.totalArea);
					this.totalArea += num6;
				}
				this.binaryTree = new BinaryTree(list.ToArray());
			}
		}
		if (this.binaryTree != null)
		{
			float p = ZPMath.RandomFloat(this.rand, 0f, this.totalArea);
			int num7 = this.binaryTree.FindPoint(p) * 3;
			Vector3[] vertices = this.triangulation.vertices;
			int[] indices = this.triangulation.indices;
			return ZPMath.RandomTrianglePoint(vertices[indices[num7]], vertices[indices[num7 + 1]], vertices[indices[num7 + 2]], this.rand);
		}
		return Vector3.zero;
	}

	// Token: 0x060011CC RID: 4556 RVA: 0x0008A728 File Offset: 0x00088928
	public override void InitializeMinigame()
	{
		base.InitializeMinigame();
		this.rand = new System.Random(UnityEngine.Random.Range(0, int.MaxValue));
		if (NetSystem.IsServer)
		{
			base.SpawnPlayers("SpellingPlayer", null);
		}
		this.triangulation = NavMesh.CalculateTriangulation();
		this.FindLetterButtonController();
		this.SetupWords();
	}

	// Token: 0x060011CD RID: 4557 RVA: 0x0008A77C File Offset: 0x0008897C
	public override void StartMinigame()
	{
		if (NetSystem.IsServer)
		{
			this.SetNextActiveWord();
		}
		this.FindLetterButtonController();
		base.CreateScoreUI(UIAnchorType.TopLeft, new Vector2(145f, 45f), 68f, false);
		base.CreateTimer(UIAnchorType.TopRight, new Vector2(75f, 75f), this.round_length);
		base.StartMinigame();
	}

	// Token: 0x060011CE RID: 4558 RVA: 0x0008A7DC File Offset: 0x000889DC
	public override void RoundEnded()
	{
		if (NetSystem.IsServer)
		{
			for (int i = 0; i < this.players.Count; i++)
			{
				if (!((SpellingPlayer)this.players[i]).IsDead)
				{
					CharacterBase characterBase = this.players[i];
					characterBase.Score += (short)(this.players.Count * 25);
				}
				else
				{
					CharacterBase characterBase2 = this.players[i];
					characterBase2.Score += this.players[i].RoundScore * 25;
					this.players[i].RoundScore = 0;
				}
			}
		}
		base.RoundEnded();
	}

	// Token: 0x060011CF RID: 4559 RVA: 0x00009C2C File Offset: 0x00007E2C
	public override void ResetRound()
	{
		base.ResetRound();
	}

	// Token: 0x060011D0 RID: 4560 RVA: 0x00009C34 File Offset: 0x00007E34
	public override void RoundStarting()
	{
		base.RoundStarting();
	}

	// Token: 0x060011D1 RID: 4561 RVA: 0x00009C3C File Offset: 0x00007E3C
	public override void StartNewRound()
	{
		base.StartNewRound();
	}

	// Token: 0x060011D2 RID: 4562 RVA: 0x0000E856 File Offset: 0x0000CA56
	private void Update()
	{
		if (base.State == MinigameControllerState.Playing && NetSystem.IsServer && base.State == MinigameControllerState.Playing && this.ui_timer.time_test <= 0f)
		{
			base.EndRound(2.5f, 2f, false);
		}
	}

	// Token: 0x060011D3 RID: 4563 RVA: 0x00009C44 File Offset: 0x00007E44
	public override void ReleaseMinigame()
	{
		base.ReleaseMinigame();
	}

	// Token: 0x060011D4 RID: 4564 RVA: 0x00009C4C File Offset: 0x00007E4C
	public override bool HasLoadedLocally()
	{
		return base.HasLoadedLocally();
	}

	// Token: 0x060011D5 RID: 4565 RVA: 0x0008A898 File Offset: 0x00088A98
	[NetRPC(false, NetRPCSecurity.SERVER, NetRPCSecurity.ALL)]
	public void SetActiveWordRPC(NetPlayer sender, short wordIndex)
	{
		string activeWord = "MISSING";
		if (!this.m_wordIndexMap.TryGetValue((int)wordIndex, out activeWord))
		{
			Debug.LogError("Unable to find word with index = " + wordIndex.ToString());
		}
		this.SetActiveWord(activeWord);
	}

	// Token: 0x04001279 RID: 4729
	[SerializeField]
	[TextArea]
	private string m_words;

	// Token: 0x0400127A RID: 4730
	[SerializeField]
	[TextArea]
	private string m_fWords;

	// Token: 0x0400127B RID: 4731
	private System.Random rand;

	// Token: 0x0400127C RID: 4732
	private NavMeshTriangulation triangulation;

	// Token: 0x0400127D RID: 4733
	private BinaryTree binaryTree;

	// Token: 0x0400127E RID: 4734
	private float totalArea;

	// Token: 0x0400127F RID: 4735
	private List<string> m_allWords;

	// Token: 0x04001280 RID: 4736
	private List<int> m_randomWords;

	// Token: 0x04001281 RID: 4737
	private Dictionary<string, int> m_wordMap;

	// Token: 0x04001282 RID: 4738
	private Dictionary<int, string> m_wordIndexMap;

	// Token: 0x04001283 RID: 4739
	private List<string> m_randomWordList = new List<string>();

	// Token: 0x04001284 RID: 4740
	private int m_nextWordIndex;

	// Token: 0x04001285 RID: 4741
	private int m_curWordIndex;

	// Token: 0x04001286 RID: 4742
	private string m_curWord = "";

	// Token: 0x04001287 RID: 4743
	private LetterButtonController letterButtonController;
}
