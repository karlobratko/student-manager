using StudentManager.DAL.Base.Repository.Db.Sql;
using StudentManager.DAL.Base.Repository.Models;
using StudentManager.DAL.Models;

namespace StudentManager.DAL.Repository.Db.Sql;

public sealed class CourseSqlRepository : SqlDbRepositoryBase<int, Course>, ICourseRepository {
  public CourseSqlRepository(string connectionString) : base(connectionString) {
  }

  public override string EntityName => $"{nameof(Course)}";
}
