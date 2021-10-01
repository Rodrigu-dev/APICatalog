using APICatalogo.Context;
using APICatalogo.Controllers;
using APICatalogo.DTOs;
using APICatalogo.DTOs.Mappings;
using APICatalogo.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;

namespace APICatalogoxUnitTests
{
    public class CategoriasUnitTestController
    {
        private IMapper mapper;
        private IUnitOfWork repository;

        //Propriedade estática que trabalha com uma instância do meu contexto:
        public static DbContextOptions<AppDbContext> dbContextOptions { get; }

        //String de conexão
        public static string connectionString =
            "Server=localhost;DataBase=CatalogoDB;Uid=root;Pwd=b0mdiar0";

        //Construtor estático para inicializar o dbContextOptions
        static CategoriasUnitTestController()
        {
            dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
                .Options;
        }
        public CategoriasUnitTestController()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            mapper = config.CreateMapper();

            var context = new AppDbContext(dbContextOptions);

            repository = new UnitOfWork(context);
        }

        //Testes Unitários========================================================
        // testar o método GET
        //====================================Get(int id) ========================
        [Fact]
        public async Task GetCategoriaById_Return_OkResult()
        {
            //Arrange  
            var controller = new CategoriasController(repository, mapper);
            var catId = 2;

            //Act  
            var data = await controller.GetById(catId);
            Console.WriteLine(data);

            //Assert  
            Assert.IsType<CategoriaDTO>(data.Value);
        }

        [Fact]
        public async Task GetCategoriaById_Return_BadRequestResult()
        {
            //Arrange  
            var controller = new CategoriasController(repository, mapper);
            int? catId = null;

            //Act  
            var data = await controller.GetById(catId);

            //Assert  
            Assert.IsType<BadRequestResult>(data.Result);
        }

        //====================================Post=====================================
        [Fact]
        public async Task Post_Categoria_AddValidData_Return_CreatedResult()
        {
            //Arrange  
            var controller = new CategoriasController(repository, mapper);

            var cat = new CategoriaDTO() { Nome = "Teste Unitario 1", ImagemUrl = "testecat.jpg" };

            //Act  
            var data = await controller.Post(cat);

            //Assert  
            Assert.IsType<CreatedAtRouteResult>(data);
        }

        


    }
}
