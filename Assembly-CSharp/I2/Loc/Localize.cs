using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace I2.Loc
{
	// Token: 0x02000813 RID: 2067
	[AddComponentMenu("I2/Localization/I2 Localize")]
	public class Localize : MonoBehaviour
	{
		// Token: 0x17000A2B RID: 2603
		// (get) Token: 0x06003A9B RID: 15003 RVA: 0x000278E4 File Offset: 0x00025AE4
		// (set) Token: 0x06003A9C RID: 15004 RVA: 0x000278EC File Offset: 0x00025AEC
		public string Term
		{
			get
			{
				return this.mTerm;
			}
			set
			{
				this.SetTerm(value);
			}
		}

		// Token: 0x17000A2C RID: 2604
		// (get) Token: 0x06003A9D RID: 15005 RVA: 0x000278F5 File Offset: 0x00025AF5
		// (set) Token: 0x06003A9E RID: 15006 RVA: 0x000278FD File Offset: 0x00025AFD
		public string SecondaryTerm
		{
			get
			{
				return this.mTermSecondary;
			}
			set
			{
				this.SetTerm(null, value);
			}
		}

		// Token: 0x06003A9F RID: 15007 RVA: 0x00027907 File Offset: 0x00025B07
		private void Awake()
		{
			this.UpdateAssetDictionary();
			this.FindTarget();
			if (this.LocalizeOnAwake)
			{
				this.OnLocalize(false);
			}
		}

		// Token: 0x06003AA0 RID: 15008 RVA: 0x00027925 File Offset: 0x00025B25
		private void OnEnable()
		{
			this.OnLocalize(false);
		}

		// Token: 0x06003AA1 RID: 15009 RVA: 0x0002792E File Offset: 0x00025B2E
		public bool HasCallback()
		{
			return this.LocalizeCallBack.HasCallback() || this.LocalizeEvent.GetPersistentEventCount() > 0;
		}

		// Token: 0x06003AA2 RID: 15010 RVA: 0x001293A0 File Offset: 0x001275A0
		public void OnLocalize(bool Force = false)
		{
			if (!Force && (!base.enabled || base.gameObject == null || !base.gameObject.activeInHierarchy))
			{
				return;
			}
			if (string.IsNullOrEmpty(LocalizationManager.CurrentLanguage))
			{
				return;
			}
			if (!this.AlwaysForceLocalize && !Force && !this.HasCallback() && this.LastLocalizedLanguage == LocalizationManager.CurrentLanguage)
			{
				return;
			}
			this.LastLocalizedLanguage = LocalizationManager.CurrentLanguage;
			if (string.IsNullOrEmpty(this.FinalTerm) || string.IsNullOrEmpty(this.FinalSecondaryTerm))
			{
				this.GetFinalTerms(out this.FinalTerm, out this.FinalSecondaryTerm);
			}
			bool flag = I2Utils.IsPlaying() && this.HasCallback();
			if (!flag && string.IsNullOrEmpty(this.FinalTerm) && string.IsNullOrEmpty(this.FinalSecondaryTerm))
			{
				return;
			}
			Localize.CallBackTerm = this.FinalTerm;
			Localize.CallBackSecondaryTerm = this.FinalSecondaryTerm;
			Localize.MainTranslation = ((string.IsNullOrEmpty(this.FinalTerm) || this.FinalTerm == "-") ? null : LocalizationManager.GetTranslation(this.FinalTerm, false, 0, true, false, null, null, true));
			Localize.SecondaryTranslation = ((string.IsNullOrEmpty(this.FinalSecondaryTerm) || this.FinalSecondaryTerm == "-") ? null : LocalizationManager.GetTranslation(this.FinalSecondaryTerm, false, 0, true, false, null, null, true));
			if (!flag && string.IsNullOrEmpty(this.FinalTerm) && string.IsNullOrEmpty(Localize.SecondaryTranslation))
			{
				return;
			}
			Localize.CurrentLocalizeComponent = this;
			this.LocalizeCallBack.Execute(this);
			this.LocalizeEvent.Invoke();
			if (this.AllowParameters)
			{
				LocalizationManager.ApplyLocalizationParams(ref Localize.MainTranslation, base.gameObject, this.AllowLocalizedParameters);
			}
			if (!this.FindTarget())
			{
				return;
			}
			bool flag2 = LocalizationManager.IsRight2Left && !this.IgnoreRTL;
			if (Localize.MainTranslation != null)
			{
				switch (this.PrimaryTermModifier)
				{
				case Localize.TermModification.ToUpper:
					Localize.MainTranslation = Localize.MainTranslation.ToUpper();
					break;
				case Localize.TermModification.ToLower:
					Localize.MainTranslation = Localize.MainTranslation.ToLower();
					break;
				case Localize.TermModification.ToUpperFirst:
					Localize.MainTranslation = GoogleTranslation.UppercaseFirst(Localize.MainTranslation);
					break;
				case Localize.TermModification.ToTitle:
					Localize.MainTranslation = GoogleTranslation.TitleCase(Localize.MainTranslation);
					break;
				}
				if (!string.IsNullOrEmpty(this.TermPrefix))
				{
					Localize.MainTranslation = (flag2 ? (Localize.MainTranslation + this.TermPrefix) : (this.TermPrefix + Localize.MainTranslation));
				}
				if (!string.IsNullOrEmpty(this.TermSuffix))
				{
					Localize.MainTranslation = (flag2 ? (this.TermSuffix + Localize.MainTranslation) : (Localize.MainTranslation + this.TermSuffix));
				}
				if (this.AddSpacesToJoinedLanguages && LocalizationManager.HasJoinedWords && !string.IsNullOrEmpty(Localize.MainTranslation))
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append(Localize.MainTranslation[0]);
					int i = 1;
					int length = Localize.MainTranslation.Length;
					while (i < length)
					{
						stringBuilder.Append(' ');
						stringBuilder.Append(Localize.MainTranslation[i]);
						i++;
					}
					Localize.MainTranslation = stringBuilder.ToString();
				}
				if (flag2 && this.mLocalizeTarget.AllowMainTermToBeRTL() && !string.IsNullOrEmpty(Localize.MainTranslation))
				{
					Localize.MainTranslation = LocalizationManager.ApplyRTLfix(Localize.MainTranslation, this.MaxCharactersInRTL, this.IgnoreNumbersInRTL);
				}
			}
			if (Localize.SecondaryTranslation != null)
			{
				switch (this.SecondaryTermModifier)
				{
				case Localize.TermModification.ToUpper:
					Localize.SecondaryTranslation = Localize.SecondaryTranslation.ToUpper();
					break;
				case Localize.TermModification.ToLower:
					Localize.SecondaryTranslation = Localize.SecondaryTranslation.ToLower();
					break;
				case Localize.TermModification.ToUpperFirst:
					Localize.SecondaryTranslation = GoogleTranslation.UppercaseFirst(Localize.SecondaryTranslation);
					break;
				case Localize.TermModification.ToTitle:
					Localize.SecondaryTranslation = GoogleTranslation.TitleCase(Localize.SecondaryTranslation);
					break;
				}
				if (flag2 && this.mLocalizeTarget.AllowSecondTermToBeRTL() && !string.IsNullOrEmpty(Localize.SecondaryTranslation))
				{
					Localize.SecondaryTranslation = LocalizationManager.ApplyRTLfix(Localize.SecondaryTranslation);
				}
			}
			if (LocalizationManager.HighlightLocalizedTargets)
			{
				Localize.MainTranslation = "LOC:" + this.FinalTerm;
			}
			this.mLocalizeTarget.DoLocalize(this, Localize.MainTranslation, Localize.SecondaryTranslation);
			Localize.CurrentLocalizeComponent = null;
		}

		// Token: 0x06003AA3 RID: 15011 RVA: 0x001297C4 File Offset: 0x001279C4
		public bool FindTarget()
		{
			if (this.mLocalizeTarget != null && this.mLocalizeTarget.IsValid(this))
			{
				return true;
			}
			if (this.mLocalizeTarget != null)
			{
				UnityEngine.Object.DestroyImmediate(this.mLocalizeTarget);
				this.mLocalizeTarget = null;
				this.mLocalizeTargetName = null;
			}
			if (!string.IsNullOrEmpty(this.mLocalizeTargetName))
			{
				foreach (ILocalizeTargetDescriptor localizeTargetDescriptor in LocalizationManager.mLocalizeTargets)
				{
					if (this.mLocalizeTargetName == localizeTargetDescriptor.GetTargetType().ToString())
					{
						if (localizeTargetDescriptor.CanLocalize(this))
						{
							this.mLocalizeTarget = localizeTargetDescriptor.CreateTarget(this);
						}
						if (this.mLocalizeTarget != null)
						{
							return true;
						}
					}
				}
			}
			foreach (ILocalizeTargetDescriptor localizeTargetDescriptor2 in LocalizationManager.mLocalizeTargets)
			{
				if (localizeTargetDescriptor2.CanLocalize(this))
				{
					this.mLocalizeTarget = localizeTargetDescriptor2.CreateTarget(this);
					this.mLocalizeTargetName = localizeTargetDescriptor2.GetTargetType().ToString();
					if (this.mLocalizeTarget != null)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06003AA4 RID: 15012 RVA: 0x0012991C File Offset: 0x00127B1C
		public void GetFinalTerms(out string primaryTerm, out string secondaryTerm)
		{
			primaryTerm = string.Empty;
			secondaryTerm = string.Empty;
			if (!this.FindTarget())
			{
				return;
			}
			if (this.mLocalizeTarget != null)
			{
				this.mLocalizeTarget.GetFinalTerms(this, this.mTerm, this.mTermSecondary, out primaryTerm, out secondaryTerm);
				primaryTerm = I2Utils.GetValidTermName(primaryTerm, false);
			}
			if (!string.IsNullOrEmpty(this.mTerm))
			{
				primaryTerm = this.mTerm;
			}
			if (!string.IsNullOrEmpty(this.mTermSecondary))
			{
				secondaryTerm = this.mTermSecondary;
			}
			if (primaryTerm != null)
			{
				primaryTerm = primaryTerm.Trim();
			}
			if (secondaryTerm != null)
			{
				secondaryTerm = secondaryTerm.Trim();
			}
		}

		// Token: 0x06003AA5 RID: 15013 RVA: 0x001299B8 File Offset: 0x00127BB8
		public string GetMainTargetsText()
		{
			string text = null;
			string text2 = null;
			if (this.mLocalizeTarget != null)
			{
				this.mLocalizeTarget.GetFinalTerms(this, null, null, out text, out text2);
			}
			if (!string.IsNullOrEmpty(text))
			{
				return text;
			}
			return this.mTerm;
		}

		// Token: 0x06003AA6 RID: 15014 RVA: 0x0002794D File Offset: 0x00025B4D
		public void SetFinalTerms(string Main, string Secondary, out string primaryTerm, out string secondaryTerm, bool RemoveNonASCII)
		{
			primaryTerm = (RemoveNonASCII ? I2Utils.GetValidTermName(Main, false) : Main);
			secondaryTerm = Secondary;
		}

		// Token: 0x06003AA7 RID: 15015 RVA: 0x001299FC File Offset: 0x00127BFC
		public void SetTerm(string primary)
		{
			if (!string.IsNullOrEmpty(primary))
			{
				this.mTerm = primary;
				this.FinalTerm = primary;
			}
			this.OnLocalize(true);
		}

		// Token: 0x06003AA8 RID: 15016 RVA: 0x00129A28 File Offset: 0x00127C28
		public void SetTerm(string primary, string secondary)
		{
			if (!string.IsNullOrEmpty(primary))
			{
				this.mTerm = primary;
				this.FinalTerm = primary;
			}
			this.mTermSecondary = secondary;
			this.FinalSecondaryTerm = secondary;
			this.OnLocalize(true);
		}

		// Token: 0x06003AA9 RID: 15017 RVA: 0x00129A64 File Offset: 0x00127C64
		internal T GetSecondaryTranslatedObj<T>(ref string mainTranslation, ref string secondaryTranslation) where T : UnityEngine.Object
		{
			string text;
			string text2;
			this.DeserializeTranslation(mainTranslation, out text, out text2);
			T t = default(T);
			if (!string.IsNullOrEmpty(text2))
			{
				t = this.GetObject<T>(text2);
				if (t != null)
				{
					mainTranslation = text;
					secondaryTranslation = text2;
				}
			}
			if (t == null)
			{
				t = this.GetObject<T>(secondaryTranslation);
			}
			return t;
		}

		// Token: 0x06003AAA RID: 15018 RVA: 0x00129AC4 File Offset: 0x00127CC4
		public void UpdateAssetDictionary()
		{
			this.TranslatedObjects.RemoveAll((UnityEngine.Object x) => x == null);
			this.mAssetDictionary = (from o in this.TranslatedObjects.Distinct<UnityEngine.Object>()
			group o by o.name).ToDictionary((IGrouping<string, UnityEngine.Object> g) => g.Key, (IGrouping<string, UnityEngine.Object> g) => g.First<UnityEngine.Object>());
		}

		// Token: 0x06003AAB RID: 15019 RVA: 0x00129B74 File Offset: 0x00127D74
		internal T GetObject<T>(string Translation) where T : UnityEngine.Object
		{
			if (string.IsNullOrEmpty(Translation))
			{
				return default(T);
			}
			return this.GetTranslatedObject<T>(Translation);
		}

		// Token: 0x06003AAC RID: 15020 RVA: 0x00027963 File Offset: 0x00025B63
		private T GetTranslatedObject<T>(string Translation) where T : UnityEngine.Object
		{
			return this.FindTranslatedObject<T>(Translation);
		}

		// Token: 0x06003AAD RID: 15021 RVA: 0x00129B9C File Offset: 0x00127D9C
		private void DeserializeTranslation(string translation, out string value, out string secondary)
		{
			if (!string.IsNullOrEmpty(translation) && translation.Length > 1 && translation[0] == '[')
			{
				int num = translation.IndexOf(']');
				if (num > 0)
				{
					secondary = translation.Substring(1, num - 1);
					value = translation.Substring(num + 1);
					return;
				}
			}
			value = translation;
			secondary = string.Empty;
		}

		// Token: 0x06003AAE RID: 15022 RVA: 0x00129BF4 File Offset: 0x00127DF4
		public T FindTranslatedObject<T>(string value) where T : UnityEngine.Object
		{
			if (string.IsNullOrEmpty(value))
			{
				T result = default(T);
				return result;
			}
			if (this.mAssetDictionary == null || this.mAssetDictionary.Count != this.TranslatedObjects.Count)
			{
				this.UpdateAssetDictionary();
			}
			foreach (KeyValuePair<string, UnityEngine.Object> keyValuePair in this.mAssetDictionary)
			{
				if (keyValuePair.Value is T && value.EndsWith(keyValuePair.Key, StringComparison.OrdinalIgnoreCase) && string.Compare(value, keyValuePair.Key, StringComparison.OrdinalIgnoreCase) == 0)
				{
					return (T)((object)keyValuePair.Value);
				}
			}
			T t = LocalizationManager.FindAsset(value) as T;
			if (t)
			{
				return t;
			}
			return ResourceManager.pInstance.GetAsset<T>(value);
		}

		// Token: 0x06003AAF RID: 15023 RVA: 0x0002796C File Offset: 0x00025B6C
		public bool HasTranslatedObject(UnityEngine.Object Obj)
		{
			return this.TranslatedObjects.Contains(Obj) || ResourceManager.pInstance.HasAsset(Obj);
		}

		// Token: 0x06003AB0 RID: 15024 RVA: 0x00027989 File Offset: 0x00025B89
		public void AddTranslatedObject(UnityEngine.Object Obj)
		{
			if (this.TranslatedObjects.Contains(Obj))
			{
				return;
			}
			this.TranslatedObjects.Add(Obj);
			this.UpdateAssetDictionary();
		}

		// Token: 0x06003AB1 RID: 15025 RVA: 0x000279AC File Offset: 0x00025BAC
		public void SetGlobalLanguage(string Language)
		{
			LocalizationManager.CurrentLanguage = Language;
		}

		// Token: 0x040038AE RID: 14510
		public string mTerm = string.Empty;

		// Token: 0x040038AF RID: 14511
		public string mTermSecondary = string.Empty;

		// Token: 0x040038B0 RID: 14512
		[NonSerialized]
		public string FinalTerm;

		// Token: 0x040038B1 RID: 14513
		[NonSerialized]
		public string FinalSecondaryTerm;

		// Token: 0x040038B2 RID: 14514
		public Localize.TermModification PrimaryTermModifier;

		// Token: 0x040038B3 RID: 14515
		public Localize.TermModification SecondaryTermModifier;

		// Token: 0x040038B4 RID: 14516
		public string TermPrefix;

		// Token: 0x040038B5 RID: 14517
		public string TermSuffix;

		// Token: 0x040038B6 RID: 14518
		public bool LocalizeOnAwake = true;

		// Token: 0x040038B7 RID: 14519
		private string LastLocalizedLanguage;

		// Token: 0x040038B8 RID: 14520
		public bool IgnoreRTL;

		// Token: 0x040038B9 RID: 14521
		public int MaxCharactersInRTL;

		// Token: 0x040038BA RID: 14522
		public bool IgnoreNumbersInRTL = true;

		// Token: 0x040038BB RID: 14523
		public bool CorrectAlignmentForRTL = true;

		// Token: 0x040038BC RID: 14524
		public bool AddSpacesToJoinedLanguages;

		// Token: 0x040038BD RID: 14525
		public bool AllowLocalizedParameters = true;

		// Token: 0x040038BE RID: 14526
		public bool AllowParameters = true;

		// Token: 0x040038BF RID: 14527
		public List<UnityEngine.Object> TranslatedObjects = new List<UnityEngine.Object>();

		// Token: 0x040038C0 RID: 14528
		[NonSerialized]
		public Dictionary<string, UnityEngine.Object> mAssetDictionary = new Dictionary<string, UnityEngine.Object>(StringComparer.Ordinal);

		// Token: 0x040038C1 RID: 14529
		public UnityEvent LocalizeEvent = new UnityEvent();

		// Token: 0x040038C2 RID: 14530
		public static string MainTranslation;

		// Token: 0x040038C3 RID: 14531
		public static string SecondaryTranslation;

		// Token: 0x040038C4 RID: 14532
		public static string CallBackTerm;

		// Token: 0x040038C5 RID: 14533
		public static string CallBackSecondaryTerm;

		// Token: 0x040038C6 RID: 14534
		public static Localize CurrentLocalizeComponent;

		// Token: 0x040038C7 RID: 14535
		public bool AlwaysForceLocalize;

		// Token: 0x040038C8 RID: 14536
		[SerializeField]
		public EventCallback LocalizeCallBack = new EventCallback();

		// Token: 0x040038C9 RID: 14537
		public bool mGUI_ShowReferences;

		// Token: 0x040038CA RID: 14538
		public bool mGUI_ShowTems = true;

		// Token: 0x040038CB RID: 14539
		public bool mGUI_ShowCallback;

		// Token: 0x040038CC RID: 14540
		public ILocalizeTarget mLocalizeTarget;

		// Token: 0x040038CD RID: 14541
		public string mLocalizeTargetName;

		// Token: 0x02000814 RID: 2068
		public enum TermModification
		{
			// Token: 0x040038CF RID: 14543
			DontModify,
			// Token: 0x040038D0 RID: 14544
			ToUpper,
			// Token: 0x040038D1 RID: 14545
			ToLower,
			// Token: 0x040038D2 RID: 14546
			ToUpperFirst,
			// Token: 0x040038D3 RID: 14547
			ToTitle
		}
	}
}
