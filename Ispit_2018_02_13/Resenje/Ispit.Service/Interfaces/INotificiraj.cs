using System.Threading.Tasks;
using Ispit.Data.EntityModels;

namespace Ispit.Service.Interfaces
{
    public interface INotificiraj
    {
        Task<PoslataNotifikacija> CreateNotification(int stanjeObavezeId);
        string CreateNotificationMessageHtml(PoslataNotifikacija notifikacija);
    }
}