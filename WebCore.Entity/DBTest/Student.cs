using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WebCore.Entity.DBTest
{
     public class Student: IEntity
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(10)]
        public string Name { get; set; }

        [MaxLength(2)]
        public string Sex { get; set; }
        public int Age { get; set; }

        [MaxLength(300)]
        public string Address { get; set; }

    }
}
