using System;
using System.Collections.Generic;
using System.Text;
using WebCore.Domain.EntityIRepositorys;
using WebCore.Entity;
using WebCore.Entity.DBTest;
using WebCore.Entity.DBTest2;

namespace WebCore.Domain.EntityRepositorys
{
    public class FoodRepository : BaseRepository<Food, Test2DbContext>, IFoodRepository
    {
        public FoodRepository(IUnitOfWork _uow) : base(_uow) { }


    }
}
