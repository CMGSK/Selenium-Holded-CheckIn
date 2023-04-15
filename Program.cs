using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace SeleniumHolded
{
    class Program
    {
        const string EMAIL = "";
        const string PASSWORD = "";

        static void Main(string[] args)
        {
            //Util creation
            new DriverManager().SetUpDriver(new ChromeConfig());
            IWebDriver driver = new ChromeDriver();
            Actions action = new Actions(driver);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);
                
            //Access holded and log in
            driver.Navigate().GoToUrl("https://app.holded.com/login?lang=es");
            new WebDriverWait(driver, TimeSpan.FromSeconds(5))
                .Until(x => x.FindElement(By.XPath("//*[@id=\"login-email\"]")))
                .Click();
            driver.FindElement(By.Name("login-email")).SendKeys(EMAIL);
            driver.FindElement(By.XPath("//*[@id=\"login-password\"]")).Click();
            driver.FindElement(By.Name("login-password")).SendKeys(PASSWORD); 
            new WebDriverWait(driver, TimeSpan.FromMilliseconds(100));
            driver.FindElement(By.XPath("//*[@id=\"form-login\"]/div/div[4]/button"))
            .Click();

            //Access check-in tab 
            new WebDriverWait(driver, TimeSpan.FromSeconds(10))
                .Until(x => x.FindElement(By.XPath("//*[@id=\"teamViewWrapper\"]/div[1]/div/div[2]/div[2]")))
                .Click(); 



                //////////////////////////////////////////////////////////////////////////
            new WebDriverWait(driver, TimeSpan.FromSeconds(10))
                .Until(x => x.FindElement(By.ClassName("//*[@id=\"timetable\"]/div[2]/div[2]/div[18]")))
                .Click(); 
            new WebDriverWait(driver, TimeSpan.FromSeconds(10))
                .Until(x => x.FindElement(By.XPath("//*[@id=\"daySummaryPopup\"]/button[1]")))
                .Click(); 


            // Daily
            driver.FindElement(By.XPath("/html/body/div[2]/div[3]/div/div[2]/div[2]/div[2]/div/div[3]/div/div[2]/div/div[2]/div[2]/div[18]/div/div[4]/div/div/div[2]/div[1]/input"))
                .Click(); 
            action.SendKeys("08:30");
            driver.FindElement(By.XPath("/html/body/div[2]/div[3]/div/div[2]/div[2]/div[2]/div/div[3]/div/div[2]/div/div[2]/div[2]/div[18]/div/div[4]/div/div/div[2]/div[1]/input"))
               .Click();
            action.SendKeys(Keys.Control + "a")
                .SendKeys("14:00");
            
        }
    }
}   