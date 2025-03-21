﻿using Microsoft.AspNetCore.Identity;
using System.Data;
using System;
using System.ComponentModel.DataAnnotations;

namespace ProyectoApi.Entidades
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }
        public string CURP { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string EmailNormalizado { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public bool EmailVerificado { get; set; }
        public string Role { get; set; } = null!;
        public DateTime FechaRegistro { get; set; }
        public bool Activo { get; set; }
        public int RoleEspecialId { get; set; }
        public DateTime UltimaSesion { get; set; }          
    }
}
