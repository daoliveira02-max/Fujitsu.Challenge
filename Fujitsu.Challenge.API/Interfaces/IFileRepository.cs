namespace Fujitsu.Challenge.API.Interfaces
{
    public interface IFileRepository<T>
    {
        List<T> GetAll();
        void SaveAll(List<T> items);
    }
}
