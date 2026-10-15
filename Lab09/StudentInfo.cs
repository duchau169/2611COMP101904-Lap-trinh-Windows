namespace COMP1019_Lab09_StudentCard
{
    /// <summary>Thông tin một sinh viên dùng để vẽ thẻ.</summary>
    public class StudentInfo
    {
        public string StudentId { get; set; } = "";
        public string FullName { get; set; } = "";
        public string ClassName { get; set; } = "";
        public string Faculty { get; set; } = "";
        public string Course { get; set; } = "";
        public DateTime BirthDate { get; set; } = DateTime.Today;
        public string Gender { get; set; } = "";

        /// <summary>Ảnh đại diện (có thể null nếu chưa chọn). Không thuộc quyền sở hữu của StudentInfo.</summary>
        public Image Avatar { get; set; }
    }
}
