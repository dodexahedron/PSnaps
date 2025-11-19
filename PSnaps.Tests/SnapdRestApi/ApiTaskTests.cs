#region COPYRIGHT
// Copyright 2025 Brandon Thetford
// 
// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0. If a copy of the MPL was not distributed with this file, You can obtain one at https://mozilla.org/MPL/2.0/.
// 
// A copy of the license is also available in the repository on GitHub at https://github.com/dodexahedron/PSnaps/blob/master/LICENSE.
#endregion

namespace PSnaps.Tests.SnapdRestApi;

[TestFixture]
[Category ( "Math" )]
[TestOf ( typeof( ApiTask ) )]
[TestOf ( typeof( ApiTask.TaskProgress ) )]
public class ApiTaskTests
{
  [Test]
  public void PercentComplete_PercentageOfDoneVsTotal ( [Range ( 0, 10 )] int done, [Values ( 10, 20 )] int total )
  {
    ApiTask.TaskProgress progress = new ( )
                                    {
                                      Done  = done,
                                      Total = total,
                                      Label = $"{done}/{total}"
                                    };
    Assume.That ( progress,       Is.Not.Null );
    Assume.That ( progress.Total, Is.Positive );
    Assume.That ( progress.Done,  Is.Not.Negative );
    Assume.That ( progress.Done,  Is.LessThanOrEqualTo ( progress.Total ) );

    Assert.That ( progress.PercentComplete, Is.EqualTo ( progress.Done / (double)progress.Total ) );
  }
}
