@{
  RootModule = 'PSnaps.Mocks.dll'
  ModuleVersion = '1.0'
  CompatiblePSEditions = @('Core')
  GUID = 'e846a36e-2c97-4976-9142-e1c1f1e08d76'
  Author = 'Brandon Thetford'
  CompanyName = ''
  Copyright = '(c) Brandon Thetford. All rights reserved.'
  Description = 'A module containing mocks and mocking-related functionality for testing the PSnaps module. Requires the PSnaps module to be loaded.'
  PowerShellVersion = '7.5.4'
  RequiredModules = @('PSnaps')
  RequiredAssemblies = @()
  ScriptsToProcess = @()
  TypesToProcess = @()
  FormatsToProcess = @()
  NestedModules = @()
  FunctionsToExport = @()
  CmdletsToExport = @('Push-MockSnapdClientType','Pop-MockSnapdClientType')
  VariablesToExport = @()
  AliasesToExport = @()
  ModuleList = @('PSnaps.Mocks.dll')
  FileList = @('./PSnaps.Mocks.dll','./PSnaps.Mocks.psd1')

  PrivateData = @{
    PSData = @{
      Tags = @('PSnaps')
      LicenseUri = 'https://github.com/dodexahedron/PSnaps/blob/master/LICENSE'
      ProjectUri = 'https://github.com/dodexahedron/PSnaps'
      # IconUri = ''
      ReleaseNotes = 'Early development version. Functionality and API surface are subject to change.'
      Prerelease = 'Alpha'
      RequireLicenseAcceptance = $false
      ExternalModuleDependencies = @('PSnaps')
      }
    }
  # HelpInfoURI = ''
}