
using MeetingRoom.Domain;
using Model = MeetingRoom.Domain.Model;
using MeetingRoom.Domain.Storage;
using MeetingRoom.Infra;
using MeetingRoom.Infra.Storage;

namespace MeetingRoomTests;

public class MeetingRoomTest : IAsyncLifetime, IDisposable
{
    private readonly IRoomService roomService;

    public MeetingRoomTest() {
        IRoomStorage storage = new RoomStorageInMemory();
        roomService = new RoomService(storage);
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
}