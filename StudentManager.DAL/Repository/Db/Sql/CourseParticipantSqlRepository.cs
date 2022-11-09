using StudentManager.DAL.Base.Models;
using System.Data.SqlClient;
using System.Data;

using StudentManager.DAL.Base.Repository.Db.Sql;
using StudentManager.DAL.Base.Repository.Models;
using StudentManager.DAL.Enums;
using StudentManager.DAL.Models;

namespace StudentManager.DAL.Repository.Db.Sql;

public sealed class CourseParticipantSqlRepository : SqlDbRepositoryBase<int, CourseParticipant>, ICourseParticipantRepository {
  public CourseParticipantSqlRepository(string connectionString) : base(connectionString) {
  }

  public override string EntityName => $"{nameof(CourseParticipant)}";

  public DeleteStatus DeleteByCourseFKAndStudentFK(int courseFK, int studentFK) => DeleteByCourseFKAndStudentFK(courseFK, studentFK, null);
  public DeleteStatus DeleteByCourseFKAndStudentFK(int courseFK, int studentFK, int? deletedBy) {
    IList<SqlParameter> parameters = new List<SqlParameter> {
      new SqlParameter {
        ParameterName = $"@{nameof(CourseParticipant.CourseFK)}",
        Direction = ParameterDirection.Input,
        SqlDbType = TypeManager.GetSqlDbType<int>(),
        Value = courseFK,
      },
      new SqlParameter {
        ParameterName = $"@{nameof(CourseParticipant.StudentFK)}",
        Direction = ParameterDirection.Input,
        SqlDbType = TypeManager.GetSqlDbType<int>(),
        Value = studentFK,
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
    sqlCommand.CommandText = $"[dbo].[{EntityName}{nameof(DeleteByCourseFKAndStudentFK)}]";
    sqlCommand.CommandType = CommandType.StoredProcedure;
    sqlCommand.Parameters.AddRange(parameters.ToArray());

    sqlConnection.Open();
    _ = sqlCommand.ExecuteNonQuery();
    sqlConnection.Close();

    return Enum.TryParse(returnParam.Value?.ToString(), out DeleteStatus status)
      ? status
      : DeleteStatus.InternalError;
  }

  public async Task<DeleteStatus> DeleteByCourseFKAndStudentFKAsync(int courseFK, int studentFK) => await DeleteByCourseFKAndStudentFKAsync(courseFK, studentFK, null);
  public async Task<DeleteStatus> DeleteByCourseFKAndStudentFKAsync(int courseFK, int studentFK, int? deletedBy) {
    IList<SqlParameter> parameters = new List<SqlParameter> {
      new SqlParameter {
        ParameterName = $"@{nameof(CourseParticipant.CourseFK)}",
        Direction = ParameterDirection.Input,
        SqlDbType = TypeManager.GetSqlDbType<int>(),
        Value = courseFK,
      },
      new SqlParameter {
        ParameterName = $"@{nameof(CourseParticipant.StudentFK)}",
        Direction = ParameterDirection.Input,
        SqlDbType = TypeManager.GetSqlDbType<int>(),
        Value = studentFK,
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
    sqlCommand.CommandText = $"[dbo].[{EntityName}{nameof(DeleteByCourseFKAndStudentFK)}]";
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

