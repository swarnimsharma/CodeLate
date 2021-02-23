using Portal.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.PortalBL.ImportExcel
{
    interface IImportBL
    {
        ResponseOut ImportVendorCandidate(string url);
        string ImportData(string Identifier, string type, string name);
    }
}
