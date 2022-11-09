using System.Data;
using System.Data.SqlClient;

using StudentManager.DAL.Base.Repository.Db.Sql;
using StudentManager.DAL.Base.Repository.Models;
using StudentManager.DAL.Extensions;
using StudentManager.DAL.Models;

namespace StudentManager.DAL.Repository.Db.Sql;

public sealed class StudentSqlRepository : SqlDbRepositoryBase<int, Student>, IStudentRepository {
  public StudentSqlRepository(string connectionString) : base(connectionString) {
  }

  public override Student Model(SqlDataReader reader) =>
  new() {
    Id = reader.GetInt32(reader.GetOrdinal(nameof(Student.Id))),
    Guid = reader.GetGuid(reader.GetOrdinal(nameof(Student.Guid))),
    CreateDate = reader.GetDateTime(reader.GetOrdinal(nameof(Student.CreateDate))),
    CreatedBy = reader.GetInt32(reader.GetOrdinal(nameof(Student.CreatedBy))),
    UpdateDate = reader.GetDateTime(reader.GetOrdinal(nameof(Student.UpdateDate))),
    UpdatedBy = reader.GetInt32(reader.GetOrdinal(nameof(Student.UpdatedBy))),
    DeleteDate = !reader.IsDBNull(reader.GetOrdinal(nameof(Student.DeleteDate)))
        ? reader.GetDateTime(reader.GetOrdinal(nameof(Student.DeleteDate)))
        : null,
    DeletedBy = !reader.IsDBNull(reader.GetOrdinal(nameof(Student.DeletedBy)))
        ? reader.GetInt32(reader.GetOrdinal(nameof(Student.DeletedBy)))
        : null,
    FName = reader.GetString(reader.GetOrdinal(nameof(Student.FName))),
    LName = reader.GetString(reader.GetOrdinal(nameof(Student.LName))),
    BirthDate = reader.GetDateTime(reader.GetOrdinal(nameof(Student.BirthDate))),
    Email = reader.GetString(reader.GetOrdinal(nameof(Student.Email))),
    PhoneNumber = !reader.IsDBNull(reader.GetOrdinal(nameof(Student.PhoneNumber)))
        ? reader.GetString(reader.GetOrdinal(nameof(Student.PhoneNumber)))
        : null,
    Address = !reader.IsDBNull(reader.GetOrdinal(nameof(Student.Address)))
        ? reader.GetString(reader.GetOrdinal(nameof(Student.Address)))
        : null,
    Image = !reader.IsDBNull(reader.GetOrdinal(nameof(Student.Image)))
          ? reader.GetVarBinary(reader.GetOrdinal(nameof(Student.Image)))
          : null,
    JMBAG = reader.GetString(reader.GetOrdinal(nameof(Student.JMBAG))),
    YearOfStudy = reader.GetByte(reader.GetOrdinal(nameof(Student.YearOfStudy))),
  };

  public override IList<SqlParameter> Parameterize(Student model) =>
    new List<SqlParameter> {
        new SqlParameter { ParameterName = $"@{nameof(Student.FName)}", SqlDbType = TypeManager.GetSqlDbType(typeof(Student).GetProperty(nameof(Student.FName))!.PropertyType), Value = model.FName },
        new SqlParameter { ParameterName = $"@{nameof(Student.LName)}", SqlDbType = TypeManager.GetSqlDbType(typeof(Student).GetProperty(nameof(Student.LName))!.PropertyType), Value = model.LName },
        new SqlParameter { ParameterName = $"@{nameof(Student.BirthDate)}", SqlDbType = TypeManager.GetSqlDbType(typeof(Student).GetProperty(nameof(Student.BirthDate))!.PropertyType), Value = model.BirthDate },
        new SqlParameter { ParameterName = $"@{nameof(Student.Email)}", SqlDbType = TypeManager.GetSqlDbType(typeof(Student).GetProperty(nameof(Student.Email))!.PropertyType), Value = model.Email },
        new SqlParameter { ParameterName = $"@{nameof(Student.PhoneNumber)}", SqlDbType = TypeManager.GetSqlDbType(typeof(Student).GetProperty(nameof(Student.PhoneNumber))!.PropertyType), Value = model.PhoneNumber ?? (object)DBNull.Value },
        new SqlParameter { ParameterName = $"@{nameof(Student.Address)}", SqlDbType = TypeManager.GetSqlDbType(typeof(Student).GetProperty(nameof(Student.Address))!.PropertyType), Value = model.Address ?? (object)DBNull.Value },
        new SqlParameter { ParameterName = $"@{nameof(Student.Image)}", SqlDbType = TypeManager.GetSqlDbType(typeof(Student).GetProperty(nameof(Student.Image))!.PropertyType), Value = model.Image ?? (object)DBNull.Value },
        new SqlParameter { ParameterName = $"@{nameof(Student.JMBAG)}", SqlDbType = TypeManager.GetSqlDbType(typeof(Student).GetProperty(nameof(Student.JMBAG))!.PropertyType), Value = model.JMBAG },
        new SqlParameter { ParameterName = $"@{nameof(Student.YearOfStudy)}", SqlDbType = TypeManager.GetSqlDbType(typeof(Student).GetProperty(nameof(Student.YearOfStudy))!.PropertyType), Value = model.YearOfStudy },
    };

  public override string EntityName => $"{nameof(Student)}";

  public IEnumerable<Student> ReadByCourseFK(int courseFK) {
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

  public async IAsyncEnumerable<Student> ReadByCourseFKAsync(int courseFK) {
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
