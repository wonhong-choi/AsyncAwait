using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTest
{
    public interface IRequestCmd
    {
        IResponseCmd Accept(ICmdVisitor engine);
    }

    public class CreateRequestCmd : IRequestCmd
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public IResponseCmd Accept(ICmdVisitor engine)
        {
            return engine.Visit(this);
        }
    }

    public class DeleteRequestCmd : IRequestCmd
    {
        public int Id { get; set; }

        public IResponseCmd Accept(ICmdVisitor engine)
        {
            return engine.Visit(this);
        }
    }

    public class NullRequestCmd : IRequestCmd
    {
        public IResponseCmd Accept(ICmdVisitor engine)
        {
            return engine.Visit(this);
        }
    }


    public interface IResponseCmd
    {
        void Accept(IClient client);
    }

    public class NullResponseCmd : IResponseCmd
    {
        public void Accept(IClient client)
        {
            
        }
    }

    public class CreateResponseCmd : IResponseCmd
    {
        public int Id { get; set; }
        public DateTime WhenCreated { get; set; }
        public bool IsSuccessful { get; set; }

        public void Accept(IClient client)
        {
            client.Visit(this);
        }
    }

    public class DeleteResponseCmd : IResponseCmd
    {
        public int Id { get; set; }
        public DateTime WhenDeleted { get; set; }
        public bool IsSuccessful { get; set; }

        public void Accept(IClient client)
        {
            client.Visit(this);
        }
    }
}
