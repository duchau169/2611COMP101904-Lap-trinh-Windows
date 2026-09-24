using System;

namespace Lab04_QuanLySanPham
{
    // Exception tu tao: phat sinh khi xoa hoac sua san pham khong ton tai
    public class ProductNotFoundException : Exception
    {
        public string MaSP { get; }

        public ProductNotFoundException(string maSP)
            : base($"Khong tim thay san pham co ma '{maSP}'.")
        {
            MaSP = maSP;
        }

        public ProductNotFoundException(string maSP, string message)
            : base(message)
        {
            MaSP = maSP;
        }
    }
}
