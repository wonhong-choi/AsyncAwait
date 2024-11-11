using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTest
{
    public interface IRequestCmd
    {
        IResponseCmd Accept(IRequestVisitor engine);
    }

    public class CreateRequestCmd : IRequestCmd
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public IResponseCmd Accept(IRequestVisitor engine)
        {
            return engine.Visit(this);
        }
    }

    public class DeleteRequestCmd : IRequestCmd
    {
        public int Id { get; set; }

        public IResponseCmd Accept(IRequestVisitor engine)
        {
            return engine.Visit(this);
        }
    }

    public class UpdateRequestCmd : IRequestCmd
    {
        public int Id { get; set; }

        public IResponseCmd Accept(IRequestVisitor engine)
        {
            return engine.Visit(this);
        }
    }

    public class NullRequestCmd : IRequestCmd
    {
        public IResponseCmd Accept(IRequestVisitor engine)
        {
            return engine.Visit(this);
        }
    }


    public interface IResponseCmd
    {
        void Accept(dynamic client);
    }

    public class NullResponseCmd : IResponseCmd
    {
        public void Accept(dynamic client)
        {

        }
    }

    public class CreateResponseCmd : IResponseCmd
    {
        public int Id { get; set; }
        public DateTime WhenCreated { get; set; }
        public bool IsSuccessful { get; set; }

        public void Accept(dynamic client)
        {
            client.Visit(this);
        }
    }

    public class DeleteResponseCmd : IResponseCmd
    {
        public int Id { get; set; }
        public DateTime WhenDeleted { get; set; }
        public bool IsSuccessful { get; set; }

        public void Accept(dynamic client)
        {
            client.Visit(this);
        }
    }

    public class UpdateResponseCmd : IResponseCmd
    {
        public int Id { get; set; }
        public DateTime WhenUpdated { get; set; }
        public bool IsSuccessful { get; set; }

        public void Accept(dynamic client)
        {
            client.Visit(this);
        }
    }
}
