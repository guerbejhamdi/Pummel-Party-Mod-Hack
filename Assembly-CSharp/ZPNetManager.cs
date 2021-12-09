using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using ZP.Net;

// Token: 0x0200048A RID: 1162
public class ZPNetManager : MonoBehaviour
{
	// Token: 0x06001F45 RID: 8005 RVA: 0x00016F33 File Offset: 0x00015133
	private void Awake()
	{
		this.InitializeNetSystem();
		ZPNetManager.Instance = this;
	}

	// Token: 0x06001F46 RID: 8006 RVA: 0x0000398C File Offset: 0x00001B8C
	public void AttemptPortForward(int port)
	{
	}

	// Token: 0x06001F47 RID: 8007 RVA: 0x00016F41 File Offset: 0x00015141
	private void InitializeNetSystem()
	{
		Debug.Log("ZPNetManager.Awake!");
		NetSystem.Initialize();
		NetSystem.Reset();
	}

	// Token: 0x06001F48 RID: 8008 RVA: 0x00016F57 File Offset: 0x00015157
	private void OnEnable()
	{
		SceneManager.sceneLoaded += this.LevelWasLoaded;
	}

	// Token: 0x06001F49 RID: 8009 RVA: 0x00016F6A File Offset: 0x0001516A
	private void OnDisable()
	{
		SceneManager.sceneLoaded -= this.LevelWasLoaded;
	}

	// Token: 0x06001F4A RID: 8010 RVA: 0x00016F7D File Offset: 0x0001517D
	private void LevelWasLoaded(Scene scene, LoadSceneMode mode)
	{
		this.loaded = false;
		Debug.Log("Loading Scene: " + scene.name);
	}

	// Token: 0x06001F4B RID: 8011 RVA: 0x00016F9C File Offset: 0x0001519C
	private IEnumerator Start()
	{
		Debug.Log("ZPNetManager.Start!");
		for (;;)
		{
			yield return null;
			this.Recieve();
		}
		yield break;
	}

	// Token: 0x06001F4C RID: 8012 RVA: 0x0000398C File Offset: 0x00001B8C
	private void Update()
	{
	}

	// Token: 0x06001F4D RID: 8013 RVA: 0x00016FAB File Offset: 0x000151AB
	private void LateUpdate()
	{
		if (!this.loaded)
		{
			NetSystem.FinishedLoading();
			this.loaded = true;
		}
		NetSystem.Update();
		NetSystem.Send();
	}

	// Token: 0x06001F4E RID: 8014 RVA: 0x0000398C File Offset: 0x00001B8C
	private void FixedUpdate()
	{
	}

	// Token: 0x06001F4F RID: 8015 RVA: 0x00016FCB File Offset: 0x000151CB
	private void OnApplicationQuit()
	{
		NetSystem.Destroy();
	}

	// Token: 0x06001F50 RID: 8016 RVA: 0x000C7938 File Offset: 0x000C5B38
	private void Recieve()
	{
		NetSystem.Recieve();
		NetSystem.IncrementTime();
	}

	// Token: 0x06001F51 RID: 8017 RVA: 0x00016FD2 File Offset: 0x000151D2
	public IEnumerator WaitForServerCreation(UnityAction<string> callback)
	{
		Debug.Log("WaitForServerCreation");
		yield return new WaitUntil(() => NetGameServer.finishedServerCreation);
		if (NetGameServer.outcome == "")
		{
			Debug.Log("Server Creation Outcome = OK");
			NetSystem.OnConnectedToLobby();
			yield return null;
			NetSystem.OnHostConnected(NetSystem.MyPlayer);
		}
		else
		{
			Debug.LogError("Server Creation Outcome = BAD - " + NetGameServer.outcome);
		}
		yield return null;
		callback(NetGameServer.outcome);
		yield break;
	}

	// Token: 0x0400222F RID: 8751
	public bool LoadLevel;

	// Token: 0x04002230 RID: 8752
	public string LevelName = "";

	// Token: 0x04002231 RID: 8753
	public int ServerSendRate = 40;

	// Token: 0x04002232 RID: 8754
	public int ClientSendRate = 40;

	// Token: 0x04002233 RID: 8755
	public int ClientCommandRate = 30;

	// Token: 0x04002234 RID: 8756
	public float InterpolationBackTime = 0.1f;

	// Token: 0x04002235 RID: 8757
	private bool loaded;

	// Token: 0x04002236 RID: 8758
	public static ZPNetManager Instance;
}
