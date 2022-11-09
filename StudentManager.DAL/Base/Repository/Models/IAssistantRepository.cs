using StudentManager.DAL.Enums;
using StudentManager.DAL.Models;

namespace StudentManager.DAL.Base.Repository.Models;

public interface IAssistantRepository : IPersistableRepository<int, Assistant> {
  public DeleteStatus DeleteByCourseFKAndLecturerFK(int courseFK, int lecturerFK);
  public DeleteStatus DeleteByCourseFKAndLecturerFK(int courseFK, int lecturerFK, int? deletedBy);
  public Task<DeleteStatus> DeleteByCourseFKAndLecturerFKAsync(int courseFK, int lecturerFK);
  public Task<DeleteStatus> DeleteByCourseFKAndLecturerFKAsync(int courseFK, int lecturerFK, int? deletedBy);
}
