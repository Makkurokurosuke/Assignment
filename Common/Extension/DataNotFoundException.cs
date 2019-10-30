using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Common.Extension
{
    public class DataNotFoundException : Exception
    {

        public DataNotFoundException(string message)
            : base(message)
        {

        }


    }
}
