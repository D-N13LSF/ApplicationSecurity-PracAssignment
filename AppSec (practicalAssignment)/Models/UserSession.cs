namespace AppSec__practicalAssignment_.Models
{
    public class UserSession
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string SessionId { get; set; }
        public string IPAddress { get; set; }
        public DateTime LastActivity { get; set; }
    }
}
