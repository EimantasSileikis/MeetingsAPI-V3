using AutoMapper;
using MeetingsAPI_V3.Data;
using MeetingsAPI_V3.Entities;
using MeetingsAPI_V3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace MeetingsAPI_V3.Services
{
    public class MeetingService: IMeetingService
    {
        private readonly DataContext _context;
        //private readonly string _url = "http://contacts:5000/contacts/";
        private readonly string _url = "http://localhost/contacts/";

        static readonly HttpClient client = new HttpClient();

        private readonly IMapper _mapper;

        public MeetingService(DataContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper;
        }

        public async Task<List<MeetingGetDto>> GetMeetingsAsync()
        {
            var meetingEntities = await _context.Meetings.ToListAsync();

            List<MeetingGetDto> meetingList = new List<MeetingGetDto>();
            bool serviceActive = true;

            foreach (var meeting in meetingEntities)
            {
                string[] usersIds = new string[0];

                if (meeting.Users != string.Empty)
                {
                    usersIds = meeting.Users.Split(',');
                }
                else
                {
                    MeetingGetDto meetingWithoutUsers = new MeetingGetDto()
                    {
                        Id = meeting.Id,
                        Name = meeting.Name
                    };
                    meetingList.Add(meetingWithoutUsers);
                    continue;
                }

                List<User> userList = new List<User>();

                if (serviceActive)
                {
                    foreach (var userId in usersIds)
                    {
                        try
                        {
                            string responseBody = await client.GetStringAsync(_url + userId);
                            var user = JsonConvert.DeserializeObject<User>(responseBody);
                            if (user != null)
                            {
                                userList.Add(user);
                            }
                        }
                        catch (HttpRequestException e)
                        {
                            Console.WriteLine("Message :{0} ", e.Message);
                            if (e.StatusCode == null)
                            {
                                serviceActive = false;
                            }
                            else if ((int)e.StatusCode == 404)
                            {
                                continue;
                            }

                            break;
                        }
                    }
                }


                MeetingGetDto meetingDto = new MeetingGetDto()
                {
                    Id = meeting.Id,
                    Name = meeting.Name,
                    Users = userList
                };
                meetingList.Add(meetingDto);
            }

            return meetingList;
        }

        public async Task<Meeting?> GetMeetingAsync(int meetingId)
        {
            return await _context.Meetings.Where(m => m.Id == meetingId).FirstOrDefaultAsync();
        }

        public async Task<Meeting> AddMeetingAsync(MeetingDto meetingDto)
        {
            var meeting = _mapper.Map<Meeting>(meetingDto);
            meeting.Users = string.Empty;

            foreach (var user in meetingDto.Users)
            {
                if (user.Id == 0 || await FindUserAsync(user.Id) == null)
                {
                    Random rnd = new Random();
                    int generatedId;
                    if (user.Id == 0)
                    {
                        generatedId = rnd.Next(1, Int16.MaxValue);
                        user.Id = generatedId;
                    }

                    HttpResponseMessage response = null!;

                    try
                    {
                        response = await client.PostAsJsonAsync(_url, user);
                        if (response.IsSuccessStatusCode)
                            meeting.Users += user.Id + ",";
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        break;
                    }
                }
                else
                {
                    meeting.Users += user.Id + ",";
                }

            }
            if (meeting.Users != string.Empty)
            {
                var usersStringWithoutComma = meeting.Users.Remove(meeting.Users.Length - 1);
                meeting.Users = usersStringWithoutComma;
            }

            _context.Add(meeting);
            await _context.SaveChangesAsync();

            return meeting;
        }



        public async Task<ResponseModel<Meeting>> DeleteMeetingAsync(int meetingId)
        {
            var meeting = await GetMeetingAsync(meetingId);

            ResponseModel<Meeting> response = new ResponseModel<Meeting>();

            if (meeting != null)
            {
                _context.Remove(meeting);
                await _context.SaveChangesAsync();
                response.Data = meeting;
                response.Message = "Meeting has been deleted successfully";
                response.ResultCode = 204;
            }
            else
            {
                response.Message = "Meeting not found";
                response.ResultCode = 404;
            }


            return response;
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
