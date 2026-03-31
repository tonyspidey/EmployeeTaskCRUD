using System;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EmployeeTaskCRUD
{
    // Class to handle all employee-related operations such as displing, inserting, updating, and deleting employees
    internal class EmployeeDetails
    {
        static SqlConnection con;

        //  EMPLOYEE MENU
        public static void EmployeeMenu(SqlConnection connection)
        {
            con = connection;

            while (true)
            {
                Console.WriteLine("========== EMPLOYEE MENU ==========");
                Console.WriteLine("1. List Employees");
                Console.WriteLine("2. Insert Employee");
                Console.WriteLine("3. Update Employee");
                Console.WriteLine("4. Delete Employee");
                Console.WriteLine("0. Back");
                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ListEmployeesMenu();
                        break;
                    case "2":
                        InsertEmployee();
                        break;
                    case "3":
                        UpdateEmployee();
                        break;
                    case "4":
                        DeleteEmployee();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

      
        //  LIST EMPLOYEES MENU
      
        static void ListEmployeesMenu()
        {
            Console.WriteLine("========== LIST EMPLOYEES ==========");
            Console.WriteLine("1. Show All Employees");
            Console.WriteLine("2. Search by Department");
            Console.WriteLine("3. Search by Designation");
            Console.WriteLine("0. Back");
            Console.Write("Enter your choice: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ListAllEmployees();
                    break;
                case "2":
                    ListByDepartment();
                    break;
                case "3":
                    ListByDesignation();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }

            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
        }

        // ---- Show all employees ----
        static void ListAllEmployees()
        {
            try
            {
                Console.WriteLine("========== ALL EMPLOYEES ==========");

                // Using stored procedure 
                SqlCommand cmd = new SqlCommand("usp_Employee_GetByName", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    Console.WriteLine("EmpID | Name | Department  | JoiningDate | Designation");
                    Console.WriteLine("----------------------------------------------------------------");

                    while (reader.Read())
                    {
                        int id = Convert.ToInt32(reader["EmpID"]);
                        string name = reader["EmpName"].ToString();
                        string dept = reader["Department"].ToString();
                        string date = Convert.ToDateTime(reader["JoiningDate"]).ToString("dd-MM-yyyy"); // date only
                        string desig = reader["Designation"].ToString();

                        Console.WriteLine(id + "     | " + name + " | " + dept + "      | " + date + " | " + desig);
                    }
                }
                else
                {
                    Console.WriteLine("No employees found.");
                }

                reader.Close();
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Database error while fetching employees: " + ex.Message);
            }
        }

        // ---- Search by department ----
        static void ListByDepartment()
        {
            try
            {
                Console.Write("Enter Department name: ");
                string dept = Console.ReadLine();

                if (dept==null || dept=="")
                {
                    Console.WriteLine("Department name cannot be empty!");
                    return;
                }

                Console.WriteLine("========== EMPLOYEES IN SAME DEPARTMENT ==========");

                // Using stored procedure 
                SqlCommand cmd = new SqlCommand("usp_Employee_GetByDepartment", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Department", dept);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    Console.WriteLine("EmpID | Name | Department  | JoiningDate | Designation");
                    Console.WriteLine("----------------------------------------------------------------");

                    while (reader.Read())
                    {
                        int id = Convert.ToInt32(reader["EmpID"]);
                        string name = reader["EmpName"].ToString();
                        string department = reader["Department"].ToString();
                        string date = Convert.ToDateTime(reader["JoiningDate"]).ToString("dd-MM-yyyy");
                        string desig = reader["Designation"].ToString();

                        Console.WriteLine(id + "     | " + name + " | " + department + "      | " + date + " | " + desig);
                    }
                }
                else
                {
                    Console.WriteLine("No employees found in this department.");
                }

                reader.Close();
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Database error while searching by department: " + ex.Message);
            }
        }

        // ---- Search by designation ----
        static void ListByDesignation()
        {
            try
            {
                Console.Write("Enter Designation: ");
                string desig = Console.ReadLine();

                if (desig==null || desig=="")
                {
                    Console.WriteLine("Designation cannot be empty!");
                    return;
                }

                Console.WriteLine("========== EMPLOYEES IN SAME DESIGNATION ==========");

                // Using stored procedure 
                SqlCommand cmd = new SqlCommand("usp_Employee_GetByDesignation", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Designation", desig);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    Console.WriteLine("EmpID | Name | Department  | JoiningDate | Designation");
                    Console.WriteLine("----------------------------------------------------------------");

                    while (reader.Read())
                    {
                        int id = Convert.ToInt32(reader["EmpID"]);
                        string name = reader["EmpName"].ToString();
                        string department = reader["Department"].ToString();
                        string date = Convert.ToDateTime(reader["JoiningDate"]).ToString("dd-MM-yyyy"); // date only
                        string designation = reader["Designation"].ToString();

                        Console.WriteLine(id + "     | " + name + " | " + department + "      | " + date + " | " + designation);
                    }
                }
                else
                {
                    Console.WriteLine("No employees found with this designation.");
                }

                reader.Close();
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Database error while searching by designation: " + ex.Message);
            }
        }
 
        //  INSERT EMPLOYEE
      
        static void InsertEmployee()
        {
            try
            {
                Console.WriteLine("========== INSERT NEW EMPLOYEE ==========");

                Console.Write("Enter Employee Name: ");
                string name = Console.ReadLine();

                if (name == null || name == "")
                {
                    Console.WriteLine("Employee name cannot be empty!");
                    return;
                }

                Console.Write("Enter Department: ");
                string dept = Console.ReadLine();

                if (dept == null || dept == "")
                {
                    Console.WriteLine("Department cannot be empty!");
                    return;
                }

                Console.Write("Enter Designation: ");
                string desig = Console.ReadLine();

                if (desig == null || desig == "")
                {
                    Console.WriteLine("Designation cannot be empty!");
                    return;
                }

                Console.Write("Enter Joining Date (dd-MM-yyyy): ");
                string joiningInput = Console.ReadLine();
                if (joiningInput == null || joiningInput == "")
                {
                    Console.WriteLine("Joining date cannot be empty!");
                    return;
                }
               
                DateTime joiningDate = DateTime.ParseExact(joiningInput, "dd-MM-yyyy", null);

                if (joiningDate.Date > DateTime.Today)
                {
                    Console.WriteLine("Joining date cannot be a greater than current  date ");
                    return;
                }

                string sqlDate = joiningDate.Year + "-" + joiningDate.Month + "-" + joiningDate.Day;

                // Using stored procedure 
                SqlCommand cmd = new SqlCommand("usp_Employee_Insert", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmpName", name);
                cmd.Parameters.AddWithValue("@Department", dept);
                cmd.Parameters.AddWithValue("@JoiningDate", joiningDate);
                cmd.Parameters.AddWithValue("@Designation", desig);

                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Console.WriteLine("Employee inserted successfully!");
                }
                else
                {
                    Console.WriteLine("Failed to insert employee.");
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Database error while inserting employee: " + ex.Message);
            }
            finally
            {
                Console.WriteLine("\nPress Enter to continue...");
                Console.ReadLine();
            }
        }

      
        //  UPDATE EMPLOYEE
       
        static void UpdateEmployee()
        {
            try
            {
                Console.WriteLine("========== UPDATE EMPLOYEE ==========");

                Console.Write("Enter EmpID to update: ");
                string empInput = Console.ReadLine();

                if (empInput == null || empInput == "")
                {
                    Console.WriteLine("EmpID cannot be empty!");
                    return;
                }

                int empId = Convert.ToInt32(empInput);

                Console.Write("Enter  Name: ");
                string newName = Console.ReadLine();

                if (newName == null || newName == "")
                {
                    Console.WriteLine("Employee name cannot be empty!");
                    return;
                }

                Console.Write("Enter  Department: ");
                string newDept = Console.ReadLine();

                if (newDept == null || newDept == "")
                {
                    Console.WriteLine("Department cannot be empty!");
                    return;
                }

                Console.Write("Enter  Designation: ");
                string newDesig = Console.ReadLine();

                if (newDesig == null || newDesig == "")
                {
                    Console.WriteLine("Designation cannot be empty!");
                    return;
                }

                Console.Write("Enter  Joining Date (dd-mm-yyyy): ");
                string newDate = Console.ReadLine();

                if (newDate == null || newDate == "")
                {
                    Console.WriteLine("Joining date cannot be empty!");
                    return;
                }

                // Converting dd-MM-yyyy string to DateTime
                DateTime joiningDate = DateTime.ParseExact(newDate, "dd-MM-yyyy", null);

                // Converting DateTime to SQL format (yyyy-MM-dd)
                string sqlDate = joiningDate.Year + "-" + joiningDate.Month + "-" + joiningDate.Day;


                // Using stored procedure 
                SqlCommand cmd = new SqlCommand("usp_Employee_Update", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmpID", empId);
                cmd.Parameters.AddWithValue("@EmpName", newName);
                cmd.Parameters.AddWithValue("@Department", newDept);
                cmd.Parameters.AddWithValue("@Designation", newDesig);
                cmd.Parameters.AddWithValue("@JoiningDate", sqlDate);

                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Console.WriteLine("Employee updated successfully!");
                }
                else
                {
                    Console.WriteLine("Employee not found. Nothing was updated.");
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Database error while updating employee: " + ex.Message);
            }
            finally
            {
                Console.WriteLine("\nPress Enter to continue...");
                Console.ReadLine();
            }
        }

    
        //  DELETE EMPLOYEE
      
        static void DeleteEmployee()
        {
            try
            {
                Console.WriteLine("========== DELETE EMPLOYEE ==========");

                Console.Write("Enter EmpID to delete: ");
                string empInput = Console.ReadLine();

                if (empInput == null || empInput == "")
                {
                    Console.WriteLine("EmpID cannot be empty!");
                    return;
                }

                int empId = Convert.ToInt32(empInput);

                Console.Write("Are you sure you want to delete EmpID " + empId + "? (y/n): ");
                string confirm = Console.ReadLine();

                if (confirm != "y" || confirm != "Y")
                {
                    Console.WriteLine("Deletion cancelled.");
                    return;
                }

                // Using stored procedure 
                SqlCommand cmd = new SqlCommand("usp_Employee_Delete", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmpID", empId);

                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Console.WriteLine("Employee deleted successfully!");
                }
                else
                {
                    Console.WriteLine("Employee not found.");
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Database error while deleting employee: " + ex.Message);
            }
            finally
            {
                Console.WriteLine("\nPress Enter to continue...");
                Console.ReadLine();
            }
        }
    }
}