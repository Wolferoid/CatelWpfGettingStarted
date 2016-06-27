using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF.GettingStarted.Services.Interfaces
{
    using WPF.GettingStarted.Models;
    public interface IFamilyService
    {
        IEnumerable<Family> LoadFamilies();
        void SaveFamilies(IEnumerable<Family> families);
    }
}
