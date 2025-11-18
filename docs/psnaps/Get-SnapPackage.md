---
document type: cmdlet
external help file: PSnaps.dll-Help.xml
HelpUri: ''
Locale: en-US
Module Name: psnaps
ms.date: 11/18/2025
PlatyPS schema version: 2024-05-01
title: Get-SnapPackage
---

# Get-SnapPackage

## SYNOPSIS

Gets one or more snap packages, according to input options.

## SYNTAX

### AllParameterSets

```
Get-SnapPackage [[-Name] <string[]>] [-All] [-Timeout <int>] [<CommonParameters>]
```

## ALIASES

This cmdlet has the following aliases:
  Get-Snap
  Get-Snaps
  Get-SnapPackages

## DESCRIPTION

Gets one or more snap packages, according to input options, as instances of [PSnaps.SnapCore.SnapPackage].

## EXAMPLES

### Example 1

To get all installed snap packages:
```powershell
Get-SnapPackage -All
```
Output:
```text
Name                                             Version Revision Tracking                   Publisher          Publisher Verified
----                                             ------- -------- --------                   ---------          ------------------
bare                                                 1.0        5 latest/stable              Canonical                 True
core20                                          20250822     2682 latest/stable              Canonical                 True
core22                                          20250923     2139 latest/stable              Canonical                 True
core24                                          20251001     1225 latest/stable              Canonical                 True
desktop-security-center                    0+git.d93b42d       99 1/stable/ubuntu-25.10      Canonical                 True
firefox                                        144.0.2-1     7177 latest/stable/ubuntu-25.10 Mozilla                   True
firefox                                          145.0-2     7298 latest/stable/ubuntu-25.10 Mozilla                   True
firmware-updater                           0+git.0052f6b      210 1/stable/ubuntu-25.10      Canonical                 True
gnome-42-2204             0+git.837775c-sdk0+git.7b07595      226 latest/stable              Canonical                 True
gtk-common-themes                        0.1-81-g442e511     1535 latest/stable              Canonical                 True
lz4                                                1.9.4        4 latest/stable              Edward Hope-Morley       False
mesa-2404                                 25.0.7-snap211     1165 latest/stable/ubuntu-25.10 Canonical                 True
powershell-preview                       7.6.0-preview.5      179 latest/stable              Canonical                 True
prompting-client                           0+git.d542a5d      104 1/stable/ubuntu-25.10      Canonical                 True
snap-store                                0+git.90575829     1270 2/stable/ubuntu-25.10      Canonical                 True
snapd                                               2.72    25577 latest/stable              Canonical                 True
snapd-desktop-integration                            0.9      315 latest/stable/ubuntu-25.10 Canonical                 True
thunderbird                                 140.4.0esr-1      846 latest/stable/ubuntu-25.10 Canonical                 True
```

### Example 2

To get a single snap package:
```powershell
Get-SnapPackage lz4
```

Output:
```text
Name Version Revision Tracking      Publisher          Publisher Verified
---- ------- -------- --------      ---------          ------------------
lz4    1.9.4        4 latest/stable Edward Hope-Morley       False
```

## PARAMETERS

### -All

If one or more package names are specified, returns both active and inactive instances of the package(s) currently installed on the system.

If no named packages are specified, returns both active and inactive instances of ALL packages currently installed on the system.

```yaml
Type: System.Management.Automation.SwitchParameter
DefaultValue: 'Not Present'
SupportsWildcards: false
Aliases:
- IncludeInactive
ParameterSets:
- Name: (All)
  Position: Named
  IsRequired: false
  ValueFromPipeline: false
  ValueFromPipelineByPropertyName: false
  ValueFromRemainingArguments: false
DontShow: false
AcceptedValues: []
HelpMessage: 'Include all instances of the specified packages in output or, if none are specified, include all instances of all packages in output.'
```

### -Name

The name (not case sensitive) or names of one or more snap packages to include in output, if they exist.\
Multiple inputs are specified as an array of strings.

```yaml
Type: System.String[]
DefaultValue: ''
SupportsWildcards: false
Aliases: []
ParameterSets:
- Name: (All)
  Position: 0
  IsRequired: false
  ValueFromPipeline: false
  ValueFromPipelineByPropertyName: false
  ValueFromRemainingArguments: false
DontShow: false
AcceptedValues: []
HelpMessage: ''
```

### -Timeout

A timeout (in milliseconds) for the operation, to guard against the snapd REST API socket being unreachable or unresponsive.

```yaml
Type: System.Int32
DefaultValue: '30000'
SupportsWildcards: false
Aliases: []
ParameterSets:
- Name: (All)
  Position: Named
  IsRequired: false
  ValueFromPipeline: false
  ValueFromPipelineByPropertyName: false
  ValueFromRemainingArguments: false
DontShow: false
AcceptedValues: [1,int.MaxValue]
HelpMessage: 'A timeout (in milliseconds) for the operation, to guard against the snapd REST API socket being unreachable or unresponsive.'
```

### CommonParameters

This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable,
-InformationAction, -InformationVariable, -OutBuffer, -OutVariable, -PipelineVariable,
-ProgressAction, -Verbose, -WarningAction, and -WarningVariable. For more information, see
[about_CommonParameters](https://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS
None

## OUTPUTS

### System.Collections.Generic.List`1[[PSnaps.SnapCore.SnapPackage]]
Always

## NOTES

Output is always a collection, regardless of the number of requested snaps or the number of results returned.
