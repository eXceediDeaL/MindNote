using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;

namespace MindNote.Frontend.Server.TagHelpers
{
    [HtmlTargetElement("keywords-display")]
    public class KeywordsDisplayTagHelper : TagHelper
    {
        public ICollection<string> Keywords { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (Keywords != null && Keywords.Count != 0)
            {
                output.TagName = "span";
                output.TagMode = TagMode.StartTagAndEndTag;

                TagBuilder icon = new TagBuilder("i");
                icon.AddCssClass("fa");
                if (Keywords.Count == 1)
                    icon.AddCssClass("fa-tag");
                else
                    icon.AddCssClass("fa-tags");
                output.Content.AppendHtml(icon);

                TagBuilder name = new TagBuilder("span");
                name.Attributes["style"] = "margin-left: 5px";
                name.InnerHtml.Append(string.Join(" ; ", Keywords));
                output.Content.AppendHtml(name);
            }
            base.Process(context, output);
        }
    }
}
