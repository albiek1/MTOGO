//using OpenQA.Selenium;
//using OpenQA.Selenium.Chrome;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace MTOGO.Tests
//{
//    public class OrderTests : IDisposable
//    {
//        private IWebDriver driver;

//        public OrderTests()
//        {
//            driver = new ChromeDriver();
//        }

//        [Fact]
//        public void CustomerCanPlaceOrder()
//        {
//            // Arrange: Naviger til systemets ordreside
//            driver.Navigate().GoToUrl("http://localhost:5000");

//            // Act: Vælg restaurant og menuvarer
//            var restaurantDropdown = driver.FindElement(By.Id("restaurant-dropdown"));
//            restaurantDropdown.Click();
//            var restaurantOption = driver.FindElement(By.XPath("//option[@value='TestRestaurant']"));
//            restaurantOption.Click();

//            var menuItemCheckbox = driver.FindElement(By.Id("menu-item-1"));
//            menuItemCheckbox.Click();

//            // Indtast leveringsoplysninger og placer ordren
//            driver.FindElement(By.Id("delivery-address")).SendKeys("Test Address");
//            driver.FindElement(By.Id("place-order-button")).Click();

//            // Assert: Bekræft at ordren er oprettet og korrekt
//            var confirmationMessage = driver.FindElement(By.Id("order-confirmation"));
//            Assert.Contains("Order placed successfully", confirmationMessage.Text);
//        }

//        public void Dispose()
//        {
//            driver.Quit();
//        }
//    }

//}
