using System;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EmployeeTaskCRUD
{
    internal class SalarySlip
    {
        static SqlConnection con;

        //  SALARY SLIP MENU

        public static void SalarySlipMenu(SqlConnection connection)
        {
            con = connection;

            while (true)
            {
                Console.WriteLine("========== SALARY SLIP MENU ==========");
                Console.WriteLine("1. View Salary Slip by EmpID");
                Console.WriteLine("2. View Salary by Department");
                Console.WriteLine("0. Back");
                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ViewSalarySlip();
                        break;
                    case "2":
                        ViewSalaryByDepartment();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }
        static void ViewSalarySlip()
        {
            try
            {
                Console.WriteLine("========== SALARY SLIP ==========");

                Console.Write("Enter EmpID: ");
                string empInput = Console.ReadLine();

                if (string.IsNullOrEmpty(empInput))
                {
                    Console.WriteLine("EmpID cannot be empty!");
                    return;
                }

                int empId = Convert.ToInt32(empInput);

               //enter only month and year
                Console.Write("Enter only  month and year for salary slip (MM-yyyy): ");
                string monthInput = Console.ReadLine();

                if (string.IsNullOrEmpty(monthInput))
                {
                    Console.WriteLine("Month and year cannot be empty!");
                    return;
                }

                string fullDate = "28-" + monthInput;
                DateTime salaryMonth = DateTime.ParseExact(fullDate, "dd-MM-yyyy", null);
                string sqlDate = salaryMonth.Year + "-"+salaryMonth.Month + "-" + salaryMonth.Day;

                // using stored procedure
                SqlCommand cmd = new SqlCommand("usp_SalarySlip_GetByEmpID", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmpID", empId);
                cmd.Parameters.AddWithValue("@SalaryMonth", sqlDate);

                SqlDataReader reader = cmd.ExecuteReader();

                if (!reader.HasRows)
                {
                    Console.WriteLine("No salary record found for this employee for the given month.");
                    reader.Close();
                    return;
                }

                reader.Read();

                string empName = reader["EmpName"].ToString();
                string department = reader["Department"].ToString();
                string designation = reader["Designation"].ToString();
                string joinDate = Convert.ToDateTime(reader["JoiningDate"]).ToString("dd-MM-yyyy");
                string salMonth = salaryMonth.ToString("MM-yyyy");

                decimal basicSalary = Convert.ToDecimal(reader["BasicSalary"]);
                decimal hra = Convert.ToDecimal(reader["Hra"]);
                decimal da = Convert.ToDecimal(reader["Da"]);
                decimal pf = Convert.ToDecimal(reader["Pf"]);
                decimal tax = Convert.ToDecimal(reader["Tax"]);
                decimal grossSalary = Convert.ToDecimal(reader["GrossSalary"]);
                decimal deduction = Convert.ToDecimal(reader["Deduction"]);
                decimal netSalary = Convert.ToDecimal(reader["Net"]);

                reader.Close();

                Console.WriteLine("==================================================");
                Console.WriteLine("                  SALARY SLIP                    ");
                Console.WriteLine("==================================================");
                Console.WriteLine("Employee     : " + empName);
                Console.WriteLine("Department   : " + department);
                Console.WriteLine("Designation  : " + designation);
                Console.WriteLine("Joining Date : " + joinDate);
                Console.WriteLine("Salary Month : " + salMonth);
                Console.WriteLine("Basic Salary : Rs. " + basicSalary);
                Console.WriteLine("HRA          : Rs. " + hra);
                Console.WriteLine("DA           : Rs. " + da);
                Console.WriteLine("Gross Salary : Rs. " + grossSalary);
                Console.WriteLine("PF           : Rs. " + pf);
                Console.WriteLine("Tax          : Rs. " + tax);
                Console.WriteLine("Deduction    : Rs. " + deduction);
                Console.WriteLine("NET SALARY   : Rs. " + netSalary);
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Database error while fetching salary slip: " + ex.Message);
            }
            finally
            {
                Console.WriteLine("\nPress Enter to continue...");
                Console.ReadLine();
            }
        }
        static void ViewSalaryByDepartment()
        {
            try
            {
                Console.WriteLine("========== SALARY BY DEPARTMENT ==========");

                Console.Write("Enter Department name: ");
                string dept = Console.ReadLine();

                if (string.IsNullOrEmpty(dept))
                {
                    Console.WriteLine("Department cannot be empty!");
                    return;
                }

                string query = "SELECT e.EmpName, e.Department, e.Designation, e.JoiningDate ,s.BasicSalary, s.GrossSalary, s.Deduction, s.Net FROM Employee e JOIN Salary s ON e.EmpID = s.EmpID WHERE e.Department = @Department";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Department", dept);

                SqlDataReader reader = cmd.ExecuteReader();

                if (!reader.HasRows)
                {
                    Console.WriteLine("No salary records found for this department.");
                    reader.Close();
                    return;
                }

                Console.WriteLine("Name   | Department  | Designation | JoiningDate  |Basic     | Gross    | Deduction | Net");
                Console.WriteLine("-------------------------------------------------------------------------------------------");

                while (reader.Read())
                {
                    string name = reader["EmpName"].ToString();
                    string department = reader["Department"].ToString();
                    string designation = reader["Designation"].ToString();
                    string joiningDate = Convert.ToDateTime(reader["JoiningDate"]).ToString("dd-MM-yyyy");
                    decimal basic = Convert.ToDecimal(reader["BasicSalary"]);
                    decimal gross = Convert.ToDecimal(reader["GrossSalary"]);
                    decimal deduction = Convert.ToDecimal(reader["Deduction"]);
                    decimal net = Convert.ToDecimal(reader["Net"]);

                    Console.WriteLine(name + " | " + department + " | " + designation + " | "+ joiningDate + " | " + basic + " | " + gross + " | " + deduction + " | " + net);
                }

                reader.Close();
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Database error while fetching salary by department: " + ex.Message);
            }
            finally
            {
                Console.WriteLine("\nPress Enter to continue...");
                Console.ReadLine();
            }
        }
    }
}