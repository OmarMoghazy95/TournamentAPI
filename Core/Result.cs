namespace Tournament.Api.Core
{

    public class Response<T>
    {
        public string Message { get; set; }
        public T Data { get; set; }

        public IEnumerable<string> Errors { get; set; }

        public ResultEnum Status { get; set; }


        public Response(ResultEnum status, string message, T data = default)
        {
            Message = message;
            Data = data;
            Errors = new HashSet<string>();
            Status = status;
        }
        public Response(ResultEnum status, string message, IEnumerable<string> errors, T data = default)
        {
            Message = message;
            Data = data;
            Errors = new HashSet<string>();
            Status = status;
            Errors = errors;
        }
        public Response()
        {

        }
    }
    public enum ResultEnum
    {
        Success,
        Validation,
        Error,
    }
    public class Result
    {

        public static Response<T> Success<T>(string message, T data = default)
        {
            return new Response<T>(ResultEnum.Success, message, data);
        }

        public static Response<T> Error<T>(string message, IEnumerable<string> errors)
        {
            return new Response<T>(ResultEnum.Error, message, errors, default);
        }


        public static Response<T> Validation<T>(string message, IEnumerable<string> errors)
        {
            return new Response<T>(ResultEnum.Validation, message, errors, default);
        }
    }
}
