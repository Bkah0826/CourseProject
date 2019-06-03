using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSS.Data.POCOs
{
    class SurveyResponses
    {
        public List<String> Gender { get; set; }

        public List<String> Age { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

    }
}
