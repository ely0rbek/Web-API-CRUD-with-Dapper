using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using PracAPI.Models;
using System.Reflection;
using System.Xml.Linq;

namespace PracAPI.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class ExperimentController : ControllerBase
    {
        public string connectionString = "Host=localhost;Port=5432;Database=Testdb;User Id=postgres;Password=1234";


        //--------------------------------------------------------------------------------




        [HttpGet]
        public IEnumerable<Experiment> GetWithDapper()
        {
            using NpgsqlConnection connection = new NpgsqlConnection(connectionString);
            connection.Open();
            IEnumerable<Experiment>? experiments = connection.Query<Experiment>("select * from experiment;");
            connection.Close();
            return experiments;
        }

        [HttpGet]
        public IEnumerable<Experiment> GetWithADO()
        {
            using NpgsqlConnection connection = new NpgsqlConnection(connectionString);
            connection.Open();

            using NpgsqlCommand cmd = new NpgsqlCommand("select * from experiment;", connection);
            List<Experiment> list= new List<Experiment>();
            var result = cmd.ExecuteReader();
            while (result.Read())
            {
                list.Add(new Experiment()
                {
                    Id=(int)result["Id"],
                    Name=(string)result["name"],
                    age=(int)result["age"]
                });
            }
            connection.Close();
            return list;
        }


        //--------------------------------------------------------------------------------



        [HttpPost]
        public ExperimentDTO PostWithDapper(ExperimentDTO model)
        {
            using NpgsqlConnection connection = new NpgsqlConnection(connectionString);
            connection.Open();
            var count = connection.Execute($"insert into experiment(name,age) values(@name,@age);",new {Name= model.Name ,age= model.age });
            connection.Close();
            return model;
        }

        [HttpPost]
        public ExperimentDTO PostWithADO(ExperimentDTO model)
        {
            using NpgsqlConnection connection = new NpgsqlConnection(connectionString);
            connection.Open();
            using NpgsqlCommand cmd = new NpgsqlCommand($"insert into experiment(name,age) values('{model.Name}','{model.age}');", connection);
            cmd.ExecuteNonQuery();
            connection.Close();
            return model;
        }


        //--------------------------------------------------------------------------------





        [HttpDelete]
        public string DeleteWithDapper(int id)
        {
            using NpgsqlConnection connection = new NpgsqlConnection(connectionString);
            connection.Open();
            var count = connection.Execute($"delete from experiment where Id=@id;", new { id=id });
            connection.Close();
            return $"{id} idli shaxs muvofaqqiyatli o'chirildi";
        }


        [HttpDelete]
        public string DeleteWithADO(int id)
        {
            using NpgsqlConnection connection = new NpgsqlConnection(connectionString);
            connection.Open();
            //var count = connection.Execute($"delete from experiment where Id=@id;", new { id = id });
            using NpgsqlCommand cmd = new NpgsqlCommand($"delete from experiment where id=@id;", connection);
            cmd.Parameters.AddWithValue("id", id);
            cmd.ExecuteNonQuery();
            connection.Close();
            return $"{id} idli shaxs muvofaqqiyatli o'chirildi";
        }


        //--------------------------------------------------------------------------------


        [HttpPut]
        public string PutWithDapper(int id, ExperimentDTO model)
        {
            using NpgsqlConnection connection = new NpgsqlConnection(connectionString);
            connection.Open();
            var count = connection.Execute($"update experiment set Name=@Name, age=@age where Id=@id;", new { Name = model.Name,age=model.age, id = id });
            connection.Close();
            return $"{id} idli user malumotlari to'liq o'zgardi";
        }


        [HttpPut]
        public string PutWithADO(int id, ExperimentDTO model)
        {
            using NpgsqlConnection connection = new NpgsqlConnection(connectionString);
            using NpgsqlCommand cmd = new NpgsqlCommand($"update experiment set name=@name,age=@age where id=@id", connection);
            cmd.Parameters.AddWithValue("name", model.Name);
            cmd.Parameters.AddWithValue("age", model.age);
            cmd.Parameters.AddWithValue("id", id);
            cmd.ExecuteNonQuery();
            connection.Close();
            return $"{id} idli user malumotlari to'liq o'zgardi";
        }



        //--------------------------------------------------------------------------------



        [HttpPatch]
        public string PatchWithDapper(int id,string newName)
        {
            using NpgsqlConnection connection = new NpgsqlConnection(connectionString);
            connection.Open();
            var count = connection.Execute($"update experiment set Name=@newName  where Id=@id;", new { newName=newName,id = id });
            connection.Close();
            return $"{id} idli username {newName} ga o'zgardi.";
        }


        [HttpPatch]
        public string PatchWithADO(int id, string newName)
        {
            using NpgsqlConnection connection = new NpgsqlConnection(connectionString);
            connection.Open();
            using NpgsqlCommand cmd = new NpgsqlCommand($"update experiment set name=@name where id=@id", connection);
            cmd.Parameters.AddWithValue("name", newName);
            cmd.Parameters.AddWithValue("id", id);
            cmd.ExecuteNonQuery();
            connection.Close();
            return $"{id} idli username {newName} ga o'zgardi.";
        }
    }
}
