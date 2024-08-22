using BackendApp.ViewModels;

namespace BackendApp.Interface
{
    public interface IUserServices
    {
        Task<(bool status, string message)> AddDataUser(ReqAddUser req);
        Task<(bool status, string message)> DeleteData(ReqIdUser userId);
        Task<(bool status, string message)> GetUserByEmail(ReqIdUser email);
    }
}
