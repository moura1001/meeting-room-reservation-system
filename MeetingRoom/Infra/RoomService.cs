using MeetingRoom.Domain;
using MeetingRoom.Domain.Model;
using MeetingRoom.Domain.Storage;

namespace MeetingRoom.Infra
{
    public class RoomService : IRoomService
    {
        private readonly IRoomStorage storage;

        public RoomService(IRoomStorage storage)
        {
            this.storage = storage;
        }

        public Room Create(Room room)
        {
            return storage.Create(room);
        }

        public void DeleteAll()
        {
            storage.DeleteAll();
        }

        public IList<Room> GetAll()
        {
            return storage.GetAll();
        }
    }
}