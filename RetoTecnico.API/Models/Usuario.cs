﻿using System;
using System.Collections.Generic;

namespace RetoTecnico.API.Models;

public partial class Usuario
{
    public int Idusuario { get; set; }

    public string? Nombre { get; set; }

    public DateTime? Fechacreacion { get; set; }

    public string? NombreUsuario { get; set; }

    public string? Password { get; set; }

    public int? Idperfil { get; set; }

    public int? Estatus { get; set; }
}
