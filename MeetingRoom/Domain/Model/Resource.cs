namespace MeetingRoom.Domain.Model
{
    public class Resource
    {
        private static int counterId = 0;
        private int resourceId;
        private string name;

        public Resource(string name)
        {
            ResourceId = counterId++;
            Name = name;
        }

        public Resource(int resourceId, string name)
        {
            ResourceId = resourceId;
            Name = name;
        }

        public int ResourceId {
            get { return resourceId; }

            private set {
                if (value < 0)
                    throw new Exception("ResourceId must be a positive integer");
                
                resourceId = value;
            }
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