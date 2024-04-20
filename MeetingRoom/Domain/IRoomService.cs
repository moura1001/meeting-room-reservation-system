using MeetingRoom.Domain.Model;

namespace MeetingRoom.Domain
{
    public interface IRoomService
    {
        Room Create(Room room);
        void DeleteAll();
        IList<Room> GetAll();
    }
}