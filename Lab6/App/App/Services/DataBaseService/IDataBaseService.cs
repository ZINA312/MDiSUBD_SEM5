namespace App.Services.DataBaseService
{
    public interface IDataBaseService
    {
        IDataBaseService AddParameter<T>(string parameterName, T value);
        int ExecuteNonQuery(string query, bool isStoredProc = false);
        IEnumerable<T> ExecuteQuery<T>(string query);
        T ExecuteScalar<T>(string query, bool isStoredProc = false);
       
    }
}
