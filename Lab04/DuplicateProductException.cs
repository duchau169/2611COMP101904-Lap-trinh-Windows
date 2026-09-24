using System;

namespace Lab04_QuanLySanPham
{
    // Exception tu tao: phat sinh khi them san pham co ma da ton tai
    public class DuplicateProductException : Exception
    {
        public string MaSP { get; }

        public DuplicateProductException(string maSP)
            : base($"San pham co ma '{maSP}' da ton tai trong kho.")
        {
            MaSP = maSP;
        }

        public DuplicateProductException(string maSP, string message)
            : base(message)
        {
            MaSP = maSP;
        }
    }
}
