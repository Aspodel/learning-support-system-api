using LearningSupportSystemAPI.Contract;
using LearningSupportSystemAPI.Core.Database;
using LearningSupportSystemAPI.Core.Entities;

namespace LearningSupportSystemAPI.Repository
{
    public class NotificationRepository : BaseRepository<Notification>, INotificationRepository
    {
        public NotificationRepository(ApplicationDbContext context) : base(context) { }
    }
}
