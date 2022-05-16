using MeetingsAPI_V3.Entities;
using System.Runtime.Serialization;

namespace MeetingsAPI_V3.Models
{
    [DataContract]
    public class MeetingGetDto
    {
        [DataMember]
        public int Id { get; init; }

        [DataMember]
        public string Name { get; set; } = string.Empty;

        [DataMember]
        public List<User> Users { get; set; } = new List<User>();
    }
}
