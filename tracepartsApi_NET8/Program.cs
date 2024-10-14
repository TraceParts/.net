using CommandLine;
using tracepartsApi_NET8.account;
using tracepartsApi_NET8.authentication;
using tracepartsApi_NET8.cadFileDelivery;
using tracepartsApi_NET8.common;
using tracepartsApi_NET8.dataIndexing;
using tracepartsApi_NET8.product;
using tracepartsApi_NET8.search.checkAvailabilityWith;

namespace tracepartsApi_NET8;

// documentation : https://developers.traceparts.com/v2/reference
internal abstract class Program
{
    private static void Main(string[] args)
    {
        // Define all option types in an array
        Type[] optionTypes =
        [
            // Authentication
            typeof(GenerateToken.Options),
            // Common
            typeof(GetLanguagesList.Options),
            // Search
            typeof(Catalog.Options),
            typeof(PartNumber.Options),
            typeof(YourOwnCode.Options),
            // Data Indexing
            typeof(ListOfCatalogs.Options),
            typeof(ListOfCategories.Options),
            typeof(ListOfProductsAndCategories.Options),
            typeof(ListOfProducts.Options),
            typeof(ListOfPartNumbers.Options),
            typeof(CatalogContactDetails.Options),
            // Account
            typeof(CheckTheExistenceOfAUserAccount.Options),
            typeof(CreateAUserAccount.Options),
            // Product
            typeof(ProductData.Options),
            typeof(ProductConfiguration.Options),
            // CAD file delivery
            typeof(GetCadFormatsList.Options),
            typeof(RequestACadFile.Options),
            typeof(GetCadFileUrl.Options),
            // 3D Viewer
            typeof(The3dViewerImplementation.Options)
        ];

        var result = Parser.Default.ParseArguments(args, optionTypes);

        result.WithParsed<object>(opts =>
        {
            switch (opts)
            {
                // Authentication
                case GenerateToken.Options genTokenOpts:
                    GenerateToken.Run(genTokenOpts);
                    break;
                // Common
                case GetLanguagesList.Options langListOpts:
                    GetLanguagesList.Run(langListOpts);
                    break;
                // Search
                case Catalog.Options catalogOpts:
                    Catalog.Run(catalogOpts);
                    break;
                case PartNumber.Options partNumberOpts:
                    PartNumber.Run(partNumberOpts);
                    break;
                case YourOwnCode.Options yourOwnCodeOpts:
                    YourOwnCode.Run(yourOwnCodeOpts);
                    break;
                // Data Indexing
                case ListOfCatalogs.Options listOfCatalogsOpts:
                    ListOfCatalogs.Run(listOfCatalogsOpts);
                    break;
                case ListOfCategories.Options listOfCategoriesOpts:
                    ListOfCategories.Run(listOfCategoriesOpts);
                    break;
                case ListOfProductsAndCategories.Options listOfProductsAndCategoriesOpts:
                    ListOfProductsAndCategories.Run(listOfProductsAndCategoriesOpts);
                    break;
                case ListOfProducts.Options listOfProductsOpts:
                    ListOfProducts.Run(listOfProductsOpts);
                    break;
                case ListOfPartNumbers.Options listOfPartNumbersOpts:
                    ListOfPartNumbers.Run(listOfPartNumbersOpts);
                    break;
                case CatalogContactDetails.Options catalogContactDetailsOpts:
                    CatalogContactDetails.Run(catalogContactDetailsOpts);
                    break;
                // Account
                case CheckTheExistenceOfAUserAccount.Options checkAccountOpts:
                    CheckTheExistenceOfAUserAccount.Run(checkAccountOpts);
                    break;
                case CreateAUserAccount.Options createAccountOpts:
                    CreateAUserAccount.Run(createAccountOpts);
                    break;
                // Product
                case ProductData.Options productDataOpts:
                    ProductData.Run(productDataOpts);
                    break;
                case ProductConfiguration.Options productConfigOpts:
                    ProductConfiguration.Run(productConfigOpts);
                    break;
                // CAD file delivery
                case GetCadFormatsList.Options getCadFormatsListOpts:
                    GetCadFormatsList.Run(getCadFormatsListOpts);
                    break;
                case RequestACadFile.Options requestCadFileOpts:
                    RequestACadFile.Run(requestCadFileOpts);
                    break;
                case GetCadFileUrl.Options getUrlOpts:
                    GetCadFileUrl.Run(getUrlOpts);
                    break;
                // 3D Viewer
                case The3dViewerImplementation.Options viewerOpts:
                    The3dViewerImplementation.Run(viewerOpts);
                    break;
            }
        }).WithNotParsed(errs =>
        {
            // Handle errors
            Console.WriteLine("Errors occurred while parsing arguments.");
        });
    }
}