using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTest
{
    public class FirstClient
    {
        private readonly IEngine _engine;

        public FirstClient(IEngine engine)
        {
            _engine = engine;
        }

        public async void OnCreateRequestMsgReceived(int id, string name)
        {
            var response = await _engine.Receive(new CreateRequestCmd()
            {
                Id = id,
                Name = name,
            });

            response.Accept(this);
        }

        public async void OnDeleteRequestMsgReceived(int id)
        {
            var response = await _engine.Receive(new DeleteRequestCmd()
            {
                Id = id,
            });

            response.Accept(this);
        }

        public async void OnNullRequestMsgReceived()
        {
            _ = await _engine.Receive(new NullRequestCmd());

            Console.WriteLine($"Null > Nothing to do.");
        }

        public void Visit(CreateResponseCmd createResponseCmd)
        {
            Console.WriteLine($"Created > {createResponseCmd.Id} : {createResponseCmd.IsSuccessful} / {createResponseCmd.WhenCreated}");
        }

        public void Visit(DeleteResponseCmd deleteResponseCmd)
        {
            Console.WriteLine($"Deleted > {deleteResponseCmd.Id} : {deleteResponseCmd.IsSuccessful} / {deleteResponseCmd.WhenDeleted}");
        }
    }
}