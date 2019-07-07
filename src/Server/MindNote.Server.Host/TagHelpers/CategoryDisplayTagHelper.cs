using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using MindNote.Client.SDK.API;
using MindNote.Data;

namespace MindNote.Server.Host.TagHelpers
{
    [HtmlTargetElement("category-display")]
    public class CategoryDisplayTagHelper : TagHelper
    {
        public Category Value { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (Value != null)
            {
                output.TagName = "span";
                output.TagMode = TagMode.StartTagAndEndTag;

                TagBuilder icon = new TagBuilder("i");
                icon.AddCssClass("far");
                icon.AddCssClass("fa-folder");
                // "display:inline-block; border-radius:50%; height:12px; width:12px; position:relative;"
                icon.Attributes["style"] = (Value.Color != null ? $"color:{Value.Color}" : "");
                output.Content.AppendHtml(icon);

                TagBuilder name = new TagBuilder("span");
                name.Attributes["style"] = "margin-left: 5px";
                name.InnerHtml.Append(Value.Name);
                output.Content.AppendHtml(name);
            }
            base.Process(context, output);
        }
    }
}
