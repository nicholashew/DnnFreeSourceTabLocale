using DotNetNuke.Services.Localization;
using System;
using System.Web.UI.WebControls;

namespace FreeSource.Modules.TabLocale.controls
{
    public partial class EditLocaleTab : System.Web.UI.UserControl
    {
        #region Properties

        public string SelectedLanguage
        {
            get
            {
                return ddlLanguage.SelectedValue;
            }
        }

        public string TabName
        {
            get
            {
                return txtTabName.Text;
            }
            set
            {
                txtTabName.Text = value;
            }
        }

        public string TabTitle
        {
            get
            {
                return txtTabTitle.Text;
            }
            set
            {
                txtTabTitle.Text = value;
            }
        }

        public string TabDescription
        {
            get
            {
                return txtTabDescription.Text;
            }
            set
            {
                txtTabDescription.Text = value;
            }
        }

        public string TabKeyWords
        {
            get
            {
                return txtTabKeyWords.Text;
            }
            set
            {
                txtTabKeyWords.Text = value;
            }
        }

        public string TabPageHeadText
        {
            get
            {
                return txtTabPageHeadText.Text;
            }
            set
            {
                txtTabPageHeadText.Text = value;
            }
        }

        #endregion

        #region Events

        public delegate void OnLanguageChangedHandler(object sender, EventArgs e);
        public event OnLanguageChangedHandler OnLanguageChanged;

        public delegate void OnSaveClickedHandler(object sender, EventArgs e);
        public event OnSaveClickedHandler OnSaveClicked;

        public delegate void OnDeleteClickedHandler(object sender, EventArgs e);
        public event OnDeleteClickedHandler OnDeleteClicked;

        protected void ddlLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            OnLanguageChanged?.Invoke(sender, e);
        }

        protected void lbtnSave_Click(object sender, EventArgs e)
        {
            OnSaveClicked?.Invoke(sender, e);
        }

        protected void lbtnDelete_Click(object sender, EventArgs e)
        {
            OnDeleteClicked?.Invoke(sender, e);
        }

        #endregion

        public void Initialize(bool includeSystemDefault = false)
        {
            Localization.LoadCultureDropDownList(ddlLanguage, CultureDropDownTypes.EnglishName, System.Threading.Thread.CurrentThread.CurrentCulture.ToString());
            if (includeSystemDefault)
            {
                ddlLanguage.Items.Insert(0, new ListItem { Text = "System Default", Value = "" });
                ddlLanguage.SelectedIndex = 0;
            }
        }

        public void SetTabInfo(string tabName, string title, string description, string keywords, string pageHeadText, bool isEditable, bool isDeleteable)
        {
            txtTabName.Text = tabName;
            txtTabTitle.Text = title;
            txtTabDescription.Text = description;
            txtTabKeyWords.Text = keywords;
            txtTabPageHeadText.Text = pageHeadText;

            txtTabName.Enabled = isEditable;
            txtTabTitle.Enabled = isEditable;
            txtTabDescription.Enabled = isEditable;
            txtTabKeyWords.Enabled = isEditable;
            txtTabPageHeadText.Enabled = isEditable;

            lbtnSave.Visible = isEditable;
            lbtnDelete.Visible = isDeleteable;
        }

        public void ClearTabInfo()
        {
            txtTabName.Text = string.Empty;
            txtTabTitle.Text = string.Empty;
            txtTabDescription.Text = string.Empty;
            txtTabKeyWords.Text = string.Empty;
            txtTabPageHeadText.Text = string.Empty;
        }
    }
}