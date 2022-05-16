using MeetingsAPI_V3.Entities;
using System.ServiceModel;

namespace MeetingsAPI_V3.Services
{
    public interface IMeetingRepository
    {
        Task<IEnumerable<Meeting>> GetMeetingsAsync();
        Task<Meeting?> GetMeetingAsync(int meetingId);
        Task<bool> SaveChangesAsync();
        Task<bool> MeetingExistAsync(int meetingId);
        void AddMeetingAsync(Meeting meeting);
        Task DeleteMeetingAsync(int meetingId);
        Task<User?> FindUserAsync(int userId);
    }
}
