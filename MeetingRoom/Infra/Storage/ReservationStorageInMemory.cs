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
    }
}