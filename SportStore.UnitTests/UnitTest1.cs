using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using SportsStore.WebUI.Controllers;
using SportsStore.WebUI.Models;
using System;
using SportsStore.WebUI.HtmlHelpers;
using System.Web.Mvc;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Can_Paginate()
        {
            // arrage
            Mock<IProductRepository> mock = new Mock<IProductRepository>();

            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product {ProductId = 1, Name = "P1"},
                new Product {ProductId = 2, Name = "P2"},
                new Product {ProductId = 3, Name = "P3"},
                new Product {ProductId = 4, Name = "P4"},
                new Product {ProductId = 5, Name = "P5"}
            }.AsQueryable());

            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            //act
            // zmiana modelu 
    //        IEnumerable<Product> result = (IEnumerable<Product>)controller.List(1).Model;       //wywołuje metode list z parametrem 2
    //                                                                                            // typ taki sam jaki jest na widoku model 
            ProductListViewModel result = (ProductListViewModel)controller.List(null,1).Model;       

            //assert

            Product[] prodArray = result.Products.ToArray();
            Assert.IsTrue(prodArray.Length == 3);
            Assert.AreEqual(prodArray[0].Name, "P1");
            Assert.AreEqual(prodArray[1].Name, "P2");
            Assert.AreEqual(prodArray[2].Name, "P3");
        }


        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            //arrage
            HtmlHelper myHelper = null;

            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };

            Func<int, string> pageUrlDelegate = i => "Strona" + i;

            //act
            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

            Assert.AreEqual(result.ToString(), @"<a href=""Strona1"">1</a>"
                +@"<a class=""selected"" href=""Strona2"">2</a>"
                +@"<a href=""Strona3"">3</a>");
        }

        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            //arrate
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product {ProductId =1, Name="P1"},
                new Product {ProductId =2, Name="P2"},
                new Product {ProductId =3, Name="P3"},
                new Product {ProductId =4, Name="P4"},
                new Product {ProductId =5, Name="P5"}
            }.AsQueryable());

            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            //act
            ProductListViewModel result = (ProductListViewModel)controller.List(null,2).Model;

            //assert
            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual((int)pageInfo.CurrentPage, 2);
        }

        [TestMethod]
        public void Can_Filter_Products()
        {
            // arrage
            Mock<IProductRepository> mock = new Mock<IProductRepository>();

            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product {ProductId = 1, Name = "P1", Category = "Cat1"},
                new Product {ProductId = 2, Name = "P2", Category = "Cat2"},
                new Product {ProductId = 3, Name = "P3", Category = "Cat1"},
                new Product {ProductId = 4, Name = "P4", Category = "Cat2"},
                new Product {ProductId = 5, Name = "P5", Category = "Cat3"}
            }.AsQueryable());

            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            //act
            Product[] result = ((ProductListViewModel)controller.List("Cat2", 1).Model).Products.ToArray();

            
            Assert.AreEqual(result.Length, 2);
            Assert.IsTrue(result[0].Name == "P2" && result[0].Category == "Cat2");
            Assert.IsTrue(result[1].Name == "P4" && result[1].Category == "Cat2");


        }

        [TestMethod]
        public void Can_Create_Categories()
        {
            // arrage
            Mock<IProductRepository> mock = new Mock<IProductRepository>();

            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product {ProductId = 1, Name = "P1", Category = "Jabłka"},
                new Product {ProductId = 2, Name = "P2", Category = "Jabłka"},
                new Product {ProductId = 3, Name = "P3", Category = "Śliwki"},
                new Product {ProductId = 4, Name = "P4", Category = "Pomarańcze"}
            }.AsQueryable());

            NavController target = new NavController(mock.Object);

            //act       //munu to nazwa Akcji
            string[] result = ((IEnumerable<string>)target.Menu().Model).ToArray();

            //assert

            Assert.AreEqual(result.Length, 3);
            Assert.AreEqual(result[0], "Jabłka");
            Assert.AreEqual(result[1], "Pomarańcze");
            Assert.AreEqual(result[2], "Śliwki");

        }

        [TestMethod]
        public void Indicates_Sekected_Category()
        {
            //arrate
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product { ProductId = 1, Name = "P1", Category ="Jabłka"},
                new Product { ProductId = 2, Name = "P2", Category = "Pomarańcze"}
            }.AsQueryable());

            NavController target = new NavController(mock.Object);

            string categoryToSelect = "Jabłka";

            //act
            string result = target.Menu(categoryToSelect).ViewBag.SelectedCategory;

            //assert
            Assert.AreEqual(categoryToSelect, result);

        }

        [TestMethod]
        public void Generate_Category_Specific_Product_Count()
        {
            //arrate
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product { ProductId = 1, Name = "P1", Category = "Cat1"},
                new Product { ProductId = 2, Name = "P2", Category = "Cat2"},
                new Product { ProductId = 3, Name = "P3", Category = "Cat1"},
                new Product { ProductId = 4, Name = "P4", Category = "Cat2"},
                new Product { ProductId = 5, Name = "P5", Category = "Cat3"}
            }.AsQueryable());

            ProductController target = new ProductController(mock.Object);
            target.PageSize = 3;

            //act
            int res1 = ((ProductListViewModel)target.List("Cat1").Model).PagingInfo.TotalItems;
            int res2 = ((ProductListViewModel)target.List("Cat2").Model).PagingInfo.TotalItems;
            int res3 = ((ProductListViewModel)target.List("Cat3").Model).PagingInfo.TotalItems;
            int resAll = ((ProductListViewModel)target.List(null).Model).PagingInfo.TotalItems;

            //assert

            Assert.AreEqual(res1, 2);
            Assert.AreEqual(res2, 2);
            Assert.AreEqual(res3, 1);
            Assert.AreEqual(resAll, 5);
        }



    }
}
