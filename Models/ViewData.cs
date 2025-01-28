using System.Collections.Generic;

namespace CramDown.Models
{
    public class ViewData<T>
    {
        public List<T> data { get; set; }
        public ViewData(List<T> list)
        {
            data = list;
        }
    }
}
