namespace ProductCatalogue.AuthenticationService.DTOs
{
    public class GenericResponse<T>
    {
        public string StatusCode { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public GenericResponse(string statusCode, string message, T data)
        {
            StatusCode = statusCode;
            Message = message;
            Data = data;
        }
    }

    public class GenericResponse
    {
        public string StatusCode { get; set; }
        public string Message { get; set; }

        public GenericResponse(string statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }
    }
}
