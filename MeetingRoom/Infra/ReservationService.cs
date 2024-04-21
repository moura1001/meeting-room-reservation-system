using MeetingRoom.Domain;
using Model = MeetingRoom.Domain.Model;
using MeetingRoom.Domain.Storage;

namespace MeetingRoom.Infra
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationStorage storage;
        private readonly IRoomService roomService;

        public ReservationService(IReservationStorage storage, IRoomService roomService)
        {
            this.storage = storage;
            this.roomService = roomService;
        }

        public void BookMeetingRoom(int roomId, Model.Reservation reservation)
        {
            var room = roomService.GetById(roomId);

            if (room is Model.MeetingRoom meetingRoom)
            {
                reservation.MeetingRoom = meetingRoom;
                storage.Create(reservation);
            }
        }

        public IList<Model.Reservation> GetAll()
        {
            return storage.GetAll();
        }

        public Model.Reservation GetById(int reservationId)
        {
            return storage.GetById(reservationId);
        }
    }
}