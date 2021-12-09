using System;
using Rewired;
using Steamworks;

// Token: 0x020002ED RID: 749
public class PlatformUtility
{
	// Token: 0x060014F1 RID: 5361 RVA: 0x00098FA8 File Offset: 0x000971A8
	public static string GetUsername()
	{
		string text = "SteamPlayer";
		string @string = RBPrefs.GetString("steam_name", "SteamPlayer");
		if (SteamManager.Initialized && SteamFriends.GetPersonaName() != @string)
		{
			RBPrefs.SetString("steam_name", SteamFriends.GetPersonaName());
			text = SteamFriends.GetPersonaName();
		}
		else if (RBPrefs.HasKey("net_player_name"))
		{
			text = RBPrefs.GetString("net_player_name", text);
		}
		RBPrefs.SetString("net_player_name", text);
		return text;
	}

	// Token: 0x060014F2 RID: 5362 RVA: 0x0009901C File Offset: 0x0009721C
	public static Joystick GetJoystickWithControllerID(long controllerID)
	{
		foreach (Joystick joystick in ReInput.controllers.Joysticks)
		{
			if (joystick.systemId != null && controllerID == joystick.systemId.Value)
			{
				return joystick;
			}
		}
		return null;
	}

	// Token: 0x060014F3 RID: 5363 RVA: 0x00099090 File Offset: 0x00097290
	public static int GetJoystickIDControllerID(long controllerID)
	{
		Joystick joystickWithControllerID = PlatformUtility.GetJoystickWithControllerID(controllerID);
		if (joystickWithControllerID != null)
		{
			return joystickWithControllerID.id;
		}
		return 0;
	}
}
