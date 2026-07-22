namespace Application.Wrappers
{
    public class Response<T>
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; } = null!;
        public List<string> Errors { get; set; } = null!;
        public T Data { get; set; } = default!;

        public Response()
        {
            
        }

        public Response(T data, string message = null!)
        {
            this.Succeeded = true;
            this.Message = message;
            this.Data = data;
        }

        public Response(string message)
        {
            this.Succeeded = false;
            this.Message = message;
        }

        /// <summary>
        /// Creates a failed response with a message.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static Response<T> Fail(string message)
        {
            return new Response<T>
            {
                Succeeded = false,
                Message = message
            };
        }

        /// <summary>
        /// Creates a failed response with a list of errors.
        /// </summary>
        /// <param name="errors"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static Response<T> Fail(List<string> errors, string message = null!)
        {
            return new Response<T>
            {
                Succeeded = false,
                Message = message,
                Errors = errors
            };
        }

        /// <summary>
        /// Creates a successful response with data and an optional message.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static Response<T> Success(T data, string message = null!)
        {
            return new Response<T>
            {
                Succeeded = true,
                Message = message,
                Data = data
            };
        }
    }
}
