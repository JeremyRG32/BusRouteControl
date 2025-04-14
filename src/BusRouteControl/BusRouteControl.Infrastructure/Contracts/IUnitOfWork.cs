using BusRouteControl.Infrastructure.Interfaces;

namespace BusRouteControl.Infrastructure.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        IBusRouteRepository BusRoutes { get; }
        IUserRepository Users { get; }
        ITicketRepository Tickets { get; }
        IScheduleRepository Schedules { get; }
        Task<int> SaveAsync();
    }
}
