using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConverterClassLib
{

    //code fist
    //this is the models which we use to generate the db ( migration )
    public class Etudiant
    {
        public int id{ get; set; }
        public string firstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Deparetement { get; set; } = string.Empty;
        public string Class { get; set; } = string.Empty;

    }
}
