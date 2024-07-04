using Microsoft.AspNetCore.Mvc;
using Dapper;
using System.Threading.Tasks;
using MySqlConnector;

namespace SummerSchool.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly string _connectionString;

        public ArticleController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        /// <summary>
        /// Lấy ra toàn bộ bài báo
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Article>>> GetAllArticles()
        {
            using var connection = new MySqlConnection(_connectionString);
            var sql = "SELECT * FROM articles  ORDER BY Created_at DESC";
            var articles = await connection.QueryAsync<Article>(sql);

            return Ok(articles);
        }
        /// <summary>
        /// Lấy bài báo theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Article>> GetArticleById(int id)
        {
            using var connection = new MySqlConnection(_connectionString);
            var sql = "SELECT * FROM articles WHERE id = @Id";
            var article = await connection.QueryFirstOrDefaultAsync<Article>(sql, new { Id = id });

            if (article == null)
            {
                return NotFound(new { message = "Article not found" });
            }

            return Ok(article);
        }
        /// <summary>
        /// Lấy ra toàn bộ bài báo
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        [HttpGet("article/{articleId}")]
        public async Task<ActionResult<IEnumerable<ArticleSection>>> GetSectionsByArticleId(int articleId)
        {
            using var connection = new MySqlConnection(_connectionString);
            var sql = "SELECT * FROM article_sections WHERE article_id = @ArticleId";
            var sections = await connection.QueryAsync<ArticleSection>(sql, new { ArticleId = articleId });

            if (sections == null || !sections.Any())
            {
                return NotFound(new { message = "Sections not found" });
            }

            return Ok(sections);
        }
        /// <summary>
        /// Đăng bài báo mới
        /// </summary>
        /// <param name="articleRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateArticle([FromBody] ArticleRequest articleRequest)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();
            using var transaction = connection.BeginTransaction();

            try
            {
                // Thêm bài báo vào bảng articles
                var sqlArticle = @"INSERT INTO articles (title, created_at, updated_at, author_id, thumbnail, descr) 
                           VALUES (@Title, @Created_at, @Updated_at, @Author_id, @Thumbnail, @Descr)";
                var resultArticle = await connection.ExecuteAsync(sqlArticle, new
                {
                    articleRequest.Title,
                    Created_at = DateTime.Now,
                    Updated_at = DateTime.Now,
                    articleRequest.Author_id,
                    articleRequest.Thumbnail,
                    articleRequest.Descr
                }, transaction);

                if (resultArticle <= 0)
                {
                    transaction.Rollback();
                    return BadRequest(new { message = "Error creating article" });
                }

                var articleId = await connection.QuerySingleAsync<int>("SELECT LAST_INSERT_ID()", transaction: transaction);

                // Kiểm tra xem Sections có null hay không và lặp qua các phần của bài báo
                if (articleRequest.Sections != null && articleRequest.Sections.Count > 0)
                {
                    foreach (var section in articleRequest.Sections)
                    {
                        var sqlSection = @"INSERT INTO article_sections (article_id, section_name, content, image_url, image_name) 
                                   VALUES (@Article_id, @Section_name, @Content, @Image_url, @Image_name)";
                        await connection.ExecuteAsync(sqlSection, new
                        {
                            Article_id = articleId,
                            section.Section_name,
                            section.Content,
                            section.Image_url,
                            section.Image_name
                        }, transaction);
                    }
                }

                transaction.Commit();
                return Ok(new { message = "Article created successfully" });
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }
        /// <summary>
        /// Xoá 1 bài báo theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();
            using var transaction = connection.BeginTransaction();

            try
            {
                // Xóa các phần của bài báo trước
                var sqlDeleteSections = "DELETE FROM article_sections WHERE article_id = @ArticleId";
                await connection.ExecuteAsync(sqlDeleteSections, new { ArticleId = id }, transaction);

                // Xóa bài báo
                var sqlDeleteArticle = "DELETE FROM articles WHERE id = @Id";
                var result = await connection.ExecuteAsync(sqlDeleteArticle, new { Id = id }, transaction);

                if (result == 0)
                {
                    transaction.Rollback();
                    return NotFound(new { message = "Article not found" });
                }

                transaction.Commit();
                return Ok(new { message = "Article deleted successfully" });
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

    }

    public class ArticleRequest
    {
        public string Title { get; set; }
        public int Author_id { get; set; }
        public string Thumbnail { get; set; }
        public string Descr { get; set; }
        public List<SectionRequest> Sections { get; set; }
    }

    public class SectionRequest
    {
        public string Section_name { get; set; }
        public string Content { get; set; }
        public string Image_url { get; set; }
        public string Image_name { get; set; }
    }
}
