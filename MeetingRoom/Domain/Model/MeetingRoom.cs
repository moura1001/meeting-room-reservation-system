namespace MeetingRoom.Domain.Model
{
    public class MeetingRoom : Room
    {
        private string name;

        public MeetingRoom(string name, int capacity, IList<Resource> resources) : base(capacity, resources)
        {
            Name = name;
        }

        public MeetingRoom(int roomId, string name, int capacity, IList<Resource> resources) : base(roomId, capacity, resources)
        {
            Name = name;
        }

        public string Name {
            get { return name; }

            private set {
                if (string.IsNullOrWhiteSpace(value))
                    throw new Exception("Name can't be null, empty or consists only of white-space characters");
                
                name = value;
            }
        }
    }
}