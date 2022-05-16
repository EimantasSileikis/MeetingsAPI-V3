using AutoMapper;
using MeetingsAPI_V3.Entities;
using MeetingsAPI_V3.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace MeetingsAPI_V2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        //private readonly string _url = "http://contacts:5000/contacts/";
        private readonly string _url = "http://localhost/contacts/";

        static readonly HttpClient client = new HttpClient();

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            try
            {
                string responseBody = await client.GetStringAsync(_url);
                return Ok(JsonConvert.DeserializeObject<IEnumerable<User>>(responseBody));
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Message :{0} ", e.Message);
            }

            return NotFound();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            try
            {
                string responseBody = await client.GetStringAsync(_url + id);
                return Ok(JsonConvert.DeserializeObject<User>(responseBody));
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Message :{0} ", e.Message);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            HttpResponseMessage response = null!;

            try
            {
                response = await client.PostAsJsonAsync(_url, user);
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            if (response.IsSuccessStatusCode)
            {
                return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
            }

            return StatusCode(Int32.Parse(response.StatusCode.ToString()));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<User>> PutUser(int id, User userDto)
        {
            User user = null;
            try
            {
                string responseBody = await client.GetStringAsync(_url + id);
                user = JsonConvert.DeserializeObject<User>(responseBody);
            }
            catch (HttpRequestException e)
            {
                if(e.StatusCode != null)
                    return StatusCode((int)e.StatusCode);
                else
                    return BadRequest();
            }

            var response = await client.PutAsJsonAsync(_url + id, userDto);
            
            return StatusCode((int)response.StatusCode);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            HttpResponseMessage response = null!;

            response = await client.DeleteAsync(_url + id);
            var statusCode = (int)response.StatusCode;

            return StatusCode(statusCode);
        }


    }
}
