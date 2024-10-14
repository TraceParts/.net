using CommandLine;
using tracepartsApi_NET8.utils;

namespace tracepartsApi_NET8;

// THERE IS NO API CALL HERE. This function provides a 3D viewer of the 3D model of one given configuration of a catalog
// documentation : https://developers.traceparts.com/v2/reference/3d-viewer-implementation
public abstract class The3dViewerImplementation
{
    private static string GetThe3dViewerImplementation(string elsid, string cultureInfo,
        Dictionary<string, string> possibleOptions)
    {
        Console.WriteLine("There is no API call here. Inputs are just arranged and formated to get the corresponding URL.");
        var defaultValues = GetDefaultValues();

        // Options are formated to be added as GET parameters in the URL
        var possibleOptionsString = "";
        foreach (var possibleOption in possibleOptions)
            // We go through all function options and check if they are usable (not empty)
            // And if they are different from their default values
            if (!string.IsNullOrEmpty(possibleOption.Value) &&
                possibleOption.Value != defaultValues[possibleOption.Key])
            {
                // Encoding the value to avoid special characters
                var formatedValue = MyUrlEncoder.Encode(possibleOption.Value);
                possibleOptionsString += $"&{possibleOption.Key}={formatedValue}";
            }

        var url = $"https://www.traceparts.com/els/{elsid}/{cultureInfo}/api/viewer/3d?{possibleOptionsString}";
        return url;
    }

    public static int Run(Options opts)
    {
        Console.WriteLine(GetThe3dViewerImplementation(opts.elsid, opts.cultureInfo, GetPossibleOptions(opts)));
        return 0;
    }

    private static Dictionary<string, string> GetPossibleOptions(Options opts)
    {
        // This is made to simplify the options management
        var possibleOptions = new Dictionary<string, string>
        {
            { "SupplierID", opts.SupplierID },
            { "PartNumber", opts.PartNumber },
            { "Product", opts.Product },
            { "SelectionPath", opts.SelectionPath },

            { "SetBackgroundColor", opts.SetBackgroundColor },
            { "SetRenderMode", opts.SetRenderMode },
            { "EnableMirrorEffect", opts.EnableMirrorEffect.ToString().ToLower() },
            { "DisplayCoordinateSystem", opts.DisplayCoordinateSystem.ToString().ToLower() },
            { "EnablePresentationMode", opts.EnablePresentationMode.ToString().ToLower() },
            { "DisplayUIMenu", opts.DisplayUIMenu.ToString().ToLower() },
            { "DisplayUIContextMenu", opts.DisplayUIContextMenu.ToString().ToLower() },
            { "MergeUIMenu", opts.MergeUIMenu.ToString().ToLower() },
            { "MenuAlwaysVisible", opts.MenuAlwaysVisible.ToString().ToLower() },
            { "DisplayUIResetButtonMenu", opts.DisplayUIResetButtonMenu.ToString().ToLower() },
            { "DisplayUIScreenshotButtonMenu", opts.DisplayUIScreenshotButtonMenu.ToString().ToLower() },
            { "DisplayUISettingsSubMenu", opts.DisplayUISettingsSubMenu.ToString().ToLower() },
            { "DisplayUIPresentationModeButtonMenu", opts.DisplayUIPresentationModeButtonMenu.ToString().ToLower() },
            { "DisplayUIViewsSubContextMenu", opts.DisplayUIViewsSubContextMenu.ToString().ToLower() },
            { "DisplayUIRenderModesSubContextMenu", opts.DisplayUIRenderModesSubContextMenu.ToString().ToLower() }
        };
        return possibleOptions;
    }

    private static Dictionary<string, string> GetDefaultValues()
    {
        // The default values are gathered from the documentation and will be used to create more concise URLs.
        // For example : if EnableMirrorEffect is affected to 'false' in the function call, it will be ignored as it is its default value.
        var possibleOptions = new Dictionary<string, string>
        {
            { "SupplierID", "" },
            { "PartNumber", "" },
            { "Product", "" },
            { "SelectionPath", "" },

            { "SetBackgroundColor", "0xFFFFFF" },
            { "SetRenderMode", "shaded-edged" },
            { "EnableMirrorEffect", "false" },
            { "DisplayCoordinateSystem", "false" },
            { "EnablePresentationMode", "false" },
            { "DisplayUIMenu", "true" },
            { "DisplayUIContextMenu", "true" },
            { "MergeUIMenu", "false" },
            { "MenuAlwaysVisible", "false" },
            { "DisplayUIResetButtonMenu", "true" },
            { "DisplayUIScreenshotButtonMenu", "true" },
            { "DisplayUISettingsSubMenu", "true" },
            { "DisplayUIPresentationModeButtonMenu", "true" },
            { "DisplayUIViewsSubContextMenu", "true" },
            { "DisplayUIRenderModesSubContextMenu", "true" }
        };
        return possibleOptions;
    }

    [Verb("the-3d-viewer-implementation", HelpText =
        "The 3D viewer implementation. Gives you the URL of a 3D model.\n" +
        "Two ways to get the URL :\n" +
        "Pair 1 : Both parameters (SupplierID and PartNumber) have to be used together. In this case, the couple “Product” and “SelectionPath” is not to use.\n" +
        "Pair 2 : Both parameters (Product and SelectionPath) have to be used together. In this case, the couple “SupplierID” and “PartNumber” is not to use.")]
    public class Options
    {
        // Required parameters
        [Value(0, MetaName = "elsid", Required = true,
            HelpText =
                "Your EasyLink Solutions ID (ELS ID), provided in the email with your Tenant Uid and your API key")]
        public string elsid { get; set; }

        [Value(1, MetaName = "cultureInfo ", Required = true,
            HelpText = "Language of the labels.")]
        public string cultureInfo { get; set; }

        // Required Pairs
        // Pair 1
        [Option("SupplierID", HelpText = "ClassificationCode provided by the \"availability\" endpoints")]
        public string SupplierID { get; set; }

        [Option("PartNumber",
            HelpText =
                "Identifier of a product (to use in combination with SupplierID). Part number as stored in the TraceParts database.")]
        public string PartNumber { get; set; }

        // Pair 2
        [Option("Product", HelpText = "PartFamilyCode provided by the \"availability\" endpoints")]
        public string Product { get; set; }

        [Option("SelectionPath",
            HelpText = "Sequence of parameters which defines a unique configuration for one given partFamilyCode.")]
        public string SelectionPath { get; set; }

        // Optional parameters
        [Option("SetBackgroundColor", Default = "0xFFFFFF",
            HelpText = "Hexadecimal. Sets a color on the background of the 3D viewer.")]
        public string SetBackgroundColor { get; set; }

        [Option("SetRenderMode", Default = "shaded-edged",
            HelpText =
                "Rendering of the 3D model. Values: “shaded-edged”, “shaded”, “transparent”, “wireframe”, “edged”")]
        public string SetRenderMode { get; set; }

        [Option("EnableMirrorEffect", Default = false, HelpText = "Enable the mirror effect on the XZ plane")]
        public bool EnableMirrorEffect { get; set; }

        [Option("DisplayCoordinateSystem", Default = false, HelpText = "Display the coordinate system")]
        public bool DisplayCoordinateSystem { get; set; }

        [Option("EnablePresentationMode",
            Default = false, HelpText = "The model rotates on the Y axis until a user interaction")]
        public bool EnablePresentationMode { get; set; }

        [Option("DisplayUIMenu", Default = true, HelpText = "Display the toolbars (on the bottom and on the right)")]
        public bool DisplayUIMenu { get; set; }

        [Option("DisplayUIContextMenu",
            Default = true, HelpText = "Enable the contextual menu with Views and Render sub menus")]
        public bool DisplayUIContextMenu { get; set; }

        [Option("MergeUIMenu", Default = false, HelpText = "Merge the contextual menu inside the main menu")]
        public bool MergeUIMenu { get; set; }

        [Option("MenuAlwaysVisible", Default = false, HelpText = "Always display the toolbar")]
        public bool MenuAlwaysVisible { get; set; }

        [Option("DisplayUIResetButtonMenu", Default = true, HelpText = "Display the Reset button")]
        public bool DisplayUIResetButtonMenu { get; set; }

        [Option("DisplayUIScreenshotButtonMenu", Default = true, HelpText = "Display the Screenshot button")]
        public bool DisplayUIScreenshotButtonMenu { get; set; }

        [Option("DisplayUISettingsSubMenu", Default = true, HelpText = "Display the Settings menu")]
        public bool DisplayUISettingsSubMenu { get; set; }

        [Option("DisplayUIPresentationModeButtonMenu", Default = true, HelpText = "Display the Presentation button")]
        public bool DisplayUIPresentationModeButtonMenu { get; set; }

        [Option("DisplayUIViewsSubContextMenu",
            Default = true, HelpText = "Display the Views sub menu (for the contextual menu)")]
        public bool DisplayUIViewsSubContextMenu { get; set; }

        [Option("DisplayUIRenderModesSubContextMenu",
            Default = true, HelpText = "Display the Render sub menu (for the contextual menu)")]
        public bool DisplayUIRenderModesSubContextMenu { get; set; }
    }
}