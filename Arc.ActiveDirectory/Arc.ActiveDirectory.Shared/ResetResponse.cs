namespace Arc.ActiveDirectory.Shared
{
    public class ResetResponse
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public bool PerformRedirect { get; set; } //even if failure, server thinks client should just redirect, but can do so at its own discretion
        public int RetryTimeout { get; set; } //how many seconds until they can retry...
        public int Attempts { get; set; } //number of attempts so far - will NOT send number of attempts remaining, as this is TMI
        public string ResponseCode { get; set; } //need to think of a code for each response
        public bool RedirectUrl { get; set; }
        public bool ContactServiceDesk { get; set; }
    }
}