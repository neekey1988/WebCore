using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using WebCore.Component.Options;

namespace WebCore.Component.Providers.ServiceRegister
{
    public interface IServiceRegisterProvider
    {
        void Register(IApplicationLifetime lifetime, OptionsServiceInfo serviceInfo, OptionsHealth health);
    }
}
