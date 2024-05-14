using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPWF
{
    public abstract class SALARYBASE: IENTITY
    {
        //1
        private string id;
        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        //2
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private string position;
        public string Position
        {
            get { return position; }
            set { position = value; }
        }
        private string iddepartment;
        public string Iddepartment
        {
            get { return iddepartment; }
            set { iddepartment = value; }
        }
        private double increase;
        public double Increase
        {
            get { return increase; }
            set
            {
                TimeSpan khoangcachnam = (DateTime.Today - Start);//Tính toán khoảng cách giữa hai ngày
                float time = (int)(khoangcachnam.TotalDays / 365);
                if (time < 2)
                {
                    increase = 0;
                }
                else if (time >= 2 && time < 5)
                {
                    increase = 2 / 100;
                }
                else if (time >= 5 && time < 10)
                {
                    increase = 5 / 100;
                }
                else if (time >= 10)
                {
                    increase = 10 / 100;
                }
            }
        }
        private double salary;
        public double Salary
        {
            get { return salary; }
            set
            {
                if (iddepartment == "NS")
                {
                    salary = 10000000;
                }
                else if (iddepartment == "MKT")
                {
                    salary = 12000000;
                }
                else if (iddepartment == "BV")
                {
                    salary = 8000000;

                }
                else if (iddepartment == "VC")
                {
                    salary = 9000000;
                }
                else if (iddepartment == "LC")
                {
                    salary = 8500000;
                }
                else if (iddepartment == "KT")
                {
                    salary = 11000000;
                }
                else if (iddepartment == "GS")
                {
                    salary = 9500000;
                }
            }
        }
        private double bonus;
        public double Bonus
        {
            get { return bonus; }
            set { bonus = value; }
        }
        private double dayoff;
        public double Dayoff
        {
            get { return dayoff; }
            set { dayoff = value; }
        }
        private DateTime start;
        public DateTime Start
        {
            get { return start; }
            set { start = value; }
        }
        private double total;
        public double Total
        {
            get { return total; }
            set { total = TotalSalary(); }
        }
        public abstract double TotalSalary();
        
    }
}
