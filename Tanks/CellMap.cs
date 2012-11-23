using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tanks
{
    public enum CellMap
    {
        /// <summary>
        /// Стена
        /// </summary>
        Brick,
        /// <summary>
        /// Земля
        /// </summary>
        Ground,
        /// <summary>
        /// Танк компьютера
        /// </summary>
        ComputerTank,
        /// <summary>
        /// Танк пользователя
        /// </summary>
        UserTank,
        /// <summary>
        /// Вода
        /// </summary>
        Water
    }
}
