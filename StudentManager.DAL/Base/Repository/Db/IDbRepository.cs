namespace StudentManager.DAL.Base.Repository.Db;

public interface IDbRepository {
  string ConnectionString { get; init; }
  string EntityName { get; }
}
