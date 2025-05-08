using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Спорт_тесты
{
    public class вопрос
    {
        public int идентификатор { get; set; }
        public string содержание { get; set; }
        public int вес { get; set; }

        public ObservableCollection<ответ> ответы { get; set; } = new ObservableCollection<ответ>();
    }
}
