using StudentManager.DAL.Base.Models;
using StudentManager.DAL.Enums;

namespace StudentManager.DAL.Base.Repository;

public interface IPersistableRepository<K, T> : IIdentifiableRepository<K, T>
  where T : class, IPersistable<K>, IEquatable<T>
  where K : struct {
  CreateStatus Create(T model, K? createdBy);
  UpdateStatus Update(Guid guid, T model, K? updatedBy);
  DeleteStatus Delete(Guid guid, K? deletedBy);
  Task<CreateStatus> CreateAsync(T model, K? createdBy);
  Task<UpdateStatus> UpdateAsync(Guid guid, T model, K? updatedBy);
  Task<DeleteStatus> DeleteAsync(Guid guid, K? deletedBy);
}
