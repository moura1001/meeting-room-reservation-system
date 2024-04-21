using MeetingRoom.Domain.Model;

namespace MeetingRoom.Domain.Storage
{
    public interface IReservationStorage
    {
        IList<Reservation> GetAll();
        Reservation GetById(int reservationId);
        Reservation Create(Reservation reservation);
    }
}