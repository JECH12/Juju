using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Utils
{
    public class SettingAutomapper
    {
        public static void CreateMaps()
        {
            AutoMapper.Mapper.Reset();
            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMissingTypeMaps = true;
                cfg.ValidateInlineMaps = false;
            });
        }
    }
}
