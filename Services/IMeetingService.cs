using MeetingsAPI_V3.Entities;
using MeetingsAPI_V3.Entities.Meeting;
using MeetingsAPI_V3.Models;
using Microsoft.AspNetCore.Mvc;
using System.ServiceModel;

namespace MeetingsAPI_V3.Services
{
    [ServiceContract]
    public interface IMeetingService
    {
        [OperationContract]
        Task<List<MeetingGetDto>> GetMeetingsAsync();

        [OperationContract]
        Task<ResponseModel<MeetingGetDto?>> GetMeetingAsync(int meetingId);

        [OperationContract]
        Task<Meeting> AddMeetingAsync(MeetingDto meeting);

        [OperationContract]
        Task<ResponseModel<Meeting>> UpdateMeeting(int id, MeetingDto meetingDto);

        [OperationContract]
        Task<ResponseModel<MeetingGetDto>> DeleteMeetingAsync(int meetingId);

        [OperationContract]
        Task<ResponseModel<MeetingWithUserObjDto>> AddUserToMeeting(int id, MeetingWithUserObjDto user);

        [OperationContract]
        Task<ResponseModel<User>> RemoveUserFromMeeting(int id, int userId);
    }
}
