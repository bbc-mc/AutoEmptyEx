using System;
using ICities;

namespace AutoEmptyEx
{
    public class AutoEmptyExt : IUserMod
    {
        public String Name
        {
        	get { return "Auto Empty Ex"; }
        }

        public String Description
        {
            get { return "Add My functions (Emergency Start) to Original Automatic Empty, which Automatically starts emptying and stops whenever it finishes. Works for landfills and cemeteries"; }
        }
    }
}