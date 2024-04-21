using MeetingRoom.Domain.Model;
using MeetingRoom.Domain.Storage;

namespace MeetingRoom.Infra.Storage
{
    public class RoomStorageInMemory : IRoomStorage
    {
        private readonly IList<Room> rooms = [];

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

        public Room GetById(int roomId)
        {
            var room = rooms.FirstOrDefault(r => r.RoomId == roomId);

            if (room == null)
                throw new Exception($"Room {roomId} does not exist");

            return room;
        }
    }
}