using MeetingRoom.Domain.Model;
using MeetingRoom.Domain.Storage;

namespace MeetingRoom.Infra.Storage
{
    public class RoomStorageInMemory : IRoomStorage
    {
        private IList<Room> rooms = [];

        public Room Create(Room room)
        {
            rooms.Add(room);
            return room;
        }

        public void DeleteAll()
        {
            rooms.Clear();
        }

        public IList<Room> GetAll()
        {
            return rooms.AsReadOnly();
        }
    }
}