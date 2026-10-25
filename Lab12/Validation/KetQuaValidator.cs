using System.Globalization;

namespace QLSinhVienLab12
{
    /// <summary>Kiểm tra điểm và ghi chú (không phụ thuộc giao diện hay database nên dễ kiểm thử).</summary>
    public static class KetQuaValidator
    {
        public const decimal DiemMin = 0m;
        public const decimal DiemMax = 10m;
        public const int GhiChuMaxLength = 200;

        /// <summary>Đọc điểm từ chuỗi, chấp nhận cả dấu chấm và dấu phẩy thập phân, không phụ thuộc cấu hình vùng của Windows.</summary>
        public static bool TryParseDiem(string text, out decimal diem)
        {
            string normalized = (text ?? "").Trim().Replace(',', '.');
            return decimal.TryParse(normalized,
                NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture, out diem);
        }

        /// <summary>Kiểm tra lúc đang gõ vào ô điểm: để trống thì cho qua (bị chặn ở bước lưu). Trả về null nếu hợp lệ.</summary>
        public static string ValidateDiemText(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return null;
            if (!TryParseDiem(text, out decimal diem))
                return "Điểm phải là số (ví dụ 7.5).";
            return CheckRange(diem);
        }

        /// <summary>Kiểm tra giá trị điểm của một dòng khi bấm Lưu: không được rỗng, phải là số trong khoảng 0 đến 10.</summary>
        public static string ValidateDiemValue(object value, out decimal diem)
        {
            diem = 0;
            if (value == null || value == DBNull.Value || string.IsNullOrWhiteSpace(Convert.ToString(value)))
                return "Điểm không được để trống.";

            if (value is decimal d)
                diem = d;
            else if (!TryParseDiem(Convert.ToString(value), out diem))
                return "Điểm phải là số.";

            return CheckRange(diem);
        }

        public static string ValidateGhiChu(string ghiChu)
        {
            if (ghiChu != null && ghiChu.Trim().Length > GhiChuMaxLength)
                return $"Ghi chú tối đa {GhiChuMaxLength} ký tự.";
            return null;
        }

        private static string CheckRange(decimal diem)
        {
            if (diem < DiemMin || diem > DiemMax)
                return $"Điểm phải nằm trong khoảng từ {DiemMin} đến {DiemMax}.";
            if (decimal.Round(diem, 2) != diem)
                return "Điểm tối đa 2 chữ số thập phân.";
            return null;
        }
    }
}
