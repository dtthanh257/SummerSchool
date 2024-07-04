using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

using MySqlConnector;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SummerSchool.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly string _connectionString;

        public UserController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        /// <summary>
        /// Lấy ra toàn bộ danh sách user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<User>> GetUsers()
        {
            using var connection = new MySqlConnection(_connectionString);
            var sql = "SELECT * FROM user";
            var result = await connection.QueryAsync<User>(sql);
            return result;
        }
        /// <summary>
        /// Lấy ra user theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            using var connection = new MySqlConnection(_connectionString);
            var sql = "SELECT * FROM user WHERE id = @Id";
            var user = await connection.QueryFirstOrDefaultAsync<User>(sql, new { Id = id });

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }
        /// <summary>
        /// Đăng ký thành viên mới
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] User user)
        {
            using var connection = new MySqlConnection(_connectionString);
            var sql = @"INSERT INTO user (name, nickname, password, gender, dob, object) 
                        VALUES (@Name, @Nickname, @Password, @Gender, @Dob, @Object )";
            var result = await connection.ExecuteAsync(sql, user);

            if (result > 0)
            {
                return Ok(new { message = "User registered successfully" });
            }

            return BadRequest(new { message = "Error registering user" });
        }
    }
}
