namespace iOrder.dataaccess
{
    public class Constants
    {
        public const string DbTrue = "1";
        public const string DbFalse = "0";

        public static readonly string ConnectionStringName = "iOrderConnectionString";

        public static string DefaultConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=iOrder;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
    }
}
