﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NuGet.Common {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Strings {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Strings() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("NuGet.Common.Strings", typeof(Strings).Assembly);
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
        ///   Looks up a localized string similar to An absolute path is required: &apos;{0}&apos;..
        /// </summary>
        internal static string AbsolutePathRequired {
            get {
                return ResourceManager.GetString("AbsolutePathRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Value must be greater than or equal to {0}.
        /// </summary>
        internal static string Argument_Must_Be_GreaterThanOrEqualTo {
            get {
                return ResourceManager.GetString("Argument_Must_Be_GreaterThanOrEqualTo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Argument cannot be null or empty.
        /// </summary>
        internal static string ArgumentNullOrEmpty {
            get {
                return ResourceManager.GetString("ArgumentNullOrEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to create random file for dotnet add pkg command..
        /// </summary>
        internal static string Error_FailedToCreateRandomFile {
            get {
                return ResourceManager.GetString("Error_FailedToCreateRandomFile", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Required environment variable &apos;{0}&apos; is not set. Try setting &apos;{0}&apos; and running the operation again..
        /// </summary>
        internal static string MissingRequiredEnvVar {
            get {
                return ResourceManager.GetString("MissingRequiredEnvVar", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Required environment variable &apos;{0}&apos; is not set. Try setting &apos;{1}&apos; or &apos;{0}&apos; and running the operation.
        /// </summary>
        internal static string MissingRequiredEnvVarsDotnet {
            get {
                return ResourceManager.GetString("MissingRequiredEnvVarsDotnet", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to At least one package folder path must be provided..
        /// </summary>
        internal static string NoPackageFoldersFound {
            get {
                return ResourceManager.GetString("NoPackageFoldersFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Package folder not found: &apos;{0}&apos;..
        /// </summary>
        internal static string PackageFolderNotFound {
            get {
                return ResourceManager.GetString("PackageFolderNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0:0.##} hr.
        /// </summary>
        internal static string TimeUnits_Hour {
            get {
                return ResourceManager.GetString("TimeUnits_Hour", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0:0.##} ms.
        /// </summary>
        internal static string TimeUnits_Millisecond {
            get {
                return ResourceManager.GetString("TimeUnits_Millisecond", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0:0.##} min.
        /// </summary>
        internal static string TimeUnits_Minute {
            get {
                return ResourceManager.GetString("TimeUnits_Minute", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0:0.##} sec.
        /// </summary>
        internal static string TimeUnits_Second {
            get {
                return ResourceManager.GetString("TimeUnits_Second", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to determine the current NuGet client version..
        /// </summary>
        internal static string UnableToDetemineClientVersion {
            get {
                return ResourceManager.GetString("UnableToDetemineClientVersion", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to obtain lock file access on &apos;{0}&apos; for operations on &apos;{1}&apos;. This may mean that a different user or administrator is holding this lock and that this process does not have permission to access it. If no other process is currently performing an operation on this file it may mean that an earlier NuGet process crashed and left an inaccessible lock file, in this case removing the file &apos;{0}&apos; will allow NuGet to continue..
        /// </summary>
        internal static string UnauthorizedLockFail {
            get {
                return ResourceManager.GetString("UnauthorizedLockFail", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Hash algorithm &apos;{0}&apos; is unsupported. Supported algorithms include: SHA512 and SHA256..
        /// </summary>
        internal static string UnsupportedHashAlgorithm {
            get {
                return ResourceManager.GetString("UnsupportedHashAlgorithm", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Hash algorithm &apos;{0}&apos; is unsupported..
        /// </summary>
        internal static string UnsupportedHashAlgorithmName {
            get {
                return ResourceManager.GetString("UnsupportedHashAlgorithmName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Signature algorithm &apos;{0}&apos; is unsupported..
        /// </summary>
        internal static string UnsupportedSignatureAlgorithmName {
            get {
                return ResourceManager.GetString("UnsupportedSignatureAlgorithmName", resourceCulture);
            }
        }
    }
}
