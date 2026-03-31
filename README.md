                   EMPLOYEE AND SALARY SLIP GENERATION  
About This Project
This Project is built using C# and SQL Server.
It helps to manage employee details and their salary information.
It will allows you to add, update, delete, and view employees and their salary records.
Project Files 
It has 4 C# files and 1 SQL file.
SQL file: In this file I have created a database, tables and inserted some values, stored procedure, index and views.
Program.cs: This is the starting point. It connects to the database and shows the Main Menu.
EmployeeDetails.cs: This file has the information about the employees and we can insert, update and delete. Here I have used the Stored Procedure to pass the value to the database.
SalaryDetails.cs: This file has the salary calculation and it will save the salary for the employee. Here I have used the Query to pass the value to the database.
SalarySlip.cs: This file will print the salary slip for an employee for a single month.
SQL File
In this file the database has been created with name  "EmployeeTaskDataBase" and then created two tables Employee and Salary.
Employee table:
EmpID - INT PRIMARY KEY IDENTITY(1,1)
EmpName - VARCHAR(100) NOT NULL
Department - VARCHAR(50) NOT NULL
JoiningDate - DATE NOT NULL
Designation - VARCHAR(50) NOT NULL
Salary table:
SalaryID - INT PRIMARY KEY IDENTITY(1,1)
EmpID - INT FOREIGN KEY REFERENCES Employee(EmpID) ON DELETE CASCADE
SalaryMonth - DATE NOT NULL
BasicSalary - DECIMAL(10,2)
Hra - DECIMAL(10,2)
Da - DECIMAL(10,2)
Pf - DECIMAL(10,2)
Tax - DECIMAL(10,2)
GrossSalary - DECIMAL(10,2)
Deduction - DECIMAL(10,2)
Net - DECIMAL(10,2)
Indexes created:
For Employee table:
EmpName index - IX_Employee_EmpName
Department index - IX_Employee_Department
For Salary table:
EmpID index - IX_Salary_EmpID
SalaryMonth index - IX_Salary_SalaryMonth
View created:
vw_SalarySlip - for the salary slip
Stored procedures created:
Get Employees Sorted by Name A to Z - usp_Employee_GetByName
Get Employees by Department - usp_Employee_GetByDepartment
Get Employees by Designation - usp_Employee_GetByDesignation
Insert Employee - usp_Employee_Insert
Update Employee - usp_Employee_Update
Delete Employee - usp_Employee_Delete
SalarySlip using EmpID - usp_SalarySlip_GetByEmpID
Program.cs
Here the database will be connected with C# using the database server name, and database name.
Next the connection will be opened and display the Main Menu.
Based upon the user choice each file will be called.
EmployeeDetails.cs
This file will be called when user press the "Employee Details" (choice 1).
In this file a separate menu for employee is there and that will be displayed.
Based upon the choice each method will be executed.
If user didn't enter any field it will show an error message like "the field can't be empty".
If user is entering the joining date and it is greater than the current date, it will show an error like "Joining date cannot be a greater than current date".
The value in each method is passing to the database using the stored procedures.
Usually the employee details will be printed in the ascending format.
If we want we can also print the employee based upon the department and designation.
If user press the "Exit" (choice 0) means it will return to the main menu.
SalaryDetails.cs
This file will be called when the user press the "Salary Details" (choice 2) in the main menu.
In this a separate menu for salary is there and that will be displayed.
Based upon the choice each method will be executed.
If user didn't enter any field means it will show an error message like "the field can't be empty".
Here we will calculate salary for each employee for a specific month and the new salary details will be saved in the database.
For one employee only one salary will be added for a single month. More than one we can't add it.
We can also able to see all the employee salary also.
The value in each method is passed to database using query.
The data will be displayed in a table format.
If user press the "Exit" (choice 0) means it will return to the main menu.
SalarySlip.cs
This file will be called when user press the "Salary slip" (choice 3) in the main menu.
In this file a separate menu for salary is there and that will be displayed.
Here we will enter the empid and the month and year for the salary month.
Don't need to enter the date by default it will be 28.
There are two methods one is for displaying the one employee salary month wise and another one is for displaying all employee salary based upon department.
I have created index for the empid and salary month to fast retrieval of data.
The data will be taken from the database through stored procedure and displayed in a table format using views.
If user press the "Exit" (choice 0) means it will return to the main menu and if we again press "Exit" (choice 0) means it will close the database connection.
This is my task which will do the CRUD operation using C# and SQL server and the output will be displayed in the cmd.