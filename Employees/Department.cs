using System.Data.Common;

namespace Employees
{
    public class Department
    {

        public void CreateTable(DbConnection con)
        {
            using var cmd = con.CreateCommand();

            cmd.CommandText = "DROP TABLE IF EXISTS Department";
            cmd.ExecuteNonQuery();

            cmd.CommandText = @"CREATE TABLE Department(Id INTEGER PRIMARY KEY, 
                                                        Name string)";
            cmd.ExecuteNonQuery();

            Console.WriteLine("Table \"Department\" created");
        }

        public void CreateDepartment(DbConnection con, string name)
        {
            using var cmd = con.CreateCommand();

            cmd.CommandText = $"INSERT INTO Department(Name) VALUES('{name}')";
            cmd.ExecuteNonQuery();
        }


        public void CreateTestDepartmentTable(DbConnection con)
        {
            CreateTable(con);

            CreateDepartment(con, "A");
            CreateDepartment(con, "B");
            CreateDepartment(con, "C");
        }

        public static void SalariesSum(DbConnection con)
        {
            using var cmd = con.CreateCommand();

            cmd.CommandText = "DROP VIEW IF EXISTS department_summary_salary;\r\n" +
                "CREATE VIEW department_summary_salary AS\r\n" +
                "SELECT department.id, department.name, SUM(employee.salary) AS total_salary\r\n" +
                "FROM department\r\n" +
                "INNER JOIN employee ON department.id = employee.department_id\r\n" +
                "GROUP BY department.id, department.name " +
                "ORDER BY total_salary DESC LIMIT 1";
            cmd.ExecuteNonQuery();
        }
    }
}
