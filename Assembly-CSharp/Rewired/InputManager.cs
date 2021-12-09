using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using Rewired.Platforms;
using Rewired.Utils;
using Rewired.Utils.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Rewired
{
	// Token: 0x02000647 RID: 1607
	[AddComponentMenu("Rewired/Input Manager")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public sealed class InputManager : InputManager_Base
	{
		// Token: 0x06002BFB RID: 11259 RVA: 0x0001DCBF File Offset: 0x0001BEBF
		protected override void OnInitialized()
		{
			this.SubscribeEvents();
		}

		// Token: 0x06002BFC RID: 11260 RVA: 0x0001DCC7 File Offset: 0x0001BEC7
		protected override void OnDeinitialized()
		{
			this.UnsubscribeEvents();
		}

		// Token: 0x06002BFD RID: 11261 RVA: 0x000F6F90 File Offset: 0x000F5190
		protected override void DetectPlatform()
		{
			this.scriptingBackend = ScriptingBackend.Mono;
			this.scriptingAPILevel = ScriptingAPILevel.Net20;
			this.editorPlatform = EditorPlatform.None;
			this.platform = Platform.Unknown;
			this.webplayerPlatform = WebplayerPlatform.None;
			this.isEditor = false;
			if (SystemInfo.deviceName == null)
			{
				string empty = string.Empty;
			}
			if (SystemInfo.deviceModel == null)
			{
				string empty2 = string.Empty;
			}
			this.platform = Platform.Windows;
			this.scriptingBackend = ScriptingBackend.Mono;
			this.scriptingAPILevel = ScriptingAPILevel.Net46;
		}

		// Token: 0x06002BFE RID: 11262 RVA: 0x0000398C File Offset: 0x00001B8C
		protected override void CheckRecompile()
		{
		}

		// Token: 0x06002BFF RID: 11263 RVA: 0x0001DCCF File Offset: 0x0001BECF
		protected override IExternalTools GetExternalTools()
		{
			return new ExternalTools();
		}

		// Token: 0x06002C00 RID: 11264 RVA: 0x0001DCD6 File Offset: 0x0001BED6
		private bool CheckDeviceName(string searchPattern, string deviceName, string deviceModel)
		{
			return Regex.IsMatch(deviceName, searchPattern, RegexOptions.IgnoreCase) || Regex.IsMatch(deviceModel, searchPattern, RegexOptions.IgnoreCase);
		}

		// Token: 0x06002C01 RID: 11265 RVA: 0x0001DCEC File Offset: 0x0001BEEC
		private void SubscribeEvents()
		{
			this.UnsubscribeEvents();
			SceneManager.sceneLoaded += this.OnSceneLoaded;
		}

		// Token: 0x06002C02 RID: 11266 RVA: 0x0001DD05 File Offset: 0x0001BF05
		private void UnsubscribeEvents()
		{
			SceneManager.sceneLoaded -= this.OnSceneLoaded;
		}

		// Token: 0x06002C03 RID: 11267 RVA: 0x0001DD18 File Offset: 0x0001BF18
		private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			base.OnSceneLoaded();
		}

		// Token: 0x04002DE1 RID: 11745
		private bool ignoreRecompile;
	}
}
