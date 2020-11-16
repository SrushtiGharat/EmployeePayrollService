﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;

namespace EmployeePayrollService
{
    public class EmployeeRepo
    {
        public static string connectionString = @"Data Source=DESKTOP-QP0QMA4\SQLEXPRESS;Initial Catalog=payroll_service;Integrated Security=True";
        SqlConnection connection;
        JSonServerOperations jSonServer = new JSonServerOperations();
        List<EmployeeModel> employeeList = new List<EmployeeModel>();

        /// <summary>
        /// Get list of employees from database
        /// </summary>
        public void GetAllEmployee()
        {
            connection = new SqlConnection(connectionString);
            try
            {
                EmployeeModel employeeModel = new EmployeeModel();
                using (this.connection)
                {
                    string query = @"select Employee.EId ,EName,BasicPay,StartDate,Gender,PhoneNo,Address,DeptName,Deduction,TaxablePay,IncomeTax,NetPay
                                     from Employee INNER JOIN Employee_Department ON Employee.EId = Employee_Department.EmpId
                                     INNER JOIN  Emp_Payroll ON Employee.EId = Emp_Payroll.EId
                                     INNER JOIN Department ON Department.DeptId = Employee_Department.DeptId;";

                    SqlCommand cmd = new SqlCommand(query, this.connection);
                    this.connection.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        employeeList.Clear();
                        while (dr.Read())
                        {
                            employeeModel.EmployeeID = dr.GetInt32(0);
                            employeeModel.EmployeeName = dr.GetString(1);
                            employeeModel.BasicPay = dr.GetDecimal(2);
                            employeeModel.StartDate = dr.GetDateTime(3);
                            employeeModel.Gender = Convert.ToChar(dr.GetString(4));
                            employeeModel.PhoneNumber = dr.GetString(5);
                            employeeModel.Address = dr.GetString(7);
                            employeeModel.Department = dr.GetString(6);
                            employeeModel.Deductions = dr.GetDecimal(8);
                            employeeModel.TaxablePay = dr.GetDecimal(9);
                            employeeModel.Tax = dr.GetDecimal(10);
                            employeeModel.NetPay = dr.GetDecimal(11);

                            employeeList.Add(employeeModel);                           

                            Console.WriteLine(employeeModel.EmployeeID + " " + employeeModel.EmployeeName + " " + employeeModel.BasicPay + " " + employeeModel.StartDate + " " + employeeModel.Gender + " " + employeeModel.PhoneNumber + " " + employeeModel.Address + " " + employeeModel.Department + " " + employeeModel.Deductions + " " + employeeModel.TaxablePay + " " + employeeModel.Tax + " " + employeeModel.NetPay);
                            Console.WriteLine("\n");
                        }
                    }
                    else
                    {
                        System.Console.WriteLine("No data found");
                    }
                }
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// Add multiple employees to database using threads
        /// </summary>
        /// <param name="empList"></param>
        /// <returns></returns>
        public int AddMultipleEmployeesUsingThread(List<EmployeeModel> empList)
        {
            int count = 0;
            empList.ForEach(employee =>
            {
                count++;
                Task task = new Task(() =>
                {
                    AddEmployee(employee);
                }
                );
                task.Start();
                task.Wait();
                //task.RunSynchronously();
            }
            );
            return count;
        }
        /// <summary>
        /// Add employee to database
        /// </summary>
        /// <param name="model"></param>
        /// <returns>true or false</returns>
        public bool AddEmployee(EmployeeModel model)
        {
            this.connection = new SqlConnection(connectionString);
            try
            {
                using (connection)
                {
                    SqlCommand command = new SqlCommand("SpAddEmployeeDetails",this.connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@EmployeeName", model.EmployeeName);
                    command.Parameters.AddWithValue("@PhoneNumber", model.PhoneNumber);
                    command.Parameters.AddWithValue("@Address", model.Address);
                    command.Parameters.AddWithValue("@Department", model.Department);
                    command.Parameters.AddWithValue("@Gender", model.Gender);
                    command.Parameters.AddWithValue("@BasicPay", model.BasicPay);
                    command.Parameters.AddWithValue("@Deductions", model.Deductions);
                    command.Parameters.AddWithValue("@TaxablePay", model.TaxablePay);
                    command.Parameters.AddWithValue("@Tax", model.Tax);
                    command.Parameters.AddWithValue("@NetPay", model.NetPay);
                    command.Parameters.AddWithValue("@StartDate", DateTime.Now);
                    command.Parameters.Add("@EmpId", SqlDbType.Int).Direction = ParameterDirection.Output;

                    this.connection.Open();
                    var result = command.ExecuteNonQuery();
                    this.connection.Close();
                    model.EmployeeID = Convert.ToInt32(command.Parameters["@EmpId"].Value);

                    if (result != 0)
                    {
                        employeeList.Add(model);
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                this.connection.Close();
            }
            return false;
        }
        //
        /// <summary>
        /// Update employee salary in database
        /// </summary>
        /// <param name="name"></param>
        /// <param name="salary"></param>
        /// <returns>true or false</returns>
        public bool UpdateSalary(int id, decimal salary)
        {
            connection = new SqlConnection(connectionString);
            try
            {
                using (this.connection)
                {
                    SqlCommand command = new SqlCommand("SpUpdateSalary", this.connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@EmpId", id);
                    command.Parameters.AddWithValue("@BasicPay", salary);

                    this.connection.Open();

                    var result = command.ExecuteNonQuery();
                    if (result != 0)
                    {
                        foreach (var employee in employeeList)
                        {
                            if (employee.EmployeeID.Equals(id))
                                employee.BasicPay = salary;
                        }
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                this.connection.Close();
            }
            return false;
        }

        /// <summary>
        /// Remove employee details from table
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool RemoveEmployee(int id)
        {
            connection = new SqlConnection(connectionString);
            try
            {
                using (connection)
                {
                    SqlCommand command = new SqlCommand("SpRemoveEmployeeData", this.connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@EmpId", id);

                    this.connection.Open();

                    var result = command.ExecuteNonQuery();
                    if (result != 0)
                    {
                        employeeList.RemoveAll(e => e.EmployeeID == id);
                        return true;
                    }
                    return false;

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                this.connection.Close();
            }
            return false;
        }

        /// <summary>
        /// Get employees given a range of start dates
        /// </summary>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        public void GetEmployeesGivenDateRange(DateTime date1, DateTime date2)
        {
            connection = new SqlConnection(connectionString);
            try
            {
                EmployeeModel employeeModel = new EmployeeModel();
                SqlCommand command = new SqlCommand("SpGetEmployeesByStartDateRange", this.connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@StartDate1", date1);
                command.Parameters.AddWithValue("@StartDate2", date2);
                this.connection.Open();

                SqlDataReader dr = command.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        employeeModel.EmployeeID = dr.GetInt32(0);
                        employeeModel.EmployeeName = dr.GetString(1);
                        employeeModel.BasicPay = dr.GetDecimal(2);
                        employeeModel.StartDate = dr.GetDateTime(3);
                        employeeModel.Gender = Convert.ToChar(dr.GetString(4));
                        employeeModel.PhoneNumber = dr.GetString(5);
                        employeeModel.Address = dr.GetString(7);
                        employeeModel.Department = dr.GetString(6);
                        employeeModel.Deductions = dr.GetDecimal(8);
                        employeeModel.TaxablePay = dr.GetDecimal(9);
                        employeeModel.Tax = dr.GetDecimal(10);
                        employeeModel.NetPay = dr.GetDecimal(11);

                        Console.WriteLine(employeeModel.EmployeeID + " " + employeeModel.EmployeeName + " " + employeeModel.BasicPay + " " + employeeModel.StartDate + " " + employeeModel.Gender + " " + employeeModel.PhoneNumber + " " + employeeModel.Address + " " + employeeModel.Department + " " + employeeModel.Deductions + " " + employeeModel.TaxablePay + " " + employeeModel.Tax + " " + employeeModel.NetPay);
                        Console.WriteLine("\n");
                    }
                }
                else
                {
                    Console.WriteLine("No such records found");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                this.connection.Close();
            }
        }

        /// <summary>
        /// Get aggregate salary details by gender
        /// </summary>
        public void GetAggregateSalaryDetailsByGender()
        {
            connection = new SqlConnection(connectionString);
            try
            {
                string query = @"select Gender,SUM(BasicPay),AVG(BasicPay), MIN(BasicPay),MAX(BasicPay),Count(Employee.EId) from Employee INNER JOIN Emp_Payroll 
                                  ON Employee.EId = Emp_Payroll.EId GROUP BY Gender";

                SqlCommand command = new SqlCommand(query, this.connection);
                this.connection.Open();
                SqlDataReader dr = command.ExecuteReader();
                if (dr.HasRows)
                {
                    Console.WriteLine("Gender\tSUM\tAVG\tMIN\tMAX\tCount");
                    while (dr.Read())
                    {
                        string gender = dr.GetString(0);
                        decimal SUM = dr.GetDecimal(1);
                        decimal AVG = dr.GetDecimal(2);
                        decimal MIN = dr.GetDecimal(3);
                        decimal MAX = dr.GetDecimal(4);
                        int Count = dr.GetInt32(5);
                        Console.WriteLine(gender + "\t" + SUM + "\t" + AVG + "\t" + MIN + "\t" + MAX + "\t" + Count);
                        Console.WriteLine("\n");
                    }
                }
                else
                {
                    Console.WriteLine("No such records found");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                this.connection.Close();
            }
        }
    }
}
