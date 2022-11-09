using StudentManager.DAL.Base.Models;
using StudentManager.DAL.Enums;

namespace StudentManager.DAL.Base.Repository;

public interface IIdentifiableRepository<K, T> : ICRUDRepository<T>
  where T : class, IIdentifiable<K>, IEquatable<T>
  where K : struct {
  T? ReadById(K id);
  UpdateStatus Update(Guid guid, T model);
  DeleteStatus Delete(Guid guid);
  Task<T?> ReadByIdAsync(K id);
  Task<UpdateStatus> UpdateAsync(Guid guid, T model);
  Task<DeleteStatus> DeleteAsync(Guid guid);
}
