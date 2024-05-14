using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPWF
{
    //dùng trong project
    public class EMPLOYEEIINPROJECT: EMPLOYEEUNDER, TASK
    {
        private string description;
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
    }
    public interface LISTEMPLOYEEINPROJECT
    {

        //List<EMPLOYEEUNDERMANAGER> employees { get; set; }
        void AddEmployee(EMPLOYEEIINPROJECT employee);//thêm nhân viên
        void RemoveEmployee(EMPLOYEEIINPROJECT employee);//xóa nhân viên
    }
}
