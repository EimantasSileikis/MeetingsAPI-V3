using System.Runtime.Serialization;

namespace MeetingsAPI_V3.Models
{
    [DataContract(Namespace = "http://www.example.com/meetings")]
    public class UserDto
    {
        [DataMember(IsRequired = true, Order = 1)]
        public string Surname { get; set; } = string.Empty;

        [DataMember(IsRequired = true, Order = 2)]
        public string Name { get; set; } = string.Empty;

        [DataMember(IsRequired = true, Order = 3)]
        public string Number { get; set; } = string.Empty;

        [DataMember(IsRequired = true, Order = 4)]
        public string Email { get; set; } = string.Empty;
    }
}
