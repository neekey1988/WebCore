using System;
using System.Collections.Generic;
using System.Text;
using WebCore.Domain.EntityIRepositorys;
using WebCore.Entity;
using WebCore.Entity.DBTest;

namespace WebCore.Domain.EntityRepositorys
{
    public class TeacherRepository : BaseRepository<Teacher, TestDbContext>, ITeacherRepository
    {
        public TeacherRepository(IUnitOfWork _uow) : base(_uow) { }


    }
}
