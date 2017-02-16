namespace Arc.ActiveDirectory.Shared.Commands
{
    public class ResetExpiredPasswordCommand
    {
        public string UserName { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}