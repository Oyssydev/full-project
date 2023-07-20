using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConverterClassLib
{
    public class Classroom
    {
        [Key] // Define primary key attribute
        public int classID { get; set; }
        public int classLocation { get; set; }
        public int classNbTables { get; set; }
        public int classNbChaires { get; set; }
        public virtual ICollection<Etudiant> Etudiants { get; set;}
    }
}
