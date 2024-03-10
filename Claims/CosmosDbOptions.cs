namespace Claims
{
    public class CosmosDbOptions
    {
        public required string DatabaseName { get; set; }
        public required string CoverContainerName { get; set; }
        public required string ClaimContainerName { get; set; }
    }
}
