create database EmployeeManagement;
use EmployeeManagement;
--table--
create table Employee(
EmployeeID int identity(100,1) primary key,
EmployeeName varchar(50),
EmailID varchar(50),
PhoneNumber bigint,
Address varchar(50),
DateOfBirth DateTime,
DateOfJoining DateTime,
Salary bigint,
);
select * from Employee;
--create storeprocedure--
create procedure EmployeeSP(
@EmployeeName varchar(50),
@EmailID varchar(50),
@PhoneNumber bigint,
@Address varchar(50),
@DateOfBirth DateTime,
@DateOfJoining DateTime,
@Salary bigint
)
As
Begin
insert into Employee values(@EmployeeName,@EmailID,@PhoneNumber,@Address,@DateOfBirth,@DateOfJoining,@Salary);
End
--Update storeprocedure--
create procedure UpdateEmployee(
@EmployeeID int,
@EmployeeName varchar(50),
@EmailID varchar(50),
@PhoneNumber bigint,
@Address varchar(50),
@DateOfBirth DateTime,
@DateOfJoining DateTime,
@Salary bigint
)
As
Begin
update Employee set EmployeeName=@EmployeeName,EmailID=@EmailID,PhoneNumber=@PhoneNumber,Address=@Address,DateOfBirth=@DateOfBirth,DateOfJoining=@DateOfJoining,Salary=@Salary where EmployeeID=@EmployeeID;
End
--Delete storeprocedure--
create procedure DeleteEmployee(
@EmployeeID int
)
As
Begin
Delete from Employee where EmployeeID=@EmployeeID;
End

