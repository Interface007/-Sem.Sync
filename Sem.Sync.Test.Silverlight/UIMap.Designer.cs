﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by coded UI test builder.
//      Version: 10.0.0.0
//
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------

namespace Sem.Sync.Test.Silverlight
{
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Text.RegularExpressions;
    using System.Windows.Input;
    using Microsoft.VisualStudio.TestTools.UITest.Extension;
    using Microsoft.VisualStudio.TestTools.UITesting;
    using Microsoft.VisualStudio.TestTools.UITesting.HtmlControls;
    using Microsoft.VisualStudio.TestTools.UITesting.SilverlightControls;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Keyboard = Microsoft.VisualStudio.TestTools.UITesting.Keyboard;
    using Mouse = Microsoft.VisualStudio.TestTools.UITesting.Mouse;
    using MouseButtons = System.Windows.Forms.MouseButtons;
    
    
    [GeneratedCode("Coded UITest Builder", "10.0.30319.1")]
    public partial class UIMap
    {
        
        /// <summary>
        /// FailedRegistration - Use 'FailedRegistrationExpectedValues' to pass parameters into this method.
        /// </summary>
        public void FailedRegistration()
        {
            #region Variable Declarations
            SilverlightText uIInvokeoperationCreatText = this.UIHomeWindowsInternetEWindow.UIHomeDocument.UISilverlightControlHoPane.UIItemCustom.UIBusyIndicatorBusyIndicator.UIItemWindow.UIInvokeoperationCreatText;
            #endregion

            // Verify that 'Invoke operation 'CreateUser' failed. Keine Verbin...' label's property 'Text' equals 'Invoke operation 'CreateUser' failed. Keine Verbindung mit der SQL Server-Datenbank. Inner exception message: Keine Verbindung mit der SQL Server-Datenbank.'
            Assert.AreEqual(this.FailedRegistrationExpectedValues.UIInvokeoperationCreatTextText, uIInvokeoperationCreatText.Text);
        }
        
        #region Properties
        public virtual FailedRegistrationExpectedValues FailedRegistrationExpectedValues
        {
            get
            {
                if ((this.mFailedRegistrationExpectedValues == null))
                {
                    this.mFailedRegistrationExpectedValues = new FailedRegistrationExpectedValues();
                }
                return this.mFailedRegistrationExpectedValues;
            }
        }
        
        public UIHomeWindowsInternetEWindow UIHomeWindowsInternetEWindow
        {
            get
            {
                if ((this.mUIHomeWindowsInternetEWindow == null))
                {
                    this.mUIHomeWindowsInternetEWindow = new UIHomeWindowsInternetEWindow();
                }
                return this.mUIHomeWindowsInternetEWindow;
            }
        }
        #endregion
        
        #region Fields
        private FailedRegistrationExpectedValues mFailedRegistrationExpectedValues;
        
        private UIHomeWindowsInternetEWindow mUIHomeWindowsInternetEWindow;
        #endregion
    }
    
    /// <summary>
    /// Parameters to be passed into 'FailedRegistration'
    /// </summary>
    [GeneratedCode("Coded UITest Builder", "10.0.30319.1")]
    public class FailedRegistrationExpectedValues
    {
        
        #region Fields
        /// <summary>
        /// Verify that 'Invoke operation 'CreateUser' failed. Keine Verbin...' label's property 'Text' equals 'Invoke operation 'CreateUser' failed. Keine Verbindung mit der SQL Server-Datenbank. Inner exception message: Keine Verbindung mit der SQL Server-Datenbank.'
        /// </summary>
        public string UIInvokeoperationCreatTextText = "Invoke operation \'CreateUser\' failed. Keine Verbindung mit der SQL Server-Datenba" +
            "nk. Inner exception message: Keine Verbindung mit der SQL Server-Datenbank.";
        #endregion
    }
    
    [GeneratedCode("Coded UITest Builder", "10.0.30319.1")]
    public class UIHomeWindowsInternetEWindow : BrowserWindow
    {
        
        public UIHomeWindowsInternetEWindow()
        {
            #region Search Criteria
            this.SearchProperties[UITestControl.PropertyNames.Name] = "Home";
            this.SearchProperties[UITestControl.PropertyNames.ClassName] = "IEFrame";
            this.WindowTitles.Add("Home");
            #endregion
        }
        
        public void LaunchUrl(System.Uri url)
        {
            this.CopyFrom(BrowserWindow.Launch(url));
        }
        
        #region Properties
        public UIHomeDocument UIHomeDocument
        {
            get
            {
                if ((this.mUIHomeDocument == null))
                {
                    this.mUIHomeDocument = new UIHomeDocument(this);
                }
                return this.mUIHomeDocument;
            }
        }
        #endregion
        
        #region Fields
        private UIHomeDocument mUIHomeDocument;
        #endregion
    }
    
    [GeneratedCode("Coded UITest Builder", "10.0.30319.1")]
    public class UIHomeDocument : HtmlDocument
    {
        
        public UIHomeDocument(UITestControl searchLimitContainer) : 
                base(searchLimitContainer)
        {
            #region Search Criteria
            this.SearchProperties[HtmlDocument.PropertyNames.Id] = null;
            this.SearchProperties[HtmlDocument.PropertyNames.RedirectingPage] = "False";
            this.SearchProperties[HtmlDocument.PropertyNames.FrameDocument] = "False";
            this.FilterProperties[HtmlDocument.PropertyNames.Title] = "Home";
            this.FilterProperties[HtmlDocument.PropertyNames.AbsolutePath] = "/Sem.Sync.Web.SilverContactsTestPage.aspx";
            this.FilterProperties[HtmlDocument.PropertyNames.PageUrl] = "http://localhost:52878/Sem.Sync.Web.SilverContactsTestPage.aspx#/Home";
            this.WindowTitles.Add("Home");
            #endregion
        }
        
        #region Properties
        public UISilverlightControlHoPane UISilverlightControlHoPane
        {
            get
            {
                if ((this.mUISilverlightControlHoPane == null))
                {
                    this.mUISilverlightControlHoPane = new UISilverlightControlHoPane(this);
                }
                return this.mUISilverlightControlHoPane;
            }
        }
        #endregion
        
        #region Fields
        private UISilverlightControlHoPane mUISilverlightControlHoPane;
        #endregion
    }
    
    [GeneratedCode("Coded UITest Builder", "10.0.30319.1")]
    public class UISilverlightControlHoPane : HtmlDiv
    {
        
        public UISilverlightControlHoPane(UITestControl searchLimitContainer) : 
                base(searchLimitContainer)
        {
            #region Search Criteria
            this.SearchProperties[HtmlDiv.PropertyNames.Id] = "silverlightControlHost";
            this.SearchProperties[HtmlDiv.PropertyNames.Name] = null;
            this.FilterProperties[HtmlDiv.PropertyNames.InnerText] = null;
            this.FilterProperties[HtmlDiv.PropertyNames.Title] = null;
            this.FilterProperties[HtmlDiv.PropertyNames.Class] = null;
            this.FilterProperties[HtmlDiv.PropertyNames.ControlDefinition] = "id=silverlightControlHost";
            this.FilterProperties[HtmlDiv.PropertyNames.TagInstance] = "2";
            this.WindowTitles.Add("Home");
            #endregion
        }
        
        #region Properties
        public UIItemCustom UIItemCustom
        {
            get
            {
                if ((this.mUIItemCustom == null))
                {
                    this.mUIItemCustom = new UIItemCustom(this);
                }
                return this.mUIItemCustom;
            }
        }
        #endregion
        
        #region Fields
        private UIItemCustom mUIItemCustom;
        #endregion
    }
    
    [GeneratedCode("Coded UITest Builder", "10.0.30319.1")]
    public class UIItemCustom : HtmlCustom
    {
        
        public UIItemCustom(UITestControl searchLimitContainer) : 
                base(searchLimitContainer)
        {
            #region Search Criteria
            this.SearchProperties["Id"] = null;
            this.SearchProperties[UITestControl.PropertyNames.Name] = null;
            this.SearchProperties["TagName"] = "OBJECT";
            this.FilterProperties["Class"] = null;
            this.FilterProperties["ControlDefinition"] = "data=\"data:application/x-oleobject;base6";
            this.FilterProperties["TagInstance"] = "1";
            this.WindowTitles.Add("Home");
            #endregion
        }
        
        #region Properties
        public UIBusyIndicatorBusyIndicator UIBusyIndicatorBusyIndicator
        {
            get
            {
                if ((this.mUIBusyIndicatorBusyIndicator == null))
                {
                    this.mUIBusyIndicatorBusyIndicator = new UIBusyIndicatorBusyIndicator(this);
                }
                return this.mUIBusyIndicatorBusyIndicator;
            }
        }
        #endregion
        
        #region Fields
        private UIBusyIndicatorBusyIndicator mUIBusyIndicatorBusyIndicator;
        #endregion
    }
    
    [GeneratedCode("Coded UITest Builder", "10.0.30319.1")]
    public class UIBusyIndicatorBusyIndicator : SilverlightControl
    {
        
        public UIBusyIndicatorBusyIndicator(UITestControl searchLimitContainer) : 
                base(searchLimitContainer)
        {
            #region Search Criteria
            this.SearchProperties[UITestControl.PropertyNames.ControlType] = "BusyIndicator";
            this.WindowTitles.Add("Home");
            #endregion
        }
        
        #region Properties
        public UIItemWindow UIItemWindow
        {
            get
            {
                if ((this.mUIItemWindow == null))
                {
                    this.mUIItemWindow = new UIItemWindow(this);
                }
                return this.mUIItemWindow;
            }
        }
        #endregion
        
        #region Fields
        private UIItemWindow mUIItemWindow;
        #endregion
    }
    
    [GeneratedCode("Coded UITest Builder", "10.0.30319.1")]
    public class UIItemWindow : SilverlightControl
    {
        
        public UIItemWindow(UITestControl searchLimitContainer) : 
                base(searchLimitContainer)
        {
            #region Search Criteria
            this.SearchProperties[UITestControl.PropertyNames.ControlType] = "Window";
            this.WindowTitles.Add("Home");
            #endregion
        }
        
        #region Properties
        public SilverlightText UIInvokeoperationCreatText
        {
            get
            {
                if ((this.mUIInvokeoperationCreatText == null))
                {
                    this.mUIInvokeoperationCreatText = new SilverlightText(this);
                    #region Search Criteria
                    this.mUIInvokeoperationCreatText.SearchProperties[SilverlightText.PropertyNames.AutomationId] = "IntroductoryText";
                    this.mUIInvokeoperationCreatText.WindowTitles.Add("Home");
                    #endregion
                }
                return this.mUIInvokeoperationCreatText;
            }
        }
        #endregion
        
        #region Fields
        private SilverlightText mUIInvokeoperationCreatText;
        #endregion
    }
}
