using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace SeleniumHolded
{
    class Program
    {
        static void Main(string[] args)
        {
            new DriverManager().SetUpDriver(new ChromeConfig());
            IWebDriver driver = new ChromeDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);
            driver.Navigate().GoToUrl("http://google.com");
            new WebDriverWait(driver, TimeSpan.FromSeconds(3)).Until(x => x.FindElement(By.XPath("//div[text()='Aceptar todo']"))).Click();
            driver.FindElement(By.Name("q")).SendKeys("Hatsune Miku");
            new WebDriverWait(driver, TimeSpan.FromSeconds(3)).Until(x => x.FindElement(By.XPath(@"/html/body/div[1]/div[3]/form/div[1]/div[1]/div[4]/center/input[1]"))).Click();
            
            
        }
    }
}   