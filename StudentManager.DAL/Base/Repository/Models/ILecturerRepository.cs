using StudentManager.DAL.Models;

namespace StudentManager.DAL.Base.Repository.Models;

public interface ILecturerRepository : IPersistableRepository<int, Lecturer> {
  public IEnumerable<Lecturer> ReadByCourseFK(int courseFK);
  public IAsyncEnumerable<Lecturer> ReadByCourseFKAsync(int courseFK);
}
