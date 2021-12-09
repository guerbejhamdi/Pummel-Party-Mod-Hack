using System;
using UnityEngine;

// Token: 0x020002E7 RID: 743
public interface IPlatformUser
{
	// Token: 0x060014D3 RID: 5331
	string GetProfileName();

	// Token: 0x060014D4 RID: 5332
	Texture2D GetProfilePicture();

	// Token: 0x170001C9 RID: 457
	// (get) Token: 0x060014D5 RID: 5333
	ulong ControllerID { get; }

	// Token: 0x170001CA RID: 458
	// (get) Token: 0x060014D6 RID: 5334
	int RewiredJoystickID { get; }

	// Token: 0x170001CB RID: 459
	// (get) Token: 0x060014D7 RID: 5335
	bool IsSignedIn { get; }

	// Token: 0x170001CC RID: 460
	// (get) Token: 0x060014D8 RID: 5336
	int UserID { get; }
}
