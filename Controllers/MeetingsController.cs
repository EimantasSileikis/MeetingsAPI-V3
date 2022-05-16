using AutoMapper;
using MeetingsAPI_V3.Entities;
using MeetingsAPI_V3.Models;
using MeetingsAPI_V3.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace MeetingsAPI_V3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeetingsController : ControllerBase
    {
        private readonly IMeetingRepository _meetingRepository;
        private readonly IMapper _mapper;

        //private readonly string _url = "http://contacts:5000/contacts/";
        private readonly string _url = "http://localhost/contacts/";

        static readonly HttpClient client = new HttpClient();

        public MeetingsController(IMeetingRepository meetingRepository,
            IMapper mapper)
        {
            _meetingRepository = meetingRepository ?? throw new ArgumentNullException(nameof(meetingRepository));
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MeetingGetDto>>> GetMeetings()
        {
            var meetingEntities = await _meetingRepository.GetMeetingsAsync();
            ICollection<MeetingGetDto> meetingList = new List<MeetingGetDto>();
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

            return Ok(meetingList);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Meeting>> GetMeeting(int id)
        {
            var meeting = await _meetingRepository.GetMeetingAsync(id);

            if (meeting == null)
            {
                return NotFound();
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

            return Ok(meetingDto);
        }

        [HttpPost]
        public async Task<ActionResult<Meeting>> CreateMeeting(MeetingDto meetingDto)
        {
            var meeting = _mapper.Map<Meeting>(meetingDto);
            meeting.Users = string.Empty;
            foreach (var user in meetingDto.Users)
            {
                if (user.Id == 0 || await _meetingRepository.FindUserAsync(user.Id) == null)
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

            _meetingRepository.AddMeetingAsync(meeting);
            await _meetingRepository.SaveChangesAsync();


            return CreatedAtAction(nameof(GetMeeting), new { id = meeting.Id }, meeting);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateMeeting(int id, MeetingDto meetingDto)
        {
            var meeting = await _meetingRepository.GetMeetingAsync(id);

            if (meeting == null)
            {
                return NotFound();
            }

            _mapper.Map(meetingDto, meeting);

            meeting.Users = string.Empty;
            foreach (var user in meetingDto.Users)
            {
                meeting.Users += user.Id + ",";
            }
            var usersStringWithoutComma = meeting.Users.Remove(meeting.Users.Length - 1);
            meeting.Users = usersStringWithoutComma;
            await _meetingRepository.SaveChangesAsync();

            return Ok(meeting);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMeeting(int id)
        {
            var meeting = await _meetingRepository.GetMeetingAsync(id);

            if (meeting == null)
            {
                return NotFound();
            }

            await _meetingRepository.DeleteMeetingAsync(id);
            await _meetingRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("{id}/Users")]
        public async Task<ActionResult<User>> AddUserToMeeting(int id, MeetingWithUserObjDto user)
        {
            var meeting = await _meetingRepository.GetMeetingAsync(id);

            if (meeting == null)
            {
                return NotFound();
            }

            if (user.Users == null) return BadRequest();

            if (meeting.Users.Length > 0)
            {
                meeting.Users += ",";
            }

            foreach (var userInfo in user.Users)
            {
                if (userInfo.Id == 0 || await _meetingRepository.FindUserAsync(userInfo.Id) == null)
                {
                    Random rnd = new Random();
                    int generatedId;
                    if (userInfo.Id == 0)
                    {
                        generatedId = rnd.Next(1, Int16.MaxValue);
                        userInfo.Id = generatedId;
                    }

                    HttpResponseMessage response = null!;

                    try
                    {
                        response = await client.PostAsJsonAsync(_url, userInfo);
                        if (response.IsSuccessStatusCode)
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


            await _meetingRepository.SaveChangesAsync();

            return Ok(user);
        }

        [HttpDelete("{id}/Users/{userId}")]
        public async Task<ActionResult> RemoveUserFromMeeting(int id, int userId)
        {
            var meeting = await _meetingRepository.GetMeetingAsync(id);

            if (meeting == null)
            {
                return NotFound();
            }

            string[] usersIds = new string[0];

            if (meeting.Users != string.Empty)
            {
                usersIds = meeting.Users.Split(',');
                int initialArrayLength = usersIds.Length;
                usersIds = usersIds.Where(x => x != userId.ToString()).ToArray();

                if (initialArrayLength == usersIds.Length)
                {
                    return NotFound();
                }

                meeting.Users = String.Join(",", usersIds);
                await _meetingRepository.SaveChangesAsync();
                return NoContent();

            }

            return NotFound();
        }

    }
}
