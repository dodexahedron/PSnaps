#region COPYRIGHT
// Copyright 2025 Brandon Thetford
// 
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0. If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.
// 
// A copy of the license is also available in the repository on GitHub at https://github.com/dodexahedron/PSnaps/blob/master/LICENSE.
#endregion

namespace PSnaps.Cmdlets;

/// <summary>
///   Gets the current instance of the <see cref="ISnapdRestClient" /> in use by the module and writes it to the pipeline.
/// </summary>
[Cmdlet ( VerbsCommon.Get, Noun )]
[OutputType ( typeof( ISnapdRestClient ) )]
[PublicAPI]
public sealed class GetSnapdClientCommand : SnapdClientCmdlet
{
  private const string Noun = "SnapdClient";

  /// <inheritdoc />
  [ExcludeFromCodeCoverage]
  protected override void ProcessRecord ( )
  {
    WriteObject ( ApiClient );
  }
}
