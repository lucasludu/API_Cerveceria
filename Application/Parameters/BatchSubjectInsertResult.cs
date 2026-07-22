namespace Application.Parameters
{
    public class BatchSubjectInsertResult
    {
        public int InsertedCount { get; set; }
        public List<string> Errors { get; set; } = new();
    }
}
