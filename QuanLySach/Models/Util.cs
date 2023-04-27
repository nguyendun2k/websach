using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLySach.Models
{
    public class Util
    {
        public static decimal GiaGiam(decimal gia , string pt)
        {

            if (gia != null && pt !="")
            {
              decimal  giamoi = gia * int.Parse(pt) / 100;
               return giamoi;
            }

            return gia;
        }
    }
}