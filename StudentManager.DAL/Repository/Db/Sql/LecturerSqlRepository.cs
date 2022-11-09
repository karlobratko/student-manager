using System.Data;
using System.Data.SqlClient;

using StudentManager.DAL.Base.Repository.Db.Sql;
using StudentManager.DAL.Base.Repository.Models;
using StudentManager.DAL.Extensions;
using StudentManager.DAL.Models;

namespace StudentManager.DAL.Repository.Db.Sql;

public sealed class LecturerSqlRepository : SqlDbRepositoryBase<int, Lecturer>, ILecturerRepository {
  public LecturerSqlRepository(string connectionString) : base(connectionString) {
  }

  public override Lecturer Model(SqlDataReader reader) =>
    new() {
      Id = reader.GetInt32(reader.GetOrdinal(nameof(Lecturer.Id))),
      Guid = reader.GetGuid(reader.GetOrdinal(nameof(Lecturer.Guid))),
      CreateDate = reader.GetDateTime(reader.GetOrdinal(nameof(Lecturer.CreateDate))),
      CreatedBy = reader.GetInt32(reader.GetOrdinal(nameof(Lecturer.CreatedBy))),
      UpdateDate = reader.GetDateTime(reader.GetOrdinal(nameof(Lecturer.UpdateDate))),
      UpdatedBy = reader.GetInt32(reader.GetOrdinal(nameof(Lecturer.UpdatedBy))),
      DeleteDate = !reader.IsDBNull(reader.GetOrdinal(nameof(Lecturer.DeleteDate)))
          ? reader.GetDateTime(reader.GetOrdinal(nameof(Lecturer.DeleteDate)))
          : null,
      DeletedBy = !reader.IsDBNull(reader.GetOrdinal(nameof(Lecturer.DeletedBy)))
          ? reader.GetInt32(reader.GetOrdinal(nameof(Lecturer.DeletedBy)))
          : null,
      FName = reader.GetString(reader.GetOrdinal(nameof(Lecturer.FName))),
      LName = reader.GetString(reader.GetOrdinal(nameof(Lecturer.LName))),
      BirthDate = reader.GetDateTime(reader.GetOrdinal(nameof(Lecturer.BirthDate))),
      Email = reader.GetString(reader.GetOrdinal(nameof(Lecturer.Email))),
      PhoneNumber = !reader.IsDBNull(reader.GetOrdinal(nameof(Lecturer.PhoneNumber)))
          ? reader.GetString(reader.GetOrdinal(nameof(Lecturer.PhoneNumber)))
          : null,
      Address = !reader.IsDBNull(reader.GetOrdinal(nameof(Lecturer.Address)))
          ? reader.GetString(reader.GetOrdinal(nameof(Lecturer.Address)))
          : null,
      Image = !reader.IsDBNull(reader.GetOrdinal(nameof(Lecturer.Image)))
          ? reader.GetVarBinary(reader.GetOrdinal(nameof(Lecturer.Image)))
          : null
    };

  public override IList<SqlParameter> Parameterize(Lecturer model) =>
    new List<SqlParameter> {
        new SqlParameter { ParameterName = $"@{nameof(Lecturer.FName)}", SqlDbType = TypeManager.GetSqlDbType(typeof(Lecturer).GetProperty(nameof(Lecturer.FName))!.PropertyType), Value = model.FName },
        new SqlParameter { ParameterName = $"@{nameof(Lecturer.LName)}", SqlDbType = TypeManager.GetSqlDbType(typeof(Lecturer).GetProperty(nameof(Lecturer.LName))!.PropertyType), Value = model.LName },
        new SqlParameter { ParameterName = $"@{nameof(Lecturer.BirthDate)}", SqlDbType = TypeManager.GetSqlDbType(typeof(Lecturer).GetProperty(nameof(Lecturer.BirthDate))!.PropertyType), Value = model.BirthDate },
        new SqlParameter { ParameterName = $"@{nameof(Lecturer.Email)}", SqlDbType = TypeManager.GetSqlDbType(typeof(Lecturer).GetProperty(nameof(Lecturer.Email))!.PropertyType), Value = model.Email },
        new SqlParameter { ParameterName = $"@{nameof(Lecturer.PhoneNumber)}", SqlDbType = TypeManager.GetSqlDbType(typeof(Lecturer).GetProperty(nameof(Lecturer.PhoneNumber))!.PropertyType), Value = model.PhoneNumber ?? (object)DBNull.Value },
        new SqlParameter { ParameterName = $"@{nameof(Lecturer.Address)}", SqlDbType = TypeManager.GetSqlDbType(typeof(Lecturer).GetProperty(nameof(Lecturer.Address))!.PropertyType), Value = model.Address ?? (object)DBNull.Value },
        new SqlParameter { ParameterName = $"@{nameof(Lecturer.Image)}", SqlDbType = TypeManager.GetSqlDbType(typeof(Lecturer).GetProperty(nameof(Lecturer.Image))!.PropertyType), Value = model.Image ?? (object)DBNull.Value },
    };

  public override string EntityName => $"{nameof(Lecturer)}";

  public IEnumerable<Lecturer> ReadByCourseFK(int courseFK) {
    using var sqlConnection = new SqlConnection(ConnectionString);
    SqlCommand sqlCommand = sqlConnection.CreateCommand();
    sqlCommand.CommandText = $"[dbo].[{EntityName}{nameof(ReadByCourseFK)}]";
    sqlCommand.CommandType = CommandType.StoredProcedure;
    _ = sqlCommand.Parameters
                  .Add(new SqlParameter {
                    ParameterName = $"@{nameof(Assistant.CourseFK)}",
                    Direction = ParameterDirection.Input,
                    SqlDbType = TypeManager.GetSqlDbType<int>(),
                    Value = courseFK
                  });

    sqlConnection.Open();
    SqlDataReader reader = sqlCommand.ExecuteReader();

    while (reader.Read())
      yield return Model(reader);
  }

  public async IAsyncEnumerable<Lecturer> ReadByCourseFKAsync(int courseFK) {
    using var sqlConnection = new SqlConnection(ConnectionString);
    SqlCommand sqlCommand = sqlConnection.CreateCommand();
    sqlCommand.CommandText = $"[dbo].[{EntityName}{nameof(ReadByCourseFK)}]";
    sqlCommand.CommandType = CommandType.StoredProcedure;
    _ = sqlCommand.Parameters
                  .Add(new SqlParameter {
                    ParameterName = $"@{nameof(Assistant.CourseFK)}",
                    Direction = ParameterDirection.Input,
                    SqlDbType = TypeManager.GetSqlDbType<int>(),
                    Value = courseFK
                  });

    await sqlConnection.OpenAsync();
    SqlDataReader reader = await sqlCommand.ExecuteReaderAsync();

    while (reader.Read())
      yield return Model(reader);
  }
}
