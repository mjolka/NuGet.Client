// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Design", "CA1056:Uri properties should not be strings", Justification = "<Pending>", Scope = "member", Target = "~P:NuGet.Packaging.Core.RepositoryMetadata.Url")]
[assembly: SuppressMessage("Design", "CA1012:Abstract types should not have constructors", Justification = "<Pending>", Scope = "type", Target = "~T:NuGet.Packaging.PackageReaderBase")]
[assembly: SuppressMessage("Build", "CA1308:In method 'GetHashCode', replace the call to 'ToLowerInvariant' with 'ToUpperInvariant'.", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.Packaging.Core.PackageDependencyComparer.GetHashCode(NuGet.Packaging.Core.PackageDependency)~System.Int32")]
[assembly: SuppressMessage("Build", "CA1054:Change the type of parameter url of method RepositoryMetadata.RepositoryMetadata(string, string, string, string) from string to System.Uri, or provide an overload to RepositoryMetadata.RepositoryMetadata(string, string, string, string) that allows url to be passed as a System.Uri object.", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.Packaging.Core.RepositoryMetadata.#ctor(System.String,System.String,System.String,System.String)")]
[assembly: SuppressMessage("Build", "CA1307:The behavior of 'string.Equals(string)' could vary based on the current user's locale settings. Replace this call in 'NuGet.Packaging.LicenseMetadata.Equals(NuGet.Packaging.LicenseMetadata)' with a call to 'string.Equals(string, System.StringComparison)'.", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.Packaging.LicenseMetadata.Equals(NuGet.Packaging.LicenseMetadata)~System.Boolean")]
[assembly: SuppressMessage("Build", "CA1304:The behavior of 'string.ToUpper()' could vary based on the current user's locale settings. Replace this call in 'LogicalOperator.ToString()' with a call to 'string.ToUpper(CultureInfo)'.", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.Packaging.Licenses.LogicalOperator.ToString~System.String")]
[assembly: SuppressMessage("Build", "CA1307:The behavior of 'string.Equals(string)' could vary based on the current user's locale settings. Replace this call in 'NuGet.Packaging.Licenses.NuGetLicense.ProcessLicenseNotInStandardData(string, bool, bool)' with a call to 'string.Equals(string, System.StringComparison)'.", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.Packaging.Licenses.NuGetLicense.ProcessLicenseNotInStandardData(System.String,System.Boolean,System.Boolean)~NuGet.Packaging.Licenses.NuGetLicense")]
[assembly: SuppressMessage("Build", "CA1307:The behavior of 'string.Equals(string)' could vary based on the current user's locale settings. Replace this call in 'NuGet.Packaging.Licenses.NuGetLicenseExpressionExtensions.IsUnlicensed(NuGet.Packaging.Licenses.NuGetLicense)' with a call to 'string.Equals(string, System.StringComparison)'.", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.Packaging.Licenses.NuGetLicenseExpressionExtensions.IsUnlicensed(NuGet.Packaging.Licenses.NuGetLicense)~System.Boolean")]
[assembly: SuppressMessage("Build", "CA1307:The behavior of 'string.Equals(string)' could vary based on the current user's locale settings. Replace this call in 'NuGet.Packaging.NupkgMetadataFile.Equals(NuGet.Packaging.NupkgMetadataFile)' with a call to 'string.Equals(string, System.StringComparison)'.", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.Packaging.NupkgMetadataFile.Equals(NuGet.Packaging.NupkgMetadataFile)~System.Boolean")]
[assembly: SuppressMessage("Build", "CA1307:The behavior of 'string.EndsWith(string)' could vary based on the current user's locale settings. Replace this call in 'NuGet.Packaging.PackageFileExtractor.GatherIntellisenseXmlFiles(System.Collections.Generic.IEnumerable<string>)' with a call to 'string.EndsWith(string, System.StringComparison)'.", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.Packaging.PackageFileExtractor.GatherIntellisenseXmlFiles(System.Collections.Generic.IEnumerable{System.String})~System.Collections.Generic.HashSet{System.String}")]
[assembly: SuppressMessage("Build", "CA1308:In method 'GetPackageLookupPaths', replace the call to 'ToLowerInvariant' with 'ToUpperInvariant'.", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.Packaging.PackagePathHelper.GetPackageLookupPaths(NuGet.Packaging.Core.PackageIdentity,NuGet.Packaging.PackagePathResolver)~System.Collections.Generic.IEnumerable{System.String}")]
[assembly: SuppressMessage("Build", "CA1307:The behavior of 'string.EndsWith(string)' could vary based on the current user's locale settings. Replace this call in 'NuGet.Packaging.PackageReaderBase.NormalizeDirectoryPath(string)' with a call to 'string.EndsWith(string, System.StringComparison)'.", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.Packaging.PackageReaderBase.NormalizeDirectoryPath(System.String)~System.String")]
[assembly: SuppressMessage("Build", "CA1307:The behavior of 'string.Equals(string, string)' could vary based on the current user's locale settings. Replace this call in 'NuGet.Packaging.Signing.CertificateUtility.HasExtendedKeyUsage(System.Security.Cryptography.X509Certificates.X509Certificate2, string)' with a call to 'string.Equals(string, string, System.StringComparison)'.", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.Packaging.Signing.CertificateUtility.HasExtendedKeyUsage(System.Security.Cryptography.X509Certificates.X509Certificate2,System.String)~System.Boolean")]
[assembly: SuppressMessage("Build", "CA1307:The behavior of 'string.Equals(string, string)' could vary based on the current user's locale settings. Replace this call in 'NuGet.Packaging.Signing.CertificateUtility.IsValidForPurposeFast(System.Security.Cryptography.X509Certificates.X509Certificate2, string)' with a call to 'string.Equals(string, string, System.StringComparison)'.", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.Packaging.Signing.CertificateUtility.IsValidForPurposeFast(System.Security.Cryptography.X509Certificates.X509Certificate2,System.String)~System.Boolean")]
[assembly: SuppressMessage("Build", "CA1308:In method 'Normalize', replace the call to 'ToLowerInvariant' with 'ToUpperInvariant'.", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.Packaging.VersionFolderPathResolver.Normalize(NuGet.Versioning.NuGetVersion)~System.String")]
[assembly: SuppressMessage("Build", "CA1308:In method 'Normalize', replace the call to 'ToLowerInvariant' with 'ToUpperInvariant'.", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.Packaging.VersionFolderPathResolver.Normalize(System.String)~System.String")]
[assembly: SuppressMessage("Build", "CA1056:Change the type of property IRepositoryCertificateInfo.ContentUrl from string to System.Uri.", Justification = "<Pending>", Scope = "member", Target = "~P:NuGet.Packaging.Core.IRepositoryCertificateInfo.ContentUrl")]
[assembly: SuppressMessage("Build", "CA1819:Properties should not return arrays", Justification = "<Pending>", Scope = "member", Target = "~P:NuGet.Packaging.PackagingConstants.Folders.Known")]
[assembly: SuppressMessage("Build", "CA1819:Properties should not return arrays", Justification = "<Pending>", Scope = "member", Target = "~P:NuGet.Packaging.Signing.CommitmentTypeQualifier.Qualifier")]
[assembly: SuppressMessage("Build", "CA1819:Properties should not return arrays", Justification = "<Pending>", Scope = "member", Target = "~P:NuGet.Packaging.Signing.EssCertId.CertificateHash")]
[assembly: SuppressMessage("Build", "CA1819:Properties should not return arrays", Justification = "<Pending>", Scope = "member", Target = "~P:NuGet.Packaging.Signing.EssCertIdV2.CertificateHash")]
[assembly: SuppressMessage("Build", "CA1819:Properties should not return arrays", Justification = "<Pending>", Scope = "member", Target = "~P:NuGet.Packaging.Signing.Extension.Value")]
[assembly: SuppressMessage("Build", "CA1819:Properties should not return arrays", Justification = "<Pending>", Scope = "member", Target = "~P:NuGet.Packaging.Signing.IssuerSerial.SerialNumber")]
[assembly: SuppressMessage("Build", "CA1819:Properties should not return arrays", Justification = "<Pending>", Scope = "member", Target = "~P:NuGet.Packaging.Signing.MessageImprint.HashedMessage")]
[assembly: SuppressMessage("Build", "CA1819:Properties should not return arrays", Justification = "<Pending>", Scope = "member", Target = "~P:NuGet.Packaging.Signing.PolicyQualifierInfo.Qualifier")]
[assembly: SuppressMessage("Build", "CA2227:Change 'CentralDirectoryHeaders' to be read-only by removing the property setter.", Justification = "<Pending>", Scope = "member", Target = "~P:NuGet.Packaging.Signing.SignedPackageArchiveMetadata.CentralDirectoryHeaders")]
[assembly: SuppressMessage("Build", "CA1819:Properties should not return arrays", Justification = "<Pending>", Scope = "member", Target = "~P:NuGet.Packaging.Signing.SigningSpecifications.AllowedHashAlgorithmOids")]
[assembly: SuppressMessage("Build", "CA1819:Properties should not return arrays", Justification = "<Pending>", Scope = "member", Target = "~P:NuGet.Packaging.Signing.SigningSpecifications.AllowedHashAlgorithms")]
[assembly: SuppressMessage("Build", "CA1819:Properties should not return arrays", Justification = "<Pending>", Scope = "member", Target = "~P:NuGet.Packaging.Signing.SigningSpecifications.AllowedSignatureAlgorithmOids")]
[assembly: SuppressMessage("Build", "CA1819:Properties should not return arrays", Justification = "<Pending>", Scope = "member", Target = "~P:NuGet.Packaging.Signing.SigningSpecifications.AllowedSignatureAlgorithms")]
[assembly: SuppressMessage("Build", "CA1819:Properties should not return arrays", Justification = "<Pending>", Scope = "member", Target = "~P:NuGet.Packaging.Signing.TstInfo.Nonce")]
[assembly: SuppressMessage("Build", "CA1819:Properties should not return arrays", Justification = "<Pending>", Scope = "member", Target = "~P:NuGet.Packaging.Signing.TstInfo.SerialNumber")]
[assembly: SuppressMessage("Build", "CA1819:Properties should not return arrays", Justification = "<Pending>", Scope = "member", Target = "~P:NuGet.Packaging.Signing.TstInfo.Tsa")]
[assembly: SuppressMessage("Build", "CA1012:Abstract type NuspecCoreReaderBase should not have constructors", Justification = "<Pending>", Scope = "type", Target = "~T:NuGet.Packaging.Core.NuspecCoreReaderBase")]
[assembly: SuppressMessage("Build", "CA2237:Add [Serializable] to PackagingException as this type implements ISerializable", Justification = "<Pending>", Scope = "type", Target = "~T:NuGet.Packaging.Core.PackagingException")]
[assembly: SuppressMessage("Build", "CA2237:Add [Serializable] to NuGetLicenseExpressionParsingException as this type implements ISerializable", Justification = "<Pending>", Scope = "type", Target = "~T:NuGet.Packaging.Licenses.NuGetLicenseExpressionParsingException")]
[assembly: SuppressMessage("Build", "CA1012:Abstract type PackageVerificationResult should not have constructors", Justification = "<Pending>", Scope = "type", Target = "~T:NuGet.Packaging.Signing.PackageVerificationResult")]
[assembly: SuppressMessage("Build", "CA1067:Type NuGet.Packaging.Signing.SignatureLog should override Equals because it implements IEquatable<T>", Justification = "<Pending>", Scope = "type", Target = "~T:NuGet.Packaging.Signing.SignatureLog")]
[assembly: SuppressMessage("Build", "CA1012:Abstract type VerificationAllowListEntry should not have constructors", Justification = "<Pending>", Scope = "type", Target = "~T:NuGet.Packaging.Signing.VerificationAllowListEntry")]
[assembly: SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "The method that raised the warning returns an IDisposable object that wraps local disposable object", Scope = "member", Target = "~M:NuGet.Packaging.NuGetExtractionFileIO.DotnetCoreCreateFile(System.String)~System.IO.FileStream")]
