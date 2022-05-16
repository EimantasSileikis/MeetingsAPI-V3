using MeetingsAPI_V3.Entities;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace MeetingsAPI_V3.Models
{
    [DataContract]
    public class MeetingWithUserObjDto
    {
        [Required]
        [DataMember(IsRequired = true)]
        public User[]? Users { get; set; }
    }
}
