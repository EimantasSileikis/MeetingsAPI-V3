using MeetingsAPI_V3.Data;
using MeetingsAPI_V3.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace MeetingsAPI_V3.Services
{
    public class MeetingRepository: IMeetingRepository
    {
        private readonly DataContext _context;
        //private readonly string _url = "http://contacts:5000/contacts/";
        private readonly string _url = "http://localhost/contacts/";

        static readonly HttpClient client = new HttpClient();


        public MeetingRepository(DataContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Meeting>> GetMeetingsAsync()
        {
            return await _context.Meetings.ToListAsync();
        }

        public async Task<Meeting?> GetMeetingAsync(int meetingId)
        {
            return await _context.Meetings.Where(m => m.Id == meetingId).FirstOrDefaultAsync();
        }

        public void AddMeetingAsync(Meeting meeting)
        {
            _context.Add(meeting);
        }

        public async Task<bool> MeetingExistAsync(int meetingId)
        {
            return await _context.Meetings.AnyAsync(meeting => meeting.Id == meetingId);
        }

        public async Task DeleteMeetingAsync(int meetingId)
        {
            var meeting = await GetMeetingAsync(meetingId);
            if (meeting != null)
            {
                _context.Remove(meeting);
            }
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }

        public async Task<User?> FindUserAsync(int userId)
        {
            User? user = null;
            try
            {
                string responseBody = await client.GetStringAsync(_url + userId);
                user = JsonConvert.DeserializeObject<User>(responseBody);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Message :{0} ", e.Message);
                return null;
            }

            return user;
        }
    }
}
