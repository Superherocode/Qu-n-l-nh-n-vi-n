using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPWF
{
    //dùng trong project
    [Serializable]
    public class PROJECT: IENTITY, TASK,LISTEMPLOYEEINPROJECT
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
        private string description;
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        private List<EMPLOYEEIINPROJECT> listemployees = new List<EMPLOYEEIINPROJECT>();
        public List<EMPLOYEEIINPROJECT> Listemployees
        {
            get { return listemployees; }
            set { listemployees = value; }
        }
        public void AddEmployee(EMPLOYEEIINPROJECT employee)//thêm 
        {
            listemployees.Add(employee);
        }
        public void RemoveEmployee(EMPLOYEEIINPROJECT employee)//xóa 
        {
            listemployees.Remove(employee);
        }
    }
}
