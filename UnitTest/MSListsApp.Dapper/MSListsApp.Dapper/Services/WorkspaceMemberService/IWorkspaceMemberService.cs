using MSListsApp.Dapper.DTOs;

namespace MSListsApp.Dapper.Services.WorkspaceMemberService
{
    public interface IWorkspaceMemberService
    {
        int AddMember(WorkspaceMemberDto dto);
        void UpdateMember(WorkspaceMemberDto dto);
        void DeleteMember(int id);

    }
}