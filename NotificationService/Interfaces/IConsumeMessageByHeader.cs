namespace NotificationService.Interfaces;

public interface IConsumeMessageByHeader
{
    string HeaderKey => "blog.events.type";
    string HeaderValue { get; }
}