using System.Data.SQLite;
using System.Data.Common;

namespace Employees
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string cs = @"URI=file:test.db";

            using var con = new SQLiteConnection(cs);
            con.Open();

            Department department = new Department();
            department.CreateTestDepartmentTable(con);

            Employee employee = new Employee();
            employee.CreateTestEmployeeTable(con);

            Console.WriteLine("------------------\n");

            GetEmployeeWithMaxSalary(con); 
            MaxChainLenght(con);
            DepartmentWithMaxSummarySalary(con);
            GetAnEmployeeWithSpecificChars(con, 'D', 'y');
        }

        public static void GetAnEmployeeWithSpecificChars(DbConnection con, char firstChar, char lastChar)
        {
            using var cmd = con.CreateCommand();

            cmd.CommandText = $"SELECT Id, Name FROM Employee WHERE Name LIKE '{firstChar}%{lastChar}'";
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine(reader.GetString(1));
            }
        }

        public static void DepartmentWithMaxSummarySalary(DbConnection con)
        {
            using var cmd = con.CreateCommand();

            Department.SalariesSum(con);

            cmd.CommandText = "SELECT id FROM department_summary_salary";
            var departmentId = cmd.ExecuteScalar();
            cmd.CommandText = "SELECT name FROM department_summary_salary";
            var departmentName = cmd.ExecuteScalar();
            cmd.CommandText = "SELECT total_salary FROM department_summary_salary";
            var departmentSalary = cmd.ExecuteScalar();

            Console.WriteLine($"{departmentId}, {departmentName}, {departmentSalary}");

        }

        public static void MaxChainLenght(DbConnection con)
        {
            using var cmd = con.CreateCommand();

            Employee.CreateChainLengthCache(con);

            cmd.CommandText = "SELECT Id, Chief_Id, Name FROM Employee";
            var reader = cmd.ExecuteReader();

            int maxChainLenght = 0;

            while (reader.Read())
            {
                var currentChainLength = Employee.ChainLength(con, reader.GetInt32(0));
                if (maxChainLenght < currentChainLength)
                {
                    maxChainLenght = currentChainLength;
                }
            }

            Employee.DestroyChainLengthCache(con);

            Console.WriteLine(maxChainLenght);
        }

        public static void GetEmployeeWithMaxSalary(DbConnection con)
        {
            using var cmd = con.CreateCommand();
            cmd.CommandText = "SELECT max(salary), Id, Name FROM Employee";
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine($"{reader.GetInt32(0)}, {reader.GetInt32(1)}, {reader.GetString(2)}");
            }
        }
    }

}