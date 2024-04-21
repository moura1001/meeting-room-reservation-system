namespace MeetingRoom.Domain.Model
{
    public class Reservation
    {
        private static int counterId = 0;
        private int reservationId;
        private DateTime startTime;
        private DateTime endTime;
        private string organizer;
        private string purpose;
        private MeetingRoom meetingRoom;

        public Reservation(DateTime startTime, DateTime endTime, string organizer, string purpose)
        {
            ReservationId = counterId++;
            StartTime = startTime;
            EndTime = endTime;
            Organizer = organizer;
            Purpose = purpose;
        }

        public Reservation(int reservationId, DateTime startTime, DateTime endTime, string organizer, string purpose)
        {
            ReservationId = reservationId;
            StartTime = startTime;
            EndTime = endTime;
            Organizer = organizer;
            Purpose = purpose;
        }

        public int ReservationId {
            get { return reservationId; }

            private set {
                if (value < 0)
                    throw new Exception("ReservationId must be a positive integer");
                
                reservationId = value;
            }
        }

        public DateTime StartTime {
            get { return startTime; }

            set {
                if (value < DateTime.Now.AddHours(24))
                    throw new Exception("It is not possible to reserve a room less than 24 hours in advance");

                startTime = value;
            }
        }

        public DateTime EndTime {
            get { return endTime; }

            set {
                endTime = value;
            }
        }

        public string Organizer {
            get { return organizer; }

            private set {
                if (string.IsNullOrWhiteSpace(value))
                    throw new Exception("Organizer can't be null, empty or consists only of white-space characters");
                
                organizer = value;
            }
        }

        public string Purpose {
            get { return purpose; }

            private set {
                if (string.IsNullOrWhiteSpace(value))
                    throw new Exception("Purpose can't be null, empty or consists only of white-space characters");
                
                purpose = value;
            }
        }

        public MeetingRoom MeetingRoom {
            get { return meetingRoom; }

            set {
                meetingRoom = value;
            }
        }
    }
}