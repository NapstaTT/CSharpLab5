///FIXME:
/// - Добавлена XML-документация
/// - Приведены скобки к стилю Allman
/// - Удалены лишние пустые строки
/// - Переименованы параметры конструктора в camelCase
/// - Улучшен ToString для читаемости

namespace lab5.Data.Models
{
    /// <summary>
    /// Представляет достижения футбольного клуба.
    /// </summary>
    public class Achievement
    {
        /// <summary>
        /// Уникальный идентификатор достижения.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Идентификатор клуба, которому принадлежат достижения.
        /// </summary>
        public int ClubId { get; set; }

        public int G { get; set; }
        public int S { get; set; }
        public int B { get; set; }
        public int C { get; set; }
        public int FC { get; set; }
        public int LC { get; set; }
        public int FLC { get; set; }
        public int LE { get; set; }
        public int FLE { get; set; }
        public int COC { get; set; }
        public int FCOC { get; set; }
        public int LK { get; set; }
        public int FLK { get; set; }

        /// <summary>
        /// Создаёт пустой объект достижений.
        /// </summary>
        public Achievement()
        {
        }

        /// <summary>
        /// Создаёт объект достижений с основными параметрами.
        /// </summary>
        public Achievement(int id, int clubId, int gold = 0, int silver = 0, int bronze = 0, int cups = 0)
        {
            Id = id;
            ClubId = clubId;
            G = gold;
            S = silver;
            B = bronze;
            C = cups;
        }

        /// <summary>
        /// Возвращает строковое представление объекта.
        /// </summary>
        public override string ToString()
        {
            return
                $"Club {ClubId}: Gold={G}, Silver={S}, Bronze={B}, Cups={C}, " +
                $"Lost Final Cups={FC}, League of Champions={LC}, Final LC={FLC}, " +
                $"League of Europe={LE}, Final LE={FLE}, Cup Winners Cup={COC}, Final CWC={FCOC}, " +
                $"Confederations League={LK}, Final Conf. League={FLK}";
        }
    }
}
