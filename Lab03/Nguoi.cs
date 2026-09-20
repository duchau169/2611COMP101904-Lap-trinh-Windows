using System;

namespace Lab03_QuanLySinhVienOOP
{
    // Class cha: Nguoi
    public class Nguoi
    {
        public string HoTen { get; set; }
        public DateTime NgaySinh { get; set; }

        public Nguoi()
        {
            HoTen = string.Empty;
            NgaySinh = DateTime.Now;
        }

        public Nguoi(string hoTen, DateTime ngaySinh)
        {
            HoTen = hoTen;
            NgaySinh = ngaySinh;
        }

        // Method co the override o class con
        public virtual string LayThongTin()
        {
            return $"Ho ten: {HoTen} - Ngay sinh: {NgaySinh:dd/MM/yyyy}";
        }
    }
}
