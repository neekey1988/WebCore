using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Text;
using WebCore.Common.Attributes;

namespace WebCore.Domain.EntityIRepositorys
{
    public interface IStudentRepository:IRepository<Entity.DBTest.Student>
    {
        [Hystrix(
      FallBacHandle = nameof(FallBack),
      IsEnableFallBack = Common.Enums.E_Hystrix.Enable,
      IsEnableCache = Common.Enums.E_Hystrix.Enable,
      IsEnableCircuitBreaker = Common.Enums.E_Hystrix.Enable,
      ExceptionsAllowedBeforeBreaking = 2, MaxRetryTimes = 0, RetryIntervalMilliseconds = 1000,
      TimeOutMilliseconds = 0,
      MillisecondsOfBreak = 1000 * 30)]
        void Test();
        [Hystrix( 
            FallBacHandle =nameof(FallBack),
            IsEnableFallBack = Common.Enums.E_Hystrix.Enable,
            IsEnableCache = Common.Enums.E_Hystrix.Enable,
            IsEnableCircuitBreaker = Common.Enums.E_Hystrix.Enable,
            ExceptionsAllowedBeforeBreaking = 2, MaxRetryTimes =0, RetryIntervalMilliseconds =1000,
            TimeOutMilliseconds = 0,
            MillisecondsOfBreak = 1000 * 30)]
        string Test2(int i, Entity.DBTest.Student en);


        string FallBack(int i);
    }
}
