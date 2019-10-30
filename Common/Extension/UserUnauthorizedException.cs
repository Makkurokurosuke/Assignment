using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Common.Extension
{
    public class UserNotFoundException : Exception
    {

        public UserNotFoundException(string message)
            : base(message)
        {

        }


    }
}
