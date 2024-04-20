using MeetingRoom.Domain.Model;

namespace MeetingRoom.Domain.Storage
{
    public interface IRoomStorage
    {
        Room Create(Room room);
        void DeleteAll();
        IList<Room> GetAll();
    }
}