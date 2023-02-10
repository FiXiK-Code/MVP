using MVP.Date.Models;

namespace MVP.Date.Interfaces
{
    public interface IEmailService
    {
        void SendEmail(Message message);
    }
}
