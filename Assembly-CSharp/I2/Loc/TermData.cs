using System;
using System.Collections.Generic;
using UnityEngine;

namespace I2.Loc
{
	// Token: 0x02000837 RID: 2103
	[Serializable]
	public class TermData
	{
		// Token: 0x06003BC1 RID: 15297 RVA: 0x0012C630 File Offset: 0x0012A830
		public string GetTranslation(int idx, string specialization = null, bool editMode = false)
		{
			string text = this.Languages[idx];
			if (text != null)
			{
				text = SpecializationManager.GetSpecializedText(text, specialization);
				if (!editMode)
				{
					text = text.Replace("[i2nt]", "").Replace("[/i2nt]", "");
				}
			}
			return text;
		}

		// Token: 0x06003BC2 RID: 15298 RVA: 0x0002810C File Offset: 0x0002630C
		public void SetTranslation(int idx, string translation, string specialization = null)
		{
			this.Languages[idx] = SpecializationManager.SetSpecializedText(this.Languages[idx], translation, specialization);
		}

		// Token: 0x06003BC3 RID: 15299 RVA: 0x0012C678 File Offset: 0x0012A878
		public void RemoveSpecialization(string specialization)
		{
			for (int i = 0; i < this.Languages.Length; i++)
			{
				this.RemoveSpecialization(i, specialization);
			}
		}

		// Token: 0x06003BC4 RID: 15300 RVA: 0x0012C6A0 File Offset: 0x0012A8A0
		public void RemoveSpecialization(int idx, string specialization)
		{
			string text = this.Languages[idx];
			if (specialization == "Any" || !text.Contains("[i2s_" + specialization + "]"))
			{
				return;
			}
			Dictionary<string, string> specializations = SpecializationManager.GetSpecializations(text, null);
			specializations.Remove(specialization);
			this.Languages[idx] = SpecializationManager.SetSpecializedText(specializations);
		}

		// Token: 0x06003BC5 RID: 15301 RVA: 0x00028125 File Offset: 0x00026325
		public bool IsAutoTranslated(int idx, bool IsTouch)
		{
			return (this.Flags[idx] & 2) > 0;
		}

		// Token: 0x06003BC6 RID: 15302 RVA: 0x0012C6FC File Offset: 0x0012A8FC
		public void Validate()
		{
			int num = Mathf.Max(this.Languages.Length, this.Flags.Length);
			if (this.Languages.Length != num)
			{
				Array.Resize<string>(ref this.Languages, num);
			}
			if (this.Flags.Length != num)
			{
				Array.Resize<byte>(ref this.Flags, num);
			}
			if (this.Languages_Touch != null)
			{
				for (int i = 0; i < Mathf.Min(this.Languages_Touch.Length, num); i++)
				{
					if (string.IsNullOrEmpty(this.Languages[i]) && !string.IsNullOrEmpty(this.Languages_Touch[i]))
					{
						this.Languages[i] = this.Languages_Touch[i];
						this.Languages_Touch[i] = null;
					}
				}
				this.Languages_Touch = null;
			}
		}

		// Token: 0x06003BC7 RID: 15303 RVA: 0x00028134 File Offset: 0x00026334
		public bool IsTerm(string name, bool allowCategoryMistmatch)
		{
			if (!allowCategoryMistmatch)
			{
				return name == this.Term;
			}
			return name == LanguageSourceData.GetKeyFromFullTerm(this.Term, false);
		}

		// Token: 0x06003BC8 RID: 15304 RVA: 0x0012C7AC File Offset: 0x0012A9AC
		public bool HasSpecializations()
		{
			for (int i = 0; i < this.Languages.Length; i++)
			{
				if (!string.IsNullOrEmpty(this.Languages[i]) && this.Languages[i].Contains("[i2s_"))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003BC9 RID: 15305 RVA: 0x0012C7F4 File Offset: 0x0012A9F4
		public List<string> GetAllSpecializations()
		{
			List<string> list = new List<string>();
			for (int i = 0; i < this.Languages.Length; i++)
			{
				SpecializationManager.AppendSpecializations(this.Languages[i], list);
			}
			return list;
		}

		// Token: 0x0400391E RID: 14622
		public string Term = string.Empty;

		// Token: 0x0400391F RID: 14623
		public eTermType TermType;

		// Token: 0x04003920 RID: 14624
		[NonSerialized]
		public string Description;

		// Token: 0x04003921 RID: 14625
		public string[] Languages = new string[0];

		// Token: 0x04003922 RID: 14626
		public byte[] Flags = new byte[0];

		// Token: 0x04003923 RID: 14627
		[SerializeField]
		private string[] Languages_Touch;
	}
}
