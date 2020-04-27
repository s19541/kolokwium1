using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Services
{
   public interface ITaskDbService
    {
         IEnumerable<Models.Task> GetTasks(int idProject);
        void addTask(Requests.TaskRequest request);
        bool inTaskType(string taskType);
    }
}
