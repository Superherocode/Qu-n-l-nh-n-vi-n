using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPWF
{
    //dùng xuyên suốt
    [Serializable]
    public class EMPLOYEE : EMPLOYEEBASE, TASK
    {
        private string position;
        public string Position
        {
            get { return position; }
            set { position = value; }
        }
        private string description;
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        private DateTime start;
        public DateTime Start
        {
            get { return start; }
            set { start = value; }
        }

    }
}
