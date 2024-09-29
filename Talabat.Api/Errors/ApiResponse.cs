
namespace Talabat.Api.Errors
{
    public class ApiResponse
    {
        public int StatusCode {  get; set; }
        public string? Message {  get; set; }
        public ApiResponse(int statuscode,string? message=null) {
            StatusCode = statuscode;
            Message = message??getDefaultMessageForStatusCode(statuscode);
        }

        private string getDefaultMessageForStatusCode(int statuscode)
        {
            return statuscode switch
            {
                400 => "BadRequst",
                401 => "UnAuthorize",
                404 => "NotFound",
                500 => "ServerError",
                _ => "Null"
            };
        }
    }
}
