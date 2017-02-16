namespace Arc.ActiveDirectory.Shared.Errors
{
    public class ErrorDetail
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public bool IsFatal { get; set; }
    }
}