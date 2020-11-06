using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
namespace EmployeePayrollService
{
    public class EmployeeRepo
    {
        public static string connectionString = "Data Source=DESKTOP-QP0QMA4\\SQLEXPRESS;Initial Catalog=payroll_service;Integrated Security=True";
        SqlConnection connection;
        public void GetAllEmployee()
        {
            connection = new SqlConnection(connectionString);
            try
            {
                EmployeeModel employeeModel = new EmployeeModel();
                using (this.connection)
                {
                    string query = @"Select * from employee_payroll;";
                    SqlCommand cmd = new SqlCommand(query, this.connection);
                    this.connection.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            employeeModel.EmployeeID = dr.GetInt32(0);
                            employeeModel.EmployeeFirstName = dr.GetString(1);
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

                            Console.WriteLine(employeeModel.EmployeeID + " " + employeeModel.EmployeeFirstName + " " + employeeModel.BasicPay + " " + employeeModel.StartDate + " " + employeeModel.Gender + " " + employeeModel.PhoneNumber + " " + employeeModel.Address + " " + employeeModel.Department + " " + employeeModel.Deductions + " " + employeeModel.TaxablePay + " " + employeeModel.Tax + " " + employeeModel.NetPay);
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
        }

        public bool AddEmployee(EmployeeModel model)
        {
            connection = new SqlConnection(connectionString);
            try
            {
                using (this.connection)
                {                   
                    SqlCommand command = new SqlCommand("SpAddEmployeeDetails", this.connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@EmployeeName", model.EmployeeFirstName);
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

                    this.connection.Open();
                    var result = command.ExecuteNonQuery();
                    this.connection.Close();
                    if (result != 0)
                    {
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

        public bool UpdateSalary(string name, decimal salary)
        {
            connection = new SqlConnection(connectionString);
            try
            {
                using (this.connection)
                {
                    SqlCommand command = new SqlCommand("SpUpdateSalary", this.connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@EmployeeName", name);
                    command.Parameters.AddWithValue("@BasicPay", salary);

                    this.connection.Open();

                    var result = command.ExecuteNonQuery();
                    if(result != 0)
                    {
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

        public void GetEmployeesGivenDateRange(DateTime date1,DateTime date2)
        {
            connection = new SqlConnection(connectionString);
            try
            {
                EmployeeModel employeeModel = new EmployeeModel();
                SqlCommand command = new SqlCommand("SpGetEmployeesByStartDateRange", this.connection);
                command.CommandType =  System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@StartDate1", date1);
                command.Parameters.AddWithValue("@StartDate2", date2);
                this.connection.Open();

                SqlDataReader dr = command.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        employeeModel.EmployeeID = dr.GetInt32(0);
                        employeeModel.EmployeeFirstName = dr.GetString(1);
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

                        Console.WriteLine(employeeModel.EmployeeID + " " + employeeModel.EmployeeFirstName + " " + employeeModel.BasicPay + " " + employeeModel.StartDate + " " + employeeModel.Gender + " " + employeeModel.PhoneNumber + " " + employeeModel.Address + " " + employeeModel.Department + " " + employeeModel.Deductions + " " + employeeModel.TaxablePay + " " + employeeModel.Tax + " " + employeeModel.NetPay);
                        Console.WriteLine("\n");
                    }
                }
                else
                {
                    Console.WriteLine("No such records found");
                }
            }
            catch(Exception e)
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
