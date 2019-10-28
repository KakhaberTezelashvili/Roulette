namespace Roulette.Infrastructure.DBProvider
{
    public interface IDBProvider
    {
        DataBase GetDBInstance();
    }

    public class DBProvider : IDBProvider
    {
        public DataBase GetDBInstance() => new DataBase();
    }
}
