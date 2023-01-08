using WikipediaDAW.Models;

namespace WikipediaDAW.RequestModels
{
    public class LoginResponse<TResponse>
    {
        public bool IsSuccess { get; set; }

        public IEnumerable<string>? Errors { get; set; }

        public TResponse Result { get; set; }

        public LoginResponse(bool isSuccess, IEnumerable<string> errors, TResponse result)
        {
            IsSuccess = isSuccess;
            Errors = errors;   
            Result = result;
        }
    }
}
