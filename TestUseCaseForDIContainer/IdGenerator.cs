using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestUseCaseForDIContainer
{
    public class IdGenerator : IIdGenerator
    {
        private IConsoleWriter _consoleWriter;

        public IdGenerator(IConsoleWriter consoleWriter)
        {
            _consoleWriter = consoleWriter;
        }

        public Guid Id { get; } = Guid.NewGuid();

        public void PrintId()
        {
            _consoleWriter.WriteLine(Id.ToString());
        }
    }

    public interface IIdGenerator
    {
        public Guid Id { get;}
        public void PrintId();
    }
}
