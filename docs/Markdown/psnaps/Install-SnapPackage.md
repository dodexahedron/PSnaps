---
document type: cmdlet
external help file: PSnaps.dll-Help.xml
HelpUri: ''
Locale: en-US
Module Name: psnaps
ms.date: 11/18/2025
PlatyPS schema version: 2024-05-01
title: Install-SnapPackage
---

# Install-SnapPackage

## SYNOPSIS

Installs one or more specified snap packages.

## SYNTAX

### AllParameterSets

```
Install-SnapPackage [-Snaps] <string[]> [-TransactionMode <TransactionMode>] [-Timeout <int>]
 [<CommonParameters>]
```

## ALIASES

This cmdlet has the following aliases:
  Install-Snap

## DESCRIPTION

Installs one or more specified snap packages to the system.

## EXAMPLES

### Example 1

```powershell
Install-SnapPackage core24
```
Output:\
No output, currently.

## PARAMETERS

### -Snaps

A string or array of string names of the snap packages to install.

```yaml
Type: System.String[]
DefaultValue: ''
SupportsWildcards: false
Aliases: []
ParameterSets:
- Name: (All)
  Position: 0
  IsRequired: true
  ValueFromPipeline: false
  ValueFromPipelineByPropertyName: false
  ValueFromRemainingArguments: false
DontShow: false
AcceptedValues: 'Any valid snap package name'
HelpMessage: 'The name or names of the snap packages to install.'
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

### -TransactionMode

Specifies the transaction mode to use for the installation operation.\
If `[TransactionMode]::PerPackage` (default for PSnaps as well as snapd), each package installation uses an isolated
transaction and failures do not affect other installations (barring dependency issues).\
If `[TransactionMode]::AllPackage`, then all packages are installed in one transaction, and all must succeed or else
all installations will be rolled back.

```yaml
Type: PSnaps.SnapdRestApi.TransactionMode
DefaultValue: 'PerPackage'
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
AcceptedValues: [PerPackage,AllPackage]
HelpMessage: 'Specifies the transaction mode to use for the installation operation.'
```

### CommonParameters

This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable,
-InformationAction, -InformationVariable, -OutBuffer, -OutVariable, -PipelineVariable,
-ProgressAction, -Verbose, -WarningAction, and -WarningVariable. For more information, see
[about_CommonParameters](https://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

## OUTPUTS

### PSnaps.SnapdRestApi.Responses.SnapApiAsyncResponse

An async response object from snapd, containing the action ID for the operation.

## NOTES

This operation is asynchronous in snapd.\
The request should return quickly, while the actual operation may take longer.\
Status of the operation is indicated in the initial response and, if an action ID is returned, detailed progress information can be retrieved using that ID.


