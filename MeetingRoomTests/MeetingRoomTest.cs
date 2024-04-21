
using MeetingRoom.Domain;
using Model = MeetingRoom.Domain.Model;
using MeetingRoom.Domain.Storage;
using MeetingRoom.Infra;
using MeetingRoom.Infra.Storage;

namespace MeetingRoomTests;

public class MeetingRoomTest : IAsyncLifetime, IDisposable
{
    private readonly IRoomService roomService;
    private readonly IReservationService reservationService;

    public MeetingRoomTest() {
        IRoomStorage storage = new RoomStorageInMemory();
        roomService = new RoomService(storage);
        
        IReservationStorage reservationStorage = new ReservationStorageInMemory();
        reservationService = new ReservationService(reservationStorage, roomService);
    }

    // Before Each
    public async Task InitializeAsync()
    {
        await Task.Run(() => {
            roomService.DeleteAll();
            
            var resource1 = new Model.Resource("Projetor");
            var resource2 = new Model.Resource("Sistema de som");
            var resource3 = new Model.Resource("Quadro branco");
            
            roomService.Create(new Model.MeetingRoom(1, "Meeting Room 1", 5, [resource3]));
            roomService.Create(new Model.MeetingRoom(2, "Meeting Room 2", 10, [resource1, resource3]));
            roomService.Create(new Model.MeetingRoom(3, "Meeting Room 3", 20, [resource1, resource2, resource3]));
        });
    }
    
    // After Each
    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    // After All
    public void Dispose()
    {
    }

    [Fact]
    public void DeveriaListarTodasAsSalasDisponiveis()
    {
        IList<Model.Room> rooms = roomService.GetAll();
        Assert.NotEmpty(rooms);
        Assert.Equal(3, rooms.Count);

        Assert.True(rooms.Where(room => room.RoomId == 1 && room.Resources.Count == 1).Any());
        Assert.True(rooms.Where(room => room.RoomId == 2 && room.Resources.Count == 2).Any());
        Assert.True(rooms.Where(room => room.RoomId == 3 && room.Resources.Count == 3).Any());
    }

    [Fact]
    public void DeveriaReservarUmaSalaDeReuniaoComSucesso()
    {
        var startTime = DateTime.Now.AddDays(5);
        var endTime = startTime.AddHours(2);
        var organizer = "Random Organizer";
        var purpose = "Random Purpose";
        reservationService.BookMeetingRoom(1, new Model.Reservation(startTime, endTime, organizer, purpose));

        IList<Model.Reservation> reservations = reservationService.GetAll();
        Assert.NotEmpty(reservations);
        Assert.Equal(1, reservations.Count);
        Assert.Equal("Meeting Room 1", reservations.ElementAt(0).MeetingRoom.Name);
    }

    [Fact]
    public void DeveriaLancarExcecaoAoTentarReservarUmaSalaQueNaoExiste()
    {
        var startTime = DateTime.Now.AddDays(5);
        var endTime = startTime.AddHours(2);
        var organizer = "Random Organizer";
        var purpose = "Random Purpose";
        
        Exception throws = Assert.Throws<Exception>(
	        () => reservationService.BookMeetingRoom(99, new Model.Reservation(startTime, endTime, organizer, purpose))
        );
        Assert.Equal("Room 99 does not exist", throws.Message);
    }

    [Fact]
    public void DeveriaReservarUmaSalaComSucessoApenasComUmaAntecedenciaMinimaDe24Horas()
    {
        var startTime = DateTime.Now.AddHours(23).AddMinutes(59);
        var endTime = startTime.AddHours(2);
        var organizer = "Random Organizer";
        var purpose = "Random Purpose";

        Exception throws = Assert.Throws<Exception>(
	        () => reservationService.BookMeetingRoom(1, new Model.Reservation(startTime, endTime, organizer, purpose))
        );
        Assert.Equal("It is not possible to reserve a room less than 24 hours in advance", throws.Message);

        startTime = DateTime.Now.AddHours(24).AddSeconds(1);
        endTime = startTime.AddHours(2);
        reservationService.BookMeetingRoom(1, new Model.Reservation(startTime, endTime, organizer, purpose));

        IList<Model.Reservation> reservations = reservationService.GetAll();
        Assert.NotEmpty(reservations);
        Assert.Equal(1, reservations.Count);
        Assert.Equal("Meeting Room 1", reservations.ElementAt(0).MeetingRoom.Name);
    }

    [Fact]
    public void DeveriaLancarExcecaoAoTentarRealizar2ReservasQueSeSobrepoeNoMesmoHorarioParaAMesmaSala()
    {
        var startTime = DateTime.Now.AddDays(2);
        var endTime = startTime.AddHours(2);
        var organizer = "Random Organizer";
        var purpose = "Random Purpose";
        reservationService.BookMeetingRoom(1, new Model.Reservation(1, startTime, endTime, organizer, purpose));

        var reservation = reservationService.GetById(1);
        Assert.NotNull(reservation);
        Assert.Equal("Meeting Room 1", reservation.MeetingRoom.Name);

        // tenta iniciar 30 min antes da reserva cadastrada
        startTime = startTime.AddMinutes(-30);
        endTime = startTime.AddHours(1);
        Exception throws = Assert.Throws<Exception>(
	        () => reservationService.BookMeetingRoom(1, new Model.Reservation(startTime, endTime, organizer, purpose))
        );
        Assert.Equal("Reservation conflict: Two or more reservations cannot overlap at the same time for the same room", throws.Message);

        // tenta iniciar 30 min depois da reserva cadastrada
        startTime = DateTime.Now.AddDays(2).AddMinutes(30);
        endTime = startTime.AddHours(2);
        throws = Assert.Throws<Exception>(
	        () => reservationService.BookMeetingRoom(1, new Model.Reservation(startTime, endTime, organizer, purpose))
        );
        Assert.Equal("Reservation conflict: Two or more reservations cannot overlap at the same time for the same room", throws.Message);

        // tenta iniciar 30 min antes mas sobrepõe todo o horário da reserva cadastrada
        startTime = DateTime.Now.AddDays(2).AddMinutes(-30);
        endTime = startTime.AddHours(4);
        throws = Assert.Throws<Exception>(
	        () => reservationService.BookMeetingRoom(1, new Model.Reservation(startTime, endTime, organizer, purpose))
        );
        Assert.Equal("Reservation conflict: Two or more reservations cannot overlap at the same time for the same room", throws.Message);

        // consegue reservar pois o horário é 30 min antes e duração de 15 min
        startTime = DateTime.Now.AddDays(2).AddMinutes(-30);
        endTime = startTime.AddMinutes(15);
        reservationService.BookMeetingRoom(1, new Model.Reservation(2, startTime, endTime, organizer, purpose));

        IList<Model.Reservation> reservations = reservationService.GetAll();
        Assert.NotEmpty(reservations);
        Assert.Equal(2, reservations.Count);
        Assert.True(reservations.Where(r => r.ReservationId == 1 && string.Equals("Meeting Room 1", r.MeetingRoom.Name)).Any());
        Assert.True(reservations.Where(r => r.ReservationId == 2 && string.Equals("Meeting Room 1", r.MeetingRoom.Name)).Any());
    }
}