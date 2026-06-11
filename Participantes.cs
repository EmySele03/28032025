namespace proyecto_progra_s
{
    public class Participantes
    {
        public int id { get; set; }

        public string Nombre { get; set; }

        public string Categoria { get; set; }

        public string PesoCategoria { get; set; }

        public override string ToString()
        {
            return Nombre + " - " + id;
        }
    }
}
