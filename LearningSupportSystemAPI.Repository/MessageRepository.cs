using LearningSupportSystemAPI.Contract;
using LearningSupportSystemAPI.Core.Database;
using LearningSupportSystemAPI.Core.Entities;

namespace LearningSupportSystemAPI.Repository
{
    public class MessageRepository : BaseRepository<Message>, IMessageRepository
    {
        public MessageRepository(ApplicationDbContext context) : base(context) { }
    }
}
