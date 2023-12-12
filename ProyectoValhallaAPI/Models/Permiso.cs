using System;
using System.Collections.Generic;

namespace ProyectoValhallaAPI.Models;

public partial class Permiso
{
    public int IdPermiso { get; set; }

    public string? NombreModulo { get; set; }

    public string? NombrePermiso { get; set; }

    public virtual ICollection<RolesPermiso> RolesPermisos { get; set; } = new List<RolesPermiso>();
}
