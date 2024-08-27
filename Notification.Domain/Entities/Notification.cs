namespace Notification.Domain.Entities
{
    public class Notification
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public DateTime SentAt { get; set; }
    }
}