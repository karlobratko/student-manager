using StudentManager.DAL.Base.Models;
using System.Data.SqlClient;
using System.Data;
using System;

using StudentManager.DAL.Base.Repository.Db.Sql;
using StudentManager.DAL.Base.Repository.Models;
using StudentManager.DAL.Enums;
using StudentManager.DAL.Models;

namespace StudentManager.DAL.Repository.Db.Sql;

public sealed class AssistantSqlRepository : SqlDbRepositoryBase<int, Assistant>, IAssistantRepository {
  public AssistantSqlRepository(string connectionString) : base(connectionString) {
  }

  public override string EntityName => $"{nameof(Assistant)}";

  public DeleteStatus DeleteByCourseFKAndLecturerFK(int courseFK, int lecturerFK) => DeleteByCourseFKAndLecturerFK(courseFK, lecturerFK, null);
  public DeleteStatus DeleteByCourseFKAndLecturerFK(int courseFK, int lecturerFK, int? deletedBy) {
    IList<SqlParameter> parameters = new List<SqlParameter> {
      new SqlParameter {
        ParameterName = $"@{nameof(Assistant.CourseFK)}",
        Direction = ParameterDirection.Input,
        SqlDbType = TypeManager.GetSqlDbType<int>(),
        Value = courseFK,
      },
      new SqlParameter {
        ParameterName = $"@{nameof(Assistant.LecturerFK)}",
        Direction = ParameterDirection.Input,
        SqlDbType = TypeManager.GetSqlDbType<int>(),
        Value = lecturerFK,
      },
      new SqlParameter() {
        ParameterName = $"@{nameof(IPersistable<int>.DeletedBy)}",
        Direction = ParameterDirection.Input,
        SqlDbType = TypeManager.GetSqlDbType<int>(),
        Value = deletedBy ?? (object)DBNull.Value
      }
    };

    var returnParam = new SqlParameter() {
      Direction = ParameterDirection.ReturnValue
    };
    parameters.Add(returnParam);

    using var sqlConnection = new SqlConnection(ConnectionString);
    SqlCommand sqlCommand = sqlConnection.CreateCommand();
    sqlCommand.CommandText = $"[dbo].[{EntityName}{nameof(DeleteByCourseFKAndLecturerFK)}]";
    sqlCommand.CommandType = CommandType.StoredProcedure;
    sqlCommand.Parameters.AddRange(parameters.ToArray());

    sqlConnection.Open();
    _ = sqlCommand.ExecuteNonQuery();
    sqlConnection.Close();

    return Enum.TryParse(returnParam.Value?.ToString(), out DeleteStatus status)
      ? status
      : DeleteStatus.InternalError;
  }

  public async Task<DeleteStatus> DeleteByCourseFKAndLecturerFKAsync(int courseFK, int lecturerFK) => await DeleteByCourseFKAndLecturerFKAsync(courseFK, lecturerFK, null);
  public async Task<DeleteStatus> DeleteByCourseFKAndLecturerFKAsync(int courseFK, int lecturerFK, int? deletedBy) {
    IList<SqlParameter> parameters = new List<SqlParameter> {
      new SqlParameter {
        ParameterName = $"@{nameof(Assistant.CourseFK)}",
        Direction = ParameterDirection.Input,
        SqlDbType = TypeManager.GetSqlDbType<int>(),
        Value = courseFK,
      },
      new SqlParameter {
        ParameterName = $"@{nameof(Assistant.LecturerFK)}",
        Direction = ParameterDirection.Input,
        SqlDbType = TypeManager.GetSqlDbType<int>(),
        Value = lecturerFK,
      },
      new SqlParameter() {
        ParameterName = $"@{nameof(IPersistable<int>.DeletedBy)}",
        Direction = ParameterDirection.Input,
        SqlDbType = TypeManager.GetSqlDbType<int>(),
        Value = deletedBy ?? (object)DBNull.Value
      }
    };

    var returnParam = new SqlParameter() {
      Direction = ParameterDirection.ReturnValue
    };
    parameters.Add(returnParam);

    using var sqlConnection = new SqlConnection(ConnectionString);
    SqlCommand sqlCommand = sqlConnection.CreateCommand();
    sqlCommand.CommandText = $"[dbo].[{EntityName}{nameof(DeleteByCourseFKAndLecturerFK)}]";
    sqlCommand.CommandType = CommandType.StoredProcedure;
    sqlCommand.Parameters.AddRange(parameters.ToArray());

    await sqlConnection.OpenAsync();
    _ = await sqlCommand.ExecuteNonQueryAsync();
    await sqlConnection.CloseAsync();

    return Enum.TryParse(returnParam.Value?.ToString(), out DeleteStatus status)
      ? status
      : DeleteStatus.InternalError;
  }
}
