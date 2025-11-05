/*
 * The MIT License, in addition to the MPL-2.0 license of the project, applies to the C# code in this file.
 * The original source code this file is based on at the time the original source was retrieved can be found at https://github.com/JetBrains/JetBrains.Annotations/tree/92b2c395b6e43caadddacfa86856cd951d4d72b3
 * The following license text or the LICENSE.txt file in the directory this file is in must remain intact if this file is redistributed in whole or in part in any form.
 *
 * MIT License

   Copyright (c) 2016-2024 JetBrains s.r.o.

   Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

   The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

namespace JetBrains.Annotations;

/// <summary>
///   Specifies what is considered to be used implicitly when marked
///   with <see cref="MeansImplicitUseAttribute" /> or <see cref="UsedImplicitlyAttribute" />.
/// </summary>
[Flags]
[SuppressMessage ( "CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "It's totally necessary, bruh." )]
[SuppressMessage ( "Naming", "CA1711:Identifiers should not have incorrect suffix", Justification = "This is how JetBrains defines it." )]
[SuppressMessage ( "Design", "CA1069:Enums values should not be duplicated", Justification = "This is how JetBrains defines it." )]
internal enum ImplicitUseTargetFlags
{
  Default = Itself,

  /// <summary>Code entity itself.</summary>
  Itself = 1,

  /// <summary>Members of the type marked with the attribute are considered used.</summary>
  Members = 2,

  /// <summary> Inherited entities are considered used. </summary>
  WithInheritors = 4,

  /// <summary>Entity marked with the attribute and all its members considered used.</summary>
  WithMembers = Itself | Members
}