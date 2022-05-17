using MeetingsAPI_V3.Entities;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace MeetingsAPI_V3.Models
{
    [DataContract(Namespace = "http://www.example.com/meetings")]
    public class MeetingWithUserObjDto
    {
        [Required]
        [DataMember(IsRequired = true)]
        public User[]? Users { get; set; }
    }
}
