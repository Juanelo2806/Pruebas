using Clase7.Modelos;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System.Data;
using System.Dynamic;
using Clase7.Modelos;



// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Clase9.Controllers
{
    [Route("Users/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        // GET: api/<UsersController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}
        [HttpGet("patito")]
        public dynamic getAlumnos()
        {
            var con = new MySqlConnection("Data Source=localhost;Database=produccion;User ID=root;Password=");
            string sql = "select * from usuarios";
            con.Open();
            MySqlCommand cmd = new MySqlCommand(sql, con);
            MySqlDataAdapter returnVal = new MySqlDataAdapter(sql, con);
            DataTable dt = new DataTable("usuarios");
            returnVal.Fill(dt);
            con.Close();
            return JsonConvert.SerializeObject(dt);
        }

        [HttpGet("Users/{id}")]
        public dynamic getUsers(int id)
        {
            var con = new MySqlConnection("Data Source=localhost;Database=produccion;User ID=root;Password=");
            string sql = "select * from usuarios where id = " + id;
            Usuarios usuarios = new Usuarios();
            con.Open();
            MySqlDataAdapter returnVal = new MySqlDataAdapter(sql, con);
            DataTable dt = new DataTable("usuarios");
            returnVal.Fill(dt);
            usuarios.Id = (int)dt.Rows[0]["Id"];
            usuarios.Nombre = dt.Rows[0]["Nombre"].ToString();
            usuarios.Apellido = dt.Rows[0]["Apellido"].ToString();
            usuarios.Dpi = (int)dt.Rows[0]["Dpi"];
            con.Close();
            return usuarios;
        }

        [HttpPost("Users/Insert")]
        public IActionResult InsertUser([FromBody] Usuarios usuario)
        {
            var con = new MySqlConnection("Data Source=localhost;Database=produccion;User ID=root;Password=");
            string sql = "INSERT INTO usuarios (Nombre, Apellido, Dpi) VALUES (@Nombre, @Apellido, @Dpi)";
            con.Open();

            MySqlCommand cmd = new MySqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@Nombre", usuario.Nombre);
            cmd.Parameters.AddWithValue("@Apellido", usuario.Apellido);
            cmd.Parameters.AddWithValue("@Dpi", usuario.Dpi);

            int rowsAffected = cmd.ExecuteNonQuery(); 
            con.Close();

            if (rowsAffected > 0)
            {
                return Ok("Usuario insertado correctamente.");
            }
            else
            {
                return BadRequest("No se pudo insertar el usuario.");
            }
        }
        [HttpPut("Users/Update/{id}")]
        public IActionResult UpdateUser(int id, [FromBody] Usuarios usuario)
        {
            var con = new MySqlConnection("Data Source=localhost;Database=produccion;User ID=root;Password=");
            string sql = "UPDATE usuarios SET Nombre = @Nombre, Apellido = @Apellido, Dpi = @Dpi WHERE id = @Id";
            con.Open();

            MySqlCommand cmd = new MySqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@Nombre", usuario.Nombre);
            cmd.Parameters.AddWithValue("@Apellido", usuario.Apellido);
            cmd.Parameters.AddWithValue("@Dpi", usuario.Dpi);

            int rowsAffected = cmd.ExecuteNonQuery(); 
            con.Close();

            if (rowsAffected > 0)
            {
                return Ok("Usuario actualizado correctamente.");
            }
            else
            {
                return NotFound("No se encontró el usuario con el ID proporcionado.");
            }
        }

        [HttpDelete("Users/Delete/{id}")]
        public IActionResult DeleteUser(int id)
        {
            var con = new MySqlConnection("Data Source=localhost;Database=produccion;User ID=root;Password=");
            string sql = "DELETE FROM usuarios WHERE id = @Id";
            con.Open();

            MySqlCommand cmd = new MySqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@Id", id);

            int rowsAffected = cmd.ExecuteNonQuery(); 
            con.Close();

            if (rowsAffected > 0)
            {
                return Ok("Usuario eliminado correctamente.");
            }
            else
            {
                return NotFound("No se encontró el usuario con el ID proporcionado.");
            }
        }

        // POST api/<UsersController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<UsersController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<UsersController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
