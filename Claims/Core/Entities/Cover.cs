namespace Claims.Core.Entities
{
    public class Cover
    {
        public string Id { get; set; } = default!;

        public required DateOnly StartDate { get; set; }

        public required DateOnly EndDate { get; set; }

        public required Enums.CoverType Type { get; set; }

        public required decimal Premium { get; set; }
    }
}
