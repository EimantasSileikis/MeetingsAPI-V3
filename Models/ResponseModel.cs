namespace MeetingsAPI_V3.Models
{
    public class ResponseModel<T>
    {
        public T? Data { get; set; }
        public int ResultCode { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
