@{
  RootModule = 'lib/net9.0/PSnaps.dll'
  ModuleVersion = '0.14.0'
  CompatiblePSEditions = @('Core')
  GUID = '7a389ff6-dd4f-4486-b430-802de6fcc65c'
  Author = 'dodexahedron'
  CompanyName = ''
  Copyright = '2025 Brandon Thetford. All rights reserved.'
  Description = 'A PowerShell module and .net library for management of Snap packages on Linux. Requires PS 7.5.4 or greater and .net 9.0 or greater.'
  PowerShellVersion = '7.5.4'
  RequiredModules = @()
  RequiredAssemblies = @('System.Management.Automation','System.Globalization')
  ScriptsToProcess = @()
  TypesToProcess = @()
  FormatsToProcess = @(
    'Formatting/GetSnapResponseResult.Format.ps1xml'
    'Formatting/Publisher.Format.ps1xml'
    'Formatting/RemoveSnapResult.Format.ps1xml'
    'Formatting/SnapPackage.Format.ps1xml'
  )
  NestedModules = @()
  FunctionsToExport = @()
  CmdletsToExport = @(
    'Get-SnapPackage'
    'Install-SnapPackage'
    'Uninstall-SnapPackage'
    'Get-SnapdClient'
    'Set-SnapdClient'
  )
  VariablesToExport = @()
  AliasesToExport = @(
    'Remove-Snap'
    'Remove-Snaps'
    'Remove-SnapPackage'
    'Uninstall-Snap'
    'Uninstall-Snaps'
    'Uninstall-SnapPackages'
  )
  ModuleList = @()
  FileList = @(
    'lib/net9.0/PSnaps.dll'
    'lib/net9.0/PSnaps.xml'
    'Formatting/GetSnapResponseResult.Format.ps1xml'
    'Formatting/Publisher.Format.ps1xml'
    'Formatting/RemoveSnapResult.Format.ps1xml'
    'Formatting/SnapPackage.Format.ps1xml'
    'LICENSE'
    'PSnapsIcon.ico'
    'PSnapsIcon.png'
    'README.md'
    'PSnaps.psd1'
  )
  PrivateData = @{
    PSData = @{
      Tags = @('snap','snapd','Linux','PSEdition_Core','PSModule')
      LicenseUri = 'https://github.com/dodexahedron/PSnaps/blob/master/LICENSE'
      ProjectUri = 'https://github.com/dodexahedron/PSnaps'
      IconUri = 'https://github.com/dodexahedron/PSnaps/blob/master/PSnaps/PSnapsIcon.png'
      ReleaseNotes = 'Early development version. Functionality and API surface are subject to change.
0.14.0-Beta1: Fixed serialization issues with responses containing SnapPackage objects.'
      Prerelease = 'Beta1'
      RequireLicenseAcceptance = $false
      ExternalModuleDependencies = @()
      }
    }
  # HelpInfoURI = ''
}
