using Microsoft.Data.SqlClient;
using Npgsql;
using Dapper;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


//string connectionString = "Server=localhost;Database=TestData;User Id=sa;Password=dev_2020!;TrustServerCertificate=True;Encrypt=false";
string connectionString = "Host=localhost;Database=postgres;Port=5432;Username=postgres;Password=1111";

app.MapGet("/article", () =>
{
    string sql = "SELECT * FROM Article";
    using SqlConnection con = new SqlConnection(connectionString);

    var res = con.Query<Article>(sql);

    return res;
    
})
.WithName("GetArticle")
.WithOpenApi();

app.MapGet("/article/{id}", (int id) => {
    string sql = "SELECT * FROM Article WHERE id = @Id";
    var categories = new List<Article>();
    SqlConnection con = new SqlConnection(connectionString);

    var res = con.Query<Article>(sql, new {Id = id});

    return res;

});

app.MapPost("/article", (Article article) =>
{
    string sql = "INSERT INTO Article (Title, Url, Content) VALUES (@Title, @Url, @Content)";
    var categories = new List<Article>();
    SqlConnection con = new SqlConnection(connectionString);

    var res = con.Execute(sql, new { Title = article.TItle, Url = article.Url, Content = article.Content });

    return article;
});

app.MapPut("/article/{id}", (Article article, int id) =>
{
    string sql = "UPDATE Article SET Title = @Title, Url = @Url, Content = @Content WHERE Id = @Id";
    var categories = new List<Article>();
    SqlConnection con = new SqlConnection(connectionString);

    var res = con.Execute(sql, new { Title = article.TItle, Url = article.Url, Content = article.Content,  Id = id});

    return res;
});

app.MapDelete("/article/{id}", (int id) =>
{
    string sql = "DELETE FROM Article WHERE Id = @Id";
    var categories = new List<Article>();
    SqlConnection con = new SqlConnection(connectionString);

    var res = con.Query<Article>(sql, new { Id = id });

    return res;
});

app.Run();

public record Article
{
    public int Id { get; set; }
    public string TItle { get; set; }
    public string Url { get; set; }
    public string Content { get; set; }
}

