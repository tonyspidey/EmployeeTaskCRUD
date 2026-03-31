# EMPLOYEE AND SALARY SLIP GENERATION

## About This Project

* This Project is built using C# and SQL Server.

* It helps to manage employee details and their salary information.

* It will allow you to add, update, delete, and view employees and their salary records.

## Project Files

* It has 4 C# files and 1 SQL file.

* SQL file: In this file I have created a database, tables and inserted some values, stored procedure, index and views.

* Program.cs: This is the starting point. It connects to the database and shows the Main Menu.

* EmployeeDetails.cs: This file has the information about the employees and we can insert, update and delete. Here I have used the Stored Procedure to pass the value to the database.

* SalaryDetails.cs: This file has the salary calculation and it will save the salary for the employee. Here I have used the Query to pass the value to the database.

* SalarySlip.cs: This file will print the salary slip for an employee for a single month.

## SQL File

In this file the database has been created with name "EmployeeTaskDataBase" and then created two tables Employee and Salary.

## Employee Table

* EmpID - INT PRIMARY KEY IDENTITY(1,1)

* EmpName - VARCHAR(100) NOT NULL

* Department - VARCHAR(50) NOT NULL

* JoiningDate - DATE NOT NULL

* Designation - VARCHAR(50) NOT NULL

## Salary Table

* SalaryID - INT PRIMARY KEY IDENTITY(1,1)

* EmpID - INT FOREIGN KEY REFERENCES Employee(EmpID) ON DELETE CASCADE

* SalaryMonth - DATE NOT NULL

* BasicSalary - DECIMAL(10,2)

* Hra - DECIMAL(10,2)

* Da - DECIMAL(10,2)

* Pf - DECIMAL(10,2)

* Tax - DECIMAL(10,2)

* GrossSalary - DECIMAL(10,2)

* Deduction - DECIMAL(10,2)

* Net - DECIMAL(10,2)

## Indexes Created

### For Employee Table

* EmpName index - IX_Employee_EmpName

* Department index - IX_Employee_Department

### For Salary Table

* EmpID index - IX_Salary_EmpID

* SalaryMonth index - IX_Salary_SalaryMonth

## View Created

* vw_SalarySlip - for the salary slip

## Stored Procedures Created

* Get Employees Sorted by Name A to Z - usp_Employee_GetByName

* Get Employees by Department - usp_Employee_GetByDepartment

* Get Employees by Designation - usp_Employee_GetByDesignation

* Insert Employee - usp_Employee_Insert

* Update Employee - usp_Employee_Update

* Delete Employee - usp_Employee_Delete

* SalarySlip using EmpID - usp_SalarySlip_GetByEmpID

## Program.cs

* Here the database will be connected with C# using the database server name and database name.

* Next the connection will be opened and display the Main Menu.

* Based upon the user choice each file will be called.

## EmployeeDetails.cs

* This file will be called when user presses the "Employee Details" (choice 1).

* In this file a separate menu for employee is there and that will be displayed.

* Based upon the choice each method will be executed.

* If user does not enter any field it will show an error message like "the field cannot be empty".

* If user is entering the joining date and it is greater than the current date, it will show an error like "Joining date cannot be greater than current date".

* The value in each method is passed to the database using stored procedures.

* Usually the employee details will be printed in ascending order.

* If needed, employees can also be displayed based on department and designation.

* If user presses "Exit" (choice 0) it will return to the main menu.

## SalaryDetails.cs

* This file will be called when the user presses the "Salary Details" (choice 2) in the main menu.

* A separate menu for salary operations will be displayed.

* Based upon the choice each method will be executed.

* If user does not enter any field, it will show an error message like "the field cannot be empty".

* Salary will be calculated for each employee for a specific month and stored in the database.

* Only one salary entry is allowed per employee per month.

* All employee salaries can also be viewed.

* The values are passed to the database using queries.

* Data will be displayed in table format.

* If user presses "Exit" (choice 0) it will return to the main menu.

## SalarySlip.cs

* This file will be called when user presses the "Salary Slip" (choice 3) in the main menu.

* A separate menu for salary slip operations will be displayed.

* User will enter EmpID, month, and year.

* The date is set to 28 by default.

* There are two methods:

  * Display salary for one employee (month-wise)
  * Display salary for all employees based on department

* Indexes are used on EmpID and SalaryMonth for faster data retrieval.

* Data is retrieved using stored procedures and displayed using views.

* If user presses "Exit" (choice 0) it returns to the main menu.

* Pressing "Exit" again will close the database connection.
