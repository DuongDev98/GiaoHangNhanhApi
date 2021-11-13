using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GiaoHangNhanh
{
    class Program
    {
        static void Main(string[] args)
        {
            string error = "";
            GiaoHangNhanhApi.CheckLogin(ref error);
        }
    }
}
