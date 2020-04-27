using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Services
{
    public class SqlServerDbService:ITaskDbService
    {
        public IEnumerable<Models.Task> GetTasks(int idProject)
        {
            String connectionString = "Data Source = db-mssql;Initial Catalog=s19541;Integrated Security=True";
            var taskList = new List<Models.Task>();
            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand();
                command.Connection = connection;
                string commandText = "select Name,(select name from TaskType t where t.IdTaskType=tt.IdTaskType)\"TaskType\" from Task tt where IdProject=@id order by deadLine desc;";
                command.Parameters.AddWithValue("id", idProject);
                command.CommandText = commandText;
                connection.Open();
                try
                {
                    var executeReader = command.ExecuteReader();
                    while (executeReader.Read())
                    {
                        Models.Task task = new Models.Task();
                        task.name = executeReader["Name"].ToString();
                        task.taskType = executeReader["TaskType"].ToString();
                        taskList.Add(task);
                    }
                }
                catch (Exception e)
                {
                    return null;
                }

                connection.Close();
            }
            return taskList;
        }
        public void addTask(Requests.TaskRequest request)
        {
            var connection = new SqlConnection("Data Source=db-mssql;Initial Catalog=s19541;Integrated Security=True");
            connection.Open();
            var transaction = connection.BeginTransaction("myTransaction");
            
            if (!inTaskType(request.taskType.name))
            {
                using (var com = new SqlCommand())
                {
                    com.Connection = connection;
                    com.CommandText = "insert into TaskType values(@name)";
                    com.Parameters.AddWithValue("name", request.taskType.name);
                    com.Transaction = transaction;
                    com.ExecuteNonQuery();
                }
            }
            using (var com = new SqlCommand())
            {
                com.Connection = connection;
                com.CommandText = "insert into Task values(@name,@description,@deadLine,@idProject,(select IdTaskType from taskType where Name=@taskTypeName),@idAssignedTo,@idCreator)";
                com.Parameters.AddWithValue("name", request.name);
                com.Parameters.AddWithValue("description", request.descritpion);
                com.Parameters.AddWithValue("deadLine", request.deadLine);
                com.Parameters.AddWithValue("idProject", request.idProject);
                com.Parameters.AddWithValue("taskTypeName", request.taskType.name);
                com.Parameters.AddWithValue("idAssignedTo", request.idAssignedTo);
                com.Parameters.AddWithValue("idCreator", request.idCreator);
                com.Transaction = transaction;
                com.ExecuteNonQuery();
            }

            transaction.Commit();
            connection.Close();
            
        }
        public bool inTaskType(string taskType)
        {
            using (var connection = new SqlConnection("Data Source=db-mssql;Initial Catalog=s19541;Integrated Security=True"))
            using (var com = new SqlCommand())
            {
                com.Connection = connection;
                com.CommandText = "select * from TaskType";

                connection.Open();
                var dr = com.ExecuteReader();
                while (dr.Read())
                    if (taskType == dr["Name"].ToString())
                        return true;
                return false;
            }
        }
        
    }
}
