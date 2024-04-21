using MeetingRoom.Domain.Model;

namespace MeetingRoom.Domain
{
    public interface IReservationService
    {
        void BookMeetingRoom(int roomId, Reservation reservation);
        IList<Reservation> GetAll();
        Reservation GetById(int reservationId);
    }
}