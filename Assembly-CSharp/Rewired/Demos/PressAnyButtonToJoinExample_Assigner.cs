using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rewired.Demos
{
	// Token: 0x020006D2 RID: 1746
	[AddComponentMenu("")]
	public class PressAnyButtonToJoinExample_Assigner : MonoBehaviour
	{
		// Token: 0x0600330C RID: 13068 RVA: 0x00022B69 File Offset: 0x00020D69
		private void Update()
		{
			if (!ReInput.isReady)
			{
				return;
			}
			this.AssignJoysticksToPlayers();
		}

		// Token: 0x0600330D RID: 13069 RVA: 0x0010970C File Offset: 0x0010790C
		private void AssignJoysticksToPlayers()
		{
			IList<Joystick> joysticks = ReInput.controllers.Joysticks;
			for (int i = 0; i < joysticks.Count; i++)
			{
				Joystick joystick = joysticks[i];
				if (!ReInput.controllers.IsControllerAssigned(joystick.type, joystick.id) && joystick.GetAnyButtonDown())
				{
					Player player = this.FindPlayerWithoutJoystick();
					if (player == null)
					{
						return;
					}
					player.controllers.AddController(joystick, false);
				}
			}
			if (this.DoAllPlayersHaveJoysticks())
			{
				ReInput.configuration.autoAssignJoysticks = true;
				base.enabled = false;
			}
		}

		// Token: 0x0600330E RID: 13070 RVA: 0x00109790 File Offset: 0x00107990
		private Player FindPlayerWithoutJoystick()
		{
			IList<Player> players = ReInput.players.Players;
			for (int i = 0; i < players.Count; i++)
			{
				if (players[i].controllers.joystickCount <= 0)
				{
					return players[i];
				}
			}
			return null;
		}

		// Token: 0x0600330F RID: 13071 RVA: 0x00022B79 File Offset: 0x00020D79
		private bool DoAllPlayersHaveJoysticks()
		{
			return this.FindPlayerWithoutJoystick() == null;
		}
	}
}
