# SourceGenerator

### Studing how this works and help on development in real application 

### How it works
> This
```
namespace SourceGeneratorTest.Builders
{
    public static partial class ModelDtoBuilder { }
} 
```
> Will generate this
```
// Generated at 01/08/2022 13:38:39
using SourceGeneratorTest.Models;
using System.Runtime.InteropServices;

namespace SourceGeneratorTest.Builders
{
    public static partial class ModelDtoBuilder
    {
        public static ModelDto CreateDefault([Optional] ModelDto src)
        {
            return src ?? new ModelDto();
        }

        public static ModelDto WithMyProperty1(this ModelDto src, Int32 value)
        {
            src.MyProperty1 = value;
            return src;
        }

        public static ModelDto WithMyProperty2(this ModelDto src, Int32 value)
        {
            src.MyProperty2 = value;
            return src;
        }

        public static ModelDto WithCanedo(this ModelDto src, Int32 value)
        {
            src.Canedo = value;
            return src;
        }
    }
}
```

- To find generated codes folow Dependences > Analyzers > SourceGenerator > SourceGenerator.HeloWorldGenerator > ModelDtoBuilder
### Need to install .NET Compiler Platform SDK on Visual Studio Installer to debug generator

> If you need change to generate every build uncheck this option, go to Tools > Options > Text Editor > C# > Advanced > Skip analyzers for implicitly triggered builds

![image](https://user-images.githubusercontent.com/29046046/182046405-2fc0a7a2-0396-4da7-8a76-74f8d68bbe1b.png)

> Documentation
https://github.com/dotnet/roslyn/blob/main/docs/features/source-generators.cookbook.md
