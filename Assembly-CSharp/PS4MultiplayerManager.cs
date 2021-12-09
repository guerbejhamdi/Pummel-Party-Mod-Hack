using System;
using I2.Loc;
using Sony.NP;
using UnityEngine;

// Token: 0x020002C8 RID: 712
public class PS4MultiplayerManager : PlatformMultiplayerManager
{
	// Token: 0x06001455 RID: 5205 RVA: 0x0000FE86 File Offset: 0x0000E086
	public override void CheckMultiplayerAvailability(IPlatformUser user, MultiplayerAvailabilityResult callback)
	{
		this.curCallback = callback;
		this.CheckPlus(true, user);
	}

	// Token: 0x06001456 RID: 5206 RVA: 0x00098B68 File Offset: 0x00096D68
	public void CheckPlus(bool async, IPlatformUser user)
	{
		try
		{
			NpUtils.CheckPlusRequest checkPlusRequest = new NpUtils.CheckPlusRequest();
			checkPlusRequest.UserId = user.UserID;
			checkPlusRequest.Async = async;
			NpUtils.CheckPlusResponse checkPlusResponse = new NpUtils.CheckPlusResponse();
			int num = NpUtils.CheckPlus(checkPlusRequest, checkPlusResponse);
			if (async)
			{
				string str = "CheckPlus Async : Request Id = ";
				uint num2 = (uint)num;
				Debug.Log(str + num2.ToString());
			}
			else
			{
				Debug.Log("CheckPlus Synchronous : Return code = (0x" + num.ToString("X8") + ")");
				if (checkPlusResponse.ReturnCodeValue < 0)
				{
					Debug.LogError("Response : " + checkPlusResponse.ConvertReturnCodeToString(checkPlusRequest.FunctionType));
				}
				else
				{
					Debug.Log("Response : " + checkPlusResponse.ConvertReturnCodeToString(checkPlusRequest.FunctionType));
				}
				this.OutputCheckPlus(checkPlusResponse);
			}
		}
		catch (NpToolkitException ex)
		{
			Debug.LogError("Exception : " + ex.ExtendedMessage);
		}
	}

	// Token: 0x06001457 RID: 5207 RVA: 0x0000FE97 File Offset: 0x0000E097
	public override void CreateMultiplayerServiceSession(IPlatformUser user, CreateSessionResult callback)
	{
		if (callback != null)
		{
			callback(CreateSessionResultType.Success);
		}
	}

	// Token: 0x06001458 RID: 5208 RVA: 0x00098C54 File Offset: 0x00096E54
	public void OutputCheckPlus(NpUtils.CheckPlusResponse response)
	{
		if (response == null)
		{
			return;
		}
		Debug.LogError("CheckPlus Response: " + response.ReturnCode.ToString());
		if (!response.Locked)
		{
			if (this.curCallback != null)
			{
				if (response.Authorized)
				{
					this.curCallback(MultiplayerAvailabilityResultType.Available, "No Issue");
				}
				else
				{
					this.DisplayJoinPlusDialog();
					this.curCallback(MultiplayerAvailabilityResultType.Unavailable, LocalizationManager.GetTranslation("MultiplayerPrevented", true, 0, true, false, null, null, true));
				}
			}
			Debug.Log("Authorized : " + response.Authorized.ToString());
		}
	}

	// Token: 0x06001459 RID: 5209 RVA: 0x00098CF4 File Offset: 0x00096EF4
	public void DisplayJoinPlusDialog()
	{
		try
		{
			Commerce.DisplayJoinPlusDialogRequest request = new Commerce.DisplayJoinPlusDialogRequest();
			Core.EmptyResponse response = new Core.EmptyResponse();
			Debug.Log("DisplayJoinPlusDialog Async : Request Id = " + Commerce.DisplayJoinPlusDialog(request, response).ToString());
		}
		catch (NpToolkitException ex)
		{
			Debug.LogError("Exception : " + ex.ExtendedMessage);
		}
	}

	// Token: 0x0600145A RID: 5210 RVA: 0x00005651 File Offset: 0x00003851
	public override bool IsConnectedToMultiplayerService()
	{
		return true;
	}

	// Token: 0x040015A9 RID: 5545
	private MultiplayerAvailabilityResult curCallback;
}
