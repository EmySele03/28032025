using System.Collections.Generic;

namespace proyecto_progra_s
{
    public class Intento
    {
        public int ParticipanteId { get; set; }

        public List<int> Arranque { get; set; }

        public List<int> Envion { get; set; }

        public List<bool> ArranqueValido { get; set; }

        public List<bool> EnvionValido { get; set; }

        public Intento()
        {
            Arranque = new List<int>();
            Envion = new List<int>();
            ArranqueValido = new List<bool>();
            EnvionValido = new List<bool>();
        }
    }
}
