using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatchNotes.Shared
{
    public partial class LoggedInLayout
    {
        private bool _drawerOpen = true;

        private void DrawerToggle()
        {
            _drawerOpen = !_drawerOpen;
        }
    }
}
