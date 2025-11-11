@{
  RootModule = 'lib\net9.0\PSnaps.dll'
  ModuleVersion = '0.3'
  CompatiblePSEditions = @('Core')
  GUID = '7a389ff6-dd4f-4486-b430-802de6fcc65c'
  Author = 'Brandon Thetford'
  CompanyName = ''
  Copyright = '(c) Brandon Thetford. All rights reserved.'
  Description = 'A PowerShell module and .net library for management of Snap packages on Linux.'
  PowerShellVersion = '7.5.4'
  RequiredModules = @()
  RequiredAssemblies = @('System.Management.Automation','System.Globalization')
  ScriptsToProcess = @()
  TypesToProcess = @()
  FormatsToProcess = @('Formatting\Publisher.Format.ps1xml','Formatting\GetSnapResponseResult.Format.ps1xml','Formatting\RemoveSnapResult.Format.ps1xml','Formatting\SnapPackage.Format.ps1xml')
  NestedModules = @()
  FunctionsToExport = @()
  CmdletsToExport = @('Get-SnapPackage','Install-SnapPackage','Uninstall-SnapPackage','Set-SnapdClient','Get-SnapdClient')
  VariablesToExport = @()
  AliasesToExport = @('Remove-Snap', 'Remove-Snaps', 'Remove-SnapPackages','Uninstall-Snap', 'Uninstall-Snaps', 'Uninstall-SnapPackages')
  ModuleList = @()
  FileList = @(
    'lib\net9.0\PSnaps.dll',
    'PSnaps.psd1',
    'Formatting\Publisher.Format.ps1xml',
    'Formatting\GetSnapResponseResult.Format.ps1xml',
    'Formatting\RemoveSnapResult.Format.ps1xml',
    'Formatting\SnapPackage.Format.ps1xml',
    'PSnapsIcon.png'
  )

  PrivateData = @{
    PSData = @{
      Tags = @('snap','snapd','linux')
      LicenseUri = 'https://github.com/dodexahedron/PSnaps/blob/master/LICENSE'
      ProjectUri = 'https://github.com/dodexahedron/PSnaps'
      # IconUri = ''
      ReleaseNotes = 'Early development version. Functionality and API surface are subject to change.'
      Prerelease = 'Alpha'
      RequireLicenseAcceptance = $false
      ExternalModuleDependencies = @()
      }
    }
  # HelpInfoURI = ''
}