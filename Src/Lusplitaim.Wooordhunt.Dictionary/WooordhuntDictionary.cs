using Microsoft.Playwright;
using System;
using System.Threading.Tasks;

namespace Lusplitaim.Wooordhunt
{
    public class WooordhuntDictionary : IWooordhuntDictionary
    {
        private IPlaywright _playwright;
        private IBrowser _browser;

        private const string url = "https://wooordhunt.ru/word/";
        private const string contentSwitcherEnId = "#content_switcher_en";
        private const string contentInEnglishId = "#content_in_english";
        private const string entryContentId = "#wd";

        private const string wordNotFoundUrlPath = "wordnotfound";

        public static Task<IWooordhuntDictionary> CreateAsync()
        {
            var wooordhuntApi = new WooordhuntDictionary();
            return wooordhuntApi.InitializeAsync();
        }

        private async Task<IWooordhuntDictionary> InitializeAsync()
        {
            _playwright = await Playwright.CreateAsync();
            _browser = await _playwright.Chromium.LaunchAsync();

            return this;
        }

        public async Task<IEngRusEntry> GetEngRusEntry(string englishWord)
        {
            var page = await GetWooordhuntPage(englishWord);

            var russianContent = page.Locator(entryContentId);

            return new EngRusEntry(await russianContent.InnerHTMLAsync());
        }

        public async Task<IEngEntry> GetEngEntry(string englishWord)
        {
            var page = await GetWooordhuntPage(englishWord);

            var englishEntryHtmlContent = await GetEngEntryHtmlContent(page);

            return new EngEntry(englishEntryHtmlContent);
        }

        private async Task<string> GetEngEntryHtmlContent(IPage page)
        {
            await page.Locator(contentSwitcherEnId).ClickAsync();

            var entryContent = page.Locator(entryContentId);

            var englishContentContainer = entryContent.Locator(contentInEnglishId);

            await englishContentContainer.WaitForAsync(new LocatorWaitForOptions 
            { 
                State = WaitForSelectorState.Visible 
            });

            return await entryContent.InnerHTMLAsync();
        }

        public async Task<IRusEngEntry> GetRusEngEntry(string russianWord)
        {
            var page = await GetWooordhuntPage(russianWord);

            var rusEngContent = page.Locator(entryContentId);

            return new RusEngEntry(await rusEngContent.InnerHTMLAsync());
        }

        private async Task<IPage> GetWooordhuntPage(string word)
        {
            var page = await _browser.NewPageAsync();
            var response = await page.GotoAsync($"{url}{word}");

            if (response.Url.Contains(wordNotFoundUrlPath))
            {
                ThrowWordNotFoundException(word);
            }

            return page;
        }

        private void ThrowWordNotFoundException(string word)
        {
            throw new Exception($"Word or phrase \"{word}\" not found");
        }

        public async ValueTask DisposeAsync()
        {
            await _browser.DisposeAsync();
            _playwright.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}