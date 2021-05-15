using Pulumi;
using Pulumi.AzureNative.SignalRService;
using Pulumi.AzureNative.SignalRService.Inputs;

class MyStack : Stack
{
    public const string _signalRResourceGroupName = "rgdevci";
    public const string _signalRName = "rgdevsignalr";

    public MyStack()
    {
        // Create an Azure Resource Group
        var resourceGroup = new Pulumi.AzureNative.Resources.ResourceGroup(_signalRResourceGroupName);

        var signalR = new SignalR("signalR", new SignalRArgs
        {
            Cors = new SignalRCorsSettingsArgs
            {
                AllowedOrigins =
                {
                    "https://foo.com",
                    "https://bar.com",
                },
            },
            Features =
            {
                new SignalRFeatureArgs
                {
                    Flag = "ServiceMode",
                    //Properties = ,
                    Value = "Serverless",
                },
                new SignalRFeatureArgs
                {
                    Flag = "EnableConnectivityLogs",
                    //Properties = ,
                    Value = "True",
                },
                new SignalRFeatureArgs
                {
                    Flag = "EnableMessagingLogs",
                    //Properties = ,
                    Value = "False",
                },
            },
            Kind = "SignalR",
            NetworkACLs = new SignalRNetworkACLsArgs
            {
                DefaultAction = "Deny",
                PrivateEndpoints =
                {
                    new PrivateEndpointACLArgs
                    {
                        Allow =
                        {
                            "ServerConnection",
                        },
                        Name = $"{_signalRName}.1fa779cd-bf3f-47f0-8c49-afb36723997e",
                    },
                },
                PublicNetwork = new NetworkACLArgs
                {
                    Allow =
                    {
                        "ClientConnection",
                    },
                },
            },
            ResourceGroupName = resourceGroup.Name,
            ResourceName = _signalRName,
            Sku = new ResourceSkuArgs
            {
                Capacity = 1,
                Name = "Standard_S1",
                Tier = "Standard",
            },
            Tags =
            {
                { "key1", "value1" },
            },
            Upstream = new ServerlessUpstreamSettingsArgs
            {
                Templates =
                {
                    new UpstreamTemplateArgs
                    {
                        CategoryPattern = "*",
                        EventPattern = "connect,disconnect",
                        HubPattern = "*",
                        UrlTemplate = "https://example.com/chat/api/connect",
                    },
                },
            },
        });
    }

}