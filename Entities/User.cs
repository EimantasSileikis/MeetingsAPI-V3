using System.Runtime.Serialization;

namespace MeetingsAPI_V3.Entities
{
    [DataContract]
    public class User
    {
        [DataMember(Order = 1)]
        public int Id { get; set; }

        [DataMember(IsRequired = true, Order = 2)]
        public string Surname { get; set; } = string.Empty;

        [DataMember(IsRequired = true, Order = 3)]
        public string Name { get; set; } = string.Empty;

        [DataMember(IsRequired = true, Order = 4)]
        public string Number { get; set; } = string.Empty;

        [DataMember(IsRequired = true, Order = 5)]
        public string Email { get; set; } = string.Empty;

        public override bool Equals(object? obj)
        {
            return obj is User user &&
                   Id == user.Id &&
                   Surname == user.Surname &&
                   Name == user.Name &&
                   Number == user.Number &&
                   Email == user.Email;
        }
    }


}
