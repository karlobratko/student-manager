using StudentManager.DAL.Models;

namespace StudentManager.DAL.Base.Repository.Models;

public interface IStudentRepository : IPersistableRepository<int, Student> {
  public IEnumerable<Student> ReadByCourseFK(int courseFK);
  public IAsyncEnumerable<Student> ReadByCourseFKAsync(int courseFK);
}
