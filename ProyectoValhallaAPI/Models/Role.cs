using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ProyectoValhallaAPI.Models;

public partial class Role
{
    public int IdRol { get; set; }

    public string? NombreRol { get; set; }

    public string? Descripcion { get; set; }

    [JsonIgnore]
    public virtual ICollection<RolesPermiso> RolesPermisos { get; set; } = new List<RolesPermiso>();
    [JsonIgnore]
    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
