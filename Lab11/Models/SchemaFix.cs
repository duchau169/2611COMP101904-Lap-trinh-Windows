namespace COMP1019_Lab11_AdoNet
{
    /// <summary>
    /// Một cột "bắt buộc nhập" trong bảng LOP mà đề Lab 11 không có (ví dụ MaKhoa của buổi 10),
    /// kèm câu lệnh ALTER để cho phép cột đó nhận NULL.
    /// </summary>
    public class SchemaFix
    {
        public string ColumnName { get; set; }
        public string AlterStatement { get; set; }
    }
}
