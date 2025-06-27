namespace Tournament.Api.Core.Entities
{
    internal class Tournament : BaseEntity
    {
        public string Name { get; set; }
        public int TeamCount { get; set; }
    }
}
