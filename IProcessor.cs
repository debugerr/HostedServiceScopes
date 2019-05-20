using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HostedServiceScopes
{
    public interface IProcessor
    {
        Task DoWork();
    }
}
