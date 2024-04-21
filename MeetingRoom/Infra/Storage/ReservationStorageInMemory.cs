using MeetingRoom.Domain.Model;
using MeetingRoom.Domain.Storage;

namespace MeetingRoom.Infra.Storage
{
    public class ReservationStorageInMemory : IReservationStorage
    {
        private readonly IList<Reservation> reservations = [];

        public Reservation Create(Reservation reservation)
        {
            reservations.Add(reservation);
            return reservation;
        }

        public IList<Reservation> GetAll()
        {
            return reservations;
        }

        public Reservation GetById(int reservationId)
        {
            var reservation = reservations.FirstOrDefault(r => r.ReservationId == reservationId);

            if (reservation == null)
                throw new Exception($"Reservation {reservationId} does not exist");

            return reservation;
        }

        public bool HasReservationsForRoomBeteweenTimes(int roomId, DateTime startTime, DateTime endTime)
        {
            return reservations.Where(reservation => 
                reservation.MeetingRoom.RoomId == roomId
                &&
                (
                    IsBeteweenTwoDates(startTime, reservation.StartTime, reservation.EndTime)
                    ||
                    IsBeteweenTwoDates(endTime, reservation.StartTime, reservation.EndTime)
                    ||
                    (startTime <= reservation.StartTime && endTime >= reservation.EndTime)
                )
            ).Any();
        }

        private bool IsBeteweenTwoDates(DateTime dt, DateTime start, DateTime end)
        {
            return dt >= start && dt <= end;
        }
    }
}