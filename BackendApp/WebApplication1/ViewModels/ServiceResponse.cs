using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeewashAPICore.ViewModels
{
    public class ServiceResponse<T>
    {
        public int CODE { get; set; }
        public string MESSAGE { get; set; }
        public IEnumerable<T> DATA { get; set; }
    }

    public class ServiceResponseSingle<T>
    {
        public int CODE { get; set; }
        public string MESSAGE { get; set; }
        public T DATA { get; set; }
    }

  


}
