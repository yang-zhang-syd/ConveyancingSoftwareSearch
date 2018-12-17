### ConveyancingSoftwareSearch

This program will use `conveyancing software` as keywords to search on google and retrieves the first 100 results. It will then find out which search result contains `www.smokeball.com.au` and return the indices of these items.

### Highlights
1) Single responsibility
    - `WebRequestSender` to send http requests;
    - `SimpleHtmlParser` to parse html as needed;
    - `GoogleSearchUrlBuilder` to build google search url;
    - `FindSmokeBall` to handle the pattern matching;
2) Program to interfaces: `IHtmlParser` and `IWebRequestSender`
3) Dependencies are injected for ease of unit testing
    ```
    public FindSmokeBall(IWebRequestSender webRequestSender, IHtmlParser htmlParser)
    {
        _webRequestSender = webRequestSender;
        _htmlParser = htmlParser;
    }
    ```
4) Example unit testing `FindSmokeBallTest`
5) Extention method for cleaner logic `StringReaderExtension`
