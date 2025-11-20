---
document type: cmdlet
external help file: PSnaps.dll-Help.xml
HelpUri: ''
Locale: en-US
Module Name: psnaps
ms.date: 11/18/2025
PlatyPS schema version: 2024-05-01
title: Get-SnapdClient
---

# Get-SnapdClient

## SYNOPSIS

Gets the current snapd client in use by the PSnaps module.

## SYNTAX

### AllParameterSets

```
Get-SnapdClient [-Timeout <int>] [<CommonParameters>]
```

## DESCRIPTION

Retrieves the instance of a [PSnaps.SnapdRestApi.Clients.ISnapdClient] that the PSnaps module is using for interaction with snapd.

By default, this is an instance of [PSnaps.SnapdRestApi.Clients.SnapdClient].

## EXAMPLES

### Example 1

Get the current client instance used by PSnaps:

```
Get-SnapdClient
```

Output:

```
BaseUri              SnapdUnixSocketUri
-------              ------------------
http://localhost/v2/ unix:///run/snapd.socket
```

## PARAMETERS

### -Timeout

Not used by this cmdlet.

```yaml
Type: System.Int32
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
HelpMessage: 'Not used by this cmdlet.'
```

### CommonParameters

This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable,
-InformationAction, -InformationVariable, -OutBuffer, -OutVariable, -PipelineVariable,
-ProgressAction, -Verbose, -WarningAction, and -WarningVariable. For more information, see
[about_CommonParameters](https://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS
None

## OUTPUTS

### PSnaps.SnapdRestApi.Clients.ISnapdRestClient

The client instance in used by the PSnaps module.
