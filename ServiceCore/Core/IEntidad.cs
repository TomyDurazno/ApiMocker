using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceCore
{
    public interface IEntidad
    {
        bool Eliminado { get; set; }
        DateTime? FechaCreacion { get; set; }
        DateTime? FechaEdicion { get; set; }

        void ReinicializarLista(string[] nombres);

        int GetId();
    }

    public abstract class Entidad : IEntidad
    {
        [Key]
        public int Id { get; set; }

        public bool Eliminado { get; set; }

        public abstract void ReinicializarLista(string[] nombres);

        public abstract int GetId();

        [Column(TypeName = "DateTime2")]
        public DateTime? FechaCreacion { get; set; }

        [Column(TypeName = "DateTime2")]
        public DateTime? FechaEdicion { get; set; }
    }

    public class EntidadComparer : IEqualityComparer<Entidad>
    {
        public bool Equals(Entidad x, Entidad y)
        {
            return x.Id.Equals(y.Id);
        }

        public int GetHashCode(Entidad obj)
        {
            return obj.GetHashCode();
        }
    }
}
