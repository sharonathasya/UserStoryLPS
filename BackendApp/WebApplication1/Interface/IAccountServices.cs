using BackendApp.ViewModels;

namespace BackendApp.Interface
{
    public interface IAccountServices
    {
        Task<(bool status, string message, ResLogin data)> LoginAccount(ReqLogin req);
    }
}
