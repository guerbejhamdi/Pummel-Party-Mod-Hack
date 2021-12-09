using System;
using System.Collections.Generic;
using Sony.NP;
using UnityEngine;

// Token: 0x020002C7 RID: 711
public class PS4EventManager : PlatformEventManager
{
	// Token: 0x0600144F RID: 5199 RVA: 0x000986BC File Offset: 0x000968BC
	public override void Initialize()
	{
		Main.OnAsyncEvent += this.Main_OnAsyncEvent;
		InitToolkit initToolkit = new InitToolkit();
		initToolkit.contentRestrictions.DefaultAgeRestriction = 0;
		initToolkit.contentRestrictions.AgeRestrictions = new AgeRestriction[]
		{
			new AgeRestriction(10, new Core.CountryCode("fr")),
			new AgeRestriction(15, new Core.CountryCode("au"))
		};
		initToolkit.threadSettings.affinity = Affinity.AllCores;
		initToolkit.memoryPools.JsonPoolSize = 6291456UL;
		InitToolkit initToolkit2 = initToolkit;
		initToolkit2.memoryPools.SslPoolSize = initToolkit2.memoryPools.SslPoolSize * 4UL;
		InitToolkit initToolkit3 = initToolkit;
		initToolkit3.memoryPools.MatchingSslPoolSize = initToolkit3.memoryPools.MatchingSslPoolSize * 4UL;
		InitToolkit initToolkit4 = initToolkit;
		initToolkit4.memoryPools.MatchingPoolSize = initToolkit4.memoryPools.MatchingPoolSize * 4UL;
		initToolkit.SetPushNotificationsFlags((PushNotificationsFlags)63);
		try
		{
			this.initResult = Main.Initialize(initToolkit);
			if (this.initResult.Initialized)
			{
				Debug.Log("NpToolkit Initialized ");
				Debug.Log("Plugin SDK Version : " + this.initResult.SceSDKVersion.ToString());
				Debug.Log("Plugin DLL Version : " + this.initResult.DllVersion.ToString());
			}
			else
			{
				Debug.Log("NpToolkit not initialized ");
			}
		}
		catch (NpToolkitException ex)
		{
			Debug.LogError("Exception During Initialization : " + ex.ExtendedMessage);
		}
	}

	// Token: 0x06001450 RID: 5200 RVA: 0x00098834 File Offset: 0x00096A34
	private void Main_OnAsyncEvent(NpCallbackEvent callbackEvent)
	{
		try
		{
			object obj = this.pendingSyncObject;
			lock (obj)
			{
				this.pendingEvents.Enqueue(callbackEvent);
			}
		}
		catch (Exception ex)
		{
			Debug.LogError("Main_OnAsyncEvent General Exception = " + ex.Message);
			Debug.LogError(ex.StackTrace);
		}
	}

	// Token: 0x06001451 RID: 5201 RVA: 0x000988AC File Offset: 0x00096AAC
	public override void Update()
	{
		object obj = this.pendingSyncObject;
		lock (obj)
		{
			if (this.pendingEvents.Count > 0)
			{
				NpCallbackEvent npCallbackEvent = this.pendingEvents.Dequeue();
				Debug.Log(string.Concat(new string[]
				{
					"Event: Service = (",
					npCallbackEvent.Service.ToString(),
					") : API Called = (",
					npCallbackEvent.ApiCalled.ToString(),
					") : Request Id = (",
					npCallbackEvent.NpRequestId.ToString(),
					") : Calling User Id = (",
					npCallbackEvent.UserId.ToString(),
					")"
				}));
				this.HandleAsynEvent(npCallbackEvent);
			}
		}
	}

	// Token: 0x06001452 RID: 5202 RVA: 0x0009899C File Offset: 0x00096B9C
	private void HandleAsynEvent(NpCallbackEvent callbackEvent)
	{
		try
		{
			if (callbackEvent.Response != null)
			{
				if (callbackEvent.Response.ReturnCodeValue < 0)
				{
					Debug.LogError("Response : " + callbackEvent.Response.ConvertReturnCodeToString(callbackEvent.ApiCalled));
				}
				else
				{
					Debug.Log("Response : " + callbackEvent.Response.ConvertReturnCodeToString(callbackEvent.ApiCalled));
				}
				if (callbackEvent.Response.HasServerError)
				{
					this.OutputSeverError(callbackEvent.Response);
				}
			}
			FunctionTypes apiCalled = callbackEvent.ApiCalled;
			if (apiCalled != FunctionTypes.NpUtilsCheckAvailability)
			{
				if (apiCalled == FunctionTypes.NpUtilsCheckPlus)
				{
					((PS4MultiplayerManager)PlatformMultiplayerManager.Instance).OutputCheckPlus(callbackEvent.Response as NpUtils.CheckPlusResponse);
				}
				else
				{
					Debug.LogError("Callback not implemented: " + callbackEvent.Service.ToString() + " : " + callbackEvent.ApiCalled.ToString());
				}
			}
		}
		catch (NpToolkitException ex)
		{
			Debug.LogError("Main_OnAsyncEvent NpToolkit Exception = " + ex.ExtendedMessage);
			Debug.LogError(ex.ExtendedMessage);
			Debug.LogError(ex.StackTrace);
		}
		catch (Exception ex2)
		{
			Debug.LogError("Main_OnAsyncEvent General Exception = " + ex2.Message);
			Debug.LogError(ex2.StackTrace);
		}
	}

	// Token: 0x06001453 RID: 5203 RVA: 0x00098AF8 File Offset: 0x00096CF8
	private void OutputSeverError(ResponseBase response)
	{
		if (response == null)
		{
			return;
		}
		if (response.HasServerError)
		{
			Debug.LogError(string.Format("Server Error : returnCode = (0x{0:X}) : webApiNextAvailableTime = ({1}) : httpStatusCode = ({2})", response.ReturnCode, response.ServerError.WebApiNextAvailableTime, response.ServerError.HttpStatusCode));
			Debug.LogError("Server Error : jsonData = " + response.ServerError.JsonData);
		}
	}

	// Token: 0x040015A6 RID: 5542
	public InitResult initResult;

	// Token: 0x040015A7 RID: 5543
	private Queue<NpCallbackEvent> pendingEvents = new Queue<NpCallbackEvent>();

	// Token: 0x040015A8 RID: 5544
	private object pendingSyncObject = new object();
}
