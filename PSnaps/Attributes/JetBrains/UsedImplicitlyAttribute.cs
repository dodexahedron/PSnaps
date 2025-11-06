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
#pragma warning disable CS8618
namespace JetBrains.Annotations;

/// <summary>
///   Indicates that the marked symbol is used implicitly (via reflection, in an external library, and so on),
///   so this symbol will be ignored by usage-checking inspections. <br />
///   You can use <see cref="ImplicitUseKindFlags" /> and <see cref="ImplicitUseTargetFlags" />
///   to configure how this attribute is applied.
/// </summary>
/// <example>
///   <code>
/// [UsedImplicitly]
/// public class TypeConverter { }
/// 
/// public class SummaryData
/// {
///   [UsedImplicitly(ImplicitUseKindFlags.InstantiatedWithFixedConstructorSignature)]
///   public SummaryData() { }
/// }
/// 
/// [UsedImplicitly(ImplicitUseTargetFlags.WithInheritors | ImplicitUseTargetFlags.Default)]
/// public interface IService { }
/// </code>
/// </example>
[AttributeUsage ( AttributeTargets.All )]
[Conditional ( "JETBRAINS_ANNOTATIONS" )]
[ExcludeFromCodeCoverage]
internal sealed class UsedImplicitlyAttribute : Attribute
{
  public UsedImplicitlyAttribute ( )
    : this ( ImplicitUseKindFlags.Default, ImplicitUseTargetFlags.Default )
  {
  }

  public UsedImplicitlyAttribute ( ImplicitUseKindFlags useKindFlags )
    : this ( useKindFlags, ImplicitUseTargetFlags.Default )
  {
  }

  public UsedImplicitlyAttribute ( ImplicitUseTargetFlags targetFlags )
    : this ( ImplicitUseKindFlags.Default, targetFlags )
  {
  }

  [SuppressMessage ( "Style",    "IDE0290:Use primary constructor", Justification = "It's how JetBrains defines it." )]
  public UsedImplicitlyAttribute ( ImplicitUseKindFlags useKindFlags, ImplicitUseTargetFlags targetFlags )
  {
    UseKindFlags = useKindFlags;
    TargetFlags  = targetFlags;
  }

  public ImplicitUseKindFlags UseKindFlags { get; }

  public ImplicitUseTargetFlags TargetFlags { get; }

  public string Reason { get; set; }
}
