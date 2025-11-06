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
/// Indicates that the resource disposal must be handled at the use site,
/// meaning that the resource ownership is transferred to the caller.
/// This annotation can be used to annotate disposable types or their constructors individually to enable
/// the IDE code analysis for resource disposal in every context where the new instance of this type is created.
/// Factory methods and <c>out</c> parameters can also be annotated to indicate that the return value
/// of the disposable type needs handling.
/// </summary>
/// <remarks>
/// Annotation of input parameters with this attribute is meaningless.<br/>
/// Constructors inherit this attribute from their type if it is annotated,
/// but not from the base constructors they delegate to (if any).<br/>
/// Resource disposal is expected via <c>using (resource)</c> statement,
/// <c>using var</c> declaration, explicit <c>Dispose()</c> call, or passing the resource as an argument
/// to a parameter annotated with the <see cref="HandlesResourceDisposalAttribute"/> attribute.
/// </remarks>
[AttributeUsage(
                 AttributeTargets.Class
               | AttributeTargets.Struct
               | AttributeTargets.Constructor
               | AttributeTargets.Method
               | AttributeTargets.Parameter)]
[Conditional("JETBRAINS_ANNOTATIONS")]
[ExcludeFromCodeCoverage]
internal sealed class MustDisposeResourceAttribute : Attribute
{
  public MustDisposeResourceAttribute()
  {
    Value = true;
  }

  public MustDisposeResourceAttribute(bool value)
  {
    Value = value;
  }

  /// <summary>
  /// When set to <c>false</c>, disposing of the resource is not obligatory.
  /// The main use-case for explicit <c>[MustDisposeResource(false)]</c> annotation
  /// is to loosen the annotation for inheritors.
  /// </summary>
  public bool Value { get; }
}