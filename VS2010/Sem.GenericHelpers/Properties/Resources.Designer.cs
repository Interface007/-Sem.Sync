﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.21006.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Sem.GenericHelpers.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Sem.GenericHelpers.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There are some information files about problems while working with programs from Sven Erik Matzen.
        ///Do you want to send these files now to the developer?
        ///If you select &gt;OK&lt; you can review each information file and decide whether to send it or not..
        /// </summary>
        internal static string ThereAreSomeInformationFiles {
            get {
                return ResourceManager.GetString("ThereAreSomeInformationFiles", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Exceptions found.
        /// </summary>
        internal static string ThereAreSomeInformationFilesTitle {
            get {
                return ResourceManager.GetString("ThereAreSomeInformationFilesTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The server has rejected the information file - this is a normal behaviour, because not all exception information can be accepted by the server..
        /// </summary>
        internal static string TheServerHasRejected {
            get {
                return ResourceManager.GetString("TheServerHasRejected", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Information rejected..
        /// </summary>
        internal static string TheServerHasRejectedTitle {
            get {
                return ResourceManager.GetString("TheServerHasRejectedTitle", resourceCulture);
            }
        }
    }
}