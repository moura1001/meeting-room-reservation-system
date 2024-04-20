namespace MeetingRoom.Domain.Model
{
    public abstract class Room
    {
        private static int counterId = 0;
        protected int roomId;
        protected int capacity;
        protected IList<Resource> resources;

        protected Room(int capacity, IList<Resource> resources)
        {
            RoomId = counterId++;
            Capacity = capacity;
            Resources = resources;
        }

        protected Room(int roomId, int capacity, IList<Resource> resources)
        {
            RoomId = roomId;
            Capacity = capacity;
            Resources = resources;
        }

        public int RoomId {
            get { return roomId; }

            private set {
                if (value < 0)
                    throw new Exception("RoomId must be a positive integer");
                
                roomId = value;
            }
        }

        public int Capacity {
            get { return capacity; }

            private set {
                if (value <= 0)
                    throw new Exception("Capacity must be a positive integer greater than zero");
                
                capacity = value;
            }
        }

        public IList<Resource> Resources {
            get { return resources.AsReadOnly(); }

            private set {
                if (value == null || value.Count < 1)
                    throw new Exception("A room must have at least 1 resource");
                
                resources = value;
            }
        }
    }
}