using System;
using Microsoft.Data.SqlClient;

namespace EmployeeTaskCRUD
{
    // Main program class to handle database connection and main menu navigation
    internal class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Data Source=CGVAK-LT114\\SQLEXP;Initial Catalog=EmployeeTaskDataBase;Integrated Security=True;TrustServerCertificate=True;";

            SqlConnection con = new SqlConnection(connectionString);

            try
            {
                con.Open();
                Console.WriteLine();
                ShowMainMenu(con);
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Something went wrong: " + ex.Message);
            }
            finally
            {
                con.Close();
                Console.WriteLine("Database connection closed.");
            }
        }

        static void ShowMainMenu(SqlConnection con)
        {
            while (true)
            {
                Console.WriteLine("========== MAIN MENU ==========");
                Console.WriteLine("1. Employee Details");
                Console.WriteLine("2. Salary Details");
                Console.WriteLine("3. Salary Slip");
                Console.WriteLine("0. Exit");
                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        EmployeeDetails.EmployeeMenu(con);
                        break;
                    case "2":
                        SalaryDetails.SalaryMenu(con);
                        break;
                    case "3":
                        SalarySlip.SalarySlipMenu(con);
                        break;
                    case "0":
                        Console.WriteLine("Exit");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }
    }
}