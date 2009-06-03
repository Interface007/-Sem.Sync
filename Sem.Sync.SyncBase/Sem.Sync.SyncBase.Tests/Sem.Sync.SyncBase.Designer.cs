#pragma warning disable 0067, 0108
// ------------------------------------
// 
// Assembly Sem.Sync.SyncBase
// 
// ------------------------------------
namespace Sem.Sync.SyncBase.DetailData.Stubs
{
    /// <summary>Stub of AddressDetail</summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Stubs", "0.13.40528.2")]
    [global::System.Serializable]
    [global::System.Diagnostics.DebuggerDisplay("Stub = AddressDetail")]
    [global::System.Diagnostics.DebuggerStepThrough]
    public partial class SAddressDetail
      : global::Sem.Sync.SyncBase.DetailData.AddressDetail
      , global::Microsoft.Stubs.Framework.IStub
      , global::Microsoft.Stubs.Framework.IPartialStub
    {
        /// <summary>Initializes a new instance of type SAddressDetail</summary>
        [global::System.Diagnostics.DebuggerHidden]
        public SAddressDetail()
        {
            this.defaultStub =
              global::Microsoft.Stubs.Framework.StubFallbackBehavior.Current;
        }

        /// <summary>Gets or sets a value that indicates if the base method should be called instead of the fallback behavior</summary>
        public bool CallBase
        {
            [global::System.Diagnostics.DebuggerHidden]
            get
            {
                return this.callBase;
            }
            [global::System.Diagnostics.DebuggerHidden]
            set
            {
                this.callBase = value;
            }
        }

        /// <summary>Gets or sets the fallback behavior.</summary>
        public global::Microsoft.Stubs.Framework.IStubBehavior FallbackBehavior
        {
            [global::System.Diagnostics.DebuggerHidden]
            get
            {
                return this.defaultStub;
            }
            [global::System.Diagnostics.DebuggerHidden]
            set
            {
                this.defaultStub = value;
            }
        }

        /// <summary>Stub of method System.String Sem.Sync.SyncBase.DetailData.AddressDetail.ToString()</summary>
        [global::System.Diagnostics.DebuggerHidden]
        public override string ToString()
        {
            global::Microsoft.Stubs.Framework.StubDelegates.Func<string> sh
               = this.ToString01;
            if (sh != (global::Microsoft.Stubs.Framework.StubDelegates.Func<string>)null)
              return sh.Invoke();
            else 
            {
              if (this.callBase)
                return base.ToString();
              global::Microsoft.Stubs.Framework.IStubBehavior stub = this.FallbackBehavior;
              return stub.Result
                  <global::Sem.Sync.SyncBase.DetailData.Stubs.SAddressDetail, string>(this);
            }
        }

        /// <summary>Stub of method System.String Sem.Sync.SyncBase.DetailData.AddressDetail.ToString()</summary>
        public global::Microsoft.Stubs.Framework.StubDelegates.Func<string> ToString01;

        private bool callBase;

        private global::Microsoft.Stubs.Framework.IStubBehavior defaultStub;
    }
}
namespace Sem.Sync.SyncBase.Merging.Stubs
{
    /// <summary>Stub of ConflictTestContainer</summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Stubs", "0.13.40528.2")]
    [global::System.Serializable]
    [global::System.Diagnostics.DebuggerDisplay("Stub = ConflictTestContainer")]
    [global::System.Diagnostics.DebuggerStepThrough]
    public partial class SConflictTestContainer
      : global::Sem.Sync.SyncBase.Merging.ConflictTestContainer
      , global::Microsoft.Stubs.Framework.IStub
      , global::Microsoft.Stubs.Framework.IPartialStub
    {
        /// <summary>Initializes a new instance of type SConflictTestContainer</summary>
        [global::System.Diagnostics.DebuggerHidden]
        public SConflictTestContainer()
        {
            this.defaultStub =
              global::Microsoft.Stubs.Framework.StubFallbackBehavior.Current;
        }

        /// <summary>Gets or sets a value that indicates if the base method should be called instead of the fallback behavior</summary>
        public bool CallBase
        {
            [global::System.Diagnostics.DebuggerHidden]
            get
            {
                return this.callBase;
            }
            [global::System.Diagnostics.DebuggerHidden]
            set
            {
                this.callBase = value;
            }
        }

        /// <summary>Gets or sets the fallback behavior.</summary>
        public global::Microsoft.Stubs.Framework.IStubBehavior FallbackBehavior
        {
            [global::System.Diagnostics.DebuggerHidden]
            get
            {
                return this.defaultStub;
            }
            [global::System.Diagnostics.DebuggerHidden]
            set
            {
                this.defaultStub = value;
            }
        }

        private bool callBase;

        private global::Microsoft.Stubs.Framework.IStubBehavior defaultStub;
    }
}
namespace Sem.Sync.SyncBase.Interfaces.Stubs
{
    /// <summary>Stub of IClientBase</summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Stubs", "0.13.40528.2")]
    [global::System.Serializable]
    [global::System.Diagnostics.DebuggerDisplay("Stub = IClientBase")]
    [global::System.Diagnostics.DebuggerStepThrough]
    public partial class SIClientBase
      : global::Microsoft.Stubs.Framework.StubBase
      , global::Sem.Sync.SyncBase.Interfaces.IClientBase
    {
        /// <summary>Initializes a new instance of type SIClientBase</summary>
        [global::System.Diagnostics.DebuggerHidden]
        public SIClientBase()
        {
        }

        /// <summary>Stub of method System.Void Sem.Sync.SyncBase.Interfaces.IClientBase.AddItem(Sem.Sync.SyncBase.StdElement element, System.String clientFolderName)</summary>
        public global::Microsoft.Stubs.Framework.StubDelegates.Action<global::Sem.Sync.SyncBase.StdElement, string> AddItem;

        /// <summary>Stub of method System.Void Sem.Sync.SyncBase.Interfaces.IClientBase.AddRange(System.Collections.Generic.List`1&lt;Sem.Sync.SyncBase.StdElement&gt; elements, System.String clientFolderName)</summary>
        public global::Microsoft.Stubs.Framework.StubDelegates.Action<global::System.Collections.Generic.List<global::Sem.Sync.SyncBase.StdElement>, string> AddRange;

        /// <summary>Stub of property Sem.Sync.SyncBase.Interfaces.IClientBase.FriendlyClientName</summary>
        string global::Sem.Sync.SyncBase.Interfaces.IClientBase.FriendlyClientName
        {
            [global::System.Diagnostics.DebuggerHidden]
            get
            {
                global::Microsoft.Stubs.Framework.StubDelegates.Func<string> sh
                   = this.FriendlyClientNameGet;
                if (sh != (global::Microsoft.Stubs.Framework.StubDelegates.Func<string>)null)
                  return sh.Invoke();
                else 
                {
                  global::Microsoft.Stubs.Framework.IStubBehavior stub = base.FallbackBehavior;
                  return 
                    stub.Result<global::Sem.Sync.SyncBase.Interfaces.Stubs.SIClientBase, string>
                        (this);
                }
            }
        }

        /// <summary>Stub of method System.String Sem.Sync.SyncBase.Interfaces.IClientBase.get_FriendlyClientName()</summary>
        public global::Microsoft.Stubs.Framework.StubDelegates.Func<string> FriendlyClientNameGet;

        /// <summary>Stub of method System.Collections.Generic.List`1&lt;Sem.Sync.SyncBase.StdElement&gt; Sem.Sync.SyncBase.Interfaces.IClientBase.GetAll(System.String clientFolderName)</summary>
        public global::Microsoft.Stubs.Framework.StubDelegates.Func<string, global::System.Collections.Generic.List<global::Sem.Sync.SyncBase.StdElement>> GetAll;

        /// <summary>Stub of property Sem.Sync.SyncBase.Interfaces.IClientBase.LogOnDomain</summary>
        string global::Sem.Sync.SyncBase.Interfaces.IClientBase.LogOnDomain
        {
            [global::System.Diagnostics.DebuggerHidden]
            get
            {
                global::Microsoft.Stubs.Framework.StubDelegates.Func<string> sh
                   = this.LogOnDomainGet;
                if (sh != (global::Microsoft.Stubs.Framework.StubDelegates.Func<string>)null)
                  return sh.Invoke();
                else 
                {
                  global::Microsoft.Stubs.Framework.IStubBehavior stub = base.FallbackBehavior;
                  return 
                    stub.Result<global::Sem.Sync.SyncBase.Interfaces.Stubs.SIClientBase, string>
                        (this);
                }
            }
            [global::System.Diagnostics.DebuggerHidden]
            set
            {
                global::Microsoft.Stubs.Framework.StubDelegates.Action<string> sh
                   = this.LogOnDomainSet;
                if (sh != (global::Microsoft.Stubs.Framework.StubDelegates.Action<string>)null)
                  sh.Invoke(value);
                else 
                {
                  global::Microsoft.Stubs.Framework.IStubBehavior stub = base.FallbackBehavior;
                  stub.VoidResult<global::Sem.Sync.SyncBase.Interfaces.Stubs.SIClientBase>(this);
                }
            }
        }

        /// <summary>Stub of method System.String Sem.Sync.SyncBase.Interfaces.IClientBase.get_LogOnDomain()</summary>
        public global::Microsoft.Stubs.Framework.StubDelegates.Func<string> LogOnDomainGet;

        /// <summary>Stub of method System.Void Sem.Sync.SyncBase.Interfaces.IClientBase.set_LogOnDomain(System.String value)</summary>
        public global::Microsoft.Stubs.Framework.StubDelegates.Action<string> LogOnDomainSet;

        /// <summary>Stub of property Sem.Sync.SyncBase.Interfaces.IClientBase.LogOnPassword</summary>
        string global::Sem.Sync.SyncBase.Interfaces.IClientBase.LogOnPassword
        {
            [global::System.Diagnostics.DebuggerHidden]
            get
            {
                global::Microsoft.Stubs.Framework.StubDelegates.Func<string> sh
                   = this.LogOnPasswordGet;
                if (sh != (global::Microsoft.Stubs.Framework.StubDelegates.Func<string>)null)
                  return sh.Invoke();
                else 
                {
                  global::Microsoft.Stubs.Framework.IStubBehavior stub = base.FallbackBehavior;
                  return 
                    stub.Result<global::Sem.Sync.SyncBase.Interfaces.Stubs.SIClientBase, string>
                        (this);
                }
            }
            [global::System.Diagnostics.DebuggerHidden]
            set
            {
                global::Microsoft.Stubs.Framework.StubDelegates.Action<string> sh
                   = this.LogOnPasswordSet;
                if (sh != (global::Microsoft.Stubs.Framework.StubDelegates.Action<string>)null)
                  sh.Invoke(value);
                else 
                {
                  global::Microsoft.Stubs.Framework.IStubBehavior stub = base.FallbackBehavior;
                  stub.VoidResult<global::Sem.Sync.SyncBase.Interfaces.Stubs.SIClientBase>(this);
                }
            }
        }

        /// <summary>Stub of method System.String Sem.Sync.SyncBase.Interfaces.IClientBase.get_LogOnPassword()</summary>
        public global::Microsoft.Stubs.Framework.StubDelegates.Func<string> LogOnPasswordGet;

        /// <summary>Stub of method System.Void Sem.Sync.SyncBase.Interfaces.IClientBase.set_LogOnPassword(System.String value)</summary>
        public global::Microsoft.Stubs.Framework.StubDelegates.Action<string> LogOnPasswordSet;

        /// <summary>Stub of property Sem.Sync.SyncBase.Interfaces.IClientBase.LogOnUserId</summary>
        string global::Sem.Sync.SyncBase.Interfaces.IClientBase.LogOnUserId
        {
            [global::System.Diagnostics.DebuggerHidden]
            get
            {
                global::Microsoft.Stubs.Framework.StubDelegates.Func<string> sh
                   = this.LogOnUserIdGet;
                if (sh != (global::Microsoft.Stubs.Framework.StubDelegates.Func<string>)null)
                  return sh.Invoke();
                else 
                {
                  global::Microsoft.Stubs.Framework.IStubBehavior stub = base.FallbackBehavior;
                  return 
                    stub.Result<global::Sem.Sync.SyncBase.Interfaces.Stubs.SIClientBase, string>
                        (this);
                }
            }
            [global::System.Diagnostics.DebuggerHidden]
            set
            {
                global::Microsoft.Stubs.Framework.StubDelegates.Action<string> sh
                   = this.LogOnUserIdSet;
                if (sh != (global::Microsoft.Stubs.Framework.StubDelegates.Action<string>)null)
                  sh.Invoke(value);
                else 
                {
                  global::Microsoft.Stubs.Framework.IStubBehavior stub = base.FallbackBehavior;
                  stub.VoidResult<global::Sem.Sync.SyncBase.Interfaces.Stubs.SIClientBase>(this);
                }
            }
        }

        /// <summary>Stub of method System.String Sem.Sync.SyncBase.Interfaces.IClientBase.get_LogOnUserId()</summary>
        public global::Microsoft.Stubs.Framework.StubDelegates.Func<string> LogOnUserIdGet;

        /// <summary>Stub of method System.Void Sem.Sync.SyncBase.Interfaces.IClientBase.set_LogOnUserId(System.String value)</summary>
        public global::Microsoft.Stubs.Framework.StubDelegates.Action<string> LogOnUserIdSet;

        /// <summary>Stub of method System.Void Sem.Sync.SyncBase.Interfaces.IClientBase.MergeMissingItem(Sem.Sync.SyncBase.StdElement element, System.String clientFolderName)</summary>
        public global::Microsoft.Stubs.Framework.StubDelegates.Action<global::Sem.Sync.SyncBase.StdElement, string> MergeMissingItem;

        /// <summary>Stub of method System.Void Sem.Sync.SyncBase.Interfaces.IClientBase.MergeMissingRange(System.Collections.Generic.List`1&lt;Sem.Sync.SyncBase.StdElement&gt; elements, System.String clientFolderName)</summary>
        public global::Microsoft.Stubs.Framework.StubDelegates.Action<global::System.Collections.Generic.List<global::Sem.Sync.SyncBase.StdElement>, string> MergeMissingRange;

        /// <summary>Stub of method System.Collections.Generic.List`1&lt;Sem.Sync.SyncBase.StdElement&gt; Sem.Sync.SyncBase.Interfaces.IClientBase.Normalize(System.Collections.Generic.List`1&lt;Sem.Sync.SyncBase.StdElement&gt; elements)</summary>
        public global::Microsoft.Stubs.Framework.StubDelegates.Func<global::System.Collections.Generic.List<global::Sem.Sync.SyncBase.StdElement>, global::System.Collections.Generic.List<global::Sem.Sync.SyncBase.StdElement>> Normalize;

        /// <summary>Event ProcessingEvent</summary>
        public global::System.EventHandler<global::Sem.Sync.SyncBase.EventArgs.ProcessingEventArgs> ProcessingEvent;

        event global::System.EventHandler<global::Sem.Sync.SyncBase.EventArgs.ProcessingEventArgs> global::Sem.Sync.SyncBase.Interfaces.IClientBase.ProcessingEvent
        {
            [global::System.Diagnostics.DebuggerHidden]
            add
            {
                this.ProcessingEvent = (global::System.EventHandler
                    <global::Sem.Sync.SyncBase.EventArgs.ProcessingEventArgs>)
                  (global::System.Delegate.Combine
                      ((global::System.Delegate)(this.ProcessingEvent), 
                      (global::System.Delegate)value));
            }
            [global::System.Diagnostics.DebuggerHidden]
            remove
            {
                this.ProcessingEvent = (global::System.EventHandler
                    <global::Sem.Sync.SyncBase.EventArgs.ProcessingEventArgs>)
                  (global::System.Delegate.Remove
                      ((global::System.Delegate)(this.ProcessingEvent), 
                      (global::System.Delegate)value));
            }
        }

        /// <summary>Event QueryForLoginCredentialsEvent</summary>
        public global::System.EventHandler<global::Sem.Sync.SyncBase.EventArgs.QueryForLogOnCredentialsEventArgs> QueryForLoginCredentialsEvent;

        event global::System.EventHandler<global::Sem.Sync.SyncBase.EventArgs.QueryForLogOnCredentialsEventArgs> global::Sem.Sync.SyncBase.Interfaces.IClientBase.QueryForLoginCredentialsEvent
        {
            [global::System.Diagnostics.DebuggerHidden]
            add
            {
                this.QueryForLoginCredentialsEvent =
                  (global::System.EventHandler<global::Sem.Sync.SyncBase.EventArgs
                    .QueryForLogOnCredentialsEventArgs>)(global::System.Delegate.Combine
                      ((global::System.Delegate)(this.QueryForLoginCredentialsEvent), 
                      (global::System.Delegate)value));
            }
            [global::System.Diagnostics.DebuggerHidden]
            remove
            {
                this.QueryForLoginCredentialsEvent =
                  (global::System.EventHandler<global::Sem.Sync.SyncBase.EventArgs
                    .QueryForLogOnCredentialsEventArgs>)(global::System.Delegate.Remove
                      ((global::System.Delegate)(this.QueryForLoginCredentialsEvent), 
                      (global::System.Delegate)value));
            }
        }

        /// <summary>Stub of method System.Void Sem.Sync.SyncBase.Interfaces.IClientBase.RemoveDuplicates(System.String clientFolderName)</summary>
        public global::Microsoft.Stubs.Framework.StubDelegates.Action<string> RemoveDuplicates;

        /// <summary>Stub of method System.Void Sem.Sync.SyncBase.Interfaces.IClientBase.AddItem(Sem.Sync.SyncBase.StdElement element, System.String clientFolderName)</summary>
        [global::System.Diagnostics.DebuggerHidden]
        void global::Sem.Sync.SyncBase.Interfaces.IClientBase.AddItem(global::Sem.Sync.SyncBase.StdElement element, string clientFolderName)
        {
            global::Microsoft.Stubs.Framework
              .StubDelegates.Action<global::Sem.Sync.SyncBase.StdElement, string> sh
               = this.AddItem;
            if (sh != (global::Microsoft.Stubs.Framework.StubDelegates
              .Action<global::Sem.Sync.SyncBase.StdElement, string>)null)
              sh.Invoke(element, clientFolderName);
            else 
            {
              global::Microsoft.Stubs.Framework.IStubBehavior stub = base.FallbackBehavior;
              stub.VoidResult<global::Sem.Sync.SyncBase.Interfaces.Stubs.SIClientBase>(this);
            }
        }

        /// <summary>Stub of method System.Void Sem.Sync.SyncBase.Interfaces.IClientBase.AddRange(System.Collections.Generic.List`1&lt;Sem.Sync.SyncBase.StdElement&gt; elements, System.String clientFolderName)</summary>
        [global::System.Diagnostics.DebuggerHidden]
        void global::Sem.Sync.SyncBase.Interfaces.IClientBase.AddRange(global::System.Collections.Generic.List<global::Sem.Sync.SyncBase.StdElement> elements, string clientFolderName)
        {
            global::Microsoft.Stubs.Framework
              .StubDelegates.Action<global::System.Collections.Generic
                .List<global::Sem.Sync.SyncBase.StdElement>, string> sh = this.AddRange;
            if (sh != (global::Microsoft.Stubs.Framework
              .StubDelegates.Action<global::System.Collections.Generic
                .List<global::Sem.Sync.SyncBase.StdElement>, string>)null)
              sh.Invoke(elements, clientFolderName);
            else 
            {
              global::Microsoft.Stubs.Framework.IStubBehavior stub = base.FallbackBehavior;
              stub.VoidResult<global::Sem.Sync.SyncBase.Interfaces.Stubs.SIClientBase>(this);
            }
        }

        /// <summary>Stub of method System.Collections.Generic.List`1&lt;Sem.Sync.SyncBase.StdElement&gt; Sem.Sync.SyncBase.Interfaces.IClientBase.GetAll(System.String clientFolderName)</summary>
        [global::System.Diagnostics.DebuggerHidden]
        global::System.Collections.Generic.List<global::Sem.Sync.SyncBase.StdElement> global::Sem.Sync.SyncBase.Interfaces.IClientBase.GetAll(string clientFolderName)
        {
            global::Microsoft.Stubs.Framework
              .StubDelegates.Func<string, global::System.Collections.Generic
                .List<global::Sem.Sync.SyncBase.StdElement>> sh = this.GetAll;
            if (sh != (global::Microsoft.Stubs.Framework
              .StubDelegates.Func<string, global::System.Collections.Generic
                .List<global::Sem.Sync.SyncBase.StdElement>>)null)
              return sh.Invoke(clientFolderName);
            else 
            {
              global::Microsoft.Stubs.Framework.IStubBehavior stub = base.FallbackBehavior;
              return stub.Result<global::Sem.Sync.SyncBase.Interfaces.Stubs.SIClientBase, 
              global::System.Collections.Generic
                .List<global::Sem.Sync.SyncBase.StdElement>>(this);
            }
        }

        /// <summary>Stub of method System.Void Sem.Sync.SyncBase.Interfaces.IClientBase.MergeMissingItem(Sem.Sync.SyncBase.StdElement element, System.String clientFolderName)</summary>
        [global::System.Diagnostics.DebuggerHidden]
        void global::Sem.Sync.SyncBase.Interfaces.IClientBase.MergeMissingItem(global::Sem.Sync.SyncBase.StdElement element, string clientFolderName)
        {
            global::Microsoft.Stubs.Framework
              .StubDelegates.Action<global::Sem.Sync.SyncBase.StdElement, string> sh
               = this.MergeMissingItem;
            if (sh != (global::Microsoft.Stubs.Framework.StubDelegates
              .Action<global::Sem.Sync.SyncBase.StdElement, string>)null)
              sh.Invoke(element, clientFolderName);
            else 
            {
              global::Microsoft.Stubs.Framework.IStubBehavior stub = base.FallbackBehavior;
              stub.VoidResult<global::Sem.Sync.SyncBase.Interfaces.Stubs.SIClientBase>(this);
            }
        }

        /// <summary>Stub of method System.Void Sem.Sync.SyncBase.Interfaces.IClientBase.MergeMissingRange(System.Collections.Generic.List`1&lt;Sem.Sync.SyncBase.StdElement&gt; elements, System.String clientFolderName)</summary>
        [global::System.Diagnostics.DebuggerHidden]
        void global::Sem.Sync.SyncBase.Interfaces.IClientBase.MergeMissingRange(global::System.Collections.Generic.List<global::Sem.Sync.SyncBase.StdElement> elements, string clientFolderName)
        {
            global::Microsoft.Stubs.Framework
              .StubDelegates.Action<global::System.Collections.Generic
                .List<global::Sem.Sync.SyncBase.StdElement>, string> sh = this.MergeMissingRange
              ;
            if (sh != (global::Microsoft.Stubs.Framework
              .StubDelegates.Action<global::System.Collections.Generic
                .List<global::Sem.Sync.SyncBase.StdElement>, string>)null)
              sh.Invoke(elements, clientFolderName);
            else 
            {
              global::Microsoft.Stubs.Framework.IStubBehavior stub = base.FallbackBehavior;
              stub.VoidResult<global::Sem.Sync.SyncBase.Interfaces.Stubs.SIClientBase>(this);
            }
        }

        /// <summary>Stub of method System.Collections.Generic.List`1&lt;Sem.Sync.SyncBase.StdElement&gt; Sem.Sync.SyncBase.Interfaces.IClientBase.Normalize(System.Collections.Generic.List`1&lt;Sem.Sync.SyncBase.StdElement&gt; elements)</summary>
        [global::System.Diagnostics.DebuggerHidden]
        global::System.Collections.Generic.List<global::Sem.Sync.SyncBase.StdElement> global::Sem.Sync.SyncBase.Interfaces.IClientBase.Normalize(global::System.Collections.Generic.List<global::Sem.Sync.SyncBase.StdElement> elements)
        {
            global::Microsoft.Stubs.Framework
              .StubDelegates.Func<global::System.Collections.Generic
                .List<global::Sem.Sync.SyncBase.StdElement>, 
              global::System.Collections.Generic
                .List<global::Sem.Sync.SyncBase.StdElement>> sh = this.Normalize;
            if (sh != (global::Microsoft.Stubs.Framework
              .StubDelegates.Func<global::System.Collections.Generic
                .List<global::Sem.Sync.SyncBase.StdElement>, 
              global::System.Collections.Generic
                .List<global::Sem.Sync.SyncBase.StdElement>>)null)
              return sh.Invoke(elements);
            else 
            {
              global::Microsoft.Stubs.Framework.IStubBehavior stub = base.FallbackBehavior;
              return stub.Result<global::Sem.Sync.SyncBase.Interfaces.Stubs.SIClientBase, 
              global::System.Collections.Generic
                .List<global::Sem.Sync.SyncBase.StdElement>>(this);
            }
        }

        /// <summary>Stub of method System.Void Sem.Sync.SyncBase.Interfaces.IClientBase.RemoveDuplicates(System.String clientFolderName)</summary>
        [global::System.Diagnostics.DebuggerHidden]
        void global::Sem.Sync.SyncBase.Interfaces.IClientBase.RemoveDuplicates(string clientFolderName)
        {
            global::Microsoft.Stubs.Framework.StubDelegates.Action<string> sh
               = this.RemoveDuplicates;
            if (sh != (global::Microsoft.Stubs.Framework.StubDelegates.Action<string>)null)
              sh.Invoke(clientFolderName);
            else 
            {
              global::Microsoft.Stubs.Framework.IStubBehavior stub = base.FallbackBehavior;
              stub.VoidResult<global::Sem.Sync.SyncBase.Interfaces.Stubs.SIClientBase>(this);
            }
        }

        /// <summary>Stub of method System.Void Sem.Sync.SyncBase.Interfaces.IClientBase.WriteRange(System.Collections.Generic.List`1&lt;Sem.Sync.SyncBase.StdElement&gt; elements, System.String clientFolderName)</summary>
        [global::System.Diagnostics.DebuggerHidden]
        void global::Sem.Sync.SyncBase.Interfaces.IClientBase.WriteRange(global::System.Collections.Generic.List<global::Sem.Sync.SyncBase.StdElement> elements, string clientFolderName)
        {
            global::Microsoft.Stubs.Framework
              .StubDelegates.Action<global::System.Collections.Generic
                .List<global::Sem.Sync.SyncBase.StdElement>, string> sh = this.WriteRange;
            if (sh != (global::Microsoft.Stubs.Framework
              .StubDelegates.Action<global::System.Collections.Generic
                .List<global::Sem.Sync.SyncBase.StdElement>, string>)null)
              sh.Invoke(elements, clientFolderName);
            else 
            {
              global::Microsoft.Stubs.Framework.IStubBehavior stub = base.FallbackBehavior;
              stub.VoidResult<global::Sem.Sync.SyncBase.Interfaces.Stubs.SIClientBase>(this);
            }
        }

        /// <summary>Stub of method System.Void Sem.Sync.SyncBase.Interfaces.IClientBase.WriteRange(System.Collections.Generic.List`1&lt;Sem.Sync.SyncBase.StdElement&gt; elements, System.String clientFolderName)</summary>
        public global::Microsoft.Stubs.Framework.StubDelegates.Action<global::System.Collections.Generic.List<global::Sem.Sync.SyncBase.StdElement>, string> WriteRange;
    }
}
namespace Sem.Sync.SyncBase.Interfaces.Stubs
{
    /// <summary>Stub of IMergeConflictResolver</summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Stubs", "0.13.40528.2")]
    [global::System.Serializable]
    [global::System.Diagnostics.DebuggerDisplay("Stub = IMergeConflictResolver")]
    [global::System.Diagnostics.DebuggerStepThrough]
    public partial class SIMergeConflictResolver
      : global::Microsoft.Stubs.Framework.StubBase
      , global::Sem.Sync.SyncBase.Interfaces.IMergeConflictResolver
    {
        /// <summary>Initializes a new instance of type SIMergeConflictResolver</summary>
        [global::System.Diagnostics.DebuggerHidden]
        public SIMergeConflictResolver()
        {
        }

        /// <summary>Stub of method System.Collections.Generic.List`1&lt;Sem.Sync.SyncBase.StdElement&gt; Sem.Sync.SyncBase.Interfaces.IMergeConflictResolver.PerformAttributeMerge(System.Collections.Generic.List`1&lt;Sem.Sync.SyncBase.Merging.MergeConflict&gt; toMerge, System.Collections.Generic.List`1&lt;Sem.Sync.SyncBase.StdElement&gt; targetList)</summary>
        public global::Microsoft.Stubs.Framework.StubDelegates.Func<global::System.Collections.Generic.List<global::Sem.Sync.SyncBase.Merging.MergeConflict>, global::System.Collections.Generic.List<global::Sem.Sync.SyncBase.StdElement>, global::System.Collections.Generic.List<global::Sem.Sync.SyncBase.StdElement>> PerformAttributeMerge;

        /// <summary>Stub of method System.Collections.Generic.List`1&lt;Sem.Sync.SyncBase.StdElement&gt; Sem.Sync.SyncBase.Interfaces.IMergeConflictResolver.PerformEntityMerge(System.Collections.Generic.List`1&lt;Sem.Sync.SyncBase.StdElement&gt; sourceList, System.Collections.Generic.List`1&lt;Sem.Sync.SyncBase.StdElement&gt; targetList, System.Collections.Generic.List`1&lt;Sem.Sync.SyncBase.StdElement&gt; baselineList)</summary>
        public global::Microsoft.Stubs.Framework.StubDelegates.Func<global::System.Collections.Generic.List<global::Sem.Sync.SyncBase.StdElement>, global::System.Collections.Generic.List<global::Sem.Sync.SyncBase.StdElement>, global::System.Collections.Generic.List<global::Sem.Sync.SyncBase.StdElement>, global::System.Collections.Generic.List<global::Sem.Sync.SyncBase.StdElement>> PerformEntityMerge;

        /// <summary>Stub of method System.Collections.Generic.List`1&lt;Sem.Sync.SyncBase.StdElement&gt; Sem.Sync.SyncBase.Interfaces.IMergeConflictResolver.PerformAttributeMerge(System.Collections.Generic.List`1&lt;Sem.Sync.SyncBase.Merging.MergeConflict&gt; toMerge, System.Collections.Generic.List`1&lt;Sem.Sync.SyncBase.StdElement&gt; targetList)</summary>
        [global::System.Diagnostics.DebuggerHidden]
        global::System.Collections.Generic.List<global::Sem.Sync.SyncBase.StdElement> global::Sem.Sync.SyncBase.Interfaces.IMergeConflictResolver.PerformAttributeMerge(global::System.Collections.Generic.List<global::Sem.Sync.SyncBase.Merging.MergeConflict> toMerge, global::System.Collections.Generic.List<global::Sem.Sync.SyncBase.StdElement> targetList)
        {
            global::Microsoft.Stubs.Framework
              .StubDelegates.Func<global::System.Collections.Generic
                .List<global::Sem.Sync.SyncBase.Merging
                  .MergeConflict>, global::System.Collections.Generic
                .List<global::Sem.Sync.SyncBase.StdElement>, 
              global::System.Collections.Generic
                .List<global::Sem.Sync.SyncBase.StdElement>> sh = this.PerformAttributeMerge;
            if (sh != (global::Microsoft.Stubs.Framework
              .StubDelegates.Func<global::System.Collections.Generic
                .List<global::Sem.Sync.SyncBase.Merging
                  .MergeConflict>, global::System.Collections.Generic
                .List<global::Sem.Sync.SyncBase.StdElement>, 
              global::System.Collections.Generic
                .List<global::Sem.Sync.SyncBase.StdElement>>)null)
              return sh.Invoke(toMerge, targetList);
            else 
            {
              global::Microsoft.Stubs.Framework.IStubBehavior stub = base.FallbackBehavior;
              return stub.Result
                  <global::Sem.Sync.SyncBase.Interfaces.Stubs.SIMergeConflictResolver, 
                  global::System.Collections.Generic
                    .List<global::Sem.Sync.SyncBase.StdElement>>(this);
            }
        }

        /// <summary>Stub of method System.Collections.Generic.List`1&lt;Sem.Sync.SyncBase.StdElement&gt; Sem.Sync.SyncBase.Interfaces.IMergeConflictResolver.PerformEntityMerge(System.Collections.Generic.List`1&lt;Sem.Sync.SyncBase.StdElement&gt; sourceList, System.Collections.Generic.List`1&lt;Sem.Sync.SyncBase.StdElement&gt; targetList, System.Collections.Generic.List`1&lt;Sem.Sync.SyncBase.StdElement&gt; baselineList)</summary>
        [global::System.Diagnostics.DebuggerHidden]
        global::System.Collections.Generic.List<global::Sem.Sync.SyncBase.StdElement> global::Sem.Sync.SyncBase.Interfaces.IMergeConflictResolver.PerformEntityMerge(
            global::System.Collections.Generic.List<global::Sem.Sync.SyncBase.StdElement> sourceList,
            global::System.Collections.Generic.List<global::Sem.Sync.SyncBase.StdElement> targetList,
            global::System.Collections.Generic.List<global::Sem.Sync.SyncBase.StdElement> baselineList
        )
        {
            global::Microsoft.Stubs.Framework
              .StubDelegates.Func<global::System.Collections.Generic
                .List<global::Sem.Sync.SyncBase.StdElement>, 
              global::System.Collections.Generic
                .List<global::Sem.Sync.SyncBase.StdElement>, 
              global::System.Collections.Generic
                .List<global::Sem.Sync.SyncBase.StdElement>, 
              global::System.Collections.Generic
                .List<global::Sem.Sync.SyncBase.StdElement>> sh = this.PerformEntityMerge;
            if (sh != (global::Microsoft.Stubs.Framework
              .StubDelegates.Func<global::System.Collections.Generic
                .List<global::Sem.Sync.SyncBase.StdElement>, 
              global::System.Collections.Generic
                .List<global::Sem.Sync.SyncBase.StdElement>, 
              global::System.Collections.Generic
                .List<global::Sem.Sync.SyncBase.StdElement>, 
              global::System.Collections.Generic
                .List<global::Sem.Sync.SyncBase.StdElement>>)null)
              return sh.Invoke(sourceList, targetList, baselineList);
            else 
            {
              global::Microsoft.Stubs.Framework.IStubBehavior stub = base.FallbackBehavior;
              return stub.Result
                  <global::Sem.Sync.SyncBase.Interfaces.Stubs.SIMergeConflictResolver, 
                  global::System.Collections.Generic
                    .List<global::Sem.Sync.SyncBase.StdElement>>(this);
            }
        }
    }
}
namespace Sem.Sync.SyncBase.Interfaces.Stubs
{
    /// <summary>Stub of IUiInteraction</summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Stubs", "0.13.40528.2")]
    [global::System.Serializable]
    [global::System.Diagnostics.DebuggerDisplay("Stub = IUiInteraction")]
    [global::System.Diagnostics.DebuggerStepThrough]
    public partial class SIUiInteraction
      : global::Microsoft.Stubs.Framework.StubBase
      , global::Sem.Sync.SyncBase.Interfaces.IUiInteraction
    {
        /// <summary>Initializes a new instance of type SIUiInteraction</summary>
        [global::System.Diagnostics.DebuggerHidden]
        public SIUiInteraction()
        {
        }

        /// <summary>Stub of method System.Boolean Sem.Sync.SyncBase.Interfaces.IUiInteraction.AskForConfirm(System.String messageForUser, System.String title)</summary>
        public global::Microsoft.Stubs.Framework.StubDelegates.Func<string, string, bool> AskForConfirm;

        /// <summary>Stub of method System.Boolean Sem.Sync.SyncBase.Interfaces.IUiInteraction.AskForLogOnCredentials(Sem.Sync.SyncBase.Interfaces.IClientBase client, System.String messageForUser, System.String logOnUserId, System.String logOnPassword)</summary>
        public global::Microsoft.Stubs.Framework.StubDelegates.Func<global::Sem.Sync.SyncBase.Interfaces.IClientBase, string, string, string, bool> AskForLogOnCredentials;

        /// <summary>Stub of method System.Boolean Sem.Sync.SyncBase.Interfaces.IUiInteraction.AskForConfirm(System.String messageForUser, System.String title)</summary>
        [global::System.Diagnostics.DebuggerHidden]
        bool global::Sem.Sync.SyncBase.Interfaces.IUiInteraction.AskForConfirm(string messageForUser, string title)
        {
            global::Microsoft.Stubs.Framework.StubDelegates.Func<string, string, bool> sh
               = this.AskForConfirm;
            if (sh !=
              (global::Microsoft.Stubs.Framework.StubDelegates.Func<string, string, bool>)
                null)
              return sh.Invoke(messageForUser, title);
            else 
            {
              global::Microsoft.Stubs.Framework.IStubBehavior stub = base.FallbackBehavior;
              return stub
                .Result<global::Sem.Sync.SyncBase.Interfaces.Stubs.SIUiInteraction, bool>
                  (this);
            }
        }

        /// <summary>Stub of method System.Boolean Sem.Sync.SyncBase.Interfaces.IUiInteraction.AskForLogOnCredentials(Sem.Sync.SyncBase.Interfaces.IClientBase client, System.String messageForUser, System.String logOnUserId, System.String logOnPassword)</summary>
        [global::System.Diagnostics.DebuggerHidden]
        bool global::Sem.Sync.SyncBase.Interfaces.IUiInteraction.AskForLogOnCredentials(
            global::Sem.Sync.SyncBase.Interfaces.IClientBase client,
            string messageForUser,
            string logOnUserId,
            string logOnPassword
        )
        {
            global::Microsoft.Stubs.Framework.StubDelegates
              .Func<global::Sem.Sync.SyncBase.Interfaces.IClientBase, string, 
              string, string, bool> sh = this.AskForLogOnCredentials;
            if (sh != (global::Microsoft.Stubs.Framework.StubDelegates
              .Func<global::Sem.Sync.SyncBase.Interfaces.IClientBase, 
              string, string, string, bool>)null)
              return sh.Invoke(client, messageForUser, logOnUserId, logOnPassword);
            else 
            {
              global::Microsoft.Stubs.Framework.IStubBehavior stub = base.FallbackBehavior;
              return stub
                .Result<global::Sem.Sync.SyncBase.Interfaces.Stubs.SIUiInteraction, bool>
                  (this);
            }
        }
    }
}
namespace Sem.Sync.SyncBase.DetailData.Stubs
{
    /// <summary>Stub of InstantMessengerAddresses</summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Stubs", "0.13.40528.2")]
    [global::System.Serializable]
    [global::System.Diagnostics.DebuggerDisplay("Stub = InstantMessengerAddresses")]
    [global::System.Diagnostics.DebuggerStepThrough]
    public partial class SInstantMessengerAddresses
      : global::Sem.Sync.SyncBase.DetailData.InstantMessengerAddresses
      , global::Microsoft.Stubs.Framework.IStub
      , global::Microsoft.Stubs.Framework.IPartialStub
    {
        /// <summary>Initializes a new instance of type SInstantMessengerAddresses</summary>
        [global::System.Diagnostics.DebuggerHidden]
        public SInstantMessengerAddresses()
        {
            this.defaultStub =
              global::Microsoft.Stubs.Framework.StubFallbackBehavior.Current;
        }

        /// <summary>Gets or sets a value that indicates if the base method should be called instead of the fallback behavior</summary>
        public bool CallBase
        {
            [global::System.Diagnostics.DebuggerHidden]
            get
            {
                return this.callBase;
            }
            [global::System.Diagnostics.DebuggerHidden]
            set
            {
                this.callBase = value;
            }
        }

        /// <summary>Gets or sets the fallback behavior.</summary>
        public global::Microsoft.Stubs.Framework.IStubBehavior FallbackBehavior
        {
            [global::System.Diagnostics.DebuggerHidden]
            get
            {
                return this.defaultStub;
            }
            [global::System.Diagnostics.DebuggerHidden]
            set
            {
                this.defaultStub = value;
            }
        }

        private bool callBase;

        private global::Microsoft.Stubs.Framework.IStubBehavior defaultStub;
    }
}
namespace Sem.Sync.SyncBase.DetailData.Stubs
{
    /// <summary>Stub of KeyValuePair</summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Stubs", "0.13.40528.2")]
    [global::System.Serializable]
    [global::System.Diagnostics.DebuggerDisplay("Stub = KeyValuePair")]
    [global::System.Diagnostics.DebuggerStepThrough]
    public partial class SKeyValuePair
      : global::Sem.Sync.SyncBase.DetailData.KeyValuePair
      , global::Microsoft.Stubs.Framework.IStub
      , global::Microsoft.Stubs.Framework.IPartialStub
    {
        /// <summary>Initializes a new instance of type SKeyValuePair</summary>
        [global::System.Diagnostics.DebuggerHidden]
        public SKeyValuePair()
        {
            this.defaultStub =
              global::Microsoft.Stubs.Framework.StubFallbackBehavior.Current;
        }

        /// <summary>Gets or sets a value that indicates if the base method should be called instead of the fallback behavior</summary>
        public bool CallBase
        {
            [global::System.Diagnostics.DebuggerHidden]
            get
            {
                return this.callBase;
            }
            [global::System.Diagnostics.DebuggerHidden]
            set
            {
                this.callBase = value;
            }
        }

        /// <summary>Gets or sets the fallback behavior.</summary>
        public global::Microsoft.Stubs.Framework.IStubBehavior FallbackBehavior
        {
            [global::System.Diagnostics.DebuggerHidden]
            get
            {
                return this.defaultStub;
            }
            [global::System.Diagnostics.DebuggerHidden]
            set
            {
                this.defaultStub = value;
            }
        }

        private bool callBase;

        private global::Microsoft.Stubs.Framework.IStubBehavior defaultStub;
    }
}
namespace Sem.Sync.SyncBase.DetailData.Stubs
{
    /// <summary>Stub of MatchingEntry</summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Stubs", "0.13.40528.2")]
    [global::System.Serializable]
    [global::System.Diagnostics.DebuggerDisplay("Stub = MatchingEntry")]
    [global::System.Diagnostics.DebuggerStepThrough]
    public partial class SMatchingEntry
      : global::Sem.Sync.SyncBase.DetailData.MatchingEntry
      , global::Microsoft.Stubs.Framework.IStub
      , global::Microsoft.Stubs.Framework.IPartialStub
    {
        /// <summary>Initializes a new instance of type SMatchingEntry</summary>
        [global::System.Diagnostics.DebuggerHidden]
        public SMatchingEntry()
        {
            this.defaultStub =
              global::Microsoft.Stubs.Framework.StubFallbackBehavior.Current;
        }

        /// <summary>Gets or sets a value that indicates if the base method should be called instead of the fallback behavior</summary>
        public bool CallBase
        {
            [global::System.Diagnostics.DebuggerHidden]
            get
            {
                return this.callBase;
            }
            [global::System.Diagnostics.DebuggerHidden]
            set
            {
                this.callBase = value;
            }
        }

        /// <summary>Stub of method System.Int32 Sem.Sync.SyncBase.StdElement.CompareTo(Sem.Sync.SyncBase.StdElement other)</summary>
        [global::System.Diagnostics.DebuggerHidden]
        public override int CompareTo(global::Sem.Sync.SyncBase.StdElement other)
        {
            global::Microsoft.Stubs.Framework
              .StubDelegates.Func<global::Sem.Sync.SyncBase.StdElement, int> sh
               = this.CompareToStdElement;
            if (sh != (global::Microsoft.Stubs.Framework
              .StubDelegates.Func<global::Sem.Sync.SyncBase.StdElement, int>)null)
              return sh.Invoke(other);
            else 
            {
              if (this.callBase)
                return base.CompareTo(other);
              global::Microsoft.Stubs.Framework.IStubBehavior stub = this.FallbackBehavior;
              return 
                stub.Result<global::Sem.Sync.SyncBase.DetailData.Stubs.SMatchingEntry, int>
                    (this);
            }
        }

        /// <summary>Stub of method System.Int32 Sem.Sync.SyncBase.StdElement.CompareTo(Sem.Sync.SyncBase.StdElement other)</summary>
        public global::Microsoft.Stubs.Framework.StubDelegates.Func<global::Sem.Sync.SyncBase.StdElement, int> CompareToStdElement;

        /// <summary>Gets or sets the fallback behavior.</summary>
        public global::Microsoft.Stubs.Framework.IStubBehavior FallbackBehavior
        {
            [global::System.Diagnostics.DebuggerHidden]
            get
            {
                return this.defaultStub;
            }
            [global::System.Diagnostics.DebuggerHidden]
            set
            {
                this.defaultStub = value;
            }
        }

        /// <summary>Stub of method System.Void Sem.Sync.SyncBase.StdElement.NormalizeContent()</summary>
        [global::System.Diagnostics.DebuggerHidden]
        public override void NormalizeContent()
        {
            global::Microsoft.Stubs.Framework.StubDelegates.Action sh
               = this.NormalizeContent01;
            if (sh != (global::Microsoft.Stubs.Framework.StubDelegates.Action)null)
              sh.Invoke();
            else 
            {
              global::Microsoft.Stubs.Framework.IStubBehavior stub = this.FallbackBehavior;
              stub.VoidResult<global::Sem.Sync.SyncBase.DetailData.Stubs.SMatchingEntry>
                  (this);
            }
        }

        /// <summary>Stub of method System.Void Sem.Sync.SyncBase.StdElement.NormalizeContent()</summary>
        public global::Microsoft.Stubs.Framework.StubDelegates.Action NormalizeContent01;

        /// <summary>Stub of method System.String Sem.Sync.SyncBase.StdElement.ToSortSimple()</summary>
        [global::System.Diagnostics.DebuggerHidden]
        public override string ToSortSimple()
        {
            global::Microsoft.Stubs.Framework.StubDelegates.Func<string> sh
               = this.ToSortSimple01;
            if (sh != (global::Microsoft.Stubs.Framework.StubDelegates.Func<string>)null)
              return sh.Invoke();
            else 
            {
              if (this.callBase)
                return base.ToSortSimple();
              global::Microsoft.Stubs.Framework.IStubBehavior stub = this.FallbackBehavior;
              return stub.Result
                  <global::Sem.Sync.SyncBase.DetailData.Stubs.SMatchingEntry, string>(this);
            }
        }

        /// <summary>Stub of method System.String Sem.Sync.SyncBase.StdElement.ToSortSimple()</summary>
        public global::Microsoft.Stubs.Framework.StubDelegates.Func<string> ToSortSimple01;

        /// <summary>Stub of method System.String Sem.Sync.SyncBase.StdElement.ToStringSimple()</summary>
        [global::System.Diagnostics.DebuggerHidden]
        public override string ToStringSimple()
        {
            global::Microsoft.Stubs.Framework.StubDelegates.Func<string> sh
               = this.ToStringSimple01;
            if (sh != (global::Microsoft.Stubs.Framework.StubDelegates.Func<string>)null)
              return sh.Invoke();
            else 
            {
              if (this.callBase)
                return base.ToStringSimple();
              global::Microsoft.Stubs.Framework.IStubBehavior stub = this.FallbackBehavior;
              return stub.Result
                  <global::Sem.Sync.SyncBase.DetailData.Stubs.SMatchingEntry, string>(this);
            }
        }

        /// <summary>Stub of method System.String Sem.Sync.SyncBase.StdElement.ToStringSimple()</summary>
        public global::Microsoft.Stubs.Framework.StubDelegates.Func<string> ToStringSimple01;

        private bool callBase;

        private global::Microsoft.Stubs.Framework.IStubBehavior defaultStub;
    }
}
namespace Sem.Sync.SyncBase.Merging.Stubs
{
    /// <summary>Stub of MergeConflict</summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Stubs", "0.13.40528.2")]
    [global::System.Serializable]
    [global::System.Diagnostics.DebuggerDisplay("Stub = MergeConflict")]
    [global::System.Diagnostics.DebuggerStepThrough]
    public partial class SMergeConflict
      : global::Sem.Sync.SyncBase.Merging.MergeConflict
      , global::Microsoft.Stubs.Framework.IStub
      , global::Microsoft.Stubs.Framework.IPartialStub
    {
        /// <summary>Initializes a new instance of type SMergeConflict</summary>
        [global::System.Diagnostics.DebuggerHidden]
        public SMergeConflict()
        {
            this.defaultStub =
              global::Microsoft.Stubs.Framework.StubFallbackBehavior.Current;
        }

        /// <summary>Gets or sets a value that indicates if the base method should be called instead of the fallback behavior</summary>
        public bool CallBase
        {
            [global::System.Diagnostics.DebuggerHidden]
            get
            {
                return this.callBase;
            }
            [global::System.Diagnostics.DebuggerHidden]
            set
            {
                this.callBase = value;
            }
        }

        /// <summary>Gets or sets the fallback behavior.</summary>
        public global::Microsoft.Stubs.Framework.IStubBehavior FallbackBehavior
        {
            [global::System.Diagnostics.DebuggerHidden]
            get
            {
                return this.defaultStub;
            }
            [global::System.Diagnostics.DebuggerHidden]
            set
            {
                this.defaultStub = value;
            }
        }

        private bool callBase;

        private global::Microsoft.Stubs.Framework.IStubBehavior defaultStub;
    }
}
namespace Sem.Sync.SyncBase.DetailData.Stubs
{
    /// <summary>Stub of PersonName</summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Stubs", "0.13.40528.2")]
    [global::System.Serializable]
    [global::System.Diagnostics.DebuggerDisplay("Stub = PersonName")]
    [global::System.Diagnostics.DebuggerStepThrough]
    public partial class SPersonName
      : global::Sem.Sync.SyncBase.DetailData.PersonName
      , global::Microsoft.Stubs.Framework.IStub
      , global::Microsoft.Stubs.Framework.IPartialStub
    {
        /// <summary>Initializes a new instance of type SPersonName</summary>
        [global::System.Diagnostics.DebuggerHidden]
        public SPersonName()
        {
            this.defaultStub =
              global::Microsoft.Stubs.Framework.StubFallbackBehavior.Current;
        }

        /// <summary>Gets or sets a value that indicates if the base method should be called instead of the fallback behavior</summary>
        public bool CallBase
        {
            [global::System.Diagnostics.DebuggerHidden]
            get
            {
                return this.callBase;
            }
            [global::System.Diagnostics.DebuggerHidden]
            set
            {
                this.callBase = value;
            }
        }

        /// <summary>Gets or sets the fallback behavior.</summary>
        public global::Microsoft.Stubs.Framework.IStubBehavior FallbackBehavior
        {
            [global::System.Diagnostics.DebuggerHidden]
            get
            {
                return this.defaultStub;
            }
            [global::System.Diagnostics.DebuggerHidden]
            set
            {
                this.defaultStub = value;
            }
        }

        private bool callBase;

        private global::Microsoft.Stubs.Framework.IStubBehavior defaultStub;
    }
}
namespace Sem.Sync.SyncBase.DetailData.Stubs
{
    /// <summary>Stub of PhoneNumber</summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Stubs", "0.13.40528.2")]
    [global::System.Serializable]
    [global::System.Diagnostics.DebuggerDisplay("Stub = PhoneNumber")]
    [global::System.Diagnostics.DebuggerStepThrough]
    public partial class SPhoneNumber
      : global::Sem.Sync.SyncBase.DetailData.PhoneNumber
      , global::Microsoft.Stubs.Framework.IStub
      , global::Microsoft.Stubs.Framework.IPartialStub
    {
        /// <summary>Initializes a new instance of type SPhoneNumber</summary>
        [global::System.Diagnostics.DebuggerHidden]
        public SPhoneNumber()
        {
            this.defaultStub =
              global::Microsoft.Stubs.Framework.StubFallbackBehavior.Current;
        }

        /// <summary>Gets or sets a value that indicates if the base method should be called instead of the fallback behavior</summary>
        public bool CallBase
        {
            [global::System.Diagnostics.DebuggerHidden]
            get
            {
                return this.callBase;
            }
            [global::System.Diagnostics.DebuggerHidden]
            set
            {
                this.callBase = value;
            }
        }

        /// <summary>Gets or sets the fallback behavior.</summary>
        public global::Microsoft.Stubs.Framework.IStubBehavior FallbackBehavior
        {
            [global::System.Diagnostics.DebuggerHidden]
            get
            {
                return this.defaultStub;
            }
            [global::System.Diagnostics.DebuggerHidden]
            set
            {
                this.defaultStub = value;
            }
        }

        /// <summary>Stub of method System.String Sem.Sync.SyncBase.DetailData.PhoneNumber.ToString()</summary>
        [global::System.Diagnostics.DebuggerHidden]
        public override string ToString()
        {
            global::Microsoft.Stubs.Framework.StubDelegates.Func<string> sh
               = this.ToString01;
            if (sh != (global::Microsoft.Stubs.Framework.StubDelegates.Func<string>)null)
              return sh.Invoke();
            else 
            {
              if (this.callBase)
                return base.ToString();
              global::Microsoft.Stubs.Framework.IStubBehavior stub = this.FallbackBehavior;
              return 
                stub.Result<global::Sem.Sync.SyncBase.DetailData.Stubs.SPhoneNumber, string>
                    (this);
            }
        }

        /// <summary>Stub of method System.String Sem.Sync.SyncBase.DetailData.PhoneNumber.ToString()</summary>
        public global::Microsoft.Stubs.Framework.StubDelegates.Func<string> ToString01;

        private bool callBase;

        private global::Microsoft.Stubs.Framework.IStubBehavior defaultStub;
    }
}
namespace Sem.Sync.SyncBase.EventArgs.Stubs
{
    /// <summary>Stub of ProcessingEventArgs</summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Stubs", "0.13.40528.2")]
    [global::System.Serializable]
    [global::System.Diagnostics.DebuggerDisplay("Stub = ProcessingEventArgs")]
    [global::System.Diagnostics.DebuggerStepThrough]
    public partial class SProcessingEventArgs
      : global::Sem.Sync.SyncBase.EventArgs.ProcessingEventArgs
      , global::Microsoft.Stubs.Framework.IStub
      , global::Microsoft.Stubs.Framework.IPartialStub
    {
        /// <summary>Initializes a new instance of type SProcessingEventArgs</summary>
        [global::System.Diagnostics.DebuggerHidden]
        public SProcessingEventArgs()
        {
            this.defaultStub =
              global::Microsoft.Stubs.Framework.StubFallbackBehavior.Current;
        }

        /// <summary>Gets or sets a value that indicates if the base method should be called instead of the fallback behavior</summary>
        public bool CallBase
        {
            [global::System.Diagnostics.DebuggerHidden]
            get
            {
                return this.callBase;
            }
            [global::System.Diagnostics.DebuggerHidden]
            set
            {
                this.callBase = value;
            }
        }

        /// <summary>Gets or sets the fallback behavior.</summary>
        public global::Microsoft.Stubs.Framework.IStubBehavior FallbackBehavior
        {
            [global::System.Diagnostics.DebuggerHidden]
            get
            {
                return this.defaultStub;
            }
            [global::System.Diagnostics.DebuggerHidden]
            set
            {
                this.defaultStub = value;
            }
        }

        private bool callBase;

        private global::Microsoft.Stubs.Framework.IStubBehavior defaultStub;
    }
}
namespace Sem.Sync.SyncBase.DetailData.Stubs
{
    /// <summary>Stub of ProfileIdentifiers</summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Stubs", "0.13.40528.2")]
    [global::System.Serializable]
    [global::System.Diagnostics.DebuggerDisplay("Stub = ProfileIdentifiers")]
    [global::System.Diagnostics.DebuggerStepThrough]
    public partial class SProfileIdentifiers
      : global::Sem.Sync.SyncBase.DetailData.ProfileIdentifiers
      , global::Microsoft.Stubs.Framework.IStub
      , global::Microsoft.Stubs.Framework.IPartialStub
    {
        /// <summary>Initializes a new instance of type SProfileIdentifiers</summary>
        [global::System.Diagnostics.DebuggerHidden]
        public SProfileIdentifiers()
        {
            this.defaultStub =
              global::Microsoft.Stubs.Framework.StubFallbackBehavior.Current;
        }

        /// <summary>Gets or sets a value that indicates if the base method should be called instead of the fallback behavior</summary>
        public bool CallBase
        {
            [global::System.Diagnostics.DebuggerHidden]
            get
            {
                return this.callBase;
            }
            [global::System.Diagnostics.DebuggerHidden]
            set
            {
                this.callBase = value;
            }
        }

        /// <summary>Gets or sets the fallback behavior.</summary>
        public global::Microsoft.Stubs.Framework.IStubBehavior FallbackBehavior
        {
            [global::System.Diagnostics.DebuggerHidden]
            get
            {
                return this.defaultStub;
            }
            [global::System.Diagnostics.DebuggerHidden]
            set
            {
                this.defaultStub = value;
            }
        }

        private bool callBase;

        private global::Microsoft.Stubs.Framework.IStubBehavior defaultStub;
    }
}
namespace Sem.Sync.SyncBase.EventArgs.Stubs
{
    /// <summary>Stub of ProgressEventArgs</summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Stubs", "0.13.40528.2")]
    [global::System.Serializable]
    [global::System.Diagnostics.DebuggerDisplay("Stub = ProgressEventArgs")]
    [global::System.Diagnostics.DebuggerStepThrough]
    public partial class SProgressEventArgs
      : global::Sem.Sync.SyncBase.EventArgs.ProgressEventArgs
      , global::Microsoft.Stubs.Framework.IStub
      , global::Microsoft.Stubs.Framework.IPartialStub
    {
        /// <summary>Initializes a new instance of type SProgressEventArgs</summary>
        [global::System.Diagnostics.DebuggerHidden]
        public SProgressEventArgs()
        {
            this.defaultStub =
              global::Microsoft.Stubs.Framework.StubFallbackBehavior.Current;
        }

        /// <summary>Gets or sets a value that indicates if the base method should be called instead of the fallback behavior</summary>
        public bool CallBase
        {
            [global::System.Diagnostics.DebuggerHidden]
            get
            {
                return this.callBase;
            }
            [global::System.Diagnostics.DebuggerHidden]
            set
            {
                this.callBase = value;
            }
        }

        /// <summary>Gets or sets the fallback behavior.</summary>
        public global::Microsoft.Stubs.Framework.IStubBehavior FallbackBehavior
        {
            [global::System.Diagnostics.DebuggerHidden]
            get
            {
                return this.defaultStub;
            }
            [global::System.Diagnostics.DebuggerHidden]
            set
            {
                this.defaultStub = value;
            }
        }

        private bool callBase;

        private global::Microsoft.Stubs.Framework.IStubBehavior defaultStub;
    }
}
namespace Sem.Sync.SyncBase.EventArgs.Stubs
{
    /// <summary>Stub of QueryForLogOnCredentialsEventArgs</summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Stubs", "0.13.40528.2")]
    [global::System.Serializable]
    [global::System.Diagnostics.DebuggerDisplay("Stub = QueryForLogOnCredentialsEventArgs")]
    [global::System.Diagnostics.DebuggerStepThrough]
    public partial class SQueryForLogOnCredentialsEventArgs
      : global::Sem.Sync.SyncBase.EventArgs.QueryForLogOnCredentialsEventArgs
      , global::Microsoft.Stubs.Framework.IStub
      , global::Microsoft.Stubs.Framework.IPartialStub
    {
        /// <summary>Initializes a new instance of type SQueryForLogOnCredentialsEventArgs</summary>
        [global::System.Diagnostics.DebuggerHidden]
        public SQueryForLogOnCredentialsEventArgs()
        {
            this.defaultStub =
              global::Microsoft.Stubs.Framework.StubFallbackBehavior.Current;
        }

        /// <summary>Gets or sets a value that indicates if the base method should be called instead of the fallback behavior</summary>
        public bool CallBase
        {
            [global::System.Diagnostics.DebuggerHidden]
            get
            {
                return this.callBase;
            }
            [global::System.Diagnostics.DebuggerHidden]
            set
            {
                this.callBase = value;
            }
        }

        /// <summary>Gets or sets the fallback behavior.</summary>
        public global::Microsoft.Stubs.Framework.IStubBehavior FallbackBehavior
        {
            [global::System.Diagnostics.DebuggerHidden]
            get
            {
                return this.defaultStub;
            }
            [global::System.Diagnostics.DebuggerHidden]
            set
            {
                this.defaultStub = value;
            }
        }

        private bool callBase;

        private global::Microsoft.Stubs.Framework.IStubBehavior defaultStub;
    }
}
namespace Sem.Sync.SyncBase.DetailData.Stubs
{
    /// <summary>Stub of ReplacementLists</summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Stubs", "0.13.40528.2")]
    [global::System.Serializable]
    [global::System.Diagnostics.DebuggerDisplay("Stub = ReplacementLists")]
    [global::System.Diagnostics.DebuggerStepThrough]
    public partial class SReplacementLists
      : global::Sem.Sync.SyncBase.DetailData.ReplacementLists
      , global::Microsoft.Stubs.Framework.IStub
      , global::Microsoft.Stubs.Framework.IPartialStub
    {
        /// <summary>Initializes a new instance of type SReplacementLists</summary>
        [global::System.Diagnostics.DebuggerHidden]
        public SReplacementLists()
        {
            this.defaultStub =
              global::Microsoft.Stubs.Framework.StubFallbackBehavior.Current;
        }

        /// <summary>Gets or sets a value that indicates if the base method should be called instead of the fallback behavior</summary>
        public bool CallBase
        {
            [global::System.Diagnostics.DebuggerHidden]
            get
            {
                return this.callBase;
            }
            [global::System.Diagnostics.DebuggerHidden]
            set
            {
                this.callBase = value;
            }
        }

        /// <summary>Gets or sets the fallback behavior.</summary>
        public global::Microsoft.Stubs.Framework.IStubBehavior FallbackBehavior
        {
            [global::System.Diagnostics.DebuggerHidden]
            get
            {
                return this.defaultStub;
            }
            [global::System.Diagnostics.DebuggerHidden]
            set
            {
                this.defaultStub = value;
            }
        }

        private bool callBase;

        private global::Microsoft.Stubs.Framework.IStubBehavior defaultStub;
    }
}
namespace Sem.Sync.SyncBase.Stubs
{
    /// <summary>Stub of StdCalendarItem</summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Stubs", "0.13.40528.2")]
    [global::System.Serializable]
    [global::System.Diagnostics.DebuggerDisplay("Stub = StdCalendarItem")]
    [global::System.Diagnostics.DebuggerStepThrough]
    public partial class SStdCalendarItem
      : global::Sem.Sync.SyncBase.StdCalendarItem
      , global::Microsoft.Stubs.Framework.IStub
      , global::Microsoft.Stubs.Framework.IPartialStub
    {
        /// <summary>Initializes a new instance of type SStdCalendarItem</summary>
        [global::System.Diagnostics.DebuggerHidden]
        public SStdCalendarItem()
        {
            this.defaultStub =
              global::Microsoft.Stubs.Framework.StubFallbackBehavior.Current;
        }

        /// <summary>Gets or sets a value that indicates if the base method should be called instead of the fallback behavior</summary>
        public bool CallBase
        {
            [global::System.Diagnostics.DebuggerHidden]
            get
            {
                return this.callBase;
            }
            [global::System.Diagnostics.DebuggerHidden]
            set
            {
                this.callBase = value;
            }
        }

        /// <summary>Stub of method System.Int32 Sem.Sync.SyncBase.StdElement.CompareTo(Sem.Sync.SyncBase.StdElement other)</summary>
        [global::System.Diagnostics.DebuggerHidden]
        public override int CompareTo(global::Sem.Sync.SyncBase.StdElement other)
        {
            global::Microsoft.Stubs.Framework
              .StubDelegates.Func<global::Sem.Sync.SyncBase.StdElement, int> sh
               = this.CompareToStdElement;
            if (sh != (global::Microsoft.Stubs.Framework
              .StubDelegates.Func<global::Sem.Sync.SyncBase.StdElement, int>)null)
              return sh.Invoke(other);
            else 
            {
              if (this.callBase)
                return base.CompareTo(other);
              global::Microsoft.Stubs.Framework.IStubBehavior stub = this.FallbackBehavior;
              return 
                stub.Result<global::Sem.Sync.SyncBase.Stubs.SStdCalendarItem, int>(this);
            }
        }

        /// <summary>Stub of method System.Int32 Sem.Sync.SyncBase.StdElement.CompareTo(Sem.Sync.SyncBase.StdElement other)</summary>
        public global::Microsoft.Stubs.Framework.StubDelegates.Func<global::Sem.Sync.SyncBase.StdElement, int> CompareToStdElement;

        /// <summary>Gets or sets the fallback behavior.</summary>
        public global::Microsoft.Stubs.Framework.IStubBehavior FallbackBehavior
        {
            [global::System.Diagnostics.DebuggerHidden]
            get
            {
                return this.defaultStub;
            }
            [global::System.Diagnostics.DebuggerHidden]
            set
            {
                this.defaultStub = value;
            }
        }

        /// <summary>Stub of method System.Void Sem.Sync.SyncBase.StdElement.NormalizeContent()</summary>
        [global::System.Diagnostics.DebuggerHidden]
        public override void NormalizeContent()
        {
            global::Microsoft.Stubs.Framework.StubDelegates.Action sh
               = this.NormalizeContent01;
            if (sh != (global::Microsoft.Stubs.Framework.StubDelegates.Action)null)
              sh.Invoke();
            else 
            {
              global::Microsoft.Stubs.Framework.IStubBehavior stub = this.FallbackBehavior;
              stub.VoidResult<global::Sem.Sync.SyncBase.Stubs.SStdCalendarItem>(this);
            }
        }

        /// <summary>Stub of method System.Void Sem.Sync.SyncBase.StdElement.NormalizeContent()</summary>
        public global::Microsoft.Stubs.Framework.StubDelegates.Action NormalizeContent01;

        /// <summary>Stub of method System.String Sem.Sync.SyncBase.StdElement.ToSortSimple()</summary>
        [global::System.Diagnostics.DebuggerHidden]
        public override string ToSortSimple()
        {
            global::Microsoft.Stubs.Framework.StubDelegates.Func<string> sh
               = this.ToSortSimple01;
            if (sh != (global::Microsoft.Stubs.Framework.StubDelegates.Func<string>)null)
              return sh.Invoke();
            else 
            {
              if (this.callBase)
                return base.ToSortSimple();
              global::Microsoft.Stubs.Framework.IStubBehavior stub = this.FallbackBehavior;
              return 
                stub.Result<global::Sem.Sync.SyncBase.Stubs.SStdCalendarItem, string>(this);
            }
        }

        /// <summary>Stub of method System.String Sem.Sync.SyncBase.StdElement.ToSortSimple()</summary>
        public global::Microsoft.Stubs.Framework.StubDelegates.Func<string> ToSortSimple01;

        /// <summary>Stub of method System.String Sem.Sync.SyncBase.StdCalendarItem.ToString()</summary>
        [global::System.Diagnostics.DebuggerHidden]
        public override string ToString()
        {
            global::Microsoft.Stubs.Framework.StubDelegates.Func<string> sh
               = this.ToString01;
            if (sh != (global::Microsoft.Stubs.Framework.StubDelegates.Func<string>)null)
              return sh.Invoke();
            else 
            {
              if (this.callBase)
                return base.ToString();
              global::Microsoft.Stubs.Framework.IStubBehavior stub = this.FallbackBehavior;
              return 
                stub.Result<global::Sem.Sync.SyncBase.Stubs.SStdCalendarItem, string>(this);
            }
        }

        /// <summary>Stub of method System.String Sem.Sync.SyncBase.StdCalendarItem.ToString()</summary>
        public global::Microsoft.Stubs.Framework.StubDelegates.Func<string> ToString01;

        /// <summary>Stub of method System.String Sem.Sync.SyncBase.StdElement.ToStringSimple()</summary>
        [global::System.Diagnostics.DebuggerHidden]
        public override string ToStringSimple()
        {
            global::Microsoft.Stubs.Framework.StubDelegates.Func<string> sh
               = this.ToStringSimple01;
            if (sh != (global::Microsoft.Stubs.Framework.StubDelegates.Func<string>)null)
              return sh.Invoke();
            else 
            {
              if (this.callBase)
                return base.ToStringSimple();
              global::Microsoft.Stubs.Framework.IStubBehavior stub = this.FallbackBehavior;
              return 
                stub.Result<global::Sem.Sync.SyncBase.Stubs.SStdCalendarItem, string>(this);
            }
        }

        /// <summary>Stub of method System.String Sem.Sync.SyncBase.StdElement.ToStringSimple()</summary>
        public global::Microsoft.Stubs.Framework.StubDelegates.Func<string> ToStringSimple01;

        private bool callBase;

        private global::Microsoft.Stubs.Framework.IStubBehavior defaultStub;
    }
}
namespace Sem.Sync.SyncBase.Stubs
{
    /// <summary>Stub of StdClient</summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Stubs", "0.13.40528.2")]
    [global::System.Serializable]
    [global::System.Diagnostics.DebuggerDisplay("Stub = StdClient")]
    [global::System.Diagnostics.DebuggerStepThrough]
    public partial class SStdClient
      : global::Sem.Sync.SyncBase.StdClient
      , global::Microsoft.Stubs.Framework.IStub
      , global::Microsoft.Stubs.Framework.IPartialStub
    {
        /// <summary>Initializes a new instance of type SStdClient</summary>
        [global::System.Diagnostics.DebuggerHidden]
        public SStdClient()
        {
            this.defaultStub =
              global::Microsoft.Stubs.Framework.StubFallbackBehavior.Current;
        }

        /// <summary>Stub of method System.Void Sem.Sync.SyncBase.StdClient.AddItem(Sem.Sync.SyncBase.StdElement element, System.String clientFolderName)</summary>
        [global::System.Diagnostics.DebuggerHidden]
        public override void AddItem(global::Sem.Sync.SyncBase.StdElement element, string clientFolderName)
        {
            global::Microsoft.Stubs.Framework
              .StubDelegates.Action<global::Sem.Sync.SyncBase.StdElement, string> sh
               = this.AddItemStdElementString;
            if (sh != (global::Microsoft.Stubs.Framework.StubDelegates
              .Action<global::Sem.Sync.SyncBase.StdElement, string>)null)
              sh.Invoke(element, clientFolderName);
            else 
            {
              if (this.callBase)
                base.AddItem(element, clientFolderName);
              else 
              {
                global::Microsoft.Stubs.Framework.IStubBehavior stub = this.FallbackBehavior
                  ;
                stub.VoidResult<global::Sem.Sync.SyncBase.Stubs.SStdClient>(this);
              }
            }
        }

        /// <summary>Stub of method System.Void Sem.Sync.SyncBase.StdClient.AddItem(Sem.Sync.SyncBase.StdElement element, System.String clientFolderName)</summary>
        public global::Microsoft.Stubs.Framework.StubDelegates.Action<global::Sem.Sync.SyncBase.StdElement, string> AddItemStdElementString;

        /// <summary>Stub of method System.Void Sem.Sync.SyncBase.StdClient.AddRange(System.Collections.Generic.List`1&lt;Sem.Sync.SyncBase.StdElement&gt; elements, System.String clientFolderName)</summary>
        [global::System.Diagnostics.DebuggerHidden]
        public override void AddRange(global::System.Collections.Generic.List<global::Sem.Sync.SyncBase.StdElement> elements, string clientFolderName)
        {
            global::Microsoft.Stubs.Framework
              .StubDelegates.Action<global::System.Collections.Generic
                .List<global::Sem.Sync.SyncBase.StdElement>, string> sh
               = this.AddRangeListStdElementString;
            if (sh != (global::Microsoft.Stubs.Framework
              .StubDelegates.Action<global::System.Collections.Generic
                .List<global::Sem.Sync.SyncBase.StdElement>, string>)null)
              sh.Invoke(elements, clientFolderName);
            else 
            {
              if (this.callBase)
                base.AddRange(elements, clientFolderName);
              else 
              {
                global::Microsoft.Stubs.Framework.IStubBehavior stub = this.FallbackBehavior
                  ;
                stub.VoidResult<global::Sem.Sync.SyncBase.Stubs.SStdClient>(this);
              }
            }
        }

        /// <summary>Stub of method System.Void Sem.Sync.SyncBase.StdClient.AddRange(System.Collections.Generic.List`1&lt;Sem.Sync.SyncBase.StdElement&gt; elements, System.String clientFolderName)</summary>
        public global::Microsoft.Stubs.Framework.StubDelegates.Action<global::System.Collections.Generic.List<global::Sem.Sync.SyncBase.StdElement>, string> AddRangeListStdElementString;

        /// <summary>Stub of method System.Void Sem.Sync.SyncBase.StdClient.BeforeStorageAccess(System.String clientFolderName)</summary>
        [global::System.Diagnostics.DebuggerHidden]
        protected override void BeforeStorageAccess(string clientFolderName)
        {
            global::Microsoft.Stubs.Framework.StubDelegates.Action<string> sh
               = this.BeforeStorageAccessString;
            if (sh != (global::Microsoft.Stubs.Framework.StubDelegates.Action<string>)null)
              sh.Invoke(clientFolderName);
            else 
            {
              if (this.callBase)
                base.BeforeStorageAccess(clientFolderName);
              else 
              {
                global::Microsoft.Stubs.Framework.IStubBehavior stub = this.FallbackBehavior
                  ;
                stub.VoidResult<global::Sem.Sync.SyncBase.Stubs.SStdClient>(this);
              }
            }
        }

        /// <summary>Stub of method System.Void Sem.Sync.SyncBase.StdClient.BeforeStorageAccess(System.String clientFolderName)</summary>
        public global::Microsoft.Stubs.Framework.StubDelegates.Action<string> BeforeStorageAccessString;

        /// <summary>Gets or sets a value that indicates if the base method should be called instead of the fallback behavior</summary>
        public bool CallBase
        {
            [global::System.Diagnostics.DebuggerHidden]
            get
            {
                return this.callBase;
            }
            [global::System.Diagnostics.DebuggerHidden]
            set
            {
                this.callBase = value;
            }
        }

        /// <summary>Gets or sets the fallback behavior.</summary>
        public global::Microsoft.Stubs.Framework.IStubBehavior FallbackBehavior
        {
            [global::System.Diagnostics.DebuggerHidden]
            get
            {
                return this.defaultStub;
            }
            [global::System.Diagnostics.DebuggerHidden]
            set
            {
                this.defaultStub = value;
            }
        }

        /// <summary>Stub of property Sem.Sync.SyncBase.StdClient.FriendlyClientName</summary>
        public override string FriendlyClientName
        {
            [global::System.Diagnostics.DebuggerHidden]
            get
            {
                global::Microsoft.Stubs.Framework.StubDelegates.Func<string> sh
                   = this.FriendlyClientNameGet;
                if (sh != (global::Microsoft.Stubs.Framework.StubDelegates.Func<string>)null)
                  return sh.Invoke();
                else 
                {
                  global::Microsoft.Stubs.Framework.IStubBehavior stub = this.FallbackBehavior;
                  return stub.Result<global::Sem.Sync.SyncBase.Stubs.SStdClient, string>(this);
                }
            }
        }

        /// <summary>Stub of method System.String Sem.Sync.SyncBase.StdClient.get_FriendlyClientName()</summary>
        public global::Microsoft.Stubs.Framework.StubDelegates.Func<string> FriendlyClientNameGet;

        /// <summary>Stub of method System.Collections.Generic.List`1&lt;Sem.Sync.SyncBase.StdElement&gt; Sem.Sync.SyncBase.StdClient.GetAll(System.String clientFolderName)</summary>
        [global::System.Diagnostics.DebuggerHidden]
        public override global::System.Collections.Generic.List<global::Sem.Sync.SyncBase.StdElement> GetAll(string clientFolderName)
        {
            global::Microsoft.Stubs.Framework
              .StubDelegates.Func<string, global::System.Collections.Generic
                .List<global::Sem.Sync.SyncBase.StdElement>> sh = this.GetAllString;
            if (sh != (global::Microsoft.Stubs.Framework
              .StubDelegates.Func<string, global::System.Collections.Generic
                .List<global::Sem.Sync.SyncBase.StdElement>>)null)
              return sh.Invoke(clientFolderName);
            else 
            {
              if (this.callBase)
                return base.GetAll(clientFolderName);
              global::Microsoft.Stubs.Framework.IStubBehavior stub = this.FallbackBehavior;
              return stub.Result<global::Sem.Sync.SyncBase.Stubs.SStdClient, 
              global::System.Collections.Generic
                .List<global::Sem.Sync.SyncBase.StdElement>>(this);
            }
        }

        /// <summary>Stub of method System.Collections.Generic.List`1&lt;Sem.Sync.SyncBase.StdElement&gt; Sem.Sync.SyncBase.StdClient.GetAll(System.String clientFolderName)</summary>
        public global::Microsoft.Stubs.Framework.StubDelegates.Func<string, global::System.Collections.Generic.List<global::Sem.Sync.SyncBase.StdElement>> GetAllString;

        /// <summary>Stub of method System.Void Sem.Sync.SyncBase.StdClient.MergeMissingItem(Sem.Sync.SyncBase.StdElement element, System.String clientFolderName)</summary>
        [global::System.Diagnostics.DebuggerHidden]
        public override void MergeMissingItem(global::Sem.Sync.SyncBase.StdElement element, string clientFolderName)
        {
            global::Microsoft.Stubs.Framework
              .StubDelegates.Action<global::Sem.Sync.SyncBase.StdElement, string> sh
               = this.MergeMissingItemStdElementString;
            if (sh != (global::Microsoft.Stubs.Framework.StubDelegates
              .Action<global::Sem.Sync.SyncBase.StdElement, string>)null)
              sh.Invoke(element, clientFolderName);
            else 
            {
              if (this.callBase)
                base.MergeMissingItem(element, clientFolderName);
              else 
              {
                global::Microsoft.Stubs.Framework.IStubBehavior stub = this.FallbackBehavior
                  ;
                stub.VoidResult<global::Sem.Sync.SyncBase.Stubs.SStdClient>(this);
              }
            }
        }

        /// <summary>Stub of method System.Void Sem.Sync.SyncBase.StdClient.MergeMissingItem(Sem.Sync.SyncBase.StdElement element, System.String clientFolderName)</summary>
        public global::Microsoft.Stubs.Framework.StubDelegates.Action<global::Sem.Sync.SyncBase.StdElement, string> MergeMissingItemStdElementString;

        /// <summary>Stub of method System.Void Sem.Sync.SyncBase.StdClient.MergeMissingRange(System.Collections.Generic.List`1&lt;Sem.Sync.SyncBase.StdElement&gt; elements, System.String clientFolderName)</summary>
        [global::System.Diagnostics.DebuggerHidden]
        public override void MergeMissingRange(global::System.Collections.Generic.List<global::Sem.Sync.SyncBase.StdElement> elements, string clientFolderName)
        {
            global::Microsoft.Stubs.Framework
              .StubDelegates.Action<global::System.Collections.Generic
                .List<global::Sem.Sync.SyncBase.StdElement>, string> sh
               = this.MergeMissingRangeListStdElementString;
            if (sh != (global::Microsoft.Stubs.Framework
              .StubDelegates.Action<global::System.Collections.Generic
                .List<global::Sem.Sync.SyncBase.StdElement>, string>)null)
              sh.Invoke(elements, clientFolderName);
            else 
            {
              if (this.callBase)
                base.MergeMissingRange(elements, clientFolderName);
              else 
              {
                global::Microsoft.Stubs.Framework.IStubBehavior stub = this.FallbackBehavior
                  ;
                stub.VoidResult<global::Sem.Sync.SyncBase.Stubs.SStdClient>(this);
              }
            }
        }

        /// <summary>Stub of method System.Void Sem.Sync.SyncBase.StdClient.MergeMissingRange(System.Collections.Generic.List`1&lt;Sem.Sync.SyncBase.StdElement&gt; elements, System.String clientFolderName)</summary>
        public global::Microsoft.Stubs.Framework.StubDelegates.Action<global::System.Collections.Generic.List<global::Sem.Sync.SyncBase.StdElement>, string> MergeMissingRangeListStdElementString;

        /// <summary>Stub of method System.Collections.Generic.List`1&lt;Sem.Sync.SyncBase.StdElement&gt; Sem.Sync.SyncBase.StdClient.Normalize(System.Collections.Generic.List`1&lt;Sem.Sync.SyncBase.StdElement&gt; elements)</summary>
        [global::System.Diagnostics.DebuggerHidden]
        public override global::System.Collections.Generic.List<global::Sem.Sync.SyncBase.StdElement> Normalize(global::System.Collections.Generic.List<global::Sem.Sync.SyncBase.StdElement> elements)
        {
            global::Microsoft.Stubs.Framework
              .StubDelegates.Func<global::System.Collections.Generic
                .List<global::Sem.Sync.SyncBase.StdElement>, 
              global::System.Collections.Generic
                .List<global::Sem.Sync.SyncBase.StdElement>> sh = this.NormalizeListStdElement;
            if (sh != (global::Microsoft.Stubs.Framework
              .StubDelegates.Func<global::System.Collections.Generic
                .List<global::Sem.Sync.SyncBase.StdElement>, 
              global::System.Collections.Generic
                .List<global::Sem.Sync.SyncBase.StdElement>>)null)
              return sh.Invoke(elements);
            else 
            {
              if (this.callBase)
                return base.Normalize(elements);
              global::Microsoft.Stubs.Framework.IStubBehavior stub = this.FallbackBehavior;
              return stub.Result<global::Sem.Sync.SyncBase.Stubs.SStdClient, 
              global::System.Collections.Generic
                .List<global::Sem.Sync.SyncBase.StdElement>>(this);
            }
        }

        /// <summary>Stub of method System.Collections.Generic.List`1&lt;Sem.Sync.SyncBase.StdElement&gt; Sem.Sync.SyncBase.StdClient.Normalize(System.Collections.Generic.List`1&lt;Sem.Sync.SyncBase.StdElement&gt; elements)</summary>
        public global::Microsoft.Stubs.Framework.StubDelegates.Func<global::System.Collections.Generic.List<global::Sem.Sync.SyncBase.StdElement>, global::System.Collections.Generic.List<global::Sem.Sync.SyncBase.StdElement>> NormalizeListStdElement;

        /// <summary>Stub of method System.Collections.Generic.List`1&lt;Sem.Sync.SyncBase.StdElement&gt; Sem.Sync.SyncBase.StdClient.ReadFullList(System.String clientFolderName, System.Collections.Generic.List`1&lt;Sem.Sync.SyncBase.StdElement&gt; result)</summary>
        [global::System.Diagnostics.DebuggerHidden]
        protected override global::System.Collections.Generic.List<global::Sem.Sync.SyncBase.StdElement> ReadFullList(string clientFolderName, global::System.Collections.Generic.List<global::Sem.Sync.SyncBase.StdElement> result)
        {
            global::Microsoft.Stubs.Framework
              .StubDelegates.Func<string, global::System.Collections.Generic
                .List<global::Sem.Sync.SyncBase.StdElement>, 
              global::System.Collections.Generic
                .List<global::Sem.Sync.SyncBase.StdElement>> sh
               = this.ReadFullListStringListStdElement;
            if (sh != (global::Microsoft.Stubs.Framework
              .StubDelegates.Func<string, global::System.Collections.Generic
                .List<global::Sem.Sync.SyncBase.StdElement>, 
              global::System.Collections.Generic
                .List<global::Sem.Sync.SyncBase.StdElement>>)null)
              return sh.Invoke(clientFolderName, result);
            else 
            {
              global::Microsoft.Stubs.Framework.IStubBehavior stub = this.FallbackBehavior;
              return stub.Result<global::Sem.Sync.SyncBase.Stubs.SStdClient, 
              global::System.Collections.Generic
                .List<global::Sem.Sync.SyncBase.StdElement>>(this);
            }
        }

        /// <summary>Stub of method System.Collections.Generic.List`1&lt;Sem.Sync.SyncBase.StdElement&gt; Sem.Sync.SyncBase.StdClient.ReadFullList(System.String clientFolderName, System.Collections.Generic.List`1&lt;Sem.Sync.SyncBase.StdElement&gt; result)</summary>
        public global::Microsoft.Stubs.Framework.StubDelegates.Func<string, global::System.Collections.Generic.List<global::Sem.Sync.SyncBase.StdElement>, global::System.Collections.Generic.List<global::Sem.Sync.SyncBase.StdElement>> ReadFullListStringListStdElement;

        /// <summary>Stub of method System.Void Sem.Sync.SyncBase.StdClient.RemoveDuplicates(System.String clientFolderName)</summary>
        [global::System.Diagnostics.DebuggerHidden]
        public override void RemoveDuplicates(string clientFolderName)
        {
            global::Microsoft.Stubs.Framework.StubDelegates.Action<string> sh
               = this.RemoveDuplicatesString;
            if (sh != (global::Microsoft.Stubs.Framework.StubDelegates.Action<string>)null)
              sh.Invoke(clientFolderName);
            else 
            {
              if (this.callBase)
                base.RemoveDuplicates(clientFolderName);
              else 
              {
                global::Microsoft.Stubs.Framework.IStubBehavior stub = this.FallbackBehavior
                  ;
                stub.VoidResult<global::Sem.Sync.SyncBase.Stubs.SStdClient>(this);
              }
            }
        }

        /// <summary>Stub of method System.Void Sem.Sync.SyncBase.StdClient.RemoveDuplicates(System.String clientFolderName)</summary>
        public global::Microsoft.Stubs.Framework.StubDelegates.Action<string> RemoveDuplicatesString;

        /// <summary>Stub of method System.Void Sem.Sync.SyncBase.StdClient.WriteFullList(System.Collections.Generic.List`1&lt;Sem.Sync.SyncBase.StdElement&gt; elements, System.String clientFolderName, System.Boolean skipIfExisting)</summary>
        [global::System.Diagnostics.DebuggerHidden]
        protected override void WriteFullList(
            global::System.Collections.Generic.List<global::Sem.Sync.SyncBase.StdElement> elements,
            string clientFolderName,
            bool skipIfExisting
        )
        {
            global::Microsoft.Stubs.Framework
              .StubDelegates.Action<global::System.Collections.Generic
                .List<global::Sem.Sync.SyncBase.StdElement>, string, bool> sh
               = this.WriteFullListListStdElementStringBoolean;
            if (sh != (global::Microsoft.Stubs.Framework
              .StubDelegates.Action<global::System.Collections.Generic
                .List<global::Sem.Sync.SyncBase.StdElement>, 
              string, bool>)null)
              sh.Invoke(elements, clientFolderName, skipIfExisting);
            else 
            {
              global::Microsoft.Stubs.Framework.IStubBehavior stub = this.FallbackBehavior;
              stub.VoidResult<global::Sem.Sync.SyncBase.Stubs.SStdClient>(this);
            }
        }

        /// <summary>Stub of method System.Void Sem.Sync.SyncBase.StdClient.WriteFullList(System.Collections.Generic.List`1&lt;Sem.Sync.SyncBase.StdElement&gt; elements, System.String clientFolderName, System.Boolean skipIfExisting)</summary>
        public global::Microsoft.Stubs.Framework.StubDelegates.Action<global::System.Collections.Generic.List<global::Sem.Sync.SyncBase.StdElement>, string, bool> WriteFullListListStdElementStringBoolean;

        /// <summary>Stub of method System.Void Sem.Sync.SyncBase.StdClient.WriteRange(System.Collections.Generic.List`1&lt;Sem.Sync.SyncBase.StdElement&gt; elements, System.String clientFolderName)</summary>
        [global::System.Diagnostics.DebuggerHidden]
        public override void WriteRange(global::System.Collections.Generic.List<global::Sem.Sync.SyncBase.StdElement> elements, string clientFolderName)
        {
            global::Microsoft.Stubs.Framework
              .StubDelegates.Action<global::System.Collections.Generic
                .List<global::Sem.Sync.SyncBase.StdElement>, string> sh
               = this.WriteRangeListStdElementString;
            if (sh != (global::Microsoft.Stubs.Framework
              .StubDelegates.Action<global::System.Collections.Generic
                .List<global::Sem.Sync.SyncBase.StdElement>, string>)null)
              sh.Invoke(elements, clientFolderName);
            else 
            {
              if (this.callBase)
                base.WriteRange(elements, clientFolderName);
              else 
              {
                global::Microsoft.Stubs.Framework.IStubBehavior stub = this.FallbackBehavior
                  ;
                stub.VoidResult<global::Sem.Sync.SyncBase.Stubs.SStdClient>(this);
              }
            }
        }

        /// <summary>Stub of method System.Void Sem.Sync.SyncBase.StdClient.WriteRange(System.Collections.Generic.List`1&lt;Sem.Sync.SyncBase.StdElement&gt; elements, System.String clientFolderName)</summary>
        public global::Microsoft.Stubs.Framework.StubDelegates.Action<global::System.Collections.Generic.List<global::Sem.Sync.SyncBase.StdElement>, string> WriteRangeListStdElementString;

        private bool callBase;

        private global::Microsoft.Stubs.Framework.IStubBehavior defaultStub;
    }
}
namespace Sem.Sync.SyncBase.Stubs
{
    /// <summary>Stub of StdContact</summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Stubs", "0.13.40528.2")]
    [global::System.Serializable]
    [global::System.Diagnostics.DebuggerDisplay("Stub = StdContact")]
    [global::System.Diagnostics.DebuggerStepThrough]
    public partial class SStdContact
      : global::Sem.Sync.SyncBase.StdContact
      , global::Microsoft.Stubs.Framework.IStub
      , global::Microsoft.Stubs.Framework.IPartialStub
    {
        /// <summary>Initializes a new instance of type SStdContact</summary>
        [global::System.Diagnostics.DebuggerHidden]
        public SStdContact()
        {
            this.defaultStub =
              global::Microsoft.Stubs.Framework.StubFallbackBehavior.Current;
        }

        /// <summary>Gets or sets a value that indicates if the base method should be called instead of the fallback behavior</summary>
        public bool CallBase
        {
            [global::System.Diagnostics.DebuggerHidden]
            get
            {
                return this.callBase;
            }
            [global::System.Diagnostics.DebuggerHidden]
            set
            {
                this.callBase = value;
            }
        }

        /// <summary>Stub of method System.Int32 Sem.Sync.SyncBase.StdElement.CompareTo(Sem.Sync.SyncBase.StdElement other)</summary>
        [global::System.Diagnostics.DebuggerHidden]
        public override int CompareTo(global::Sem.Sync.SyncBase.StdElement other)
        {
            global::Microsoft.Stubs.Framework
              .StubDelegates.Func<global::Sem.Sync.SyncBase.StdElement, int> sh
               = this.CompareToStdElement;
            if (sh != (global::Microsoft.Stubs.Framework
              .StubDelegates.Func<global::Sem.Sync.SyncBase.StdElement, int>)null)
              return sh.Invoke(other);
            else 
            {
              if (this.callBase)
                return base.CompareTo(other);
              global::Microsoft.Stubs.Framework.IStubBehavior stub = this.FallbackBehavior;
              return stub.Result<global::Sem.Sync.SyncBase.Stubs.SStdContact, int>(this);
            }
        }

        /// <summary>Stub of method System.Int32 Sem.Sync.SyncBase.StdElement.CompareTo(Sem.Sync.SyncBase.StdElement other)</summary>
        public global::Microsoft.Stubs.Framework.StubDelegates.Func<global::Sem.Sync.SyncBase.StdElement, int> CompareToStdElement;

        /// <summary>Gets or sets the fallback behavior.</summary>
        public global::Microsoft.Stubs.Framework.IStubBehavior FallbackBehavior
        {
            [global::System.Diagnostics.DebuggerHidden]
            get
            {
                return this.defaultStub;
            }
            [global::System.Diagnostics.DebuggerHidden]
            set
            {
                this.defaultStub = value;
            }
        }

        /// <summary>Stub of method System.Void Sem.Sync.SyncBase.StdElement.NormalizeContent()</summary>
        [global::System.Diagnostics.DebuggerHidden]
        public override void NormalizeContent()
        {
            global::Microsoft.Stubs.Framework.StubDelegates.Action sh
               = this.NormalizeContent01;
            if (sh != (global::Microsoft.Stubs.Framework.StubDelegates.Action)null)
              sh.Invoke();
            else 
            {
              global::Microsoft.Stubs.Framework.IStubBehavior stub = this.FallbackBehavior;
              stub.VoidResult<global::Sem.Sync.SyncBase.Stubs.SStdContact>(this);
            }
        }

        /// <summary>Stub of method System.Void Sem.Sync.SyncBase.StdElement.NormalizeContent()</summary>
        public global::Microsoft.Stubs.Framework.StubDelegates.Action NormalizeContent01;

        /// <summary>Stub of method System.String Sem.Sync.SyncBase.StdElement.ToSortSimple()</summary>
        [global::System.Diagnostics.DebuggerHidden]
        public override string ToSortSimple()
        {
            global::Microsoft.Stubs.Framework.StubDelegates.Func<string> sh
               = this.ToSortSimple01;
            if (sh != (global::Microsoft.Stubs.Framework.StubDelegates.Func<string>)null)
              return sh.Invoke();
            else 
            {
              if (this.callBase)
                return base.ToSortSimple();
              global::Microsoft.Stubs.Framework.IStubBehavior stub = this.FallbackBehavior;
              return stub.Result<global::Sem.Sync.SyncBase.Stubs.SStdContact, string>(this);
            }
        }

        /// <summary>Stub of method System.String Sem.Sync.SyncBase.StdElement.ToSortSimple()</summary>
        public global::Microsoft.Stubs.Framework.StubDelegates.Func<string> ToSortSimple01;

        /// <summary>Stub of method System.String Sem.Sync.SyncBase.StdContact.ToString()</summary>
        [global::System.Diagnostics.DebuggerHidden]
        public override string ToString()
        {
            global::Microsoft.Stubs.Framework.StubDelegates.Func<string> sh
               = this.ToString01;
            if (sh != (global::Microsoft.Stubs.Framework.StubDelegates.Func<string>)null)
              return sh.Invoke();
            else 
            {
              if (this.callBase)
                return base.ToString();
              global::Microsoft.Stubs.Framework.IStubBehavior stub = this.FallbackBehavior;
              return stub.Result<global::Sem.Sync.SyncBase.Stubs.SStdContact, string>(this);
            }
        }

        /// <summary>Stub of method System.String Sem.Sync.SyncBase.StdContact.ToString()</summary>
        public global::Microsoft.Stubs.Framework.StubDelegates.Func<string> ToString01;

        /// <summary>Stub of method System.String Sem.Sync.SyncBase.StdElement.ToStringSimple()</summary>
        [global::System.Diagnostics.DebuggerHidden]
        public override string ToStringSimple()
        {
            global::Microsoft.Stubs.Framework.StubDelegates.Func<string> sh
               = this.ToStringSimple01;
            if (sh != (global::Microsoft.Stubs.Framework.StubDelegates.Func<string>)null)
              return sh.Invoke();
            else 
            {
              if (this.callBase)
                return base.ToStringSimple();
              global::Microsoft.Stubs.Framework.IStubBehavior stub = this.FallbackBehavior;
              return stub.Result<global::Sem.Sync.SyncBase.Stubs.SStdContact, string>(this);
            }
        }

        /// <summary>Stub of method System.String Sem.Sync.SyncBase.StdElement.ToStringSimple()</summary>
        public global::Microsoft.Stubs.Framework.StubDelegates.Func<string> ToStringSimple01;

        private bool callBase;

        private global::Microsoft.Stubs.Framework.IStubBehavior defaultStub;
    }
}
namespace Sem.Sync.SyncBase.Stubs
{
    /// <summary>Stub of StdElement</summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Stubs", "0.13.40528.2")]
    [global::System.Serializable]
    [global::System.Diagnostics.DebuggerDisplay("Stub = StdElement")]
    [global::System.Diagnostics.DebuggerStepThrough]
    public partial class SStdElement
      : global::Sem.Sync.SyncBase.StdElement
      , global::Microsoft.Stubs.Framework.IStub
      , global::Microsoft.Stubs.Framework.IPartialStub
    {
        /// <summary>Initializes a new instance of type SStdElement</summary>
        [global::System.Diagnostics.DebuggerHidden]
        public SStdElement()
        {
            this.defaultStub =
              global::Microsoft.Stubs.Framework.StubFallbackBehavior.Current;
        }

        /// <summary>Gets or sets a value that indicates if the base method should be called instead of the fallback behavior</summary>
        public bool CallBase
        {
            [global::System.Diagnostics.DebuggerHidden]
            get
            {
                return this.callBase;
            }
            [global::System.Diagnostics.DebuggerHidden]
            set
            {
                this.callBase = value;
            }
        }

        /// <summary>Stub of method System.Int32 Sem.Sync.SyncBase.StdElement.CompareTo(Sem.Sync.SyncBase.StdElement other)</summary>
        [global::System.Diagnostics.DebuggerHidden]
        public override int CompareTo(global::Sem.Sync.SyncBase.StdElement other)
        {
            global::Microsoft.Stubs.Framework
              .StubDelegates.Func<global::Sem.Sync.SyncBase.StdElement, int> sh
               = this.CompareToStdElement;
            if (sh != (global::Microsoft.Stubs.Framework
              .StubDelegates.Func<global::Sem.Sync.SyncBase.StdElement, int>)null)
              return sh.Invoke(other);
            else 
            {
              if (this.callBase)
                return base.CompareTo(other);
              global::Microsoft.Stubs.Framework.IStubBehavior stub = this.FallbackBehavior;
              return stub.Result<global::Sem.Sync.SyncBase.Stubs.SStdElement, int>(this);
            }
        }

        /// <summary>Stub of method System.Int32 Sem.Sync.SyncBase.StdElement.CompareTo(Sem.Sync.SyncBase.StdElement other)</summary>
        public global::Microsoft.Stubs.Framework.StubDelegates.Func<global::Sem.Sync.SyncBase.StdElement, int> CompareToStdElement;

        /// <summary>Gets or sets the fallback behavior.</summary>
        public global::Microsoft.Stubs.Framework.IStubBehavior FallbackBehavior
        {
            [global::System.Diagnostics.DebuggerHidden]
            get
            {
                return this.defaultStub;
            }
            [global::System.Diagnostics.DebuggerHidden]
            set
            {
                this.defaultStub = value;
            }
        }

        /// <summary>Stub of method System.Void Sem.Sync.SyncBase.StdElement.NormalizeContent()</summary>
        [global::System.Diagnostics.DebuggerHidden]
        public override void NormalizeContent()
        {
            global::Microsoft.Stubs.Framework.StubDelegates.Action sh
               = this.NormalizeContent01;
            if (sh != (global::Microsoft.Stubs.Framework.StubDelegates.Action)null)
              sh.Invoke();
            else 
            {
              global::Microsoft.Stubs.Framework.IStubBehavior stub = this.FallbackBehavior;
              stub.VoidResult<global::Sem.Sync.SyncBase.Stubs.SStdElement>(this);
            }
        }

        /// <summary>Stub of method System.Void Sem.Sync.SyncBase.StdElement.NormalizeContent()</summary>
        public global::Microsoft.Stubs.Framework.StubDelegates.Action NormalizeContent01;

        /// <summary>Stub of method System.String Sem.Sync.SyncBase.StdElement.ToSortSimple()</summary>
        [global::System.Diagnostics.DebuggerHidden]
        public override string ToSortSimple()
        {
            global::Microsoft.Stubs.Framework.StubDelegates.Func<string> sh
               = this.ToSortSimple01;
            if (sh != (global::Microsoft.Stubs.Framework.StubDelegates.Func<string>)null)
              return sh.Invoke();
            else 
            {
              if (this.callBase)
                return base.ToSortSimple();
              global::Microsoft.Stubs.Framework.IStubBehavior stub = this.FallbackBehavior;
              return stub.Result<global::Sem.Sync.SyncBase.Stubs.SStdElement, string>(this);
            }
        }

        /// <summary>Stub of method System.String Sem.Sync.SyncBase.StdElement.ToSortSimple()</summary>
        public global::Microsoft.Stubs.Framework.StubDelegates.Func<string> ToSortSimple01;

        /// <summary>Stub of method System.String Sem.Sync.SyncBase.StdElement.ToStringSimple()</summary>
        [global::System.Diagnostics.DebuggerHidden]
        public override string ToStringSimple()
        {
            global::Microsoft.Stubs.Framework.StubDelegates.Func<string> sh
               = this.ToStringSimple01;
            if (sh != (global::Microsoft.Stubs.Framework.StubDelegates.Func<string>)null)
              return sh.Invoke();
            else 
            {
              if (this.callBase)
                return base.ToStringSimple();
              global::Microsoft.Stubs.Framework.IStubBehavior stub = this.FallbackBehavior;
              return stub.Result<global::Sem.Sync.SyncBase.Stubs.SStdElement, string>(this);
            }
        }

        /// <summary>Stub of method System.String Sem.Sync.SyncBase.StdElement.ToStringSimple()</summary>
        public global::Microsoft.Stubs.Framework.StubDelegates.Func<string> ToStringSimple01;

        private bool callBase;

        private global::Microsoft.Stubs.Framework.IStubBehavior defaultStub;
    }
}
namespace Sem.Sync.SyncBase.Binding.Stubs
{
    /// <summary>Stub of SyncCollection</summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Stubs", "0.13.40528.2")]
    [global::System.Serializable]
    [global::System.Diagnostics.DebuggerDisplay("Stub = SyncCollection")]
    [global::System.Diagnostics.DebuggerStepThrough]
    public partial class SSyncCollection
      : global::Sem.Sync.SyncBase.Binding.SyncCollection
      , global::Microsoft.Stubs.Framework.IStub
      , global::Microsoft.Stubs.Framework.IPartialStub
    {
        /// <summary>Initializes a new instance of type SSyncCollection</summary>
        [global::System.Diagnostics.DebuggerHidden]
        public SSyncCollection()
        {
            this.defaultStub =
              global::Microsoft.Stubs.Framework.StubFallbackBehavior.Current;
        }

        /// <summary>Stub of method System.Object Sem.Sync.SyncBase.Binding.SyncCollection.AddNewCore()</summary>
        [global::System.Diagnostics.DebuggerHidden]
        protected override object AddNewCore()
        {
            global::Microsoft.Stubs.Framework.StubDelegates.Func<object> sh
               = this.AddNewCore01;
            if (sh != (global::Microsoft.Stubs.Framework.StubDelegates.Func<object>)null)
              return sh.Invoke();
            else 
            {
              if (this.callBase)
                return base.AddNewCore();
              global::Microsoft.Stubs.Framework.IStubBehavior stub = this.FallbackBehavior;
              return 
                stub.Result<global::Sem.Sync.SyncBase.Binding.Stubs.SSyncCollection, object>
                    (this);
            }
        }

        /// <summary>Stub of method System.Object Sem.Sync.SyncBase.Binding.SyncCollection.AddNewCore()</summary>
        public global::Microsoft.Stubs.Framework.StubDelegates.Func<object> AddNewCore01;

        /// <summary>Gets or sets a value that indicates if the base method should be called instead of the fallback behavior</summary>
        public bool CallBase
        {
            [global::System.Diagnostics.DebuggerHidden]
            get
            {
                return this.callBase;
            }
            [global::System.Diagnostics.DebuggerHidden]
            set
            {
                this.callBase = value;
            }
        }

        /// <summary>Gets or sets the fallback behavior.</summary>
        public global::Microsoft.Stubs.Framework.IStubBehavior FallbackBehavior
        {
            [global::System.Diagnostics.DebuggerHidden]
            get
            {
                return this.defaultStub;
            }
            [global::System.Diagnostics.DebuggerHidden]
            set
            {
                this.defaultStub = value;
            }
        }

        private bool callBase;

        private global::Microsoft.Stubs.Framework.IStubBehavior defaultStub;
    }
}
namespace Sem.Sync.SyncBase.Stubs
{
    /// <summary>Stub of SyncComponent</summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Stubs", "0.13.40528.2")]
    [global::System.Serializable]
    [global::System.Diagnostics.DebuggerDisplay("Stub = SyncComponent")]
    [global::System.Diagnostics.DebuggerStepThrough]
    public partial class SSyncComponent
      : global::Sem.Sync.SyncBase.SyncComponent
      , global::Microsoft.Stubs.Framework.IStub
      , global::Microsoft.Stubs.Framework.IPartialStub
    {
        /// <summary>Initializes a new instance of type SSyncComponent</summary>
        [global::System.Diagnostics.DebuggerHidden]
        public SSyncComponent()
        {
            this.defaultStub =
              global::Microsoft.Stubs.Framework.StubFallbackBehavior.Current;
        }

        /// <summary>Gets or sets a value that indicates if the base method should be called instead of the fallback behavior</summary>
        public bool CallBase
        {
            [global::System.Diagnostics.DebuggerHidden]
            get
            {
                return this.callBase;
            }
            [global::System.Diagnostics.DebuggerHidden]
            set
            {
                this.callBase = value;
            }
        }

        /// <summary>Gets or sets the fallback behavior.</summary>
        public global::Microsoft.Stubs.Framework.IStubBehavior FallbackBehavior
        {
            [global::System.Diagnostics.DebuggerHidden]
            get
            {
                return this.defaultStub;
            }
            [global::System.Diagnostics.DebuggerHidden]
            set
            {
                this.defaultStub = value;
            }
        }

        private bool callBase;

        private global::Microsoft.Stubs.Framework.IStubBehavior defaultStub;
    }
}
namespace Sem.Sync.SyncBase.DetailData.Stubs
{
    /// <summary>Stub of SyncData</summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Stubs", "0.13.40528.2")]
    [global::System.Serializable]
    [global::System.Diagnostics.DebuggerDisplay("Stub = SyncData")]
    [global::System.Diagnostics.DebuggerStepThrough]
    public partial class SSyncData
      : global::Sem.Sync.SyncBase.DetailData.SyncData
      , global::Microsoft.Stubs.Framework.IStub
      , global::Microsoft.Stubs.Framework.IPartialStub
    {
        /// <summary>Initializes a new instance of type SSyncData</summary>
        [global::System.Diagnostics.DebuggerHidden]
        public SSyncData()
        {
            this.defaultStub =
              global::Microsoft.Stubs.Framework.StubFallbackBehavior.Current;
        }

        /// <summary>Gets or sets a value that indicates if the base method should be called instead of the fallback behavior</summary>
        public bool CallBase
        {
            [global::System.Diagnostics.DebuggerHidden]
            get
            {
                return this.callBase;
            }
            [global::System.Diagnostics.DebuggerHidden]
            set
            {
                this.callBase = value;
            }
        }

        /// <summary>Gets or sets the fallback behavior.</summary>
        public global::Microsoft.Stubs.Framework.IStubBehavior FallbackBehavior
        {
            [global::System.Diagnostics.DebuggerHidden]
            get
            {
                return this.defaultStub;
            }
            [global::System.Diagnostics.DebuggerHidden]
            set
            {
                this.defaultStub = value;
            }
        }

        private bool callBase;

        private global::Microsoft.Stubs.Framework.IStubBehavior defaultStub;
    }
}
namespace Sem.Sync.SyncBase.Stubs
{
    /// <summary>Stub of SyncDescription</summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Stubs", "0.13.40528.2")]
    [global::System.Serializable]
    [global::System.Diagnostics.DebuggerDisplay("Stub = SyncDescription")]
    [global::System.Diagnostics.DebuggerStepThrough]
    public partial class SSyncDescription
      : global::Sem.Sync.SyncBase.SyncDescription
      , global::Microsoft.Stubs.Framework.IStub
      , global::Microsoft.Stubs.Framework.IPartialStub
    {
        /// <summary>Initializes a new instance of type SSyncDescription</summary>
        [global::System.Diagnostics.DebuggerHidden]
        public SSyncDescription()
        {
            this.defaultStub =
              global::Microsoft.Stubs.Framework.StubFallbackBehavior.Current;
        }

        /// <summary>Gets or sets a value that indicates if the base method should be called instead of the fallback behavior</summary>
        public bool CallBase
        {
            [global::System.Diagnostics.DebuggerHidden]
            get
            {
                return this.callBase;
            }
            [global::System.Diagnostics.DebuggerHidden]
            set
            {
                this.callBase = value;
            }
        }

        /// <summary>Gets or sets the fallback behavior.</summary>
        public global::Microsoft.Stubs.Framework.IStubBehavior FallbackBehavior
        {
            [global::System.Diagnostics.DebuggerHidden]
            get
            {
                return this.defaultStub;
            }
            [global::System.Diagnostics.DebuggerHidden]
            set
            {
                this.defaultStub = value;
            }
        }

        private bool callBase;

        private global::Microsoft.Stubs.Framework.IStubBehavior defaultStub;
    }
}
namespace Sem.Sync.SyncBase.Stubs
{
    /// <summary>Stub of SyncEngine</summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Stubs", "0.13.40528.2")]
    [global::System.Serializable]
    [global::System.Diagnostics.DebuggerDisplay("Stub = SyncEngine")]
    [global::System.Diagnostics.DebuggerStepThrough]
    public partial class SSyncEngine
      : global::Sem.Sync.SyncBase.SyncEngine
      , global::Microsoft.Stubs.Framework.IStub
      , global::Microsoft.Stubs.Framework.IPartialStub
    {
        /// <summary>Initializes a new instance of type SSyncEngine</summary>
        [global::System.Diagnostics.DebuggerHidden]
        public SSyncEngine()
        {
            this.defaultStub =
              global::Microsoft.Stubs.Framework.StubFallbackBehavior.Current;
        }

        /// <summary>Gets or sets a value that indicates if the base method should be called instead of the fallback behavior</summary>
        public bool CallBase
        {
            [global::System.Diagnostics.DebuggerHidden]
            get
            {
                return this.callBase;
            }
            [global::System.Diagnostics.DebuggerHidden]
            set
            {
                this.callBase = value;
            }
        }

        /// <summary>Gets or sets the fallback behavior.</summary>
        public global::Microsoft.Stubs.Framework.IStubBehavior FallbackBehavior
        {
            [global::System.Diagnostics.DebuggerHidden]
            get
            {
                return this.defaultStub;
            }
            [global::System.Diagnostics.DebuggerHidden]
            set
            {
                this.defaultStub = value;
            }
        }

        private bool callBase;

        private global::Microsoft.Stubs.Framework.IStubBehavior defaultStub;
    }
}
namespace Sem.Sync.SyncBase.Helpers.Stubs
{
    /// <summary>Stub of VCardConverter</summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Stubs", "0.13.40528.2")]
    [global::System.Serializable]
    [global::System.Diagnostics.DebuggerDisplay("Stub = VCardConverter")]
    [global::System.Diagnostics.DebuggerStepThrough]
    public partial class SVCardConverter
      : global::Sem.Sync.SyncBase.Helpers.VCardConverter
      , global::Microsoft.Stubs.Framework.IStub
      , global::Microsoft.Stubs.Framework.IPartialStub
    {
        /// <summary>Initializes a new instance of type SVCardConverter</summary>
        [global::System.Diagnostics.DebuggerHidden]
        public SVCardConverter()
        {
            this.defaultStub =
              global::Microsoft.Stubs.Framework.StubFallbackBehavior.Current;
        }

        /// <summary>Gets or sets a value that indicates if the base method should be called instead of the fallback behavior</summary>
        public bool CallBase
        {
            [global::System.Diagnostics.DebuggerHidden]
            get
            {
                return this.callBase;
            }
            [global::System.Diagnostics.DebuggerHidden]
            set
            {
                this.callBase = value;
            }
        }

        /// <summary>Gets or sets the fallback behavior.</summary>
        public global::Microsoft.Stubs.Framework.IStubBehavior FallbackBehavior
        {
            [global::System.Diagnostics.DebuggerHidden]
            get
            {
                return this.defaultStub;
            }
            [global::System.Diagnostics.DebuggerHidden]
            set
            {
                this.defaultStub = value;
            }
        }

        private bool callBase;

        private global::Microsoft.Stubs.Framework.IStubBehavior defaultStub;
    }
}
namespace Sem.Sync.SyncBase.Helpers.Stubs
{
    /// <summary>Stub of VersionCheck</summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Stubs", "0.13.40528.2")]
    [global::System.Serializable]
    [global::System.Diagnostics.DebuggerDisplay("Stub = VersionCheck")]
    [global::System.Diagnostics.DebuggerStepThrough]
    public partial class SVersionCheck
      : global::Sem.Sync.SyncBase.Helpers.VersionCheck
      , global::Microsoft.Stubs.Framework.IStub
      , global::Microsoft.Stubs.Framework.IPartialStub
    {
        /// <summary>Initializes a new instance of type SVersionCheck</summary>
        [global::System.Diagnostics.DebuggerHidden]
        public SVersionCheck()
        {
            this.defaultStub =
              global::Microsoft.Stubs.Framework.StubFallbackBehavior.Current;
        }

        /// <summary>Gets or sets a value that indicates if the base method should be called instead of the fallback behavior</summary>
        public bool CallBase
        {
            [global::System.Diagnostics.DebuggerHidden]
            get
            {
                return this.callBase;
            }
            [global::System.Diagnostics.DebuggerHidden]
            set
            {
                this.callBase = value;
            }
        }

        /// <summary>Gets or sets the fallback behavior.</summary>
        public global::Microsoft.Stubs.Framework.IStubBehavior FallbackBehavior
        {
            [global::System.Diagnostics.DebuggerHidden]
            get
            {
                return this.defaultStub;
            }
            [global::System.Diagnostics.DebuggerHidden]
            set
            {
                this.defaultStub = value;
            }
        }

        private bool callBase;

        private global::Microsoft.Stubs.Framework.IStubBehavior defaultStub;
    }
}

