using OoxmlDemo.ExternalServices;
using OoxmlDemo.ExternalServices.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;

namespace OoxmlDemo.DocumentProcessing
{
    public class MyUpdater : DocumentContainer
    {
        public MyUpdater(string path) : base(path) { }

        protected override async Task UpdateDocumentAsync()
        {
            Dictionary<string, object> labelStorage = new Dictionary<string, object>();

            TokenDescription token;
            while ((token = FindToken(ContentPart.Content)).TokenName != null)
            {
                if ("foreach".Equals(token.TokenName.FirstOrDefault() ??string.Empty, StringComparison.OrdinalIgnoreCase))
                {
                    ContentPart.Content = await ProcessForeachTokenAsync(token, ContentPart.Content, labelStorage);
                }
                else
                {
                    ContentPart.Content = await ProcessTokenAsync(token, ContentPart.Content, labelStorage);
                }
            }
        }


        protected async Task<string> ProcessTokenAsync(TokenDescription token, string content, Dictionary<string, object> labelStorage)
        {
            object newValue = null;
            string label = null;
            foreach (var prp in token.Properties.ToArray())
            {
                if ("label".Equals(prp.Key, StringComparison.Ordinal))
                {
                    token.Properties.Remove(prp.Key);
                    label = prp.Value;
                }
            }

            var firstTokenPart = (token.TokenName.FirstOrDefault() ?? string.Empty).ToLower();

            object propertyValue;
            if (labelStorage.TryGetValue(firstTokenPart, out propertyValue))
            {
                newValue = PropertyNavigator.GetPropertyValue(propertyValue, token.TokenName.Skip(1));
            }
            else
            {

                switch (firstTokenPart)
                {
                    case "swap":
                        await SwapImage(token.TokenName.Skip(1).ToArray(), token.Properties);
                        break;
                    default:
                        newValue = await ServiceContext.GetInfo(token.TokenName, token.Properties);
                        break;
                }
            }
            if (label != null)
            {
                labelStorage[label.ToLower()] = newValue;
                newValue = null;
            }
            return ReplaceToken( content, token, newValue ?? string.Empty);
        }
        protected async Task<string> ProcessForeachTokenAsync(TokenDescription token, string content, Dictionary<string, object> labelStorage)
        {
            var newValue = string.Empty;
            var endToken = FindToken(content, token.EndPosition, "end");
            if (endToken.StartPosition != 0)
            {
                var itemTemplate = content.Substring(token.EndPosition, endToken.StartPosition - token.EndPosition);
                var sourceName = (token.Properties.GetValue("src") ?? "NoSource").Split('.');
                var source = await ServiceContext.GetInfo(sourceName, token.Properties) as IEnumerable;
                if (source != null)
                {
                    List<string> itemOutput = new List<string>();
                    var take = token.Properties.GetValue<int>("take");
                    foreach (var item in source)
                    {
                        TokenDescription itemToken;
                        labelStorage["item"] = item;
                        var itemContent = itemTemplate;
                        while ((itemToken = FindToken(itemContent)).TokenName != null)
                        {
                            itemContent = await ProcessTokenAsync(itemToken, itemContent, labelStorage);
                        }
                        itemOutput.Add(itemContent);

                        if (--take == 0)
                        {
                            break;
                        }
                    }
                    labelStorage.Remove( "item");
                    newValue = string.Concat(itemOutput);
                }

                token.EndPosition = endToken.EndPosition;
            }
            return ReplaceToken(content, token, newValue ?? string.Empty);
        }


    }
}