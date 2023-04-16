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
        static void Main(string[] args)
        {
            //Loading personal Login info
            var root = Directory.GetCurrentDirectory();
            var dotEnv = Path.Combine(root, ".env");
            DotEnv.Load(dotEnv);

            //Util creation
            new DriverManager().SetUpDriver(new ChromeConfig());
            IWebDriver driver = new ChromeDriver();
            Actions action = new Actions(driver);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);
                
            //Access holded and log in
            driver.Navigate().GoToUrl("https://app.holded.com/login?lang=es");
            new WebDriverWait(driver, TimeSpan.FromSeconds(5))
                .Until(x => x.FindElement(By.XPath("//*[@id=\"login-email\"]")))
                .SendKeys(Environment.GetEnvironmentVariable("USERNAME"));
            driver.FindElement(By.XPath("//*[@id=\"login-password\"]")).SendKeys(Environment.GetEnvironmentVariable("PASSWORD")); 
            new WebDriverWait(driver, TimeSpan.FromMilliseconds(100));
            driver.FindElement(By.XPath("//*[@id=\"form-login\"]/div/div[4]/button")).Click();

            //Access check-in tab and click the input
            new WebDriverWait(driver, TimeSpan.FromSeconds(3))
                .Until(x => x.FindElement(By.XPath("//*[@id=\"teamViewWrapper\"]/div[1]/div/div[2]/div[2]")))
                .Click(); 
            Thread.Sleep(2000); //Needed. Holded internal initialization stuff, dont ask me about it
            new WebDriverWait(driver, TimeSpan.FromSeconds(3))
                .Until(x => x.FindElement(By.XPath("//*[@id=\"timetable\"]/div[2]/div[2]/div[18]")))
                .Click(); 
            new WebDriverWait(driver, TimeSpan.FromSeconds(3))
                .Until(x => x.FindElement(By.XPath("//*[@id=\"daySummaryPopup\"]/button[1]")))
                .Click();

            //Checking Weekday and calling input function 
            DateTime d = DateTime.Now;
            bool friday = d.DayOfWeek == DayOfWeek.Friday;
            fillHours(driver, friday);
            new WebDriverWait(driver, TimeSpan.FromSeconds(3))
                .Until(x => x.FindElement(By.XPath("//*[@id=\"updateSummary\"]")))
                .Click(); 

            //Needed sleep. Chrome will close if the work hours input were successfuly set.
            Thread.Sleep(500);
            closeIfCorrect(driver, friday);
        }

        public static void fillHours (IWebDriver driver, Boolean friday){

            driver.FindElement(By.XPath("//*[@id=\"daySummaryPopup\"]/div[4]/div[1]/div/div[2]/div[1]/input"))
                .SendKeys( friday ? "9:00" : "08:30" );
            driver.FindElement(By.XPath("//*[@id=\"daySummaryPopup\"]/div[4]/div[1]/div/div[2]/div[2]/input"))
                .SendKeys(Keys.Backspace + Keys.Backspace + Keys.Backspace + Keys.Backspace + Keys.Backspace +  "14:00"); //Yeah i did that. Sue me.

            if (!friday) {
                new WebDriverWait(driver, TimeSpan.FromSeconds(3))
                    .Until(x => x.FindElement(By.XPath("//*[@id=\"daySummaryPopup\"]/button[1]")))
                    .Click(); 
                driver.FindElement(By.XPath("//*[@id=\"daySummaryPopup\"]/div[4]/div[2]/div/div[2]/div[1]/input"))
                    .SendKeys( "15:15" );
                driver.FindElement(By.XPath("//*[@id=\"daySummaryPopup\"]/div[4]/div[2]/div/div[2]/div[2]/input"))
                    .SendKeys(Keys.Backspace + Keys.Backspace + Keys.Backspace + Keys.Backspace + Keys.Backspace + "18:30");
            }          
        }

        public static void closeIfCorrect (IWebDriver driver, Boolean friday){
            string check = driver.FindElement(By.XPath("//*[@id=\"timetable\"]/div[2]/div[3]/div[1]")).Text;
            Console.WriteLine(check);
            if (friday){
                if (check == "05h 00m"){
                    driver.Quit();
                }
            }
            else{
                if (check == "08h 45m"){
                    driver.Quit();
                }
            }
        }
    }
}   