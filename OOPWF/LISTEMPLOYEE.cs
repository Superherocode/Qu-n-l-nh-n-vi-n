using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPWF
{
    //dùng trong department
    public class EMPLOYEEUNDER : IENTITY//kiểu dữ liệu task
    {
        private string id;
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private string iddepartment;
        public string Iddepartment
        {
            get { return iddepartment; }
            set { iddepartment = value; }
        }
    }
    public interface LISTEMPLOYEE
    {

        //List<EMPLOYEEUNDERMANAGER> employees { get; set; }
        void AddEmployee(EMPLOYEEUNDER employee);//thêm nhân viên
        void RemoveEmployee(EMPLOYEEUNDER employee);//xóa nhân viên
    }
}
