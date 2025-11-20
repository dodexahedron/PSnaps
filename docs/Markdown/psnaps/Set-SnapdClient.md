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

Sets the client object the PSnaps module will use.

## SYNTAX

### AllParameterSets

```
Set-SnapdClient [-Client] <ISnapdRestClient> [-DoNotDisposeOldClient] [<CommonParameters>]
```

## DESCRIPTION

Sets the client object used by the PSnaps module for all interactions with the snapd REST API to the provided instance of a [PSnaps.SnapdRestApi.Clients.ISnapdClient] object, and returns a reference to the new instance.\
The object must be fully constructed and ready for use.

The client object that was previously in use will be disposed when calling this command, unless the `-DoNotDisposeOldClient` parameter is specified.

This command is not usually needed in normal use, but can be used for testing purposes and for changing the configuration of the client, such as the socket path, if necessary.

## EXAMPLES

### Example 1

Using the null client (which does nothing at all, discards all input, and outputs only null results):

```
Set-SnapdClient [PSnaps.SnapdRestApi.Clients.NullSnapdClient]::new()
```

### Example 2

Using the built-in SnapdClient, but with a different socket path:

```
Set-SnapdClient [PSnaps.SnapdRestApi.Clients.SnapdClient]::new('unix:///usr/run/snapd/restapi.sock')
```

### Example 3

Preserving the previous client so it can be reset later:

```
# Save a reference to the current client for later use.
$originalClient = Get-SnapdClient

# Set a new client, as in Example 2, but provide the -DoNotDisposeOldClient parameter, so our $originalClient stays alive.
$newClient = Set-SnapdClient [PSnaps.SnapdRestApi.Clients.SnapdClient]::new('unix:///usr/run/snapd/restapi.sock') -DoNotDisposeOldClient

# ... Do some stuff, which will now use the new client.

# Use our original client to reset things back to how they were before, automatically disposing the new client in the process.
# Do not attempt to access $newClient again after this step, as it will have been disposed!
Set-SnapdClient $originalClient

# ... Do some stuff, which now will use the original client.

```
## PARAMETERS

### -Client

A constructed and valid instance of a `[PSnaps.SnapdRestApi.Clients.ISnapdRestClient]` to use for future operations by the PSnaps module.

```yaml
Type: PSnaps.SnapdRestApi.Clients.ISnapdRestClient
DefaultValue: 'An instance of [PSnaps.SnapdRestApi.Clients.SnapdClient] ready for normal use.'
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
AcceptedValues: ['Any type implementing [PSnaps.SnapdRestApi.Clients.ISnapdRestClient]' ]
HelpMessage: 'A constructed and valid instance of a `[PSnaps.SnapdRestApi.Clients.ISnapdRestClient]` to use for future operations by the PSnaps module.'
```

### -DoNotDisposeOldClient

If set, the previous client object will will NOT be disposed. Default behavior is to dispose of the previous client object.

```yaml
Type: System.Management.Automation.SwitchParameter
DefaultValue: 'Not present'
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
HelpMessage: 'If set, the previous client object will will NOT be disposed. Default behavior is to dispose of the previous client object.'
```

### CommonParameters

This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable,
-InformationAction, -InformationVariable, -OutBuffer, -OutVariable, -PipelineVariable,
-ProgressAction, -Verbose, -WarningAction, and -WarningVariable. For more information, see
[about_CommonParameters](https://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### PSnaps.SnapdRestApi.Clients.ISnapdRestClient

A constructed and valid instance of a `[PSnaps.SnapdRestApi.Clients.ISnapdRestClient]` to use for future operations by the PSnaps module.

## OUTPUTS

### PSnaps.SnapdRestApi.Clients.ISnapdRestClient

A reference to the client that was provided to the command.

## NOTES

If you want to switch back to the previous client object without creating a new one, you should first run `Get-SnapdClient` and save it to a variable, and use the `-DoNotDisposeOldClient` switch parameter when calling `Set-SnapdClient`, so that the previous client will not be disposed when setting the new one.\
You can then use that variable as input to this command to return to using the previous client.
