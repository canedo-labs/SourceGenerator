using SourceGenerator.Core.Models.BusinessA.BusinessB;

namespace SourceGenerator.Core.Models.BusinessA
{
    public class BusinessADto
    {
        public int Amout { get; set; }
        public string Description { get; set; }
        public long Value { get; set; }
        public BusinessBDto BusinessBDto { get; set; }
        public IEnumerable<BusinessBDto> BusinessBDtos { get; set; }
    }
}