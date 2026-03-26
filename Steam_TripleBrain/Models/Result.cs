namespace Steam_TripleBrain.Models
{
    public class Result<T>
    {
        public bool IsSuccess { get; set; }
        public T? Data { get; set; }
        public string? ErrorMessage { get; set; }
        //If success it would return object Result with default or rewrited parameters which then returning from method to user 
        public static Result<T> Success(T data, string message = "Success")
        {
            return new Result<T> { IsSuccess = true, Data = data, ErrorMessage = message };
        }

        //If Failed and returning object result with parameters.
        public static Result<T> Failure(string errorMessage)
        {
            return new Result<T> { IsSuccess = false, ErrorMessage = errorMessage, Data = default };
        }
    }
}
