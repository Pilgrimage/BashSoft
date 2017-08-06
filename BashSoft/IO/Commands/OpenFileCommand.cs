namespace BashSoft.IO.Commands
{
    using BashSoft.Exceptions;
    using System.Diagnostics;
    using BashSoft.Contracts;

    public class OpenFileCommand : Command
    {
        public OpenFileCommand(string input, string[] data, IContentComparer judge, IDatabase repository, IDirectoryManager inputOutputManager) 
            : base(input, data, judge, repository, inputOutputManager)
        {
        }

        public override void Execute()
        {
            if (this.Data.Length == 2)
            {
                string fileName = this.Data[1];
                Process.Start(SessionData.currentPath + "\\" + fileName);
            }
            else
            {
                throw new InvalidCommandException(this.Input);
            }
        }
    }
}