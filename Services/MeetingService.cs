using AutoMapper;
using MeetingsAPI_V3.Data;
using MeetingsAPI_V3.Entities.Meeting;
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
        private readonly string _url = "http://contacts:5000/contacts/";

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

        public async Task<ResponseModel<MeetingGetDto?>> GetMeetingAsync(int meetingId)
        {
            var meeting = await _context.Meetings.Where(m => m.Id == meetingId).FirstOrDefaultAsync();

            ResponseModel<MeetingGetDto> response = new ResponseModel<MeetingGetDto>();

            if(meeting == null)
            {
                response.Data = null;
                response.Message = "Meeting not found";
                response.ResultCode = 404;

                return response;
            }

            string[] usersIds = new string[0];

            if (meeting.Users != string.Empty)
            {
                usersIds = meeting.Users.Split(',');
            }


            List<User> userList = new List<User>();

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
                    break;
                }
            }

            MeetingGetDto meetingDto = new MeetingGetDto()
            {
                Id = meeting.Id,
                Name = meeting.Name,
                Users = userList
            };

            response.Data = meetingDto;
            response.Message = "Success";
            response.ResultCode = 200;

            return response;
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

        public async Task<ResponseModel<Meeting>> UpdateMeeting(int id, MeetingDto meetingDto)
        {
            var meeting = await _context.Meetings.Where(m => m.Id == id).FirstOrDefaultAsync();

            ResponseModel<Meeting> response = new ResponseModel<Meeting>();

            if (meeting == null)
            {
                response.ResultCode = 404;
                response.Message = "Meeting not found";
                return response;
            }

            _mapper.Map(meetingDto, meeting);
            Random rnd = new Random();

            meeting.Users = string.Empty;
            foreach (var user in meetingDto.Users)
            {
                var existingUser = await FindUserAsync(user.Id);
                if (user != null && existingUser != null)
                {
                    if (user.Equals(existingUser))
                    {
                        meeting.Users += user.Id + ",";
                        continue;
                    }

                    var newId = rnd.Next(1, Int16.MaxValue);
                    var userId = user.Id;
                    user.Id = newId;
                    var httpResponse = await client.PutAsJsonAsync(_url + userId, user);
                    if (httpResponse.IsSuccessStatusCode)
                    {
                        meeting.Users += newId + ",";
                    }
                    else
                    {
                        break;
                    }
                }
            }
            if (meeting.Users.Length > 1 && meeting.Users[meeting.Users.Length - 1] == ',')
            {
                var usersStringWithoutComma = meeting.Users.Remove(meeting.Users.Length - 1);
                meeting.Users = usersStringWithoutComma;
            }
            await _context.SaveChangesAsync();

            response.Data = meeting;
            response.ResultCode = 200;
            response.Message = "Meeting updated successfully";
            return response;
        }

        public async Task<ResponseModel<MeetingGetDto>> DeleteMeetingAsync(int meetingId)
        {
            var meeting = await GetMeetingAsync(meetingId);

            ResponseModel<MeetingGetDto> response = new ResponseModel<MeetingGetDto>();

            if (meeting.Data != null)
            {
                var meetingToDelete = await _context.Meetings.Where(m => m.Id == meetingId).FirstOrDefaultAsync();
                _context.Remove(meetingToDelete);
                await _context.SaveChangesAsync();
                response.Data = meeting.Data;
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

        public async Task<ResponseModel<MeetingWithUserObjDto>> AddUserToMeeting(int id, MeetingWithUserObjDto user)
        {
            var responseFromMeetings = await GetMeetingAsync(id);
            ResponseModel<MeetingWithUserObjDto> response = new ResponseModel<MeetingWithUserObjDto>();

            if (responseFromMeetings.Data == null)
            {
                response.Message = "Meeting not found";
                response.ResultCode = 404;
                return response;
            }

            if (user.Users == null) 
            {
                response.ResultCode = 400;
                response.Message = "Bad request";
                return response;
            }

            var meeting = await _context.Meetings.Where(m => m.Id == id).FirstOrDefaultAsync();

            if (meeting?.Users.Length > 0)
            {
                meeting.Users += ",";
            }

            foreach (var userInfo in user.Users)
            {
                if (userInfo.Id == 0 || await FindUserAsync(userInfo.Id) == null)
                {
                    Random rnd = new Random();
                    int generatedId;
                    if (userInfo.Id == 0)
                    {
                        generatedId = rnd.Next(1, Int16.MaxValue);
                        userInfo.Id = generatedId;
                    }

                    HttpResponseMessage httpResponse = null!;

                    try
                    {
                        httpResponse = await client.PostAsJsonAsync(_url, userInfo);
                        if (httpResponse.IsSuccessStatusCode)
                            meeting.Users += userInfo.Id + ",";
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        break;
                    }
                }
                else
                {
                    meeting.Users += userInfo.Id + ",";
                }
            }

            var usersStringWithoutComma = meeting.Users.Remove(meeting.Users.Length - 1);
            meeting.Users = usersStringWithoutComma;


            await _context.SaveChangesAsync();

            response.Data = user;
            response.Message = "User added successfully";
            response.ResultCode = 200;

            return response;
        }

        public async Task<ResponseModel<User>> RemoveUserFromMeeting(int id, int userId)
        {
            var meeting = await _context.Meetings.Where(m => m.Id == id).FirstOrDefaultAsync();
            ResponseModel<User> response = new ResponseModel<User>();

            if (meeting == null)
            {
                response.ResultCode = 404;
                response.Message = "Meeting not found";
                return response;
            }

            string[] usersIds = new string[0];

            if (meeting.Users != string.Empty)
            {
                usersIds = meeting.Users.Split(',');
                int initialArrayLength = usersIds.Length;
                usersIds = usersIds.Where(x => x != userId.ToString()).ToArray();

                if (initialArrayLength == usersIds.Length)
                {
                    response.ResultCode = 404;
                    response.Message = "User not found in meeting";
                    return response;
                }

                meeting.Users = String.Join(",", usersIds);
                await _context.SaveChangesAsync();

                response.ResultCode = 204;
                response.Message = "User removed successfully from meeting";
                return response;
            }

            response.ResultCode = 404;
            response.Message = "User not found in meeting";
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
