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
                if (storage.HasReservationsForRoomBeteweenTimes(roomId, reservation.StartTime, reservation.EndTime))
                    throw new Exception("Reservation conflict: Two or more reservations cannot overlap at the same time for the same room");

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

        public void UpdateReservation(int reservationId, int roomId, Model.Reservation updatedReservation)
        {
            var reservation = storage.GetById(reservationId);

            if (reservation.MeetingRoom.RoomId != roomId)
            {
                var room = roomService.GetById(roomId);

                if (room is Model.MeetingRoom meetingRoom)
                {
                    reservation.MeetingRoom = meetingRoom;
                }
            }

            if ((updatedReservation.StartTime != reservation.StartTime || updatedReservation.EndTime != reservation.EndTime)
                &&
                storage.HasReservationsForRoomBeteweenTimes(roomId, updatedReservation.StartTime, updatedReservation.EndTime)
            )
                throw new Exception("Reservation conflict: Two or more reservations cannot overlap at the same time for the same room");
            
            reservation.StartTime = updatedReservation.StartTime;
            reservation.EndTime = updatedReservation.EndTime;
            reservation.Organizer = updatedReservation.Organizer;
            reservation.Purpose = updatedReservation.Purpose;

            storage.UpdateReservation(reservation);
        }

        public void DeleteAll()
        {
            storage.DeleteAll();
        }
    }
}