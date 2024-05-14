using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Text.Json;
using System.IO;
using Newtonsoft.Json.Linq;

namespace OOPWF
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }
        public void LoadAll()//load dữ liệu
        {
            LoadDataFromJsonEmployee();
            LoadDataFromJsonEmployeeToDepartment();
            LoadDataFromJsonSalaryemployeesAndSalarymanagers();
        }
        private void Open_Click(object sender, EventArgs e)//nếu chạy thẳng LoadAll() thì khi file rỗng sẽ xảy ra lỗi, nên để Open này như 1 nút nguồn, vừa kiểm tra file, vừa báo lỗi nếu có lỗi
        {
            LoadAll();
            LoadDataFromJsonProject();
            ClearAll();
            ClearProject();
        }
        //1.TAB EMPLOYEE
        #region TABEMPLOYEE
        public void ClearAll()//mỗi lần cập nhật thì xóa các thông tin hiển thị
        {
            rbMaleEpl.Checked = false;
            rbEmployeeEpl.Checked = false;
            rbFemaleEpl.Checked = false;
            rbManagerEpl.Checked = false;
            tbIdEpl.Text = null;
            tbNameEpl.Text = null;
            tbGmailEpl.Text = null;
            dtpBirthEpl.Value = DateTime.Now;
            tbPhoneEpl.Text = null;
            cbIddepartmentEpl.Text = null;
            dtpStartEpl.Value = DateTime.Now;
            tbIdEpl.ReadOnly = false;
            tbNameEpl.ReadOnly = false;
        }
        public List<EMPLOYEE> employees = new List<EMPLOYEE>();//list này sử dụng xuyên suốt chương trình
        //hàm để đưa dữ liệu từ file json vào list employees và hiển thị vào dgv
        public void LoadDataFromJsonEmployee()//load lại dữ liệu để hiển thị đúng với dữ liệu hiện tại
        {
            string filePath = @"D:\Bài tập để nộp\Employees.txt";
            try
            {
                string json = File.ReadAllText(filePath);//đọc dữ liệu từ file
                List<EMPLOYEE> loadedEmployees = JsonSerializer.Deserialize<List<EMPLOYEE>>(json);//dùng kĩ thuật để đưa dữ liệu vào list employees
                employees.Clear(); // Xóa danh sách hiện tại trước khi thêm dữ liệu mới từ file JSON
                employees.AddRange(loadedEmployees); // Thêm dữ liệu từ file JSON vào danh sách hiện tại
                // Hiển thị dữ liệu từ danh sách mới trong DataGridView
                dgvEmployee.Rows.Clear(); // Xóa tất cả các hàng hiện có trong DataGridView trước khi thêm mới
                foreach (EMPLOYEE emp in employees)
                {
                    dgvEmployee.Rows.Add(emp.Id, emp.Name, emp.Gender, emp.Gmail, emp.Position, emp.Birth, emp.Phone, emp.Iddepartment, emp.Start);
                    cbIdsearchEpl.Items.Add(emp.Id);//combobox id ở employee
                    cbListemployeeMng.Items.Add(emp.Id);//combobox id ở manager and departmnet
                    cbIdemployeeProject.Items.Add(emp.Id);//combobox id ở project
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi đọc dữ liệu từ tập tin JSON: " + ex.Message);
            }
        }
        // Button Add
        private void btAddEpl_Click(object sender, EventArgs e)
        {
            int demid = 0;//biến để kiểm tra xem trong list có trùng id không
            int demmanager = 0;//biến để kiểm tra xem trong list có hơn 1 manager trong 1 phòng ban không
            if (!string.IsNullOrEmpty(tbIdEpl.Text) &&
                !string.IsNullOrEmpty(tbNameEpl.Text) &&
                !string.IsNullOrEmpty(tbGmailEpl.Text) &&
                !string.IsNullOrEmpty(tbPhoneEpl.Text) &&
                !string.IsNullOrEmpty(rbEmployeeEpl.Text) &&
                (cbIddepartmentEpl.Text == "NS" ||
                 cbIddepartmentEpl.Text == "GS" ||
                 cbIddepartmentEpl.Text == "BV" ||
                 cbIddepartmentEpl.Text == "VC" ||
                 cbIddepartmentEpl.Text == "KT" ||
                 cbIddepartmentEpl.Text == "MKT" ||
                 cbIddepartmentEpl.Text == "LC") &&
                (rbMaleEpl.Checked || rbFemaleEpl.Checked) &&
                (rbEmployeeEpl.Checked || rbManagerEpl.Checked))
            {// khi điền đầy đủ các ô, tiến hành kiểm tra xem có trùng id hoặc 1 phòng ban có hơn 1 manager không
                for (int i = 0; i < employees.Count; i++)
                {
                    if (tbIdEpl.Text == employees[i].Id)
                    {
                        demid++;
                    }
                    if (cbIddepartmentEpl.Text == employees[i].Iddepartment && rbManagerEpl.Checked == true && employees[i].Position == "Manager")
                    {
                        demmanager++;
                    }
                }
            }
            if (!string.IsNullOrEmpty(tbIdEpl.Text) &&
                !string.IsNullOrEmpty(tbNameEpl.Text) &&
                !string.IsNullOrEmpty(tbGmailEpl.Text) &&
                !string.IsNullOrEmpty(tbPhoneEpl.Text) &&
                !string.IsNullOrEmpty(rbEmployeeEpl.Text) &&
                (cbIddepartmentEpl.Text == "NS" ||
                 cbIddepartmentEpl.Text == "GS" ||
                 cbIddepartmentEpl.Text == "BV" ||
                 cbIddepartmentEpl.Text == "VC" ||
                 cbIddepartmentEpl.Text == "KT" ||
                 cbIddepartmentEpl.Text == "MKT" ||
                 cbIddepartmentEpl.Text == "LC") &&
                (rbMaleEpl.Checked || rbFemaleEpl.Checked) &&
                (rbEmployeeEpl.Checked || rbManagerEpl.Checked) &&
                demid == 0 &&
                demmanager == 0)
            {// khi không trùng id hoặc 1 phòng ban không quá 1 manager thì tiến hành thêm nhân viên
                if (rbMaleEpl.Checked)//là Male
                {
                    if (rbEmployeeEpl.Checked)//là Male và Employee
                    {
                        employees.Add(new EMPLOYEE()
                        {
                            Id = tbIdEpl.Text,
                            Name = tbNameEpl.Text.ToUpper(),
                            Gmail = tbGmailEpl.Text,
                            Gender = rbMaleEpl.Text,
                            Birth = dtpBirthEpl.Value,
                            Phone = tbPhoneEpl.Text,
                            Position = rbEmployeeEpl.Text,
                            Iddepartment = cbIddepartmentEpl.Text,
                            Start = dtpStartEpl.Value,
                        });
                    }
                    else//là Male và Manager
                    {
                        employees.Add(new EMPLOYEE()
                        {
                            Id = tbIdEpl.Text,
                            Name = tbNameEpl.Text.ToUpper(),
                            Gmail = tbGmailEpl.Text,
                            Gender = rbMaleEpl.Text,
                            Birth = dtpBirthEpl.Value,
                            Phone = tbPhoneEpl.Text,
                            Position = rbManagerEpl.Text,
                            Iddepartment = cbIddepartmentEpl.Text,
                            Start = dtpStartEpl.Value,
                        });
                    }
                }
                else//là Female
                {
                    if (rbEmployeeEpl.Checked)//là Female và Employee
                    {
                        employees.Add(new EMPLOYEE()
                        {
                            Id = tbIdEpl.Text,
                            Name = tbNameEpl.Text.ToUpper(),
                            Gmail = tbGmailEpl.Text,
                            Gender = rbFemaleEpl.Text,
                            Birth = dtpBirthEpl.Value,
                            Phone = tbPhoneEpl.Text,
                            Position = rbEmployeeEpl.Text,
                            Iddepartment = cbIddepartmentEpl.Text,
                            Start = dtpStartEpl.Value,
                        });
                    }
                    else//là Female và Manager
                    {
                        employees.Add(new EMPLOYEE()
                        {
                            Id = tbIdEpl.Text,
                            Name = tbNameEpl.Text.ToUpper(),
                            Gmail = tbGmailEpl.Text,
                            Gender = rbFemaleEpl.Text,
                            Birth = dtpBirthEpl.Value,
                            Phone = tbPhoneEpl.Text,
                            Position = rbManagerEpl.Text,
                            Iddepartment = cbIddepartmentEpl.Text,
                            Start = dtpStartEpl.Value,
                        });
                    }
                }
                string filePath = @"D:\Bài tập để nộp\Employees.txt";//đường link file json
                string json = JsonSerializer.Serialize(employees, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(filePath, json);
                LoadAll();
                ClearAll();
            }
            else
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin hoặc đúng thông tin !");
            }

        }
        //Button Remove
        private void btRemoveEpl_Click(object sender, EventArgs e)
        {
            if (dgvEmployee.SelectedRows.Count > 0 && dgvEmployee.SelectedRows[0].Cells["clIdEpl"].Value != null)// vế điều kiện sau nhằm fix bug khi xóa hàng sẵn của dgv
            {
                // Lấy chỉ mục của hàng được chọn
                int selectedindex = dgvEmployee.SelectedRows[0].Index;
                // lấy id của nhân viên
                string idremove = dgvEmployee.Rows[selectedindex].Cells[0].Value.ToString();
                // Xóa dữ liệu từ danh sách employees
                for (int i = 0; i < employees.Count; i++)
                {
                    if (employees[i].Id == idremove)
                    {
                        employees.RemoveAt(i);
                        i--; // Giảm chỉ số để không bỏ qua phần tử sau khi xóa tránh lỗi "quá độ dài của list"
                    }
                }
                // Cập nhật lại tệp JSON
                string filePath = @"D:\Bài tập để nộp\Employees.txt";
                string json = JsonSerializer.Serialize(employees, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(filePath, json);
                LoadAll();
                ClearAll();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một hàng để xóa.");
            }
        }
        private void brClearEpl_Click(object sender, EventArgs e)
        {
            ClearAll();
            tbIdEpl.ReadOnly = false;
            tbNameEpl.ReadOnly = false;
        }
        // Chọn 1 hàng dữ liệu
        private void dgvEmployee_SelectionChanged(object sender, EventArgs e) //Chọn 1 hàng để sửa thông tin
                                                                              //event này dở 1 cái là khi click 1 lần ở ô n, sau khi sửa thông tin ở các textbox và click lại ô n đó thì nó không load lại
        {
            ClearAll();
            //lấy vị trí của hàng cần sửa
            if (dgvEmployee.SelectedRows.Count > 0 && dgvEmployee.SelectedRows[0].Cells["clIdEpl"].Value != null)// vế điều kiện sau nhằm fix bug khi chọn hàng sẵn của dgv
            {
                int selectindex = dgvEmployee.SelectedRows[0].Index;//vị trí hàng
                //đưa dữ liệu từ dgv vào bảng điền thông tin để sửa
                if (dgvEmployee.Rows[selectindex].Cells["clGenderEpl"].Value.ToString() == "Male")//là Male
                {
                    if (dgvEmployee.Rows[selectindex].Cells["clPositionEpl"].Value.ToString() == "Employee")//là Male và Employee
                    {
                        rbMaleEpl.Checked = true;
                        rbEmployeeEpl.Checked = true;
                    }
                    else//là Male và Manager
                    {
                        rbMaleEpl.Checked = true;
                        rbManagerEpl.Checked = true;
                    }
                }
                else//là Female
                {
                    if (dgvEmployee.Rows[selectindex].Cells["clPositionEpl"].Value.ToString() == "Employee")//là Female và Employee
                    {
                        rbFemaleEpl.Checked = true;
                        rbEmployeeEpl.Checked = true;
                    }
                    else//là Female và Manager
                    {
                        rbFemaleEpl.Checked = true;
                        rbManagerEpl.Checked = true;
                    }
                }
                tbIdEpl.Text = dgvEmployee.Rows[selectindex].Cells["clIdEpl"].Value.ToString();
                tbNameEpl.Text = dgvEmployee.Rows[selectindex].Cells["clNameEpl"].Value.ToString();
                tbGmailEpl.Text = dgvEmployee.Rows[selectindex].Cells["clGmailEpl"].Value.ToString();
                string rbBirth = dgvEmployee.Rows[selectindex].Cells["clBirthEpl"].Value.ToString();
                DateTime rbBirthValue;
                if (DateTime.TryParse(rbBirth, out rbBirthValue))
                {
                    dtpBirthEpl.Value = rbBirthValue;
                }
                tbPhoneEpl.Text = dgvEmployee.Rows[selectindex].Cells["clPhoneEpl"].Value.ToString();
                cbIddepartmentEpl.Text = dgvEmployee.Rows[selectindex].Cells["clIddepartmentEpl"].Value.ToString();
                string rbStart = dgvEmployee.Rows[selectindex].Cells["clStartEpl"].Value.ToString();
                DateTime rbStartValue;
                if (DateTime.TryParse(rbStart, out rbStartValue))
                {
                    dtpStartEpl.Value = rbStartValue;
                }
            }
            tbIdEpl.ReadOnly = true;//cố định, muốn sửa thì phải xóa đi add lại
            tbNameEpl.ReadOnly = true;//cố định, muốn sửa thì phải xóa đi add lại
        }

        //Button Change
        private void btChangeEpl_Click(object sender, EventArgs e)
        {
            int demmanager = 0;//biến để kiểm tra xem trong list có hơn 1 manager trong 1 phòng ban không
            if (!string.IsNullOrEmpty(tbIdEpl.Text) &&
                !string.IsNullOrEmpty(tbNameEpl.Text) &&
                !string.IsNullOrEmpty(tbGmailEpl.Text) &&
                !string.IsNullOrEmpty(tbPhoneEpl.Text) &&
                !string.IsNullOrEmpty(rbEmployeeEpl.Text) &&
                (cbIddepartmentEpl.Text == "NS" ||
                 cbIddepartmentEpl.Text == "GS" ||
                 cbIddepartmentEpl.Text == "BV" ||
                 cbIddepartmentEpl.Text == "VC" ||
                 cbIddepartmentEpl.Text == "KT" ||
                 cbIddepartmentEpl.Text == "MKT" ||
                 cbIddepartmentEpl.Text == "LC") &&
                (rbMaleEpl.Checked || rbFemaleEpl.Checked) &&
                (rbEmployeeEpl.Checked || rbManagerEpl.Checked))
            {// khi điền đầy đủ các ô, tiến hành kiểm tra xem có trùng id hoặc 1 phòng ban có hơn 1 manager không
                for (int i = 0; i < employees.Count; i++)
                {
                    if (cbIddepartmentEpl.Text == employees[i].Iddepartment && rbManagerEpl.Checked == true && employees[i].Position == "Manager")
                    {
                        demmanager++;
                    }
                }
            }
            if (dgvEmployee.SelectedRows.Count > 0)
            {
                if (!string.IsNullOrEmpty(tbIdEpl.Text) &&
                    !string.IsNullOrEmpty(tbNameEpl.Text) &&
                    !string.IsNullOrEmpty(tbGmailEpl.Text) &&
                    !string.IsNullOrEmpty(tbPhoneEpl.Text) &&
                    !string.IsNullOrEmpty(rbEmployeeEpl.Text) &&
                    (cbIddepartmentEpl.Text == "NS" ||
                     cbIddepartmentEpl.Text == "GS" ||
                     cbIddepartmentEpl.Text == "BV" ||
                     cbIddepartmentEpl.Text == "VC" ||
                     cbIddepartmentEpl.Text == "KT" ||
                     cbIddepartmentEpl.Text == "MKT" ||
                     cbIddepartmentEpl.Text == "LC") &&
                    (rbMaleEpl.Checked || rbFemaleEpl.Checked) &&
                    (rbEmployeeEpl.Checked || rbManagerEpl.Checked) &&
                    demmanager == 0)
                {
                    // Xóa dữ liệu từ danh sách employees
                    for (int i = 0; i < employees.Count; i++)
                    {
                        if (employees[i].Id == tbIdEpl.Text)
                        {
                            employees.RemoveAt(i);
                            i--; // Giảm chỉ số để không bỏ qua phần tử sau khi xóa
                            if (rbMaleEpl.Checked)//là Male
                            {
                                if (rbEmployeeEpl.Checked)//là Male và Employee
                                {
                                    employees.Add(new EMPLOYEE()
                                    {
                                        Id = tbIdEpl.Text,
                                        Name = tbNameEpl.Text.ToUpper(),
                                        Gmail = tbGmailEpl.Text,
                                        Gender = rbMaleEpl.Text,
                                        Birth = dtpBirthEpl.Value,
                                        Phone = tbPhoneEpl.Text,
                                        Position = rbEmployeeEpl.Text,
                                        Iddepartment = cbIddepartmentEpl.Text,
                                        Start = dtpStartEpl.Value,
                                    });
                                }
                                else//là Male và Manager
                                {
                                    employees.Add(new EMPLOYEE()
                                    {
                                        Id = tbIdEpl.Text,
                                        Name = tbNameEpl.Text.ToUpper(),
                                        Gmail = tbGmailEpl.Text,
                                        Gender = rbMaleEpl.Text,
                                        Birth = dtpBirthEpl.Value,
                                        Phone = tbPhoneEpl.Text,
                                        Position = rbManagerEpl.Text,
                                        Iddepartment = cbIddepartmentEpl.Text,
                                        Start = dtpStartEpl.Value,
                                    });
                                }
                            }
                            else//là Female
                            {
                                if (rbEmployeeEpl.Checked)//là Female và Employee
                                {
                                    employees.Add(new EMPLOYEE()
                                    {
                                        Id = tbIdEpl.Text,
                                        Name = tbNameEpl.Text.ToUpper(),
                                        Gmail = tbGmailEpl.Text,
                                        Gender = rbFemaleEpl.Text,
                                        Birth = dtpBirthEpl.Value,
                                        Phone = tbPhoneEpl.Text,
                                        Position = rbEmployeeEpl.Text,
                                        Iddepartment = cbIddepartmentEpl.Text,
                                        Start = dtpStartEpl.Value,
                                    });
                                }
                                else//là Female và Manager
                                {
                                    employees.Add(new EMPLOYEE()
                                    {
                                        Id = tbIdEpl.Text,
                                        Name = tbNameEpl.Text.ToUpper(),
                                        Gmail = tbGmailEpl.Text,
                                        Gender = rbFemaleEpl.Text,
                                        Birth = dtpBirthEpl.Value,
                                        Phone = tbPhoneEpl.Text,
                                        Position = rbManagerEpl.Text,
                                        Iddepartment = cbIddepartmentEpl.Text,
                                        Start = dtpStartEpl.Value,
                                    });
                                }
                            }
                            string filePath = @"D:\Bài tập để nộp\Employees.txt";//đường link file json
                            string json = JsonSerializer.Serialize(employees, new JsonSerializerOptions { WriteIndented = true });
                            File.WriteAllText(filePath, json);
                            LoadAll();
                            ClearAll();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng điền đầy đủ thông tin hoặc đúng thông tin !");
                }
                LoadAll();
            }
        }

        // Button Search Mannager
        private void btManagerEpl_Click(object sender, EventArgs e)//tìm trong employees position = manager
        {
            dgvEmployee.Rows.Clear();
            foreach (EMPLOYEE emp in employees)
            {
                if (emp.Position == "Manager")
                {
                    dgvEmployee.Rows.Add(emp.Id, emp.Name, emp.Gender, emp.Gmail, emp.Position, emp.Birth, emp.Phone, emp.Iddepartment);
                }
            }
            ClearAll();
        }
        private void btEmployeeEpl_Click(object sender, EventArgs e)
        {
            dgvEmployee.Rows.Clear();
            foreach (EMPLOYEE emp in employees)
            {
                if (emp.Position == "Employee")
                {
                    dgvEmployee.Rows.Add(emp.Id, emp.Name, emp.Gender, emp.Gmail, emp.Position, emp.Birth, emp.Phone, emp.Iddepartment, emp.Start);
                }
            }
            ClearAll();
        }
        // Button Search Id Department = null (sau khi sửa ở tab department)
        private void btIddepartmentnullEpl_Click(object sender, EventArgs e)
        {
            dgvEmployee.Rows.Clear();
            foreach (EMPLOYEE emp in employees)
            {
                if (emp.Id == "null")
                {
                    dgvEmployee.Rows.Add(emp.Id, emp.Name, emp.Gender, emp.Gmail, emp.Position, emp.Birth, emp.Phone, emp.Iddepartment, emp.Start);
                }
            }
            ClearAll();
        }
        // Button Search theo Id
        private void btSearchEpl_Click(object sender, EventArgs e)
        {
            dgvEmployee.Rows.Clear();
            foreach (EMPLOYEE emp in employees)
            {
                if (emp.Id == cbIdsearchEpl.Text)
                {
                    dgvEmployee.Rows.Add(emp.Id, emp.Name, emp.Gender, emp.Gmail, emp.Position, emp.Birth, emp.Phone, emp.Iddepartment, emp.Start);
                }
            }
            ClearAll();
        }
        // Button hiển thị All
        private void btAllEpl_Click(object sender, EventArgs e)
        {
            LoadAll();
            ClearAll();
        }

        #endregion


        //2.TAB DEPARTMENT AND MANAGER
        #region TABDEPARTMENTANDMANAGER

        public DEPARTMENT nhansu = new DEPARTMENT() { Id = "NS", Name = "Nhân sự" };
        public DEPARTMENT ketoan = new DEPARTMENT() { Id = "KT", Name = "Kế toán" };
        public DEPARTMENT marketing = new DEPARTMENT() { Id = "MKT", Name = "Marketing" };
        public DEPARTMENT baove = new DEPARTMENT() { Id = "BV", Name = "Bảo vệ" };
        public DEPARTMENT laocong = new DEPARTMENT() { Id = "LC", Name = "Lao công" };
        public DEPARTMENT vanchuyen = new DEPARTMENT() { Id = "VC", Name = "Vận chuyển" };
        public DEPARTMENT giamsat = new DEPARTMENT() { Id = "GS", Name = "Giám sát" };
        //
        public List<EMPLOYEEUNDER> employeeundertemp = new List<EMPLOYEEUNDER>();
        // hàm xóa dữ liệu listemployee trong 1 phòng ban
        public void DeleteList(DEPARTMENT department)
        {
            department.Listemployees.Clear();
        }
        // hàm lấy dữ liệu từ employees
        public void LoadDataFromJsonEmployeeToDepartment()// ý tưởng là lấy dữ liệu từ danh sách employees (vì có cả employee à manager), kiểm tra coi nào là position thì thêm vô list managers rồi deserialize ngược lại vào file managers
        {
            //trước khi đưa dữ liệu vào các phòng ban, cần đảm bảo list employee đang rỗng
            DeleteList(nhansu);
            DeleteList(ketoan);
            DeleteList(giamsat);
            DeleteList(vanchuyen);
            DeleteList(baove);
            DeleteList(laocong);
            DeleteList(marketing);
            string filePath = @"D:\Bài tập để nộp\Employees.txt";
            try
            {
                string json = File.ReadAllText(filePath);
                // Phân tích dữ liệu JSON để lấy ra chỉ những bản ghi có iddepartment phù hơp
                JArray jsonArray = JArray.Parse(json);//kiểu dữ liệu JArray à một lớp trong thư viện Newtonsoft.Json, để phân tích dữ liệu JSON từ file và truy cập vào từng phần tử trong mảng JSON
                foreach (JObject obj in jsonArray)
                {
                    // NS
                    if (obj["Iddepartment"].ToString() == "NS")
                    {
                        if (obj["Position"].ToString() == "Manager")
                        {
                            nhansu.Manager = obj.ToObject<MANAGER>();
                        }
                        else
                        {
                            // Deserialize đối tượng từ bản ghi JSON
                            EMPLOYEEUNDER employee = obj.ToObject<EMPLOYEEUNDER>();
                            nhansu.AddEmployee(employee);
                        }
                    }
                    // KT
                    else if (obj["Iddepartment"].ToString() == "KT")
                    {
                        if (obj["Position"].ToString() == "Manager")
                        {
                            ketoan.Manager = obj.ToObject<MANAGER>();
                        }
                        else
                        {
                            EMPLOYEEUNDER employee = obj.ToObject<EMPLOYEEUNDER>();
                            ketoan.AddEmployee(employee);
                        }
                    }
                    // MKT
                    else if (obj["Iddepartment"].ToString() == "MKT")
                    {
                        if (obj["Position"].ToString() == "Manager")
                        {
                            marketing.Manager = obj.ToObject<MANAGER>();
                        }
                        else
                        {
                            EMPLOYEEUNDER employee = obj.ToObject<EMPLOYEEUNDER>();
                            marketing.AddEmployee(employee);
                        }
                    }
                    // BV
                    else if (obj["Iddepartment"].ToString() == "BV")
                    {
                        if (obj["Position"].ToString() == "Manager")
                        {
                            baove.Manager = obj.ToObject<MANAGER>();
                        }
                        else
                        {
                            EMPLOYEEUNDER employee = obj.ToObject<EMPLOYEEUNDER>();
                            baove.AddEmployee(employee);
                        }
                    }
                    // LC
                    else if (obj["Iddepartment"].ToString() == "LC")
                    {
                        if (obj["Position"].ToString() == "Manager")
                        {
                            laocong.Manager = obj.ToObject<MANAGER>();
                        }
                        else
                        {
                            EMPLOYEEUNDER employee = obj.ToObject<EMPLOYEEUNDER>();
                            laocong.AddEmployee(employee);
                        }
                    }
                    // VC
                    else if (obj["Iddepartment"].ToString() == "VC")
                    {
                        if (obj["Position"].ToString() == "Manager")
                        {
                            vanchuyen.Manager = obj.ToObject<MANAGER>();
                        }
                        else
                        {
                            EMPLOYEEUNDER employee = obj.ToObject<EMPLOYEEUNDER>();
                            vanchuyen.AddEmployee(employee);
                        }
                    }
                    // GS
                    else if (obj["Iddepartment"].ToString() == "GS")
                    {
                        if (obj["Position"].ToString() == "Manager")
                        {
                            giamsat.Manager = obj.ToObject<MANAGER>();
                        }
                        else
                        {
                            EMPLOYEEUNDER employee = obj.ToObject<EMPLOYEEUNDER>();
                            giamsat.AddEmployee(employee);
                        }
                    }
                }
                string filePath2 = @"D:\Bài tập để nộp\Nhansu.txt";//đường link file json 
                string json2 = JsonSerializer.Serialize(nhansu, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(filePath2, json2);
                string filePath3 = @"D:\Bài tập để nộp\Ketoan.txt";//đường link file json 
                string json3 = JsonSerializer.Serialize(ketoan, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(filePath3, json3);
                string filePath4 = @"D:\Bài tập để nộp\Laocong.txt";//đường link file json 
                string json4 = JsonSerializer.Serialize(laocong, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(filePath4, json4);
                string filePath5 = @"D:\Bài tập để nộp\Baove.txt";//đường link file json 
                string json5 = JsonSerializer.Serialize(baove, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(filePath5, json5);
                string filePath6 = @"D:\Bài tập để nộp\Marketing.txt";//đường link file json 
                string json6 = JsonSerializer.Serialize(marketing, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(filePath6, json6);
                string filePath7 = @"D:\Bài tập để nộp\Giamsat.txt";//đường link file json 
                string json7 = JsonSerializer.Serialize(giamsat, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(filePath7, json7);
                string filePath8 = @"D:\Bài tập để nộp\Vanchuyen.txt";//đường link file json 
                string json8 = JsonSerializer.Serialize(vanchuyen, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(filePath8, json8);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi đọc dữ liệu từ tập tin JSON: " + ex.Message);
            }
        }
        //hàm hiển thị dữ liệu
        public void ViewData(DEPARTMENT ketoan)// hiển thị dữ liệu vào các bảng(:))) do viết từ chỗ kế toán nên lười chỉnh, để luôn là ketoan)
        {
            employeeundertemp.Clear();//khi chuyển department thì cái list chứa employee trong department clear
            lbListemployeeMng.Items.Clear();//xóa dữ liệu hiển thị trước khi hiển thị dữ liệu mới
            if (ketoan.Manager != null)//kiểm tra coi phòng ban có manager không, có thì hiển thị thông tin, không thì không hiển thị
            {
                if (ketoan.Manager.Gender == "Male")//là Male
                {
                    rbMaleMng.Checked = true;
                }
                else if (ketoan.Manager.Gender == "Female")//là Female
                {
                    rbFemaleMng.Checked = true;
                }
                tbIdMng.Text = ketoan.Manager.Id.ToString();
                tbNameMng.Text = ketoan.Manager.Name.ToString();
                tbGmailMng.Text = ketoan.Manager.Gmail.ToString();
                string rbBirth = ketoan.Manager.Birth.ToString();
                DateTime rbBirthValue;
                if (DateTime.TryParse(rbBirth, out rbBirthValue))
                {
                    dtpBirthMng.Value = rbBirthValue;
                }
                tbPhoneMng.Text = ketoan.Manager.Phone.ToString();
                tbIddepartmentMng.Text = ketoan.Manager.Iddepartment.ToString();
                tbNamedepartmentDpm.Text = ketoan.Name.ToString();
            }
            foreach (EMPLOYEEUNDER employeeunder in ketoan.Listemployees)
            {
                lbListemployeeMng.Items.Add(employeeunder.Id + "." + employeeunder.Name + "\n");
            }
        }
        // Button Kế toán
        private void btKetoanDpm_Click(object sender, EventArgs e)
        {
            ViewData(ketoan);
        }
        // Button vận chuyển
        private void btVanchuyenDpm_Click(object sender, EventArgs e)
        {
            ViewData(vanchuyen);
        }
        // Button Nhân sự
        private void btNhansuDpm_Click(object sender, EventArgs e)
        {
            ViewData(nhansu);
        }
        // Button lao công
        private void btLaocongDpm_Click(object sender, EventArgs e)
        {
            ViewData(laocong);
        }
        // Button Marketing
        private void btMarketingDpm_Click(object sender, EventArgs e)
        {
            ViewData(marketing);
        }
        // Button Bảo vệ
        private void btBaoveDpm_Click(object sender, EventArgs e)
        {
            ViewData(baove);
        }
        // Button Giám sát
        private void btGiamsatDpm_Click(object sender, EventArgs e)
        {
            ViewData(giamsat);
        }
        // hàm thêm nhân viên vào phòng ban và hiển thị tạm thời
        public void AddEmployee(DEPARTMENT department)
        {
            lbListemployeeMng.Items.Clear();
            if (employeeundertemp.Count != 0)// trường hợp đầu tiên lúc mới khởi tạo list
            {
                if (employeeundertemp[0].Iddepartment != tbIddepartmentMng.Text)//nếu nhảy sang department khác thì mới reload, còn không cứ giữ để add tiếp
                {
                    for (int i = 0; i < department.Listemployees.Count; i++)//không dùng kiểu employeeundertemp=department.Listemployees vì nó sẽ thành 1 list có 2 cách gọi tên. lưu ý
                    {
                        employeeundertemp.Add(department.Listemployees[i]);
                    }
                }
            }
            else
            {
                for (int i = 0; i < department.Listemployees.Count; i++)//không dùng kiểu employeeundertemp=department.Listemployees vì nó sẽ thành 1 list có 2 cách gọi tên. lưu ý
                {
                    employeeundertemp.Add(department.Listemployees[i]);
                }
            }
            int demid = 0;//biến kiểm tra xem id employee định add vào department có tồn tại chưa
            for (int i = 0; i < employeeundertemp.Count; i++)
            {
                if (employeeundertemp[i].Id == cbListemployeeMng.Text)
                {
                    demid++;
                }
            }
            for (int i = 0; i < employees.Count; i++)
            {
                if (cbListemployeeMng.Text == employees[i].Id && demid == 0 && employees[i].Position == "Employee")//không được add manager 
                {
                    EMPLOYEEUNDER newemployee = new EMPLOYEEUNDER();
                    newemployee.Id = employees[i].Id;
                    newemployee.Name = employees[i].Name;
                    newemployee.Iddepartment = tbIddepartmentMng.Text;
                    employeeundertemp.Add(newemployee);
                }
            }
            foreach (EMPLOYEEUNDER employeeunder in employeeundertemp)
            {

                lbListemployeeMng.Items.Add(employeeunder.Id + "." + employeeunder.Name + "\n");

            }
        }
        // Button Add nhân viên (tạm thời)
        private void btAddMng_Click(object sender, EventArgs e)//thêm nhân viên vào phòng ban (chỉ mới thêm ở list trong quá trình xử lí, chưa vào file chính, nghĩa là chưa lưu)
        {
            if (tbIddepartmentMng.Text == "NS")
            {
                AddEmployee(nhansu);
            }
            if (tbIddepartmentMng.Text == "GS")
            {
                AddEmployee(giamsat);
            }
            if (tbIddepartmentMng.Text == "VC")
            {
                AddEmployee(vanchuyen);
            }
            if (tbIddepartmentMng.Text == "KT")
            {
                AddEmployee(ketoan);
            }
            if (tbIddepartmentMng.Text == "LC")
            {
                AddEmployee(laocong);
            }
            if (tbIddepartmentMng.Text == "BV")
            {
                AddEmployee(baove);
            }
            if (tbIddepartmentMng.Text == "MRT")
            {
                AddEmployee(marketing);
            }
        }
        // hàm xóa nhân viên trong phòng ban và hiển thị tạm thời
        public void RemoveEmployee(DEPARTMENT department)//
        {
            lbListemployeeMng.Items.Clear();
            if (employeeundertemp.Count != 0)// trường hợp đầu tiên lúc mới khởi tạo list
            {
                if (employeeundertemp[0].Iddepartment != tbIddepartmentMng.Text)//nếu nhảy sang department khác thì mới reload, còn không cứ giữ để add tiếp
                {
                    for (int i = 0; i < department.Listemployees.Count; i++)//không dùng kiểu employeeundertemp=department.Listemployees vì nó sẽ thành 1 list có 2 cách gọi tên. lưu ý
                    {
                        employeeundertemp.Add(department.Listemployees[i]);
                    }
                }
            }
            else
            {
                for (int i = 0; i < department.Listemployees.Count; i++)//không dùng kiểu employeeundertemp=department.Listemployees vì nó sẽ thành 1 list có 2 cách gọi tên. lưu ý
                {
                    employeeundertemp.Add(department.Listemployees[i]);
                }
            }
            for (int i = 0; i < employeeundertemp.Count; i++)
            {
                if (cbListemployeeMng.Text == employeeundertemp[i].Id)
                {
                    employeeundertemp.RemoveAt(i);
                    i--;//tránh lỗi out of index
                }
            }
            foreach (EMPLOYEEUNDER employeeunder in employeeundertemp)
            {

                lbListemployeeMng.Items.Add(employeeunder.Id + "." + employeeunder.Name + "\n");

            }
        }
        // Button Remove nhân viên
        private void btRemoveMng_Click(object sender, EventArgs e)//xóa nhân viên vào phòng ban (chỉ mới xóa ở list trong quá trình xử lí, chưa vào file chính, nghĩa là chưa lưu)
        {
            if (tbIddepartmentMng.Text == "NS")
            {
                RemoveEmployee(nhansu);
            }
            if (tbIddepartmentMng.Text == "GS")
            {
                RemoveEmployee(giamsat);
            }
            if (tbIddepartmentMng.Text == "VC")
            {
                RemoveEmployee(vanchuyen);
            }
            if (tbIddepartmentMng.Text == "KT")
            {
                RemoveEmployee(ketoan);
            }
            if (tbIddepartmentMng.Text == "LC")
            {
                RemoveEmployee(laocong);
            }
            if (tbIddepartmentMng.Text == "BV")
            {
                RemoveEmployee(baove);
            }
            if (tbIddepartmentMng.Text == "MRT")
            {
                RemoveEmployee(marketing);
            }
        }
        // hàm cập nhật lại dữ liệu
        public void ReloadDataFromDepartment(DEPARTMENT department)//cập nhật lại id department vào file employees (cập nhật chính thức)
        {
            for (int z = 0; z < department.Listemployees.Count; z++)//kiểm tra xem list thực tế có chứa employee đã remove không
            {
                int dem = 0;
                for (int i = 0; i < employeeundertemp.Count; i++)//còn tồn tại thì ++
                {
                    if (department.Listemployees[z].Id == employeeundertemp[i].Id)
                    {
                        dem++;
                    }
                }
                if (dem == 0)//không tồn tại thì iddepartment = null
                {
                    for (int j = 0; j < employees.Count; j++)
                    {
                        if (employees[j].Id == department.Listemployees[z].Id)
                        {
                            employees[j].Iddepartment = "null";
                        }
                    }
                }
            }
            for (int i = 0; i < employeeundertemp.Count; i++)//cập nhật lại các employee trong list tạm vào list thực tế
            {
                for (int j = 0; j < employees.Count; j++)
                {
                    if (employees[j].Id == employeeundertemp[i].Id)
                    {
                        employees[j].Iddepartment = employeeundertemp[i].Iddepartment;
                    }
                }
            }
        }
        //Button change (lúc này mới lưu vào dữ liệu chính). nghĩa là đã xác nhận sửa dữ liệu
        private void btChangeMng_Click(object sender, EventArgs e)
        {
            if (tbIddepartmentMng.Text == "NS")
            {
                ReloadDataFromDepartment(nhansu);
            }
            if (tbIddepartmentMng.Text == "GS")
            {
                ReloadDataFromDepartment(giamsat);
            }
            if (tbIddepartmentMng.Text == "LC")
            {
                ReloadDataFromDepartment(laocong);
            }
            if (tbIddepartmentMng.Text == "BV")
            {
                ReloadDataFromDepartment(baove);
            }
            if (tbIddepartmentMng.Text == "VC")
            {
                ReloadDataFromDepartment(vanchuyen);
            }
            if (tbIddepartmentMng.Text == "MKT")
            {
                ReloadDataFromDepartment(marketing);
            }
            if (tbIddepartmentMng.Text == "KT")
            {
                ReloadDataFromDepartment(ketoan);
            }
            //lưu lại thông tin nhân viên vì có thể đã chỉnh sửa phòng ban
            string filePath = @"D:\Bài tập để nộp\Employees.txt";//đường link file json
            string json = JsonSerializer.Serialize(employees, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
            LoadAll();
        }
        #endregion


        //3. TAB PROJECT
        #region TABPROJECT
        public List<PROJECT> projects = new List<PROJECT>();
        public List<EMPLOYEEIINPROJECT> listemployeeinproject = new List<EMPLOYEEIINPROJECT>();//list tạm dùng để lưu thông tin danh sách nhân viên chưa được admin chính thức add vô project nào nên khi dùng nhớ clear trước
        //hàm load lại dữ liệu
        public void LoadDataFromJsonProject()//load lại dữ liệu để hiển thị đúng với dữ liệu hiện tại
        {
            ClearProject();
            try
            {
                string filePath = @"D:\Bài tập để nộp\Projects.txt";
                if (File.Exists(filePath))
                {
                    string json = File.ReadAllText(filePath);//đọc dữ liệu từ file
                    List<PROJECT> loadedProjects = JsonSerializer.Deserialize<List<PROJECT>>(json);//dùng kĩ thuật để đưa dữ liệu vào list employees
                    projects.Clear(); // Xóa danh sách hiện tại trước khi thêm dữ liệu mới từ file JSON
                    projects.AddRange(loadedProjects); // Thêm dữ liệu từ file JSON vào danh sách hiện tại
                                                       // Hiển thị dữ liệu từ danh sách mới trong DataGridView
                    dgvPrj.Rows.Clear(); // Xóa tất cả các hàng hiện có trong DataGridView trước khi thêm mới
                    foreach (PROJECT project in projects)
                    {
                        dgvPrj.Rows.Add(project.Id, project.Name, project.Description);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi khi đọc dữ liệu từ tập tin JSON ở Project: " + ex.Message);
            }
        }
        //Button Add nhân viên vào dự án (tạm thời)
        private void btAddemployeeprojectPrj_Click(object sender, EventArgs e)//thêm nhân viên vào list nhân viên (tạm thời, chưa lưu chính thức)
        {
            int demid = 0;
            for (int i = 0; i < lbListemployeesPrj.Items.Count; i++)
            {
                if (lbListemployeesPrj.Items[i].ToString() == cbIdemployeeProject.Text)
                {
                    demid++;
                }
            }
            for (int i = 0; i < employees.Count; i++)
            {
                if (listemployeeinproject.Count == 0 && cbIdemployeeProject.Text == employees[i].Id)//nếu ban đầu chưa có phần tử nào thì không thực hiện so sánh coi có tồn tại id hay chưa
                {
                    listemployeeinproject.Add(new EMPLOYEEIINPROJECT()
                    {
                        Id = cbIdemployeeProject.Text,
                        Name = tbNamePrj.Text.ToString(),
                        Iddepartment = tbNamePrj.Text,
                        Description = rtbTaskPrj.Text,
                    });
                }
                else if (listemployeeinproject.Count != 0 && cbIdemployeeProject.Text == employees[i].Id)// nếu list > 0 phần tử
                {
                    if (demid == 0)//xem coi có trùng id không
                    {
                        listemployeeinproject.Add(new EMPLOYEEIINPROJECT()
                        {
                            Id = cbIdemployeeProject.Text,
                            Name = tbNamePrj.Text,
                            Iddepartment = tbNamePrj.Text,
                            Description = rtbTaskPrj.Text,
                        });
                    }
                }
            }
            lbListemployeesPrj.Items.Clear();
            for (int i = 0; i < listemployeeinproject.Count; i++)
            {
                lbListemployeesPrj.Items.Add(listemployeeinproject[i].Id);
            }
        }
        //Button Remove nhân viên ra khỏi dự án (tạ thời)
        private void btRemoveemployeeprojectPrj_Click(object sender, EventArgs e)//xóa nhân viên khỏi list nhân viên (tạm thời, chưa lưu chính thức)
        {
            for (int i = 0; i < listemployeeinproject.Count; i++)
            {
                if (lbListemployeesPrj.SelectedItem.ToString() == listemployeeinproject[i].Id)
                {
                    lbListemployeesPrj.Items.Remove(lbListemployeesPrj.SelectedItem.ToString());
                    listemployeeinproject.RemoveAt(i);
                    i--; // Giảm chỉ số để không bỏ qua phần tử sau khi xóa tránh lỗi "quá độ dài của list"
                }
            }
        }
        //Button sửa thông tin nhân viên (tạm thời)
        private void btChangeemployeeprojectPrj_Click(object sender, EventArgs e)// chỉnh sửa thông tin nhân viên 
        {
            for (int i = 0; i < listemployeeinproject.Count; i++)
            {
                if (cbIdemployeeProject.Text == listemployeeinproject[i].Id)
                { 
                    listemployeeinproject.RemoveAt(i);
                    i--; // Giảm chỉ số để không bỏ qua phần tử sau khi xóa tránh lỗi "quá độ dài của list"
                    listemployeeinproject.Add(new EMPLOYEEIINPROJECT()
                    {
                        Id = cbIdemployeeProject.Text,
                        Name = tbNamePrj.Text,
                        Iddepartment = tbNamePrj.Text,
                        Description = rtbTaskPrj.Text,
                    });
                    lbListemployeesPrj.Items.Clear();
                    for (int j = 0; j < listemployeeinproject.Count; j++)
                    {
                        lbListemployeesPrj.Items.Add(listemployeeinproject[j].Id);
                    }    
                }
                break;
            }
        }
        //Button Add dự án (dữ liệu thật). nghĩa là lưu chính xác vào bộ nhớ
        private void btAddProjectPrj_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbIdprojectPrj.Text) && !string.IsNullOrEmpty(tbNameprojectPrj.Text))
            {
                int demid = 0;
                for (int i = 0; i < projects.Count; i++)
                {
                    if (tbIdprojectPrj.Text == projects[i].Id)
                    {
                        demid++;
                    }
                }
                if (demid == 0)
                {
                    projects.Add(new PROJECT()
                    {
                        Id = tbIdprojectPrj.Text,
                        Name = tbNameprojectPrj.Text.ToUpper(),
                        Description = rtbDiscriptionPrj.Text,
                        Listemployees = listemployeeinproject,
                    });
                    string filePath = @"D:\Bài tập để nộp\Projects.txt";//đường link file json
                    string json = JsonSerializer.Serialize(projects, new JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(filePath, json);
                    listemployeeinproject.Clear();
                    LoadDataFromJsonProject();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin hoặc đúng thông tin !");
            }
            ClearProject();
        }
        // Button Remove dự án (dữ liệu chính xác vào bộ nhớ)
        private void btRemoveProjectPrj_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < projects.Count; i++)
            {
                if (projects[i].Id == tbIdprojectPrj.Text)
                {
                    projects.Remove(projects[i]);
                    i--; // Giảm chỉ số để không bỏ qua phần tử sau khi xóa tránh lỗi "quá độ dài của list"
                }
            }
            string filePath = @"D:\Bài tập để nộp\Projects.txt";//đường link file json
            string json = JsonSerializer.Serialize(projects, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
            listemployeeinproject.Clear();
            LoadDataFromJsonProject();
            ClearProject();
        }
        // Button Change dữ liệu dự án (như thông tin nhân viên)
        private void btChangeProjectPrj_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbIdprojectPrj.Text) && !string.IsNullOrEmpty(tbNameprojectPrj.Text))
            {
                for (int i = 0; i < projects.Count; i++)
                {
                    if (projects[i].Id == tbIdprojectPrj.Text)
                    {
                        projects.Remove(projects[i]);
                        i--; // Giảm chỉ số để không bỏ qua phần tử sau khi xóa tránh lỗi "quá độ dài của list"
                    }
                }
                if (tbIdprojectPrj.Text != null && tbNameprojectPrj.Text != null)
                {
                    projects.Add(new PROJECT()
                    {
                        Id = tbIdprojectPrj.Text,
                        Name = tbNameprojectPrj.Text,
                        Description = rtbDiscriptionPrj.Text,
                        Listemployees = listemployeeinproject,
                    });
                }
                string filePath = @"D:\Bài tập để nộp\Projects.txt";//đường link file json
                string json = JsonSerializer.Serialize(projects, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(filePath, json);
                listemployeeinproject.Clear();
                LoadDataFromJsonProject();
                ClearProject();
            }
            else
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin hoặc đúng thông tin !");
            }

        }
        //Hàm xóa thông tin hiển thị
        public void ClearProject()
        {
            tbIdprojectPrj.ReadOnly = false;
            tbNameprojectPrj.ReadOnly = false;
            //
            tbNameprojectPrj.Text = null;
            tbIdprojectPrj.Text = null;
            lbListemployeesPrj.Items.Clear();
        }
        //Button Clear để xóa các thông tin hiển thị
        private void btClearPrj_Click(object sender, EventArgs e)
        {
            ClearProject();
        }
        // Button Search để tìm kiếm dự án theo Id
        private void btSearchPrj_Click(object sender, EventArgs e)
        {
            dgvPrj.Rows.Clear();
            for (int i = 0; i < projects.Count; i++)
            {
                if (projects[i].Id == cbSearchidprojectPrj.Text)
                {
                    dgvPrj.Rows.Add(projects[i].Id, projects[i].Name, projects[i].Description);
                }
            }
        }
        // hàm chọn 1 hàng dữ liệu
        private void dgvPrj_SelectionChanged(object sender, EventArgs e)
        {
            //xóa các hiển thị trước đi
            tbIdprojectPrj.Text = null;
            tbNameprojectPrj.Text = null;
            lbListemployeesPrj.Items.Clear();
            rtbDiscriptionPrj.Clear();
            //lấy vị trí của hàng cần sửa
            if (dgvPrj.SelectedRows.Count > 0 && dgvPrj.SelectedRows[0].Cells["clIdPrj"].Value != null)// vế điều kiện sau nhằm fix bug khi chọn hàng sẵn của dgv
            {
                int selectindex = dgvPrj.SelectedRows[0].Index;//vị trí hàng
                //đưa dữ liệu từ dgv vào bảng điền thông tin để sửa
                tbIdprojectPrj.Text = dgvPrj.Rows[selectindex].Cells["clIdPrj"].Value.ToString();
                tbNameprojectPrj.Text = dgvPrj.Rows[selectindex].Cells["clNamePrj"].Value.ToString();
                for (int i = 0; i < projects.Count; i++)
                {
                    if (dgvPrj.Rows[selectindex].Cells["clIdPrj"].Value.ToString() == projects[i].Id)
                    {
                        for (int j = 0; j < projects[i].Listemployees.Count; j++)
                        {
                            lbListemployeesPrj.Items.Add(projects[i].Listemployees[j].Id);
                        }
                        listemployeeinproject = projects[i].Listemployees;
                        rtbDiscriptionPrj.Text = projects[i].Description;
                    }

                }
            }
            //
            tbIdprojectPrj.ReadOnly = true;
            tbNameprojectPrj.ReadOnly = true;
        }
        // Button Ok để hiển thị thông tin nhân viên
        private void btOkPrj_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < employees.Count; i++)
            {
                if (cbIdemployeeProject.Text == employees[i].Id)
                {
                    tbNamePrj.Text = employees[i].Name;
                    tbIddepartmentPrj.Text = employees[i].Id;
                }
            }
        }
        // Button Show để hiển thị thông tin nhân viên trong dự án (description)
        private void btShowPrj_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listemployeeinproject.Count; i++)
            {
                if (lbListemployeesPrj.Text == listemployeeinproject[i].Id)
                {
                    cbIdemployeeProject.Text = listemployeeinproject[i].Id;
                    tbNamePrj.Text = listemployeeinproject[i].Name;
                    tbIddepartmentPrj.Text = listemployeeinproject[i].Id;
                    rtbTaskPrj.Text = listemployeeinproject[i].Description;
                }
            }
        }
        //Button All hiển thị tất cả dự án
        private void btAllPrj_Click(object sender, EventArgs e)
        {
            LoadDataFromJsonProject();
        }
        #endregion


        //4. TAB SALARY
        #region TABSALARY
        List<SALARYEMPLOYEE> salaryemployees = new List<SALARYEMPLOYEE>();//Lương nhân viên (không nhân thêm)
        List<SALARYMANAGER> salarymanagers = new List<SALARYMANAGER>();//Lương quản lí (nhân thêm 1.2)
        //hàm load dữ liệu
        public void LoadDataFromJsonSalaryemployeesAndSalarymanagers()//load lại dữ liệu để hiển thị đúng với dữ liệu hiện tại
        {
            string filePath = @"D:\Bài tập để nộp\Salaryemployees.txt";
            try
            {
                string json = File.ReadAllText(filePath);//đọc dữ liệu từ file
                List<SALARYEMPLOYEE> loadedSalaryemployees = JsonSerializer.Deserialize<List<SALARYEMPLOYEE>>(json);//dùng kĩ thuật để đưa dữ liệu vào list employees
                salaryemployees.Clear(); // Xóa danh sách hiện tại trước khi thêm dữ liệu mới từ file JSON
                salaryemployees.AddRange(loadedSalaryemployees); // Thêm dữ liệu từ file JSON vào danh sách hiện tại
                                                                 // Hiển thị dữ liệu từ danh sách mới trong DataGridView
                for (int i = 0; i < salaryemployees.Count; i++) 
                {
                    int dem = 0; ;
                    for (int j = 0; j < employees.Count; j++)
                    {
                        if (employees[j].Position == "Employee" && employees[j].Id == salaryemployees[i].Id)
                        {
                            dem++;
                        }
                    }
                    if (dem == 0)
                    {
                        salaryemployees.RemoveAt(i);
                        i--;
                    }
                }
                for (int i = 0; i < employees.Count; i++)
                {
                    int dem = 0; ;
                    for (int j = 0; j < salaryemployees.Count; j++)
                    {
                        if (employees[i].Position == "Employee" && employees[i].Id == salaryemployees[j].Id)
                        {
                            dem++;
                        }
                    }
                    if (dem == 0)
                    {
                        SALARYEMPLOYEE newsalaryemployee = new SALARYEMPLOYEE();
                        newsalaryemployee.Id = employees[i].Id;
                        newsalaryemployee.Name = employees[i].Name;
                        newsalaryemployee.Position = employees[i].Position;
                        newsalaryemployee.Start = employees[i].Start;
                        newsalaryemployee.Iddepartment = employees[i].Iddepartment;
                        salaryemployees.Add(newsalaryemployee);
                    }
                }
                foreach (SALARYEMPLOYEE slrepl in salaryemployees)
                {
                    dgvSalary.Rows.Add(slrepl.Id, slrepl.Name, slrepl.Position, slrepl.Iddepartment, slrepl.Salary, slrepl.Start, slrepl.Increase, slrepl.Bonus, slrepl.Dayoff, slrepl.Total);
                }
            }
            catch 
            {
                string filePathemployees = @"D:\Bài tập để nộp\Employees.txt";
                if (File.Exists(filePath))
                {
                    string json = File.ReadAllText(filePathemployees);
                    // Phân tích dữ liệu JSON để lấy ra chỉ những bản ghi có iddepartment phù hơp
                    JArray jsonArray = JArray.Parse(json);//kiểu dữ liệu JArray à một lớp trong thư viện Newtonsoft.Json, để phân tích dữ liệu JSON từ file và truy cập vào từng phần tử trong mảng JSON
                    foreach (JObject obj in jsonArray)
                    {
                        if (obj["Position"].ToString() == "Employee")
                        {
                            // Deserialize đối tượng từ bản ghi JSON
                            SALARYEMPLOYEE salaryemployee = obj.ToObject<SALARYEMPLOYEE>();
                            salaryemployees.Add(salaryemployee);
                        }
                    }
                }
                string filePathslrepls = @"D:\Bài tập để nộp\Salaryemployees.txt";//đường link file json 
                string jsonslrepls = JsonSerializer.Serialize(salaryemployees, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(filePathslrepls, jsonslrepls);
                foreach (SALARYEMPLOYEE slrepl in salaryemployees)
                {
                    dgvSalary.Rows.Add(slrepl.Id, slrepl.Name, slrepl.Position, slrepl.Iddepartment, slrepl.Salary, slrepl.Start, slrepl.Increase, slrepl.Bonus, slrepl.Dayoff, slrepl.Total);
                }
            }
            //
            string filePath2 = @"D:\Bài tập để nộp\Salarymanagers.txt";
            try
            {
                string json = File.ReadAllText(filePath2);//đọc dữ liệu từ file
                List<SALARYMANAGER> loadedSalarymanagers = JsonSerializer.Deserialize<List<SALARYMANAGER>>(json);//dùng kĩ thuật để đưa dữ liệu vào list employees
                salarymanagers.Clear(); // Xóa danh sách hiện tại trước khi thêm dữ liệu mới từ file JSON
                salarymanagers.AddRange(loadedSalarymanagers); // Thêm dữ liệu từ file JSON vào danh sách hiện tại
                                                               // Hiển thị dữ liệu từ danh sách mới trong DataGridView
                for (int i = 0; i < salarymanagers.Count; i++)
                {
                    int dem = 0; ;
                    for (int j = 0; j < employees.Count; j++)
                    {
                        if (employees[j].Position == "Employee" && employees[j].Id == salarymanagers[i].Id)
                        {
                            dem++;
                        }
                    }
                    if (dem == 0)
                    {
                        salarymanagers.RemoveAt(i);
                        i--;
                    }
                }
                for (int i = 0; i < employees.Count; i++)
                {
                    int dem = 0; ;
                    for (int j = 0; j < salarymanagers.Count; j++)
                    {
                        if (employees[i].Position == "Employee" && employees[i].Id == salarymanagers[j].Id)
                        {
                            dem++;
                        }
                    }
                    if (dem == 0)
                    {
                        SALARYMANAGER newsalaryemployee = new SALARYMANAGER();
                        newsalaryemployee.Id = employees[i].Id;
                        newsalaryemployee.Name = employees[i].Name;
                        newsalaryemployee.Position = employees[i].Position;
                        newsalaryemployee.Start = employees[i].Start;
                        newsalaryemployee.Iddepartment = employees[i].Iddepartment;
                        salarymanagers.Add(newsalaryemployee);
                    }
                }
                foreach (SALARYMANAGER slrepl in salarymanagers)
                {
                    dgvSalary.Rows.Add(slrepl.Id, slrepl.Name, slrepl.Position, slrepl.Iddepartment, slrepl.Salary, slrepl.Start, slrepl.Increase, slrepl.Bonus, slrepl.Dayoff, slrepl.Total);
                }
            }

            catch 
            {
                string filePathemployees = @"D:\Bài tập để nộp\Employees.txt";
                if (File.Exists(filePath))
                {
                    string json = File.ReadAllText(filePathemployees);
                    // Phân tích dữ liệu JSON để lấy ra chỉ những bản ghi có iddepartment phù hơp
                    JArray jsonArray = JArray.Parse(json);//kiểu dữ liệu JArray à một lớp trong thư viện Newtonsoft.Json, để phân tích dữ liệu JSON từ file và truy cập vào từng phần tử trong mảng JSON
                    foreach (JObject obj in jsonArray)
                    {
                        if (obj["Position"].ToString() == "Manager")
                        {
                            // Deserialize đối tượng từ bản ghi JSON
                            SALARYMANAGER salarymanager = obj.ToObject<SALARYMANAGER>();
                            salarymanagers.Add(salarymanager);
                        }
                    }
                }
                string filePathslrepls = @"D:\Bài tập để nộp\Salarymanagers.txt";//đường link file json 
                string jsonslrepls = JsonSerializer.Serialize(salarymanagers, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(filePathslrepls, jsonslrepls);
                foreach (SALARYMANAGER slrepl in salarymanagers)
                {
                    dgvSalary.Rows.Add(slrepl.Id, slrepl.Name, slrepl.Position, slrepl.Iddepartment, slrepl.Salary, slrepl.Start, slrepl.Increase, slrepl.Bonus, slrepl.Dayoff, slrepl.Total);
                }
            }

        }
        //hàm chọn 1 hàng của dữ liệu
        private void dgvSalary_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvSalary.SelectedRows.Count > 0 && dgvSalary.SelectedRows[0].Cells["clIdSlr"].Value != null)// vế điều kiện sau nhằm fix bug khi chọn hàng sẵn của dgv
            {
                int selectindex = dgvSalary.SelectedRows[0].Index;//vị trí hàng
                                                                  //đưa dữ liệu từ dgv vào bảng điền thông tin để sửa

                if (dgvSalary.Rows[selectindex].Cells["clPositionSlr"].Value.ToString() == "Employee")//là Employee
                {
                    rbEmployeeSlr.Checked = true;
                }
                else//là Manager
                {
                    rbEmployeeSlr.Checked = true;
                }
                tbIdSlr.Text = dgvSalary.Rows[selectindex].Cells["clIdSlr"].Value.ToString();
                tbNameSlr.Text = dgvSalary.Rows[selectindex].Cells["clNameSlr"].Value.ToString();
                tbSalarySlr.Text = dgvSalary.Rows[selectindex].Cells["clSalarySlr"].Value.ToString(); 
                tbBonusSlr.Text = dgvSalary.Rows[selectindex].Cells["clBonusSlr"].Value.ToString();
                tbIncreaseSlr.Text = dgvSalary.Rows[selectindex].Cells["clIncreaseSlr"].Value.ToString();
                tbIddepartmentSlr.Text = dgvSalary.Rows[selectindex].Cells["clIddepartmentSlr"].Value.ToString();
                tbDayoffSlr.Text = dgvSalary.Rows[selectindex].Cells["clDayoffSlr"].Value.ToString();
                string rbStart = dgvSalary.Rows[selectindex].Cells["clStartSlr"].Value.ToString();
                DateTime rbStartValue;
                if (DateTime.TryParse(rbStart, out rbStartValue))
                {
                    dtpStartSlr.Value = rbStartValue;
                }
                tbTotalSlr.Text = dgvSalary.Rows[selectindex].Cells["clTotalSlr"].Value.ToString();
            }
        }
        //Button Ok để tính ra lương tổng và đưa vào dữ liệu
        private void btOkSlr_Click(object sender, EventArgs e)
        {
            if (rbEmployeeSlr.Checked)
            {
                for (int i = 0; i < salaryemployees.Count; i++)
                {
                    if (salaryemployees[i].Id == tbIdSlr.Text)
                    {
                        try
                        {
                            tbTotalSlr.Text = salaryemployees[i].TotalSalary().ToString(); // Gán giá trị vào textbox tbTotalSlr
                            salaryemployees[i].Bonus = double.Parse(tbBonusSlr.Text); // Chuyển đổi giá trị từ textbox sang kiểu double và gán vào thuộc tính Bonus
                            salaryemployees[i].Increase = double.Parse(tbIncreaseSlr.Text); // Chuyển đổi giá trị từ textbox sang kiểu double và gán vào thuộc tính Increase
                            salaryemployees[i].Dayoff = double.Parse(tbDayoffSlr.Text); // Chuyển đổi giá trị từ textbox sang kiểu double và gán vào thuộc tính Dayoff
                            salaryemployees[i].Total = double.Parse(tbTotalSlr.Text);
                            string filePath = @"D:\Bài tập để nộp\Salaryemployees.txt";//đường link file json
                            string json = JsonSerializer.Serialize(salaryemployees, new JsonSerializerOptions { WriteIndented = true });
                            File.WriteAllText(filePath, json);
                        }
                        catch (FormatException ex) // Xử lý ngoại lệ FormatException nếu giá trị nhập không đúng định dạng số
                        {
                            MessageBox.Show("Giá trị nhập không đúng định dạng số.");
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < salaryemployees.Count; i++)
                {
                    if (salaryemployees[i].Id == tbIdSlr.Text)
                    {
                        try
                        {
                            tbTotalSlr.Text = salaryemployees[i].TotalSalary().ToString(); // Gán giá trị vào textbox tbTotalSlr
                            salaryemployees[i].Bonus = double.Parse(tbBonusSlr.Text); // Chuyển đổi giá trị từ textbox sang kiểu double và gán vào thuộc tính Bonus
                            salaryemployees[i].Increase = double.Parse(tbIncreaseSlr.Text); // Chuyển đổi giá trị từ textbox sang kiểu double và gán vào thuộc tính Increase
                            salaryemployees[i].Dayoff = double.Parse(tbDayoffSlr.Text); // Chuyển đổi giá trị từ textbox sang kiểu double và gán vào thuộc tính Dayoff
                            salaryemployees[i].Total = double.Parse(tbTotalSlr.Text);
                            string filePath = @"D:\Bài tập để nộp\Salarymanagers.txt";//đường link file json
                            string json = JsonSerializer.Serialize(salarymanagers, new JsonSerializerOptions { WriteIndented = true });
                            File.WriteAllText(filePath, json);
                        }
                        catch (FormatException ex) // Xử lý ngoại lệ FormatException nếu giá trị nhập không đúng định dạng số
                        {
                            MessageBox.Show("Giá trị nhập không đúng định dạng số.");
                        }
                    }
                }
            }
            dgvSalary.Rows.Clear();
            LoadDataFromJsonSalaryemployeesAndSalarymanagers();
        }
        //Button Manager để hiện thị thông tin của các Manager
        private void btManagerSlr_Click(object sender, EventArgs e)
        {
            dgvSalary.Rows.Clear();
            foreach (SALARYMANAGER slrmng in salarymanagers)
            {
                if (slrmng.Position == "Manager")
                {
                    dgvSalary.Rows.Add(slrmng.Id, slrmng.Name, slrmng.Position, slrmng.Iddepartment, slrmng.Salary, slrmng.Start, slrmng.Increase, slrmng.Bonus, slrmng.Dayoff, slrmng.Total);
                }
            }
        }
        //Button Employee để hiển thị thông tin của các Employee
        private void btEmployeeSlr_Click(object sender, EventArgs e)
        {
            dgvSalary.Rows.Clear();
            foreach (SALARYEMPLOYEE slrepl in salaryemployees)
            {
                if (slrepl.Position == "Employee")
                {
                    dgvSalary.Rows.Add(slrepl.Id, slrepl.Name, slrepl.Position, slrepl.Iddepartment, slrepl.Salary, slrepl.Start, slrepl.Increase, slrepl.Bonus, slrepl.Dayoff, slrepl.Total);
                }
            }
        }
        //Button Search để tìm dữ liệu theo Id
        private void btSearchSlr_Click(object sender, EventArgs e)
        {
            dgvSalary.Rows.Clear();
            foreach (SALARYMANAGER slrmng in salarymanagers)
            {
                if (slrmng.Id == cbIdSlr.Text)
                {
                    dgvSalary.Rows.Add(slrmng.Id, slrmng.Name, slrmng.Position, slrmng.Iddepartment, slrmng.Salary, slrmng.Start, slrmng.Increase, slrmng.Bonus, slrmng.Dayoff, slrmng.Total);
                }
            }
            foreach (SALARYEMPLOYEE slrepl in salaryemployees)
            {
                if (slrepl.Id == cbIdSlr.Text)
                {
                    dgvSalary.Rows.Add(slrepl.Id, slrepl.Name, slrepl.Position, slrepl.Iddepartment, slrepl.Salary, slrepl.Start, slrepl.Increase, slrepl.Bonus, slrepl.Dayoff, slrepl.Total);
                }
            }
        }
        //Button All để hiển thị tất cả thông tin
        private void btAllSlr_Click(object sender, EventArgs e)
        {
            LoadDataFromJsonSalaryemployeesAndSalarymanagers();
        }

        #endregion


        /*cần tạo 11 file text
         * 1. Employees
         * 2. Managers
         * 3. Ketoan
         * 4. Laocong
         * 5. Nhansu
         * 6. Giamsat
         * 7. Baove
         * 8. Marketing
         * 9. Vanchuyen
         * 10. Salaryemployees
         * 11. Salarymanagers
         */
        /* Các dòng cần sửa đường link
         * dòng 59
dòng 196
dòng 227
dòng 420
dòng 525
từ dong 626 đến 646
dòng 935
dòng 953
dòng 1076
dòng 1100
dòng 1130
dòng 1244
dòng 1296
dòng 1312
dòng 1321
dòng 1374
dòng 1390
dòng 1448
dòng 1472
         Khi sửa nhớ chỗ nào giống nhau thì phải giống nhau vì nó cần đồng bộ*/
    }
}

