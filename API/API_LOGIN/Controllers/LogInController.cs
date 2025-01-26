using API_LOGIN.MODELS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace API_LOGIN.Controllers
{
    [Route("api/[controller]")]
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
        public IActionResult Login([FromBody] Usuario credenciales)
        {
            if (credenciales == null)
            {
                return BadRequest("Las credenciales no son válidas.");
            }

            try
            {

                string jsonData = System.IO.File.ReadAllText(filePath);
                List<Usuario> usuarios = JsonConvert.DeserializeObject<List<Usuario>>(jsonData) ?? new List<Usuario>();

                var usuario = usuarios.FirstOrDefault(u => u.nombre == credenciales.nombre && u.contraseña == credenciales.contraseña);

                if (usuario != null)
                {
                    return Ok($"Bienvenido {usuario.nombre} {usuario.apellido}");
                }
                else
                {
                    return Unauthorized("Credenciales incorrectas.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}
