namespace QLSinhVienLab12
{
    /// <summary>Một dòng kết quả học tập cần lưu (một sinh viên, một môn).</summary>
    public class KetQuaItem
    {
        public string MaSV { get; set; }
        public string HoTen { get; set; }
        public decimal Diem { get; set; }
        public string GhiChu { get; set; }   // null nếu để trống
    }
}
