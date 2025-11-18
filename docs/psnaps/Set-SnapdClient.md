---
document type: cmdlet
external help file: PSnaps.dll-Help.xml
HelpUri: ''
Locale: en-US
Module Name: psnaps
ms.date: 11/18/2025
PlatyPS schema version: 2024-05-01
title: Set-SnapdClient
---

# Set-SnapdClient

## SYNOPSIS

{{ Fill in the Synopsis }}

## SYNTAX

### __AllParameterSets

```
Set-SnapdClient [-Client] <ISnapdRestClient> [-DoNotDisposeOldClient] [<CommonParameters>]
```

## ALIASES

This cmdlet has the following aliases,
  {{Insert list of aliases}}

## DESCRIPTION

{{ Fill in the Description }}

## EXAMPLES

### Example 1

{{ Add example description here }}

## PARAMETERS

### -Client

A constructed and valid instance of an [PSnaps.ISnapdRestClient] to use for future operations by the PSnaps module.

```yaml
Type: PSnaps.SnapdRestApi.Clients.ISnapdRestClient
DefaultValue: ''
SupportsWildcards: false
Aliases: []
ParameterSets:
- Name: (All)
  Position: 0
  IsRequired: true
  ValueFromPipeline: true
  ValueFromPipelineByPropertyName: false
  ValueFromRemainingArguments: false
DontShow: false
AcceptedValues: []
HelpMessage: ''
```

### -DoNotDisposeOldClient

If set, the previous client object will will NOT be disposed. Default behavior is to dispose of the previous client object.

```yaml
Type: System.Management.Automation.SwitchParameter
DefaultValue: ''
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
AcceptedValues: []
HelpMessage: ''
```

### CommonParameters

This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable,
-InformationAction, -InformationVariable, -OutBuffer, -OutVariable, -PipelineVariable,
-ProgressAction, -Verbose, -WarningAction, and -WarningVariable. For more information, see
[about_CommonParameters](https://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### PSnaps.SnapdRestApi.Clients.ISnapdRestClient

{{ Fill in the Description }}

## OUTPUTS

### PSnaps.SnapdRestApi.Clients.ISnapdRestClient

{{ Fill in the Description }}

## NOTES

{{ Fill in the Notes }}

## RELATED LINKS

{{ Fill in the related links here }}

