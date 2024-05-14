using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOPWF
{
    //lớp thực thể có dùng cho mọi lớp, bởi lớp chính nào cũng sẽ có ID riêng và tên riêng
    public interface IENTITY
    {
        string Id { get; set; }
        string Name { get; set; }
    }
}
