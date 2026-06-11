using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace proyecto_progra_s
{
    public partial class MainWindow : Window
    {
        private const string ArchivoParticipantes = "Participantes.dat";
        private const string ArchivoResultados = "Resultados.dat";
        private const int CantidadIntentos = 3;

        private readonly List<Participantes> participantes = new List<Participantes>();
        private readonly List<Intento> intentos = new List<Intento>();
        private readonly List<ResultadoGuardado> resultadosGuardados = new List<ResultadoGuardado>();

        public MainWindow()
        {
            InitializeComponent();
            CargarParticipantes();
            CargarResultados();
        }

        //=================================================
        // CARGA Y GUARDADO DE ARCHIVOS
        //=================================================

        private void CargarParticipantes()
        {
            if (!File.Exists(ArchivoParticipantes))
            {
                MessageBox.Show("Primera vez registrando, asegurarse de guardar");
                return;
            }

            try
            {
                participantes.Clear();
                intentos.Clear();

                try
                {
                    CargarParticipantesFormatoActual();
                }
                catch
                {
                    participantes.Clear();
                    intentos.Clear();
                    CargarParticipantesFormatoAnterior();
                }

                ActualizarGridParticipantes();
                ActualizarCombosParticipantes();
                ActualizarGridIntentos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo cargar la lista: " + ex.Message);
            }
        }

        private void CargarParticipantesFormatoActual()
        {
            using (FileStream fs = new FileStream(ArchivoParticipantes, FileMode.Open, FileAccess.Read))
            using (BinaryReader br = new BinaryReader(fs))
            {
                int cantidadParticipantes = br.ReadInt32();

                for (int i = 0; i < cantidadParticipantes; i++)
                {
                    Participantes participante = LeerParticipante(br);
                    participantes.Add(participante);

                    int cantidadIntentosGuardados = br.ReadInt32();

                    for (int j = 0; j < cantidadIntentosGuardados; j++)
                    {
                        intentos.Add(LeerIntento(br));
                    }
                }
            }
        }

        private void CargarParticipantesFormatoAnterior()
        {
            using (FileStream fs = new FileStream(ArchivoParticipantes, FileMode.Open, FileAccess.Read))
            using (BinaryReader br = new BinaryReader(fs))
            {
                int cantidadParticipantes = br.ReadInt32();

                for (int i = 0; i < cantidadParticipantes; i++)
                {
                    participantes.Add(LeerParticipante(br));
                }

                if (fs.Position >= fs.Length)
                {
                    return;
                }

                int cantidadIntentosGuardados = br.ReadInt32();

                for (int i = 0; i < cantidadIntentosGuardados; i++)
                {
                    intentos.Add(LeerIntento(br));
                }
            }
        }

        private void CargarResultados()
        {
            if (!File.Exists(ArchivoResultados))
            {
                return;
            }

            try
            {
                resultadosGuardados.Clear();

                using (FileStream fs = new FileStream(ArchivoResultados, FileMode.Open, FileAccess.Read))
                using (BinaryReader br = new BinaryReader(fs))
                {
                    int cantidad = br.ReadInt32();

                    for (int i = 0; i < cantidad; i++)
                    {
                        resultadosGuardados.Add(new ResultadoGuardado
                        {
                            Id = br.ReadInt32(),
                            Nombre = br.ReadString(),
                            Categoria = br.ReadString(),
                            PesoCategoria = br.ReadString(),
                            Total = br.ReadInt32()
                        });
                    }
                }

                dgResultados.ItemsSource = null;
                dgResultados.ItemsSource = resultadosGuardados
                    .OrderByDescending(r => r.Total)
                    .ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudieron cargar los resultados: " + ex.Message);
            }
        }

        private void BtnGuardarLista_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (FileStream fs = new FileStream(ArchivoParticipantes, FileMode.Create, FileAccess.Write))
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    bw.Write(participantes.Count);

                    foreach (Participantes participante in participantes)
                    {
                        bw.Write(participante.id);
                        bw.Write(participante.Nombre ?? "");
                        bw.Write(participante.Categoria ?? "");
                        bw.Write(participante.PesoCategoria ?? "");

                        List<Intento> intentosDelParticipante = intentos
                            .Where(i => i.ParticipanteId == participante.id)
                            .ToList();

                        bw.Write(intentosDelParticipante.Count);

                        foreach (Intento intento in intentosDelParticipante)
                        {
                            EscribirIntento(bw, intento);
                        }
                    }
                }

                MessageBox.Show("Lista guardada");
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo guardar la lista: " + ex.Message);
            }
        }

        private void BtnGuardarResultados_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<ResultadoGuardado> resultados = ObtenerResultadosOrdenados();

                using (FileStream fs = new FileStream(ArchivoResultados, FileMode.Create, FileAccess.Write))
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    bw.Write(resultados.Count);

                    foreach (ResultadoGuardado resultado in resultados)
                    {
                        bw.Write(resultado.Id);
                        bw.Write(resultado.Nombre ?? "");
                        bw.Write(resultado.Categoria ?? "");
                        bw.Write(resultado.PesoCategoria ?? "");
                        bw.Write(resultado.Total);
                    }
                }

                resultadosGuardados.Clear();
                resultadosGuardados.AddRange(resultados);

                dgResultados.ItemsSource = null;
                dgResultados.ItemsSource = resultadosGuardados;

                MessageBox.Show("Resultados guardados correctamente");
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudieron guardar los resultados: " + ex.Message);
            }
        }

        private Participantes LeerParticipante(BinaryReader br)
        {
            return new Participantes
            {
                id = br.ReadInt32(),
                Nombre = br.ReadString(),
                Categoria = br.ReadString(),
                PesoCategoria = br.ReadString()
            };
        }

        private Intento LeerIntento(BinaryReader br)
        {
            Intento intento = new Intento
            {
                ParticipanteId = br.ReadInt32()
            };

            for (int i = 0; i < CantidadIntentos; i++)
            {
                intento.Arranque.Add(br.ReadInt32());
            }

            for (int i = 0; i < CantidadIntentos; i++)
            {
                intento.Envion.Add(br.ReadInt32());
            }

            InicializarValidaciones(intento);

            return intento;
        }

        private void EscribirIntento(BinaryWriter bw, Intento intento)
        {
            bw.Write(intento.ParticipanteId);

            for (int i = 0; i < CantidadIntentos; i++)
            {
                bw.Write(intento.Arranque[i]);
            }

            for (int i = 0; i < CantidadIntentos; i++)
            {
                bw.Write(intento.Envion[i]);
            }
        }

        //=================================================
        // CRUD DE PARTICIPANTES
        //=================================================

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!TryLeerEntero(txtId, "ID", out int id))
                {
                    return;
                }

                if (BuscarParticipantePorId(id) != null)
                {
                    MessageBox.Show("Ya existe un participante con ese ID. Use el boton Editar para modificarlo.");
                    return;
                }

                if (!DatosParticipanteValidos())
                {
                    return;
                }

                Participantes participante = new Participantes
                {
                    id = id,
                    Nombre = txtNombre.Text.Trim(),
                    Categoria = cbSexo.Text,
                    PesoCategoria = cbPeso.Text
                };

                participantes.Add(participante);

                ActualizarGridParticipantes();
                ActualizarCombosParticipantes();

                MessageBox.Show("Participante registrado");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnEditarParticipante_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Participantes seleccionado = dgParticipantes.SelectedItem as Participantes;

                if (seleccionado == null)
                {
                    MessageBox.Show("Seleccione un participante");
                    return;
                }

                if (!TryLeerEntero(txtId, "ID", out int nuevoId))
                {
                    return;
                }

                Participantes participanteConMismoId = BuscarParticipantePorId(nuevoId);

                if (participanteConMismoId != null && participanteConMismoId != seleccionado)
                {
                    MessageBox.Show("Ya existe otro participante con ese ID.");
                    return;
                }

                if (!DatosParticipanteValidos())
                {
                    return;
                }

                int idAnterior = seleccionado.id;

                seleccionado.id = nuevoId;
                seleccionado.Nombre = txtNombre.Text.Trim();
                seleccionado.Categoria = cbSexo.Text;
                seleccionado.PesoCategoria = cbPeso.Text;

                if (idAnterior != nuevoId)
                {
                    foreach (Intento intento in intentos.Where(i => i.ParticipanteId == idAnterior))
                    {
                        intento.ParticipanteId = nuevoId;
                    }
                }

                ActualizarGridParticipantes();
                ActualizarCombosParticipantes();
                ActualizarGridIntentos();
                MostrarResultados();

                MessageBox.Show("Participante actualizado");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnEliminarParticipante_Click(object sender, RoutedEventArgs e)
        {
            Participantes seleccionado = dgParticipantes.SelectedItem as Participantes;

            if (seleccionado == null)
            {
                MessageBox.Show("Seleccione un participante");
                return;
            }

            participantes.Remove(seleccionado);
            intentos.RemoveAll(i => i.ParticipanteId == seleccionado.id);

            ActualizarGridParticipantes();
            ActualizarCombosParticipantes();
            ActualizarGridIntentos();
            MostrarResultados();

            MessageBox.Show("Participante eliminado");
        }

        private void BtnBuscarParticipante_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!TryLeerEntero(txtId, "ID", out int idBuscado))
                {
                    return;
                }

                Participantes encontrado = BuscarParticipantePorId(idBuscado);

                if (encontrado == null)
                {
                    MessageBox.Show("No se encontro el participante");
                    return;
                }

                MostrarParticipanteEnFormulario(encontrado);

                dgParticipantes.SelectedItem = encontrado;
                dgParticipantes.ScrollIntoView(encontrado);

                MessageBox.Show("Participante encontrado");
            }
            catch
            {
                MessageBox.Show("Ingrese un ID valido");
            }
        }

        private void dgParticipantes_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Participantes participante = dgParticipantes.SelectedItem as Participantes;

            if (participante == null)
            {
                return;
            }

            MostrarParticipanteEnFormulario(participante);
        }

        //=================================================
        // CRUD DE INTENTOS
        //=================================================

        private void BtnGuardarIntentos_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Participantes participante = cbPreparacionParticipante.SelectedItem as Participantes;

                if (participante == null)
                {
                    MessageBox.Show("Seleccione participante");
                    return;
                }

                if (BuscarIntentoPorParticipante(participante.id) != null)
                {
                    MessageBox.Show("Este participante ya tiene intentos guardados. Use el boton Editar para modificarlos.");
                    return;
                }

                if (!TryLeerIntentos(out List<int> arranque, out List<int> envion))
                {
                    return;
                }

                Intento nuevo = new Intento
                {
                    ParticipanteId = participante.id
                };

                nuevo.Arranque.AddRange(arranque);
                nuevo.Envion.AddRange(envion);

                InicializarValidaciones(nuevo);
                intentos.Add(nuevo);

                ActualizarGridIntentos();

                MessageBox.Show("Intentos guardados");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnEditarIntentos_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Participantes participante = cbPreparacionParticipante.SelectedItem as Participantes;

                if (participante == null)
                {
                    MessageBox.Show("Seleccione participante");
                    return;
                }

                Intento intento = BuscarIntentoPorParticipante(participante.id);

                if (intento == null)
                {
                    MessageBox.Show("Este participante no tiene intentos guardados. Use el boton Guardar para registrarlos.");
                    return;
                }

                if (!TryLeerIntentos(out List<int> arranque, out List<int> envion))
                {
                    return;
                }

                intento.Arranque.Clear();
                intento.Arranque.AddRange(arranque);

                intento.Envion.Clear();
                intento.Envion.AddRange(envion);
                AjustarValidaciones(intento);

                ActualizarGridIntentos();

                MessageBox.Show("Intentos actualizados");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnEliminarIntentos_Click(object sender, RoutedEventArgs e)
        {
            Participantes participante = cbPreparacionParticipante.SelectedItem as Participantes;

            if (participante == null)
            {
                MessageBox.Show("Seleccione participante");
                return;
            }

            Intento intento = BuscarIntentoPorParticipante(participante.id);

            if (intento == null)
            {
                MessageBox.Show("Este participante no tiene intentos guardados");
                return;
            }

            intentos.Remove(intento);
            ActualizarGridIntentos();
            MostrarResultados();

            MessageBox.Show("Intentos eliminados");
        }

        //=================================================
        // COMPETENCIA Y RESULTADOS
        //=================================================

        private void BtnBuscar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Participantes participante = cbCompetenciaParticipante.SelectedItem as Participantes;

                if (participante == null)
                {
                    MessageBox.Show("Seleccione participante");
                    return;
                }

                Intento intento = BuscarIntentoPorParticipante(participante.id);

                if (intento == null)
                {
                    MessageBox.Show("No existen intentos para este participante");
                    return;
                }

                lblNombre.Text = participante.Nombre;
                lblCategoria.Text = participante.PesoCategoria;

                chkA1.Content = intento.Arranque[0] + " kg";
                chkA2.Content = intento.Arranque[1] + " kg";
                chkA3.Content = intento.Arranque[2] + " kg";

                chkE1.Content = intento.Envion[0] + " kg";
                chkE2.Content = intento.Envion[1] + " kg";
                chkE3.Content = intento.Envion[2] + " kg";

                chkA1.IsChecked = intento.ArranqueValido[0];
                chkA2.IsChecked = intento.ArranqueValido[1];
                chkA3.IsChecked = intento.ArranqueValido[2];

                chkE1.IsChecked = intento.EnvionValido[0];
                chkE2.IsChecked = intento.EnvionValido[1];
                chkE3.IsChecked = intento.EnvionValido[2];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Participantes participante = cbCompetenciaParticipante.SelectedItem as Participantes;

                if (participante == null)
                {
                    MessageBox.Show("Seleccione participante");
                    return;
                }

                Intento intento = BuscarIntentoPorParticipante(participante.id);

                if (intento == null)
                {
                    MessageBox.Show("No existen intentos");
                    return;
                }

                intento.ArranqueValido[0] = chkA1.IsChecked == true;
                intento.ArranqueValido[1] = chkA2.IsChecked == true;
                intento.ArranqueValido[2] = chkA3.IsChecked == true;

                intento.EnvionValido[0] = chkE1.IsChecked == true;
                intento.EnvionValido[1] = chkE2.IsChecked == true;
                intento.EnvionValido[2] = chkE3.IsChecked == true;

                MostrarResultados();

                MessageBox.Show("Resultados registrados");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void MostrarResultados()
        {
            List<ResultadoGuardado> resultados = ObtenerResultadosOrdenados();

            dgResultados.ItemsSource = null;
            dgResultados.ItemsSource = resultados;
        }

        //=================================================
        // ACTUALIZACION DE PANTALLAS
        //=================================================

        private void ActualizarGridParticipantes()
        {
            dgParticipantes.ItemsSource = null;
            dgParticipantes.ItemsSource = participantes;
        }

        private void ActualizarCombosParticipantes()
        {
            int? preparacionId = (cbPreparacionParticipante.SelectedItem as Participantes)?.id;
            int? competenciaId = (cbCompetenciaParticipante.SelectedItem as Participantes)?.id;

            cbPreparacionParticipante.ItemsSource = null;
            cbPreparacionParticipante.ItemsSource = participantes;
            cbPreparacionParticipante.SelectedItem = participantes.FirstOrDefault(p => p.id == preparacionId);

            cbCompetenciaParticipante.ItemsSource = null;
            cbCompetenciaParticipante.ItemsSource = participantes;
            cbCompetenciaParticipante.SelectedItem = participantes.FirstOrDefault(p => p.id == competenciaId);
        }

        private void ActualizarGridIntentos()
        {
            List<IntentoVista> vista = new List<IntentoVista>();

            foreach (Intento intento in intentos)
            {
                Participantes participante = BuscarParticipantePorId(intento.ParticipanteId);

                vista.Add(new IntentoVista
                {
                    Nombre = participante?.Nombre ?? "",
                    A1 = intento.Arranque[0],
                    A2 = intento.Arranque[1],
                    A3 = intento.Arranque[2],
                    E1 = intento.Envion[0],
                    E2 = intento.Envion[1],
                    E3 = intento.Envion[2]
                });
            }

            dgIntentos.ItemsSource = null;
            dgIntentos.ItemsSource = vista;
        }

        private void MostrarParticipanteEnFormulario(Participantes participante)
        {
            txtId.Text = participante.id.ToString();
            txtNombre.Text = participante.Nombre;
            cbSexo.Text = participante.Categoria;
            cbPeso.Text = participante.PesoCategoria;
        }

        //=================================================
        // HELPERS
        //=================================================

        private Participantes BuscarParticipantePorId(int id)
        {
            return participantes.FirstOrDefault(p => p.id == id);
        }

        private Intento BuscarIntentoPorParticipante(int participanteId)
        {
            return intentos.FirstOrDefault(i => i.ParticipanteId == participanteId);
        }

        private bool DatosParticipanteValidos()
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("Ingrese el nombre del participante");
                txtNombre.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(cbSexo.Text))
            {
                MessageBox.Show("Seleccione la categoria");
                cbSexo.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(cbPeso.Text))
            {
                MessageBox.Show("Seleccione la categoria de peso");
                cbPeso.Focus();
                return false;
            }

            return true;
        }

        private bool TryLeerIntentos(out List<int> arranque, out List<int> envion)
        {
            arranque = new List<int>();
            envion = new List<int>();

            if (!TryLeerEntero(txtA1, "Arranque 1", out int a1) ||
                !TryLeerEntero(txtA2, "Arranque 2", out int a2) ||
                !TryLeerEntero(txtA3, "Arranque 3", out int a3) ||
                !TryLeerEntero(txtE1, "Envion 1", out int e1) ||
                !TryLeerEntero(txtE2, "Envion 2", out int e2) ||
                !TryLeerEntero(txtE3, "Envion 3", out int e3))
            {
                return false;
            }

            arranque.Add(a1);
            arranque.Add(a2);
            arranque.Add(a3);

            envion.Add(e1);
            envion.Add(e2);
            envion.Add(e3);

            return true;
        }

        private bool TryLeerEntero(TextBox control, string campo, out int valor)
        {
            if (!int.TryParse(control.Text, out valor))
            {
                MessageBox.Show("Ingrese un valor valido para " + campo);
                control.Focus();
                return false;
            }

            return true;
        }

        private void InicializarValidaciones(Intento intento)
        {
            intento.ArranqueValido.Clear();
            intento.EnvionValido.Clear();

            for (int i = 0; i < CantidadIntentos; i++)
            {
                intento.ArranqueValido.Add(false);
                intento.EnvionValido.Add(false);
            }
        }

        private void AjustarValidaciones(Intento intento)
        {
            while (intento.ArranqueValido.Count < CantidadIntentos)
            {
                intento.ArranqueValido.Add(false);
            }

            while (intento.EnvionValido.Count < CantidadIntentos)
            {
                intento.EnvionValido.Add(false);
            }

            while (intento.ArranqueValido.Count > CantidadIntentos)
            {
                intento.ArranqueValido.RemoveAt(intento.ArranqueValido.Count - 1);
            }

            while (intento.EnvionValido.Count > CantidadIntentos)
            {
                intento.EnvionValido.RemoveAt(intento.EnvionValido.Count - 1);
            }
        }

        private List<ResultadoGuardado> ObtenerResultadosOrdenados()
        {
            return participantes
                .Select(CrearResultado)
                .OrderByDescending(r => r.Total)
                .ToList();
        }

        private ResultadoGuardado CrearResultado(Participantes participante)
        {
            Intento intento = BuscarIntentoPorParticipante(participante.id);

            return new ResultadoGuardado
            {
                Id = participante.id,
                Nombre = participante.Nombre,
                Categoria = participante.Categoria,
                PesoCategoria = participante.PesoCategoria,
                Total = intento == null ? 0 : CalcularTotal(intento)
            };
        }

        private int CalcularTotal(Intento intento)
        {
            // En halterofilia el total se calcula con el mejor arranque valido y el mejor envion valido.
            return ObtenerMejorIntentoValido(intento.Arranque, intento.ArranqueValido) +
                   ObtenerMejorIntentoValido(intento.Envion, intento.EnvionValido);
        }

        private int ObtenerMejorIntentoValido(List<int> pesos, List<bool> validaciones)
        {
            int mejor = 0;

            for (int i = 0; i < CantidadIntentos && i < pesos.Count && i < validaciones.Count; i++)
            {
                if (validaciones[i] && pesos[i] > mejor)
                {
                    mejor = pesos[i];
                }
            }

            return mejor;
        }

        private sealed class IntentoVista
        {
            public string Nombre { get; set; }
            public int A1 { get; set; }
            public int A2 { get; set; }
            public int A3 { get; set; }
            public int E1 { get; set; }
            public int E2 { get; set; }
            public int E3 { get; set; }
        }

        private sealed class ResultadoGuardado
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
            public string Categoria { get; set; }
            public string PesoCategoria { get; set; }
            public int Total { get; set; }
        }
    }
}
