using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using WebCore.Domain.EntityIRepositorys;
using WebCore.Entity;
using WebCore.Entity.DBTest;
using WebCore.Common.Extensions;
using System.Threading;

namespace WebCore.Domain.EntityRepositorys
{
    public class StudentRepository : BaseRepository<Student, TestDbContext>, IStudentRepository
    {
        public StudentRepository(IUnitOfWork _uow) : base(_uow) { }


        public void Test() {
            var s = new Entity.DBTest.Student()
            {
                Name = "li",
                Age = 11,
                Sex = "男"
            };
            var s2 = new Entity.DBTest2.Food()
            {
                Name = "奥斯念佛"
            };
            var s3 = new Entity.DBTest.Student()
            {
                Name = "li",
                Age = 11,
                Sex = "男"
            };
            SaveChanges(() => {
                db.Students.Add(s);
                SaveChanges();
                db.Students.Add(s3);
                SaveChanges();
                //_unitOfWork.Get<Entity.Test2DbContext>().Add(s2);
                //SaveChanges<Test2DbContext>();
                return true;

            });
        }

        public string Test2(int i, Entity.DBTest.Student en)
        {
            //throw new Exception("55");
            return "1 23";
        }

        public string FallBack(int i) {
            return "99999";
        }


    }
}
