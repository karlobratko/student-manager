using System.Data;
using System.Data.SqlClient;
using System.Reflection;

using StudentManager.DAL.Base.Models;
using StudentManager.DAL.Enums;

namespace StudentManager.DAL.Base.Repository.Db.Sql;

public abstract class SqlDbRepositoryBase<K, T> : IPersistableRepository<K, T>, IDbRepository
  where T : class, IPersistable<K>, IEquatable<T>
  where K : struct {

  protected sealed class SqlDbTypeManager {
    private readonly Dictionary<Type, SqlDbType> _typeDictionary =
      new() {
        { typeof(string),         SqlDbType.NVarChar },
        { typeof(char[]),         SqlDbType.NVarChar },
        { typeof(byte),           SqlDbType.TinyInt },
        { typeof(short),          SqlDbType.SmallInt },
        { typeof(int),            SqlDbType.Int },
        { typeof(int?),           SqlDbType.Int },
        { typeof(long),           SqlDbType.BigInt },
        { typeof(long?),          SqlDbType.BigInt },
        { typeof(byte[]),         SqlDbType.VarBinary },
        { typeof(bool),           SqlDbType.Bit },
        { typeof(bool?),          SqlDbType.Bit },
        { typeof(DateTime),       SqlDbType.DateTime },
        { typeof(DateTime?),      SqlDbType.DateTime },
        { typeof(DateTimeOffset), SqlDbType.DateTimeOffset },
        { typeof(decimal),        SqlDbType.Decimal },
        { typeof(decimal?),       SqlDbType.Decimal },
        { typeof(float),          SqlDbType.Real },
        { typeof(double),         SqlDbType.Float },
        { typeof(TimeSpan),       SqlDbType.Time },
        { typeof(Guid),           SqlDbType.UniqueIdentifier }
      };

    public SqlDbType GetSqlDbType(Type type) => _typeDictionary[type];
    public SqlDbType GetSqlDbType<Type>() => GetSqlDbType(typeof(Type));
  }

  private const string CREATE_PROCEDURE_NAME = "Create";
  private const string READ_PROCEDURE_NAME = "Read";
  private const string UPDATE_PROCEDURE_NAME = "Update";
  private const string DELETE_PROCEDURE_NAME = "Delete";

  protected SqlDbTypeManager TypeManager { get; } = new();

  public string ConnectionString { get; init; }

  public abstract string EntityName { get; }

  public SqlDbRepositoryBase(string connectionString) => ConnectionString = connectionString;

  public virtual T Model(SqlDataReader reader) =>
    typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public)
             .Aggregate(seed: Activator.CreateInstance<T>(),
                        func: (obj, property) => {
                          obj.GetType()
                             .GetProperty(property.Name)
                             ?.SetValue(obj: obj,
                                        value: !reader.IsDBNull(reader.GetOrdinal(property.Name))
                                          ? reader.GetValue(reader.GetOrdinal(property.Name))
                                          : default);
                          return obj;
                        });

  public virtual IList<SqlParameter> Parameterize(T model) =>
    typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
             .Select(selector: property => new SqlParameter {
               ParameterName = $"@{property.Name}",
               SqlDbType = Nullable.GetUnderlyingType(property.PropertyType) is not null
                  ? TypeManager.GetSqlDbType(Nullable.GetUnderlyingType(property.PropertyType)!)
                  : TypeManager.GetSqlDbType(property.PropertyType),
               Value = Nullable.GetUnderlyingType(property.PropertyType) is not null || !property.PropertyType.IsValueType
                  ? property.GetValue(model) ?? DBNull.Value
                  : property.GetValue(model)
             })
             .ToList();

  public async Task<CreateStatus> CreateAsync(T model) => await CreateAsync(model, null);
  public async Task<CreateStatus> CreateAsync(T model, K? createdBy) {
    IList<SqlParameter> parameters = Parameterize(model);

    parameters.Add(new SqlParameter {
      ParameterName = $"@{nameof(IPersistable<K>.CreatedBy)}",
      Direction = ParameterDirection.Input,
      SqlDbType = TypeManager.GetSqlDbType<K>(),
      Value = createdBy ?? (object)DBNull.Value
    });

    var returnParam = new SqlParameter {
      Direction = ParameterDirection.ReturnValue
    };
    parameters.Add(returnParam);

    using var sqlConnection = new SqlConnection(ConnectionString);

    SqlCommand sqlCommand = sqlConnection.CreateCommand();
    sqlCommand.CommandText = $"[dbo].[{EntityName}{CREATE_PROCEDURE_NAME}]";
    sqlCommand.CommandType = CommandType.StoredProcedure;
    sqlCommand.Parameters.AddRange(parameters.ToArray());

    await sqlConnection.OpenAsync();

    SqlDataReader reader = await sqlCommand.ExecuteReaderAsync();
    if (reader.Read())
      typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
               .ToList()
               .ForEach(property =>
                 model.GetType()
                      .GetProperty(property.Name)
                      ?.SetValue(obj: model,
                                 value: !reader.IsDBNull(reader.GetOrdinal(property.Name))
                                   ? reader.GetValue(reader.GetOrdinal(property.Name))
                                   : default));

    await sqlConnection.CloseAsync();

    return Enum.TryParse(returnParam.Value?.ToString(), out CreateStatus status)
      ? status
      : CreateStatus.InternalError;
  }

  public CreateStatus Create(T model) => Create(model, null);
  public CreateStatus Create(T model, K? createdBy) {
    IList<SqlParameter> parameters = Parameterize(model);

    parameters.Add(new SqlParameter {
      ParameterName = $"@{nameof(IPersistable<K>.CreatedBy)}",
      Direction = ParameterDirection.Input,
      SqlDbType = TypeManager.GetSqlDbType<K>(),
      Value = createdBy ?? (object)DBNull.Value
    });

    var returnParam = new SqlParameter {
      Direction = ParameterDirection.ReturnValue,
      SqlDbType = SqlDbType.Int
    };
    parameters.Add(returnParam);

    using var sqlConnection = new SqlConnection(ConnectionString);

    SqlCommand sqlCommand = sqlConnection.CreateCommand();
    sqlCommand.CommandText = $"[dbo].[{EntityName}{CREATE_PROCEDURE_NAME}]";
    sqlCommand.CommandType = CommandType.StoredProcedure;
    sqlCommand.Parameters.AddRange(parameters.ToArray());

    sqlConnection.Open();

    SqlDataReader reader = sqlCommand.ExecuteReader();
    if (reader.Read())
      typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
               .ToList()
               .ForEach(property =>
                 model.GetType()
                      .GetProperty(property.Name)
                      ?.SetValue(obj: model,
                                 value: !reader.IsDBNull(reader.GetOrdinal(property.Name))
                                   ? reader.GetValue(reader.GetOrdinal(property.Name))
                                   : default));

    sqlConnection.Close();

    return Enum.TryParse(returnParam.Value?.ToString(), out CreateStatus status)
      ? status
      : CreateStatus.InternalError;
  }

  public async Task<DeleteStatus> DeleteAsync(T model) => await DeleteAsync(model.Guid);
  public async Task<DeleteStatus> DeleteAsync(Guid guid) => await DeleteAsync(guid, null);
  public async Task<DeleteStatus> DeleteAsync(Guid guid, K? deletedBy) {
    IList<SqlParameter> parameters = new List<SqlParameter> {
      new SqlParameter {
        ParameterName = $"@{nameof(IPersistable<K>.Guid)}",
        Direction = ParameterDirection.Input,
        SqlDbType = SqlDbType.UniqueIdentifier,
        Value = guid,
      },
      new SqlParameter() {
        ParameterName = $"@{nameof(IPersistable<K>.DeletedBy)}",
        Direction = ParameterDirection.Input,
        SqlDbType = TypeManager.GetSqlDbType<K>(),
        Value = deletedBy ?? (object)DBNull.Value
      }
    };

    var returnParam = new SqlParameter() {
      Direction = ParameterDirection.ReturnValue
    };
    parameters.Add(returnParam);

    using var sqlConnection = new SqlConnection(ConnectionString);
    SqlCommand sqlCommand = sqlConnection.CreateCommand();
    sqlCommand.CommandText = $"[dbo].[{EntityName}{DELETE_PROCEDURE_NAME}]";
    sqlCommand.CommandType = CommandType.StoredProcedure;
    sqlCommand.Parameters.AddRange(parameters.ToArray());

    await sqlConnection.OpenAsync();
    _ = await sqlCommand.ExecuteNonQueryAsync();
    await sqlConnection.CloseAsync();

    return Enum.TryParse(returnParam.Value?.ToString(), out DeleteStatus status)
      ? status
      : DeleteStatus.InternalError;
  }

  public DeleteStatus Delete(T model) => Delete(model.Guid);
  public DeleteStatus Delete(Guid guid) => Delete(guid, null);
  public DeleteStatus Delete(Guid guid, K? deletedBy) {
    IList<SqlParameter> parameters = new List<SqlParameter> {
      new SqlParameter {
        ParameterName = $"@{nameof(IPersistable<K>.Guid)}",
        Direction = ParameterDirection.Input,
        SqlDbType = SqlDbType.UniqueIdentifier,
        Value = guid,
      },
      new SqlParameter() {
        ParameterName = $"@{nameof(IPersistable<K>.DeletedBy)}",
        Direction = ParameterDirection.Input,
        SqlDbType = TypeManager.GetSqlDbType<K>(),
        Value = deletedBy ?? (object)DBNull.Value
      }
    };

    var returnParam = new SqlParameter() {
      Direction = ParameterDirection.ReturnValue
    };
    parameters.Add(returnParam);

    using var sqlConnection = new SqlConnection(ConnectionString);
    SqlCommand sqlCommand = sqlConnection.CreateCommand();
    sqlCommand.CommandText = $"[dbo].[{EntityName}{DELETE_PROCEDURE_NAME}]";
    sqlCommand.CommandType = CommandType.StoredProcedure;
    sqlCommand.Parameters.AddRange(parameters.ToArray());

    sqlConnection.Open();
    _ = sqlCommand.ExecuteNonQuery();
    sqlConnection.Close();

    return Enum.TryParse(returnParam.Value?.ToString(), out DeleteStatus status)
      ? status
      : DeleteStatus.InternalError;
  }

  public async IAsyncEnumerable<T> ReadAllAsync() {
    using var sqlConnection = new SqlConnection(ConnectionString);
    SqlCommand sqlCommand = sqlConnection.CreateCommand();
    sqlCommand.CommandText = $"[dbo].[{EntityName}{READ_PROCEDURE_NAME}]";
    sqlCommand.CommandType = CommandType.StoredProcedure;

    await sqlConnection.OpenAsync();
    SqlDataReader reader = await sqlCommand.ExecuteReaderAsync();

    while (reader.Read())
      yield return Model(reader);
  }

  public IEnumerable<T> ReadAll() {
    using var sqlConnection = new SqlConnection(ConnectionString);
    SqlCommand sqlCommand = sqlConnection.CreateCommand();
    sqlCommand.CommandText = $"[dbo].[{EntityName}{READ_PROCEDURE_NAME}]";
    sqlCommand.CommandType = CommandType.StoredProcedure;

    sqlConnection.Open();
    SqlDataReader reader = sqlCommand.ExecuteReader();

    while (reader.Read())
      yield return Model(reader);
  }

  public async Task<T?> ReadByIdAsync(K id) {
    using var sqlConnection = new SqlConnection(ConnectionString);
    SqlCommand sqlCommand = sqlConnection.CreateCommand();
    sqlCommand.CommandText = $"[dbo].[{EntityName}{READ_PROCEDURE_NAME}]";
    sqlCommand.CommandType = CommandType.StoredProcedure;
    _ = sqlCommand.Parameters
                  .Add(new SqlParameter {
                    ParameterName = $"@{nameof(IPersistable<K>.Id)}",
                    Direction = ParameterDirection.Input,
                    SqlDbType = TypeManager.GetSqlDbType(typeof(K)),
                    Value = id
                  });

    await sqlConnection.OpenAsync();
    SqlDataReader reader = await sqlCommand.ExecuteReaderAsync();

    return reader.Read() ? Model(reader) : null;
  }

  public T? ReadById(K id) {
    using var sqlConnection = new SqlConnection(ConnectionString);
    SqlCommand sqlCommand = sqlConnection.CreateCommand();
    sqlCommand.CommandText = $"[dbo].[{EntityName}{READ_PROCEDURE_NAME}]";
    sqlCommand.CommandType = CommandType.StoredProcedure;
    _ = sqlCommand.Parameters
                  .Add(new SqlParameter {
                    ParameterName = $"@{nameof(IPersistable<K>.Id)}",
                    Direction = ParameterDirection.Input,
                    SqlDbType = TypeManager.GetSqlDbType(typeof(K)),
                    Value = id
                  });

    sqlConnection.Open();
    SqlDataReader reader = sqlCommand.ExecuteReader();

    return reader.Read() ? Model(reader) : null;
  }

  public async Task<Enums.UpdateStatus> UpdateAsync(T model) => await UpdateAsync(model.Guid, model);
  public async Task<Enums.UpdateStatus> UpdateAsync(Guid guid, T model) => await UpdateAsync(guid, model, null);
  public async Task<Enums.UpdateStatus> UpdateAsync(Guid guid, T model, K? updatedBy) {
    IList<SqlParameter> parameters = Parameterize(model);

    parameters.Add(item: new SqlParameter {
      ParameterName = $"@{nameof(IPersistable<K>.Guid)}",
      Direction = ParameterDirection.Input,
      SqlDbType = SqlDbType.UniqueIdentifier,
      Value = guid,
    });

    parameters.Add(item: new SqlParameter {
      ParameterName = $"@{nameof(IPersistable<K>.UpdatedBy)}",
      Direction = ParameterDirection.Input,
      SqlDbType = TypeManager.GetSqlDbType<K>(),
      Value = updatedBy ?? (object)DBNull.Value
    });

    var returnParam = new SqlParameter {
      Direction = ParameterDirection.ReturnValue
    };
    parameters.Add(returnParam);

    using var sqlConnection = new SqlConnection(ConnectionString);
    SqlCommand sqlCommand = sqlConnection.CreateCommand();
    sqlCommand.CommandText = $"[dbo].[{EntityName}{UPDATE_PROCEDURE_NAME}]";
    sqlCommand.CommandType = CommandType.StoredProcedure;
    sqlCommand.Parameters.AddRange(parameters.ToArray());

    await sqlConnection.OpenAsync();
    _ = await sqlCommand.ExecuteNonQueryAsync();
    await sqlConnection.CloseAsync();

    return Enum.TryParse(returnParam.Value?.ToString(), out Enums.UpdateStatus status)
      ? status
      : Enums.UpdateStatus.InternalError;
  }

  public Enums.UpdateStatus Update(T model) => Update(model.Guid, model);
  public Enums.UpdateStatus Update(Guid guid, T model) => Update(guid, model, null);
  public Enums.UpdateStatus Update(Guid guid, T model, K? updatedBy) {
    IList<SqlParameter> parameters = Parameterize(model);

    parameters.Add(item: new SqlParameter {
      ParameterName = $"@{nameof(IPersistable<K>.Guid)}",
      Direction = ParameterDirection.Input,
      SqlDbType = SqlDbType.UniqueIdentifier,
      Value = guid,
    });

    parameters.Add(item: new SqlParameter {
      ParameterName = $"@{nameof(IPersistable<K>.UpdatedBy)}",
      Direction = ParameterDirection.Input,
      SqlDbType = TypeManager.GetSqlDbType<K>(),
      Value = updatedBy ?? (object)DBNull.Value
    });

    var returnParam = new SqlParameter {
      Direction = ParameterDirection.ReturnValue
    };
    parameters.Add(returnParam);

    using var sqlConnection = new SqlConnection(ConnectionString);
    SqlCommand sqlCommand = sqlConnection.CreateCommand();
    sqlCommand.CommandText = $"[dbo].[{EntityName}{UPDATE_PROCEDURE_NAME}]";
    sqlCommand.CommandType = CommandType.StoredProcedure;
    sqlCommand.Parameters.AddRange(parameters.ToArray());

    sqlConnection.Open();
    _ = sqlCommand.ExecuteNonQuery();
    sqlConnection.Close();

    return Enum.TryParse(returnParam.Value?.ToString(), out Enums.UpdateStatus status)
      ? status
      : Enums.UpdateStatus.InternalError;
  }
}
