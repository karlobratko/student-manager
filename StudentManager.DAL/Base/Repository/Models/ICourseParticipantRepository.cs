using StudentManager.DAL.Enums;
using StudentManager.DAL.Models;

namespace StudentManager.DAL.Base.Repository.Models;

public interface ICourseParticipantRepository : IPersistableRepository<int, CourseParticipant> {
  public DeleteStatus DeleteByCourseFKAndStudentFK(int courseFK, int studentFK);
  public DeleteStatus DeleteByCourseFKAndStudentFK(int courseFK, int studentFK, int? deletedBy);
  public Task<DeleteStatus> DeleteByCourseFKAndStudentFKAsync(int courseFK, int studentFK);
  public Task<DeleteStatus> DeleteByCourseFKAndStudentFKAsync(int courseFK, int studentFK, int? deletedBy);
}
