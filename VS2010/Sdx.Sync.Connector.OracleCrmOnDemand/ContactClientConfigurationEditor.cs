// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactClientConfigurationEditor.cs" company="SDX-AG">
//   (c) 2010 SDX-AG
// </copyright>
// <summary>
//   Defines the ContactClientConfigurationEditor type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------


namespace Sdx.Sync.Connector.OracleCrmOnDemand
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Windows.Forms;

    using Sdx.Sync.Connector.OracleCrmOnDemand.Helpers;

    using Sem.GenericHelpers;
    using Sem.GenericHelpers.Entities;

    public partial class ContactClientConfigurationEditor : Form
    {
        public ContactClientConfigurationEditor()
        {
            InitializeComponent();
        }

        public DialogResult ShowDialog(ContactClientConfigurationData theData)
        {

            this.PageSize.Text = theData.PageSize.ToString(CultureInfo.CurrentCulture);
            this.ReadAllAttributes.Checked = theData.GetAllAttributes;
            this.IgnoreCertificateErrors.Checked = theData.IgnoreCertificateErrors;
            if (theData.FilterList != null)
            {
                this.Filter.Items.AddRange((from x in theData.FilterList select x.Key + " : " + x.Value).ToArray());
            }

            // read filter-properties from ContactQuery and AccountQuery
            var fieldsList = new List<string>();
            fieldsList.AddRange((from x in Tools.GetPropertyList(typeof(ContactSR.ContactQuery)) orderby x select Utils.ExtractPropertyName(x, "Contact")).Distinct());
            fieldsList.AddRange((from x in Tools.GetPropertyList(typeof(AccountSR.AccountQuery)) orderby x select Utils.ExtractPropertyName(x, "Account")).Distinct());
            fieldsList.AddRange((from x in Tools.GetPropertyList(typeof(ActivitySR.Activity)) orderby x select Utils.ExtractPropertyName(x, "Activity")).Distinct());
            this.Field.Items.AddRange(fieldsList.ToArray());

            var result = this.ShowDialog();

            if (result == DialogResult.OK)
            {
                theData.PageSize = int.Parse(this.PageSize.Text, CultureInfo.InvariantCulture);
                theData.GetAllAttributes = this.ReadAllAttributes.Checked;
                theData.IgnoreCertificateErrors = this.IgnoreCertificateErrors.Checked;
                theData.FilterList = new List<KeyValuePair>();
                foreach (var item in this.Filter.Items)
                {
                    theData.FilterList.Add(Utils.SplitToKeyValuePair(item.ToString()));
                }

                if (this.filterForTestAccount.Checked)
                {
                    theData.FilterList.Add(new KeyValuePair(@"Account.AccountName", @"= 'Z. SDX AG (Test)'"));
                }

                if (this.filterForMapAccounts.Checked)
                {
                    theData.FilterList.Add(new KeyValuePair(@"Account.CustomBoolean14", @"= 'Y'"));
                }
            }

            return result;
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            this.Filter.Items.Add(this.Field.Text + " : " + this.Condition.Text + " '" + Utils.EscapeCharacters(this.Argument.Text) + "'");
        }

        private void Remove_Click(object sender, EventArgs e)
        {
            if (this.Filter.SelectedIndex > -1)
            {
                this.Filter.Items.RemoveAt(this.Filter.SelectedIndex);
            }
        }
    }
}
