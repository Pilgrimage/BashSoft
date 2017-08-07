namespace BashSoft
{
    using System;
    using System.Linq;
    using System.Reflection;
    using BashSoft.Attributes;
    using BashSoft.IO.Commands;
    using BashSoft.Exceptions;
    using BashSoft.Contracts;

    public class CommandInterpreter : IInterpreter
    {
        private IContentComparer judge;
        private IDatabase repository;
        private IDirectoryManager inputOutputManager;

        public CommandInterpreter(IContentComparer judge, IDatabase repository, IDirectoryManager inputOutputManager)
        {
            this.judge = judge;
            this.repository = repository;
            this.inputOutputManager = inputOutputManager;
        }

        public void InterpretCommand(string input)
        {
            string[] data = input.Split(' ');
            string commandName = data[0].ToLower();

            try
            {
                IExecutable command = this.ParseCommand(input, commandName, data);
                command.Execute();
            }
            catch (Exception ex)
            {
                OutputWriter.DisplayException(ex.Message);
            }
        }

        private IExecutable ParseCommand(string input, string command, string[] data)
        {

            object[] parametersForConstruction = new object[]
            {
                input,
                data
            };

            // or .GetCustomAttributes(typeof(AliasAttribute))

            Type typeOfCommand = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .FirstOrDefault(t => t.GetCustomAttributes<AliasAttribute>()
                                      .Where(atr => atr.Equals(command))
                                      .ToArray().Length > 0);

            if (typeOfCommand==null)
            {
                throw new InvalidCommandException(input);
            }

            // Create Instance of Command 
            IExecutable currentCommand = (Command)Activator.CreateInstance(typeOfCommand, parametersForConstruction);
            
            // Inject values to fields
            currentCommand = this.InjectDependencies(currentCommand);

            return currentCommand;
        }


        private IExecutable InjectDependencies(IExecutable currentCommand)
        {
            FieldInfo[] commandFields = currentCommand
                .GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                .Where(f => f.GetCustomAttributes<InjectAttribute>() != null)
                .ToArray();

            FieldInfo[] interpreterFields = this
                .GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.NonPublic);

            foreach (FieldInfo commandField in commandFields)
            {
                FieldInfo interpreterField = interpreterFields
                    .First(f => f.FieldType == commandField.FieldType);

                object valueToInject = interpreterField.GetValue(this);

                commandField.SetValue(currentCommand, valueToInject);
            }

            return currentCommand;
        }
    }

}

