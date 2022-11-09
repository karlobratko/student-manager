using StudentManager.DAL.Enums;

namespace StudentManager.DAL.Base.Repository;

public interface ICRUDRepository<T> : IReadOnlyRepository<T>
  where T : class, IEquatable<T> {
  CreateStatus Create(T model);
  UpdateStatus Update(T model);
  DeleteStatus Delete(T model);
  Task<CreateStatus> CreateAsync(T model);
  Task<UpdateStatus> UpdateAsync(T model);
  Task<DeleteStatus> DeleteAsync(T model);
}
