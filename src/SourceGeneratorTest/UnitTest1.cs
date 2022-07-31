using SourceGeneratorTest.Builders;

namespace SourceGeneratorTest
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            ModelDtoBuilder.CreateDefault().WithMyProperty1(1);
        }
    }
}