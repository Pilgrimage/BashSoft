namespace BashSoft.IO.Commands
{
    using BashSoft.Attributes;
    using BashSoft.Exceptions;
    using BashSoft.Contracts;

    [Alias("show")]
    public class ShowCourseCommand : Command
    {
        [Inject]
        private IDatabase repository;

        public ShowCourseCommand(string input, string[] data) : base(input, data)
        {
        }

        public override void Execute()
        {
            if (this.Data.Length != 2 && this.Data.Length != 3)
            {
                throw new InvalidCommandException(this.Input);
            }
            else if (this.Data.Length == 2)
            {
                string courseName = this.Data[1];
                this.repository.GetAllStudentsFromCourse(courseName);
            }
            else
            {
                string courseName = this.Data[1];
                string userName = this.Data[2];
                this.repository.GetStudentScoresFromCourse(courseName, userName);
            }
        }
    }
}