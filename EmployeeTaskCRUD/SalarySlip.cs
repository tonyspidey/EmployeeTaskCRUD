using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EmployeeTaskCRUD
{
    internal class SalaryInfo
    {
        public int EmpID = 0;
        public string Name = "";
        public string Department = "";
        public string Designation = "";
        public string JoiningDate = "";
        public string SalaryMonth = "";
        public decimal Basic = 0;
        public decimal Hra = 0;
        public decimal Da = 0;
        public decimal Pf = 0;
        public decimal Tax = 0;
        public decimal Gross = 0;
        public decimal Deduction = 0;
        public decimal Net = 0;
    }

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

              
                Console.Write("Enter only month and year for salary slip (MM-yyyy): ");
                string monthInput = Console.ReadLine();

                if (string.IsNullOrEmpty(monthInput))
                {
                    Console.WriteLine("Month and year cannot be empty!");
                    return;
                }

                string fullDate = "28-" + monthInput;
                DateTime salaryMonth = DateTime.ParseExact(fullDate, "dd-MM-yyyy", null);


                string query = "SELECT * FROM vw_SalarySlip WHERE EmpID = @EmpID AND MONTH(SalaryMonth) = @Month AND YEAR(SalaryMonth) = @Year";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@EmpID", empId);
                    cmd.Parameters.AddWithValue("@Month", salaryMonth.Month);
                    cmd.Parameters.AddWithValue("@Year", salaryMonth.Year);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            Console.WriteLine("No salary record found for this employee for the given month.");
                            return;
                        }

                      
                        List<SalaryInfo> slipList = new List<SalaryInfo>();

                        while (reader.Read())
                        {
                            slipList.Add(new SalaryInfo
                            {
                                EmpID = Convert.ToInt32(reader["EmpID"]),
                                Name = reader["EmpName"].ToString(),
                                Department = reader["Department"].ToString(),
                                Designation = reader["Designation"].ToString(),
                                JoiningDate = Convert.ToDateTime(reader["JoiningDate"]).ToString("dd-MM-yyyy"),
                                SalaryMonth = Convert.ToDateTime(reader["SalaryMonth"]).ToString("MM-yyyy"),
                                Basic = Convert.ToDecimal(reader["BasicSalary"]),
                                Hra = Convert.ToDecimal(reader["Hra"]),
                                Da = Convert.ToDecimal(reader["Da"]),
                                Pf = Convert.ToDecimal(reader["Pf"]),
                                Tax = Convert.ToDecimal(reader["Tax"]),
                                Gross = Convert.ToDecimal(reader["GrossSalary"]),
                                Deduction = Convert.ToDecimal(reader["Deduction"]),
                                Net = Convert.ToDecimal(reader["Net"])
                            });
                        }


                        var result = from s in slipList
                                     select s;

                        SalaryInfo slip = null;
                        foreach (var s in result)
                        {
                            if (s.SalaryMonth == salaryMonth.ToString("MM-yyyy"))
                            {
                                slip = s;
                            }
                        }

                        if (slip == null)
                        {
                            Console.WriteLine("No salary record found for this employee for the given month.");
                            return;
                        }

                        Console.WriteLine("==================================================");
                        Console.WriteLine("                  SALARY SLIP                    ");
                        Console.WriteLine("==================================================");
                        Console.WriteLine("Employee     : " + slip.Name);
                        Console.WriteLine("Department   : " + slip.Department);
                        Console.WriteLine("Designation  : " + slip.Designation);
                        Console.WriteLine("Joining Date : " + slip.JoiningDate);
                        Console.WriteLine("Salary Month : " + "28-"+slip.SalaryMonth);
                        Console.WriteLine("Basic Salary : Rs. " + slip.Basic);
                        Console.WriteLine("HRA          : Rs. " + slip.Hra);
                        Console.WriteLine("DA           : Rs. " + slip.Da);
                        Console.WriteLine("Gross Salary : Rs. " + slip.Gross);
                        Console.WriteLine("PF           : Rs. " + slip.Pf);
                        Console.WriteLine("Tax          : Rs. " + slip.Tax);
                        Console.WriteLine("Deduction    : Rs. " + slip.Deduction);
                        Console.WriteLine("NET SALARY   : Rs. " + slip.Net);
                    }
                }
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

        // ---- View Salary by Department ----
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


                string query = "SELECT * FROM vw_SalaryByDepartment WHERE Department = @Department";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Department", dept);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            Console.WriteLine("No salary records found for this department.");
                            return;
                        }

                        List<SalaryInfo> salaryList = new List<SalaryInfo>();

                        while (reader.Read())
                        {
                            salaryList.Add(new SalaryInfo
                            {
                                Name = reader["EmpName"].ToString(),
                                Department = reader["Department"].ToString(),
                                Designation = reader["Designation"].ToString(),
                                JoiningDate = Convert.ToDateTime(reader["JoiningDate"]).ToString("dd-MM-yyyy"),
                                SalaryMonth = Convert.ToDateTime(reader["SalaryMonth"]).ToString("MM-yyyy"),
                                Basic = Convert.ToDecimal(reader["BasicSalary"]),
                                Gross = Convert.ToDecimal(reader["GrossSalary"]),
                                Deduction = Convert.ToDecimal(reader["Deduction"]),
                                Net = Convert.ToDecimal(reader["Net"])
                            });
                        }

                      
                        var sortedList = from s in salaryList
                                         orderby s.Name ascending
                                         select s;

                        Console.WriteLine("Name       | Department  | Designation  | JoiningDate  | SalaryMonth  | Basic Pay    | Gross Pay    | Deduction    | Net Pay");
                        Console.WriteLine("---------------------------------------------------------------------------------------------------------------------------------");

                        foreach (var emp in sortedList)
                        {
                            Console.WriteLine(emp.Name  +"\t"+  " | " + emp.Department  +"\t"+   " | " + emp.Designation   +"\t" +  " | " + emp.JoiningDate   +"\t" +  " | " + " 28-" + emp.SalaryMonth   +"\t"+  " | " + emp.Basic   +"\t"+  " | " + emp.Gross   +"\t"+  " | " + emp.Deduction   +"\t"+  " | " + emp.Net);
                        }
                    }
                }
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