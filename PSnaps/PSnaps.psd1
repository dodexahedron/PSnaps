@{
  RootModule = 'PSnaps.dll'
  ModuleVersion = '0.1'
  CompatiblePSEditions = @('Core')
  GUID = '7a389ff6-dd4f-4486-b430-802de6fcc65c'
  Author = 'Brandon Thetford'
  CompanyName = ''
  Copyright = '(c) Brandon Thetford. All rights reserved.'
  Description = 'A PowerShell module and .net library for management of Snap packages on Linux.'
  PowerShellVersion = '7.6.0'
  RequiredModules = @('Microsoft.PowerShell.Management')
  RequiredAssemblies = @('System.Management.Automation','System.Globalization')
  ScriptsToProcess = @()
  TypesToProcess = @()
  FormatsToProcess = @('./Formatting/Publisher.format.ps1xml','./Formatting/GetSnapResponseResult.format.ps1xml','./Formatting/RemoveSnapResult.format.ps1xml')
  NestedModules = @()
  FunctionsToExport = @()
  CmdletsToExport = @('Get-SnapPackage','Remove-SnapPackage')
  VariablesToExport = @()
  AliasesToExport = @()
  ModuleList = @('PSnaps.dll')
  FileList = @('PSnaps.dll','PSnaps.psd1','./Formatting/Publisher.format.ps1xml','./Formatting/GetSnapResponseResult.format.ps1xml','./Formatting/RemoveSnapResult.format.ps1xml')

  PrivateData = @{
    PSData = @{
      Tags = @('snap','snapd','linux')
      LicenseUri = 'https://github.com/dodexahedron/PSnaps/blob/master/LICENSE'
      ProjectUri = 'https://github.com/dodexahedron/PSnaps'
      # IconUri = ''
      ReleaseNotes = 'Early development version. Functionality and API surface are subject to change.'
      Prerelease = 'Alpha01'
      RequireLicenseAcceptance = $false
      ExternalModuleDependencies = @()
      }
    }
  # HelpInfoURI = ''
}