using PickleballStore.BLL.ViewModels.Admin.Dashboard;

namespace PickleballStore.BLL.Services.Contracts
{
    public interface IDashboardService
    {
        Task<DashboardViewModel> GetDashboardDataAsync();
    }
}