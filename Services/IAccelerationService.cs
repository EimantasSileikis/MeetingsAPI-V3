using MeetingsAPI_V3.Entities;
using MeetingsAPI_V3.Models;
using System.ServiceModel;

namespace MeetingsAPI_V3.Services
{
    [ServiceContract]
    public interface IAccelerationService
    {
        [OperationContract]
        string Speed(string s);
        [OperationContract]
        void XmlMethod(System.Xml.Linq.XElement xml);
        
        [OperationContract]
        User TestUser(User user);
    }
}
