using System;
using System.Collections.Generic;

namespace RetoTecnico.API.Models;

public partial class Formasfarmaceutica
{
    public int Idformafarmaceutica { get; set; }

    public string? Nombre { get; set; }

    public int? Habilitado { get; set; }

    public virtual ICollection<Medicamento> Medicamentos { get; set; } = new List<Medicamento>();
}
