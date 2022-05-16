using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace MeetingsAPI_V3.Entities.Meeting
{
    [DataContract]
    public class Meeting
    {
        [Key]
        [Required]
        [DataMember]
        public int Id { get; init; }

        [Required]
        [DataMember(IsRequired = true)]
        public string Name { get; set; } = string.Empty;

        [DataMember]
        public string Users { get; set; } = string.Empty;
    }
}
