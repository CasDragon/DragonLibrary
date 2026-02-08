using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonLibrary.Utils
{
    public class TTTSettingChecker
    {
        public static bool CheckSpellsFixes(string key)
        {
            if (!ModCompat.tttbase)
                return false;
            return TabletopTweaks.Base.Main.TTTContext.Fixes.Spells.IsEnabled(key);
        }
    }
}
