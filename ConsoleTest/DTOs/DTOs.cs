using ConsoleTest.Services;
using ConsoleTest.VOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTest.DTOs
{
    public interface IRequestCmd
    {
        IResponseCmd Accept(IRequestVisitor requestVisitor);
    }

    public class PlatformCreateRequestCmd : IRequestCmd
    {
        public int Id { get; set; }
        
        public LatLonAlt LatLonAlt { get; set; }

        public IResponseCmd Accept(IRequestVisitor requestVisitor)
        {
            return requestVisitor.Visit(this);
        }
    }

    public class PlatformDeleteRequestCmd : IRequestCmd
    {
        public int Id { get; set; }

        public IResponseCmd Accept(IRequestVisitor requestVisitor)
        {
            return requestVisitor.Visit(this);
        }
    }

    public class PlatformSyncRequestCmd : IRequestCmd
    {
        public int TargetId { get; set; }
        public int OriginatorId { get; set; }

        public IResponseCmd Accept(IRequestVisitor requestVisitor)
        {
            return requestVisitor.Visit(this);
        }
    }

    public class NullRequestCmd : IRequestCmd
    {
        public IResponseCmd Accept(IRequestVisitor requestVisitor)
        {
            return requestVisitor.Visit(this);
        }
    }

    public class MissileShootRequestCmd : IRequestCmd
    {
        public int MissileId { get; set; }
        public int SrcId { get; set; } 
        public int TargetId { get; set; } 

        public IResponseCmd Accept(IRequestVisitor requestVisitor)
        {
            return requestVisitor.Visit(this);
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

    public class PlatformCreateResponseCmd : IResponseCmd
    {
        public int Id { get; set; }
        public DateTime WhenCreated { get; set; }
        public bool IsSuccessful { get; set; }

        public void Accept(dynamic client)
        {
            client.Visit(this);
        }
    }

    public class PlatformDeleteResponseCmd : IResponseCmd
    {
        public int Id { get; set; }
        public DateTime WhenDeleted { get; set; }
        public bool IsSuccessful { get; set; }

        public void Accept(dynamic client)
        {
            client.Visit(this);
        }
    }

    public class PlatformSyncResponseCmd : IResponseCmd
    {
        public int Id { get; set; }
        public DateTime WhenSynced { get; set; }
        public bool IsSuccessful { get; set; }

        public void Accept(dynamic client)
        {
            client.Visit(this);
        }
    }

    public class MissileShootResponseCmd : IResponseCmd
    {
        public int MissileId { get; set; }
        
        public int SrcId { get; set; }
        public int TargetId { get; set; }

        public DateTime WhenShot { get; set; }
        public bool IsSuccessful { get; set; }


        public void Accept(dynamic client)
        {
            client.Visit(this);
        }
    }

}
