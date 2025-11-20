---
document type: cmdlet
external help file: PSnaps.dll-Help.xml
HelpUri: ''
Locale: en-US
Module Name: psnaps
ms.date: 11/18/2025
PlatyPS schema version: 2024-05-01
title: Uninstall-SnapPackage
---

# Uninstall-SnapPackage

## SYNOPSIS

Uninstalls one or more installed snap packages from the system.

## SYNTAX

### SingleSnapAllRevisions (Default)

```
Uninstall-SnapPackage [-Snaps] <string[]> [-Purge] [-Timeout <int>] [<CommonParameters>]
```

### AllSnaps

```
Uninstall-SnapPackage -All [-Disabled] [-Purge] [-Timeout <int>] [<CommonParameters>]
```

### NamedSnaps

```
Uninstall-SnapPackage [-Snaps] <string[]> [-Disabled] [-Purge] [-Timeout <int>] [<CommonParameters>]
```

### SingleSnapByRevision

```
Uninstall-SnapPackage [-Snaps] <string[]> [-Revision] <int> [-Purge] [-Timeout <int>]
 [<CommonParameters>]
```

## DESCRIPTION

Uninstalls one or more installed snap packages from the system, according to the specified parameters.

## EXAMPLES

### Example 1

Uninstall a single snap package completely (all installed revisions):
```
Uninstall-SnapPackage lz4
```

### Example 2

Uninstall only the inactive instances of a single snap package:
```
Uninstall-SnapPackage lz4 -Disabled
```

### Example 3

Uninstall only a specific revision of a single snap package:
```
Uninstall-SnapPackage lz4 -Revision 4
```

### Example 4

Uninstall all inactive revisions of all snap packages installed on the system:
```
Uninstall-SnapPackage -All -Disabled
```


## PARAMETERS

### -All

In combination with the `-Disabled` parameter, instructs PSnaps to request uninstallation of all inactive revisions of all snap packages installed on the system.

```yaml
Type: System.Management.Automation.SwitchParameter
DefaultValue: ''
SupportsWildcards: false
Aliases: []
ParameterSets:
- Name: AllSnaps
  Position: Named
  IsRequired: true
  ValueFromPipeline: false
  ValueFromPipelineByPropertyName: false
  ValueFromRemainingArguments: false
DontShow: false
AcceptedValues: []
HelpMessage: ''
```

### -Disabled

Limits the operation to inactive revisions only.

```yaml
Type: System.Management.Automation.SwitchParameter
DefaultValue: ''
SupportsWildcards: false
Aliases: []
ParameterSets:
- Name: AllSnaps
  Position: Named
  IsRequired: false
  ValueFromPipeline: false
  ValueFromPipelineByPropertyName: false
  ValueFromRemainingArguments: false
- Name: NamedSnaps
  Position: Named
  IsRequired: false
  ValueFromPipeline: false
  ValueFromPipelineByPropertyName: false
  ValueFromRemainingArguments: false
DontShow: false
AcceptedValues: []
HelpMessage: ''
```

### -Purge

(Not yet implemented)\
If specified, uninstallation requests will include the purge parameter.

```yaml
Type: System.Management.Automation.SwitchParameter
DefaultValue: 'Not present'
SupportsWildcards: false
Aliases: []
ParameterSets:
- Name: AllSnaps
  Position: Named
  IsRequired: false
  ValueFromPipeline: false
  ValueFromPipelineByPropertyName: false
  ValueFromRemainingArguments: false
- Name: NamedSnaps
  Position: Named
  IsRequired: false
  ValueFromPipeline: false
  ValueFromPipelineByPropertyName: false
  ValueFromRemainingArguments: false
- Name: SingleSnapAllRevisions
  Position: Named
  IsRequired: false
  ValueFromPipeline: false
  ValueFromPipelineByPropertyName: false
  ValueFromRemainingArguments: false
- Name: SingleSnapByRevision
  Position: Named
  IsRequired: false
  ValueFromPipeline: false
  ValueFromPipelineByPropertyName: false
  ValueFromRemainingArguments: false
DontShow: false
AcceptedValues: []
HelpMessage: ''
```

### -Revision

When uninstalling a single snap package, explicitly specifies a single revision of that snap to uninstall.

```yaml
Type: System.Int32
DefaultValue: ''
SupportsWildcards: false
Aliases: []
ParameterSets:
- Name: SingleSnapByRevision
  Position: 1
  IsRequired: true
  ValueFromPipeline: false
  ValueFromPipelineByPropertyName: false
  ValueFromRemainingArguments: false
DontShow: false
AcceptedValues: []
HelpMessage: ''
```

### -Snaps

One or more names of snap packages to uninstall.

```yaml
Type: System.String[]
DefaultValue: ''
SupportsWildcards: false
Aliases: []
ParameterSets:
- Name: NamedSnaps
  Position: 0
  IsRequired: true
  ValueFromPipeline: false
  ValueFromPipelineByPropertyName: false
  ValueFromRemainingArguments: false
- Name: SingleSnapAllRevisions
  Position: 0
  IsRequired: true
  ValueFromPipeline: false
  ValueFromPipelineByPropertyName: false
  ValueFromRemainingArguments: false
- Name: SingleSnapByRevision
  Position: 0
  IsRequired: true
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

## OUTPUTS

### System.Collections.Generic.IEnumerable`1[[PSnaps.SnapdRestApi.Responses.SnapApiAsyncResponse]]

A collection of one or more async response objects from snapd, containing the action ID(s) for the operation(s).

## NOTES

This operation is asynchronous in snapd.\
The request should return quickly, while the actual operation may take longer.\
Status of the operation is indicated in the initial response and, if an action ID is returned, detailed progress information can be retrieved using that ID.
