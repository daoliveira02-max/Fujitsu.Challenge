using Fujitsu.Challenge.API.Interfaces;

namespace Fujitsu.Challenge.API
{
    public class Response<T> : IResponse<T>
    {
        public T Content { get; private set; }

        public bool IsSuccess { get; private set; }

        public string FailureReason { get; private set; }

        public static Response<T> Success(T content)
        {
            return new Response<T> { Content = content, IsSuccess = true };
        }

        public static Response<T> Failure(string reason)
        {
            return new Response<T> { IsSuccess = false, FailureReason = reason };
        }
    }
}
