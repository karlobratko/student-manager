namespace StudentManager.DAL.Base.Repository;

public interface IReadOnlyRepository<T>
  where T : class {
  IEnumerable<T> ReadAll();
  IAsyncEnumerable<T> ReadAllAsync();
}
