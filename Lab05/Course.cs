namespace CourseRegistrationApp
{
    /// <summary>Khóa học và học phí mỗi tháng.</summary>
    public class Course
    {
        public string Name { get; }
        public decimal FeePerMonth { get; }

        public Course(string name, decimal feePerMonth)
        {
            Name = name;
            FeePerMonth = feePerMonth;
        }

        // ComboBox hiển thị tên khóa học nhờ ToString()
        public override string ToString() => Name;
    }
}
