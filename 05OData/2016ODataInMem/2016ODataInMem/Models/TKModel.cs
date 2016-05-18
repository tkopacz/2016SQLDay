using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2016ODataInMem.Models
{
    public class TKModelEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime Dt { get; set; }
    }

    public class TKRepository
    {
        List<TKModelEntity> m_lst;
        public TKRepository()
        {
            m_lst = new List<TKModelEntity>();
            for (int i = 0; i < 500; i++)
            {
                m_lst.Add(new TKModelEntity() { Id=i,Dt=new DateTime(2016,01,01).AddHours(i*13),Name=$"Name{i}", Surname = $"Surname{i % 9}" });
            }
        }

        public List<TKModelEntity> Get() { return m_lst; }
    }
}
