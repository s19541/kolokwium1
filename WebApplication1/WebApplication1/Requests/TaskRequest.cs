using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Requests
{
    public class TaskRequest
    {
        public string name { get; set; }
        public string descritpion { get; set; }
        public DateTime deadLine { get; set; }
        public int idProject { get; set; }
        public int idAssignedTo { get; set; }
        public int idCreator { get; set; }
        public Models.TaskType taskType { get; set; }
    }
}
