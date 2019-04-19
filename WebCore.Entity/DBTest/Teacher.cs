using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WebCore.Entity.DBTest
{
    public class Teacher: IEntity
    {
        public int Id { get; set; }

        [MaxLength(10)]
        public string Name { get; set; }

        [MaxLength(2)]
        public string Sex { get; set; }
        public int Age { get; set; }
    }
}
