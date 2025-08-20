namespace Fujitsu.Challenge.API.Interfaces
{
    public interface IResponse<T>
    {
        T Content { get; }

        bool IsSuccess { get; }

        string FailureReason { get; }
    }
}
