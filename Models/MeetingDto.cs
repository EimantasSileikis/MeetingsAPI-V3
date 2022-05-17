using MeetingsAPI_V3.Entities;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace MeetingsAPI_V3.Models
{
    [DataContract(Namespace = "http://www.example.com/meetings")]
    public class MeetingDto
    {
        [Required]
        [DataMember(IsRequired = true)]
        public string Name { get; set; } = string.Empty;

        [DataMember(IsRequired = true, Name="Users")]
        public User[]? Users { get; set; }
    }
}
