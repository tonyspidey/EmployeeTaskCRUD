using System;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EmployeeTaskCRUD
{
    internal class SalaryDetails
    {
        static SqlConnection con;

        //  SALARY MENU

        public static void SalaryMenu(SqlConnection connection)
        {
            con = connection;

            while (true)
            {
                Console.WriteLine("========== SALARY MENU ==========");
                Console.WriteLine("1. Calculate Salary Details");
                Console.WriteLine("2. View All Salaries Details");
                Console.WriteLine("0. Back");
                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        InsertSalary();
                        break;
                    case "2":
                        ViewAllSalaries();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        //  INSERT SALARY

        static void InsertSalary()
        {
            try
            {
                Console.WriteLine("========== CALCULATE SALARY ==========");

               
                Console.Write("Enter EmpID: ");
                string empInput = Console.ReadLine();

                if (string.IsNullOrEmpty(empInput))
                {
                    Console.WriteLine("EmpID cannot be empty!");
                    return;
                }

                int empId = Convert.ToInt32(empInput);

               
                string empQuery = "SELECT EmpName, Designation, Department FROM Employee WHERE EmpID = @EmpID";
                SqlCommand checkCmd = new SqlCommand(empQuery, con);
                checkCmd.Parameters.AddWithValue("@EmpID", empId);

                SqlDataReader checkReader = checkCmd.ExecuteReader();

                if (!checkReader.HasRows)
                {
                    Console.WriteLine("Employee not found!");
                    checkReader.Close();
                    return;
                }

                checkReader.Read();
                Console.WriteLine("Employee: " + checkReader["EmpName"].ToString() + " (" + checkReader["Designation"].ToString() + " - " + checkReader["Department"].ToString() + ")");
                checkReader.Close();

               //salary month 
                Console.Write("Enter Salary Month (28-MM-yyyy): ");
                string monthInput = Console.ReadLine();

                if (string.IsNullOrEmpty(monthInput))
                {
                    Console.WriteLine("Salary month cannot be empty!");
                    return;
                }

                DateTime salaryMonth = DateTime.ParseExact(monthInput, "dd-MM-yyyy", null);

             
                string duplicateQuery = "SELECT COUNT(*) FROM Salary WHERE EmpID = @EmpID AND MONTH(SalaryMonth) = @Month AND YEAR(SalaryMonth) = @Year";
                SqlCommand dupCmd = new SqlCommand(duplicateQuery, con);
                dupCmd.Parameters.AddWithValue("@EmpID", empId);
                dupCmd.Parameters.AddWithValue("@Month", salaryMonth.Month);
                dupCmd.Parameters.AddWithValue("@Year", salaryMonth.Year);

                int existingCount = Convert.ToInt32(dupCmd.ExecuteScalar());
                if (existingCount > 0)
                {
                    Console.WriteLine("Salary already added for this month");
                    return;
                }

               
                Console.Write("Enter Basic Salary: Rs. ");
                string salaryInput = Console.ReadLine();

                if (string.IsNullOrEmpty(salaryInput))
                {
                    Console.WriteLine("Basic salary cannot be empty!");
                    return;
                }

                double basicSalary = Convert.ToDouble(salaryInput);

                if (basicSalary <= 0)
                {
                    Console.WriteLine("Basic salary must be greater than zero!");
                    return;
                }

                // Calculate all salary parts
                double hra = 0, da = 0, pf = 0, tax = 0;

                if (basicSalary > 40000)
                {
                    da = 58.5 / 100 * basicSalary;
                    hra = 15.0 / 100 * basicSalary;
                    pf = 20.0 / 100 * basicSalary;
                    tax = 17.0 / 100 * basicSalary;
                }
                else if (basicSalary > 20000)
                {
                    da = 46.0 / 100 * basicSalary;
                    hra = 12.0 / 100 * basicSalary;
                    pf = 15.0 / 100 * basicSalary;
                    tax = 12.0 / 100 * basicSalary;
                }
                else
                {
                    da = 42.5 / 100 * basicSalary;
                    hra = 1500;
                    pf = 10.0 / 100 * basicSalary;
                    tax = 0;
                }

                double grossSalary = basicSalary + da + hra;
                double deduction = pf + tax;
                double netSalary = grossSalary - deduction;

               
                Console.WriteLine("\n---------- SALARY DETAILS ----------");
                Console.WriteLine("  Salary Month    : " + salaryMonth.ToString("dd-MM-yyyy"));
                Console.WriteLine("  Basic Salary    : Rs. " + basicSalary);
                Console.WriteLine("  HRA             : Rs. " + hra);
                Console.WriteLine("  DA              : Rs. " + da);
                Console.WriteLine("  Gross Salary    : Rs. " + grossSalary);
                Console.WriteLine("  PF              : Rs. " + pf);
                Console.WriteLine("  Tax             : Rs. " + tax);
                Console.WriteLine("  Total Deductions: Rs. " + deduction);
                Console.WriteLine("  NET SALARY      : Rs. " + netSalary);

               
                string insertQuery = "INSERT INTO Salary(EmpID, BasicSalary, Hra, Da, Pf, Tax, GrossSalary, Deduction, Net, SalaryMonth) " +
                                     "VALUES (@EmpID, @BasicSalary, @Hra, @Da, @Pf, @Tax, @GrossSalary, @Deduction, @Net, @SalaryMonth)";

                SqlCommand insertCmd = new SqlCommand(insertQuery, con);
                insertCmd.Parameters.AddWithValue("@EmpID", empId);
                insertCmd.Parameters.AddWithValue("@BasicSalary", basicSalary);
                insertCmd.Parameters.AddWithValue("@Hra", hra);
                insertCmd.Parameters.AddWithValue("@Da", da);
                insertCmd.Parameters.AddWithValue("@Pf", pf);
                insertCmd.Parameters.AddWithValue("@Tax", tax);
                insertCmd.Parameters.AddWithValue("@GrossSalary", grossSalary);
                insertCmd.Parameters.AddWithValue("@Deduction", deduction);
                insertCmd.Parameters.AddWithValue("@Net", netSalary);
                insertCmd.Parameters.AddWithValue("@SalaryMonth", salaryMonth.ToString("yyyy-MM-dd"));

                int rowsInserted = insertCmd.ExecuteNonQuery();
                if (rowsInserted > 0)
                {
                    Console.WriteLine("Salary saved successfully!");
                }
                else
                {
                    Console.WriteLine("Failed to save salary.");
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Database error while saving salary: " + ex.Message);
            }
            finally
            {
                Console.WriteLine("\nPress Enter to continue...");
                Console.ReadLine();
            }
        }

        //  VIEW ALL SALARIES

        static void ViewAllSalaries()
        {
            try
            {
                Console.WriteLine("========== ALL SALARY RECORDS ==========");

                string query = "SELECT s.SalaryID, e.EmpName, s.SalaryMonth, s.BasicSalary, s.Hra, s.Da, s.Pf, s.Tax, s.GrossSalary, s.Deduction, s.Net FROM Salary s JOIN Employee e ON s.EmpID = e.EmpID";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    Console.WriteLine("SalID | Name       | Month      | Basic   | HRA    | DA     | PF     | Tax   | Gross   | Deduct  | Net");
                    Console.WriteLine("----------------------------------------------------------------------------------------------------------");

                    while (reader.Read())
                    {
                        int salId = Convert.ToInt32(reader["SalaryID"]);
                        string name = reader["EmpName"].ToString();
                        string month = Convert.ToDateTime(reader["SalaryMonth"]).ToString("dd-MM-yyyy");
                        decimal basic = Convert.ToDecimal(reader["BasicSalary"]);
                        decimal hra = Convert.ToDecimal(reader["Hra"]);
                        decimal da = Convert.ToDecimal(reader["Da"]);
                        decimal pf = Convert.ToDecimal(reader["Pf"]);
                        decimal tax = Convert.ToDecimal(reader["Tax"]);
                        decimal gross = Convert.ToDecimal(reader["GrossSalary"]);
                        decimal deduction = Convert.ToDecimal(reader["Deduction"]);
                        decimal net = Convert.ToDecimal(reader["Net"]);

                        Console.WriteLine(salId + "     | " + name + " | " + month + " | " + basic + " | " + hra + " | " + da + " | " + pf + " | " + tax + " | " + gross + " | " + deduction + " | " + net);
                    }
                }
                else
                {
                    Console.WriteLine("No salary records found.");
                }

                reader.Close();
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Database error while fetching salaries: " + ex.Message);
            }
            finally
            {
                Console.WriteLine("\nPress Enter to continue...");
                Console.ReadLine();
            }
        }
    }
}