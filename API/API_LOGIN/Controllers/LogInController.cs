using API_LOGIN.Models;
using API_LOGIN.MODELS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace API_LOGIN.Controllers
{
    [Route("api/login")]
    [ApiController]
    public class LogInController : ControllerBase
    {
        private readonly string filePath = "Data/usuarios.json";

        [HttpPost("registro")]
        public IActionResult Registro([FromBody] Usuario nuevoUsuario)
        {
            if (nuevoUsuario == null)
            {
                return BadRequest("Los datos del usuario no son válidos.");
            }

            List<Usuario> usuarios = new List<Usuario>();

            try
            {
                if (System.IO.File.Exists(filePath))
                {
                    string jsonData = System.IO.File.ReadAllText(filePath);
                    usuarios = JsonConvert.DeserializeObject<List<Usuario>>(jsonData) ?? new List<Usuario>();
                }

                usuarios.Add(nuevoUsuario);

                string nuevaDataJson = JsonConvert.SerializeObject(usuarios, Formatting.Indented);

                System.IO.File.WriteAllText(filePath, nuevaDataJson);

                return Ok($"Usuario {nuevoUsuario.nombre} registrado con éxito.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest credenciales)
        {
            if (credenciales == null || string.IsNullOrEmpty(credenciales.email) || string.IsNullOrEmpty(credenciales.contraseña))
            {
                return BadRequest("El email y la contraseña son requeridos.");
            }

            try
            {
                // Leer usuarios del archivo JSON
                if (!System.IO.File.Exists(filePath))
                {
                    return Unauthorized("No hay usuarios registrados.");
                }

                string jsonData = System.IO.File.ReadAllText(filePath);
                var usuarios = JsonConvert.DeserializeObject<List<Usuario>>(jsonData) ?? new List<Usuario>();

                // Validar credenciales
                var usuario = usuarios.FirstOrDefault(u => u.email == credenciales.email && u.contraseña == credenciales.contraseña);
                if (usuario == null)
                {
                    return Unauthorized("Credenciales incorrectas.");
                }

                // Retornar datos del usuario
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }


        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok("API funcionando correctamente.");
        }

    }
}
