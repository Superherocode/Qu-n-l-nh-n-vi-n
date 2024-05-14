using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPWF
{
    //lớp abstract chứa những biến, thuộc tính mà cả nhân viên và quản lí cần
    public abstract class EMPLOYEEBASE : IENTITY
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
        
        private DateTime birth;
        public DateTime Birth
        {
            get { return birth; }
            set { birth = value; }
        }
        
        private string gmail;
        public string Gmail
        {
            get { return gmail; }
            set { gmail = value; }
        }
       
        private string phone;
        public string Phone
        {
            get { return phone; }
            set { phone = value; }
        }
       
        private string iddepartment;
        public string Iddepartment
        {
            get { return iddepartment; }
            set { iddepartment = value; }
        }
   
        private string gender;
        public string Gender
        {
            get { return gender; }
            set { gender = value; }
        }
    }
}
