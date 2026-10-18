using System.Text.RegularExpressions;

namespace COMP1019_Lab11_AdoNet
{
    /// <summary>Kiểm tra dữ liệu nhập (không đụng tới giao diện hay database nên dễ kiểm thử).</summary>
    public static class LopValidator
    {
        public const int MaLopMaxLength = 20;
        public const int TenLopMaxLength = 100;

        // Cột MaLop là VARCHAR nên chỉ cho chữ không dấu, số, gạch dưới, gạch ngang
        private static readonly Regex MaLopPattern = new Regex("^[A-Za-z0-9_-]+$");

        /// <summary>Trả về thông báo lỗi, hoặc null nếu hợp lệ.</summary>
        public static string ValidateMaLop(string maLop)
        {
            if (string.IsNullOrWhiteSpace(maLop))
                return "Mã lớp không được để trống!";
            if (maLop.Trim().Length > MaLopMaxLength)
                return $"Mã lớp tối đa {MaLopMaxLength} ký tự!";
            if (!MaLopPattern.IsMatch(maLop.Trim()))
                return "Mã lớp chỉ gồm chữ không dấu, số, dấu _ hoặc - (không có khoảng trắng)!";
            return null;
        }

        public static string ValidateTenLop(string tenLop)
        {
            if (string.IsNullOrWhiteSpace(tenLop))
                return "Tên lớp không được để trống!";
            if (tenLop.Trim().Length > TenLopMaxLength)
                return $"Tên lớp tối đa {TenLopMaxLength} ký tự!";
            return null;
        }

        /// <summary>Kiểm tra cả mã và tên (dùng khi thêm).</summary>
        public static string Validate(string maLop, string tenLop)
        {
            return ValidateMaLop(maLop) ?? ValidateTenLop(tenLop);
        }
    }
}
