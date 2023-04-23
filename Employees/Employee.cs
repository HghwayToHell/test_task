using System.Data.Common;
using System.Reflection.PortableExecutable;

namespace Employees
{
    public class Employee
    {

        public void CreateTable(DbConnection con)
        {
            using var cmd = con.CreateCommand();

            cmd.CommandText = "DROP TABLE IF EXISTS Employee";
            cmd.ExecuteNonQuery();

            cmd.CommandText = @"CREATE TABLE Employee(  Id INTEGER PRIMARY KEY, 
                                                        Department_Id INTEGER, 
                                                        Chief_Id INTEGER, 
                                                        Name STRING, 
                                                        Salary INTEGER)";
            cmd.ExecuteNonQuery();

            Console.WriteLine("Table \"Employee\" created");
        }

        public void CreateEmployee(DbConnection con, int departmentId, int? chiefId, string name, int salary)
        {
            using var cmd = con.CreateCommand();

            cmd.CommandText = $"INSERT INTO Employee(Department_Id, Chief_Id, Name, Salary) " +
                $"VALUES({departmentId},{chiefId?.ToString() ?? "null"},'{name}',{salary})";
            cmd.ExecuteNonQuery();
        }

        public static void CreateChainLengthCache(DbConnection con)
        {
            using var cmd = con.CreateCommand();

            cmd.CommandText = "DROP TABLE IF EXISTS ChainLengthCache";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "CREATE TABLE ChainLengthCache (" +
                              "Id INTEGER PRIMARY KEY," +
                              "ChainLength INTEGER)";
            cmd.ExecuteNonQuery();
        }

        public static void DestroyChainLengthCache(DbConnection con)
        {
            using var cmd = con.CreateCommand();

            cmd.CommandText = $"DROP TABLE ChainLengthCache";
            cmd.ExecuteNonQuery();
        }

        public static int ChainLengthCached(DbConnection con, int employeeId)
        {
            using var cmd = con.CreateCommand();
            cmd.CommandText = $"SELECT ChainLength FROM ChainLengthCache WHERE Id={employeeId}";
            var result = cmd.ExecuteScalar();

            if (result == null)
            {
                var cachedResult = ChainLength(con, employeeId);
                cmd.CommandText = $"INSERT INTO ChainLengthCache (Id, ChainLength) VALUES ({employeeId},{cachedResult})";
                return cachedResult;
            }

            return (int)(Int64)result;
        }

        public static int ChainLength(DbConnection con, int employeeId)
        {
            using var cmd = con.CreateCommand();

            cmd.CommandText = $"SELECT Chief_Id FROM Employee WHERE Id={employeeId}";
            var chiefId = cmd.ExecuteScalar();

            if (chiefId == null || chiefId.GetType() == typeof(DBNull))
            {
                return 1;
            }

            return ChainLengthCached(con, (int)(Int64)chiefId) + 1;
        }

        public void CreateTestEmployeeTable(DbConnection con)
        {
            CreateTable(con);

            CreateEmployee(con, 1, null, "Overlord", 10000);
            CreateEmployee(con, 2, 1, "John", 100);
            CreateEmployee(con, 3, 2, "Till", 70);
            CreateEmployee(con, 1, 1, "Billy", 150);
            CreateEmployee(con, 2, 1, "Edward", 200);
            CreateEmployee(con, 3, 5, "Van", 120);
            CreateEmployee(con, 2, 6, "Danny", 80);
        }
    }
}
