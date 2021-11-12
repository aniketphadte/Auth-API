using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Core.Models
{
    public interface ISettings
    {
        int RequestBuffer { get; set; }
    }
    public class Settings : ISettings
    {
        public int RequestBuffer { get; set; }
    }
}
