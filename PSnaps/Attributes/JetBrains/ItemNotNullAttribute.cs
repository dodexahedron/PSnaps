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
///   Can be applied to symbols of types derived from <c>IEnumerable</c> as well as to symbols of <c>Task</c>
///   and <c>Lazy</c> classes to indicate that the value of a collection item, of the <c>Task.Result</c> property,
///   or of the <c>Lazy.Value</c> property can never be null.
/// </summary>
/// <example>
///   <code>
/// public void Foo([ItemNotNull]List&lt;string&gt; books)
/// {
///   foreach (var book in books)
///   {
///     if (book != null) // Warning: Expression is always true
///     {
///       Console.WriteLine(book.ToUpper());
///     }
///   }
/// }
/// </code>
/// </example>
[AttributeUsage (
                  AttributeTargets.Method
                | AttributeTargets.Parameter
                | AttributeTargets.Property
                | AttributeTargets.Delegate
                | AttributeTargets.Field
                )]
[Conditional ( "JETBRAINS_ANNOTATIONS" )]
[ExcludeFromCodeCoverage]
internal sealed class ItemNotNullAttribute : Attribute
{
}