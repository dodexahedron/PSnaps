@{
  RootModule = 'lib/net9.0/PSnaps.dll'
  ModuleVersion = '0.18.0'
  CompatiblePSEditions = @('Core')
  GUID = '7a389ff6-dd4f-4486-b430-802de6fcc65c'
  Author = 'dodexahedron'
  CompanyName = ''
  Copyright = '2025 Brandon Thetford. All rights reserved.'
  Description = 'A PowerShell module and .net library for management of Snap packages on Linux. Requires PS 7.5.4 or greater and .net 9.0 or greater.'
  PowerShellVersion = '7.5.4'
  RequiredModules = @()
  RequiredAssemblies = @()
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
    'lib/net10.0/PSnaps.dll'
    'lib/net10.0/PSnaps.xml'
    'Formatting/GetSnapResponseResult.Format.ps1xml'
    'Formatting/Publisher.Format.ps1xml'
    'Formatting/RemoveSnapResult.Format.ps1xml'
    'Formatting/SnapPackage.Format.ps1xml'
    'LICENSE'
    'PSnapsIcon.ico'
    'PSnapsIcon.png'
    'README.md'
    'PSnaps.psd1'
    'en-US/PSnaps.dll-Help.xml'
  )
  PrivateData = @{
    PSData = @{
      Tags = @('snap','snapd','Linux','PSEdition_Core','PSModule')
      LicenseUri = 'https://github.com/dodexahedron/PSnaps/blob/master/LICENSE'
      ProjectUri = 'https://github.com/dodexahedron/PSnaps'
      IconUri = 'https://github.com/dodexahedron/PSnaps/blob/master/PSnaps/PSnapsIcon.png'
      ReleaseNotes = '0.18.0-Beta1: Adds multi-targeting for .net9/10 and PowerShell 7.5/7.6.
Switched to the PowershellStandard.Library package instead of System.Management.Automation for compatibility. with .net 10 target.
Adds module help XML.
0.16.0-Beta1: Corrected property name for Tracking column of SnapPackage formatting.
0.15.0-Beta1: Altered SnapPackage formatting to remove the grouping that was not as helpful as predicted.'
      Prerelease = 'Beta1'
      RequireLicenseAcceptance = $false
      ExternalModuleDependencies = @()
      }
    }
  # HelpInfoURI = ''
}
